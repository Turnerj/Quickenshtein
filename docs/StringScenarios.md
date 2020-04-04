# String Scenarios

These benchmarks are designed to test specific aspects of the calculation, avoiding specific optimizations.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]     : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Job-FOAFUS : .NET Framework 4.8 (4.8.4150.0), X64 RyuJIT
  Job-BAOOIO : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

## Edge Difference Benchmark

This benchmark eliminates bonuses for trimming equal characters at the start and end.
The strings the same length and, besides the edges, are only filled with matching characters.
This then forces the fastest path internally (least operations) for calculating distance.

|        Method |       Runtime |      Mean |    Error |   StdDev | Ratio | Speedup |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------:|---------:|---------:|------:|--------:|--------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 | 313.30 us | 2.425 us | 2.025 us |  1.00 |    1.00 | 73.2422 | 19.0430 |     - |  301044 B |
| Quickenshtein |    .NET 4.7.2 |  42.00 us | 0.504 us | 0.472 us |  0.13 |    7.45 |       - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | 108.55 us | 1.024 us | 0.958 us |  0.35 |    2.88 |  0.2441 |       - |     - |    1092 B |
|      Baseline | .NET Core 3.0 | 339.09 us | 4.349 us | 4.068 us |  1.08 |    0.92 | 64.9414 | 23.4375 |     - |  291737 B |
| Quickenshtein | .NET Core 3.0 |  47.92 us | 0.299 us | 0.279 us |  0.15 |    6.54 |       - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | 102.03 us | 0.593 us | 0.555 us |  0.33 |    3.07 |  0.2441 |       - |     - |    1088 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method |       Runtime | NumberOfCharacters |        Mean |      Error |     StdDev | Ratio | Speedup | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |------------:|-----------:|-----------:|------:|--------:|--------:|-------:|------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |                  5 |   620.63 ns |   8.269 ns |   7.330 ns |  1.00 |    1.00 |    0.00 | 0.2241 |     - |     - |     706 B |
| Quickenshtein |    .NET 4.7.2 |                  5 |   162.83 ns |   1.570 ns |   1.468 ns |  0.26 |    3.81 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                  5 |    68.35 ns |   0.633 ns |   0.592 ns |  0.11 |    9.08 |    0.00 | 0.0153 |     - |     - |      48 B |
|      Baseline | .NET Core 3.0 |                  5 |   328.11 ns |   3.367 ns |   3.150 ns |  0.53 |    1.89 |    0.01 | 0.1707 |     - |     - |     536 B |
| Quickenshtein | .NET Core 3.0 |                  5 |   116.66 ns |   0.441 ns |   0.368 ns |  0.19 |    5.32 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                  5 |    66.70 ns |   0.755 ns |   0.706 ns |  0.11 |    9.30 |    0.00 | 0.0153 |     - |     - |      48 B |
|               |               |                    |             |            |            |       |         |         |        |       |       |           |
|      Baseline |    .NET 4.7.2 |                 20 | 3,462.43 ns |  25.824 ns |  22.893 ns |  1.00 |    1.00 |    0.00 | 1.0567 |     - |     - |    3330 B |
| Quickenshtein |    .NET 4.7.2 |                 20 |   844.94 ns |   5.410 ns |   5.061 ns |  0.24 |    4.10 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 20 | 1,097.31 ns |   9.042 ns |   8.457 ns |  0.32 |    3.16 |    0.00 | 0.0324 |     - |     - |     104 B |
|      Baseline | .NET Core 3.0 |                 20 | 2,570.55 ns |  14.528 ns |  13.589 ns |  0.74 |    1.35 |    0.01 | 0.8659 |     - |     - |    2720 B |
| Quickenshtein | .NET Core 3.0 |                 20 |   511.33 ns |   4.059 ns |   3.797 ns |  0.15 |    6.77 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 20 | 1,062.36 ns |  10.671 ns |   9.982 ns |  0.31 |    3.26 |    0.00 | 0.0324 |     - |     - |     104 B |
|               |               |                    |             |            |            |       |         |         |        |       |       |           |
|      Baseline |    .NET 4.7.2 |                 40 | 9,250.09 ns | 115.539 ns | 108.075 ns |  1.00 |    1.00 |    0.00 | 3.0365 |     - |     - |    9564 B |
| Quickenshtein |    .NET 4.7.2 |                 40 | 2,918.38 ns |  20.384 ns |  17.022 ns |  0.32 |    3.17 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 40 | 3,939.97 ns |  32.757 ns |  30.641 ns |  0.43 |    2.35 |    0.01 | 0.0534 |     - |     - |     185 B |
|      Baseline | .NET Core 3.0 |                 40 | 7,906.92 ns | 136.467 ns | 127.651 ns |  0.85 |    1.17 |    0.02 | 2.6703 |     - |     - |    8400 B |
| Quickenshtein | .NET Core 3.0 |                 40 | 1,388.36 ns |  11.077 ns |   9.820 ns |  0.15 |    6.66 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 40 | 3,986.00 ns |  24.052 ns |  22.498 ns |  0.43 |    2.32 |    0.01 | 0.0534 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|                      Method |       Runtime | NumberOfCharacters |        Mean |     Error |    StdDev | Ratio | Speedup | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |-------------- |------------------- |------------:|----------:|----------:|------:|--------:|--------:|------:|------:|------:|----------:|
|   Quickenshtein_NoThreading |    .NET 4.7.2 |               8192 |    90.79 ms |  0.626 ms |  0.555 ms |  1.00 |    1.00 |    0.00 |     - |     - |     - |         - |
|   Quickenshtein_NoThreading | .NET Core 3.0 |               8192 |    47.29 ms |  0.339 ms |  0.317 ms |  0.52 |    1.92 |    0.01 |     - |     - |     - |         - |
|                             |               |                    |             |           |           |       |         |         |       |       |       |           |
| Quickenshtein_WithThreading |    .NET 4.7.2 |               8192 |    33.44 ms |  0.666 ms |  0.865 ms |  1.00 |    1.00 |    0.00 |     - |     - |     - |    1024 B |
| Quickenshtein_WithThreading | .NET Core 3.0 |               8192 |    24.92 ms |  0.329 ms |  0.292 ms |  0.75 |    1.33 |    0.02 |     - |     - |     - |     800 B |
|                             |               |                    |             |           |           |       |         |         |       |       |       |           |
|                Fastenshtein |    .NET 4.7.2 |               8192 |   140.59 ms |  2.275 ms |  2.128 ms |  1.00 |    1.00 |    0.00 |     - |     - |     - |   32816 B |
|                Fastenshtein | .NET Core 3.0 |               8192 |   143.57 ms |  0.955 ms |  0.893 ms |  1.02 |    0.98 |    0.01 |     - |     - |     - |   32792 B |
|                             |               |                    |             |           |           |       |         |         |       |       |       |           |
|   Quickenshtein_NoThreading |    .NET 4.7.2 |              32768 | 1,448.65 ms |  7.158 ms |  6.695 ms |  1.00 |    1.00 |    0.00 |     - |     - |     - |         - |
|   Quickenshtein_NoThreading | .NET Core 3.0 |              32768 |   755.54 ms |  3.855 ms |  3.418 ms |  0.52 |    1.92 |    0.00 |     - |     - |     - |         - |
|                             |               |                    |             |           |           |       |         |         |       |       |       |           |
| Quickenshtein_WithThreading |    .NET 4.7.2 |              32768 |   501.19 ms |  4.422 ms |  3.693 ms |  1.00 |    1.00 |    0.00 |     - |     - |     - |    8192 B |
| Quickenshtein_WithThreading | .NET Core 3.0 |              32768 |   387.47 ms |  6.540 ms |  5.798 ms |  0.77 |    1.29 |    0.01 |     - |     - |     - |     800 B |
|                             |               |                    |             |           |           |       |         |         |       |       |       |           |
|                Fastenshtein |    .NET 4.7.2 |              32768 | 2,185.64 ms | 53.292 ms | 61.372 ms |  1.00 |    1.00 |    0.00 |     - |     - |     - |  131096 B |
|                Fastenshtein | .NET Core 3.0 |              32768 | 2,286.02 ms |  8.631 ms |  7.651 ms |  1.05 |    0.95 |    0.02 |     - |     - |     - |  131096 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |       Runtime | NumberOfCharacters |             Mean |         Error |        StdDev | Ratio | Speedup |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |-----------------:|--------------:|--------------:|------:|--------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |                 40 |         196.0 ns |       1.82 ns |       1.71 ns |  1.00 |    1.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |                 40 |         107.6 ns |       0.74 ns |       0.66 ns |  0.55 |    1.82 |      - |     - |     - |         - |
|               |               |                    |                  |               |               |       |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |                 40 |       2,651.4 ns |      16.77 ns |      15.69 ns |  1.00 |    1.00 | 0.0572 |     - |     - |     185 B |
|  Fastenshtein | .NET Core 3.0 |                 40 |       3,087.4 ns |      24.00 ns |      22.45 ns |  1.16 |    0.86 | 0.0572 |     - |     - |     184 B |
|               |               |                    |                  |               |               |       |         |        |       |       |           |
| Quickenshtein |    .NET 4.7.2 |               8192 |       8,866.9 ns |     100.80 ns |      94.29 ns |  1.00 |    1.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |               8192 |         822.9 ns |       5.20 ns |       4.86 ns |  0.09 |   10.78 |      - |     - |     - |         - |
|               |               |                    |                  |               |               |       |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |               8192 |  94,875,423.8 ns | 522,395.36 ns | 463,089.88 ns |  1.00 |    1.00 |      - |     - |     - |   32816 B |
|  Fastenshtein | .NET Core 3.0 |               8192 | 106,396,420.0 ns | 800,954.50 ns | 749,213.33 ns |  1.12 |    0.89 |      - |     - |     - |   33758 B |

## Empty Text Benchmark

This benchmark shows how well the calculator can optimise an empty string in either the source or target.

|        Method |       Runtime | IsSourceEmpty |       Mean |     Error |    StdDev | Ratio | Speedup | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |-------------- |-----------:|----------:|----------:|------:|--------:|--------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |         False |  14.579 ns | 0.0907 ns | 0.0848 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |         False |  14.508 ns | 0.1007 ns | 0.0942 ns |  1.00 |    1.00 |    0.01 |      - |     - |     - |         - |
|               |               |               |            |           |           |       |         |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |         False |   2.441 ns | 0.0307 ns | 0.0287 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |         False |   2.470 ns | 0.0571 ns | 0.0534 ns |  1.01 |    0.99 |    0.03 |      - |     - |     - |         - |
|               |               |               |            |           |           |       |         |         |        |       |       |           |
| Quickenshtein |    .NET 4.7.2 |          True |  11.904 ns | 0.0746 ns | 0.0661 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |          True |  14.431 ns | 0.1308 ns | 0.1224 ns |  1.21 |    0.82 |    0.01 |      - |     - |     - |         - |
|               |               |               |            |           |           |       |         |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |          True | 404.780 ns | 2.5827 ns | 2.4158 ns |  1.00 |    1.00 |    0.00 | 0.5174 |     - |     - |    1629 B |
|  Fastenshtein | .NET Core 3.0 |          True | 370.214 ns | 4.0625 ns | 3.8001 ns |  0.91 |    1.09 |    0.01 | 0.5174 |     - |     - |    1624 B |