using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameForestTest
{
    class Chip : Button
    {
        public (int, int) Position { get; set; }
        internal static (int, int) MoveFrom { get; set; } = (-1, -1);

        public Color Color { get; set; }
        public string Form { get; private set; }
        public static Dictionary<int, string> NUMBER_TO_FORM = new Dictionary<int, string>
        {
            [0] = "triangle",
            [1] = "square",
            [2] = "pentagon",
            [3] = "hexagon",
            [4] = "circle"
        };
        public static Dictionary<string, int> FORM_TO_NUMBER = new Dictionary<string, int>
        {
            ["triangle"] = 0,
            ["square"] = 1,
            ["pentagon"] = 2,
            ["hexagon"] = 3,
            ["circle"] = 4
        };
        public Chip(int form_number, int i, int k)
        {
            Width = 70;
            Height = 60;
            Margin = Padding.Empty;
            Padding = Padding.Empty;
            Position = (i, k);

            Form = NUMBER_TO_FORM[form_number];
            switch (Form)
            {
                case "triangle":
                    Color = Color.Red;
                    Paint += Triangle_Paint;
                    break;
                case "square":
                    Color = Color.DarkOliveGreen;
                    Paint += Square_Paint;
                    break;
                case "pentagon":
                    Color = Color.DarkOrange;
                    Paint += Pentagon_Paint;
                    break;
                case "hexagon":
                    Color = Color.MediumPurple;
                    Paint += Hexagon_Paint;
                    break;
                case "circle":
                    Color = Color.Coral;
                    Paint += Circle_Paint;
                    break;
            }
            Click += Chip_Click;
        }

        private void Chip_Click(object sender, EventArgs e)
        {
            if (Chip.MoveFrom == (-1, -1))
            {
                Chip.MoveFrom = (Position.Item1, Position.Item2);   
            }
            else
            {
                var playW = Parent as Field;
                if (IsNeighbours(Chip.MoveFrom, Position))
                {
                    Chip chip_last = playW.GetControlFromPosition(Chip.MoveFrom.Item2, Chip.MoveFrom.Item1) as Chip;

                    playW.SetCellPosition(this, new TableLayoutPanelCellPosition(Chip.MoveFrom.Item2, Chip.MoveFrom.Item1));
                    playW.SetCellPosition(chip_last, new TableLayoutPanelCellPosition(Position.Item2, Position.Item1));
                    chip_last.Position = Position;
                    Position = Chip.MoveFrom;
                    Chip.MoveFrom = (-1, -1);
                    playW.Focus();
                    playW.OnMovingEnd(chip_last, this);
                }
                else
                {
                    playW.Focus();
                    Chip.MoveFrom = (-1, -1);
                }
            }
        }

        private void Triangle_Paint(object sender, PaintEventArgs e)
        {
            Point[] points = new Point[4] { 
                new Point(5,55),
                new Point(35, 0),
                new Point(65, 55),
                new Point(5, 55)
            };
            
            e.Graphics.FillPolygon(new SolidBrush(Color), points);
        }

        private void Square_Paint(object sender, PaintEventArgs e)
        {
            Point[] points = new Point[5] {
                new Point(5, 30),
                new Point(35, 5),
                new Point(65, 30),
                new Point(35, 55),
                new Point(5, 30) 
            };
            e.Graphics.FillPolygon(new SolidBrush(Color), points);
        }

        private void Pentagon_Paint(object sender, PaintEventArgs e)
        {
            Point[] points = new Point[6] { 
                new Point(35, 5),
                new Point(65, 60*2/5),
                new Point(70*5/7, 55),
                new Point(70*2/7, 55),
                new Point(5, 60*2/5),
                new Point(35, 5)
            };
            e.Graphics.FillPolygon(new SolidBrush(Color), points);
        }

        private void Hexagon_Paint(object sender, PaintEventArgs e)
        {
            Point[] points = new Point[7] {
                new Point(35, 55),
                new Point(5, 60*3/4),
                new Point(5, 60/4),
                new Point(35, 5),
                new Point(65, 60/4),
                new Point(65, 60*3/4),
                new Point(35, 55)
            };
            e.Graphics.FillPolygon(new SolidBrush(Color), points);
        }

        private void Circle_Paint(object sender, PaintEventArgs e)
        {
            Rectangle rect = new Rectangle(5, 5, 60, 50);
            e.Graphics.FillEllipse(new SolidBrush(Color), rect);
        }

        private bool IsNeighbours((int,int) from, (int, int) to)
        {
            return Math.Abs((from.Item1 + from.Item2) - (to.Item1 + to.Item2)) == 1;
        }
    }
}
