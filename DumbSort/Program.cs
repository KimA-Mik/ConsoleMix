using System.Collections.Concurrent;

var arr = new int[25];
var sortedList = new List<int>();
var concurrentSorted = new ConcurrentDictionary<int, int>();
var bag = new ConcurrentBag<int>();

Random random = new Random();
for (int i = 0; i < arr.Length; i++)
{
    arr[i] = random.Next() % 1000;
}

List<Task> tasks = new List<Task>();
foreach (var i in arr)
{
    tasks.Add(new(() =>
    {
        Task.Delay(i);
        concurrentSorted.TryAdd(concurrentSorted.Count, i);
        bag.Add(i);
        sortedList.Add(i);
    }));
}

foreach (var task in tasks)
{
    task.Start();
}

await Task.WhenAll(tasks);

Console.WriteLine("Original array(size: {0}):",arr.Length);
foreach (var i in arr)
{
    Console.Write("{0} ",i);
}
Console.WriteLine("\nSorted List (size: {0}):", sortedList.Count);
foreach (var i in sortedList)
{
    Console.Write("{0} ",i);
}

Console.WriteLine("\nSorted Bag (size: {0}):", bag.Count);
foreach (var i in bag)
{
    Console.Write("{0} ",i);
}

Console.WriteLine("\nSorted Dict (size: {0}):", concurrentSorted.Count);
foreach (var i in concurrentSorted)
{
    Console.Write("{0} ",i);
}