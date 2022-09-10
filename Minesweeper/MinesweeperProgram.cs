using System.Text;
StringBuilder fieldBuilder = new StringBuilder();

int width = 0, height = 0, mines = 0;
Console.CursorVisible = false;

ConsoleKey key = ConsoleKey.Enter;

bool isRunning = true;
while (isRunning)
{
    Console.Clear();
    bool inMenu = true;
    while (isRunning && inMenu)
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine("Select game mode");
        Console.WriteLine("1 - Easy (9x9)");
        Console.WriteLine("2 - Medium (16x16)");
        Console.WriteLine("3 - Hard (24x20)");
        Console.WriteLine("\n-----------------------------\n");
        Console.WriteLine("Move cursor - WASD/Arrow keys");
        Console.WriteLine("Open a cell - Space/Enter");
        Console.WriteLine("Mark a mine - F");
        key = Console.ReadKey(true).Key;

        switch (key)
        {
            case ConsoleKey.D1:
                width = 9;
                height = 9;
                mines = 10;
                inMenu = false;
                break;
            case ConsoleKey.D2:
                width = 16;
                height = 16;
                mines = 40;
                inMenu = false;
                break;
            case ConsoleKey.D3:
                width = 24;
                height = 20;
                mines = 100;
                inMenu = false;
                break;
            case ConsoleKey.Escape:
                isRunning = false;
                break;
        }
    }

    Minesweeper.Minesweeper game = new Minesweeper.Minesweeper(width, height, mines);
    Console.Clear();
    DrawField(game.Field, game.FieldWidth, game.FieldHeight, game.SelectedCell);


    while (true)
    {
        if (game.Victory)
        {
            Console.Clear();
            Console.WriteLine("Victory");
        }
        if (!game.IsRunning)
        {
            Console.ReadKey(true);
            Console.Clear();
            break;
        }

        if (key == ConsoleKey.Escape)
        {
            Console.Clear();
            break;
        }
        Console.SetCursorPosition(0, 0);
        key = Console.ReadKey(true).Key;
        switch (key)
        {
            case ConsoleKey.RightArrow:
            case ConsoleKey.D:
                game.MoveRight();
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.A:
                game.MoveLeft();
                break;
            case ConsoleKey.UpArrow:
            case ConsoleKey.W:
                game.MoveDown();
                break;
            case ConsoleKey.DownArrow:
            case ConsoleKey.S:
                game.MoveUp();
                break;
            case ConsoleKey.F:
                game.MarkCell();
                break;
            case ConsoleKey.Enter:
            case ConsoleKey.Spacebar:
                game.MakeMove();
                break;

        }
        DrawField(game.Field, game.FieldWidth, game.FieldHeight, game.SelectedCell);
    }
}
Console.Clear();
Console.WriteLine("Thanks for playing");

void DrawField(char[] field, int width, int height, int selectedCell)
{
    fieldBuilder.Clear();
    fieldBuilder.EnsureCapacity(width * 3 * height);
    for (int y = 0; y < height; y++)
    {
        for (int x = 0; x < width; x++)
        {
            int index = x + y * width;
            if (index == selectedCell)
            {
                fieldBuilder.Append('[');
                fieldBuilder.Append(field[index]);
                fieldBuilder.Append(']');
            }
            else
            {
                fieldBuilder.Append(' ');
                fieldBuilder.Append(field[index]);
                fieldBuilder.Append(' ');
            }
        }
        fieldBuilder.Append('\n');
    }
    Console.WriteLine(fieldBuilder.ToString());
}
