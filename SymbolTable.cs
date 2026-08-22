using HackAssembler;

public class SymbolTable
{

  public int currentCount { get; private set; } = 0;

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
    { "KBD", 0x6000 } 
  };

  public void addEntry(string symbol, int address)
  {
    if (contains(symbol)) 
    {
      DebugPrinter.LogError($"symbol {symbol} already present in symbol table");
      Environment.Exit(1);
    }
    DebugPrinter.LogAction("symbol table", $"add {symbol} with address {address} to Symbol Table");

    symbols.Add(symbol, address);
  }

  public void addEntry(string symbol)
  {
    addEntry(symbol, currentCount);
    currentCount++;
  }

  public bool contains(string symbol)
  {
    return symbols.Any(s => s.Key == symbol);
  }

  public int getAddress(string symbol)
  {
    if (!contains(symbol))
    {
      DebugPrinter.LogError($"symbol {symbol} does not exist");
      Environment.Exit(1);
    }
    return symbols.First(s => s.Key == symbol).Value;
  }
}
