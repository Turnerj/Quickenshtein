# Intrinsics Performance

These benchmarks are designed to give an overall impression of a Levenshtein distance solution.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18362.778 (1903/May2019Update/19H1)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=3.1.200
  [Host]                : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Core (w/o SSE41)      : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Core (No Intrinsics)  : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Framework             : .NET Framework 4.8 (4.8.4150.0), X64 RyuJIT
  Core (w/o AVX2)       : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
  Core (All Intrinsics) : .NET Core 3.0.1 (CoreCLR 4.700.19.51502, CoreFX 4.700.19.51609), X64 RyuJIT
```

|        Method |                   Job |        EnvironmentVariables |       Runtime |      Mean |    Error |   StdDev | Code Size | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |---------------------- |---------------------------- |-------------- |----------:|---------:|---------:|----------:|------:|------:|------:|----------:|
| Quickenshtein |      Core (w/o SSE41) |       COMPlus_EnableSSE41=0 | .NET Core 3.0 | 110.90 ms | 1.486 ms | 1.390 ms |    5783 B |     - |     - |     - |    2045 B |
| Quickenshtein |  Core (No Intrinsics) | COMPlus_EnableHWIntrinsic=0 | .NET Core 3.0 | 110.77 ms | 0.829 ms | 0.776 ms |    5368 B |     - |     - |     - |     968 B |
| Quickenshtein |             Framework |                       Empty |    .NET 4.7.2 | 109.65 ms | 0.893 ms | 0.835 ms |    3689 B |     - |     - |     - |         - |
| Quickenshtein |       Core (w/o AVX2) |        COMPlus_EnableAVX2=0 | .NET Core 3.0 |  45.55 ms | 0.265 ms | 0.248 ms |    3137 B |     - |     - |     - |         - |
| Quickenshtein | Core (All Intrinsics) |                       Empty | .NET Core 3.0 |  26.32 ms | 0.160 ms | 0.142 ms |    3439 B |     - |     - |     - |         - |