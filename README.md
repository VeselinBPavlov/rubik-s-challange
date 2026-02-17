# Rubik's Cube Simulator

A Rubik's Cube simulator built with Clean Architecture principles in C# / .NET 10.
Demonstrates clear separation of concerns, comprehensive testing, and standard
Rubik's Cube notation support.

## Overview

This simulator programmatically models a 3x3x3 Rubik's Cube with the ability to:

- Rotate any face clockwise or counter-clockwise
- Parse and execute move sequences in standard notation
- Visualise the cube state in an exploded-view format

**Initial Configuration (standard orientation):**

| Face  | Color  |
|-------|--------|
| Front | Green  |
| Right | Red    |
| Up    | White  |
| Back  | Blue   |
| Left  | Orange |
| Down  | Yellow |

## Architecture

The solution follows **Clean Architecture** with four layers:

```
Presentation    CubeController, Program.cs (entry point & DI)
      |
Application     CubeService, Orchestrator (workflow coordination)
      |
Domain          Cube entity, Move / MoveSequence value objects, enums, exceptions
      ^
Infrastructure  ConsoleCubeRenderer, ConsoleColorHelper (implements ICubeRenderer)
```

Dependencies point inward - the Domain layer has zero external references.

### Project Structure

```
Src/
+-- Domain/
|   +-- Entities/        Cube.cs
|   +-- ValueObjects/    Move.cs, MoveSequence.cs
|   +-- Enums/           FaceType, ColorType, RotationDirection
|   +-- Exceptions/      DomainException, InvalidMoveNotationException
+-- Application/
|   +-- Interfaces/      ICubeService, IOrchestrator
|   +-- Services/        CubeService, Orchestrator
+-- Infrastructure/
|   +-- Interfaces/      ICubeRenderer
|   +-- Rendering/       ConsoleCubeRenderer, ConsoleColorHelper
+-- Presentation/
|   +-- Controllers/     CubeController
+-- Program.cs
```

## Prerequisites

- **.NET 10.0 SDK** (or later)
- Windows, macOS, or Linux
- No external paid software required

## Getting Started

```bash
# Build
cd Src
dotnet build

# Run
dotnet run

# Run tests
cd ../Tests
dotnet test
```

## Using the Application

When you run the application it will:

1. Display the initial solved cube in exploded-view format
2. Show the challenge rotation sequence: **F R' U B' L D'**
3. Execute each rotation step-by-step with interactive prompts
4. Visualise the cube state after each move with coloured output
5. Display the final state once all rotations complete

## Standard Notation

| Notation | Meaning                                  |
|----------|------------------------------------------|
| ``F``    | Front face clockwise 90 degrees          |
| ``F'``   | Front face counter-clockwise 90 degrees  |
| ``R``    | Right face clockwise 90 degrees          |
| ``R'``   | Right face counter-clockwise 90 degrees  |
| ``U``    | Up face clockwise 90 degrees             |
| ``U'``   | Up face counter-clockwise 90 degrees     |
| ``B``    | Back face clockwise 90 degrees           |
| ``B'``   | Back face counter-clockwise 90 degrees   |
| ``L``    | Left face clockwise 90 degrees           |
| ``L'``   | Left face counter-clockwise 90 degrees   |
| ``D``    | Down face clockwise 90 degrees           |
| ``D'``   | Down face counter-clockwise 90 degrees   |

### Challenge Sequence

**F R' U B' L D'**

1. Front face clockwise 90 degrees
2. Right face anti-clockwise 90 degrees
3. Up face clockwise 90 degrees
4. Back face anti-clockwise 90 degrees
5. Left face clockwise 90 degrees
6. Down face anti-clockwise 90 degrees

## Code Examples

### Creating and Manipulating a Cube

```csharp
// Create a solved cube
var cube = Cube.CreateSolved();

// Apply a single move
var move = Move.FromNotation("F");
cube.ApplyMove(move);

// Execute a sequence
var sequence = MoveSequence.FromNotation("F R' U B' L D'");
foreach (var m in sequence)
    cube.ApplyMove(m);

// Read face state (returns a defensive copy)
ColorType[,] frontFace = cube.GetFace(FaceType.Front);
```

### Using Standard Notation

```csharp
var move = Move.FromNotation("R'");
Console.WriteLine(move.GetDescription()); // "Right face anti-clockwise 90 degrees"

var sequence = MoveSequence.FromNotation("F R U R' U' F'");
Console.WriteLine(sequence);              // "F R U R' U' F'"
```

### Error Handling

```csharp
try
{
    var move = Move.FromNotation("X"); // Invalid notation
}
catch (InvalidMoveNotationException ex)
{
    Console.WriteLine(ex.Message);
    // "Invalid move notation: 'X'. Valid format: F, F', R, R', ..."
    Console.WriteLine(ex.Notation);  // "X"
}
```

## Output Format

The cube is displayed in **exploded-view** format:

```
         UP
         W W W
         W W W
         W W W

LEFT     FRONT    RIGHT    BACK
O O O    G G G    R R R    B B B
O O O    G G G    R R R    B B B
O O O    G G G    R R R    B B B

         DOWN
         Y Y Y
         Y Y Y
         Y Y Y
```

Colour key: **W** White, **Y** Yellow, **G** Green, **B** Blue, **R** Red, **O** Orange

## Testing

The test suite contains **44 test methods** covering **81 individual test cases**
(Theory tests with InlineData expand to multiple cases).

| Area | Tests | Cases | What is covered |
|------|-------|-------|----------------|
| CubeTests | 12 | 15 | Initialisation, rotation correctness, reversibility, 360 degree identity, exact challenge-sequence snapshot, null guard |
| MoveTests | 12 | 48 | Construction, validation, notation parsing (FromNotation), ToString, descriptions, factory methods, Inverse, equality |
| MoveSequenceTests | 10 | 10 | Construction, FromNotation, challenge sequence, immutability, enumeration |
| CubeServiceTests | 4 | 4 | Service wiring, single move, sequence execution, challenge integration |
| OrchestratorTests | 4 | 4 | Sequence retrieval, full execution, step-by-step callbacks, move ordering |

```bash
cd Tests
dotnet test --verbosity normal
```

## Verification

Compare the output of the challenge sequence against
[rubiks-cube-solver.com](https://rubiks-cube-solver.com/):

1. Set initial state: Green front, Red right, White up
2. Apply the sequence using their UI buttons: F R' U B' L D'
3. Compare the final state with this simulator's output

## Extending the Solution

The architecture makes it straightforward to add features:

```csharp
// New rendering target - implement ICubeRenderer
public class WebCubeRenderer : ICubeRenderer { ... }

// Persistence
public interface ICubeRepository { Task SaveAsync(Cube cube); }

// Solving algorithms
public interface ICubeSolver { MoveSequence Solve(Cube cube); }
```

## License

See [LICENSE](LICENSE) for details.
