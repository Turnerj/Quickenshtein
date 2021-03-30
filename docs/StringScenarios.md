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

|        Method |        Job |       Runtime |      Mean |    Error |   StdDev |     Op/s | Ratio | Speedup | RatioSD | Code Size |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |----------:|---------:|---------:|---------:|------:|--------:|--------:|----------:|--------:|--------:|------:|----------:|
|      Baseline | Job-LYUZXD |    .NET 4.7.2 | 247.07 us | 3.845 us | 3.408 us |  4,047.4 |  1.00 |    1.00 |    0.00 |    2559 B | 74.7070 | 18.5547 |     - |  301049 B |
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |  29.03 us | 0.492 us | 0.460 us | 34,449.2 |  0.12 |    8.50 |    0.00 |    3705 B |       - |       - |     - |         - |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |  93.08 us | 1.011 us | 0.896 us | 10,743.2 |  0.38 |    2.65 |    0.01 |     324 B |  0.2441 |       - |     - |    1092 B |
|      Baseline | Job-MALOLQ | .NET Core 5.0 | 301.59 us | 4.449 us | 4.162 us |  3,315.8 |  1.22 |    0.82 |    0.03 |    2057 B | 66.8945 | 20.5078 |     - |  291736 B |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |  32.33 us | 0.557 us | 0.596 us | 30,930.8 |  0.13 |    7.64 |    0.00 |    4757 B |       - |       - |     - |         - |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |  94.43 us | 0.798 us | 0.746 us | 10,589.5 |  0.38 |    2.62 |    0.01 |     317 B |  0.2441 |       - |     - |    1088 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method |        Job |       Runtime | NumberOfCharacters |        Mean |      Error |     StdDev |         Op/s | Ratio | Speedup | RatioSD | Code Size |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |------------------- |------------:|-----------:|-----------:|-------------:|------:|--------:|--------:|----------:|-------:|------:|------:|----------:|
|      Baseline | Job-LYUZXD |    .NET 4.7.2 |                  5 |   652.06 ns |  12.670 ns |  12.444 ns |  1,533,589.5 |  1.00 |    1.00 |    0.00 |    2559 B | 0.2241 |     - |     - |     706 B |
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |                  5 |   103.35 ns |   2.103 ns |   2.422 ns |  9,675,749.3 |  0.16 |    6.31 |    0.01 |    3705 B |      - |     - |     - |         - |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |                  5 |    48.28 ns |   0.714 ns |   0.668 ns | 20,710,439.7 |  0.07 |   13.49 |    0.00 |     324 B | 0.0153 |     - |     - |      48 B |
|      Baseline | Job-MALOLQ | .NET Core 5.0 |                  5 |   231.63 ns |   4.422 ns |   4.731 ns |  4,317,188.3 |  0.36 |    2.81 |    0.01 |    2057 B | 0.1707 |     - |     - |     536 B |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |                  5 |   218.24 ns |   2.175 ns |   2.035 ns |  4,582,144.8 |  0.34 |    2.99 |    0.01 |    4757 B |      - |     - |     - |         - |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |                  5 |    48.01 ns |   0.938 ns |   0.877 ns | 20,829,144.2 |  0.07 |   13.57 |    0.00 |     317 B | 0.0153 |     - |     - |      48 B |
|               |            |               |                    |             |            |            |              |       |         |         |           |        |       |       |           |
|      Baseline | Job-LYUZXD |    .NET 4.7.2 |                 20 | 3,169.29 ns |  60.686 ns |  56.766 ns |    315,527.8 |  1.00 |    1.00 |    0.00 |    2559 B | 1.0567 |     - |     - |    3330 B |
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |                 20 |   669.12 ns |  12.200 ns |  11.412 ns |  1,494,508.2 |  0.21 |    4.74 |    0.01 |    3705 B |      - |     - |     - |         - |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |                 20 |   563.85 ns |  10.762 ns |   9.540 ns |  1,773,506.2 |  0.18 |    5.62 |    0.00 |     324 B | 0.0324 |     - |     - |     104 B |
|      Baseline | Job-MALOLQ | .NET Core 5.0 |                 20 | 2,085.53 ns |  41.421 ns |  38.745 ns |    479,493.8 |  0.66 |    1.52 |    0.01 |    2057 B | 0.8659 |     - |     - |    2720 B |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |                 20 |   871.95 ns |  15.622 ns |  14.613 ns |  1,146,850.3 |  0.28 |    3.64 |    0.01 |    4757 B |      - |     - |     - |         - |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |                 20 |   557.58 ns |  10.901 ns |  10.706 ns |  1,793,477.5 |  0.18 |    5.68 |    0.00 |     317 B | 0.0324 |     - |     - |     104 B |
|               |            |               |                    |             |            |            |              |       |         |         |           |        |       |       |           |
|      Baseline | Job-LYUZXD |    .NET 4.7.2 |                 40 | 8,088.09 ns |  93.844 ns |  87.782 ns |    123,638.6 |  1.00 |    1.00 |    0.00 |    2559 B | 3.0365 |     - |     - |    9564 B |
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |                 40 | 2,431.94 ns |  46.144 ns |  43.163 ns |    411,194.7 |  0.30 |    3.33 |    0.01 |    3705 B |      - |     - |     - |         - |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |                 40 | 2,387.54 ns |  44.385 ns |  41.518 ns |    418,840.8 |  0.30 |    3.39 |    0.01 |     324 B | 0.0572 |     - |     - |     185 B |
|      Baseline | Job-MALOLQ | .NET Core 5.0 |                 40 | 7,172.82 ns | 136.403 ns | 127.592 ns |    139,415.1 |  0.89 |    1.13 |    0.02 |    2057 B | 2.6779 |     - |     - |    8400 B |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |                 40 | 2,042.65 ns |  30.383 ns |  28.420 ns |    489,559.9 |  0.25 |    3.96 |    0.01 |    4757 B |      - |     - |     - |         - |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |                 40 | 2,494.32 ns |  49.260 ns |  48.379 ns |    400,911.4 |  0.31 |    3.24 |    0.01 |     317 B | 0.0572 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|                      Method |        Job |       Runtime | NumberOfCharacters |        Mean |     Error |    StdDev |    Op/s | Ratio | Speedup | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated | Code Size |
|---------------------------- |----------- |-------------- |------------------- |------------:|----------:|----------:|--------:|------:|--------:|--------:|------:|------:|------:|----------:|----------:|
|   Quickenshtein_NoThreading | Job-LYUZXD |    .NET 4.7.2 |               8192 |    82.33 ms |  1.479 ms |  1.383 ms | 12.1465 |  1.00 |    1.00 |    0.00 |     - |     - |     - |         - |    3705 B |
|   Quickenshtein_NoThreading | Job-MALOLQ | .NET Core 5.0 |               8192 |    18.02 ms |  0.184 ms |  0.172 ms | 55.4877 |  0.22 |    4.57 |    0.01 |     - |     - |     - |       9 B |    4925 B |
|                             |            |               |                    |             |           |           |         |       |         |         |       |       |       |           |           |
| Quickenshtein_WithThreading | Job-LYUZXD |    .NET 4.7.2 |               8192 |    29.18 ms |  0.492 ms |  0.436 ms | 34.2644 |  1.00 |    1.00 |    0.00 |     - |     - |     - |    1024 B |    5048 B |
| Quickenshtein_WithThreading | Job-MALOLQ | .NET Core 5.0 |               8192 |    18.53 ms |  0.258 ms |  0.241 ms | 53.9635 |  0.64 |    1.57 |    0.01 |     - |     - |     - |     818 B |   10408 B |
|                             |            |               |                    |             |           |           |         |       |         |         |       |       |       |           |           |
|                Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |               8192 |    84.62 ms |  1.636 ms |  1.680 ms | 11.8178 |  1.00 |    1.00 |    0.00 |     - |     - |     - |   32816 B |     324 B |
|                Fastenshtein | Job-MALOLQ | .NET Core 5.0 |               8192 |    84.46 ms |  1.330 ms |  1.244 ms | 11.8403 |  1.00 |    1.00 |    0.03 |     - |     - |     - |   32999 B |      48 B |
|                             |            |               |                    |             |           |           |         |       |         |         |       |       |       |           |           |
|   Quickenshtein_NoThreading | Job-LYUZXD |    .NET 4.7.2 |              32768 | 1,293.23 ms | 22.708 ms | 21.241 ms |  0.7733 |  1.00 |    1.00 |    0.00 |     - |     - |     - |         - |    3705 B |
|   Quickenshtein_NoThreading | Job-MALOLQ | .NET Core 5.0 |              32768 |   295.37 ms |  5.449 ms |  5.097 ms |  3.3855 |  0.23 |    4.38 |    0.01 |     - |     - |     - |     144 B |    4925 B |
|                             |            |               |                    |             |           |           |         |       |         |         |       |       |       |           |           |
| Quickenshtein_WithThreading | Job-LYUZXD |    .NET 4.7.2 |              32768 |   436.31 ms |  6.126 ms |  5.730 ms |  2.2920 |  1.00 |    1.00 |    0.00 |     - |     - |     - |    8192 B |    5048 B |
| Quickenshtein_WithThreading | Job-MALOLQ | .NET Core 5.0 |              32768 |   286.32 ms |  3.283 ms |  2.741 ms |  3.4926 |  0.66 |    1.53 |    0.01 |     - |     - |     - |    1768 B |     158 B |
|                             |            |               |                    |             |           |           |         |       |         |         |       |       |       |           |           |
|                Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |              32768 | 1,346.64 ms | 22.841 ms | 21.366 ms |  0.7426 |  1.00 |    1.00 |    0.00 |     - |     - |     - |  131096 B |     324 B |
|                Fastenshtein | Job-MALOLQ | .NET Core 5.0 |              32768 | 1,354.35 ms | 21.269 ms | 19.895 ms |  0.7384 |  1.01 |    0.99 |    0.02 |     - |     - |     - |  131096 B |      48 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |        Job |       Runtime | NumberOfCharacters |            Mean |           Error |        StdDev |         Op/s | Ratio | Speedup | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated | Code Size |
|-------------- |----------- |-------------- |------------------- |----------------:|----------------:|--------------:|-------------:|------:|--------:|--------:|-------:|------:|------:|----------:|----------:|
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |                 40 |        104.1 ns |         1.40 ns |       1.31 ns | 9,603,518.53 |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |    3705 B |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |                 40 |        159.2 ns |         2.23 ns |       2.08 ns | 6,282,205.52 |  1.53 |    0.65 |    0.03 |      - |     - |     - |         - |    4757 B |
|               |            |               |                    |                 |                 |               |              |       |         |         |        |       |       |           |           |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |                 40 |      1,991.4 ns |        34.20 ns |      31.99 ns |   502,156.11 |  1.00 |    1.00 |    0.00 | 0.0572 |     - |     - |     185 B |     324 B |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |                 40 |      1,437.9 ns |        26.20 ns |      24.51 ns |   695,439.33 |  0.72 |    1.39 |    0.02 | 0.0572 |     - |     - |     184 B |     317 B |
|               |            |               |                    |                 |                 |               |              |       |         |         |        |       |       |           |           |
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |               8192 |      5,327.3 ns |       103.54 ns |     106.33 ns |   187,713.19 |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |    3705 B |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |               8192 |        852.8 ns |        23.53 ns |      66.38 ns | 1,172,647.93 |  0.15 |    6.72 |    0.02 |      - |     - |     - |         - |    4757 B |
|               |            |               |                    |                 |                 |               |              |       |         |         |        |       |       |           |           |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |               8192 | 65,273,509.2 ns |   772,466.84 ns | 722,565.96 ns |        15.32 |  1.00 |    1.00 |    0.00 |      - |     - |     - |   32816 B |     324 B |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |               8192 | 54,018,758.5 ns | 1,053,343.43 ns | 985,298.10 ns |        18.51 |  0.83 |    1.21 |    0.02 |      - |     - |     - |   32792 B |     317 B |

## Empty Text Benchmark

This benchmark shows how well the calculator can optimise an empty string in either the source or target.

|        Method |        Job |       Runtime | IsSourceEmpty |       Mean |     Error |    StdDev |          Op/s | Ratio | Speedup | RatioSD | Code Size |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |----------- |-------------- |-------------- |-----------:|----------:|----------:|--------------:|------:|--------:|--------:|----------:|-------:|------:|------:|----------:|
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |         False |   2.669 ns | 0.0702 ns | 0.0657 ns | 374,651,568.4 |  1.00 |    1.00 |    0.00 |     199 B |      - |     - |     - |         - |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |         False |   4.232 ns | 0.1143 ns | 0.1223 ns | 236,283,845.8 |  1.59 |    0.63 |    0.06 |     254 B |      - |     - |     - |         - |
|               |            |               |               |            |           |           |               |       |         |         |           |        |       |       |           |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |         False |   2.190 ns | 0.0649 ns | 0.0575 ns | 456,581,987.1 |  1.00 |    1.00 |    0.00 |     324 B |      - |     - |     - |         - |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |         False |   2.357 ns | 0.0596 ns | 0.0498 ns | 424,233,188.8 |  1.07 |    0.93 |    0.03 |     317 B |      - |     - |     - |         - |
|               |            |               |               |            |           |           |               |       |         |         |           |        |       |       |           |
| Quickenshtein | Job-LYUZXD |    .NET 4.7.2 |          True |   2.365 ns | 0.0363 ns | 0.0322 ns | 422,752,251.3 |  1.00 |    1.00 |    0.00 |     199 B |      - |     - |     - |         - |
| Quickenshtein | Job-MALOLQ | .NET Core 5.0 |          True |   3.587 ns | 0.1037 ns | 0.1018 ns | 278,787,396.6 |  1.52 |    0.66 |    0.05 |     254 B |      - |     - |     - |         - |
|               |            |               |               |            |           |           |               |       |         |         |           |        |       |       |           |
|  Fastenshtein | Job-LYUZXD |    .NET 4.7.2 |          True | 300.852 ns | 6.0131 ns | 6.1751 ns |   3,323,894.6 |  1.00 |    1.00 |    0.00 |     324 B | 0.5174 |     - |     - |    1629 B |
|  Fastenshtein | Job-MALOLQ | .NET Core 5.0 |          True | 291.861 ns | 5.6623 ns | 5.8147 ns |   3,426,286.5 |  0.97 |    1.03 |    0.03 |     317 B | 0.5174 |     - |     - |    1624 B |