# Intrinsics Performance

These benchmarks are designed to give an overall impression of a Levenshtein distance solution.

All benchmarks were run with the following configuration:

```
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.867 (2004/?/20H1)
Intel Core i7-6700HQ CPU 2.60GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.201
  [Host]                : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT
  Framework             : .NET Framework 4.8 (4.8.4341.0), X64 RyuJIT
  Core (w/o SSE41)      : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT
  Core (No Intrinsics)  : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT
  Core (w/o AVX2)       : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT
  Core (All Intrinsics) : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT
```


|        Method |                   Job |        EnvironmentVariables |       Runtime |      Mean |    Error |   StdDev |   Op/s | Code Size | Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------------- |---------------------- |---------------------------- |-------------- |----------:|---------:|---------:|-------:|----------:|------:|------:|------:|----------:|
| Quickenshtein | Core (All Intrinsics) |                       Empty | .NET Core 5.0 |  17.31 ms | 0.326 ms | 0.305 ms | 57.779 |    4925 B |     - |     - |     - |       9 B |
| Quickenshtein |       Core (w/o AVX2) |        COMPlus_EnableAVX2=0 | .NET Core 5.0 |  28.94 ms | 0.539 ms | 0.504 ms | 34.551 |    4410 B |     - |     - |     - |       9 B |
| Quickenshtein |  Core (No Intrinsics) | COMPlus_EnableHWIntrinsic=0 | .NET Core 5.0 | 100.04 ms | 1.844 ms | 1.725 ms |  9.996 |      48 B |     - |     - |     - |     307 B |
| Quickenshtein |      Core (w/o SSE41) |       COMPlus_EnableSSE41=0 | .NET Core 5.0 | 100.52 ms | 1.468 ms | 1.373 ms |  9.948 |      48 B |     - |     - |     - |     307 B |
| Quickenshtein |             Framework |                       Empty |    .NET 4.7.2 | 100.77 ms | 1.484 ms | 1.316 ms |  9.923 |    3705 B |     - |     - |     - |         - |