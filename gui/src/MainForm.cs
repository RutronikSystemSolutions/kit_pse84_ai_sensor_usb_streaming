using System;
using System.Drawing.Imaging;
using System.IO.Ports;
using kit_pse84_ai_streaming;

namespace ov7675
{
    public partial class MainForm : Form
    {
        private OV7675CDCReader cdcreader;
        private DataLogger dataLogger;

        private const int width = 320;
        private const int height = 240;

        private const int bytes_per_pix = 2;

        // TODO dynamic
        private const int samplesPerChirp = 128;

        public MainForm()
        {
            InitializeComponent();

            cdcreader = new OV7675CDCReader();
            cdcreader.OnNewConnectionState += Cdcreader_OnNewConnectionState;
            cdcreader.OnNewOV7675 += Cdcreader_OnNewOV7675;
            cdcreader.OnNewRadarPacket += Cdcreader_OnNewRadarPacket;

            dataLogger = new DataLogger();
            dataLogger.OnNewLoggerState += DataLogger_OnNewLoggerState;
            dataLogger.OnNewBytesWritten += DataLogger_OnNewBytesWritten;
        }

        private void Cdcreader_OnNewRadarPacket(object sender, byte[] data)
        {
            dataLogger.LogRadar(data);

            // Get the first chirp and display it
            double[] samples = new double[samplesPerChirp];

            double sum = 0;

            for (int i = 0; i < samplesPerChirp; ++i)
            {
                samples[i] = BitConverter.ToUInt16(data, i * 2);
                samples[i] = samples[i] / 4095;
                sum += samples[i];
            }

            double avg = sum / samplesPerChirp;

            for (int i = 0; i < samplesPerChirp; ++i)
            {
                samples[i] = samples[i] - avg;
            }

            rawRadarSignalsView.updateData(samples);
        }

        private void Cdcreader_OnNewOV7675(object sender, byte[] data)
        {
            int data_count = width * height * bytes_per_pix;
            //if (data_count != (data.Length - OV7675CDCReader.COM_OVERHEAD))
            if (data_count != data.Length)
            {
                System.Diagnostics.Debug.WriteLine("Problem...");
                return;
            }

            dataLogger.LogOV7675(data);

            // Cast buffer to uint16
            UInt16[] u16buffer = new UInt16[data_count / 2];
            for (int i = 0; i < (data_count / 2); ++i)
            {
                u16buffer[i] = BitConverter.ToUInt16(data, i * 2);
            }

            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    UInt16 p = u16buffer[y * width + x];

                    // convert RGB565 to RGB 24-bit
                    int r = ((p >> 11) & 0x1f) << 3;
                    int g = ((p >> 5) & 0x3f) << 2;
                    int b = ((p >> 0) & 0x1f) << 3;

                    Color color = Color.FromArgb(r, g, b);
                    bmp.SetPixel(x, y, color);
                }
            }

            ov7675PictureBox.Image = bmp;
        }

        private void Cdcreader_OnNewConnectionState(object sender, OV7675CDCReader.ConnectionState state)
        {
            // System.Diagnostics.Debug.WriteLine("CDC New connection state: " + state.ToString());
            switch (state)
            {
                case OV7675CDCReader.ConnectionState.Connected:
                    connectButton.Enabled = false;
                    comPortComboBox.Enabled = false;
                    disconnectButton.Enabled = true;
                    statusTextBox.Text = "Connected";
                    statusTextBox.BackColor = Color.Green;
                    break;

                case OV7675CDCReader.ConnectionState.Error:
                    connectButton.Enabled = true;
                    comPortComboBox.Enabled = true;
                    disconnectButton.Enabled = false;
                    statusTextBox.Text = "Error";
                    statusTextBox.BackColor = Color.Red;
                    break;

                case OV7675CDCReader.ConnectionState.Iddle:
                    connectButton.Enabled = true;
                    comPortComboBox.Enabled = true;
                    disconnectButton.Enabled = false;
                    statusTextBox.Text = "Iddle";
                    statusTextBox.BackColor = Color.Gray;
                    break;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Load the possible com ports
            string[] serialPorts = SerialPort.GetPortNames();
            comPortComboBox.DataSource = serialPorts;
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            if ((comPortComboBox.SelectedIndex < 0) || (comPortComboBox.SelectedIndex >= comPortComboBox.Items.Count)) return;

            var selectedItem = comPortComboBox.Items[comPortComboBox.SelectedIndex];
            if (selectedItem != null)
            {
                string? portName = selectedItem.ToString();
                if (portName != null)
                {
                    cdcreader.SetPortName(portName);
                    connectButton.Enabled = false;
                    comPortComboBox.Enabled = false;
                }
            }
        }

        private void disconnectButton_Click(object sender, EventArgs e)
        {
            cdcreader.Disconnect();
            disconnectButton.Enabled = false;
        }

        private void dataLoggerOpenPathButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Binary Log file|*.log";
            sfd.Title = "Save stream to file";
            if (sfd.ShowDialog() != DialogResult.OK) return;

            dataLoggerPathTextBox.Text = sfd.FileName;
            dataLogger.SetPath(sfd.FileName);
        }

        private void dataLoggerStartButton_Click(object sender, EventArgs e)
        {
            dataLogger.Start();
            dataLoggerStartButton.Enabled = false;
        }

        private void dataLoggerStopButton_Click(object sender, EventArgs e)
        {
            dataLogger.Stop();
            dataLoggerStopButton.Enabled = false;
        }

        private void DataLogger_OnNewBytesWritten(object sender, int bytesWritten)
        {
            string unit = "B";

            float writtenSize = (float)bytesWritten;

            if (writtenSize > (1024 * 1024))
            {
                unit = "MB";
                writtenSize = (writtenSize / (1024 * 1024));
            }
            else if (writtenSize > (1024))
            {
                unit = "kB";
                writtenSize = (writtenSize / (1024));
            }

            storedSizeTextBox.Text = string.Format("{0:F1} {1}", writtenSize, unit);
        }

        private void DataLogger_OnNewLoggerState(object sender, DataLogger.LoggerState state)
        {
            switch (state)
            {
                case DataLogger.LoggerState.Iddle:
                    dataLoggerStartButton.Enabled = false;
                    dataLoggerStopButton.Enabled = false;
                    break;

                case DataLogger.LoggerState.Ready:
                    dataLoggerStartButton.Enabled = true;
                    dataLoggerStopButton.Enabled = false;
                    break;

                case DataLogger.LoggerState.Logging:
                    dataLoggerStartButton.Enabled = false;
                    dataLoggerStopButton.Enabled = true;
                    break;
            }
        }

        
    }
}
