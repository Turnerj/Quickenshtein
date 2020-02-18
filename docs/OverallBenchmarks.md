# Overall Benchmarks

These benchmarks are designed to give an overall impression of a Levenshtein distance solution.
With varied string lengths and text combinations, they can show where a solution peforms best and worst.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4121.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4121.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

## Short Text Benchmark

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |         9.662 |
|  Fastenshtein |         7.314 |

|        Method |       Runtime |    StringA |    StringB |         Mean |      Error |     StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------- |----------- |-------------:|-----------:|-----------:|------:|-------:|------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |            |            |   195.740 ns |  1.1865 ns |  1.1098 ns |  1.00 | 0.1018 |     - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |            |            |    15.475 ns |  0.2066 ns |  0.1725 ns |  0.08 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            |            |     2.241 ns |  0.0682 ns |  0.0638 ns |  0.01 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            |            |   105.939 ns |  0.9124 ns |  0.8535 ns |  1.00 | 0.0764 |     - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |            |            |    13.651 ns |  0.1773 ns |  0.1658 ns |  0.13 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            |            |     2.208 ns |  0.0452 ns |  0.0423 ns |  0.02 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | abcdefghij |   230.018 ns |  2.8817 ns |  2.5546 ns |  1.00 | 0.1144 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | abcdefghij |    15.515 ns |  0.2361 ns |  0.2209 ns |  0.07 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | abcdefghij |    15.277 ns |  0.1749 ns |  0.1636 ns |  0.07 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | abcdefghij |   110.449 ns |  1.3865 ns |  1.2970 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | abcdefghij |    13.844 ns |  0.2570 ns |  0.2404 ns |  0.13 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | abcdefghij |    15.536 ns |  0.2720 ns |  0.2544 ns |  0.14 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | jihgfedcba |   211.850 ns |  3.0182 ns |  2.6756 ns |  1.00 | 0.1147 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | jihgfedcba |    15.545 ns |  0.1252 ns |  0.1110 ns |  0.07 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | jihgfedcba |    15.340 ns |  0.1530 ns |  0.1431 ns |  0.07 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | jihgfedcba |   111.079 ns |  1.1222 ns |  1.0497 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | jihgfedcba |    13.643 ns |  0.1520 ns |  0.1269 ns |  0.12 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | jihgfedcba |    15.826 ns |  0.2929 ns |  0.2740 ns |  0.14 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij |            |   730.924 ns | 12.2063 ns | 11.4177 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij |            |    16.123 ns |  0.2240 ns |  0.2095 ns | 0.022 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij |            |     2.253 ns |  0.0717 ns |  0.0599 ns | 0.003 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij |            |   285.335 ns |  2.6090 ns |  2.1786 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij |            |    13.614 ns |  0.1338 ns |  0.1251 ns | 0.048 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij |            |     2.206 ns |  0.0396 ns |  0.0371 ns | 0.008 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | abcdefghij | 1,384.793 ns | 10.6727 ns |  9.9833 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |    29.313 ns |  0.3490 ns |  0.3265 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |   198.691 ns |  2.5870 ns |  2.4199 ns |  0.14 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | abcdefghij |   678.478 ns |  7.1475 ns |  6.6857 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |    25.362 ns |  0.1824 ns |  0.1617 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |   203.290 ns |  2.6461 ns |  2.4752 ns |  0.30 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | jihgfedcba | 1,385.150 ns | 13.3516 ns | 11.8358 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |   338.759 ns |  4.2267 ns |  3.9537 ns |  0.24 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |   239.560 ns |  2.3204 ns |  2.1705 ns |  0.17 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | jihgfedcba |   693.211 ns |  5.8551 ns |  5.1904 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |   168.395 ns |  1.3449 ns |  1.2580 ns |  0.24 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |   238.492 ns |  3.5812 ns |  3.3499 ns |  0.34 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba |            |   984.623 ns |  4.4791 ns |  4.1898 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba |            |    16.085 ns |  0.1205 ns |  0.1127 ns | 0.016 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba |            |     2.269 ns |  0.0346 ns |  0.0307 ns | 0.002 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba |            |   286.402 ns |  3.1903 ns |  2.9842 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba |            |    13.401 ns |  0.2048 ns |  0.1916 ns | 0.047 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba |            |     2.264 ns |  0.0412 ns |  0.0386 ns | 0.008 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | abcdefghij | 1,135.711 ns |  6.6567 ns |  6.2267 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |   341.170 ns |  2.8479 ns |  2.3781 ns |  0.30 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |   243.029 ns |  2.8899 ns |  2.7032 ns |  0.21 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | abcdefghij |   687.974 ns |  6.7678 ns |  6.3306 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |   161.045 ns |  1.7999 ns |  1.6836 ns |  0.23 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |   239.243 ns |  3.0669 ns |  2.7187 ns |  0.35 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | jihgfedcba | 1,139.077 ns | 12.3701 ns | 10.9658 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |    27.335 ns |  0.1215 ns |  0.1136 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |   199.568 ns |  2.6660 ns |  2.4938 ns |  0.18 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | jihgfedcba |   684.668 ns |  9.2611 ns |  8.2097 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |    24.767 ns |  0.2143 ns |  0.2004 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |   202.760 ns |  2.2859 ns |  2.1382 ns |  0.30 | 0.0203 |     - |     - |      64 B |

## Medium Text Benchmark

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |         7.241 |
|  Fastenshtein |         2.855 |

|        Method |       Runtime |             StringA |             StringB |           Mean |          Error |         StdDev | Ratio | RatioSD |    Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |-------------------- |-------------------- |---------------:|---------------:|---------------:|------:|--------:|---------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |                     |                     |     197.023 ns |      1.7118 ns |      1.6013 ns |  1.00 |    0.00 |   0.1018 |       - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |                     |                     |      17.406 ns |      0.2781 ns |      0.2601 ns |  0.09 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     |                     |       2.238 ns |      0.0379 ns |      0.0354 ns |  0.01 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     |                     |     101.540 ns |      0.8181 ns |      0.7653 ns |  1.00 |    0.00 |   0.0764 |       - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |                     |                     |      13.591 ns |      0.2820 ns |      0.2638 ns |  0.13 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     |                     |       2.215 ns |      0.0291 ns |      0.0258 ns |  0.02 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | aaba(...)bade [400] |     605.315 ns |      5.5808 ns |      4.9472 ns |  1.00 |    0.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |      23.028 ns |      0.2501 ns |      0.2339 ns |  0.04 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |     384.604 ns |      2.8927 ns |      2.7059 ns |  0.64 |    0.01 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | aaba(...)bade [400] |     479.039 ns |      5.1389 ns |      4.8069 ns |  1.00 |    0.00 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |      13.936 ns |      0.0822 ns |      0.0729 ns |  0.03 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |     338.406 ns |      6.5874 ns |      6.1618 ns |  0.71 |    0.02 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     634.821 ns |      4.6708 ns |      4.3691 ns |  1.00 |    0.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |      15.420 ns |      0.2342 ns |      0.2191 ns |  0.02 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     385.320 ns |      5.1582 ns |      4.5726 ns |  0.61 |    0.01 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     477.282 ns |      4.1545 ns |      3.6828 ns |  1.00 |    0.00 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |      13.986 ns |      0.1742 ns |      0.1629 ns |  0.03 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     337.712 ns |      6.2462 ns |      5.8427 ns |  0.71 |    0.02 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] |                     |  17,425.651 ns |    233.5566 ns |    218.4690 ns | 1.000 |    0.00 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |      15.960 ns |      0.2147 ns |      0.2008 ns | 0.001 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |       2.331 ns |      0.0688 ns |      0.0610 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] |                     |   6,885.818 ns |     63.6982 ns |     59.5834 ns | 1.000 |    0.00 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |      13.344 ns |      0.1814 ns |      0.1697 ns | 0.002 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |       2.203 ns |      0.0492 ns |      0.0411 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 729,674.499 ns |  6,787.7264 ns |  6,349.2435 ns | 1.000 |    0.00 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] |     355.110 ns |      4.8682 ns |      4.5537 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 283,061.891 ns |  3,996.0984 ns |  3,737.9529 ns | 0.388 |    0.01 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 670,330.378 ns |  6,699.6008 ns |  6,266.8107 ns | 1.000 |    0.00 | 121.0938 | 60.5469 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] |      37.848 ns |      0.1176 ns |      0.1042 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 273,922.562 ns |  3,285.7310 ns |  3,073.4748 ns | 0.409 |    0.01 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 796,480.931 ns | 11,127.4097 ns | 10,408.5860 ns |  1.00 |    0.00 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 721,459.648 ns |  6,840.2613 ns |  6,398.3846 ns |  0.91 |    0.02 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 379,693.929 ns |  5,322.3761 ns |  4,978.5539 ns |  0.48 |    0.01 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 763,913.190 ns | 11,322.1042 ns | 10,590.7033 ns |  1.00 |    0.00 | 120.1172 | 59.5703 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 115,824.089 ns |  1,348.7765 ns |  1,053.0360 ns |  0.15 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 394,268.281 ns |  5,374.2455 ns |  5,027.0726 ns |  0.52 |    0.01 |   0.4883 |       - |     - |    1626 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |  17,462.321 ns |    203.9125 ns |    190.7399 ns | 1.000 |    0.00 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |      24.891 ns |      0.2231 ns |      0.2087 ns | 0.001 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |       2.247 ns |      0.0376 ns |      0.0352 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] |                     |   6,558.226 ns |    104.5617 ns |     97.8071 ns | 1.000 |    0.00 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |      13.609 ns |      0.1689 ns |      0.1411 ns | 0.002 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |       2.197 ns |      0.0482 ns |      0.0451 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 783,196.875 ns |  5,738.9296 ns |  5,368.1983 ns |  1.00 |    0.00 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 728,991.296 ns | 10,615.6260 ns |  9,929.8631 ns |  0.93 |    0.02 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 379,858.089 ns |  3,026.7450 ns |  2,831.2191 ns |  0.49 |    0.01 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 763,283.975 ns |  6,392.3526 ns |  5,979.4106 ns |  1.00 |    0.00 | 121.0938 | 60.5469 |     - |  657845 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 115,810.563 ns |  1,134.6754 ns |  1,005.8602 ns |  0.15 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 384,485.052 ns |  6,287.4124 ns |  5,881.2494 ns |  0.50 |    0.01 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 722,671.094 ns |  5,506.6038 ns |  5,150.8806 ns | 1.000 |    0.00 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |     359.816 ns |      3.4561 ns |      2.8860 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 290,177.669 ns |  2,944.2092 ns |  2,754.0152 ns | 0.402 |    0.00 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |                |                |       |         |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 663,216.370 ns |  7,158.3374 ns |  6,695.9132 ns | 1.000 |    0.00 | 121.0938 | 60.5469 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |      42.361 ns |      0.4857 ns |      0.4544 ns | 0.000 |    0.00 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 275,902.041 ns |  2,837.0661 ns |  2,653.7934 ns | 0.416 |    0.01 |   0.4883 |       - |     - |    1626 B |

## Long Text Comparison

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |        17.176 |
|  Fastenshtein |         2.978 |

|        Method |       Runtime |              StringA |              StringB |               Mean |             Error |            StdDev | Ratio | RatioSD |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|-------------- |-------------- |--------------------- |--------------------- |-------------------:|------------------:|------------------:|------:|--------:|-----------:|-----------:|----------:|------------:|
|      Baseline |    .NET 4.7.2 |                      |                      |         196.816 ns |         1.9785 ns |         1.6521 ns |  1.00 |    0.00 |     0.1018 |          - |         - |       321 B |
| Quickenshtein |    .NET 4.7.2 |                      |                      |          15.708 ns |         0.2174 ns |         0.2033 ns |  0.08 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      |                      |           2.329 ns |         0.0769 ns |         0.0642 ns |  0.01 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      |                      |         102.702 ns |         1.2210 ns |         1.1421 ns |  1.00 |    0.00 |     0.0764 |          - |         - |       240 B |
| Quickenshtein | .NET Core 3.0 |                      |                      |          13.511 ns |         0.1152 ns |         0.1078 ns |  0.13 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      |                      |           2.925 ns |         0.0406 ns |         0.0380 ns |  0.03 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       8,074.978 ns |       147.0423 ns |       137.5435 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |          17.688 ns |         0.2402 ns |         0.2247 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       6,978.405 ns |        73.7075 ns |        65.3398 ns | 0.866 |    0.02 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       7,112.579 ns |        81.4836 ns |        76.2198 ns | 1.000 |    0.00 |    10.2005 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |          13.577 ns |         0.2074 ns |         0.1940 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       7,043.130 ns |        66.3289 ns |        62.0441 ns | 0.990 |    0.01 |    10.0937 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       8,103.947 ns |       161.8313 ns |       143.4592 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |          15.595 ns |         0.2691 ns |         0.2517 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       6,973.487 ns |        57.2032 ns |        53.5079 ns | 0.861 |    0.02 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       7,988.915 ns |       166.6674 ns |       155.9008 ns | 1.000 |    0.00 |    10.1929 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |          13.337 ns |         0.2232 ns |         0.2087 ns | 0.002 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       6,193.093 ns |        74.0674 ns |        69.2827 ns | 0.776 |    0.02 |    10.1013 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |     338,494.183 ns |     6,950.6189 ns |     6,501.6132 ns | 1.000 |    0.00 |   142.5781 |          - |         - |    452484 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |          19.685 ns |         0.2563 ns |         0.2398 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |           2.669 ns |         0.0646 ns |         0.0604 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] |                      |     154,817.900 ns |     2,899.1411 ns |     2,847.3442 ns | 1.000 |    0.00 |    95.9473 |     4.3945 |         - |    320202 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |          13.802 ns |         0.1482 ns |         0.1314 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |           2.295 ns |         0.0647 ns |         0.0606 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 371,687,828.571 ns | 3,001,669.3388 ns | 2,660,901.6845 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |       6,704.293 ns |        84.6719 ns |        75.0594 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 117,494,400.000 ns | 1,387,491.7796 ns | 1,297,860.6715 ns | 0.316 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 365,743,780.000 ns | 4,699,600.8790 ns | 4,396,009.5780 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |         535.536 ns |         7.0728 ns |         6.6159 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 100,187,800.000 ns |   606,781.6514 ns |   537,896.1292 ns | 0.274 |    0.00 |          - |          - |         - |     32829 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 371,757,450.000 ns | 2,739,361.6515 ns | 2,428,372.7520 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 105,897,741.333 ns | 1,343,981.0622 ns | 1,257,160.7195 ns |  0.28 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 124,382,023.333 ns |   877,244.0641 ns |   820,574.6419 ns |  0.33 |    0.00 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 356,313,906.667 ns | 5,260,450.7722 ns | 4,920,628.9162 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] |  46,681,844.242 ns |   681,397.4159 ns |   637,379.5656 ns |  0.13 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 115,847,282.667 ns | 1,295,961.4801 ns | 1,212,243.1725 ns |  0.33 |    0.01 |          - |          - |         - |     32990 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |     553,788.197 ns |     5,876.6344 ns |     5,497.0074 ns | 1.000 |    0.00 |   142.5781 |          - |         - |    452488 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |          16.218 ns |         0.1996 ns |         0.1770 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |           2.280 ns |         0.0421 ns |         0.0373 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] |                      |     150,890.346 ns |     2,881.3464 ns |     2,958.9306 ns | 1.000 |    0.00 |    94.4824 |     5.1270 |         - |    320201 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |          13.718 ns |         0.1959 ns |         0.1833 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |           2.147 ns |         0.0376 ns |         0.0352 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 368,561,258.333 ns | 3,948,644.2839 ns | 3,082,841.7261 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 106,214,789.333 ns | 1,319,035.4994 ns | 1,233,826.6245 ns |  0.29 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 124,291,400.000 ns | 1,899,951.5292 ns | 1,777,215.8392 ns |  0.34 |    0.01 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 360,519,780.000 ns | 4,399,574.2678 ns | 4,115,364.4998 ns |  1.00 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] |  47,307,625.455 ns |   399,505.2949 ns |   373,697.5007 ns |  0.13 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 115,781,706.154 ns |   853,302.2128 ns |   712,545.9939 ns |  0.32 |    0.01 |          - |          - |         - |     32291 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 370,644,760.000 ns | 2,930,612.1506 ns | 2,741,296.4240 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |       6,659.875 ns |        89.8308 ns |        84.0277 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 117,084,244.000 ns | 1,357,318.8050 ns | 1,269,636.8523 ns | 0.316 |    0.01 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |         |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 360,076,193.333 ns | 4,325,400.7985 ns | 4,045,982.5906 ns | 1.000 |    0.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |         534.866 ns |         5.6158 ns |         4.9783 ns | 0.000 |    0.00 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 102,655,793.333 ns |   964,573.5569 ns |   902,262.7035 ns | 0.285 |    0.00 |          - |          - |         - |     33942 B |