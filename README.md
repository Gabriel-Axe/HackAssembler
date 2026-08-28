# Hack Assembler

A program designed to generate binary code from a `.asm` file according to the Hack computer instructions from the Nand2Tetris course.

## Structure

```txt
├── Assembler.cs
├── Code.cs
├── DebugPrinter.cs
├── Parser.cs
├── Program.cs
└── SymbolTable.cs
```

`Assembler.cs`: The structure that coordinates the whole system
`Code.cs`: The module that understand how to read a instruction and output binary from it
`DebugPrinter.cs`: A utility class designed for simple logging
`Parser.cs`: The module that reads the code and has semantic understanding of it
`Program.cs`: The entry point of the program
`SymbolTable.cs`: The module that understand how to asign symbols and fetch them as labels

## Logging

Inside the DebugPrinter.cs, you can choose the level of logging within the LogLevels enum, choosing between:

- Info
- Debug
- Trace

```cs
const LogLevels LOG_LEVEL = LogLevels.TRACE;
```

Every level also brings all logs of the levels above.

## Enums

The parser uses three enums to represent different parts of the Hack instruction set:

- InstructionTypeEnum: used for saying what type of instruction is currently being parsed/read, defines if it's a A, C or L type of instruction
- JumpType: used to say what kind of jump is used in the current line being parsed
- CTokenType: used to indicate the different parts of the C instruction

## Executing

Simply run `dotnet run` with a valid* `*.asm` file** as a argument.

```bash
dotnet run TestFiles/max/Max.asm
```

\* The project does not contain error handling at the moment
\** A Hack .asm, not a x86_64 .asm

## Considerations

There are some edge cases in the assembler currently, specifically, in the `Max.asm` test file
