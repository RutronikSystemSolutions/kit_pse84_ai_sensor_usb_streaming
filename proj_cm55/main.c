/******************************************************************************
* File Name:   main.c
*
* Description: This is the source code for the OV7675 camera and radar streaming
*              over USB application for ModusToolbox.
*
* Related Document: See README.md
*
*
*  Created on: 2025-12-08
*  Company: Rutronik Elektronische Bauelemente GmbH
*  Address: Industriestraße 2, 75228 Ispringen, Germany
*  Author: RJ030
*
*******************************************************************************
* Copyright 2023-2025, Cypress Semiconductor Corporation (an Infineon company) or
* an affiliate of Cypress Semiconductor Corporation.  All rights reserved.
*
* This software, including source code, documentation and related
* materials ("Software") is owned by Cypress Semiconductor Corporation
* or one of its affiliates ("Cypress") and is protected by and subject to
* worldwide patent protection (United States and foreign),
* United States copyright laws and international treaty provisions.
* Therefore, you may use this Software only as provided in the license
* agreement accompanying the software package from which you
* obtained this Software ("EULA").
* If no EULA applies, Cypress hereby grants you a personal, non-exclusive,
* non-transferable license to copy, modify, and compile the Software
* source code solely for use in connection with Cypress's
* integrated circuit products.  Any reproduction, modification, translation,
* compilation, or representation of this Software except as specified
* above is prohibited without the express written permission of Cypress.
*
* Disclaimer: THIS SOFTWARE IS PROVIDED AS-IS, WITH NO WARRANTY OF ANY KIND,
* EXPRESS OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, NONINFRINGEMENT, IMPLIED
* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE. Cypress
* reserves the right to make changes to the Software without notice. Cypress
* does not assume any liability arising out of the application or use of the
* Software or any product or circuit described in the Software. Cypress does
* not authorize its products for use in any products where a malfunction or
* failure of the Cypress product may reasonably be expected to result in
* significant property damage, injury or death ("High Risk Product"). By
* including Cypress's product in a High Risk Product, the manufacturer
* of such system or application assumes all risk of such use and in doing
* so agrees to indemnify Cypress against all liability.
*
* Rutronik Elektronische Bauelemente GmbH Disclaimer: The evaluation board
* including the software is for testing purposes only and,
* because it has limited functions and limited resilience, is not suitable
* for permanent use under real conditions. If the evaluation board is
* nevertheless used under real conditions, this is done at one’s responsibility;
* any liability of Rutronik is insofar excluded
*******************************************************************************/

#include "cybsp.h"
#include "cy_pdl.h"

#include <stdlib.h>
#include <string.h>

// Access to retarget-io initialization
// used to have printf over KitProg3
#include "driver/retarget_io/retarget_io_init.h"

// Driver of the OV7675 camera (over DVP for stream and I2C for configuration)
#include "driver/ov7675/mtb_dvp_camera_ov7675.h"

// Driver for USBD support (using emUSB from Seeger)
#include "driver/usbd/usbd.h"

// Driver for radar (BGT60TR13C)
#include "driver/radar/radar.h"

#include "crc.h"

/**
 * @def COM_OVERHEAD
 * Size of the header packet (USB communication)
 * Used for synchronization and validation
 */
#define COM_OVERHEAD	8

/**
 * @def COM_CMD_SIZE
 * Size of a command from computer to PSoC Edge
 */
#define COM_CMD_SIZE	1

/**
 * @def COM_CMD_START_STREAM
 * Command enabling to start the streaming of data
 */
#define COM_CMD_START_STREAM	49

int main(void)
{
	// Used to store video stream
	// Double buffering used
	uint8_t* image_buffer_0 = NULL;
	uint8_t* image_buffer_1 = NULL;

	uint8_t* comm_buffer = NULL;
	uint16_t* radar_data = NULL;
	uint16_t radar_num_samples = 0;
	size_t radar_data_size = 0;

	cy_stc_scb_i2c_context_t i2c_master_context;
	usbd_t* usb_handle;

	bool frame_ready = false;
	bool active_frame = false;

	int send_data = 0;

	uint8_t counterint = 0;

    cy_rslt_t result;

    // Initialize the device and board peripherals
    result = cybsp_init();
    if (CY_RSLT_SUCCESS != result)
    {
        CY_ASSERT(0);
    }

    // Init retarget-io -> printf redirected to KitProg3
	init_retarget_io();

    // Enable global interrupts
    __enable_irq();

	printf("PSOC EDGE OV7675 Streaming over USB v1.0\r\n");

    // Enable I2C Controller -> used to configure the OV7675
    result = Cy_SCB_I2C_Init(CYBSP_I2C_CAM_CONTROLLER_HW, &CYBSP_I2C_CAM_CONTROLLER_config,
                             &i2c_master_context);
    if (CY_SCB_I2C_SUCCESS != result)
    {
        CY_ASSERT(0);
    }
    Cy_SCB_I2C_Enable(CYBSP_I2C_CAM_CONTROLLER_HW);

    // Memory allocation
    image_buffer_0 = malloc(OV7675_MEMORY_BUFFER_SIZE);
	if (image_buffer_0 == NULL)
	{
		printf("Cannot allocate image_buffer_0 \r\n");
		return 0;
	}
	memset(image_buffer_0, 0, OV7675_MEMORY_BUFFER_SIZE);

	image_buffer_1 = malloc(OV7675_MEMORY_BUFFER_SIZE);
	if (image_buffer_1 == NULL)
	{
		printf("Cannot allocate image_buffer_1 \r\n");
		return 0;
	}

	// Allocate communication buffer
	// We assume that the size of a picture captured by the OV7675
	// is bigger than the size of a radar frame
	comm_buffer = malloc(OV7675_MEMORY_BUFFER_SIZE + COM_OVERHEAD);
	if (comm_buffer == NULL)
	{
		printf("Cannot allocate comm_buffer \r\n");
		return 0;
	}

	// Allocate for radar sensor
	radar_num_samples = radar_get_num_samples_per_frame();
	radar_data_size = radar_num_samples * sizeof(uint16_t);
	radar_data = malloc(radar_data_size);
	if (radar_data == NULL)
	{
		printf("Cannot allocate radar_data \r\n");
		return 0;
	}

	// Init USB CDC
	// This call will block until USB cable is plugged to a computer
	usb_handle = usbd_create();

    // Initialize the camera DVP OV7675
    result = mtb_dvp_cam_ov7675_init(image_buffer_0, image_buffer_1,
    		&i2c_master_context,
			&frame_ready, &active_frame);
    if (CY_RSLT_SUCCESS != result)
    {
		printf("Cannot initialize OV7675 \r\n");
		return 0;
    }

    // Initialize radar sensor
	if (radar_init() != 0)
	{
		printf("Cannot initialize radar sensor ...\r\n");
		return 0;
	}

	printf("Sensors have been initialized - Start streaming \r\n");

    for (;;)
    {
    	uint8_t cmd = 0;

    	// Something in USB read buffer?
    	if ( usbd_read(usb_handle, &cmd, COM_CMD_SIZE) == COM_CMD_SIZE)
    	{
    		printf("Received command: %d \r\n", cmd);
    		if (cmd == COM_CMD_START_STREAM)
    		{
    			send_data = 1;
    			counterint = 0;
    		}
    		else send_data = 0;
    	}

    	// Frame ready from the OV7675?
		if (frame_ready)
		{
			frame_ready = false;

			// Copy to the communication buffer
			if (active_frame == 0)
				memcpy(&comm_buffer[COM_OVERHEAD], image_buffer_0, OV7675_MEMORY_BUFFER_SIZE);
			else
				memcpy(&comm_buffer[COM_OVERHEAD], image_buffer_1, OV7675_MEMORY_BUFFER_SIZE);

			// Add overhead
			comm_buffer[0] = 0x55;
			comm_buffer[1] = 0x55;
			comm_buffer[2] = counterint;
			counterint++;
			*((uint32_t*)&comm_buffer[3]) = OV7675_MEMORY_BUFFER_SIZE;
			comm_buffer[7] = crc_compute(&comm_buffer[COM_OVERHEAD], OV7675_MEMORY_BUFFER_SIZE);

			// Send per USB
			if (send_data == 1)
			{
				Cy_GPIO_Write(CYBSP_LED_RGB_GREEN_PORT, CYBSP_LED_RGB_GREEN_PIN, 1);
				if (usbd_write(usb_handle, comm_buffer, OV7675_MEMORY_BUFFER_SIZE + COM_OVERHEAD) != 0)
				{
					printf("Failed to write OV7675 values over USB\r\n");
					send_data = 0;
				}
				Cy_GPIO_Write(CYBSP_LED_RGB_GREEN_PORT, CYBSP_LED_RGB_GREEN_PIN, 0);
			}
		}

		// Radar data available?
		if (radar_is_data_available())
		{
			if (radar_read_data(radar_data, radar_num_samples) != 0)
			{
				printf("Error reading radar data\r\n");
			}
			else
			{
				// Alive LED
				Cy_GPIO_Inv(CYBSP_USER_LED1_PORT, CYBSP_USER_LED1_PIN);

				// Copy data
				memcpy(&comm_buffer[COM_OVERHEAD], (uint8_t*)radar_data, radar_data_size);

				// Add overhead
				comm_buffer[0] = 0x55;
				comm_buffer[1] = 0x55;
				comm_buffer[2] = counterint;
				counterint++;
				*((uint32_t*)&comm_buffer[3]) = (uint32_t)radar_data_size;
				comm_buffer[7] = crc_compute(&comm_buffer[COM_OVERHEAD], radar_data_size);

				// Send once per USB
				if (send_data == 1)
				{
					Cy_GPIO_Write(CYBSP_USER_LED2_PORT, CYBSP_USER_LED2_PIN, 1);
					if (usbd_write(usb_handle, comm_buffer, radar_data_size + COM_OVERHEAD) != 0)
					{
						printf("Failed to write radar data over USB\r\n");
						send_data = 0;
					}
					Cy_GPIO_Write(CYBSP_USER_LED2_PORT, CYBSP_USER_LED2_PIN, 0);
				}
			}
		}
    }
}
