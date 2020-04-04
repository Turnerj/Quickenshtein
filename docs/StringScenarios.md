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

|        Method |       Runtime |      Mean |    Error |   StdDev | Ratio | Speedup | RatioSD |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------:|---------:|---------:|------:|--------:|--------:|--------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 | 298.79 us | 1.989 us | 1.860 us |  1.00 |    1.00 |    0.00 | 73.2422 | 19.0430 |     - |  301044 B |
| Quickenshtein |    .NET 4.7.2 |  40.78 us | 0.277 us | 0.231 us |  0.14 |    7.32 |    0.00 |       - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | 103.02 us | 0.376 us | 0.352 us |  0.34 |    2.90 |    0.00 |  0.2441 |       - |     - |    1092 B |
|      Baseline | .NET Core 3.0 | 327.31 us | 4.373 us | 4.091 us |  1.10 |    0.91 |    0.02 | 65.4297 | 22.9492 |     - |  291738 B |
| Quickenshtein | .NET Core 3.0 |  41.20 us | 0.241 us | 0.226 us |  0.14 |    7.25 |    0.00 |       - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | 101.97 us | 0.712 us | 0.666 us |  0.34 |    2.93 |    0.00 |  0.2441 |       - |     - |    1089 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method |       Runtime | NumberOfCharacters |        Mean |     Error |    StdDev | Ratio | Speedup |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |------------:|----------:|----------:|------:|--------:|-------:|------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |                  5 |   589.48 ns |  6.800 ns |  6.028 ns |  1.00 |    1.00 | 0.2241 |     - |     - |     706 B |
| Quickenshtein |    .NET 4.7.2 |                  5 |   115.02 ns |  0.574 ns |  0.509 ns |  0.20 |    5.13 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                  5 |    65.10 ns |  0.501 ns |  0.469 ns |  0.11 |    9.05 | 0.0153 |     - |     - |      48 B |
|      Baseline | .NET Core 3.0 |                  5 |   313.37 ns |  3.879 ns |  3.629 ns |  0.53 |    1.88 | 0.1707 |     - |     - |     536 B |
| Quickenshtein | .NET Core 3.0 |                  5 |    93.28 ns |  0.377 ns |  0.315 ns |  0.16 |    6.32 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                  5 |    63.13 ns |  0.416 ns |  0.389 ns |  0.11 |    9.33 | 0.0153 |     - |     - |      48 B |
|               |               |                    |             |           |           |       |         |        |       |       |           |
|      Baseline |    .NET 4.7.2 |                 20 | 3,281.75 ns | 35.335 ns | 33.052 ns |  1.00 |    1.00 | 1.0567 |     - |     - |    3330 B |
| Quickenshtein |    .NET 4.7.2 |                 20 |   747.73 ns |  3.462 ns |  3.069 ns |  0.23 |    4.38 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 20 | 1,053.86 ns |  8.593 ns |  7.617 ns |  0.32 |    3.11 | 0.0324 |     - |     - |     104 B |
|      Baseline | .NET Core 3.0 |                 20 | 2,513.76 ns | 10.126 ns |  9.471 ns |  0.77 |    1.31 | 0.8659 |     - |     - |    2720 B |
| Quickenshtein | .NET Core 3.0 |                 20 |   466.99 ns |  4.904 ns |  4.587 ns |  0.14 |    7.03 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 20 | 1,075.45 ns |  5.510 ns |  5.154 ns |  0.33 |    3.05 | 0.0324 |     - |     - |     104 B |
|               |               |                    |             |           |           |       |         |        |       |       |           |
|      Baseline |    .NET 4.7.2 |                 40 | 8,746.46 ns | 45.044 ns | 37.613 ns |  1.00 |    1.00 | 3.0365 |     - |     - |    9564 B |
| Quickenshtein |    .NET 4.7.2 |                 40 | 2,771.33 ns | 21.480 ns | 19.042 ns |  0.32 |    3.16 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 40 | 3,796.08 ns | 29.709 ns | 24.808 ns |  0.43 |    2.30 | 0.0572 |     - |     - |     185 B |
|      Baseline | .NET Core 3.0 |                 40 | 7,498.10 ns | 31.140 ns | 29.129 ns |  0.86 |    1.17 | 2.6779 |     - |     - |    8400 B |
| Quickenshtein | .NET Core 3.0 |                 40 | 1,290.20 ns |  8.051 ns |  6.285 ns |  0.15 |    6.78 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 40 | 3,813.29 ns | 17.070 ns | 15.967 ns |  0.44 |    2.30 | 0.0572 |     - |     - |     184 B |

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

|        Method |       Runtime | NumberOfCharacters |              Mean |            Error |           StdDev |            Median | Ratio | Speedup | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |------------------:|-----------------:|-----------------:|------------------:|------:|--------:|--------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |                 40 |         124.49 ns |         1.555 ns |         1.298 ns |         124.19 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |                 40 |          80.60 ns |         0.364 ns |         0.340 ns |          80.57 ns |  0.65 |    1.54 |    0.01 |      - |     - |     - |         - |
|               |               |                    |                   |                  |                  |                   |       |         |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |                 40 |       2,608.64 ns |        51.829 ns |       118.040 ns |       2,566.19 ns |  1.00 |    1.00 |    0.00 | 0.0572 |     - |     - |     185 B |
|  Fastenshtein | .NET Core 3.0 |                 40 |       2,532.75 ns |        15.402 ns |        13.653 ns |       2,531.92 ns |  0.93 |    1.08 |    0.05 | 0.0572 |     - |     - |     184 B |
|               |               |                    |                   |                  |                  |                   |       |         |         |        |       |       |           |
| Quickenshtein |    .NET 4.7.2 |               8192 |       7,662.71 ns |        35.519 ns |        31.487 ns |       7,663.41 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |               8192 |         718.92 ns |         4.110 ns |         3.845 ns |         718.91 ns |  0.09 |   10.65 |    0.00 |      - |     - |     - |         - |
|               |               |                    |                   |                  |                  |                   |       |         |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |               8192 |  90,687,371.43 ns |   693,400.442 ns |   614,681.431 ns |  90,527,475.00 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |   32816 B |
|  Fastenshtein | .NET Core 3.0 |               8192 | 102,309,202.67 ns | 1,735,233.691 ns | 1,623,138.671 ns | 101,789,320.00 ns |  1.13 |    0.89 |    0.02 |      - |     - |     - |   34710 B |

## Empty Text Benchmark

This benchmark shows how well the calculator can optimise an empty string in either the source or target.

|        Method |       Runtime | IsSourceEmpty |       Mean |     Error |    StdDev | Ratio | Speedup | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |-------------- |-----------:|----------:|----------:|------:|--------:|--------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |         False |   2.324 ns | 0.0344 ns | 0.0305 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |         False |   4.323 ns | 0.0355 ns | 0.0315 ns |  1.86 |    0.54 |    0.02 |      - |     - |     - |         - |
|               |               |               |            |           |           |       |         |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |         False |   2.330 ns | 0.0296 ns | 0.0247 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |         False |   2.287 ns | 0.0258 ns | 0.0242 ns |  0.98 |    1.02 |    0.01 |      - |     - |     - |         - |
|               |               |               |            |           |           |       |         |         |        |       |       |           |
| Quickenshtein |    .NET 4.7.2 |          True |   2.304 ns | 0.0571 ns | 0.0506 ns |  1.00 |    1.00 |    0.00 |      - |     - |     - |         - |
| Quickenshtein | .NET Core 3.0 |          True |   3.737 ns | 0.0351 ns | 0.0311 ns |  1.62 |    0.62 |    0.04 |      - |     - |     - |         - |
|               |               |               |            |           |           |       |         |         |        |       |       |           |
|  Fastenshtein |    .NET 4.7.2 |          True | 373.257 ns | 2.3123 ns | 2.0498 ns |  1.00 |    1.00 |    0.00 | 0.5174 |     - |     - |    1629 B |
|  Fastenshtein | .NET Core 3.0 |          True | 350.061 ns | 5.8269 ns | 5.4505 ns |  0.94 |    1.07 |    0.02 | 0.5174 |     - |     - |    1624 B |