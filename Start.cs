using System;
using System.Windows.Forms;
using System.Drawing;

namespace GameForestTest
{
    class Start : ContainerControl
    {
        private Button playBtn;
        
        public Start()
        {
            Text = "Match3 Test";
            Height = Screen.PrimaryScreen.Bounds.Size.Height;
            Width = Screen.PrimaryScreen.Bounds.Size.Width;

            playBtn = new Button()
            {
                Text = "Let`s play!",
                Width = 300,
                Height = 70,
                Font = new Font("Segoe Script", 30),
                ForeColor = Color.IndianRed,
            };
            playBtn.Location = new System.Drawing.Point
                (
                this.Width / 2 - (playBtn.Width / 2),
                this.Height / 2 - (playBtn.Height / 2)
                );
            
            playBtn.Click += new EventHandler(PlayBtn_Click);

            this.Controls.Add(playBtn);
        }


        private void PlayBtn_Click(Object sender, EventArgs e)
        {
            this.Visible = false;
        }

    }
}
