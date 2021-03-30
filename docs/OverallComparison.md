# Overall Performance

These benchmarks provide a general overview of how fast Quickenshtein performs compared to other solutions.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.867 (2004/?/20H1)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.201
  [Host]   : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT
  ShortRun : .NET Framework 4.8 (4.8.4341.0), X64 RyuJIT

Job=ShortRun  IterationCount=3  LaunchCount=1
WarmupCount=3
```

|                 Method |       Runtime | NumberOfCharacters |             Mean |             Error |          StdDev |          Op/s | Ratio | Speedup | RatioSD | Code Size |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|----------------------- |-------------- |------------------- |-----------------:|------------------:|----------------:|--------------:|------:|--------:|--------:|----------:|-----------:|-----------:|----------:|------------:|
|               Baseline |    .NET 4.7.2 |                 10 |       1,088.8 ns |          77.45 ns |         4.25 ns |   918,426.326 |  1.00 |    1.00 |    0.00 |    2559 B |     0.4463 |          - |         - |      1404 B |
|          Quickenshtein |    .NET 4.7.2 |                 10 |         198.7 ns |          76.62 ns |         4.20 ns | 5,033,786.976 |  0.18 |    5.48 |    0.00 |    3705 B |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                 10 |         199.8 ns |           7.34 ns |         0.40 ns | 5,005,774.603 |  0.18 |    5.45 |    0.00 |    4082 B |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                 10 |         208.4 ns |          28.58 ns |         1.57 ns | 4,798,365.974 |  0.19 |    5.22 |    0.00 |     324 B |     0.0203 |          - |         - |        64 B |
|               Baseline | .NET Core 5.0 |                 10 |         563.7 ns |          86.92 ns |         4.76 ns | 1,773,886.044 |  0.52 |    1.93 |    0.00 |    2057 B |     0.3443 |          - |         - |      1080 B |
|          Quickenshtein | .NET Core 5.0 |                 10 |         317.7 ns |          29.94 ns |         1.64 ns | 3,147,173.182 |  0.29 |    3.43 |    0.00 |    4757 B |          - |          - |         - |           - |
| Quickenshtein_Threaded | .NET Core 5.0 |                 10 |         319.0 ns |          11.93 ns |         0.65 ns | 3,134,892.401 |  0.29 |    3.41 |    0.00 |    5419 B |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 5.0 |                 10 |         181.4 ns |          43.26 ns |         2.37 ns | 5,512,763.783 |  0.17 |    6.00 |    0.00 |     317 B |     0.0203 |          - |         - |        64 B |
|                        |               |                    |                  |                   |                 |               |       |         |         |           |            |            |           |             |
|               Baseline |    .NET 4.7.2 |                400 |     746,161.8 ns |     236,310.09 ns |    12,952.95 ns |     1,340.192 |  1.00 |    1.00 |    0.00 |    2559 B |   142.5781 |    59.5703 |         - |    668278 B |
|          Quickenshtein |    .NET 4.7.2 |                400 |     625,731.9 ns |     147,464.59 ns |     8,083.03 ns |     1,598.129 |  0.84 |    1.19 |    0.01 |    3705 B |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                400 |     642,601.3 ns |      33,678.67 ns |     1,846.04 ns |     1,556.175 |  0.86 |    1.16 |    0.01 |    4082 B |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                400 |     346,120.9 ns |     104,788.04 ns |     5,743.79 ns |     2,889.164 |  0.46 |    2.16 |    0.00 |     324 B |     0.4883 |          - |         - |      1633 B |
|               Baseline | .NET Core 5.0 |                400 |     849,690.9 ns |     325,333.68 ns |    17,832.64 ns |     1,176.899 |  1.14 |    0.88 |    0.04 |    2057 B |   122.0703 |    60.5469 |         - |    657840 B |
|          Quickenshtein | .NET Core 5.0 |                400 |      64,781.7 ns |      27,856.75 ns |     1,526.92 ns |    15,436.466 |  0.09 |   11.52 |    0.00 |    4925 B |          - |          - |         - |           - |
| Quickenshtein_Threaded | .NET Core 5.0 |                400 |      64,594.2 ns |      26,585.61 ns |     1,457.25 ns |    15,481.273 |  0.09 |   11.56 |    0.00 |    5673 B |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 5.0 |                400 |     342,278.1 ns |     111,251.17 ns |     6,098.05 ns |     2,921.601 |  0.46 |    2.18 |    0.00 |     317 B |     0.4883 |          - |         - |      1624 B |
|                        |               |                    |                  |                   |                 |               |       |         |         |           |            |            |           |             |
|               Baseline |    .NET 4.7.2 |               8000 | 360,927,200.0 ns |  73,588,885.34 ns | 4,033,655.12 ns |         2.771 |  1.00 |    1.00 |    0.00 |    2559 B | 43000.0000 | 22500.0000 | 3000.0000 | 256683568 B |
|          Quickenshtein |    .NET 4.7.2 |               8000 |  99,787,886.7 ns |  33,748,730.63 ns | 1,849,881.81 ns |        10.021 |  0.28 |    3.62 |    0.01 |    3705 B |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |               8000 |  30,820,936.5 ns |  27,687,376.92 ns | 1,517,638.56 ns |        32.445 |  0.09 |   11.73 |    0.01 |    5063 B |          - |          - |         - |      1024 B |
|           Fastenshtein |    .NET 4.7.2 |               8000 | 101,566,466.7 ns |   6,074,999.48 ns |   332,991.22 ns |         9.846 |  0.28 |    3.55 |    0.00 |     324 B |          - |          - |         - |     32048 B |
|               Baseline | .NET Core 5.0 |               8000 | 356,315,300.0 ns | 119,479,146.88 ns | 6,549,055.20 ns |         2.807 |  0.99 |    1.01 |    0.03 |    2334 B | 43000.0000 | 22500.0000 | 3000.0000 | 256352240 B |
|          Quickenshtein | .NET Core 5.0 |               8000 |  17,253,112.5 ns |   3,559,513.67 ns |   195,108.96 ns |        57.961 |  0.05 |   20.92 |    0.00 |    4925 B |          - |          - |         - |         9 B |
| Quickenshtein_Threaded | .NET Core 5.0 |               8000 |  16,973,617.7 ns |   7,798,979.49 ns |   427,488.38 ns |        58.915 |  0.05 |   21.28 |    0.00 |    5673 B |          - |          - |         - |         9 B |
|           Fastenshtein | .NET Core 5.0 |               8000 |  82,207,344.4 ns |   5,757,332.33 ns |   315,578.81 ns |        12.164 |  0.23 |    4.39 |    0.00 |     317 B |          - |          - |         - |     32024 B |