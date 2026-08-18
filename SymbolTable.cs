using System.Diagnostics;
using HackAssembler;

using Level = HackAssembler.DebugPrinter.LogLevels;

public class SymbolTable
{

  private Dictionary<string, int> symbols { get; set; } = new();

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
