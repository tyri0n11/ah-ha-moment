using System;
using System.Windows.Forms;

namespace ah_ha_moment
{
    public partial class FileNameForm : Form
    {
        public string FileName { get; private set; }

        public FileNameForm()
        {
            InitializeComponent();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFileName.Text))
            {
                FileName = txtFileName.Text;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("File's name cannot be empty!", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
