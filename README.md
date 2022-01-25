# CoveX: Quantum Circuit Simulator

![Nuget](https://img.shields.io/nuget/v/CoveX.LocalSimulation)

CoveX is a .NET Core framework for simulating quantum circuits written in C#.

It was originally written in .NET Framework 4.5 in the context of my BSc thesis on quantum computing. It is now updated to .NET Core 3.1 and listed on GitHub for reference reasons. There are no plans for further active development, however some code samples will be created for educational purposes and .NET version may update.

## Getting Started

CoveX is [available on NuGet](https://www.nuget.org/packages/CoveX.LocalSimulation/):

```bash
dotnet add package CoveX.LocalSimulation
```

## Usage Example

Demostrate entanglement via an EPR pair:

```csharp
// Create a Quantum Register
IQuantumRegister qRegister = new QuantumRegister(2);

// Entangle via Hadamard followed by CNot
(qRegister.SliceTo(0)).OperationHadamard();
qRegister.OperationCNot();

// Measure and display the result
ClassicalResult cResult = qRegister.Measure();
Console.WriteLine("Result: " + cResult.ToString());
```

## License

CoveX is released under the MIT License. See [LICENSE][1] file for details.

## Author

Vyron Vasileiadis <hi@fedonman.com>

## Links

- My BSc thesis on quantum computing: <https://fedonman.com/BSc-Thesis-Vyron-Vasileiadis.pdf>

[1]: https://github.com/fedonman/covex-quantum-circuit-simulator/blob/main/LICENSE
