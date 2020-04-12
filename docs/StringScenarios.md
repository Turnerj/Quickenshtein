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

|        Method |        Job |       Runtime |      Mean |    Error |   StdDev | Ratio | Speedup | Worthiness |   Gen 0 |   Gen 1 | Gen 2 | Allocated | Code Size |
|-------------- |----------- |-------------- |----------:|---------:|---------:|------:|--------:|-----------:|--------:|--------:|------:|----------:|----------:|
|      Baseline | Job-SRIUUF |    .NET 4.7.2 | 321.68 us | 2.589 us | 2.422 us |  1.00 |    1.00 |       1.00 | 74.7070 | 18.5547 |     - |  301049 B |     616 B |
| Quickenshtein | Job-SRIUUF |    .NET 4.7.2 |  42.67 us | 0.385 us | 0.341 us |  0.13 |    7.54 |      23.35 |       - |       - |     - |         - |     199 B |
|  Fastenshtein | Job-SRIUUF |    .NET 4.7.2 | 144.21 us | 0.743 us | 0.695 us |  0.45 |    2.23 |       4.19 |  0.2441 |       - |     - |    1092 B |     328 B |
|      Baseline | Job-SWNBOM | .NET Core 3.0 | 338.61 us | 3.291 us | 2.917 us |  1.05 |    0.95 |       0.96 | 65.4297 | 22.4609 |     - |  291740 B |     608 B |
| Quickenshtein | Job-SWNBOM | .NET Core 3.0 |  42.83 us | 0.210 us | 0.196 us |  0.13 |    7.51 |      18.00 |       - |       - |     - |         - |     257 B |
|  Fastenshtein | Job-SWNBOM | .NET Core 3.0 | 126.18 us | 1.275 us | 1.130 us |  0.39 |    2.55 |       4.78 |  0.2441 |       - |     - |    1088 B |     329 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method |        Job |       Runtime | NumberOfCharacters |         Mean |      Error |     StdDev | Ratio | Speedup | Worthiness | Code Size |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |------------------- |-------------:|-----------:|-----------:|------:|--------:|-----------:|----------:|-------:|------:|------:|----------:|
|      Baseline | Job-QRNJHQ |    .NET 4.7.2 |                  5 |    629.55 ns |   3.361 ns |   2.806 ns |  1.00 |    1.00 |       1.00 |     616 B | 0.2241 |     - |     - |     706 B |
| Quickenshtein | Job-QRNJHQ |    .NET 4.7.2 |                  5 |    120.87 ns |   0.713 ns |   0.667 ns |  0.19 |    5.21 |      16.12 |     199 B |      - |     - |     - |         - |
|  Fastenshtein | Job-QRNJHQ |    .NET 4.7.2 |                  5 |     77.64 ns |   0.385 ns |   0.360 ns |  0.12 |    8.11 |      15.23 |     328 B | 0.0153 |     - |     - |      48 B |
|      Baseline | Job-XRRJUN | .NET Core 3.0 |                  5 |    315.02 ns |   2.257 ns |   2.111 ns |  0.50 |    2.00 |       2.02 |     608 B | 0.1707 |     - |     - |     536 B |
| Quickenshtein | Job-XRRJUN | .NET Core 3.0 |                  5 |     93.66 ns |   0.580 ns |   0.484 ns |  0.15 |    6.72 |      16.11 |     257 B |      - |     - |     - |         - |
|  Fastenshtein | Job-XRRJUN | .NET Core 3.0 |                  5 |     66.55 ns |   0.625 ns |   0.585 ns |  0.11 |    9.45 |      17.70 |     329 B | 0.0153 |     - |     - |      48 B |
|               |            |               |                    |              |            |            |       |         |            |           |        |       |       |           |
|      Baseline | Job-QRNJHQ |    .NET 4.7.2 |                 20 |  3,732.32 ns |  29.000 ns |  27.126 ns |  1.00 |    1.00 |       1.00 |     616 B | 1.0567 |     - |     - |    3330 B |
| Quickenshtein | Job-QRNJHQ |    .NET 4.7.2 |                 20 |    762.64 ns |   3.769 ns |   3.526 ns |  0.20 |    4.89 |      15.15 |     199 B |      - |     - |     - |         - |
|  Fastenshtein | Job-QRNJHQ |    .NET 4.7.2 |                 20 |  1,192.41 ns |   7.679 ns |   6.807 ns |  0.32 |    3.13 |       5.88 |     328 B | 0.0324 |     - |     - |     104 B |
|      Baseline | Job-XRRJUN | .NET Core 3.0 |                 20 |  2,557.90 ns |  12.871 ns |  12.039 ns |  0.69 |    1.46 |       1.48 |     608 B | 0.8659 |     - |     - |    2720 B |
| Quickenshtein | Job-XRRJUN | .NET Core 3.0 |                 20 |    437.85 ns |   4.491 ns |   3.981 ns |  0.12 |    8.52 |      20.43 |     257 B |      - |     - |     - |         - |
|  Fastenshtein | Job-XRRJUN | .NET Core 3.0 |                 20 |  1,115.61 ns |   7.364 ns |   6.888 ns |  0.30 |    3.35 |       6.26 |     329 B | 0.0324 |     - |     - |     104 B |
|               |            |               |                    |              |            |            |       |         |            |           |        |       |       |           |
|      Baseline | Job-QRNJHQ |    .NET 4.7.2 |                 40 | 10,460.64 ns | 124.409 ns | 110.285 ns |  1.00 |    1.00 |       1.00 |     616 B | 3.0365 |     - |     - |    9564 B |
| Quickenshtein | Job-QRNJHQ |    .NET 4.7.2 |                 40 |  2,840.28 ns |  13.321 ns |  12.461 ns |  0.27 |    3.68 |      11.40 |     199 B |      - |     - |     - |         - |
|  Fastenshtein | Job-QRNJHQ |    .NET 4.7.2 |                 40 |  4,351.14 ns |  50.139 ns |  41.868 ns |  0.42 |    2.41 |       4.52 |     328 B | 0.0534 |     - |     - |     185 B |
|      Baseline | Job-XRRJUN | .NET Core 3.0 |                 40 |  7,833.54 ns |  51.969 ns |  48.611 ns |  0.75 |    1.34 |       1.35 |     608 B | 2.6703 |     - |     - |    8400 B |
| Quickenshtein | Job-XRRJUN | .NET Core 3.0 |                 40 |  1,281.15 ns |  13.969 ns |  11.665 ns |  0.12 |    8.17 |      19.59 |     257 B |      - |     - |     - |         - |
|  Fastenshtein | Job-XRRJUN | .NET Core 3.0 |                 40 |  4,153.60 ns |  29.673 ns |  27.756 ns |  0.40 |    2.52 |       4.71 |     329 B | 0.0534 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|                      Method |        Job |       Runtime | NumberOfCharacters |        Mean |     Error |    StdDev | Ratio | Speedup | Worthiness | Code Size | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |----------- |-------------- |------------------- |------------:|----------:|----------:|------:|--------:|-----------:|----------:|------:|------:|------:|----------:|
|   Quickenshtein_NoThreading | Job-NWUPYP |    .NET 4.7.2 |               8192 |    97.00 ms |  0.665 ms |  0.622 ms |  1.00 |    1.00 |       1.00 |     199 B |     - |     - |     - |         - |
|   Quickenshtein_NoThreading | Job-LAXDMC | .NET Core 3.0 |               8192 |    47.61 ms |  0.223 ms |  0.198 ms |  0.49 |    2.04 |       1.58 |     257 B |     - |     - |     - |         - |
|                             |            |               |                    |             |           |           |       |         |            |           |       |       |       |           |
| Quickenshtein_WithThreading | Job-NWUPYP |    .NET 4.7.2 |               8192 |    28.07 ms |  0.448 ms |  0.419 ms |  1.00 |    1.00 |       1.00 |    1567 B |     - |     - |     - |    1024 B |
| Quickenshtein_WithThreading | Job-LAXDMC | .NET Core 3.0 |               8192 |    18.01 ms |  0.243 ms |  0.227 ms |  0.64 |    1.56 |       1.00 |    2433 B |     - |     - |     - |     800 B |
|                             |            |               |                    |             |           |           |       |         |            |           |       |       |       |           |
|                Fastenshtein | Job-NWUPYP |    .NET 4.7.2 |               8192 |   168.26 ms |  0.789 ms |  0.700 ms |  1.00 |    1.00 |       1.00 |     328 B |     - |     - |     - |   32816 B |
|                Fastenshtein | Job-LAXDMC | .NET Core 3.0 |               8192 |   148.11 ms |  2.362 ms |  2.209 ms |  0.88 |    1.14 |       1.13 |     329 B |     - |     - |     - |   33126 B |
|                             |            |               |                    |             |           |           |       |         |            |           |       |       |       |           |
|   Quickenshtein_NoThreading | Job-NWUPYP |    .NET 4.7.2 |              32768 | 1,536.85 ms |  8.455 ms |  7.909 ms |  1.00 |    1.00 |       1.00 |     199 B |     - |     - |     - |         - |
|   Quickenshtein_NoThreading | Job-LAXDMC | .NET Core 3.0 |              32768 |   762.24 ms |  5.427 ms |  4.811 ms |  0.50 |    2.02 |       1.49 |     269 B |     - |     - |     - |    1344 B |
|                             |            |               |                    |             |           |           |       |         |            |           |       |       |       |           |
| Quickenshtein_WithThreading | Job-NWUPYP |    .NET 4.7.2 |              32768 |   420.43 ms |  6.345 ms |  4.954 ms |  1.00 |    1.00 |       1.00 |    1567 B |     - |     - |     - |    8192 B |
| Quickenshtein_WithThreading | Job-LAXDMC | .NET Core 3.0 |              32768 |   275.10 ms |  1.943 ms |  1.722 ms |  0.65 |    1.53 |       0.98 |    2433 B |     - |     - |     - |     800 B |
|                             |            |               |                    |             |           |           |       |         |            |           |       |       |       |           |
|                Fastenshtein | Job-NWUPYP |    .NET 4.7.2 |              32768 | 2,703.84 ms | 14.474 ms | 12.086 ms |  1.00 |    1.00 |       1.00 |     328 B |     - |     - |     - |  131096 B |
|                Fastenshtein | Job-LAXDMC | .NET Core 3.0 |              32768 | 2,458.61 ms | 24.463 ms | 21.686 ms |  0.91 |    1.10 |       1.06 |     340 B |     - |     - |     - |  131096 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |        Job |       Runtime | NumberOfCharacters |              Mean |            Error |           StdDev | Ratio | Speedup | Worthiness | Code Size |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |------------------- |------------------:|-----------------:|-----------------:|------:|--------:|-----------:|----------:|-------:|------:|------:|----------:|
| Quickenshtein | Job-LVNZVE |    .NET 4.7.2 |                 40 |         127.45 ns |         0.853 ns |         0.798 ns |  1.00 |    1.00 |       1.00 |     199 B |      - |     - |     - |         - |
| Quickenshtein | Job-WKKKOF | .NET Core 3.0 |                 40 |          83.57 ns |         0.821 ns |         0.768 ns |  0.66 |    1.53 |       1.18 |     257 B |      - |     - |     - |         - |
|               |            |               |                    |                   |                  |                  |       |         |            |           |        |       |       |           |
|  Fastenshtein | Job-LVNZVE |    .NET 4.7.2 |                 40 |       3,059.51 ns |        34.276 ns |        30.385 ns |  1.00 |    1.00 |       1.00 |     328 B | 0.0572 |     - |     - |     185 B |
|  Fastenshtein | Job-WKKKOF | .NET Core 3.0 |                 40 |       3,089.97 ns |        15.603 ns |        14.595 ns |  1.01 |    0.99 |       0.99 |     329 B | 0.0572 |     - |     - |     184 B |
|               |            |               |                    |                   |                  |                  |       |         |            |           |        |       |       |           |
| Quickenshtein | Job-LVNZVE |    .NET 4.7.2 |               8192 |       7,534.16 ns |        54.246 ns |        50.742 ns |  1.00 |    1.00 |       1.00 |     199 B |      - |     - |     - |         - |
| Quickenshtein | Job-WKKKOF | .NET Core 3.0 |               8192 |         724.11 ns |         3.142 ns |         2.624 ns |  0.10 |   10.41 |       8.06 |     257 B |      - |     - |     - |         - |
|               |            |               |                    |                   |                  |                  |       |         |            |           |        |       |       |           |
|  Fastenshtein | Job-LVNZVE |    .NET 4.7.2 |               8192 | 105,796,777.14 ns | 1,105,235.985 ns |   979,762.913 ns |  1.00 |    1.00 |       1.00 |     328 B |      - |     - |     - |   32816 B |
|  Fastenshtein | Job-WKKKOF | .NET Core 3.0 |               8192 | 106,420,849.33 ns | 1,161,318.660 ns | 1,086,298.195 ns |  1.01 |    0.99 |       0.99 |     329 B |      - |     - |     - |   32792 B |

## Empty Text Benchmark

This benchmark shows how well the calculator can optimise an empty string in either the source or target.

|        Method |        Job |       Runtime | IsSourceEmpty |       Mean |     Error |    StdDev | Ratio | Speedup | Worthiness | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated | Code Size |
|-------------- |----------- |-------------- |-------------- |-----------:|----------:|----------:|------:|--------:|-----------:|--------:|-------:|------:|------:|----------:|----------:|
| Quickenshtein | Job-LVNZVE |    .NET 4.7.2 |         False |   2.968 ns | 0.0461 ns | 0.0431 ns |  1.00 |    1.00 |       1.00 |    0.00 |      - |     - |     - |         - |     199 B |
| Quickenshtein | Job-WKKKOF | .NET Core 3.0 |         False |   4.667 ns | 0.0307 ns | 0.0256 ns |  1.58 |    0.63 |       0.49 |    0.02 |      - |     - |     - |         - |     257 B |
|               |            |               |               |            |           |           |       |         |            |         |        |       |       |           |           |
|  Fastenshtein | Job-LVNZVE |    .NET 4.7.2 |         False |   2.440 ns | 0.0262 ns | 0.0233 ns |  1.00 |    1.00 |       1.00 |    0.00 |      - |     - |     - |         - |     328 B |
|  Fastenshtein | Job-WKKKOF | .NET Core 3.0 |         False |   2.441 ns | 0.0364 ns | 0.0341 ns |  1.00 |    1.00 |       1.00 |    0.02 |      - |     - |     - |         - |     329 B |
|               |            |               |               |            |           |           |       |         |            |         |        |       |       |           |           |
| Quickenshtein | Job-LVNZVE |    .NET 4.7.2 |          True |   2.658 ns | 0.0311 ns | 0.0243 ns |  1.00 |    1.00 |       1.00 |    0.00 |      - |     - |     - |         - |     199 B |
| Quickenshtein | Job-WKKKOF | .NET Core 3.0 |          True |   3.893 ns | 0.0265 ns | 0.0222 ns |  1.46 |    0.68 |       0.53 |    0.01 |      - |     - |     - |         - |     257 B |
|               |            |               |               |            |           |           |       |         |            |         |        |       |       |           |           |
|  Fastenshtein | Job-LVNZVE |    .NET 4.7.2 |          True | 373.053 ns | 3.7298 ns | 3.4888 ns |  1.00 |    1.00 |       1.00 |    0.00 | 0.5174 |     - |     - |    1629 B |     328 B |
|  Fastenshtein | Job-WKKKOF | .NET Core 3.0 |          True | 418.223 ns | 3.1024 ns | 2.9020 ns |  1.12 |    0.89 |       0.89 |    0.01 | 0.5174 |     - |     - |    1624 B |     329 B |