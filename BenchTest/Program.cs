// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Text;

BenchmarkRunner.Run<StringConcatVsStringBuilderBenchmark>();
Console.ReadLine();

[MemoryDi]
public class StringConcatVsStringBuilderBenchmark
{
    private readonly string
        _str1, _str2, _str3, _str4, _str5, _str6, _str7;
    public StringConcatVsStringBuilderBenchmark()
    {
        _str1 = new string('1', 10);
        _str2 = new string('2', 50);
        _str3 = new string('3', 150);
        _str4 = new string('4', 500);
        _str5 = new string('5', 1000);
        _str6 = new string('6', 1500);
        _str7 = new string('7', 2500);
    }

    [Benchmark]
    public string StringConcat()
    {
        return _str1 + _str2 + _str3 + _str4 + _str5 + _str6 + _str7;
    }

    [Benchmark]
    public string StringBuilderAuto()
    {
        var builder = new StringBuilder();
        builder.
            Append(_str1).
            Append(_str2).
            Append(_str3).
            Append(_str4).
            Append(_str5).
            Append(_str6).
            Append(_str7);
        return builder.ToString();
    }

    [Benchmark]
    public string StringBuilderHelped()
    {
        var size = _str1.Length+_str2.Length +_str3.Length + _str4.Length + _str5.Length + _str6.Length + _str7.Length;
        var builder = new StringBuilder();
        builder.EnsureCapacity(size);
        builder.
            Append(_str1).
            Append(_str2).
            Append(_str3).
            Append(_str4).
            Append(_str5).
            Append(_str6).
            Append(_str7);
        return builder.ToString();
    }
}
