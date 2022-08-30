var random = new Random(DateTime.Now.Millisecond);
int read = 0;

Console.WriteLine("Игра угадай число");

int tries = 0;
int victoryGoal = random.Next(100);
while (read != victoryGoal)
{
    string input = Console.ReadLine();
    while (String.IsNullOrWhiteSpace(input))
    {
        input = Console.ReadLine();
    }

    read = Int32.Parse(input);
    tries++;
    if (read < victoryGoal)
    {
        Console.WriteLine("Ваше число меньше загаданного");
    }
    if (read > victoryGoal)
    {
        Console.WriteLine("Ваше число больше загаданного");
    }
}
Console.WriteLine("Вы угадали.\nВам потребовалось {0} попыток", tries);