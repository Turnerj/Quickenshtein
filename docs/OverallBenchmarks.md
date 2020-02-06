# Overall Benchmarks

These benchmarks are designed to give an overall impression of a Levenshtein distance solution.
With varied string lengths and text combinations, they can show where a solution peforms best and worst.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

## Short Text Benchmark

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |        11.036 |
|  Fastenshtein |         7.081 |

|        Method |       Runtime |    StringA |    StringB |         Mean |      Error |     StdDev |       Median | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------- |----------- |-------------:|-----------:|-----------:|-------------:|------:|-------:|------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |            |            |   197.577 ns |  1.7194 ns |  1.4358 ns |   197.635 ns |  1.00 | 0.1018 |     - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |            |            |    11.988 ns |  0.0754 ns |  0.0705 ns |    11.992 ns |  0.06 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            |            |     2.296 ns |  0.0303 ns |  0.0268 ns |     2.294 ns |  0.01 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            |            |   103.745 ns |  1.4577 ns |  1.3635 ns |   103.322 ns |  1.00 | 0.0764 |     - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |            |            |     5.054 ns |  0.0390 ns |  0.0365 ns |     5.049 ns |  0.05 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            |            |     2.943 ns |  0.0280 ns |  0.0248 ns |     2.941 ns |  0.03 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | abcdefghij |   206.151 ns |  1.5801 ns |  1.4008 ns |   206.433 ns |  1.00 | 0.1147 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | abcdefghij |    12.059 ns |  0.1274 ns |  0.1192 ns |    12.047 ns |  0.06 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | abcdefghij |    15.219 ns |  0.1262 ns |  0.1180 ns |    15.238 ns |  0.07 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | abcdefghij |   114.022 ns |  1.8917 ns |  1.6769 ns |   113.917 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | abcdefghij |     5.100 ns |  0.0707 ns |  0.0661 ns |     5.121 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | abcdefghij |    15.666 ns |  0.1540 ns |  0.1440 ns |    15.624 ns |  0.14 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | jihgfedcba |   211.465 ns |  1.6962 ns |  1.5036 ns |   211.569 ns |  1.00 | 0.1147 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | jihgfedcba |    14.127 ns |  0.2408 ns |  0.2011 ns |    14.199 ns |  0.07 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | jihgfedcba |    19.148 ns |  0.4037 ns |  0.4649 ns |    19.142 ns |  0.09 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | jihgfedcba |   117.201 ns |  3.5509 ns |  4.6172 ns |   115.880 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | jihgfedcba |     4.728 ns |  0.0429 ns |  0.0401 ns |     4.731 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | jihgfedcba |    15.485 ns |  0.1975 ns |  0.1751 ns |    15.513 ns |  0.13 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij |            |   766.600 ns |  2.9119 ns |  2.5814 ns |   767.436 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij |            |    12.494 ns |  0.1185 ns |  0.1108 ns |    12.526 ns | 0.016 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij |            |     2.337 ns |  0.0352 ns |  0.0329 ns |     2.329 ns | 0.003 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij |            |   300.820 ns |  2.5575 ns |  2.3923 ns |   300.090 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij |            |     5.512 ns |  0.0390 ns |  0.0346 ns |     5.505 ns | 0.018 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij |            |     2.217 ns |  0.0400 ns |  0.0374 ns |     2.223 ns | 0.007 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | abcdefghij | 1,137.291 ns | 10.5860 ns |  9.9022 ns | 1,139.545 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |    22.697 ns |  0.2021 ns |  0.1791 ns |    22.684 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |   233.399 ns |  4.5150 ns |  5.0184 ns |   233.520 ns |  0.21 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | abcdefghij |   759.784 ns | 16.8996 ns | 49.2968 ns |   732.746 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |    16.352 ns |  0.3397 ns |  0.3178 ns |    16.460 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |   211.369 ns |  2.2843 ns |  2.0250 ns |   211.857 ns |  0.25 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | jihgfedcba | 1,216.135 ns | 13.4144 ns | 12.5478 ns | 1,218.322 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |   288.745 ns |  2.8551 ns |  2.6707 ns |   289.237 ns |  0.24 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |   249.454 ns |  2.5594 ns |  2.3941 ns |   249.327 ns |  0.21 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | jihgfedcba |   697.494 ns |  5.4684 ns |  5.1151 ns |   696.755 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |   239.061 ns |  1.8918 ns |  1.7696 ns |   239.149 ns |  0.34 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |   244.821 ns |  2.3952 ns |  2.2405 ns |   244.168 ns |  0.35 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba |            |   743.001 ns |  9.2866 ns |  8.2323 ns |   740.712 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba |            |    12.643 ns |  0.0923 ns |  0.0863 ns |    12.615 ns | 0.017 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba |            |     2.728 ns |  0.0275 ns |  0.0244 ns |     2.732 ns | 0.004 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba |            |   294.069 ns |  1.8320 ns |  1.7136 ns |   294.376 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba |            |     5.829 ns |  0.0396 ns |  0.0371 ns |     5.837 ns | 0.020 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba |            |     2.311 ns |  0.0335 ns |  0.0297 ns |     2.309 ns | 0.008 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | abcdefghij | 1,171.652 ns | 18.7567 ns | 16.6273 ns | 1,170.391 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |   275.818 ns |  1.6594 ns |  1.5522 ns |   275.415 ns |  0.24 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |   249.203 ns |  2.0110 ns |  1.8811 ns |   249.658 ns |  0.21 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | abcdefghij |   706.025 ns |  3.8601 ns |  3.4219 ns |   706.622 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |   239.639 ns |  1.9640 ns |  1.8371 ns |   239.556 ns |  0.34 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |   247.187 ns |  2.1758 ns |  2.0353 ns |   246.824 ns |  0.35 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | jihgfedcba | 1,161.702 ns | 22.4212 ns | 22.0206 ns | 1,157.439 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |    23.918 ns |  0.4892 ns |  0.5634 ns |    24.029 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |   224.521 ns |  2.9209 ns |  2.7322 ns |   224.377 ns |  0.19 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |              |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | jihgfedcba |   754.939 ns | 14.9221 ns | 20.4255 ns |   756.211 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |    13.032 ns |  0.2578 ns |  0.2412 ns |    12.978 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |   210.461 ns |  2.4807 ns |  2.3205 ns |   210.250 ns |  0.28 | 0.0203 |     - |     - |      64 B |

## Medium Text Benchmark

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |         6.198 |
|  Fastenshtein |         2.839 |

|        Method |       Runtime |             StringA |             StringB |           Mean |          Error |         StdDev | Ratio | RatioSD |    Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |-------------------- |-------------------- |---------------:|---------------:|---------------:|------:|--------:|---------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |                     |                     |     201.617 ns |      4.0677 ns |      6.6833 ns |  1.00 |    0.00 |   0.1018 |       - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |                     |                     |      11.944 ns |      0.1112 ns |      0.1041 ns |  0.06 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     |                     |       2.248 ns |      0.0459 ns |      0.0407 ns |  0.01 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     |                     |     108.505 ns |      1.0544 ns |      0.9863 ns |  1.00 |    0.00 |   0.0764 |       - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |                     |                     |       5.380 ns |      0.0754 ns |      0.0705 ns |  0.05 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     |                     |       2.219 ns |      0.0427 ns |      0.0400 ns |  0.02 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | aaba(...)bade [400] |     609.553 ns |      5.3495 ns |      4.7422 ns |  1.00 |    0.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |      11.958 ns |      0.0683 ns |      0.0639 ns |  0.02 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |     378.666 ns |      4.7112 ns |      3.9341 ns |  0.62 |    0.01 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | aaba(...)bade [400] |     478.573 ns |      5.1581 ns |      4.8249 ns | 1.000 |    0.00 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |       4.732 ns |      0.0235 ns |      0.0220 ns | 0.010 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |     360.052 ns |      4.4403 ns |      4.1535 ns | 0.752 |    0.01 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     620.371 ns |     12.1686 ns |     18.2134 ns |  1.00 |    0.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |      11.970 ns |      0.1656 ns |      0.1549 ns |  0.02 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     377.954 ns |      2.5624 ns |      2.3968 ns |  0.61 |    0.02 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     476.900 ns |      4.4747 ns |      4.1856 ns |  1.00 |    0.00 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |       5.065 ns |      0.0706 ns |      0.0661 ns |  0.01 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     383.337 ns |      7.3674 ns |      6.8914 ns |  0.80 |    0.02 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] |                     |  19,256.127 ns |    373.3783 ns |    349.2583 ns | 1.000 |    0.00 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |      12.932 ns |      0.3090 ns |      0.2890 ns | 0.001 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |       2.406 ns |      0.0656 ns |      0.0613 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] |                     |   7,347.427 ns |    101.2861 ns |     84.5785 ns | 1.000 |    0.00 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |       5.780 ns |      0.1251 ns |      0.1171 ns | 0.001 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |       2.447 ns |      0.0843 ns |      0.0788 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 809,916.729 ns | 15,937.0943 ns | 28,328.1690 ns | 1.000 |    0.00 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] |     358.738 ns |      3.1872 ns |      2.6615 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 301,947.140 ns |  5,396.0158 ns |  4,783.4275 ns | 0.385 |    0.01 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 718,330.951 ns | 10,566.6908 ns |  9,884.0891 ns | 1.000 |    0.00 | 121.0938 | 60.5469 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] |     348.213 ns |      3.4491 ns |      3.2263 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 291,673.405 ns |  2,670.9939 ns |  2,498.4494 ns | 0.406 |    0.00 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 847,076.758 ns | 12,031.3026 ns | 11,254.0879 ns |  1.00 |    0.00 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 523,633.255 ns |  8,734.3176 ns |  8,170.0861 ns |  0.62 |    0.01 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 405,253.770 ns |  5,827.4305 ns |  5,450.9821 ns |  0.48 |    0.01 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 837,523.223 ns | 14,259.3312 ns | 13,338.1872 ns |  1.00 |    0.00 | 121.0938 | 60.5469 |     - |  657848 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 603,136.251 ns |  4,214.5585 ns |  3,736.0963 ns |  0.72 |    0.01 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 415,975.376 ns |  5,517.3183 ns |  5,160.9030 ns |  0.50 |    0.01 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |  19,126.668 ns |    379.6868 ns |    519.7196 ns | 1.000 |    0.00 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |      12.418 ns |      0.0889 ns |      0.0831 ns | 0.001 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |       2.269 ns |      0.0423 ns |      0.0396 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] |                     |   6,670.389 ns |    111.3552 ns |     98.7135 ns | 1.000 |    0.00 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |       5.455 ns |      0.1010 ns |      0.0945 ns | 0.001 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |       2.292 ns |      0.0339 ns |      0.0317 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 796,420.098 ns |  4,055.6595 ns |  3,793.6664 ns |  1.00 |    0.00 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 500,883.949 ns |  5,353.4989 ns |  4,745.7374 ns |  0.63 |    0.01 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 385,177.897 ns |  3,207.6234 ns |  3,000.4129 ns |  0.48 |    0.00 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 764,360.299 ns |  4,703.5741 ns |  3,927.6974 ns |  1.00 |    0.00 | 121.0938 | 59.5703 |     - |  657848 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 578,130.964 ns |  4,609.7424 ns |  4,311.9559 ns |  0.76 |    0.01 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 346,342.236 ns |  4,291.9674 ns |  4,014.7090 ns |  0.45 |    0.01 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 726,502.402 ns |  6,164.8706 ns |  5,766.6238 ns | 1.000 |    0.00 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |     347.403 ns |      6.9058 ns |      6.4597 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 305,745.110 ns |  4,293.8562 ns |  3,806.3917 ns | 0.421 |    0.01 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 710,180.247 ns | 12,629.4737 ns | 11,813.6176 ns | 1.000 |    0.00 | 122.0703 | 60.5469 |     - |  657848 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |     341.467 ns |      4.0213 ns |      3.7616 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 288,544.839 ns |  2,895.9713 ns |  2,708.8934 ns | 0.406 |    0.01 |   0.4883 |       - |     - |    1625 B |

## Long Text Comparison

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |        12.278 |
|  Fastenshtein |         2.895 |

|        Method |       Runtime |              StringA |              StringB |               Mean |             Error |            StdDev | Ratio | RatioSD |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|-------------- |-------------- |--------------------- |--------------------- |-------------------:|------------------:|------------------:|------:|--------:|-----------:|-----------:|----------:|------------:|
|      Baseline |    .NET 4.7.2 |                      |                      |         192.750 ns |         0.9976 ns |         0.9332 ns |  1.00 |    0.00 |     0.1018 |          - |         - |       321 B |
| Quickenshtein |    .NET 4.7.2 |                      |                      |          11.802 ns |         0.1287 ns |         0.1204 ns |  0.06 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      |                      |           2.244 ns |         0.0290 ns |         0.0243 ns |  0.01 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      |                      |         100.859 ns |         1.1278 ns |         1.0549 ns |  1.00 |    0.00 |     0.0764 |          - |         - |       240 B |
| Quickenshtein | .NET Core 3.0 |                      |                      |           4.647 ns |         0.0793 ns |         0.0742 ns |  0.05 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      |                      |           2.198 ns |         0.0466 ns |         0.0436 ns |  0.02 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       7,895.033 ns |       156.6692 ns |       130.8259 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |          11.866 ns |         0.1384 ns |         0.1294 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       6,732.158 ns |        68.7164 ns |        64.2773 ns | 0.852 |    0.02 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       7,766.478 ns |        79.9472 ns |        70.8711 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |           5.532 ns |         0.1485 ns |         0.2481 ns | 0.001 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       6,901.137 ns |       248.7993 ns |       305.5479 ns | 0.899 |    0.05 |    10.0937 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       8,021.664 ns |        67.1231 ns |        59.5028 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |          13.159 ns |         0.3013 ns |         0.7334 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       7,532.363 ns |       150.3953 ns |       147.7083 ns | 0.938 |    0.02 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       7,927.764 ns |       154.7524 ns |       221.9412 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |           5.076 ns |         0.1362 ns |         0.1398 ns | 0.001 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       7,291.226 ns |       103.7682 ns |        97.0649 ns | 0.910 |    0.03 |    10.1013 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |     354,240.063 ns |     2,969.2365 ns |     2,777.4257 ns | 1.000 |    0.00 |   142.5781 |          - |         - |    452484 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |          12.382 ns |         0.1270 ns |         0.1188 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |           2.284 ns |         0.0315 ns |         0.0295 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] |                      |     153,995.417 ns |     2,083.9942 ns |     1,847.4066 ns | 1.000 |    0.00 |    78.8574 |    20.5078 |         - |    320202 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |           4.719 ns |         0.0532 ns |         0.0498 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |           2.227 ns |         0.0342 ns |         0.0319 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 370,839,257.143 ns | 1,914,061.5911 ns | 1,696,765.7450 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |       6,218.058 ns |        68.7499 ns |        64.3087 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 116,873,678.571 ns | 1,459,302.7517 ns | 1,293,633.8790 ns | 0.315 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 362,763,686.667 ns | 2,053,582.8256 ns | 1,920,922.6492 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |       6,252.661 ns |        43.5173 ns |        40.7061 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 117,985,237.333 ns |   383,070.5673 ns |   358,324.4463 ns | 0.325 |    0.00 |          - |          - |         - |     32272 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 369,703,260.000 ns | 2,942,339.4699 ns | 2,752,266.1657 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 121,278,736.000 ns | 1,489,570.8897 ns | 1,393,345.5345 ns |  0.33 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 125,332,941.667 ns |   948,324.9666 ns |   887,063.7622 ns |  0.34 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 365,339,933.333 ns | 3,070,780.9476 ns | 2,872,410.4036 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 114,895,071.429 ns |   698,514.8330 ns |   619,215.2052 ns |  0.31 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 116,593,050.667 ns |   616,084.8032 ns |   576,286.1072 ns |  0.32 |    0.00 |          - |          - |         - |     32291 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |     338,068.561 ns |     2,998.6038 ns |     2,804.8959 ns | 1.000 |    0.00 |   142.5781 |          - |         - |    452484 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |          12.385 ns |         0.1019 ns |         0.0953 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |           2.240 ns |         0.0490 ns |         0.0459 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] |                      |     156,538.476 ns |     3,182.9676 ns |     5,574.7111 ns | 1.000 |    0.00 |    98.1445 |     1.7090 |         - |    320200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |           5.387 ns |         0.0583 ns |         0.0517 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |           2.245 ns |         0.0470 ns |         0.0439 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 369,287,333.333 ns | 1,660,558.0096 ns | 1,553,286.9925 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 121,385,274.667 ns | 1,479,366.8719 ns | 1,383,800.6898 ns |  0.33 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 125,036,863.333 ns | 1,241,648.3963 ns | 1,161,438.6802 ns |  0.34 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 360,123,100.000 ns | 1,837,984.5525 ns | 1,629,325.4319 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 136,293,608.333 ns |   877,946.9249 ns |   821,232.0982 ns |  0.38 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 125,518,110.000 ns |   718,665.3783 ns |   672,240.0408 ns |  0.35 |    0.00 |          - |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 371,723,653.333 ns | 2,047,129.3954 ns | 1,914,886.1066 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |       6,243.217 ns |        47.8579 ns |        42.4248 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 118,475,233.333 ns |   526,877.2818 ns |   492,841.3363 ns | 0.319 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 364,028,266.667 ns | 2,421,943.7256 ns | 2,265,487.6636 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256257536 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |       5,310.181 ns |        48.5102 ns |        45.3765 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 101,903,851.429 ns |   922,637.9750 ns |   817,894.5329 ns | 0.280 |    0.00 |          - |          - |         - |     32291 B |