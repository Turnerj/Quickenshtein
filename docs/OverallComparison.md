# Intrinsics Performance

These benchmarks provide information on what performance is available for a supported hardware configuration.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  ShortRun : .NET Framework 4.8 (4.8.4150.0), X64 RyuJIT
```

|        Method |       Runtime | NumberOfCharacters |               Mean |              Error |            StdDev | Ratio | Speedup |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|-------------- |-------------- |------------------- |-------------------:|-------------------:|------------------:|------:|--------:|-----------:|-----------:|----------:|------------:|
|      Baseline |    .NET 4.7.2 |                  0 |         215.918 ns |          4.6618 ns |         0.2555 ns |  1.00 |    1.00 |     0.1018 |          - |         - |       321 B |
| Quickenshtein |    .NET 4.7.2 |                  0 |          13.780 ns |          0.4783 ns |         0.0262 ns |  0.06 |   15.67 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                  0 |           3.353 ns |          0.5839 ns |         0.0320 ns |  0.02 |   64.40 |          - |          - |         - |           - |
|      Baseline | .NET Core 3.0 |                  0 |         113.326 ns |         10.3518 ns |         0.5674 ns |  0.52 |    1.91 |     0.0764 |          - |         - |       240 B |
| Quickenshtein | .NET Core 3.0 |                  0 |          13.974 ns |          0.6746 ns |         0.0370 ns |  0.06 |   15.45 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                  0 |           3.480 ns |          0.7334 ns |         0.0402 ns |  0.02 |   62.04 |          - |          - |         - |           - |
|               |               |                    |                    |                    |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                 10 |       1,274.067 ns |        102.5804 ns |         5.6228 ns |  1.00 |    1.00 |     0.4463 |          - |         - |      1404 B |
| Quickenshtein |    .NET 4.7.2 |                 10 |         290.713 ns |         33.0888 ns |         1.8137 ns |  0.23 |    4.38 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                 10 |         249.906 ns |         27.9199 ns |         1.5304 ns |  0.20 |    5.10 |     0.0200 |          - |         - |        64 B |
|      Baseline | .NET Core 3.0 |                 10 |         734.049 ns |         41.0985 ns |         2.2527 ns |  0.58 |    1.74 |     0.3443 |          - |         - |      1080 B |
| Quickenshtein | .NET Core 3.0 |                 10 |         194.080 ns |         14.0232 ns |         0.7687 ns |  0.15 |    6.56 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                 10 |         263.092 ns |         34.5942 ns |         1.8962 ns |  0.21 |    4.84 |     0.0200 |          - |         - |        64 B |
|               |               |                    |                    |                    |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |                400 |     961,858.919 ns |     80,569.6043 ns |     4,416.2919 ns |  1.00 |    1.00 |   142.5781 |    59.5703 |         - |    668278 B |
| Quickenshtein |    .NET 4.7.2 |                400 |     743,454.232 ns |     74,574.6173 ns |     4,087.6864 ns |  0.77 |    1.29 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |                400 |     390,661.458 ns |     43,559.6970 ns |     2,387.6540 ns |  0.41 |    2.46 |     0.4883 |          - |         - |      1632 B |
|      Baseline | .NET Core 3.0 |                400 |     811,831.901 ns |     87,194.0804 ns |     4,779.4018 ns |  0.84 |    1.18 |   121.0938 |    60.5469 |         - |    657840 B |
| Quickenshtein | .NET Core 3.0 |                400 |     163,363.330 ns |     28,837.7977 ns |     1,580.6970 ns |  0.17 |    5.89 |          - |          - |         - |           - |
|  Fastenshtein | .NET Core 3.0 |                400 |     447,923.389 ns |     27,506.7729 ns |     1,507.7390 ns |  0.47 |    2.15 |     0.4883 |          - |         - |      1626 B |
|               |               |                    |                    |                    |                   |       |         |            |            |           |             |
|      Baseline |    .NET 4.7.2 |               8000 | 428,417,500.000 ns | 88,952,601.6317 ns | 4,875,792.2525 ns |  1.00 |    1.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256683568 B |
| Quickenshtein |    .NET 4.7.2 |               8000 | 115,440,340.000 ns | 16,113,952.5060 ns |   883,260.1110 ns |  0.27 |    3.71 |          - |          - |         - |           - |
|  Fastenshtein |    .NET 4.7.2 |               8000 | 137,741,266.667 ns | 18,941,396.2715 ns | 1,038,241.8445 ns |  0.32 |    3.11 |          - |          - |         - |     32048 B |
|      Baseline | .NET Core 3.0 |               8000 | 375,373,633.333 ns | 39,731,624.1008 ns | 2,177,824.3853 ns |  0.88 |    1.14 | 44000.0000 | 23000.0000 | 4000.0000 | 256352240 B |
| Quickenshtein | .NET Core 3.0 |               8000 |  45,687,322.222 ns |  5,503,733.3119 ns |   301,678.1943 ns |  0.11 |    9.38 |          - |          - |         - |       575 B |
|  Fastenshtein | .NET Core 3.0 |               8000 | 143,639,475.000 ns | 17,552,179.2389 ns |   962,094.1713 ns |  0.34 |    2.98 |          - |          - |         - |     32024 B |