# Quickenshtein

A quick and memory efficient Levenshtein Distance calculator for .NET

[![AppVeyor](https://img.shields.io/appveyor/ci/Turnerj/Quickenshtein/master.svg)](https://ci.appveyor.com/project/Turnerj/Quickenshtein)
[![Codecov](https://img.shields.io/codecov/c/github/Turnerj/Quickenshtein/master.svg)](https://codecov.io/gh/Turnerj/Quickenshtein)
[![NuGet](https://img.shields.io/nuget/v/Quickenshtein.svg)](https://www.nuget.org/packages/Quickenshtein/)

## Performance

Quickenshtein gets its speed and memory efficiency from a number of different optimizations.
To get the most performance out of the library, you will need .NET Core 3 or higher as this has support for hardware intrinsics.

Quickenshtein takes advantage of the following hardware intrinsics. On any recent x86 system, you will likely have these available.
- [SSE2](https://en.wikipedia.org/wiki/SSE2#CPU_support)
- [SSE4.1](https://en.wikipedia.org/wiki/SSE4#Supporting_CPUs)
- [AVX2](https://en.wikipedia.org/wiki/Advanced_Vector_Extensions#CPUs_with_AVX2)

If your computer doesn't have one of the hardware intrinsics available, Quickenshtein will still work - just slower than optimal.

## Multi-Threading

By default, Quickenshtein performs in single-threaded mode as this mode performs best for small to medium size strings while having no memory allocations.
When dealing with huge strings of 8000 characters or more, it may be useful to switch to multi-threaded mode.
In this mode, the calculation is broken up and shared between multiple cores in a system.

Multi-threading is especially useful for systems without hardware intrinsics or for .NET Framework as shown in the table below where it provided a 3x performance improvement.

|                 Method |       Runtime | NumberOfCharacters |       Mean |     Error |     StdDev |      Gen 0 |      Gen 1 |     Gen 2 |   Allocated |
|----------------------- |-------------- |------------------- |-----------:|----------:|-----------:|-----------:|-----------:|----------:|------------:|
|          Quickenshtein |    .NET 4.7.2 |               8000 | 110.686 ms | 10.118 ms |   0.554 ms |          - |          - |         - |           - |
| Quickenshtein_Threaded |    .NET 4.7.2 |               8000 |  36.601 ms | 16.121 ms |   0.883 ms |          - |          - |         - |      1260 B |

To enable threading, you can pass in `CalculationOptions.DefaultWithThreading` to `Levenshtein.GetDistance()` or configure your own `CalculationOptions` with settings that work best for you.

_Note: Multi-threading is not allocation free (unlike single-threading mode) and will allocate a small amount depending on the number of threads used._

## Benchmarking

There are a number of benchmarks in the repository that you can run on your system to see how well Quickenshtein performs.

Most of these benchmarks...
- Run .NET Framework and .NET Core so you can see how the performance changes between them
- Compare against a simple baseline Levenshtein Distance implementation with no specific optimizations
- Compare against [Fastenshtein](https://github.com/DanHarltey/Fastenshtein/), one of the other fast .NET Levenshtein Distance implementations

You can view results to these benchmarks at the links below:
- [Benchmarking against Fastenshtein](/docs/OverallComparison.md)
- [Isolated Benchmarks](/docs/StringScenarios.md)
- [Intrinsics Performance](/docs/IntrinsicsPerformance.md)

## Example Usage

```csharp
using Quickenshtein;

// Common usage (uses default CalculationOptions with threading disabled)
var distance1 = Levenshtein.GetDistance("Saturday", "Sunday");

// Enable threading
var distance2 = Levenshtein.GetDistance("Saturday", "Sunday", CalculationOptions.DefaultWithThreading);

// Custom calculation options (helps with tuning for your specific workload and environment - you should benchmark your configurations on your system)
var distance3 = Levenshtein.GetDistance("Saturday", "Sunday", new CalculationOptions {
    EnableThreadingAfterXCharacters = 10000,
    MinimumCharactersPerThread = 25000
});
```
