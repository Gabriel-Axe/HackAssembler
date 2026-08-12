using System.Text;
using System.Text.Unicode;

public class Assembler
{

  private Parser AssemblerParser { get; init; }
  private Code AssemblerCode { get; init; } = new();
  private List<string> Outputs { get; set; }

  public Assembler(string path)
  {
    AssemblerParser = new(path);
    Outputs = new();
  }

  public void Execute()
  {
    var parser = AssemblerParser;
    var code = AssemblerCode;

    while (!parser.hasMoreLines())
    {
      DebugPrinter.PrintValue("Advancing in File");
      parser.advance();
      var type = parser.instructionType();
      switch (type)
      {
        case Parser.InstructionTypeEnum.A_INSTRUCTION:
          DebugPrinter.PrintValue("Parsing A Instruction");
          var symbol = parser.symbol();
          var int_symbol = int.Parse(symbol);
          var bin = Convert.ToString(int_symbol, 2);
          Outputs.Add(bin);
          break;
        case Parser.InstructionTypeEnum.C_INSTRUCTION:
          DebugPrinter.PrintValue("Parsing C Instruction");
          var c_initial = "1111";
          var dest = parser.dest();
          var comp = parser.comp();
          var jump = parser.jump();

          var dest_bin = code.dest(dest);
          var comp_bin = code.comp(comp);
          var jump_bin = code.jump(jump);

          var builder = new StringBuilder();
          builder
            .Append(c_initial)
            .Append(comp_bin)
            .Append(dest_bin)
            .Append(jump_bin);

          var c_instruction = builder.ToString();
          Outputs.Add(c_instruction);
          break;
        case Parser.InstructionTypeEnum.L_INSTRUCTION:
          // NOTE: Idk what to do with this
          break;
      }
    }
    code.outputFile(Outputs);
  }
}
