using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiplataformaDesktop
{
    public partial class FrmSplashScreen : Form
    {
        public FrmSplashScreen()
        {
            InitializeComponent();
        }

        private void FrmSplashScreen_Load(object sender, EventArgs e)
        {
            timer1.Start();
            progressBar1.Style = ProgressBarStyle.Marquee;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Style != ProgressBarStyle.Marquee)
            {
               
                int percentComplete = (int)(((double)progressBar1.Value / (double)progressBar1.Maximum) * 100);

                lblPorcentagem.Text = percentComplete.ToString() + "%";

                progressBar1.Value += 1;

                if (progressBar1.Value == progressBar1.Maximum)
                {
                    timer1.Stop();
                    this.Close();
                }
            }
        }
    }
}
