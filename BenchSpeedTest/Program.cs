// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

BenchmarkRunner.Run<FibonacciBench>();

[MemoryDiagnoser]
public class FibonacciBench
{
    [Params(25)]
    public long index;

    [Benchmark]
    public void BenchDynamicMethod()
    {
        var res = DynamicWay(index);
    }
    
    [Benchmark]
    public void BenchRecursiveMethod()
    {
        var res = RecursiveWay(index);
    }
    
    public long DynamicWay(long ind)
    { 
        long[] arr = new long[ind];
        arr[0] = 1;
        arr[1] = 1;

        for (long i = 2; i < arr.Length; i++)
            arr[i] = arr[i - 1] + arr[i - 2];
        
        return arr[ind-1];
    }

    public long RecursiveWay(long ind)
    {
        if (ind < 3)
            return 1;

        return RecursiveWay(ind - 1) + RecursiveWay(ind - 2);
    }
}