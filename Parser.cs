using System.Text;

public class Parser
{
  private StreamReader fileStream { get; set; }
  private InstructionTypeEnum curInstructionType { get; set; }
  private string curLine { get; set; }

  public Parser(string path)
  {
    var file = new FileInfo(path);
    if (!file.Exists)
    {
      DebugPrinter.Print($"The file {file.FullName} doesn't exist");
      Environment.Exit(1);
    }

    DebugPrinter.Print($"Parsing {file.FullName}\n");
    fileStream = new StreamReader(file.FullName, Encoding.UTF8);
  }

  private enum InstructionTypeEnum
  {
    A_INSTRUCTION,
    C_INSTRUCTION,
    L_INSTRUCTION,
  }

  private enum JumpType
  {
    JGT,
    JEQ,
    JGE,
    JLT,
    JLE,
    JMP
  }

  private enum RegisterType
  {
    D,
    A,
    M,
  }

  private int Line { get; set; } = 0;
  private int Col { get; set; } = 0;

  /// <summary>
  /// Reads the specified file and changes the internal Assembler state
  /// </summary>
  private void Parse(FileInfo file) {
    
  }

  private InstructionTypeEnum instructionType() {
    if (curLine.StartsWith("@")) return InstructionTypeEnum.A_INSTRUCTION;
    else if (curLine.Contains("=")) return InstructionTypeEnum.C_INSTRUCTION;
    else return InstructionTypeEnum.L_INSTRUCTION;
  }

  private bool hasMoreLines()
  {
    return fileStream.EndOfStream;
  }

  private void advance()
  {
    curLine = fileStream.ReadLine();
    if (!curLine.Any() || curLine.StartsWith("//")) advance();
  }

  private string symbol()
  {
    if (curInstructionType == InstructionTypeEnum.A_INSTRUCTION)
    {
      return curLine.Substring(1);
    }
    else if (curInstructionType == InstructionTypeEnum.L_INSTRUCTION)
    {
      return curLine;
    }
    return "";
    // else {
    //   // WARN: Retornar erro aqui
    // }
  }

  private string dest()
  {
    if (curInstructionType != InstructionTypeEnum.C_INSTRUCTION)
    {
      // WARN: Retornar erro aqui
    }

    var tokens = curLine.ToCharArray();
    return tokens[0].ToString(); // WARN: Assume que o camp `dest` foi preenchido corretamente,
                      // e que eh de tamanho 1 o simbolo
  }

  private string comp()
  {
    if (curInstructionType != InstructionTypeEnum.C_INSTRUCTION)
    {
      // WARN: Retornar erro aqui
    }

    var tokens = curLine.ToCharArray();
    return tokens[2].ToString(); // WARN: Assume que o camp `dest` foi preenchido corretamente,
                      // e que eh de tamanho 1 o simbolo
  }

  private string jump()
  {
    var tokens = curLine.ToCharArray();
    return tokens[3].ToString(); // WARN: Assume que o camp `dest` foi preenchido corretamente,
                      // e que eh de tamanho 1 o simbolo
  }

  void ReadCInstruction(string line)
  {
    int c_initial = 0b1110_0000_0000_0000;
    var token_builder = new StringBuilder();

    char[] chars = line.ToCharArray();
    var stopping_lexemes = new List<char>{'=', ';'};
    for (int i = 0; i < chars.Length; i++)
    {
      var lexeme = chars[i];
      if (!stopping_lexemes.Contains(lexeme))
      {
        token_builder.Append(lexeme);
      }
      else
      {
        switch (lexeme) {
          case '=':
            var next_lexeme = chars[i+1];
            break;
          case ';':
            break;
        }
      }
    }
    string dest = line[0].ToString();

    int b_dest = 0b0;

    switch (dest) {
      case "D":
        b_dest = 0b010;
        break;

      case "A":
        b_dest = 0b100;
        break;

      case "M":
        b_dest = 0b001;
        break;
    }

    var final = c_initial | b_dest;
    Console.WriteLine(Convert.ToString(final, 2).PadLeft(16, '0'));

    // int b_comp;
    // switch (comp) {
    //   case "D":
    //     b_dest = 0b010;
    //     break;
    //
    //   case "A":
    //     b_dest = 0b100;
    //     break;
    //
    //   case "M":
    //     b_dest = 0b001;
    //     break;
    // }
  }


  private void ReadCompiledFile(string path) 
  {
    List<short> instructions = new List<short>();

    using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
    {
      while (reader.BaseStream.Position < reader.BaseStream.Length)
      {
        instructions.Add(reader.ReadInt16());
      }
    }

    foreach (var instruction in instructions)
    {
      DebugPrinter.Print(instruction);
    }
  }
}
