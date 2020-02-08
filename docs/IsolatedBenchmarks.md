# Isolated Benchmarks

These benchmarks are designed to test specific aspects of the calculation, avoiding specific optimizations.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

## Edge Difference Benchmark

This benchmark eliminates bonuses for trimming equal characters at the start and end.
The strings the same length and, besides the edges, are only filled with matching characters.
This then forces the fastest path internally (least operations) for calculating distance.

|        Method |       Runtime |          Mean |        Error |       StdDev | Ratio |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |--------------:|-------------:|-------------:|------:|--------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 | 305,045.28 ns | 3,064.405 ns | 2,866.446 ns | 1.000 | 74.2188 | 20.9961 |     - |  301049 B |
| Quickenshtein |    .NET 4.7.2 |     520.40 ns |     5.396 ns |     4.783 ns | 0.002 |       - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |  75,289.86 ns |   527.362 ns |   493.295 ns | 0.247 |  0.2441 |       - |     - |    1092 B |
|               |               |               |              |              |       |         |         |       |           |
|      Baseline | .NET Core 3.0 | 312,523.21 ns | 4,324.621 ns | 3,611.254 ns | 1.000 | 64.9414 | 22.9492 |     - |  291737 B |
| Quickenshtein | .NET Core 3.0 |      43.12 ns |     0.454 ns |     0.425 ns | 0.000 |       - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |  95,870.55 ns |   350.006 ns |   310.271 ns | 0.307 |  0.2441 |       - |     - |    1088 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method | NumberOfCharacters |        Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |------------------- |------------:|----------:|----------:|------:|-------:|------:|------:|----------:|
|      Baseline |                  5 |   312.55 ns |  2.342 ns |  2.190 ns |  1.00 | 0.1707 |     - |     - |     536 B |
| Quickenshtein |                  5 |    24.17 ns |  0.208 ns |  0.195 ns |  0.08 |      - |     - |     - |         - |
|  Fastenshtein |                  5 |    63.43 ns |  1.606 ns |  1.502 ns |  0.20 | 0.0153 |     - |     - |      48 B |
|               |                    |             |           |           |       |        |       |       |           |
|      Baseline |                 20 | 2,506.03 ns | 22.981 ns | 21.496 ns |  1.00 | 0.8659 |     - |     - |    2720 B |
| Quickenshtein |                 20 |    41.30 ns |  0.455 ns |  0.425 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein |                 20 | 1,015.29 ns | 13.311 ns | 12.451 ns |  0.41 | 0.0324 |     - |     - |     104 B |
|               |                    |             |           |           |       |        |       |       |           |
|      Baseline |                 40 | 7,375.61 ns | 56.352 ns | 52.712 ns | 1.000 | 2.6779 |     - |     - |    8400 B |
| Quickenshtein |                 40 |    62.22 ns |  0.389 ns |  0.364 ns | 0.008 |      - |     - |     - |         - |
|  Fastenshtein |                 40 | 3,773.44 ns | 39.405 ns | 34.931 ns | 0.511 | 0.0534 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|        Method |       Runtime | NumberOfCharacters |             Mean |          Error |          StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |-----------------:|---------------:|----------------:|------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |               8192 |        13.890 us |      0.1051 us |       0.0983 us |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 |   144,457.520 us |    766.1778 us |     716.6832 us |     - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |         8.307 us |      0.0496 us |       0.0464 us |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 |   142,265.863 us |  1,432.1924 us |   1,339.6736 us |     - |     - |     - |   33126 B |
| Quickenshtein |    .NET 4.7.2 |              32768 |        55.567 us |      0.5462 us |       0.4561 us |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |              32768 | 2,272,544.636 us | 21,296.0112 us |  18,878.3592 us |     - |     - |     - |  131096 B |
| Quickenshtein | .NET Core 3.0 |              32768 |        33.253 us |      0.2050 us |       0.1917 us |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |              32768 | 2,281,452.903 us | 59,541.4504 us | 171,790.6984 us |     - |     - |     - |  131096 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |       Runtime | NumberOfCharacters |             Mean |            Error |           StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |-----------------:|-----------------:|-----------------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |                 40 |         62.77 ns |         0.431 ns |         0.403 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 40 |      2,036.32 ns |        27.206 ns |        25.448 ns | 0.0572 |     - |     - |     185 B |
| Quickenshtein | .NET Core 3.0 |                 40 |         31.40 ns |         0.367 ns |         0.343 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 40 |      2,470.88 ns |        16.798 ns |        15.713 ns | 0.0572 |     - |     - |     184 B |
| Quickenshtein |    .NET 4.7.2 |               8192 |      8,983.72 ns |        86.651 ns |        81.053 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 | 69,341,373.33 ns | 1,306,751.333 ns | 1,222,336.007 ns |      - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |        637.02 ns |         6.822 ns |         6.382 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 | 99,598,365.33 ns | 1,053,841.459 ns |   985,763.955 ns |      - |     - |     - |   33040 B |