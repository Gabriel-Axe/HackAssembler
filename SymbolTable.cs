using System.Diagnostics;
using HackAssembler;

using Level = HackAssembler.DebugPrinter.LogLevels;

public class SymbolTable
{

  public SymbolTable() {}

  private Dictionary<string, int> symbols { get; set; } = new Dictionary<string, int>()
  {
    { "R0", 0 }, 
    { "R1", 1 }, 
    { "R2", 2 }, 
    { "R3", 3 }, 
    { "R4", 4 }, 
    { "R5", 5 }, 
    { "R6", 6 }, 
    { "R7", 7 }, 
    { "R8", 8 }, 
    { "R9", 9 }, 
    { "R10", 10 }, 
    { "R11", 11 }, 
    { "R12", 12 }, 
    { "R13", 13 }, 
    { "R14", 14 }, 
    { "R15", 15 }, 
    { "SP", 0 }, 
    { "LCL", 1 }, 
    { "ARG", 2 }, 
    { "THIS", 3 },
    { "THAT", 4 },
    { "SCREEN", 0x4000 },
    { "SCREEN", 0x6000 } 
  };

  public void addEntry(string symbol, int address)
  {
    if (symbols.TryAdd(symbol, address)) 
    {
      DebugPrinter.Log("Error: symbol already exists",  Level.INFO);
      Environment.Exit(1);
    }
  }

  public bool contains(string symbol)
  {
    return symbols.Any(s => s.Key == symbol);
  }

  public int getAddress(string symbol)
  {
    if (!contains(symbol))
    {
      DebugPrinter.Log("Error: symbol does not exists",  Level.INFO);
      Environment.Exit(1);
    }
    return symbols.First(s => s.Key == symbol).Value;
  }
}
