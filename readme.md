# Quickenshtein

A quick and memory efficient Levenshtein Distance calculator for .NET

[![AppVeyor](https://img.shields.io/appveyor/ci/Turnerj/Quickenshtein/master.svg)](https://ci.appveyor.com/project/Turnerj/Quickenshtein)
[![Codecov](https://img.shields.io/codecov/c/github/Turnerj/Quickenshtein/master.svg)](https://codecov.io/gh/Turnerj/Quickenshtein)
[![NuGet](https://img.shields.io/nuget/v/Quickenshtein.svg)](https://www.nuget.org/packages/Quickenshtein/)

## Performance

Quickenshtein gets its speed and memory effiency from a number of different optimizations.
For .NET Core in particular, Quickenshtein attempts to make use of AVX2 and SSE2 intrinsics to speed up various parts of the calculation.
While doing all of this, utilising other recent features for .NET Framework and .NET Core, Quickenshtein manages to have zero allocations.

**See Also**

- [Benchmarking against Fastenshtein](/docs/OverallBenchmarks.md)
- [Isolated Benchmarks](/docs/IsolatedBenchmarks.md)