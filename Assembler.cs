namespace HackAssembler;

using System.Text;

public class Assembler
{

  private Parser AssemblerParser { get; init; }
  private Code AssemblerCode { get; init; } = new();
  private SymbolTable AssemblerSymbolTable { get; init; } = new();
  private List<string> Outputs { get; set; }

  public Assembler(string path)
  {
    AssemblerParser = new(path);
    Outputs = new();
  }

  public void AddToOutputs(string stored)
  {
    LogAssemblerAction($"store in Outputs: {stored}");
    Outputs.Add(stored);
  }

  private void LogAssemblerAction(string what)
  {
    DebugPrinter.LogAction("assembler", what);
  }

  public void Execute()
  {
    var parser = AssemblerParser;
    var code = AssemblerCode;
    ReadAllSymbols();

    while (!parser.hasMoreLines())
    {
      LogAssemblerAction("advancing in file");
      parser.advance(this.AssemblerSymbolTable);
      DebugPrinter.LogState($"parsing line {parser.parsedMeaningfulLines}: {parser.curLine}");
      var symbol = parser.symbol(this.AssemblerSymbolTable);
      var type = parser.instructionType(this.AssemblerSymbolTable);
      switch (type)
      {
        case Parser.InstructionTypeEnum.A_INSTRUCTION:
          LogAssemblerAction($"parse A Instruction @{symbol}");
          if (int.TryParse(symbol, out _))
          {
            var int_symbol = int.Parse(symbol);
            var bin = Convert.ToString(int_symbol, 2).PadLeft(16, '0').ToString();
            AddToOutputs(bin);
          }
          else
          {
            if (!this.AssemblerSymbolTable.contains(symbol))
            {
              // LogAssemblerAction("add entry 2");
              LogAssemblerAction($"found non internal label {symbol}, continuing");
              break;
              // this.AssemblerSymbolTable.addEntry(symbol, AssemblerParser);
            }

              LogAssemblerAction($"found internal label {symbol}");
            var int_symbol = this.AssemblerSymbolTable.getAddress(symbol);
            var bin = Convert.ToString(int_symbol, 2).PadLeft(16, '0').ToString();
            AddToOutputs(bin);
          }
          parser.incrementParsedMeaningfulLines();
          break;
        case Parser.InstructionTypeEnum.C_INSTRUCTION:
          var c_initial = "111";
          var dest = parser.dest();
          var comp = parser.comp();
          var jump = parser.jump();
          if (parser.hasDest()) LogAssemblerAction($"parse C instruction: {dest}={comp};{jump}");
          else LogAssemblerAction($"parse C instruction: {comp};{jump}");
          
          DebugPrinter.LogState($"dest: {dest}, comp: {comp}, jump: {jump}");

          var dest_bin = code.dest(dest);
          var comp_bin = code.comp(comp);
          var jump_bin = code.jump(jump);
          DebugPrinter.LogState($"dest bin: {dest_bin}, comp bin: {comp_bin}, jump bin: {jump_bin}");

          var builder = new StringBuilder();
          builder
            .Append(c_initial)
            // .Append("_")
            .Append(comp_bin)
            // .Append("_")
            .Append(dest_bin)
            // .Append("_")
            .Append(jump_bin);

          var c_instruction = builder.ToString();
          // DebugPrinter.PrintValue($"Instruction `{dest}={comp};{jump}` generated {c_instruction}");
          AddToOutputs(c_instruction);
          parser.incrementParsedMeaningfulLines();
          break;
        case Parser.InstructionTypeEnum.L_INSTRUCTION:
          // WARN: WHY ARE YOU SAYING THIS IS L INSTRUCTION?
          var address = parser.parsedMeaningfulLines;
          DebugPrinter.LogState($"DS symbol: {symbol}, address: {address}");
          LogAssemblerAction($"add {symbol} type {parser.instructionType(this.AssemblerSymbolTable)} address {address} in symbol table 2");

          LogAssemblerAction("add entry 1");
          parser.incrementParsedMeaningfulLines();
          if (!AssemblerSymbolTable.contains(symbol)) 
            AssemblerSymbolTable.addEntry(symbol, AssemblerParser);
          break;
      }
    }
    LogAssemblerAction($"parsed lines: {parser.parsedMeaningfulLines}");
    code.outputFile(Outputs);
  }

  private void ReadAllSymbols()
  {
    LogAssemblerAction("read all symbols");
    var parser = AssemblerParser.Clone();
    while (parser.hasMoreLines())
    {
      var type = parser.instructionType(this.AssemblerSymbolTable);
      if (type == Parser.InstructionTypeEnum.L_INSTRUCTION)
      {
        // WARN: It seems like this code is not being used at
        // all to add stuff to the symbol table
        // WARN: This code isnt being executed
        // var symbol = parser.symbol(this.AssemblerSymbolTable);
        // var address = parser.parsedMeaningfulLines;
        // DebugPrinter.LogState($"DS symbol: {symbol}, address: {address}");
        // LogAssemblerAction($"add {symbol} type {parser.instructionType(this.AssemblerSymbolTable)} address {address} in symbol table 1");
        // AssemblerSymbolTable.addEntry(symbol, address);
      }
    }
  }
}
