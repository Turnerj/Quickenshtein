# Intrinsics Performance

These benchmarks are designed to give an overall impression of a Levenshtein distance solution.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]                : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Core (No Intrinsics)  : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Core (<= SSE2)        : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Framework             : .NET Framework 4.8 (4.8.4150.0), X64 RyuJIT
  Core (<= SSE41)       : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Core (All Intrinsics) : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

|        Method |                   Job |        EnvironmentVariables |       Runtime |      Mean |    Error |   StdDev | Ratio | Speedup | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |---------------------- |---------------------------- |-------------- |----------:|---------:|---------:|------:|--------:|------:|------:|------:|----------:|
| Quickenshtein |  Core (No Intrinsics) | COMPlus_EnableHWIntrinsic=0 | .NET Core 3.0 | 117.17 ms | 1.108 ms | 1.036 ms |  1.01 |    0.99 |     - |     - |     - |     250 B |
| Quickenshtein |        Core (<= SSE2) |       COMPlus_EnableSSE41=0 | .NET Core 3.0 | 117.02 ms | 0.701 ms | 0.655 ms |  1.01 |    0.99 |     - |     - |     - |     165 B |
| Quickenshtein |             Framework |                       Empty |    .NET 4.7.2 | 115.97 ms | 0.862 ms | 0.806 ms |  1.00 |    1.00 |     - |     - |     - |         - |
| Quickenshtein |       Core (<= SSE41) |        COMPlus_EnableAVX2=0 | .NET Core 3.0 |  45.88 ms | 0.242 ms | 0.214 ms |  0.40 |    2.53 |     - |     - |     - |         - |
| Quickenshtein | Core (All Intrinsics) |                       Empty | .NET Core 3.0 |  45.57 ms | 0.377 ms | 0.353 ms |  0.39 |    2.54 |     - |     - |     - |         - |