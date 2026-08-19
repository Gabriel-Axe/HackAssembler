namespace HackAssembler;

using System.Text;
using System.Text.Unicode;

using Level = HackAssembler.DebugPrinter.LogLevels;

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
    DebugPrinter.Log($"store in Outputs: {stored}", Level.TRACE);
    Outputs.Add(stored);
  }

  public void Execute()
  {
    var parser = AssemblerParser;
    var code = AssemblerCode;

    while (!parser.hasMoreLines())
    {
      DebugPrinter.Log("advancing parser", Level.TRACE);
      parser.advance();
      var type = parser.instructionType();
      switch (type)
      {
        case Parser.InstructionTypeEnum.A_INSTRUCTION:
          var symbol = parser.symbol();
          DebugPrinter.Log($"parse A instruction: @{symbol}", Level.DEBUG);
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
          if (parser.hasDest()) DebugPrinter.Log($"parse C instruction: {dest}={comp};{jump}", Level.DEBUG);
          else DebugPrinter.Log($"parse C instruction: {comp};{jump}", Level.DEBUG);
          
          DebugPrinter.Log($"dest: {dest}, comp: {comp}, jump: {jump}", Level.DEBUG);

          var dest_bin = code.dest(dest);
          var comp_bin = code.comp(comp);
          var jump_bin = code.jump(jump);
          DebugPrinter.Log($"dest bin: {dest_bin}, comp bin: {comp_bin}, jump bin: {jump_bin}", Level.DEBUG);

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

          break;
      }
    }
    DebugPrinter.Log($"parsed lines: {parser.parsedLines}", Level.DEBUG);
    code.outputFile(Outputs);
  }
}
