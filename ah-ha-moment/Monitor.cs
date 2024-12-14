using ScottPlot;
using NeuroSDK;
using System.Text;

namespace ah_ha_moment
{
    public partial class Monitor : Form
    {
        GreetingScreen screen;
        public Monitor()
        {
            using (var fileNameForm = new FileNameForm())
            {
                if (fileNameForm.ShowDialog() == DialogResult.OK)
                {
                    InitializeComponent();
                    string fileName = fileNameForm.FileName;
                    // Set up UI update actions
                    btnRecord.Enabled = false;
                    AppManager.UpdateStatusAction = message =>
                        Invoke((MethodInvoker)(() => lblStatus.Text = message));
                    AppManager.UpdateBatteryAction = message =>
                        Invoke((MethodInvoker)(() => txtBattery.Text = message));
                    AppManager.UpdateSignalAction = message =>
                        Invoke((MethodInvoker)(() => txtSignal.AppendText(message + Environment.NewLine)));
                    AppManager.UpdateResultAction = message =>
                        Invoke((MethodInvoker)(() => txtResult.AppendText(message + Environment.NewLine)));

                    // Initialize sensor with filename
                    AppManager.InitializeSensor(fileName);

                    // Set up scanner event
                    AppManager.Scanner.EventSensorsChanged += OnDeviceFound;
                }
                else
                {
                    MessageBox.Show("Canceled, you're leaving", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ExitApplication();
                }
            }
        }
        private void btnStartScan_Click(object sender, EventArgs e)
        {
            AppManager.StartScan(lstDevices);
            btnStartScan.Enabled = false;
            btnStopScan.Enabled = true;
        }

        private void btnStopScan_Click(object sender, EventArgs e)
        {
            AppManager.StopScan();
            btnStartScan.Enabled = true;
            btnStopScan.Enabled = false;
        }

        private void lstDevices_DoubleClick(object sender, EventArgs e)
        {
            if (lstDevices.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a device to connect.");
                return;
            }

            // Connect to selected device
            if (AppManager.ConnectToDevice(AppManager.Scanner.Sensors[lstDevices.SelectedIndex]))
            {
                // Display device info
                txtDeviceInfo.Text = AppManager.GetDeviceInfo();
                btnRecord.Enabled = true;
            }
        }

        private void btnShowGame_Click(object sender, EventArgs e)
        {
            try
            {
                screen = new GreetingScreen();
                screen.Show();
                btnRecord.Enabled = false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Use AppManager to clean up resources
            AppManager.CleanupSensor();
        }

        private void ExitApplication()
        {
            Environment.Exit(0);
        }
    }

    public class Record
    {
        public string Timestamp { get; set; }
        public long Milliseconds { get; set; }
        public long? TimeDiff { get; set; }
        public int Second { get; set; }
    }
    public static class AppManager
    {
        // Sensor-related properties
        public static bool isRecord { get; set; }
        public static SensorInfo SelectedDevice { get; private set; }
        public static BrainBitSensor Sensor { get; private set; }
        public static Scanner Scanner { get; private set; }
        public static StreamWriter Writer { get; private set; }
        public static List<Record> Records { get; private set; }

        // UI Update Delegates
        public static Action<string> UpdateStatusAction { get; set; }
        public static Action<string> UpdateBatteryAction { get; set; }
        public static Action<string> UpdateSignalAction { get; set; }
        public static Action<string> UpdateResultAction { get; set; }

        // File path for data storage
        private static string filePath = @"C:\Users\USER\Documents\data-sets\";
        private static string fileCSV = " ";
        private static string fileTxt = " ";

        // Initialize scanner and prepare for data collection
        public static void InitializeSensor(string fileName)
        {
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            Scanner = new Scanner(SensorFamily.SensorLEBrainBit);
            Scanner.EventSensorsChanged += OnDeviceFound;
            Records = new List<Record>();
            fileCSV = $"{filePath}{fileName}_{GetTimestamp()}.csv";
            fileTxt = $"{filePath}{fileName}_{GetTimestamp()}.txt";
            Writer = new StreamWriter($"{filePath}{fileName}_{GetTimestamp()}.csv");
            Writer.WriteLine("Timestamp,O1,O2,T3,T4,Order,Question,IsCorrect, IsSubmitted");
        }

        // Scan for available devices
        public static void StartScan(ListBox deviceList)
        {
            UpdateStatus("Scanning for devices...");
            deviceList.Items.Clear();
            Scanner.Start();
        }

        // Stop scanning for devices
        public static void StopScan()
        {
            UpdateStatus("Scan stopped.");
            Scanner.Stop();
        }

        // Device found event handler
        private static void OnDeviceFound(IScanner sender, IReadOnlyList<SensorInfo> devices)
        {
            // This assumes the ListBox is passed through a method or set up separately
            UpdateStatus($"{devices.Count} devices found");
        }

        // Connect to a selected device
        public static bool ConnectToDevice(SensorInfo selectedDevice)
        {
            try
            {
                SelectedDevice = selectedDevice;
                Sensor = (BrainBitSensor)Scanner.CreateSensor(selectedDevice);

                if (Sensor == null)
                {
                    UpdateStatus("Failed to create sensor instance.");
                    return false;
                }

                // Add sensor events
                Sensor.EventBatteryChanged += Sensor_EventBatteryChanged;
                Sensor.EventSensorStateChanged += Sensor_EventSensorStateChanged;
                Sensor.EventBrainBitSignalDataRecived += Sensor_EventBrainBitSignalDataRecived;

                UpdateStatus($"Connected to {selectedDevice.Name}.");
                return true;
            }
            catch (SDKException ex)
            {
                UpdateStatus($"Error creating BLE device: {ex.Message}");
                return false;
            }
        }

        // Display device information
        public static string GetDeviceInfo()
        {
            if (Sensor == null) return "No device connected";

            return $"Name: {Sensor.Name}\r\n" +
                   $"Address: {Sensor.Address}\r\n" +
                   $"Serial Number: {Sensor.SerialNumber}\r\n" +
                   $"Firmware Version: {Sensor.Version.FwMajor}.{Sensor.Version.FwMinor}\r\n";
        }

        // Start collecting signal data
        public static void StartCollectingSignal()
        {
            if (Sensor != null)
            {
                Sensor.ExecCommand(SensorCommand.CommandStartSignal);
            }
        }

        // Stop collecting signal data
        public static void StopCollectingSignal()
        {
            if (Sensor != null)
            {
                Sensor.ExecCommand(SensorCommand.CommandStopSignal);
                Writer.Flush();
                Writer.Dispose();
            }
        }

        // Event handler for signal data received
        private static void Sensor_EventBrainBitSignalDataRecived(ISensor sensor, BrainBitSignalData[] data)
        {
            foreach (BrainBitSignalData signal in data)
            {
                var record = new Record { Timestamp = GetTimestampDetail() };
                string temp = $"{record.Timestamp},{signal.O1 * 1e3},{signal.O2 * 1e3},{signal.T3 * 1e3},{signal.T4 * 1e3}," +
                    $"{RemoveCommas(GameSessionData.QuestionOrder.ToString())}," +
                    $"{RemoveCommas(GameSessionData.Question.ToString())}," +
                    $"{GameSessionData.IsCorrect}," + $"{CheckSubmit(GameSessionData.IsSubmitted)}";

                UpdateSignal(temp);
                Writer.WriteLine(temp);
                Records.Add(record);
            }
        }
        public async static void AnalyzeRecordedData()
        {
            string outputFilePath = fileTxt; // Path to save the output text file
            var outputBuilder = new StringBuilder(); // StringBuilder to collect output data

            void UpdateMessage(string message)
            {
                UpdateResult(message);
                outputBuilder.AppendLine(message); // Add message to output builder
            }

            UpdateMessage("Processing data.... please wait");
            await Task.Delay(5000);

            try
            {
                UpdateMessage("=== Timestamp Processor ===");

                // Check if the CSV file exists
                if (!File.Exists(fileCSV))
                {
                    UpdateMessage($"CSV file not found: {fileCSV}");
                    File.WriteAllText(outputFilePath, outputBuilder.ToString()); // Write output to file
                    return;
                }

                long? previousMilliseconds = null;
                long startTime = 0;
                bool isFirstRecord = true;
                var timeDiffs = new List<long>();
                var summaryDict = new Dictionary<int, int>();

                try
                {
                    var lines = File.ReadLines(fileCSV).Skip(1); // Skip header row

                    foreach (var line in lines)
                    {
                        var fields = line.Split(',');
                        if (fields.Length < 1) continue;

                        string timestamp = fields[0].Trim();
                        long currentMilliseconds;

                        // Convert timestamp to milliseconds
                        try
                        {
                            currentMilliseconds = TimestampToMilliseconds(timestamp);
                        }
                        catch (Exception ex)
                        {
                            UpdateMessage($"Error converting timestamp '{timestamp}': {ex.Message}");
                            continue;
                        }

                        // Calculate time difference
                        if (isFirstRecord)
                        {
                            startTime = currentMilliseconds;
                            isFirstRecord = false;
                        }
                        else
                        {
                            if (previousMilliseconds.HasValue)
                            {
                                long timeDiff = currentMilliseconds - previousMilliseconds.Value;
                                timeDiffs.Add(timeDiff);
                            }
                        }

                        // Calculate 'Second' and update summary
                        int second = (int)((currentMilliseconds - startTime) / 1000) + 1;
                        if (summaryDict.ContainsKey(second))
                        {
                            summaryDict[second]++;
                        }
                        else
                        {
                            summaryDict[second] = 1;
                        }

                        // Update previous timestamp
                        previousMilliseconds = currentMilliseconds;
                    }
                }
                catch (Exception ex)
                {
                    UpdateMessage($"Error reading CSV file: {ex.Message}");
                    File.WriteAllText(outputFilePath, outputBuilder.ToString()); // Write output to file
                    return;
                }

                if (!timeDiffs.Any())
                {
                    UpdateMessage("No valid time differences to calculate statistics.");
                    File.WriteAllText(outputFilePath, outputBuilder.ToString()); // Write output to file
                    return;
                }

                // Calculate average time difference and sampling rate
                double averageTimeDifference = timeDiffs.Average();
                double samplingRate = 1000.0 / averageTimeDifference;

                UpdateMessage($"\nAverage Time Difference: {averageTimeDifference:F2} ms");
                UpdateMessage($"Sampling Rate: {samplingRate:F0} Hz");

                // Display summary table
                int maxSecond = summaryDict.Keys.Max();
                UpdateMessage("\n=== Summary of Samples per Second ===");
                UpdateMessage("Second\tSample Count");

                for (int sec = 1; sec <= maxSecond; sec++)
                {
                    int sampleCount = summaryDict.ContainsKey(sec) ? summaryDict[sec] : 0;
                    UpdateMessage($"{sec}\t{sampleCount}");
                }

                // Calculate average and standard deviation of sampling rates
                double averageSamplingRateSummary = summaryDict.Values.Average();
                double stdDevSamplingRateSummary = Math.Sqrt(summaryDict.Values.Average(s => Math.Pow(s - averageSamplingRateSummary, 2)));

                UpdateMessage($"\nAverage Sampling Rate: {averageSamplingRateSummary:F2} samples/second");
                UpdateMessage($"Standard Deviation of Sampling Rate: {stdDevSamplingRateSummary:F2} samples/second");

                UpdateMessage("\nAnalysis Complete.");
            }
            catch (Exception ex)
            {
                UpdateMessage($"An error occurred during analysis: {ex.Message}");
                Console.WriteLine($"An error occurred during analysis: {ex}");
            }

            // Write all collected output to the text file
            File.WriteAllText(outputFilePath, outputBuilder.ToString());
        }

        // Method to clear records after analysis if needed
        public static void ClearRecords()
        {
            lock (Records)
            {
                Records.Clear();
            }
        }

        // Helper methods for UI Updates
        public static void UpdateStatus(string message)
        {
            UpdateStatusAction?.Invoke($"Status: {message}");
        }

        public static void UpdateBattery(string message)
        {
            UpdateBatteryAction?.Invoke(message);
        }

        public static void UpdateSignal(string message)
        {
            UpdateSignalAction?.Invoke(message);
        }

        public static void UpdateResult(string message)
        {
            UpdateResultAction?.Invoke(message);
        }

        // Battery event handler
        private static void Sensor_EventBatteryChanged(ISensor sensor, int batteryLevel)
        {
            UpdateBattery($"Battery Level: {batteryLevel}%");
        }

        // Sensor state event handler
        private static void Sensor_EventSensorStateChanged(ISensor sensor, SensorState state)
        {
            UpdateStatus($"{sensor.Name} state: {state}");
        }

        // Helper methods
        private static string GetTimestamp()
        {
            return DateTime.Now.ToString("yyyy.MM.dd.HH.mm");
        }

        private static string GetTimestampDetail()
        {
            return DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.fff");
        }

        private static string CheckSubmit(bool isSubmitted)
        {
            return isSubmitted ? "CLICKED SUBMIT" : " ";
        }

        private static string RemoveCommas(string s)
        {
            return s == null ? "default" : s.Replace(",", "");
        }

        // Cleanup method
        public static void CleanupSensor()
        {
            Writer?.Close();
            Scanner?.Stop();
            Scanner?.Dispose();
            Sensor?.Disconnect();
            Sensor?.Dispose();
        }

        // Existing method for timestamp conversion
        public static long TimestampToMilliseconds(string timestamp)
        {
            var parts = timestamp.Split('.');
            if (parts.Length < 7)
            {
                throw new ArgumentException("Timestamp does not have enough parts to convert.");
            }

            if (!int.TryParse(parts[3], out int hours) ||
                !int.TryParse(parts[4], out int minutes) ||
                !int.TryParse(parts[5], out int seconds) ||
                !int.TryParse(parts[6], out int milliseconds))
            {
                throw new ArgumentException("Timestamp contains invalid numeric parts.");
            }

            return hours * 3600 * 1000L + minutes * 60 * 1000L + seconds * 1000L + milliseconds;
        }
    }

}

