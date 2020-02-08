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
| Quickenshtein |        19.355 |
|  Fastenshtein |         7.020 |

|        Method |       Runtime |    StringA |    StringB |         Mean |      Error |     StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------- |----------- |-------------:|-----------:|-----------:|------:|-------:|------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |            |            |   195.485 ns |  1.1929 ns |  1.1159 ns |  1.00 | 0.1018 |     - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |            |            |    12.119 ns |  0.0929 ns |  0.0869 ns |  0.06 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            |            |     2.332 ns |  0.0975 ns |  0.0912 ns |  0.01 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            |            |   103.726 ns |  0.6590 ns |  0.6164 ns |  1.00 | 0.0764 |     - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |            |            |    13.908 ns |  0.1638 ns |  0.1532 ns |  0.13 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            |            |     2.839 ns |  0.0232 ns |  0.0217 ns |  0.03 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | abcdefghij |   208.475 ns |  2.1741 ns |  2.0337 ns |  1.00 | 0.1147 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | abcdefghij |    12.056 ns |  0.1008 ns |  0.0942 ns |  0.06 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | abcdefghij |    15.226 ns |  0.1618 ns |  0.1514 ns |  0.07 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | abcdefghij |   113.180 ns |  1.3419 ns |  1.1896 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | abcdefghij |    13.880 ns |  0.1430 ns |  0.1337 ns |  0.12 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | abcdefghij |    15.392 ns |  0.2352 ns |  0.2200 ns |  0.14 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 |            | jihgfedcba |   214.273 ns |  2.0945 ns |  1.9592 ns |  1.00 | 0.1147 |     - |     - |     361 B |
| Quickenshtein |    .NET 4.7.2 |            | jihgfedcba |    12.040 ns |  0.1070 ns |  0.1001 ns |  0.06 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |            | jihgfedcba |    15.073 ns |  0.1150 ns |  0.1020 ns |  0.07 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 |            | jihgfedcba |   110.830 ns |  1.3424 ns |  1.1900 ns |  1.00 | 0.0892 |     - |     - |     280 B |
| Quickenshtein | .NET Core 3.0 |            | jihgfedcba |    14.025 ns |  0.1535 ns |  0.1436 ns |  0.13 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |            | jihgfedcba |    15.207 ns |  0.1832 ns |  0.1713 ns |  0.14 | 0.0204 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij |            |   772.483 ns | 15.3810 ns | 28.1251 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij |            |    12.723 ns |  0.1627 ns |  0.1522 ns | 0.017 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij |            |     2.379 ns |  0.0385 ns |  0.0321 ns | 0.003 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij |            |   308.854 ns |  4.0147 ns |  3.7553 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij |            |    14.588 ns |  0.1345 ns |  0.1050 ns | 0.047 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij |            |     2.934 ns |  0.0681 ns |  0.0637 ns | 0.010 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | abcdefghij | 1,219.534 ns | 19.2729 ns | 18.0279 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |    21.440 ns |  0.4577 ns |  0.6110 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | abcdefghij |   204.606 ns |  1.9757 ns |  1.8480 ns |  0.17 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | abcdefghij |   704.014 ns | 11.4350 ns | 10.6963 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |    23.488 ns |  0.2208 ns |  0.2065 ns |  0.03 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | abcdefghij |   207.616 ns |  2.0881 ns |  1.9532 ns |  0.29 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | abcdefghij | jihgfedcba | 1,157.937 ns | 14.1175 ns | 13.2055 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |    35.224 ns |  0.3628 ns |  0.3216 ns |  0.03 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | abcdefghij | jihgfedcba |   248.582 ns |  2.2194 ns |  2.0760 ns |  0.21 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | abcdefghij | jihgfedcba |   704.702 ns | 10.0087 ns |  9.3621 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |    29.269 ns |  0.2889 ns |  0.2413 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | abcdefghij | jihgfedcba |   246.302 ns |  2.3378 ns |  2.1868 ns |  0.35 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba |            |   744.097 ns |  8.5920 ns |  8.0370 ns | 1.000 | 0.3052 |     - |     - |     963 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba |            |    12.324 ns |  0.1317 ns |  0.1232 ns | 0.017 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba |            |     2.304 ns |  0.0363 ns |  0.0339 ns | 0.003 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba |            |   290.342 ns |  2.6366 ns |  2.3372 ns | 1.000 | 0.2036 |     - |     - |     640 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba |            |    14.151 ns |  0.1383 ns |  0.1294 ns | 0.049 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba |            |     2.300 ns |  0.0487 ns |  0.0456 ns | 0.008 |      - |     - |     - |         - |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | abcdefghij | 1,160.764 ns |  5.9749 ns |  5.2966 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |    35.399 ns |  0.2721 ns |  0.2545 ns |  0.03 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | abcdefghij |   250.473 ns |  2.6240 ns |  2.4545 ns |  0.22 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | abcdefghij |   698.769 ns |  8.7612 ns |  8.1953 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |    29.337 ns |  0.3301 ns |  0.3088 ns |  0.04 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | abcdefghij |   248.950 ns |  1.5263 ns |  1.4277 ns |  0.36 | 0.0200 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline |    .NET 4.7.2 | jihgfedcba | jihgfedcba | 1,157.654 ns | 11.0095 ns | 10.2983 ns |  1.00 | 0.4463 |     - |     - |    1404 B |
| Quickenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |    21.033 ns |  0.1378 ns |  0.1289 ns |  0.02 |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | jihgfedcba | jihgfedcba |   202.977 ns |  2.0278 ns |  1.8968 ns |  0.18 | 0.0203 |     - |     - |      64 B |
|               |               |            |            |              |            |            |       |        |       |       |           |
|      Baseline | .NET Core 3.0 | jihgfedcba | jihgfedcba |   703.408 ns |  8.5526 ns |  8.0001 ns |  1.00 | 0.3443 |     - |     - |    1080 B |
| Quickenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |    24.527 ns |  0.2160 ns |  0.2021 ns |  0.03 |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | jihgfedcba | jihgfedcba |   209.000 ns |  1.1925 ns |  1.0571 ns |  0.30 | 0.0203 |     - |     - |      64 B |

## Medium Text Benchmark

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |        58.065 |
|  Fastenshtein |         2.873 |

|        Method |       Runtime |             StringA |             StringB |           Mean |         Error |        StdDev | Ratio |    Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |-------------------- |-------------------- |---------------:|--------------:|--------------:|------:|---------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 |                     |                     |     196.969 ns |     1.6119 ns |     1.5078 ns |  1.00 |   0.1018 |       - |     - |     321 B |
| Quickenshtein |    .NET 4.7.2 |                     |                     |      12.134 ns |     0.0611 ns |     0.0541 ns |  0.06 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     |                     |       2.294 ns |     0.0309 ns |     0.0289 ns |  0.01 |        - |       - |     - |         - |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     |                     |     103.080 ns |     1.0849 ns |     1.0148 ns |  1.00 |   0.0764 |       - |     - |     240 B |
| Quickenshtein | .NET Core 3.0 |                     |                     |      14.234 ns |     0.1446 ns |     0.1353 ns |  0.14 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     |                     |       2.827 ns |     0.0395 ns |     0.0369 ns |  0.03 |        - |       - |     - |         - |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | aaba(...)bade [400] |     609.703 ns |     6.6105 ns |     6.1834 ns |  1.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |      12.119 ns |     0.0613 ns |     0.0573 ns |  0.02 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | aaba(...)bade [400] |     382.236 ns |     4.4630 ns |     4.1747 ns |  0.63 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | aaba(...)bade [400] |     509.854 ns |     5.1962 ns |     4.8606 ns |  1.00 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |      14.109 ns |     0.1601 ns |     0.1498 ns |  0.03 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | aaba(...)bade [400] |     339.769 ns |     4.5968 ns |     4.2999 ns |  0.67 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     611.434 ns |     7.9768 ns |     7.0712 ns |  1.00 |   0.6113 |       - |     - |    1926 B |
| Quickenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |      12.096 ns |     0.1153 ns |     0.1022 ns |  0.02 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                     | bbeb(...)aabe [400] |     378.744 ns |     3.2227 ns |     2.8568 ns |  0.62 |   0.5174 |       - |     - |    1629 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     471.262 ns |     3.7827 ns |     3.3533 ns |  1.00 |   0.5865 |       - |     - |    1840 B |
| Quickenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |      14.019 ns |     0.1426 ns |     0.1333 ns |  0.03 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                     | bbeb(...)aabe [400] |     338.351 ns |     3.6283 ns |     3.3939 ns |  0.72 |   0.5174 |       - |     - |    1624 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] |                     |  17,908.040 ns |   195.2200 ns |   182.6090 ns | 1.000 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |      12.151 ns |     0.1431 ns |     0.1338 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] |                     |       2.287 ns |     0.0428 ns |     0.0400 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] |                     |   6,606.397 ns |    60.6936 ns |    56.7729 ns | 1.000 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |      13.846 ns |     0.1269 ns |     0.1187 ns | 0.002 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] |                     |       2.265 ns |     0.0418 ns |     0.0391 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 730,189.779 ns | 5,636.4903 ns | 5,272.3766 ns | 1.000 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] |     360.052 ns |     1.7219 ns |     1.6106 ns | 0.000 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | aaba(...)bade [400] | 293,678.240 ns | 3,117.0113 ns | 2,763.1493 ns | 0.402 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 676,741.204 ns | 6,301.9943 ns | 5,894.8894 ns | 1.000 | 121.0938 | 59.5703 |     - |  657848 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] |      39.263 ns |     0.4469 ns |     0.4181 ns | 0.000 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | aaba(...)bade [400] | 280,389.229 ns | 3,053.9643 ns | 2,856.6801 ns | 0.414 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 802,033.887 ns | 7,295.3281 ns | 6,824.0545 ns | 1.000 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] |     711.868 ns |     5.0607 ns |     4.7337 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | aaba(...)bade [400] | bbeb(...)aabe [400] | 390,636.569 ns | 3,753.2346 ns | 3,510.7780 ns | 0.487 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 772,281.393 ns | 5,195.9701 ns | 4,860.3137 ns | 1.000 | 121.0938 | 60.5469 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] |     417.380 ns |     1.3937 ns |     1.2355 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | aaba(...)bade [400] | bbeb(...)aabe [400] | 348,422.861 ns | 2,634.5377 ns | 2,464.3482 ns | 0.451 |   0.4883 |       - |     - |    1624 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |  17,789.744 ns |   164.6874 ns |   154.0487 ns | 1.000 |   7.8430 |       - |     - |   24689 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |      12.134 ns |     0.1348 ns |     0.1261 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] |                     |       2.323 ns |     0.0307 ns |     0.0287 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] |                     |   6,600.206 ns |    52.9472 ns |    49.5269 ns | 1.000 |   5.1727 |       - |     - |   16240 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |      13.932 ns |     0.1480 ns |     0.1384 ns | 0.002 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] |                     |       2.271 ns |     0.0335 ns |     0.0297 ns | 0.000 |        - |       - |     - |         - |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 795,002.018 ns | 5,723.7642 ns | 5,354.0126 ns | 1.000 | 138.6719 | 55.6641 |     - |  668316 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] |     710.511 ns |     4.6377 ns |     3.6208 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | aaba(...)bade [400] | 385,848.630 ns | 3,554.9801 ns | 3,325.3306 ns | 0.485 |   0.4883 |       - |     - |    1632 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 761,051.712 ns | 7,603.3728 ns | 7,112.1996 ns | 1.000 | 122.0703 | 60.5469 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] |     419.557 ns |     2.6098 ns |     2.4412 ns | 0.001 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | aaba(...)bade [400] | 388,320.096 ns | 2,141.7485 ns | 1,898.6043 ns | 0.510 |   0.4883 |       - |     - |    1625 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 733,527.448 ns | 5,662.5627 ns | 5,296.7647 ns | 1.000 | 139.6484 | 56.6406 |     - |  668257 B |
| Quickenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |     358.921 ns |     2.3036 ns |     2.1548 ns | 0.000 |        - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 293,684.290 ns | 4,452.0701 ns | 4,164.4692 ns | 0.400 |   0.4883 |       - |     - |    1633 B |
|               |               |                     |                     |                |               |               |       |          |         |       |           |
|      Baseline | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 672,673.021 ns | 6,745.2998 ns | 6,309.5576 ns | 1.000 | 120.1172 | 59.5703 |     - |  657841 B |
| Quickenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] |      43.663 ns |     0.2282 ns |     0.2023 ns | 0.000 |        - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | bbeb(...)aabe [400] | bbeb(...)aabe [400] | 294,220.428 ns | 2,356.5661 ns | 2,089.0345 ns | 0.437 |   0.4883 |       - |     - |    1625 B |

## Long Text Comparison

|          Name | Avg. Speed Up |
|-------------- |--------------:|
|      Baseline |         1.000 |
| Quickenshtein |        86.957 |
|  Fastenshtein |         2.927 |

|        Method |       Runtime |              StringA |              StringB |               Mean |             Error |            StdDev | Ratio |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|-------------- |-------------- |--------------------- |--------------------- |-------------------:|------------------:|------------------:|------:|-----------:|-----------:|----------:|------------:|
|      Baseline |    .NET 4.7.2 |                      |                      |         195.907 ns |         1.0593 ns |         0.9390 ns |  1.00 |     0.1018 |          - |         - |       321 B |
| Quickenshtein |    .NET 4.7.2 |                      |                      |          12.273 ns |         0.1064 ns |         0.0995 ns |  0.06 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      |                      |           2.286 ns |         0.0305 ns |         0.0285 ns |  0.01 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      |                      |         102.179 ns |         0.9315 ns |         0.8714 ns |  1.00 |     0.0764 |          - |         - |       240 B |
| Quickenshtein | .NET Core 3.0 |                      |                      |          14.178 ns |         0.0932 ns |         0.0778 ns |  0.14 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      |                      |           2.270 ns |         0.0278 ns |         0.0232 ns |  0.02 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       8,183.797 ns |        83.6001 ns |        78.1996 ns | 1.000 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |          12.300 ns |         0.1357 ns |         0.1269 ns | 0.002 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | aabc(...)ecba [7999] |       6,932.085 ns |        43.4210 ns |        40.6160 ns | 0.847 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       7,975.368 ns |        59.9921 ns |        53.1814 ns | 1.000 |    10.1929 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |          14.422 ns |         0.1054 ns |         0.0986 ns | 0.002 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | aabc(...)ecba [7999] |       6,938.799 ns |       106.5959 ns |        99.7099 ns | 0.870 |    10.0937 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       8,192.034 ns |       160.1474 ns |       149.8020 ns | 1.000 |    10.1929 |          - |         - |     32382 B |
| Quickenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |          12.281 ns |         0.0938 ns |         0.0832 ns | 0.001 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                      | babd(...)cbaa [7999] |       6,922.213 ns |        85.6166 ns |        80.0858 ns | 0.845 |    10.0937 |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       7,124.061 ns |        55.6556 ns |        52.0603 ns | 1.000 |    10.2005 |          - |         - |     32232 B |
| Quickenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |          14.384 ns |         0.0857 ns |         0.0760 ns | 0.002 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                      | babd(...)cbaa [7999] |       6,966.830 ns |        80.9325 ns |        75.7043 ns | 0.978 |    10.0937 |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |     352,827.158 ns |     2,672.1877 ns |     2,499.5660 ns | 1.000 |   142.5781 |          - |         - |    452484 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |          12.281 ns |         0.1051 ns |         0.0983 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] |                      |           2.299 ns |         0.0335 ns |         0.0313 ns | 0.000 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] |                      |     155,691.965 ns |     3,044.8926 ns |     2,990.4917 ns | 1.000 |    97.6563 |     2.1973 |         - |    320200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |          14.173 ns |         0.1212 ns |         0.1134 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] |                      |           2.322 ns |         0.0358 ns |         0.0335 ns | 0.000 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 375,471,893.333 ns | 3,436,040.9082 ns | 3,214,074.7974 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |       6,812.940 ns |        43.9773 ns |        38.9847 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 119,838,486.667 ns |   985,516.4006 ns |   921,852.6526 ns | 0.319 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 368,348,353.333 ns | 4,195,417.1705 ns | 3,924,395.8243 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] |         542.484 ns |         4.5727 ns |         4.2773 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | aabc(...)ecba [7999] | 101,930,517.333 ns |   761,471.1837 ns |   712,280.6177 ns | 0.277 |          - |          - |         - |     32291 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 376,106,766.667 ns | 2,816,811.6194 ns | 2,634,847.3365 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] |      14,159.215 ns |       102.9645 ns |        96.3131 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 126,812,776.667 ns | 1,613,556.3759 ns | 1,509,321.6351 ns | 0.337 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 362,382,873.333 ns | 4,457,290.1672 ns | 4,169,351.9879 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] |       7,788.833 ns |        75.6812 ns |        70.7922 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | aabc(...)ecba [7999] | babd(...)cbaa [7999] | 117,562,413.333 ns |   899,482.2147 ns |   841,376.2217 ns | 0.324 |          - |          - |         - |     32272 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |     348,211.803 ns |     2,515.3036 ns |     2,352.8165 ns | 1.000 |   142.5781 |          - |         - |    452484 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |          12.306 ns |         0.2042 ns |         0.1810 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] |                      |           2.324 ns |         0.0389 ns |         0.0364 ns | 0.000 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] |                      |     154,682.648 ns |     2,781.9207 ns |     2,171.9407 ns | 1.000 |    97.1680 |     2.9297 |         - |    320200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |          14.218 ns |         0.1773 ns |         0.1659 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] |                      |           2.300 ns |         0.0523 ns |         0.0489 ns | 0.000 |          - |          - |         - |           - |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 374,821,614.286 ns | 2,207,818.2156 ns | 1,957,173.3412 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] |      14,112.471 ns |       115.6910 ns |       108.2175 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 126,876,391.667 ns |   855,851.6234 ns |   800,564.1396 ns | 0.338 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 371,233,033.333 ns | 3,494,544.7220 ns | 3,268,799.3011 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] |       7,782.919 ns |        48.4824 ns |        45.3505 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | aabc(...)ecba [7999] | 126,170,493.333 ns |   876,957.3228 ns |   820,306.4238 ns | 0.340 |          - |          - |         - |     32024 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 379,881,187.500 ns | 7,224,072.6931 ns | 7,095,005.3293 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256587504 B |
| Quickenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |       6,792.866 ns |        45.2057 ns |        42.2855 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 120,436,213.333 ns | 1,041,136.5580 ns |   973,879.7824 ns | 0.317 |          - |          - |         - |     32048 B |
|               |               |                      |                      |                    |                   |                   |       |            |            |           |             |
|      Baseline | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 366,779,313.333 ns | 4,033,100.7016 ns | 3,772,564.8986 ns | 1.000 | 44000.0000 | 23000.0000 | 4000.0000 | 256256200 B |
| Quickenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] |         536.652 ns |         5.2443 ns |         4.9055 ns | 0.000 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 | babd(...)cbaa [7999] | babd(...)cbaa [7999] | 120,461,665.333 ns | 1,160,367.5312 ns | 1,085,408.5086 ns | 0.328 |          - |          - |         - |     32272 B |