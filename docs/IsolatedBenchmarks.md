# Isolated Benchmarks

These benchmarks are designed to test specific aspects of the calculation, avoiding specific optimizations.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4121.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4121.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

## Edge Difference Benchmark

This benchmark eliminates bonuses for trimming equal characters at the start and end.
The strings the same length and, besides the edges, are only filled with matching characters.
This then forces the fastest path internally (least operations) for calculating distance.

|        Method |       Runtime |      Mean |     Error |    StdDev |    Median | Ratio | RatioSD |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------:|----------:|----------:|----------:|------:|--------:|--------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 | 322.39 us |  1.889 us |  1.767 us | 322.10 us |  1.00 |    0.00 | 74.2188 | 20.9961 |     - |  301049 B |
| Quickenshtein |    .NET 4.7.2 |  39.84 us |  0.561 us |  0.498 us |  39.61 us |  0.12 |    0.00 |       - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |  78.19 us |  0.487 us |  0.432 us |  78.08 us |  0.24 |    0.00 |  0.2441 |       - |     - |    1092 B |
|               |               |           |           |           |           |       |         |         |         |       |           |
|      Baseline | .NET Core 3.0 | 363.86 us | 10.378 us | 30.599 us | 354.01 us |  1.00 |    0.00 | 64.4531 | 23.4375 |     - |  291736 B |
| Quickenshtein | .NET Core 3.0 |  48.82 us |  1.059 us |  2.091 us |  48.23 us |  0.13 |    0.02 |       - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | 101.55 us |  0.959 us |  0.898 us | 101.40 us |  0.30 |    0.03 |  0.2441 |       - |     - |    1089 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method | NumberOfCharacters |        Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |------------------- |------------:|----------:|----------:|------:|-------:|------:|------:|----------:|
|      Baseline |                  5 |   324.35 ns |  2.897 ns |  2.709 ns |  1.00 | 0.1707 |     - |     - |     536 B |
| Quickenshtein |                  5 |    99.95 ns |  0.487 ns |  0.455 ns |  0.31 |      - |     - |     - |         - |
|  Fastenshtein |                  5 |    74.17 ns |  0.525 ns |  0.491 ns |  0.23 | 0.0153 |     - |     - |      48 B |
|               |                    |             |           |           |       |        |       |       |           |
|      Baseline |                 20 | 2,581.01 ns | 14.953 ns | 13.987 ns |  1.00 | 0.8659 |     - |     - |    2720 B |
| Quickenshtein |                 20 |   425.37 ns |  3.062 ns |  2.557 ns |  0.16 |      - |     - |     - |         - |
|  Fastenshtein |                 20 | 1,094.60 ns |  3.844 ns |  3.408 ns |  0.42 | 0.0324 |     - |     - |     104 B |
|               |                    |             |           |           |       |        |       |       |           |
|      Baseline |                 40 | 7,501.47 ns | 28.716 ns | 26.861 ns |  1.00 | 2.6779 |     - |     - |    8400 B |
| Quickenshtein |                 40 | 1,448.47 ns |  8.915 ns |  8.339 ns |  0.19 |      - |     - |     - |         - |
|  Fastenshtein |                 40 | 3,709.47 ns | 13.481 ns | 11.951 ns |  0.49 | 0.0572 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|        Method |       Runtime | NumberOfCharacters |        Mean |     Error |    StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |------------:|----------:|----------:|------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |               8192 |    96.65 ms |  0.745 ms |  0.622 ms |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 |   149.42 ms |  2.786 ms |  2.861 ms |     - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |    55.97 ms |  0.276 ms |  0.258 ms |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 |   141.94 ms |  2.308 ms |  2.159 ms |     - |     - |     - |   33126 B |
| Quickenshtein |    .NET 4.7.2 |              32768 | 1,541.15 ms |  7.157 ms |  6.344 ms |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |              32768 | 2,338.83 ms |  9.771 ms |  9.140 ms |     - |     - |     - |  131096 B |
| Quickenshtein | .NET Core 3.0 |              32768 |   896.43 ms |  5.037 ms |  4.712 ms |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |              32768 | 2,198.06 ms | 43.731 ms | 59.860 ms |     - |     - |     - |  131096 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |       Runtime | NumberOfCharacters |              Mean |            Error |         StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |------------------:|-----------------:|---------------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |                 40 |         211.08 ns |         1.166 ns |       1.090 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 40 |       2,105.47 ns |        23.972 ns |      22.423 ns | 0.0572 |     - |     - |     185 B |
| Quickenshtein | .NET Core 3.0 |                 40 |          83.96 ns |         0.340 ns |       0.301 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 40 |       2,541.54 ns |        12.051 ns |      10.683 ns | 0.0572 |     - |     - |     184 B |
| Quickenshtein |    .NET 4.7.2 |               8192 |       8,533.92 ns |        43.783 ns |      38.813 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 |  70,494,914.29 ns |   576,015.375 ns | 510,622.627 ns |      - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |         752.24 ns |         3.986 ns |       3.728 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 | 102,957,977.33 ns | 1,062,307.345 ns | 993,682.950 ns |      - |     - |     - |   33040 B |