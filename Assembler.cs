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
    DebugPrinter.LogAction("assembler", $"store in Outputs: {stored}");
    Outputs.Add(stored);
  }

  public void Execute()
  {
    var parser = AssemblerParser;
    var code = AssemblerCode;
    ReadAllSymbols();

    while (!parser.hasMoreLines())
    {
      DebugPrinter.LogAction("parser", "advance");
      parser.advance();
      var symbol = parser.symbol();
      var type = parser.instructionType();
      switch (type)
      {
        case Parser.InstructionTypeEnum.A_INSTRUCTION:
          DebugPrinter.LogAction("parser", $"parse A instruction @{symbol}");
          var int_symbol = int.Parse(symbol);
          var bin = Convert.ToString(int_symbol, 2).PadLeft(16, '0').ToString();
          AddToOutputs(bin);
          parser.incrementParsedLines();
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
          DebugPrinter.LogAction("assembler", $"add {symbol} type {parser.instructionType()} to symbol table 2");
          AssemblerSymbolTable.addEntry(symbol);
          break;
      }
    }
    DebugPrinter.LogAction("assembler", $"parsed lines: {parser.parsedLines}");
    code.outputFile(Outputs);
  }

  private void ReadAllSymbols()
  {
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
