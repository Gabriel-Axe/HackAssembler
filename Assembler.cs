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
      parser.advance();
      var symbol = parser.symbol();
      var type = parser.instructionType();
      LogAssemblerAction("advancing in file");
      switch (type)
      {
        case Parser.InstructionTypeEnum.A_INSTRUCTION:
          var int_symbol = int.Parse(symbol);
          var bin = Convert.ToString(int_symbol, 2).PadLeft(16, '0').ToString();
          AddToOutputs(bin);
          parser.incrementParsedLines();
          LogAssemblerAction($"parse A Instruction @{symbol}");
          break;
        case Parser.InstructionTypeEnum.C_INSTRUCTION:
          var c_initial = "111";
          var dest = parser.dest();
          var comp = parser.comp();
          var jump = parser.jump();
          if (parser.hasDest()) DebugPrinter.LogAction("parser", $"parse C instruction: {dest}={comp};{jump}");
          else DebugPrinter.LogAction("parser", $"parse C instruction: {comp};{jump}");
          
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
          parser.incrementParsedLines();
          break;
        case Parser.InstructionTypeEnum.L_INSTRUCTION:
          AssemblerSymbolTable.addEntry(symbol);
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
      var type = parser.instructionType();
      if (type == Parser.InstructionTypeEnum.L_INSTRUCTION)
      {
        var symbol = parser.symbol();
        DebugPrinter.LogAction("assembler", $"add {symbol} type {parser.instructionType()} to symbol table 1");
        AssemblerSymbolTable.addEntry(symbol);
      }
    }
  }
}
