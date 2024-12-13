using NeuroSDK;
using System;
using System.IO;
using System.Windows.Forms;

namespace ah_ha_moment
{
    public partial class Monitor : Form
    {
        private SensorInfo selectedDevice;
        private BrainBitSensor sensor;
        private Scanner scanner;
        private StreamWriter writer;
        private String fileName;
        private String filePath = @"C:\Users\kn183\OneDrive\Documents\EEG"; //available for Windows
        GreetingScreen screen;
        public Monitor()
        {
            using (var fileNameForm = new FileNameForm())
            {
                if (fileNameForm.ShowDialog() == DialogResult.OK)
                {
                    InitializeComponent();
                    fileName = fileNameForm.FileName;
                    scanner = new Scanner(SensorFamily.SensorLEBrainBit);
                    scanner.EventSensorsChanged += OnDeviceFound;
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    writer = new StreamWriter($"{filePath}{fileName}_{getTimestamp()}.csv");
                    writer.WriteLine("Timestamp,O1,O2,T3,T4,Order,Question,IsCorrect");

                    // Fullscreen and responsive settings
                    this.WindowState = FormWindowState.Maximized;
                    this.AutoScaleMode = AutoScaleMode.Dpi;
                    this.MinimumSize = new Size(800, 600); // Set a minimum size
                }
                else
                {
                    MessageBox.Show("Canceled, you're leaving", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ExitApplication();
                }
            }
        }
        private string getTimestamp()
        {
            return DateTime.Now.ToString("yyyy.MM.dd.HH.mm");
        }
        private string getTimestampDetail()
        {
            return DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.fff");
        }
        private void ExitApplication()
        {
            Environment.Exit(0);
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            UpdateStatus("Scanning for devices...");

            lstDevices.Items.Clear();
            btnStartScan.Enabled = false;
            btnStopScan.Enabled = true;
            scanner.Start();
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            UpdateStatus("Scan stopped.");

            btnStartScan.Enabled = true;
            btnStopScan.Enabled = false;
            scanner.Stop();
        }

        private void lstDevices_DoubleClick(object sender, EventArgs e)
        {
            if (lstDevices.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a device to connect.");
                return;
            }

            selectedDevice = scanner.Sensors[lstDevices.SelectedIndex];
            UpdateStatus($"Connecting to {selectedDevice.Name}...");


            try
            {
                sensor = (BrainBitSensor)scanner.CreateSensor(selectedDevice);

                if (sensor == null)
                {
                    UpdateStatus("Failed to create sensor instance.");

                    return;
                }
                UpdateStatus($"Connected to {selectedDevice.Name}.");
                DisplayDeviceInfo(sensor);

                // Add sensor events
                sensor.EventBatteryChanged += Sensor_EventBatteryChanged;
                sensor.EventSensorStateChanged += Sensor_EventSensorStateChanged;
                sensor.EventBrainBitSignalDataRecived += Sensor_EventBrainBitSignalDataRecived;
            }
            catch (NeuroSDK.SDKException ex)
            {
                UpdateStatus($"Error creating BLE device: {ex.Message}");

            }
        }
        private async void btnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                screen = new GreetingScreen();
                screen.Show();
                startCollectingSignal();
            }
            catch (Exception ex)
            {

            }

        }
        private void btnStopRecord_Click(object sender, EventArgs e)
        {
            try
            {
                stopCollectingSignal();
                screen.Dispose();
            }
            catch (Exception ex)
            {

            }
        }

        private void OnDeviceFound(IScanner sender, IReadOnlyList<SensorInfo> devices)
        {
            Invoke((MethodInvoker)(() =>
            {
                lstDevices.Items.Clear();
                foreach (var device in devices)
                {
                    lstDevices.Items.Add($"{device.Name} ({device.Address})");
                }
            }));
        }

        private void Sensor_EventSensorStateChanged(ISensor sensor, SensorState state)
        {
            UpdateStatus($"{sensor.Name} state: {state}");
        }

        private void Sensor_EventBatteryChanged(ISensor sensor, int batteryLevel)
        {
            UpdateBattery($"Battery Level: {batteryLevel}%");
        }

        void Sensor_EventBrainBitSignalDataRecived(ISensor sensor, BrainBitSignalData[] data)
        {
            foreach (BrainBitSignalData signal in data)
            {
                string temp = $"{getTimestampDetail()},{signal.O1 * 1e3},{signal.O2 * 1e3},{signal.T3 * 1e3},{signal.T4 * 1e3}," +
                    $"{GameSessionData.QuestionOrder},{GameSessionData.Question},{GameSessionData.IsCorrect}";

                UpdateSignal(temp);

                writer.WriteLine(temp);
            }
        }
        private void startCollectingSignal()
        {

            sensor.EventBrainBitSignalDataRecived += Sensor_EventBrainBitSignalDataRecived;
            sensor.ExecCommand(SensorCommand.CommandStartSignal);

        }
        private void stopCollectingSignal()
        {
            sensor.ExecCommand(SensorCommand.CommandStopSignal);
            sensor.EventBrainBitSignalDataRecived -= Sensor_EventBrainBitSignalDataRecived;

        }



        private void DisplayDeviceInfo(BrainBitSensor sensor)
        {
            txtDeviceInfo.Text = $"Name: {sensor.Name}\r\n" +
                                 $"Address: {sensor.Address}\r\n" +
                                 $"Serial Number: {sensor.SerialNumber}\r\n" +
                                 $"Firmware Version: {sensor.Version.FwMajor}.{sensor.Version.FwMinor}\r\n";
        }

        private void UpdateStatus(string message)
        {
            Invoke((MethodInvoker)(() => lblStatus.Text = $"Status: {message}"));
        }

        private void UpdateBattery(string message)
        {
            Invoke((MethodInvoker)(() => txtBattery.Text = message));
        }

        private void UpdateSignal(string message)
        {
            Invoke((MethodInvoker)(() => txtSignal.AppendText(message + Environment.NewLine)));
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //writer.Flush();
            writer.Close();

            scanner?.Stop();
            scanner?.Dispose();
            sensor?.Disconnect();
            sensor?.Dispose();
        }
        private void txtSignal_TextChanged(object sender, EventArgs e)
        {

        }

    }
    public static class AppManager
    {
        public static bool isRecord { get; set; }
    }

}