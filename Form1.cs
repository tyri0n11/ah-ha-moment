using System;
using System.Windows.Forms;

namespace AhhaMoment
{
    
    public partial class AhhaMoment1 : Form
    {
        public int timeStart = 3;
        public AhhaMoment1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("The experiment will start in 3s");
            
            //Form2 f2 = new Form2();
            //f2.Show();
            timeStart = 3;
            countStartTimer.Start();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timeRemainStart.Text = timeStart--.ToString();
            if(timeStart == -1)
            {   
                AhhaMoment2 f2 = new AhhaMoment2();
                f2.Show();
                this.Hide();
                //settings.Close();
            }
            
        }

        private void timeRemainStart_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}
