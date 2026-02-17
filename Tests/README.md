# Rubik's Cube Tests

Test suite for the Rubik's Cube Simulator using xUnit and FluentAssertions.

## Test Summary

**44 test methods** expanding to **81 test cases** (100% pass rate).

### Domain Layer Tests

#### CubeTests.cs (12 methods, 15 cases)
- Cube initialisation with correct colours
- Defensive copying of face data (GetFace returns a copy)
- Rotation reversibility (clockwise + counter-clockwise = original) for all 6 faces
- Face sticker rotation correctness
- Edge piece movement validation for Front rotation
- Four rotations return to original state (360 degrees)
- Exact challenge sequence snapshot (cell-by-cell assertion of F R' U B' L D')
- Up face rotation behaviour
- Right face anti-clockwise rotation
- Null move argument guard

#### MoveTests.cs (12 methods, 48 cases)
- Move construction with valid parameters
- Validation of invalid face type / rotation direction
- FromNotation parsing for all 12 standard tokens
- FromNotation rejection of invalid notation
- FromNotation null guard
- Standard notation output (ToString)
- Human-readable descriptions (GetDescription)
- Factory methods (Clockwise, AntiClockwise)
- Inverse method
- Value-object equality (Equals, GetHashCode, ==, !=)

#### MoveSequenceTests.cs (10 methods, 10 cases)
- Sequence construction
- Challenge sequence creation (F R' U B' L D')
- Standard notation string output
- Detailed multi-line descriptions
- Enumeration / iteration support
- Immutability guarantee
- FromNotation parsing
- FromNotation single-move string
- FromNotation invalid-token rejection
- FromNotation null guard

### Application Layer Tests

#### CubeServiceTests.cs (4 methods, 4 cases)
- Service initialisation with cube instance
- Single move application
- Sequence execution with multiple moves
- Challenge sequence integration

#### OrchestratorTests.cs (4 methods, 4 cases)
- Challenge sequence retrieval
- Full challenge execution
- Step-by-step execution with callbacks
- Correct move ordering and descriptions

## Running Tests

```bash
cd Tests
dotnet test                                        # run all
dotnet test --verbosity normal                     # with detailed output
dotnet test --filter "CubeTests"                   # single class
dotnet test --filter "FullyQualifiedName~Domain"   # by layer
dotnet test --collect:"XPlat Code Coverage"        # with coverage
```
