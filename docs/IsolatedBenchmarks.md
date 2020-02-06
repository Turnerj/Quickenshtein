# Isolated Benchmarks

These benchmarks are designed to test specific aspects of the calculation, avoiding specific optimizations.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
  [Host]     : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-VTGAFA : .NET Framework 4.8 (4.8.4075.0), X64 RyuJIT
  Job-ZVNHQX : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

## Edge Difference Benchmark

This benchmark eliminates bonuses for trimming equal characters at the start and end.
The strings the same length and, besides the edges, are only filled with matching characters.
This then forces the fastest path internally (least operations) for calculating distance.

|        Method |       Runtime |      Mean |    Error |   StdDev | Ratio |   Gen 0 |   Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |----------:|---------:|---------:|------:|--------:|--------:|------:|----------:|
|      Baseline |    .NET 4.7.2 | 299.32 us | 1.332 us | 1.246 us |  1.00 | 74.2188 | 20.9961 |     - |  301049 B |
| Quickenshtein |    .NET 4.7.2 |  75.03 us | 0.303 us | 0.284 us |  0.25 |       - |       - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 | 102.12 us | 0.577 us | 0.540 us |  0.34 |  0.2441 |       - |     - |    1092 B |
|               |               |           |          |          |       |         |         |       |           |
|      Baseline | .NET Core 3.0 | 316.46 us | 3.712 us | 3.472 us |  1.00 | 64.4531 | 22.4609 |     - |  291737 B |
| Quickenshtein | .NET Core 3.0 |  68.63 us | 0.482 us | 0.451 us |  0.22 |       - |       - |     - |         - |
|  Fastenshtein | .NET Core 3.0 | 100.12 us | 0.561 us | 0.468 us |  0.32 |  0.2441 |       - |     - |    1088 B |

## Text Length Benchmark

This benchmark shows the impact of various, though relatively short, string lengths.
The calculation is forced to compute the worst possible case due to no matching characters.

|        Method | NumberOfCharacters |        Mean |     Error |    StdDev | Ratio |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |------------------- |------------:|----------:|----------:|------:|-------:|------:|------:|----------:|
|      Baseline |                  5 |   304.43 ns |  2.954 ns |  2.763 ns |  1.00 | 0.1707 |     - |     - |     536 B |
| Quickenshtein |                  5 |    95.02 ns |  0.765 ns |  0.716 ns |  0.31 |      - |     - |     - |         - |
|  Fastenshtein |                  5 |    62.05 ns |  1.283 ns |  1.317 ns |  0.20 | 0.0153 |     - |     - |      48 B |
|               |                    |             |           |           |       |        |       |       |           |
|      Baseline |                 20 | 2,440.08 ns | 14.153 ns | 12.546 ns |  1.00 | 0.8659 |     - |     - |    2720 B |
| Quickenshtein |                 20 |   812.83 ns |  6.044 ns |  5.654 ns |  0.33 |      - |     - |     - |         - |
|  Fastenshtein |                 20 | 1,054.91 ns |  9.393 ns |  8.786 ns |  0.43 | 0.0324 |     - |     - |     104 B |
|               |                    |             |           |           |       |        |       |       |           |
|      Baseline |                 40 | 7,152.43 ns | 43.605 ns | 34.044 ns |  1.00 | 2.6779 |     - |     - |    8400 B |
| Quickenshtein |                 40 | 2,977.93 ns | 31.614 ns | 28.025 ns |  0.42 |      - |     - |     - |         - |
|  Fastenshtein |                 40 | 3,745.75 ns | 36.140 ns | 32.037 ns |  0.52 | 0.0572 |     - |     - |     184 B |

## Huge Text Benchmark

This benchmark really pushes the calculator to the limit with extremely long string lengths.
This is combined with the fact that this is a worst case run with no matching characters.

|        Method |       Runtime | NumberOfCharacters |       Mean |    Error |   StdDev | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |-----------:|---------:|---------:|------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |               8192 |   124.0 ms |  0.50 ms |  0.47 ms |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 |   154.0 ms |  1.71 ms |  1.60 ms |     - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |   108.8 ms |  0.48 ms |  0.45 ms |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 |   136.6 ms |  2.73 ms |  4.09 ms |     - |     - |     - |   32840 B |
| Quickenshtein |    .NET 4.7.2 |              32768 | 1,968.3 ms | 19.63 ms | 17.40 ms |     - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |              32768 | 2,483.4 ms | 12.36 ms | 10.96 ms |     - |     - |     - |  131096 B |
| Quickenshtein | .NET Core 3.0 |              32768 | 2,103.5 ms |  6.38 ms |  5.33 ms |     - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |              32768 | 2,226.3 ms | 27.75 ms | 24.60 ms |     - |     - |     - |  131096 B |

## Edge Match Benchmark

This benchmark shows how well the calculator can optimise matching characters at the start and end.

|        Method |       Runtime | NumberOfCharacters |             Mean |            Error |           StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |-------------- |------------------- |-----------------:|-----------------:|-----------------:|-------:|------:|------:|----------:|
| Quickenshtein |    .NET 4.7.2 |                 40 |        132.92 ns |         1.441 ns |         1.348 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |                 40 |      2,622.65 ns |        23.465 ns |        21.949 ns | 0.0572 |     - |     - |     185 B |
| Quickenshtein | .NET Core 3.0 |                 40 |         79.94 ns |         0.846 ns |         0.792 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |                 40 |      2,432.23 ns |        28.872 ns |        25.594 ns | 0.0572 |     - |     - |     184 B |
| Quickenshtein |    .NET 4.7.2 |               8192 |      8,902.75 ns |        81.773 ns |        76.491 ns |      - |     - |     - |         - |
|  Fastenshtein |    .NET 4.7.2 |               8192 | 97,871,204.44 ns | 1,248,954.715 ns | 1,168,273.015 ns |      - |     - |     - |   32816 B |
| Quickenshtein | .NET Core 3.0 |               8192 |        742.43 ns |         6.004 ns |         5.616 ns |      - |     - |     - |         - |
|  Fastenshtein | .NET Core 3.0 |               8192 | 85,940,415.56 ns | 1,157,319.269 ns | 1,082,557.162 ns |      - |     - |     - |   33597 B |