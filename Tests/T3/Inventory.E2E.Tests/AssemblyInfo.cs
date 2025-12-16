using NUnit.Framework;

// Force ALL tests in this assembly (T3) to run sequentially
[assembly: LevelOfParallelism(1)]

// Explicitly disable parallel execution
[assembly: Parallelizable(ParallelScope.None)]
