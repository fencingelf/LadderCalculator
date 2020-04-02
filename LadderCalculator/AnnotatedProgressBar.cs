using System;
using System.Drawing;
using System.Windows.Forms;

namespace LadderCalculator
{
    public partial class AnnotatedProgressBar : UserControl
    {

        public event EventHandler<AnnotatedProgressBarClickEventArgs> ProgressBarClickedEvent;

        public int Maximum
        {
            get { return progressBar1.Maximum; }
            set { progressBar1.Maximum = value; }
        }
        
        public override string Text
        {
            get { return label1.Text; }
            set { label1.Text = value; }
        }

        public string TooltipText
        {
            get { return toolTip1.GetToolTip(this); }
            set
            {
                toolTip1.SetToolTip(this, value);
                toolTip1.Active = true;
            }
        }

        public int Value
        {
            get { return progressBar1.Value; }
            set { progressBar1.Value = value; }
        }

        public AnnotatedProgressBar()
        {
            InitializeComponent();
            BackColor = Color.Transparent;
        }

        public int GetProgress()
        {
            return progressBar1.Value;
        }

        public void SetProgress(int progressPercentage, string text)
        {
            Text = text;
            if (progressPercentage <= 0)
            {
                progressPercentage = 0;
            }
            else if (progressPercentage > 100)
            {
                progressPercentage = 100;
            }
            Maximum = 100;
            progressBar1.Value = progressPercentage;
        }

        private void CalculateFraction(decimal xpos)
        {
            var frac = (double)xpos / Width;
            ProgressBarClickedEvent?.Invoke(this, new AnnotatedProgressBarClickEventArgs() { Fraction = frac });
        }

        private void AnnotatedProgressBar_Click(object sender, System.EventArgs e)
        {
            if (e is MouseEventArgs ee)
            {
                CalculateFraction(ee.X);
            }
        }

        private void AnnotatedProgressBar_MouseClick(object sender, MouseEventArgs e)
        {
            CalculateFraction(e.X);

        }

        private void Label1_MouseClick(object sender, MouseEventArgs e)
        {
            var screenPoint = label1.PointToScreen(e.Location);
            var clientPoint = PointToClient(screenPoint);

            CalculateFraction(clientPoint.X);
        }

        private void ProgressBar1_MouseClick(object sender, MouseEventArgs e)
        {
            var screenPoint = progressBar1.PointToScreen(e.Location);
            var clientPoint = PointToClient(screenPoint);

            CalculateFraction(clientPoint.X);
        }
    }
    public class AnnotatedProgressBarClickEventArgs : EventArgs
    {
        public double Fraction { get; set; }
    }
}
