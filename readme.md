# Quickenshtein

A quick and memory efficient Levenshtein Distance calculator for .NET



## Additional Benchmarks

### Short Text Comparison

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |         9.091 |
|  Fastenshtein |         7.143 |


```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

|        Method |       Runtime |    StringA |    StringB |         Mean |      Error |     StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------- |----------- |-------------:|-----------:|-----------:|------:|-------:|------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |            |            |   200.512 ns |  1.2272 ns |  1.0879 ns |  1.00 | 0.1018 |     - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |            |            |    16.359 ns |  0.1243 ns |  0.1163 ns |  0.08 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            |            |     2.324 ns |  0.0240 ns |  0.0224 ns |  0.01 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            |            |   104.829 ns |  0.8522 ns |  0.7972 ns |  1.00 | 0.0764 |     - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |            |            |     4.565 ns |  0.0284 ns |  0.0237 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            |            |     2.301 ns |  0.0265 ns |  0.0248 ns |  0.02 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | abcdefghij |   213.126 ns |  1.7547 ns |  1.5555 ns |  1.00 | 0.1147 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | abcdefghij |    14.160 ns |  0.0933 ns |  0.0827 ns |  0.07 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | abcdefghij |    15.680 ns |  0.1158 ns |  0.1027 ns |  0.07 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | abcdefghij |   117.372 ns |  1.2112 ns |  1.1330 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | abcdefghij |     5.085 ns |  0.1342 ns |  0.1255 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | abcdefghij |    15.672 ns |  0.2432 ns |  0.2275 ns |  0.13 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | jihgfedcba |   210.836 ns |  1.6588 ns |  1.4705 ns |  1.00 | 0.1147 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | jihgfedcba |    20.360 ns |  0.1389 ns |  0.1231 ns |  0.10 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | jihgfedcba |    15.798 ns |  0.3436 ns |  0.3214 ns |  0.07 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | jihgfedcba |   113.497 ns |  0.9928 ns |  0.9287 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | jihgfedcba |     4.963 ns |  0.0278 ns |  0.0260 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | jihgfedcba |    15.996 ns |  0.3532 ns |  0.3779 ns |  0.14 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij |            |   754.789 ns |  3.0890 ns |  2.8895 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij |            |    14.508 ns |  0.1690 ns |  0.1498 ns | 0.019 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij |            |     2.319 ns |  0.0380 ns |  0.0356 ns | 0.003 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij |            |   295.352 ns |  2.5912 ns |  2.4239 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij |            |     4.881 ns |  0.1001 ns |  0.0936 ns | 0.017 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij |            |     2.320 ns |  0.0505 ns |  0.0473 ns | 0.008 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | abcdefghij | 1,166.632 ns |  9.4799 ns |  8.8675 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |    36.012 ns |  0.2756 ns |  0.2578 ns |  0.03 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |   206.585 ns |  1.5449 ns |  1.3696 ns |  0.18 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | abcdefghij |   699.825 ns |  2.8798 ns |  2.5529 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |    12.934 ns |  0.1056 ns |  0.0987 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |   205.416 ns |  1.1419 ns |  1.0682 ns |  0.29 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | jihgfedcba | 1,197.323 ns |  7.8963 ns |  7.3862 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |   382.780 ns |  2.0987 ns |  1.9631 ns |  0.32 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |   248.753 ns |  1.7316 ns |  1.6197 ns |  0.21 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | jihgfedcba |   710.301 ns |  4.3243 ns |  4.0450 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |   284.951 ns |  2.4234 ns |  2.1482 ns |  0.40 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |   246.815 ns |  1.6280 ns |  1.5229 ns |  0.35 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba |            |   749.064 ns |  5.5070 ns |  5.1512 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba |            |    14.485 ns |  0.0863 ns |  0.0807 ns | 0.019 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba |            |     2.321 ns |  0.0247 ns |  0.0231 ns | 0.003 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba |            |   290.240 ns |  2.7829 ns |  2.4670 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba |            |     5.513 ns |  0.0529 ns |  0.0469 ns | 0.019 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba |            |     2.293 ns |  0.0244 ns |  0.0228 ns | 0.008 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | abcdefghij | 1,189.159 ns |  6.7155 ns |  6.2817 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |   382.873 ns |  1.5463 ns |  1.3707 ns |  0.32 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |   248.189 ns |  1.4575 ns |  1.3633 ns |  0.21 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | abcdefghij |   709.714 ns |  4.6425 ns |  4.3426 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |   281.479 ns |  1.5593 ns |  1.4585 ns |  0.40 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |   246.909 ns |  1.4453 ns |  1.3519 ns |  0.35 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | jihgfedcba | 1,168.333 ns | 14.2088 ns | 12.5957 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |    36.198 ns |  0.2801 ns |  0.2620 ns |  0.03 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |   205.159 ns |  1.3027 ns |  1.2186 ns |  0.18 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | jihgfedcba |   707.057 ns |  4.2054 ns |  3.9337 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |    12.860 ns |  0.0669 ns |  0.0626 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |   203.563 ns |  2.1884 ns |  2.0471 ns |  0.29 | 0.0203 |     - |     - |      64 B |


### Medium Text Comparison

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |         7.020 |
|  Fastenshtein |         2.847 |

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

|        Method |       Runtime |             StringA |             StringB |           Mean |          Error |         StdDev | Ratio |    Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |-------------------- |-------------------- |---------------:|---------------:|---------------:|------:|---------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |                     |                     |     193.791 ns |      2.0033 ns |      1.6729 ns |  1.00 |   0.1018 |       - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |                     |                     |      13.981 ns |      0.1845 ns |      0.1541 ns |  0.07 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     |                     |       2.290 ns |      0.0351 ns |      0.0311 ns |  0.01 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     |                     |     101.577 ns |      1.1680 ns |      0.9753 ns |  1.00 |   0.0764 |       - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |                     |                     |       4.891 ns |      0.0698 ns |      0.0653 ns |  0.05 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     |                     |       3.135 ns |      0.1034 ns |      0.1727 ns |  0.03 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | aaba(...)bade [400] |     611.824 ns |      5.5537 ns |      4.9232 ns |  1.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |      14.282 ns |      0.1478 ns |      0.1383 ns |  0.02 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |     378.518 ns |      2.0168 ns |      1.8865 ns |  0.62 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | aaba(...)bade [400] |     484.579 ns |      9.5762 ns |      8.9576 ns | 1.000 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |       4.527 ns |      0.0522 ns |      0.0463 ns | 0.009 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |     337.067 ns |      2.5290 ns |      2.2419 ns | 0.695 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     605.042 ns |      5.4083 ns |      4.7943 ns |  1.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |      14.089 ns |      0.2514 ns |      0.2100 ns |  0.02 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     378.047 ns |      7.0817 ns |      6.6243 ns |  0.63 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     511.996 ns |      4.1006 ns |      3.8357 ns | 1.000 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |       4.845 ns |      0.0538 ns |      0.0503 ns | 0.009 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     363.236 ns |      3.0612 ns |      2.8635 ns | 0.709 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] |                     |  18,073.650 ns |    342.8108 ns |    320.6655 ns | 1.000 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |      14.089 ns |      0.1380 ns |      0.1224 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |       2.307 ns |      0.0253 ns |      0.0236 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] |                     |   6,874.365 ns |     48.5199 ns |     43.0116 ns | 1.000 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |       5.418 ns |      0.0603 ns |      0.0534 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |       2.275 ns |      0.0372 ns |      0.0348 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 722,542.571 ns |  6,717.1058 ns |  5,954.5394 ns | 1.000 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] |     710.911 ns |      4.4346 ns |      4.1482 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 288,396.917 ns |  2,999.7603 ns |  2,805.9776 ns | 0.399 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 677,948.821 ns |  5,975.7664 ns |  4,990.0356 ns | 1.000 | 121.0938 | 60.5469 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] |     283.949 ns |      2.6227 ns |      2.4533 ns | 0.000 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 277,844.176 ns |  2,241.4490 ns |  2,096.6528 ns | 0.411 |   0.4883 |       - |     - |    1626 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 800,329.316 ns | 10,822.0706 ns | 10,122.9715 ns |  1.00 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 464,280.291 ns |  3,584.7607 ns |  3,353.1874 ns |  0.58 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 388,389.310 ns |  2,718.5630 ns |  2,542.9455 ns |  0.49 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 766,968.141 ns |  8,336.2469 ns |  7,797.7305 ns |  1.00 | 121.0938 | 60.5469 |     - |  657845 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 468,948.434 ns |  4,551.8030 ns |  4,257.7594 ns |  0.61 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 398,343.486 ns |  4,404.4972 ns |  4,119.9694 ns |  0.52 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |  18,045.764 ns |    301.5986 ns |    282.1155 ns | 1.000 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |      14.152 ns |      0.1100 ns |      0.1029 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |       2.265 ns |      0.0329 ns |      0.0308 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] |                     |   6,843.530 ns |     68.3328 ns |     63.9186 ns | 1.000 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |       5.425 ns |      0.0388 ns |      0.0363 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |       2.235 ns |      0.0338 ns |      0.0316 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 795,425.293 ns | 11,005.5684 ns | 10,294.6155 ns |  1.00 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 463,463.577 ns |  4,007.1814 ns |  3,748.3200 ns |  0.58 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 381,889.818 ns |  4,803.2056 ns |  4,492.9215 ns |  0.48 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 762,424.530 ns |  3,898.8493 ns |  3,255.7157 ns |  1.00 | 122.0703 | 60.5469 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 468,336.589 ns |  1,185.0395 ns |    925.2009 ns |  0.61 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 390,183.763 ns |  2,029.1246 ns |  1,898.0444 ns |  0.51 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 721,642.214 ns |  5,191.8107 ns |  4,856.4229 ns | 1.000 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |     708.513 ns |      7.2182 ns |      6.0275 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 292,410.863 ns |  5,170.6447 ns |  4,836.6242 ns | 0.405 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |                |                |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 670,104.492 ns |  4,897.9282 ns |  4,341.8858 ns | 1.000 | 121.0938 | 60.5469 |     - |  657846 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |     281.458 ns |      1.5394 ns |      1.4400 ns | 0.000 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 276,994.092 ns |  1,956.5353 ns |  1,830.1443 ns | 0.413 |   0.4883 |       - |     - |    1625 B |


### Long Text Comparison

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |        10.022 |
|  Fastenshtein |         2.883 |


```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

|        Method |       Runtime |              StringA |              StringB |               Mean |             Error |            StdDev | Ratio | RatioSD |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|-------------- |-------------- |--------------------- |--------------------- |-------------------:|------------------:|------------------:|------:|--------:|-----------:|-----------:|----------:|------------:|
|      Baseline |    .NET 4.7.2 |                      |                      |         194.085 ns |         1.0771 ns |         0.9548 ns |  1.00 |    0.00 |     0.1018 |          - |         - |       321 B |
| Quickenshtein |    .NET 4.7.2 |                      |                      |          14.066 ns |         0.1245 ns |         0.0972 ns |  0.07 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      |                      |           2.699 ns |         0.0436 ns |         0.0408 ns |  0.01 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      |                      |         103.287 ns |         1.2308 ns |         1.1513 ns |  1.00 |    0.00 |     0.0764 |          - |         - |       240 B |
| Quickenshtein | .NET Core 3.0 |                      |                      |           4.872 ns |         0.1031 ns |         0.0964 ns |  0.05 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      |                      |           2.252 ns |         0.0294 ns |         0.0261 ns |  0.02 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       7,961.294 ns |        84.7393 ns |        79.2652 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |          13.991 ns |         0.1944 ns |         0.1819 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       6,924.978 ns |       118.4667 ns |       105.0177 ns | 0.871 |    0.01 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       7,028.516 ns |        78.8925 ns |        73.7961 ns | 1.000 |    0.00 |    10.2005 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |           4.865 ns |         0.0511 ns |         0.0478 ns | 0.001 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       6,958.293 ns |        95.1704 ns |        84.3661 ns | 0.991 |    0.02 |    10.0937 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       8,032.654 ns |        78.6865 ns |        73.6034 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |          14.284 ns |         0.3243 ns |         0.3033 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       6,946.254 ns |        81.8650 ns |        76.5766 ns | 0.865 |    0.01 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       7,883.666 ns |        79.4432 ns |        74.3112 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |           4.500 ns |         0.0251 ns |         0.0235 ns | 0.001 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       6,895.210 ns |       105.9834 ns |        99.1369 ns | 0.875 |    0.01 |    10.0937 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |     364,820.911 ns |     2,335.5049 ns |     2,184.6328 ns | 1.000 |    0.00 |   142.5781 |          - |         - |    452484 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |          14.402 ns |         0.1767 ns |         0.1653 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |           2.296 ns |         0.0252 ns |         0.0223 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] |                      |     156,242.492 ns |     5,025.2976 ns |     5,160.6107 ns | 1.000 |    0.00 |    90.5762 |     9.7656 |         - |    320201 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |           5.416 ns |         0.0666 ns |         0.0623 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |           2.263 ns |         0.0568 ns |         0.0474 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 373,002,460.000 ns | 2,520,829.8056 ns | 2,357,985.7642 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |      14,786.615 ns |       295.3003 ns |       290.0244 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 118,059,838.667 ns | 1,193,477.7026 ns | 1,116,379.7835 ns | 0.317 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 365,421,566.667 ns | 2,715,028.5395 ns | 2,539,639.3804 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |       5,380.852 ns |        60.8007 ns |        56.8731 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 119,075,097.333 ns |   626,349.2985 ns |   585,887.5224 ns | 0.326 |    0.00 |          - |          - |         - |     33942 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 372,817,850.000 ns | 3,672,316.2469 ns | 3,255,412.7002 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 168,510,720.000 ns | 1,360,684.3953 ns | 1,272,785.0276 ns |  0.45 |    0.01 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 124,753,578.125 ns | 2,422,100.5762 ns | 2,378,826.6296 ns |  0.33 |    0.01 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 368,838,753.333 ns | 3,706,543.4879 ns | 3,467,103.0783 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256257536 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 141,734,045.000 ns | 1,027,796.1950 ns |   961,401.1985 ns |  0.38 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 117,141,932.857 ns |   784,700.0859 ns |   695,616.1871 ns |  0.32 |    0.00 |          - |          - |         - |     32291 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |     345,366.447 ns |     4,700.2631 ns |     4,396.6290 ns | 1.000 |    0.00 |   142.5781 |          - |         - |    452484 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |          14.193 ns |         0.2418 ns |         0.2144 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |           2.243 ns |         0.0381 ns |         0.0338 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] |                      |     152,962.170 ns |     2,986.5502 ns |     3,667.7518 ns | 1.000 |    0.00 |    94.2383 |     5.6152 |         - |    320200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |           5.433 ns |         0.0818 ns |         0.0765 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |           2.242 ns |         0.0360 ns |         0.0336 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 370,269,006.667 ns | 1,719,338.6551 ns | 1,608,270.4448 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 166,542,130.357 ns | 1,195,988.2292 ns | 1,060,212.4134 ns |  0.45 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 124,353,401.333 ns | 1,830,537.6561 ns | 1,712,286.0592 ns |  0.34 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 360,406,673.333 ns | 2,507,157.4341 ns | 2,345,196.6194 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 141,427,170.000 ns | 1,078,524.0396 ns | 1,008,852.0558 ns |  0.39 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 117,482,293.333 ns | 1,201,471.6266 ns | 1,123,857.3050 ns |  0.33 |    0.00 |          - |          - |         - |     32990 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 373,590,528.571 ns | 1,651,318.9193 ns | 1,463,851.2101 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |      14,686.551 ns |       177.4364 ns |       148.1674 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 119,041,126.667 ns |   803,556.0801 ns |   751,646.8559 ns | 0.318 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 363,631,306.667 ns | 1,623,430.1374 ns | 1,518,557.5578 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |       5,414.213 ns |        54.0346 ns |        50.5440 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 119,863,869.333 ns |   948,714.9193 ns |   887,428.5241 ns | 0.330 |    0.00 |          - |          - |         - |     32272 B |