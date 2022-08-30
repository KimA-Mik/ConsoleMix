using Memory_Eater;

ConsoleKey key = ConsoleKey.Enter;
List<DataChunk> data = new();

while (key != ConsoleKey.Escape)
{
    key = Console.ReadKey(true).Key;
    Console.Clear();

    switch (key)
    {
        case ConsoleKey.UpArrow:
            data.Add(new(256));
            Console.WriteLine("Data chunk was allocated.");
            Console.WriteLine("Total {0} GB allocated", data.Count / 4f);
            break;
        case ConsoleKey.DownArrow:
            data = new();
            GC.Collect();
            Console.WriteLine("Memory cleared");
            break;
    }
}
