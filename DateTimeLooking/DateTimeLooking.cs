long ticks = DateTime.Now.Ticks;
Console.WriteLine("Total ticks \t{0}"       , ticks);
Console.WriteLine("Ticks max value {0}"     , long.MaxValue);
Console.WriteLine("Remaining Ticks {0}\n"   , long.MaxValue - ticks);

long seconds    = ticks     / 10_000_000;
long minutes    = seconds   / 60;
long hours      = minutes   / 60;
long days       = hours     / 24;
long tempDays   = days;
long years      = 0;
long curYear    = 0;
while (tempDays > 0)
{
    tempDays -= GetDaysCount(curYear++);
    years++;
}
Console.WriteLine("Passed {0} seconds"  , seconds);
Console.WriteLine("Passed {0} minutes"  , minutes);
Console.WriteLine("Passed {0} hours"    , hours);
Console.WriteLine("Passed {0} days"     , days);
Console.WriteLine("Passed {0} years\n"  , years);

seconds     = long.MaxValue / 10_000_000;
minutes     = seconds       / 60;
hours       = minutes       / 60;
days        = hours         / 24;
tempDays    = days;
years       = 0;
curYear     = 0;
while (tempDays > 0)
{
    tempDays -= GetDaysCount(curYear++);
    years++;
}
Console.WriteLine("Total avialiable {0} seconds in Long data type by ticks" , seconds);
Console.WriteLine("Total avialiable {0} minutes in Long data type by ticks" , minutes);
Console.WriteLine("Total avialiable {0} hours in Long data type by ticks"   , hours);
Console.WriteLine("Total avialiable {0} days in Long data type by ticks"    , days);
Console.WriteLine("Total avialiable {0} years in Long data type by ticks\n" , years);

seconds     = (long.MaxValue - ticks)   / 10_000_000;
minutes     = seconds                   / 60;
hours       = minutes                   / 60;
days        = hours                     / 24;
tempDays    = days;
years       = 0;
curYear     = 0;
while (tempDays > 0)
{
    tempDays -= GetDaysCount(curYear++);
    years++;
}
Console.WriteLine("Total remaining {0} seconds in Long data type by ticks"  , seconds);
Console.WriteLine("Total remaining {0} minutes in Long data type by ticks"  , minutes);
Console.WriteLine("Total remaining {0} hours in Long data type by ticks"    , hours);
Console.WriteLine("Total remaining {0} days in Long data type by ticks"     , days);
Console.WriteLine("Total remaining {0} years in Long data type by ticks\n"  , years);

Console.ReadKey();

static long GetDaysCount(long year)
{
    if (year % 4 == 0 && year % 100 != 0 && year % 400 > 0)
    {
        return 366;
    }
    return 365;
}