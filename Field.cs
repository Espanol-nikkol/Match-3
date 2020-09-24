using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameForestTest
{
    class Field : TableLayoutPanel
    {
        Random _rnd;
        private int[,] _field = new int[8, 8];
        private List<(int,int)> _chain = new List<(int,int)>();
        private bool isChain = false;
        private delegate void MovingEndHandler(Chip chip_1, Chip chip_2);
        event MovingEndHandler MovingEnd;

        public Field() 
        {
            _rnd = new Random();
            RowCount = 8;
            ColumnCount = 8;
            Height = 560;
            Width = 630;
            Location = new System.Drawing.Point
                (
                    Screen.PrimaryScreen.Bounds.Size.Width / 2 - (this.Width / 2),
                    Screen.PrimaryScreen.Bounds.Size.Height / 2 - (this.Height / 2)
                );

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    int chipNumber = GetChipNumber(i, k);
                    _field[i, k] = chipNumber;
                    Chip chip = new Chip(chipNumber, i, k);
                    Controls.Add(chip);
                }
            }
            MovingEnd += MovingEndAction;
        }

        private void MovingEndAction(Chip chip_1, Chip chip_2)
        {
            (int x1, int y1) = chip_1.Position;
            (int x2, int y2) = chip_2.Position;
            _field[x2, y2] = Chip.FORM_TO_NUMBER[chip_2.Form];
            _field[x1, y1] = Chip.FORM_TO_NUMBER[chip_1.Form];
            _chain = FindChains();
            if (isChain)
            {
                while (isChain)
                {
                    SuspendLayout();
                    RemoveChains();
                    _chain = FindChains();
                    ResumeLayout();
                }
            } else
            {
                SetCellPosition(chip_1, new TableLayoutPanelCellPosition(y2, x2));
                SetCellPosition(chip_2, new TableLayoutPanelCellPosition(y1, x1));
                chip_1.Position = (x2, y2);
                chip_2.Position = (x1, y1);
                _field[x1, y1] = Chip.FORM_TO_NUMBER[chip_2.Form];
                _field[x2, y2] = Chip.FORM_TO_NUMBER[chip_1.Form];
            }
        }

        public void OnMovingEnd(Chip chip_1, Chip chip_2) => MovingEnd(chip_1, chip_2);

        public bool IsChain(int last_last, int last, int cur)
        {
            if (last_last == last && cur == last_last)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetChipNumber(int i, int k )
        {
            int n = GetRandom();
            if (i < 2)
            {
                if (k >= 2)
                {
                    while (IsChain(_field[i, k - 2], _field[i, k - 1], n))
                    {
                        n = GetRandom();
                    }
                }
            }
            else
            {
                if (k < 2)
                {
                    while (IsChain(_field[i - 2, k], _field[i - 1, k], n))
                    {
                        n = GetRandom();
                    }
                }
                else
                {
                    while (IsChain(_field[i - 2, k], _field[i - 1, k], n) ||
                        IsChain(_field[i, k - 2], _field[i, k - 1], n))
                    {
                        n = GetRandom();
                    }
                }
            }
            return n;
        }

        private int GetRandom() => _rnd.Next(0, 500) / 100;

        private List<(int,int)> FindChains()
        {
            int[,] arr = new int[8, 8];
            var ans = new List<(int, int)>();
            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    if (arr[i,k] != 1)
                    {
                        (int left, int right, int top, int bottom) =
                            (
                            Go(i, k, 0, "left"),
                            Go(i, k, 0, "right"),
                            Go(i, k, 0, "up"),
                            Go(i, k, 0, "down")
                            );

                        if (left + right < 2 && top + bottom < 2)
                        {
                            arr[i, k] = 0;
                            continue;
                        }
                        else
                        {
                            if (!isChain)
                            {
                                isChain = true;
                            }
                            arr[i, k] = 1;
                            ans.Add((i, k));
                            if (left + right >= 2)
                            {
                                while (left > 0)
                                {
                                    arr[i, k - left] = 1;
                                    ans.Add((i, k - left));
                                    left--;
                                }
                                while (right > 0)
                                {
                                    arr[i, k + right] = 1;
                                    ans.Add((i, k + right));
                                    right--;
                                }
                            }
                            if (top + bottom >= 2)
                            {
                                while (top > 0)
                                {
                                    arr[i - top, k] = 1;
                                    ans.Add((i - top, k));

                                    top--;
                                }
                                while (bottom > 0)
                                {
                                    arr[i + bottom, k] = 1;
                                    ans.Add((i + bottom, k));
                                    bottom--;
                                }
                            }
                        }
                    }   
                }
            }
            ans.Sort();
            return ans;

            int Go(int i, int k, int depth, string direction)
            {
                if (depth <= 4)
                {
                    switch (direction)
                    {
                        case "left":
                            if (k != 0 && _field[i, k] == _field[i, k - 1])
                            {
                                return Go(i, k - 1, depth + 1, direction);
                            }
                            else 
                            {
                                return depth;
                            }
                        case "right":
                            if (k != 7 && _field[i, k] == _field[i, k + 1])
                            {
                                return Go(i, k + 1, depth + 1, direction);
                            }
                            else
                            {
                                return depth;
                            }
                        case "up":
                            if (i != 0 && _field[i, k] == _field[i - 1, k])
                            {
                                return Go(i - 1, k, depth + 1, direction);
                            }
                            else
                            {
                                return depth;
                            }
                        case "down":
                            if (i != 7 && _field[i, k] == _field[i + 1, k])
                            {
                                return Go(i + 1, k, depth + 1, direction);
                            }
                            else
                            {
                                return depth;
                            }
                        default:
                            return 0;
                    }
                }
                else
                {
                    return 0;
                }
            }
        }
        
        private void RemoveChains()
        {
            ((Play)Parent).CounterScore += _chain.Count * 100;
            foreach(var i in _chain)
            {
                Gravitation(i);
            }
        }

        private void Gravitation((int, int) pos)
        {
            (int row, int col) = pos;
            var chip_cur = GetControlFromPosition(col, row);
            var chip_cur_pos = GetPositionFromControl(chip_cur);
            if (row != 0)
            {
                var chip_top = GetControlFromPosition(col, row - 1);
                var chip_top_pos = GetPositionFromControl(chip_top);
                SetCellPosition(chip_top, chip_cur_pos);
                ((Chip)chip_top).Position = (chip_cur_pos.Row, chip_cur_pos.Column);
                _field[row, col] = Chip.FORM_TO_NUMBER[((Chip)chip_top).Form];
                SetCellPosition(chip_cur, chip_top_pos);
                Gravitation((row - 1, col));
            } else
            {
                var chip_new = new Chip(GetRandom(), 0, col);
                _field[0, col] = Chip.FORM_TO_NUMBER[chip_new.Form];
                Controls.Add(chip_new);
                var chip_new_pos = GetPositionFromControl(chip_new);
                SetCellPosition(chip_new, chip_cur_pos);
                SetCellPosition(chip_cur, chip_new_pos);
                Controls.Remove(GetControlFromPosition(0, 8));
            }
            if (isChain)
            {
                isChain = false;
            }
        }
    }
}
