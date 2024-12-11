using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using NeuroSDK;

namespace AhhaMoment
{
    public partial class AhhaMoment2 : Form
    {
        private int timeRemaining = 60000;
        private System.Windows.Forms.Timer countDownTimer;
        private int currentProblemIndex = 0;
        private int correctAnswers = 0;
        private int incorrectAnswers = 0;
        private static Random rng = new Random();
        private static List<Quizz> questions = new List<Quizz>();
        private Quizz currentQuiz = null;
        public static string dataQuestion = "C:\\BrainLife Codespace\\aha_dataset.txt";
        private string csvFilePath = @"C:\\Users\\USER\\Documents\\gaming_session_data.csv";
        private List<GameSessionData> sessionData = new List<GameSessionData>();
        private brainbit brainbitInstance;

        public AhhaMoment2()
        {
            InitializeComponent();
            InitializeTimer();
            initQuestion();
            Shuffle(questions);
            DisplayCurrentQuestion();

            // Initialize brainbit instance
            brainbitInstance = new brainbit();
        }

        private void initQuestion()
        {
            string pattern = @"\{\[(?<Question>.*?)\],\[(?<Hint>.*?)\],\[(?<Answer>.*?)\]\}";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            if (!File.Exists(dataQuestion))
            {
                MessageBox.Show($"Quiz data file not found: {dataQuestion}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            foreach (string line in File.ReadLines(dataQuestion))
            {
                Match match = regex.Match(line);
                if (match.Success)
                {
                    string question = match.Groups["Question"].Value.Trim();
                    string hint = match.Groups["Hint"].Value.Trim();
                    string answer = match.Groups["Answer"].Value.Trim();

                    if (!string.IsNullOrEmpty(question) && !string.IsNullOrEmpty(answer))
                    {
                        questions.Add(new Quizz(question, hint, answer));
                    }
                }
            }
        }

        public static void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        private void InitializeTimer()
        {
            countDownTimer = new System.Windows.Forms.Timer { Interval = 100 };
            countDownTimer.Tick += (sender, e) =>
            {
                timeRemaining -= 100;
                UpdateTimerDisplay();

                if (timeRemaining <= 0)
                {
                    countDownTimer.Stop();
                    MessageBox.Show("Time's up!", "Time Over", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    incorrectAnswers++;
                    currentProblemIndex++;
                    DisplayCurrentQuestion();
                }
            };
            countDownTimer.Start();
        }

        private void UpdateTimerDisplay()
        {
            int minutes = timeRemaining / 60000;
            int seconds = (timeRemaining % 60000) / 1000;
            int milliseconds = timeRemaining % 1000;

            timerLabel.Text = $"{minutes:D2}:{seconds:D2}.{milliseconds:D3}";
        }

        private void DisplayCurrentQuestion()
        {
            if (currentProblemIndex >= questions.Count)
            {
                MessageBox.Show($"Quiz completed!\nCorrect: {correctAnswers}\nIncorrect: {incorrectAnswers}", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                WriteSessionDataToCsv();
                this.Close();
                return;
            }

            Quizz currentQuiz = questions[currentProblemIndex];
            questionLabel.Text = currentQuiz.Question;
            hintLabel.Text = currentQuiz.Hint;
            answerBox.Text = "";
            answerBox.Focus();

            timeRemaining = 60000;
            UpdateTimerDisplay();
            countDownTimer.Start();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            currentQuiz = questions[currentProblemIndex];
            string userAnswer = answerBox.Text.Trim();
            bool isCorrect = userAnswer.Equals(currentQuiz.Answer, StringComparison.OrdinalIgnoreCase);

            sessionData.Add(new GameSessionData
            {
                QuestionOrder = currentProblemIndex + 1,
                Question = currentQuiz.Question,
                IsCorrect = isCorrect,
                EEGData = GetEEGData(),
                TimeStamp = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss.fff")
            });

            if (isCorrect)
            {
                correctAnswers++;
                MessageBox.Show("Correct!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                incorrectAnswers++;
                MessageBox.Show($"Incorrect! The correct answer is: {currentQuiz.Answer}", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            currentProblemIndex++;
            DisplayCurrentQuestion();
            WriteSessionDataToCsv();
        }

        private string GetEEGData()
        {
            if (brainbitInstance.queueData.Any())
            {
                return string.Join(", ", brainbitInstance.queueData.Select(signal => $"O1: {signal.O1 * 1e3} O2: {signal.O2 * 1e3} T3: {signal.T3 * 1e3} T4: {signal.T4 * 1e3}"));
            }
            return "No EEG data available";
        }

        private void WriteSessionDataToCsv()
        {
            if (sessionData.Count == 0) return;

            using (var writer = new StreamWriter(csvFilePath, append: true))
            {
                if (writer.BaseStream.Length == 0)
                {
                    writer.WriteLine("TimeStamp,QuestionOrder,Question,IsCorrect,EEGData");
                }

                foreach (var data in sessionData)
                {
                    writer.WriteLine($"{data.TimeStamp},{data.QuestionOrder},{data.Question},{data.IsCorrect},{data.EEGData}");
                }

                sessionData.Clear();
            }
        }
    }

    public class Quizz
    {
        public string Question { get; set; }
        public string Hint { get; set; }
        public string Answer { get; set; }

        public Quizz(string question, string hint, string answer)
        {
            Question = question;
            Hint = hint;
            Answer = answer;
        }
    }

    public class GameSessionData
    {
        public string TimeStamp { get; set; }
        public int QuestionOrder { get; set; }
        public string Question { get; set; }
        public bool IsCorrect { get; set; }
        public string EEGData { get; set; }
    }

    public class brainbit
    {
        private SensorInfo detectedDevice;
        private BrainBitSensor sensor;
        private Scanner scanner;
        public Queue<BrainBitSignalData> queueData = new Queue<BrainBitSignalData>();

        public brainbit()
        {
            scanner = new Scanner(SensorFamily.SensorLEBrainBit);
            scanner.EventSensorsChanged += onDeviceFound;
            scanner.Start();

            // Attempt to connect to a device
            ConnectToDevice();

            if (sensor == null)
            {
                MessageBox.Show("No device found to connect! Shutting down.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        public void ConnectToDevice()
        {
            sensor = scanner.CreateSensor(detectedDevice) as BrainBitSensor;
        }

        public void StartRecordSignalForAnInterval(int durationInSeconds)
        {
            sensor.EventBrainBitSignalDataRecived += Sensor_EventBrainBitSignalDataRecived;
            sensor.ExecCommand(SensorCommand.CommandStartSignal);
            Thread.Sleep(durationInSeconds * 1000);
            sensor.ExecCommand(SensorCommand.CommandStopSignal);
            sensor.EventBrainBitSignalDataRecived -= Sensor_EventBrainBitSignalDataRecived;
        }

        private void Sensor_EventBrainBitSignalDataRecived(ISensor sensor, BrainBitSignalData[] data)
        {
            foreach (var signal in data)
            {
                queueData.Enqueue(signal);
            }
        }

        private void onDeviceFound(IScanner scanner, IReadOnlyList<SensorInfo> sensors)
        {
            Console.WriteLine($"Found {sensors.Count} devices");
            foreach (var sensor in sensors)
            {
                Console.WriteLine($"With name {sensor.Name} and address {sensor.Address}");
            }
        }
    }
}
