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

|        Method |       Runtime |      Mean |    Error |   StdDev | Ratio |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------:|---------:|---------:|------:|--------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 | 323.46 us | 2.087 us | 1.952 us |  1.00 | 74.2188 | 20.9961 |     - |  301049 B |
| Quickenshtein |    .NET 4.7.2 |  83.67 us | 0.452 us | 0.422 us |  0.26 |       - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |  79.60 us | 0.575 us | 0.538 us |  0.25 |  0.2441 |       - |     - |    1092 B |
|               |               |           |          |          |       |         |         |       |           |
|      Baseline | .NET Core 3.0 | 334.81 us | 4.696 us | 4.392 us |  1.00 | 66.8945 | 20.5078 |     - |  291738 B |
| Quickenshtein | .NET Core 3.0 |  90.28 us | 0.519 us | 0.485 us |  0.27 |       - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | 105.87 us | 0.524 us | 0.438 us |  0.32 |  0.2441 |       - |     - |    1089 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method | NumberOfCharacters |        Mean |      Error |     StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |------------------- |------------:|-----------:|-----------:|------:|--------:|-------:|------:|------:|----------:|
|      Baseline |                  5 |   323.53 ns |   3.233 ns |   2.866 ns |  1.00 |    0.00 | 0.1707 |     - |     - |     536 B |
| Quickenshtein |                  5 |   107.14 ns |   0.943 ns |   0.882 ns |  0.33 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein |                  5 |    65.76 ns |   0.554 ns |   0.491 ns |  0.20 |    0.00 | 0.0153 |     - |     - |      48 B |
|               |                    |             |            |            |       |         |        |       |       |           |
|      Baseline |                 20 | 2,535.09 ns |  13.696 ns |  12.811 ns |  1.00 |    0.00 | 0.8659 |     - |     - |    2720 B |
| Quickenshtein |                 20 |   718.54 ns |   3.816 ns |   3.382 ns |  0.28 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein |                 20 | 1,038.10 ns |   5.104 ns |   4.775 ns |  0.41 |    0.00 | 0.0324 |     - |     - |     104 B |
|               |                    |             |            |            |       |         |        |       |       |           |
|      Baseline |                 40 | 7,928.18 ns | 329.631 ns | 338.507 ns |  1.00 |    0.00 | 2.6703 |     - |     - |    8400 B |
| Quickenshtein |                 40 | 2,565.35 ns |  24.465 ns |  22.885 ns |  0.32 |    0.01 |      - |     - |     - |         - |
|  Fastenshtein |                 40 | 3,947.53 ns |  20.538 ns |  17.150 ns |  0.50 |    0.02 | 0.0534 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|        Method |       Runtime | NumberOfCharacters |       Mean |    Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |-----------:|---------:|---------:|------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |               8192 |   128.4 ms |  1.23 ms |  1.03 ms |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 |   150.6 ms |  1.15 ms |  1.08 ms |     - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |   101.4 ms |  0.88 ms |  0.83 ms |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 |   142.7 ms |  2.77 ms |  3.40 ms |     - |     - |     - |   32792 B |
| Quickenshtein |    .NET 4.7.2 |              32768 | 2,048.8 ms |  7.53 ms |  7.04 ms |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |              32768 | 2,399.1 ms | 22.28 ms | 19.75 ms |     - |     - |     - |  131096 B |
| Quickenshtein | .NET Core 3.0 |              32768 | 1,660.3 ms |  8.26 ms |  7.72 ms |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |              32768 | 2,581.2 ms | 50.50 ms | 89.77 ms |     - |     - |     - |  131192 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |       Runtime | NumberOfCharacters |             Mean |          Error |         StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |-----------------:|---------------:|---------------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |                 40 |        152.51 ns |       0.879 ns |       0.822 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 40 |      2,144.45 ns |      15.712 ns |      14.697 ns | 0.0572 |     - |     - |     185 B |
| Quickenshtein | .NET Core 3.0 |                 40 |         78.27 ns |       0.331 ns |       0.294 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 40 |      3,024.42 ns |      16.364 ns |      15.307 ns | 0.0572 |     - |     - |     184 B |
| Quickenshtein |    .NET 4.7.2 |               8192 |      9,373.59 ns |      71.471 ns |      66.854 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 | 71,853,440.00 ns | 590,087.576 ns | 551,968.285 ns |      - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |        749.65 ns |       6.100 ns |       5.407 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 | 93,414,675.56 ns | 764,175.373 ns | 714,810.118 ns |      - |     - |     - |   33015 B |