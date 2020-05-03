# String Scenarios

These benchmarks are designed to test specific aspects of the calculation, avoiding specific optimizations.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]     : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Job-AQLRZV : .NET Framework 4.8 (4.8.4150.0), X64 RyuJIT
  Job-IVETVG : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

## Edge Difference Benchmark

This benchmark eliminates bonuses for trimming equal characters at the start and end.
The strings the same length and, besides the edges, are only filled with matching characters.
This then forces the fastest path internally (least operations) for calculating distance.

|        Method |        Job |       Runtime |      Mean |    Error |   StdDev | Ratio | Speedup | Worthiness | Code Size |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |----------:|---------:|---------:|------:|--------:|-----------:|----------:|--------:|--------:|------:|----------:|
|      Baseline | Job-AQLRZV |    .NET 4.7.2 | 307.79 us | 2.245 us | 2.100 us |  1.00 |    1.00 |       1.00 |    2557 B | 74.7070 | 18.5547 |     - |  301049 B |
| Quickenshtein | Job-AQLRZV |    .NET 4.7.2 |  32.21 us | 0.281 us | 0.263 us |  0.10 |    9.55 |       6.62 |    3689 B |       - |       - |     - |         - |
|  Fastenshtein | Job-AQLRZV |    .NET 4.7.2 | 138.29 us | 1.198 us | 1.121 us |  0.45 |    2.23 |      17.35 |     328 B |  0.2441 |       - |     - |    1092 B |
|      Baseline | Job-IVETVG | .NET Core 3.0 | 319.10 us | 2.959 us | 2.767 us |  1.04 |    0.96 |       1.27 |    1938 B | 65.4297 | 22.4609 |     - |  291740 B |
| Quickenshtein | Job-IVETVG | .NET Core 3.0 |  34.84 us | 0.330 us | 0.309 us |  0.11 |    8.84 |       6.57 |    3439 B |       - |       - |     - |         - |
|  Fastenshtein | Job-IVETVG | .NET Core 3.0 | 119.47 us | 0.560 us | 0.467 us |  0.39 |    2.58 |      20.02 |     329 B |  0.2441 |       - |     - |    1088 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method |        Job |       Runtime | NumberOfCharacters |         Mean |     Error |    StdDev | Ratio | Speedup | Worthiness | RatioSD | Code Size |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |------------------- |-------------:|----------:|----------:|------:|--------:|-----------:|--------:|----------:|-------:|------:|------:|----------:|
|      Baseline | Job-AQLRZV |    .NET 4.7.2 |                  5 |    603.10 ns |  6.016 ns |  5.627 ns |  1.00 |    1.00 |       1.00 |    0.00 |    2557 B | 0.2241 |     - |     - |     706 B |
| Quickenshtein | Job-AQLRZV |    .NET 4.7.2 |                  5 |    113.43 ns |  1.060 ns |  0.991 ns |  0.19 |    5.32 |       3.69 |    0.00 |    3689 B |      - |     - |     - |         - |
|  Fastenshtein | Job-AQLRZV |    .NET 4.7.2 |                  5 |     74.20 ns |  0.871 ns |  0.727 ns |  0.12 |    8.13 |      63.36 |    0.00 |     328 B | 0.0153 |     - |     - |      48 B |
|      Baseline | Job-IVETVG | .NET Core 3.0 |                  5 |    300.44 ns |  1.715 ns |  1.604 ns |  0.50 |    2.01 |       2.65 |    0.01 |    1938 B | 0.1707 |     - |     - |     536 B |
| Quickenshtein | Job-IVETVG | .NET Core 3.0 |                  5 |    244.47 ns |  1.471 ns |  1.376 ns |  0.41 |    2.47 |       1.83 |    0.00 |    3439 B |      - |     - |     - |         - |
|  Fastenshtein | Job-IVETVG | .NET Core 3.0 |                  5 |     72.28 ns |  0.610 ns |  0.570 ns |  0.12 |    8.34 |      64.85 |    0.00 |     329 B | 0.0153 |     - |     - |      48 B |
|               |            |               |                    |              |           |           |       |         |            |         |           |        |       |       |           |
|      Baseline | Job-AQLRZV |    .NET 4.7.2 |                 20 |  3,860.01 ns | 57.192 ns | 53.497 ns |  1.00 |    1.00 |       1.00 |    0.00 |    2557 B | 1.0567 |     - |     - |    3330 B |
| Quickenshtein | Job-AQLRZV |    .NET 4.7.2 |                 20 |    774.49 ns |  8.767 ns |  7.320 ns |  0.20 |    4.98 |       3.45 |    0.00 |    3689 B |      - |     - |     - |         - |
|  Fastenshtein | Job-AQLRZV |    .NET 4.7.2 |                 20 |  1,200.80 ns |  7.456 ns |  6.226 ns |  0.31 |    3.21 |      25.02 |    0.00 |     328 B | 0.0324 |     - |     - |     104 B |
|      Baseline | Job-IVETVG | .NET Core 3.0 |                 20 |  2,720.06 ns | 36.904 ns | 34.520 ns |  0.70 |    1.42 |       1.87 |    0.02 |    1938 B | 0.8659 |     - |     - |    2720 B |
| Quickenshtein | Job-IVETVG | .NET Core 3.0 |                 20 |    687.89 ns | 11.898 ns | 11.130 ns |  0.18 |    5.61 |       4.17 |    0.00 |    3439 B |      - |     - |     - |         - |
|  Fastenshtein | Job-IVETVG | .NET Core 3.0 |                 20 |  1,183.44 ns | 17.080 ns | 15.977 ns |  0.31 |    3.26 |      25.35 |    0.00 |     329 B | 0.0324 |     - |     - |     104 B |
|               |            |               |                    |              |           |           |       |         |            |         |           |        |       |       |           |
|      Baseline | Job-AQLRZV |    .NET 4.7.2 |                 40 | 10,021.63 ns | 77.920 ns | 65.067 ns |  1.00 |    1.00 |       1.00 |    0.00 |    2557 B | 3.0365 |     - |     - |    9564 B |
| Quickenshtein | Job-AQLRZV |    .NET 4.7.2 |                 40 |  2,679.29 ns | 10.766 ns |  9.544 ns |  0.27 |    3.74 |       2.59 |    0.00 |    3689 B |      - |     - |     - |         - |
|  Fastenshtein | Job-AQLRZV |    .NET 4.7.2 |                 40 |  4,183.91 ns | 24.444 ns | 20.412 ns |  0.42 |    2.40 |      18.67 |    0.00 |     328 B | 0.0534 |     - |     - |     185 B |
|      Baseline | Job-IVETVG | .NET Core 3.0 |                 40 |  7,492.68 ns | 28.145 ns | 26.327 ns |  0.75 |    1.34 |       1.76 |    0.00 |    1938 B | 2.6779 |     - |     - |    8400 B |
| Quickenshtein | Job-IVETVG | .NET Core 3.0 |                 40 |  1,508.35 ns | 13.531 ns | 12.657 ns |  0.15 |    6.65 |       4.94 |    0.00 |    3439 B |      - |     - |     - |         - |
|  Fastenshtein | Job-IVETVG | .NET Core 3.0 |                 40 |  3,834.78 ns | 28.830 ns | 26.968 ns |  0.38 |    2.61 |      20.29 |    0.00 |     329 B | 0.0534 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|                      Method |        Job |       Runtime | NumberOfCharacters |        Mean |     Error |    StdDev | Ratio | Speedup | Worthiness | RatioSD | Code Size | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------- |-------------- |------------------- |------------:|----------:|----------:|------:|--------:|-----------:|--------:|----------:|------:|------:|------:|----------:|
|   Quickenshtein_NoThreading | Job-XUTCZT |    .NET 4.7.2 |               8192 |    89.91 ms |  0.586 ms |  0.548 ms |  1.00 |    1.00 |       1.00 |    0.00 |    3689 B |     - |     - |     - |         - |
|   Quickenshtein_NoThreading | Job-DRGSBT | .NET Core 3.0 |               8192 |    27.04 ms |  0.140 ms |  0.124 ms |  0.30 |    3.33 |       3.57 |    0.00 |    3439 B |     - |     - |     - |         - |
|                             |            |               |                    |             |           |           |       |         |            |         |           |       |       |       |           |
| Quickenshtein_WithThreading | Job-XUTCZT |    .NET 4.7.2 |               8192 |    28.42 ms |  0.555 ms |  0.545 ms |  1.00 |    1.00 |       1.00 |    0.00 |    5029 B |     - |     - |     - |    1024 B |
| Quickenshtein_WithThreading | Job-DRGSBT | .NET Core 3.0 |               8192 |    17.96 ms |  0.286 ms |  0.294 ms |  0.63 |    1.58 |       0.84 |    0.02 |    9514 B |     - |     - |     - |     800 B |
|                             |            |               |                    |             |           |           |       |         |            |         |           |       |       |       |           |
|                Fastenshtein | Job-XUTCZT |    .NET 4.7.2 |               8192 |   161.42 ms |  1.479 ms |  1.383 ms |  1.00 |    1.00 |       1.00 |    0.00 |     328 B |     - |     - |     - |   32816 B |
|                Fastenshtein | Job-DRGSBT | .NET Core 3.0 |               8192 |   148.11 ms |  2.213 ms |  2.070 ms |  0.92 |    1.09 |       1.09 |    0.02 |     329 B |     - |     - |     - |   32792 B |
|                             |            |               |                    |             |           |           |       |         |            |         |           |       |       |       |           |
|   Quickenshtein_NoThreading | Job-XUTCZT |    .NET 4.7.2 |              32768 | 1,427.36 ms |  3.808 ms |  3.375 ms |  1.00 |    1.00 |       1.00 |    0.00 |    3689 B |     - |     - |     - |         - |
|   Quickenshtein_NoThreading | Job-DRGSBT | .NET Core 3.0 |              32768 |   450.70 ms |  4.090 ms |  3.626 ms |  0.32 |    3.17 |       6.36 |    0.00 |    1836 B |     - |     - |     - |    1344 B |
|                             |            |               |                    |             |           |           |       |         |            |         |           |       |       |       |           |
| Quickenshtein_WithThreading | Job-XUTCZT |    .NET 4.7.2 |              32768 |   412.56 ms |  4.516 ms |  3.526 ms |  1.00 |    1.00 |       1.00 |    0.00 |    5029 B |     - |     - |     - |    8192 B |
| Quickenshtein_WithThreading | Job-DRGSBT | .NET Core 3.0 |              32768 |   274.29 ms |  1.931 ms |  1.613 ms |  0.66 |    1.50 |       0.80 |    0.00 |    9480 B |     - |     - |     - |     800 B |
|                             |            |               |                    |             |           |           |       |         |            |         |           |       |       |       |           |
|                Fastenshtein | Job-XUTCZT |    .NET 4.7.2 |              32768 | 2,575.96 ms |  9.110 ms |  8.521 ms |  1.00 |    1.00 |       1.00 |    0.00 |     328 B |     - |     - |     - |  131096 B |
|                Fastenshtein | Job-DRGSBT | .NET Core 3.0 |              32768 | 2,359.66 ms | 22.966 ms | 21.483 ms |  0.92 |    1.09 |       1.05 |    0.01 |     340 B |     - |     - |     - |  131096 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |        Job |       Runtime | NumberOfCharacters |             Mean |           Error |          StdDev | Ratio | Speedup | Worthiness | RatioSD | Code Size |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |------------------- |-----------------:|----------------:|----------------:|------:|--------:|-----------:|--------:|----------:|-------:|------:|------:|----------:|
| Quickenshtein | Job-YKPFGX |    .NET 4.7.2 |                 40 |         128.3 ns |         3.02 ns |         8.91 ns |  1.00 |    1.00 |       1.00 |    0.00 |    3689 B |      - |     - |     - |         - |
| Quickenshtein | Job-OFHMSW | .NET Core 3.0 |                 40 |         187.9 ns |         1.08 ns |         1.01 ns |  1.38 |    0.73 |       0.78 |    0.10 |    3439 B |      - |     - |     - |         - |
|               |            |               |                    |                  |                 |                 |       |         |            |         |           |        |       |       |           |
|  Fastenshtein | Job-YKPFGX |    .NET 4.7.2 |                 40 |       2,934.3 ns |        21.09 ns |        19.73 ns |  1.00 |    1.00 |       1.00 |    0.00 |     328 B | 0.0572 |     - |     - |     185 B |
|  Fastenshtein | Job-OFHMSW | .NET Core 3.0 |                 40 |       3,329.6 ns |        16.07 ns |        15.03 ns |  1.13 |    0.88 |       0.88 |    0.01 |     329 B | 0.0572 |     - |     - |     184 B |
|               |            |               |                    |                  |                 |                 |       |         |            |         |           |        |       |       |           |
| Quickenshtein | Job-YKPFGX |    .NET 4.7.2 |               8192 |       7,222.4 ns |        45.59 ns |        35.59 ns |  1.00 |    1.00 |       1.00 |    0.00 |    3689 B |      - |     - |     - |         - |
| Quickenshtein | Job-OFHMSW | .NET Core 3.0 |               8192 |         816.0 ns |         6.44 ns |         6.03 ns |  0.11 |    8.86 |       9.50 |    0.00 |    3439 B |      - |     - |     - |         - |
|               |            |               |                    |                  |                 |                 |       |         |            |         |           |        |       |       |           |
|  Fastenshtein | Job-YKPFGX |    .NET 4.7.2 |               8192 | 103,060,980.0 ns | 1,186,548.01 ns | 1,109,897.74 ns |  1.00 |    1.00 |       1.00 |    0.00 |     328 B |      - |     - |     - |   32816 B |
|  Fastenshtein | Job-OFHMSW | .NET Core 3.0 |               8192 | 102,817,096.0 ns |   602,997.70 ns |   564,044.43 ns |  1.00 |    1.00 |       1.00 |    0.01 |     329 B |      - |     - |     - |   33059 B |

## Empty Text Benchmark

This benchmark shows how well the calculator can optimise an empty string in either the source or target.

|        Method |        Job |       Runtime | IsSourceEmpty |       Mean |     Error |    StdDev | Ratio | Speedup | Worthiness | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated | Code Size |
|-------------- |----------- |-------------- |-------------- |-----------:|----------:|----------:|------:|--------:|-----------:|--------:|-------:|------:|------:|----------:|----------:|
| Quickenshtein | Job-YKPFGX |    .NET 4.7.2 |         False |   2.833 ns | 0.0377 ns | 0.0334 ns |  1.00 |    1.00 |       1.00 |    0.00 |      - |     - |     - |         - |     199 B |
| Quickenshtein | Job-OFHMSW | .NET Core 3.0 |         False |   4.008 ns | 0.0235 ns | 0.0196 ns |  1.41 |    0.71 |       0.55 |    0.02 |      - |     - |     - |         - |     257 B |
|               |            |               |               |            |           |           |       |         |            |         |        |       |       |           |           |
|  Fastenshtein | Job-YKPFGX |    .NET 4.7.2 |         False |   2.712 ns | 0.0380 ns | 0.0337 ns |  1.00 |    1.00 |       1.00 |    0.00 |      - |     - |     - |         - |     328 B |
|  Fastenshtein | Job-OFHMSW | .NET Core 3.0 |         False |   2.283 ns | 0.0230 ns | 0.0204 ns |  0.84 |    1.19 |       1.18 |    0.01 |      - |     - |     - |         - |     329 B |
|               |            |               |               |            |           |           |       |         |            |         |        |       |       |           |           |
| Quickenshtein | Job-YKPFGX |    .NET 4.7.2 |          True |   2.568 ns | 0.0399 ns | 0.0373 ns |  1.00 |    1.00 |       1.00 |    0.00 |      - |     - |     - |         - |     199 B |
| Quickenshtein | Job-OFHMSW | .NET Core 3.0 |          True |   3.474 ns | 0.0365 ns | 0.0341 ns |  1.35 |    0.74 |       0.57 |    0.02 |      - |     - |     - |         - |     257 B |
|               |            |               |               |            |           |           |       |         |            |         |        |       |       |           |           |
|  Fastenshtein | Job-YKPFGX |    .NET 4.7.2 |          True | 357.676 ns | 2.8656 ns | 2.6805 ns |  1.00 |    1.00 |       1.00 |    0.00 | 0.5174 |     - |     - |    1629 B |     328 B |
|  Fastenshtein | Job-OFHMSW | .NET Core 3.0 |          True | 426.236 ns | 3.5513 ns | 3.3219 ns |  1.19 |    0.84 |       0.84 |    0.01 | 0.5174 |     - |     - |    1624 B |     329 B |