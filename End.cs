using System.Windows.Forms;
using System.Drawing;

namespace GameForestTest
{
    internal class End : ContainerControl
    {
        private TextBox msg;
        private Button btnRestart;
        public End()
        {
            Width = Screen.PrimaryScreen.Bounds.Size.Width;
            Height = Screen.PrimaryScreen.Bounds.Size.Height;
            Visible = false;

            msg = new TextBox()
            {
                Width = 680,
                Height = 180,
                Text = $"Ooops, end of times. GameOver. YourScore - {Play.SCORE_FINAL}",
                Multiline = true,
                WordWrap = true,
                Font = new Font("Segoe Script", 30),
                ForeColor = Color.IndianRed,
                BackColor = this.BackColor,
                BorderStyle = BorderStyle.None,
                TextAlign = HorizontalAlignment.Center,
            };
            msg.Location = new System.Drawing.Point
                (
                Width / 2 - (msg.Width / 2),
                Height / 2 - (msg.Height / 2)
                );

            btnRestart = new Button()
            {
                Width = 200,
                Height = 70,
                Text = "Restart",
                Font = new Font("Segoe Script", 30),
                ForeColor = Color.IndianRed,
            };
            btnRestart.Location = new System.Drawing.Point
                (
                Width / 2 - 130,
                Height / 2 + 80
                );
            btnRestart.Click += BtnRestartClick;

            Controls.Add(msg);
            Controls.Add(btnRestart);
            VisibleChanged += End_VisibleChanged;
        }

        private void End_VisibleChanged(object sender, System.EventArgs e)
        {
            if (Visible)
            {
                msg.Text = $"Ooops, end of times. GameOver. YourScore - {Play.SCORE_FINAL}";
            }
        }

        private void BtnRestartClick(object sender, System.EventArgs e)
        {
            Visible = false;
        }
    }
}