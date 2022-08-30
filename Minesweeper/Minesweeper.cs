namespace Minesweeper
{
    internal class Minesweeper
    {
        public int fieldWidth { get; }
        public int fieldHeight { get; }
        public char[] field { get; }
        public int selectedCell { get; set; }
        public bool isRunning { get; set; }
        public bool victory { get; set; }

        private int[] mines;
        private int minesCount;
        private bool firstMove;
        private int cellsLeft;

        private const char CLOSED_CELL = '@';
        private const char EMPTY_CELL = '.';
        private const char BOMB_CELL = '*';
        private const char FLAG_CELL = '^';
        public Minesweeper(int width, int height, int minesCnt)
        {
            fieldWidth = width;
            fieldHeight = height;
            minesCount = minesCnt;
            mines = new int[minesCount];
            cellsLeft = width * height;
            field = new char[width * height];
            for (int i = 0; i < field.Length; i++)
            {
                field[i] = CLOSED_CELL;
            }
            selectedCell = 0;
            firstMove = true;
            isRunning = true;
            victory = false;
        }


        public void MoveRight()
        {
            int x, y;
            CellToCoordinate(selectedCell, out x, out y);
            x = (x + 1) % fieldWidth;
            selectedCell = CoordinatesToCell(x, y);
        }

        public void MoveLeft()
        {
            int x, y;
            CellToCoordinate(selectedCell, out x, out y);
            x = (x == 0) ? fieldWidth - 1 : x - 1;
            selectedCell = CoordinatesToCell(x, y);
        }

        public void MoveUp()
        {
            int x, y;
            CellToCoordinate(selectedCell, out x, out y);
            y = (y + 1) % fieldHeight;
            selectedCell = CoordinatesToCell(x, y);
        }

        public void MoveDown()
        {
            int x, y;
            CellToCoordinate(selectedCell, out x, out y);
            y = (y == 0) ? fieldHeight - 1 : y - 1;
            selectedCell = CoordinatesToCell(x, y);
        }

        public void MarkCell()
        {
            if (field[selectedCell] == FLAG_CELL)
            {
                field[selectedCell] = CLOSED_CELL;
            }
            else if (field[selectedCell] == CLOSED_CELL)
            {
                field[selectedCell] = FLAG_CELL;
            }

            cellsLeft = field.Where(x => x == CLOSED_CELL).Count();
            if (cellsLeft == 0)
            {
                victory = true;
                isRunning = false;
            }
        }

        public void MakeMove()
        {
            if (firstMove)
            {
                Init();
            }

            if (field[selectedCell] != CLOSED_CELL)
            {
                return;
            }

            if (mines.Contains(selectedCell))
            {
                for (int i = 0; i < mines.Length; i++)
                {
                    field[mines[i]] = BOMB_CELL;
                }
                isRunning = false;
                return;
            }
            OpenFreeCells(selectedCell);

            cellsLeft = field.Where(x => x == CLOSED_CELL).Count();
            if (cellsLeft == 0)
            {
                victory = true;
                isRunning = false;
            }
        }

        private void Init()
        {
            Random random = new(DateTime.Now.Millisecond);
            int index;
            int selectedX, selectedY;
            CellToCoordinate(selectedCell, out selectedX, out selectedY);
            double initialSafeZoneSize;
            if (field.Length < 100)
            {
                initialSafeZoneSize = 1.5;
            }
            else if (field.Length < 300)
            {
                initialSafeZoneSize = 2.5;
            }
            else
            {
                initialSafeZoneSize = 3.5;
            }
            for (int i = 0; i < minesCount; i++)
            {
                while (true)
                {
                    index = random.Next() % (fieldWidth * fieldHeight);
                    int curX, curY;
                    CellToCoordinate(index, out curX, out curY);

                    if (index != selectedCell && !mines.Contains(index)
                        && DistBetweenCells(selectedX, selectedY, curX, curY) > initialSafeZoneSize)
                    {
                        break;
                    }
                }
                mines[i] = index;
            }
            firstMove = false;
        }

        private int CountSurroundingBobs(int cell)
        {
            int x, y;
            int bobms = 0;
            CellToCoordinate(cell, out x, out y);

            for (int curY = y - 1; curY < y + 2; curY++)
            {
                if (curY < 0 || curY == fieldHeight)
                {
                    continue;
                }
                for (int curX = x - 1; curX < x + 2; curX++)
                {
                    if (curX < 0 || curX == fieldWidth)
                    {
                        continue;
                    }

                    int curCell = CoordinatesToCell(curX, curY);
                    if (mines.Contains(curCell))
                    {
                        bobms++;
                    }
                }
            }
            return bobms;
        }

        private void OpenFreeCells(int cell, int depth = 0)
        {
            if (field[cell] != CLOSED_CELL)
            {
                return;
            }

            int num = CountSurroundingBobs(cell);
            if (num == 0)
            {
                field[cell] = EMPTY_CELL;
            }
            else
            {
                field[cell] = num.ToString()[0];
                depth++;
            }
            if (depth < 1)
            {
                int x, y;
                CellToCoordinate(cell, out x, out y);
                for (int curY = y - 1; curY < y + 2; curY++)
                {
                    if (curY < 0 || curY == fieldHeight)
                    {
                        continue;
                    }
                    for (int curX = x - 1; curX < x + 2; curX++)
                    {
                        if (curX < 0 || curX == fieldWidth)
                        {
                            continue;
                        }
                        int curCell = CoordinatesToCell(curX, curY);
                        if (field[curCell] == CLOSED_CELL && !mines.Contains(curCell))
                        {
                            OpenFreeCells(curCell, depth);
                        }
                    }
                }
            }
        }

        private void CellToCoordinate(int cell, out int x, out int y)
        {
            y = cell / fieldWidth;
            x = cell - y * fieldWidth;
        }

        private int CoordinatesToCell(int x, int y)
        {
            return x + y * fieldWidth;
        }

        private double DistBetweenCells(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
    }
}
