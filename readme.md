# Quickenshtein

A quick and memory efficient Levenshtein Distance calculator for .NET

[![AppVeyor](https://img.shields.io/appveyor/ci/Turnerj/Quickenshtein/master.svg)](https://ci.appveyor.com/project/Turnerj/Quickenshtein)
[![Codecov](https://img.shields.io/codecov/c/github/Turnerj/Quickenshtein/master.svg)](https://codecov.io/gh/Turnerj/Quickenshtein)
[![NuGet](https://img.shields.io/nuget/v/Quickenshtein.svg)](https://www.nuget.org/packages/Quickenshtein/)

## Performance

Quickenshtein gets its speed and memory effiency from a number of different optimizations.
For .NET Core in particular, Quickenshtein attempts to make use of hardware intrinsics to speed up various parts of the calculation.
Additionally, Quickenshtein utilises other recent features in .NET which help prevent any memory allocations that need to be GC'ed.

To get the most performance out of Quickenshtein, you will need:
- .NET Core 3+
- [SSE2 Support](https://en.wikipedia.org/wiki/SSE2#CPU_support) - You very likely have this on your CPU
- [SSE 4.1 Support](https://en.wikipedia.org/wiki/SSE4#Supporting_CPUs) - You likely have this on your CPU
- [AVX2 Support](https://en.wikipedia.org/wiki/Advanced_Vector_Extensions#CPUs_with_AVX2) - You likely have this on your CPU

If your computer doesn't have one of the hardware intrinsics available, Quickenshtein will still work - just slower.

## Benchmarking

There are a number of benchmarks in the repository that you can run on your system to see how well Quickenshtein performs.

Most of these benchmarks...
- Run .NET Framework and .NET Core so you can see how the performance changes between them
- Compare against a simple baseline Levenshtein Distance implementation with no specific optimizations
- Compare against [Fastenshtein](https://github.com/DanHarltey/Fastenshtein/), one of the other fastest .NET Levenshtein Distance implementations

You can view results to these benchmarks at the links below:
- [Benchmarking against Fastenshtein](/docs/OverallBenchmarks.md)
- [Isolated Benchmarks](/docs/IsolatedBenchmarks.md)