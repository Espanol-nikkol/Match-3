using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GameForestTest
{
    internal class Play : ContainerControl
    {
        private int _counterTimer  = 60;
        private int _counterScore = 0;
        internal int CounterScore { 
            get { return _counterScore; }
            set 
            {
                _counterScore = value;
                ScoreField.Text = $"{value}";
            }
        }
        internal static int SCORE_FINAL { get; private set;}
        private Field Field;
        private readonly TextBox TimerTimeField;
        private TextBox ScoreField;
        private System.Windows.Forms.Timer TimerTime;
        public Play()
        {
            TimerTime = new System.Windows.Forms.Timer()
            {
                Interval = 1000,
            };
            TimerTime.Tick += TimerTime_Tick;
            NewField();
            TimerTimeField = new TextBox()
            {
                Text = $"{_counterTimer}",
                Width = 100,
                Height = 80,
                Font = new Font("Segoe Script", 30),
                ForeColor = Color.IndianRed,
                BackColor = this.BackColor,
                Location = new System.Drawing.Point
                (
                     Screen.PrimaryScreen.Bounds.Size.Width - 200,
                     Screen.PrimaryScreen.Bounds.Size.Height - 260
                ),
                BorderStyle = BorderStyle.None,
            };
            ScoreField = new TextBox()
            {
                Text = $"{CounterScore}",
                Width = 200,
                Height = 100,
                Font = new Font("Segoe Script", 30),
                ForeColor = Color.IndianRed,
                BackColor = this.BackColor,
                Location = new System.Drawing.Point
                (
                     Screen.PrimaryScreen.Bounds.Size.Width - 200,
                     Screen.PrimaryScreen.Bounds.Size.Height - 200
                ),
                BorderStyle = BorderStyle.None,
            };
            Width = Screen.PrimaryScreen.Bounds.Size.Width;
            Height = Screen.PrimaryScreen.Bounds.Size.Height;
            Visible = false;
            Controls.Add(Field);
            Controls.Add(TimerTimeField);
            Controls.Add(ScoreField);
            VisibleChanged += PlayVisibleChanged;
        }

        internal void NewField()
        {
            Field = new Field();
        }

        private void PlayVisibleChanged(object sender, System.EventArgs e)
        {
            if (Visible)
            {
                TimerTime.Start();
            }
        }

        private void TimerTime_Tick(object sender, System.EventArgs e)
        {
            _counterTimer--;
            TimerTimeField.Text = $"{_counterTimer}";
            if (_counterTimer == 0)
            {
                SCORE_FINAL = CounterScore;
                TimerTime.Stop();
                Visible = false;
                _counterTimer = 60;
                CounterScore = 0;
            }
        }
    }
}