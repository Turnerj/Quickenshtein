# Overall Performance

These benchmarks provide a general overview of how fast Quickenshtein performs compared to other solutions.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]   : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  ShortRun : .NET Framework 4.8 (4.8.4150.0), X64 RyuJIT
```

|                 Method |       Runtime | NumberOfCharacters |               Mean |              Error |            StdDev | Ratio | Speedup |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|----------------------- |-------------- |------------------- |-------------------:|-------------------:|------------------:|------:|--------:|-----------:|-----------:|----------:|------------:|
|               Baseline |    .NET 4.7.2 |                  0 |         206.784 ns |         14.5462 ns |         0.7973 ns | 1.000 |    1.00 |     0.1018 |          - |         - |       321 B |
|          Quickenshtein |    .NET 4.7.2 |                  0 |           2.774 ns |          0.8699 ns |         0.0477 ns | 0.013 |   74.56 |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                  0 |           1.371 ns |          0.5710 ns |         0.0313 ns | 0.007 |  150.91 |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                  0 |           2.363 ns |          0.7050 ns |         0.0386 ns | 0.011 |   87.53 |          - |          - |         - |           - |
|               Baseline | .NET Core 3.0 |                  0 |         108.814 ns |          8.1936 ns |         0.4491 ns | 0.526 |    1.90 |     0.0764 |          - |         - |       240 B |
|          Quickenshtein | .NET Core 3.0 |                  0 |           3.909 ns |          0.6915 ns |         0.0379 ns | 0.019 |   52.90 |          - |          - |         - |           - |
| Quickenshtein_Threaded | .NET Core 3.0 |                  0 |           1.792 ns |          0.2613 ns |         0.0143 ns | 0.009 |  115.40 |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 3.0 |                  0 |           2.304 ns |          0.5456 ns |         0.0299 ns | 0.011 |   89.74 |          - |          - |         - |           - |
|                        |               |                    |                    |                    |                   |       |         |            |            |           |             |
|               Baseline |    .NET 4.7.2 |                 10 |       1,198.867 ns |        155.1287 ns |         8.5031 ns |  1.00 |    1.00 |     0.4463 |          - |         - |      1404 B |
|          Quickenshtein |    .NET 4.7.2 |                 10 |         228.140 ns |         28.9589 ns |         1.5873 ns |  0.19 |    5.26 |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                 10 |         222.873 ns |         27.8674 ns |         1.5275 ns |  0.19 |    5.38 |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                 10 |         239.509 ns |         21.0814 ns |         1.1555 ns |  0.20 |    5.01 |     0.0200 |          - |         - |        64 B |
|               Baseline | .NET Core 3.0 |                 10 |         681.953 ns |         55.4929 ns |         3.0418 ns |  0.57 |    1.76 |     0.3443 |          - |         - |      1080 B |
|          Quickenshtein | .NET Core 3.0 |                 10 |         166.341 ns |         27.1907 ns |         1.4904 ns |  0.14 |    7.21 |          - |          - |         - |           - |
| Quickenshtein_Threaded | .NET Core 3.0 |                 10 |         159.929 ns |         35.7616 ns |         1.9602 ns |  0.13 |    7.50 |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 3.0 |                 10 |         250.140 ns |         16.5498 ns |         0.9072 ns |  0.21 |    4.79 |     0.0200 |          - |         - |        64 B |
|                        |               |                    |                    |                    |                   |       |         |            |            |           |             |
|               Baseline |    .NET 4.7.2 |                400 |     918,052.767 ns |    168,835.9710 ns |     9,254.4693 ns |  1.00 |    1.00 |   142.5781 |    59.5703 |         - |    668278 B |
|          Quickenshtein |    .NET 4.7.2 |                400 |     702,502.051 ns |     47,628.5602 ns |     2,610.6821 ns |  0.77 |    1.31 |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                400 |     699,643.783 ns |    112,474.2660 ns |     6,165.0940 ns |  0.76 |    1.31 |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                400 |     372,251.400 ns |     15,749.1648 ns |       863.2649 ns |  0.41 |    2.47 |     0.4883 |          - |         - |      1632 B |
|               Baseline | .NET Core 3.0 |                400 |     812,294.401 ns |     46,583.8706 ns |     2,553.4191 ns |  0.88 |    1.13 |   121.0938 |    60.5469 |         - |    657840 B |
|          Quickenshtein | .NET Core 3.0 |                400 |     155,662.858 ns |     25,755.2152 ns |     1,411.7303 ns |  0.17 |    5.90 |          - |          - |         - |         1 B |
| Quickenshtein_Threaded | .NET Core 3.0 |                400 |     154,251.579 ns |     24,880.2958 ns |     1,363.7730 ns |  0.17 |    5.95 |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 3.0 |                400 |     429,392.025 ns |     57,973.6077 ns |     3,177.7291 ns |  0.47 |    2.14 |     0.4883 |          - |         - |      1626 B |
|                        |               |                    |                    |                    |                   |       |         |            |            |           |             |
|               Baseline |    .NET 4.7.2 |               8000 | 407,072,400.000 ns | 29,684,353.6933 ns | 1,627,099.5913 ns |  1.00 |    1.00 | 44000.0000 | 23000.0000 | 4000.0000 | 256683568 B |
|          Quickenshtein |    .NET 4.7.2 |               8000 | 110,685,573.333 ns | 10,111,751.3824 ns |   554,259.2138 ns |  0.27 |    3.68 |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |               8000 |  36,600,807.692 ns | 16,120,782.0907 ns |   883,634.4635 ns |  0.09 |   11.13 |          - |          - |         - |      1260 B |
|           Fastenshtein |    .NET 4.7.2 |               8000 | 131,106,050.000 ns |  9,199,908.6675 ns |   504,278.0378 ns |  0.32 |    3.10 |          - |          - |         - |     32048 B |
|               Baseline | .NET Core 3.0 |               8000 | 360,192,066.667 ns |  9,619,883.1888 ns |   527,298.2584 ns |  0.88 |    1.13 | 44000.0000 | 23000.0000 | 4000.0000 | 256352240 B |
|          Quickenshtein | .NET Core 3.0 |               8000 |  42,957,902.778 ns |    153,604.6344 ns |     8,419.5883 ns |  0.11 |    9.48 |          - |          - |         - |       841 B |
| Quickenshtein_Threaded | .NET Core 3.0 |               8000 |  42,946,722.222 ns |  1,535,381.4757 ns |    84,159.4396 ns |  0.11 |    9.48 |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 3.0 |               8000 | 138,896,816.667 ns |  1,212,345.1606 ns |    66,452.7291 ns |  0.34 |    2.93 |          - |          - |         - |     32024 B |