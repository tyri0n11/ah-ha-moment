
using System.Text.RegularExpressions;

namespace ah_ha_moment
{

    public partial class GamingScreen : Form
    {
        private static int TIME_INTERVAL = 45000;
        private int timeRemaining = TIME_INTERVAL;
        private System.Windows.Forms.Timer countDownTimer;
        private int currentProblemIndex = 0;
        private int correctAnswers = 0;
        private int incorrectAnswers = 0;
        private static Random rng = new Random();
        private static List<Quizz> questions = new List<Quizz>();
        private Quizz currentQuiz;


        public static string dataQuestion = "C:\\BrainLife Codespace\\ahha-questions.txt";

        public GamingScreen()
        {
            InitializeComponent();
            InitializeTimer();
            initQuestion();
            Shuffle(questions);
            DisplayCurrentQuestion();

        }



        private void initQuestion()
        {
            string pattern = @"\{\[(?<Question>.*?)\], \[(?<Hint>.*?)\], \[(?<Answer>.*?)\]\}";
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
            countDownTimer = new System.Windows.Forms.Timer { Interval = 1000 };
            countDownTimer.Tick += (sender, e) =>
            {
                timeRemaining -=1000;
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

            timerLabel.Text = $"{minutes:D2}:{seconds:D2}";
            if (timeRemaining <= ((TIME_INTERVAL*2)/3))
            {
                hintLabel.Visible = true;
            }
        }

        private async void DisplayCurrentQuestion()
        {
            GameSessionData.EventCode = "Reading question";
            if (currentProblemIndex >= questions.Count)
            {
                try
                {
                    AppManager.isRecord = false;
                    AppManager.StopCollectingSignal();
                    MessageBox.Show($"Quiz completed!\nCorrect: {correctAnswers}\nIncorrect: {incorrectAnswers}", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    countDownTimer.Stop();
                    countDownTimer.Dispose();
                    AppManager.AnalyzeRecordedData();
                    Hide();
                    Dispose();
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                
            }
            else
            {
                AppManager.isRecord = true;
                currentQuiz = questions[currentProblemIndex];
                questionLabel.Text = currentQuiz.Question;
                hintLabel.Text = "Hint";
                hintLabel.Visible = false;
                answerBox.Text = "";
                answerBox.Focus();

                GameSessionData.QuestionOrder = currentProblemIndex + 1;
                GameSessionData.Question = currentQuiz.Question;
                countDownTimer.Stop();
                timeRemaining = TIME_INTERVAL;
                UpdateTimerDisplay();
                countDownTimer.Start();
            }


        }
        private void HintLabel_Click(object sender, EventArgs e)
        {
            if (hintLabel.Text == "Hint")
            {
                hintLabel.Text = currentQuiz.Hint;
                GameSessionData.EventCode = "Reading hint";
            }
            else
            {
                hintLabel.Text = "Hint";
            }
        }
        string GenerateQuestResult(bool condition)
        {
            if (condition)
            {
                return "correct";
            }
            return "incorrect";
        }

        private async void SubmitButton_Click(object sender, EventArgs e)
        {
            currentQuiz = questions[currentProblemIndex];
            string userAnswer = answerBox.Text.Trim();
            bool isCorrect = userAnswer.Equals(currentQuiz.Answer, StringComparison.OrdinalIgnoreCase);

            GameSessionData.QuestionOrder = currentProblemIndex + 1;
            GameSessionData.Question = currentQuiz.Question;
            //GameSessionData.IsCorrect = isCorrect;
            GameSessionData.EventCode = $"Click Submit, {GenerateQuestResult(isCorrect)}";
            await Task.Delay(10);
            if (isCorrect)
            {
                correctAnswers++;
                MessageBox.Show("Correct!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                GameSessionData.EventCode = " ";
            }
            else
            {
                incorrectAnswers++;
                MessageBox.Show($"Incorrect! The correct answer is: {currentQuiz.Answer}", "Try Again", MessageBoxButtons.OK, MessageBoxIcon.Error);
                GameSessionData.EventCode = " ";
            }

            currentProblemIndex++;
            DisplayCurrentQuestion();
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

    public static class GameSessionData
    {
        public static int QuestionOrder { get; set; }
        public static string Question { get; set; }
        public static string EventCode { get; set; }
        public static bool IsCorrect { get; set; }
        public static bool IsSubmitted { get; set; }
    }
}
