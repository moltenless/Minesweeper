using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MinesweeperNew
{
    public partial class Form1 : Form
    {
        int height = 17, width = 14, bombCount = 30;
        Point enterPoint;
        Game game;

        public Form1()
        {
            InitializeComponent();
            PrepareField();
        }

        private void PrepareField()
        {
            int sizeValue = 40;
            Button[,] buttons = new Button[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    buttons[i, j] = new Button
                    {
                        Size = new Size(sizeValue, sizeValue),
                        Location = new Point(j * sizeValue, i * sizeValue),
                        Tag = new Point(i, j),
                    };
                    buttons[i, j].MouseDown += EmptyFieldClick;
                }
            this.Size = new Size(sizeValue * width + 20, sizeValue * height + 40);
            this.Controls.Clear();
            foreach (var row in buttons)
                this.Controls.Add(row);
        }

        private void EmptyFieldClick(object sender, MouseEventArgs e)
        {
            enterPoint = (Point)(sender as Button).Tag;
            StartGame();
            OpenCell(sender, e);
        }

        private void StartGame()
        {
            game = new Game();
            game.Start(height, width, bombCount, enterPoint);

            passedCellsBuffer.Clear();

            for (int i = 0; i < this.Controls.Count; i++)
            {
                this.Controls[i].MouseDown -= EmptyFieldClick;
                this.Controls[i].MouseDown += OpenCell;
            }
        }

        List<Point> passedCellsBuffer = new List<Point>();
        private void OpenCell(object sender, MouseEventArgs e)
        {
            Button button = sender as Button;
            Point point = (Point)button.Tag;

            if (e != null && e.Button == MouseButtons.Right)
            {
                button.BackColor = Color.Yellow;
                return;
            }

            if (ContainsPoint(passedCellsBuffer, point))
                return;

            passedCellsBuffer.Add(point);
            if (game.Matrix[point.X, point.Y] == Cell.Empty)
            {
                button.Enabled = false;

                Point[] around = game.GetPointsAround(point.X, point.Y);
                for (int i = 0; i < around.Length; i++)
                {
                    Button next = FindButtonByPoint(around[i]);
                    OpenCell(next, null);
                }
            }
            if (game.Matrix[point.X, point.Y] == Cell.Digit)
            {
                int digit = game.CountMinesAround(point.X, point.Y);
                button.Text = digit.ToString();
                button.Enabled = false;
            }
            if (game.Matrix[point.X, point.Y] == Cell.Mine) PrepareField();
        }

        private Button FindButtonByPoint(Point point)
        {
            for (int i = 0; i < Controls.Count; i++)
            {
                Point current = (Point)(Controls[i] as Button).Tag;
                if (current.X == point.X && current.Y == point.Y)
                    return Controls[i] as Button;
            }
            throw new Exception();
        }

        private bool ContainsPoint(List<Point> list, Point point)
        {
            foreach (var p in list)
                if (p.X == point.X && p.Y == point.Y)
                    return true;
            return false;
        }
    }
}
