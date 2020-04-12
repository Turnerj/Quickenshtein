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

|                 Method |       Runtime | NumberOfCharacters |               Mean |              Error |            StdDev | Ratio | Speedup | Worthiness | Code Size |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|----------------------- |-------------- |------------------- |-------------------:|-------------------:|------------------:|------:|--------:|-----------:|----------:|-----------:|-----------:|----------:|------------:|
|               Baseline |    .NET 4.7.2 |                  0 |         220.389 ns |         39.1346 ns |         2.1451 ns | 1.000 |    1.00 |       1.00 |     616 B |     0.1018 |          - |         - |       321 B |
|          Quickenshtein |    .NET 4.7.2 |                  0 |           2.641 ns |          0.9263 ns |         0.0508 ns | 0.012 |   83.49 |     258.45 |     199 B |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                  0 |           1.679 ns |          0.0802 ns |         0.0044 ns | 0.008 |  131.24 |     451.65 |     179 B |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                  0 |           2.491 ns |          0.5916 ns |         0.0324 ns | 0.011 |   88.49 |     166.19 |     328 B |          - |          - |         - |           - |
|               Baseline | .NET Core 3.0 |                  0 |         114.638 ns |          2.1269 ns |         0.1166 ns | 0.520 |    1.92 |       1.95 |     608 B |     0.0764 |          - |         - |       240 B |
|          Quickenshtein | .NET Core 3.0 |                  0 |           3.751 ns |          1.1861 ns |         0.0650 ns | 0.017 |   58.76 |     140.85 |     257 B |          - |          - |         - |           - |
| Quickenshtein_Threaded | .NET Core 3.0 |                  0 |           1.721 ns |          0.2323 ns |         0.0127 ns | 0.008 |  128.03 |     424.00 |     186 B |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 3.0 |                  0 |           2.375 ns |          0.7063 ns |         0.0387 ns | 0.011 |   92.81 |     173.77 |     329 B |          - |          - |         - |           - |
|                        |               |                    |                    |                    |                   |       |         |            |           |            |            |           |             |
|               Baseline |    .NET 4.7.2 |                 10 |       1,264.971 ns |        196.1335 ns |        10.7507 ns |  1.00 |    1.00 |       1.00 |     616 B |     0.4463 |          - |         - |      1404 B |
|          Quickenshtein |    .NET 4.7.2 |                 10 |         223.599 ns |          4.3615 ns |         0.2391 ns |  0.18 |    5.66 |      17.51 |     199 B |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                 10 |         226.902 ns |         30.7952 ns |         1.6880 ns |  0.18 |    5.58 |       2.17 |    1582 B |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                 10 |         262.048 ns |         33.0572 ns |         1.8120 ns |  0.21 |    4.83 |       9.07 |     328 B |     0.0200 |          - |         - |        64 B |
|               Baseline | .NET Core 3.0 |                 10 |         749.855 ns |        107.4049 ns |         5.8872 ns |  0.59 |    1.69 |       1.71 |     608 B |     0.3443 |          - |         - |      1080 B |
|          Quickenshtein | .NET Core 3.0 |                 10 |         154.557 ns |          9.9932 ns |         0.5478 ns |  0.12 |    8.18 |      19.62 |     257 B |          - |          - |         - |           - |
| Quickenshtein_Threaded | .NET Core 3.0 |                 10 |         163.807 ns |         16.5201 ns |         0.9055 ns |  0.13 |    7.72 |       1.94 |    2448 B |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 3.0 |                 10 |         239.815 ns |         26.3942 ns |         1.4468 ns |  0.19 |    5.27 |       9.88 |     329 B |     0.0200 |          - |         - |        64 B |
|                        |               |                    |                    |                    |                   |       |         |            |           |            |            |           |             |
|               Baseline |    .NET 4.7.2 |                400 |     899,324.170 ns |    127,990.2076 ns |     7,015.5752 ns |  1.00 |    1.00 |       1.00 |     616 B |   142.5781 |    59.5703 |         - |    668278 B |
|          Quickenshtein |    .NET 4.7.2 |                400 |     725,624.447 ns |     33,149.5617 ns |     1,817.0393 ns |  0.81 |    1.24 |       3.84 |     199 B |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |                400 |     726,502.702 ns |    134,317.5241 ns |     7,362.3967 ns |  0.81 |    1.24 |       0.48 |    1582 B |          - |          - |         - |           - |
|           Fastenshtein |    .NET 4.7.2 |                400 |     439,688.835 ns |     30,021.2645 ns |     1,645.5668 ns |  0.49 |    2.05 |       3.84 |     328 B |     0.4883 |          - |         - |      1633 B |
|               Baseline | .NET Core 3.0 |                400 |     860,822.461 ns |     77,397.3648 ns |     4,242.4107 ns |  0.96 |    1.04 |       1.06 |     608 B |   122.0703 |    60.5469 |         - |    657840 B |
|          Quickenshtein | .NET Core 3.0 |                400 |     151,761.466 ns |     28,996.9182 ns |     1,589.4189 ns |  0.17 |    5.93 |      14.20 |     257 B |          - |          - |         - |           - |
| Quickenshtein_Threaded | .NET Core 3.0 |                400 |     151,052.702 ns |     10,655.4223 ns |       584.0597 ns |  0.17 |    5.95 |       1.50 |    2448 B |          - |          - |         - |           - |
|           Fastenshtein | .NET Core 3.0 |                400 |     387,029.085 ns |     23,021.9953 ns |     1,261.9132 ns |  0.43 |    2.32 |       4.35 |     329 B |     0.4883 |          - |         - |      1624 B |
|                        |               |                    |                    |                    |                   |       |         |            |           |            |            |           |             |
|               Baseline |    .NET 4.7.2 |               8000 | 394,663,266.667 ns | 31,092,725.3360 ns | 1,704,297.1934 ns |  1.00 |    1.00 |       1.00 |     616 B | 44000.0000 | 23000.0000 | 4000.0000 | 256683568 B |
|          Quickenshtein |    .NET 4.7.2 |               8000 | 112,734,440.000 ns | 11,860,500.0532 ns |   650,114.0294 ns |  0.29 |    3.50 |      10.84 |     199 B |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |               8000 |  30,425,380.208 ns | 11,213,926.6547 ns |   614,673.1596 ns |  0.08 |   12.98 |       5.05 |    1582 B |          - |          - |         - |      1024 B |
|           Fastenshtein |    .NET 4.7.2 |               8000 | 143,850,133.333 ns | 15,172,632.3962 ns |   831,663.1794 ns |  0.36 |    2.74 |       5.15 |     328 B |          - |          - |         - |     32048 B |
|               Baseline | .NET Core 3.0 |               8000 | 395,754,833.333 ns | 13,237,952.4515 ns |   725,616.8433 ns |  1.00 |    1.00 |       0.99 |     619 B | 44000.0000 | 23000.0000 | 4000.0000 | 256352240 B |
|          Quickenshtein | .NET Core 3.0 |               8000 |  42,934,775.000 ns |  6,765,424.4117 ns |   370,835.7409 ns |  0.11 |    9.19 |      22.03 |     257 B |          - |          - |         - |       347 B |
| Quickenshtein_Threaded | .NET Core 3.0 |               8000 |  42,976,533.333 ns |  6,483,349.2063 ns |   355,374.2471 ns |  0.11 |    9.18 |       2.31 |    2448 B |          - |          - |         - |        87 B |
|           Fastenshtein | .NET Core 3.0 |               8000 | 138,045,400.000 ns | 26,200,392.3662 ns | 1,436,131.9149 ns |  0.35 |    2.86 |       5.35 |     329 B |          - |          - |         - |     32024 B |