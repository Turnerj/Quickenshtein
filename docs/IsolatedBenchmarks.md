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