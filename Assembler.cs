namespace HackAssembler;

using System.Text;

public class Assembler(string path)
{

  private Parser AssemblerParser { get; init; } = new(path);
  private Code AssemblerCode { get; init; } = new();
  private SymbolTable AssemblerSymbolTable { get; init; } = new();
  private List<string> Outputs { get; set; } = [];

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

    while (!parser.HasMoreLines())
    {
      LogAssemblerAction("advancing in file");
      parser.Advance(AssemblerSymbolTable);
      DebugPrinter.LogState($"parsing line {parser.ParsedMeaningfulLines}: {parser.CurLine}");
      var symbol = parser.Symbol(AssemblerSymbolTable);
      var type = parser.InstructionType(AssemblerSymbolTable);
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
            if (!AssemblerSymbolTable.Contains(symbol))
            {
              LogAssemblerAction($"found non internal label {symbol}, continuing");
              break;
            }

              LogAssemblerAction($"found internal label {symbol}");
            var int_symbol = AssemblerSymbolTable.GetAddress(symbol);
            var bin = Convert.ToString(int_symbol, 2).PadLeft(16, '0').ToString();
            AddToOutputs(bin);
          }
          parser.IncrementParsedMeaningfulLines();
          break;
        case Parser.InstructionTypeEnum.C_INSTRUCTION:
          var c_initial = "111";
          var dest = parser.Dest();
          var comp = parser.Comp();
          var jump = parser.Jump();
          if (parser.HasDest()) LogAssemblerAction($"parse C instruction: {dest}={comp};{jump}");
          else LogAssemblerAction($"parse C instruction: {comp};{jump}");
          
          DebugPrinter.LogState($"dest: {dest}, comp: {comp}, jump: {jump}");

          var dest_bin = code.Dest(dest);
          var comp_bin = code.Comp(comp);
          var jump_bin = code.Jump(jump);
          DebugPrinter.LogState($"dest bin: {dest_bin}, comp bin: {comp_bin}, jump bin: {jump_bin}");

          var builder = new StringBuilder();
          builder
            .Append(c_initial)
            .Append(comp_bin)
            .Append(dest_bin)
            .Append(jump_bin);

          var c_instruction = builder.ToString();
          LogAssemblerAction($"instruction `{dest}={comp};{jump}` generates {c_instruction}");
          AddToOutputs(c_instruction);
          parser.IncrementParsedMeaningfulLines();
          break;
        case Parser.InstructionTypeEnum.L_INSTRUCTION:
          // WARN: WHY ARE YOU SAYING THIS IS L INSTRUCTION?
          var address = parser.ParsedMeaningfulLines;
          DebugPrinter.LogState($"DS symbol: {symbol}, address: {address}");
          LogAssemblerAction($"add {symbol} type {parser.InstructionType(AssemblerSymbolTable)} address {address} in symbol table 2");

          LogAssemblerAction("add entry 1");
          parser.IncrementParsedMeaningfulLines();
          if (!AssemblerSymbolTable.Contains(symbol)) 
            AssemblerSymbolTable.AddEntry(symbol, AssemblerParser);
          break;
      }
    }
    LogAssemblerAction($"parsed lines: {parser.ParsedMeaningfulLines}");
    code.OutputFile(Outputs);
  }

  private void ReadAllSymbols()
  {
    LogAssemblerAction("read all symbols");
    var parser = AssemblerParser.Clone();
    while (parser.HasMoreLines())
    {
      var type = parser.InstructionType(AssemblerSymbolTable);
      if (type == Parser.InstructionTypeEnum.L_INSTRUCTION)
      {
        // WARN: It seems like this code is not being used at
        // all to add stuff to the symbol table
        // WARN: This code isnt being executed
        var symbol = parser.Symbol(AssemblerSymbolTable);
        var address = parser.ParsedMeaningfulLines;
        DebugPrinter.LogState($"DS symbol: {symbol}, address: {address}");
        LogAssemblerAction($"add {symbol} type {parser.InstructionType(AssemblerSymbolTable)} address {address} in symbol table 1");
        AssemblerSymbolTable.AddEntry(symbol, address);
      }
    }
  }
}
