using System.Windows.Forms;

namespace GameForestTest
{
    public partial class Window : Form
    {
        Start startScreen;
        Play playScreen;
        End endScreen;
        public Window()
        {
            InitializeComponent();
            Height = Screen.PrimaryScreen.Bounds.Size.Height;
            Width = Screen.PrimaryScreen.Bounds.Size.Width;
            StartPosition = FormStartPosition.CenterScreen;
            WindowState = FormWindowState.Maximized;

            startScreen = new Start();
            playScreen = new Play();
            endScreen = new End();
            Controls.Add(startScreen);
            Controls.Add(playScreen);
            Controls.Add(endScreen);

            for (int i = 0; i < Controls.Count; i++)
            {
                ((ContainerControl)Controls[i]).VisibleChanged += Window_VisibleChanged;
            }
        }

        private void Window_VisibleChanged(object sender, System.EventArgs e)
        {
            var screen = sender as ContainerControl;
            if (!screen.Visible)
            {
                var nextScreen = GetNextControl(screen, true);
                if (nextScreen == null)
                {
                    Controls[0].Visible = true;
                    playScreen.NewField();
                } else
                {
                    nextScreen.Visible = true;
                }
            }

        }
    }
}
