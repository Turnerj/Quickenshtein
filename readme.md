# Quickenshtein

A quick and memory efficient Levenshtein Distance calculator for .NET

[![AppVeyor](https://img.shields.io/appveyor/ci/Turnerj/Quickenshtein/master.svg)](https://ci.appveyor.com/project/Turnerj/Quickenshtein)
[![Codecov](https://img.shields.io/codecov/c/github/Turnerj/Quickenshtein/master.svg)](https://codecov.io/gh/Turnerj/Quickenshtein)
[![NuGet](https://img.shields.io/nuget/v/Quickenshtein.svg)](https://www.nuget.org/packages/Quickenshtein/)

## Performance

Quickenshtein gets its speed and memory effiency from a number of different optimizations.
To get the most performance out of the library, you will need .NET Core 3 or higher as this has support for hardware intrinsics.

Quickenshtein takes advantage of the following hardware intrinsics. On any recent x86 system, you will likely have these available.
- [SSE2](https://en.wikipedia.org/wiki/SSE2#CPU_support)
- [SSE4.1](https://en.wikipedia.org/wiki/SSE4#Supporting_CPUs)
- [AVX2](https://en.wikipedia.org/wiki/Advanced_Vector_Extensions#CPUs_with_AVX2)

If your computer doesn't have one of the hardware intrinsics available, Quickenshtein will still work - just slower than optimal.

## Benchmarking

There are a number of benchmarks in the repository that you can run on your system to see how well Quickenshtein performs.

Most of these benchmarks...
- Run .NET Framework and .NET Core so you can see how the performance changes between them
- Compare against a simple baseline Levenshtein Distance implementation with no specific optimizations
- Compare against [Fastenshtein](https://github.com/DanHarltey/Fastenshtein/), one of the other fast .NET Levenshtein Distance implementations

You can view results to these benchmarks at the links below:
- [Benchmarking against Fastenshtein](/docs/OverallBenchmarks.md)
- [Isolated Benchmarks](/docs/IsolatedBenchmarks.md)