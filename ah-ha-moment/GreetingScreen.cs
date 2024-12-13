

namespace ah_ha_moment
{
    public partial class GreetingScreen : Form
    {
        private int timeStart = 3;
        public GreetingScreen()
        {
            InitializeComponent();
            storeGlobal();
            StartPosition = FormStartPosition.CenterScreen;
        }
        void storeGlobal()
        {
            GameSessionData.Question = "default";
            GameSessionData.QuestionOrder = 0;
            GameSessionData.IsSubmitted = false;
            GameSessionData.IsCorrect = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            timeStart = 3;
            countStartTimer.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timeRemainStart.Text = timeStart--.ToString();
            if (timeStart == -1)
            {
                GamingScreen game = new GamingScreen();
                game.Show();
                Hide();

                AppManager.StartCollectingSignal();
                //settings.Close();
            }

        }
    }
}
