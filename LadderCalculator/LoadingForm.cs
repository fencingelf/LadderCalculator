using System;
using System.Windows.Forms;
using LadderCalculator;

namespace LadderCalculator
{
    public sealed partial class LoadingForm : Form
    {

        public LoadingForm(decimal total, string title)
        {
            Total = total;
            InitializeComponent();
            Progress = new AnnotatedProgressBar();
            Controls.Add(Progress);
            Progress.Dock = DockStyle.Fill;
            Text = title;
            Title = title;
        }

        public new void Close()
        {
            TaskbarProgress.SetState(Handle, TaskbarProgress.TaskbarStates.NoProgress);
            base.Close();
        }

        private AnnotatedProgressBar Progress { get; }
        private string Title { get; }
        private decimal Total { get; }

        public void UpdateProgress(int done, string message)
        {
            var progressPercentage = Total > 0 ? done * 100 / Total : 99;
            var text = $@"{message} {done} / {Total} ({progressPercentage:F2}%)";
            Text = Title + "..." + $@"{done} / {Total} ({progressPercentage:F2}%)";
            Progress.SetProgress((int)progressPercentage, text);
            TaskbarProgress.SetState(this.Handle, TaskbarProgress.TaskbarStates.Normal);
            TaskbarProgress.SetValue(this.Handle, (double)progressPercentage, 100.0);
            Application.DoEvents();
        }
    }
}
