using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesweeperNew
{
    class Game
    {
        public int BombCount { get; private set; }
        public Cell[,] Matrix { get; private set; }

        private int height;
        private int width;

        public void Start(int height, int width, int bombCount, Point enterPoint)
        {
            Matrix = new Cell[height, width];
            BombCount = bombCount;
            this.height = height;
            this.width = width;
            FillMatrix(enterPoint);
            SetDigits();
        }

        private void FillMatrix(Point enterPoint)
        {
            Random random = new Random();
            for (int i = 0; i < BombCount; i++)
            {
                int x = random.Next(height), y = random.Next(width);
                if (Matrix[x, y] == Cell.Mine
                    || (Math.Abs(enterPoint.X - x) < 2 && Math.Abs(enterPoint.Y - y) < 2))
                {
                    i--;
                    continue;
                }
                Matrix[x, y] = Cell.Mine;
            }
        }

        private void SetDigits()
        {
            for (int i = 0; i< height; i++)
                for (int j = 0; j< width; j++)
                {
                    if (Matrix[i, j] != Cell.Mine && CountMinesAround(i, j) > 0)
                        Matrix[i, j] = Cell.Digit;
                }
        }

        public int CountMinesAround(int x, int y)
        {
            Point[] cellsAround = GetPointsAround(x, y);
            int minesAround = 0;
            foreach (var cell in cellsAround)
                if (Matrix[cell.X, cell.Y] == Cell.Mine)
                    minesAround++;
            return minesAround;
        }

        public Point[] GetPointsAround(int x, int y)
        {
            List<Point> cellsAround = new List<Point>();
            for (int i = -1; i <= 1; i++)
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    if (x + i < 0 || x + i >= height
                        || y + j < 0 || y + j >= width) continue;
                    cellsAround.Add(new Point(x + i, y + j));
                }
            return cellsAround.ToArray();
        }
    }
    public enum Cell { Empty, Digit, Mine }
}
