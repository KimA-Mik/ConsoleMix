namespace Minesweeper
{
    internal class Minesweeper
    {
        public int FieldWidth { get; }
        public int FieldHeight { get; }
        public char[] Field { get; }
        public int SelectedCell { get; set; }
        public bool IsRunning { get; set; }
        public bool Victory { get; set; }

        private readonly int[] _mines;
        private readonly int _minesCount;
        private bool _firstMove;
        private int _cellsLeft;

        private const char CLOSED_CELL = '@';
        private const char EMPTY_CELL = '.';
        private const char BOMB_CELL = '*';
        private const char FLAG_CELL = '^';
        public Minesweeper(int width, int height, int minesCnt)
        {
            FieldWidth = width;
            FieldHeight = height;
            _minesCount = minesCnt;
            _mines = new int[_minesCount];
            _cellsLeft = width * height;
            Field = new char[width * height];
            for (int i = 0; i < Field.Length; i++)
            {
                Field[i] = CLOSED_CELL;
            }
            SelectedCell = 0;
            _firstMove = true;
            IsRunning = true;
            Victory = false;
        }


        public void MoveRight()
        {
            int x, y;
            CellToCoordinate(SelectedCell, out x, out y);
            x = (x + 1) % FieldWidth;
            SelectedCell = CoordinatesToCell(x, y);
        }

        public void MoveLeft()
        {
            int x, y;
            CellToCoordinate(SelectedCell, out x, out y);
            x = (x == 0) ? FieldWidth - 1 : x - 1;
            SelectedCell = CoordinatesToCell(x, y);
        }

        public void MoveUp()
        {
            int x, y;
            CellToCoordinate(SelectedCell, out x, out y);
            y = (y + 1) % FieldHeight;
            SelectedCell = CoordinatesToCell(x, y);
        }

        public void MoveDown()
        {
            int x, y;
            CellToCoordinate(SelectedCell, out x, out y);
            y = (y == 0) ? FieldHeight - 1 : y - 1;
            SelectedCell = CoordinatesToCell(x, y);
        }

        public void MarkCell()
        {
            if (Field[SelectedCell] == FLAG_CELL)
            {
                Field[SelectedCell] = CLOSED_CELL;
            }
            else if (Field[SelectedCell] == CLOSED_CELL)
            {
                Field[SelectedCell] = FLAG_CELL;
            }

            _cellsLeft = Field.Where(x => x == CLOSED_CELL).Count();
            if (_cellsLeft == 0)
            {
                Victory = true;
                IsRunning = false;
            }
        }

        public void MakeMove()
        {
            if (_firstMove)
            {
                Init();
            }

            if (Field[SelectedCell] != CLOSED_CELL)
            {
                return;
            }

            if (_mines.Contains(SelectedCell))
            {
                for (int i = 0; i < _mines.Length; i++)
                {
                    Field[_mines[i]] = BOMB_CELL;
                }
                IsRunning = false;
                return;
            }
            OpenFreeCells(SelectedCell);

            _cellsLeft = Field.Where(x => x == CLOSED_CELL).Count();
            if (_cellsLeft == 0)
            {
                Victory = true;
                IsRunning = false;
            }
        }

        private void Init()
        {
            Random random = new(DateTime.Now.Millisecond);
            int index;
            int selectedX, selectedY;
            CellToCoordinate(SelectedCell, out selectedX, out selectedY);
            double initialSafeZoneSize;
            if (Field.Length < 100)
            {
                initialSafeZoneSize = 1.5;
            }
            else if (Field.Length < 300)
            {
                initialSafeZoneSize = 2.5;
            }
            else
            {
                initialSafeZoneSize = 3.5;
            }
            for (int i = 0; i < _minesCount; i++)
            {
                while (true)
                {
                    index = random.Next() % (FieldWidth * FieldHeight);
                    int curX, curY;
                    CellToCoordinate(index, out curX, out curY);

                    if (index != SelectedCell && !_mines.Contains(index)
                        && DistBetweenCells(selectedX, selectedY, curX, curY) > initialSafeZoneSize)
                    {
                        break;
                    }
                }
                _mines[i] = index;
            }
            _firstMove = false;
        }

        private int CountSurroundingBobs(int cell)
        {
            int x, y;
            int bobms = 0;
            CellToCoordinate(cell, out x, out y);

            for (int curY = y - 1; curY < y + 2; curY++)
            {
                if (curY < 0 || curY == FieldHeight)
                {
                    continue;
                }
                for (int curX = x - 1; curX < x + 2; curX++)
                {
                    if (curX < 0 || curX == FieldWidth)
                    {
                        continue;
                    }

                    int curCell = CoordinatesToCell(curX, curY);
                    if (_mines.Contains(curCell))
                    {
                        bobms++;
                    }
                }
            }
            return bobms;
        }

        private void OpenFreeCells(int cell, int depth = 0)
        {
            if (Field[cell] != CLOSED_CELL)
            {
                return;
            }

            int num = CountSurroundingBobs(cell);
            if (num == 0)
            {
                Field[cell] = EMPTY_CELL;
            }
            else
            {
                Field[cell] = num.ToString()[0];
                depth++;
            }
            if (depth < 1)
            {
                int x, y;
                CellToCoordinate(cell, out x, out y);
                for (int curY = y - 1; curY < y + 2; curY++)
                {
                    if (curY < 0 || curY == FieldHeight)
                    {
                        continue;
                    }
                    for (int curX = x - 1; curX < x + 2; curX++)
                    {
                        if (curX < 0 || curX == FieldWidth)
                        {
                            continue;
                        }
                        int curCell = CoordinatesToCell(curX, curY);
                        if (Field[curCell] == CLOSED_CELL && !_mines.Contains(curCell))
                        {
                            OpenFreeCells(curCell, depth);
                        }
                    }
                }
            }
        }

        private void CellToCoordinate(int cell, out int x, out int y)
        {
            y = cell / FieldWidth;
            x = cell - y * FieldWidth;
        }

        private int CoordinatesToCell(int x, int y)
        {
            return x + y * FieldWidth;
        }

        private double DistBetweenCells(int x1, int y1, int x2, int y2)
        {
            return Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1));
        }
    }
}
