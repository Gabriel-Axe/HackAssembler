namespace HackAssembler;

using System.Text;

public class Parser
{
  private string Path { get; set; }
  private StreamReader fileStream { get; set; }
  private bool ShouldIncrement { get; set; } = true;
  public int parsedMeaningfulLines { get; private set; } = 0;
  public string curLine { get; private set; }

  public Parser(string path)
  {
    Path = path;
    var file = new FileInfo(path);
    if (!file.Exists)
    {
      DebugPrinter.LogError($"file {file.FullName} does not exist");
      Environment.Exit(1);
    }

    LogParserAction($"parse {file.FullName}\n");
    fileStream = new StreamReader(file.FullName, Encoding.UTF8);
  }

  public void incrementParsedMeaningfulLines() 
  {
    // if (ShouldIncrement)
    // {
    ShouldIncrement = false;
    var old = parsedMeaningfulLines;
    parsedMeaningfulLines++;
    LogParserAction($"increment parsedLines, old: {old} new: {parsedMeaningfulLines}");
    // }
  }

  public enum CTokenType 
  {
    DEST,
    COMP,
    JUMP
  }

  public enum InstructionTypeEnum
  {
    NO_INSTRUCTION,
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

  public CTokenType getTokenType(string token)
  {
    JumpType[] jumpTypes = {JumpType.JGT, JumpType.JEQ, JumpType.JGE, JumpType.JLT, JumpType.JLE, JumpType.JMP};
    var jumps = new List<string>();
    foreach (var jump in jumpTypes)
    {
      jumps.Add(jump.ToString());
    }
    if (jumps.Contains(token)) return CTokenType.JUMP;
    return CTokenType.DEST;
  }

  public InstructionTypeEnum instructionType(SymbolTable symbolTable) {

    InstructionTypeEnum nextType = InstructionTypeEnum.NO_INSTRUCTION;

    var isAInstruction = 
      curLine.StartsWith("@");
      // && (
      //     int.TryParse(curLine.Substring(1), out _)
      //     || symbolTable.contains(curLine.Substring(1)));

    var isLInstruction = 
      curLine.StartsWith("(") ;

    if (isAInstruction) 
    {
      nextType = InstructionTypeEnum.A_INSTRUCTION;
      this.ShouldIncrement = true;
    }
    else if (isLInstruction) 
    {
      nextType = InstructionTypeEnum.L_INSTRUCTION;
    }
    else 
    {
      this.ShouldIncrement = true;
      nextType = InstructionTypeEnum.C_INSTRUCTION;
    }
    LogParserAction($"return instruction type {nextType}");
    return nextType;
    // else if (curLine.Contains("=")) return InstructionTypeEnum.C_INSTRUCTION;
    // else return InstructionTypeEnum.L_INSTRUCTION;
  }

  private void LogParserAction(string what)
  {
    DebugPrinter.LogAction("parser", what);
  }

  // public void handleAInstrution(string line, SymbolTable symbolTable)
  // {
  //   var builder = new StringBuilder();
  //   for (int i = 0; i > line.Length; i++)
  //   {
  //     if (line[i] == '@') continue;
  //     builder.Append(line[i]);
  //   }
  //
  //   var contents = builder.ToString();
  //   if (int.TryParse(contents, out _)) return;
  // }

  public bool hasMoreLines()
  {
    var eof = fileStream.EndOfStream;
    DebugPrinter.LogState($"end of file? {eof}");
    return eof;
  }

  // WARN: I should not send inverted dependencies wtvr its convenient
  public void advance(SymbolTable symbolTable)
  {
    LogParserAction("advance");
    curLine = fileStream
        .ReadLine()
        .Trim();

    if (!curLine.Any() || curLine.StartsWith("//")) 
    {
      advance(symbolTable);
    }
    else
    {
      DebugPrinter.LogState($"current line: {curLine}");
    }
  }

  public string symbol(SymbolTable symbolTable)
  {
    LogParserAction("fetch symbol");
    if (curInstructionType == InstructionTypeEnum.A_INSTRUCTION)
    {
      return curLine.Substring(1);
    }
    else if (curInstructionType == InstructionTypeEnum.L_INSTRUCTION)
    {
      LogParserAction($"L Instruction {curLine} received");
      var returning = curLine.Substring(1, curLine.Length - 2);
      LogParserAction($"returning {returning}");
      return returning;
    }
    return "";
    // else {
    //   // WARN: Retornar erro aqui
    // }
  }

  public string dest()
  {
    if (!hasDest()) 
    {
      LogParserAction("no dest, skip");
      return "";
    }
    LogParserAction("fetch Dest");
    var lexemes = curLine.ToCharArray();
    string dest = "";
    var builder = new StringBuilder();
    DebugPrinter.LogState($"curLine: {curLine}");
    for (var i = 0; i < lexemes.Length; i++)
    {
      DebugPrinter.LogState($"lexeme length: {lexemes.Length}, i: {i}");
      builder.Append(lexemes[i]);
      if (lexemes.Length > i + 1 && (lexemes[i + 1] == '=' || lexemes[i + 1] == ';')) break;
    }
    dest = builder.ToString();
    return dest; // WARN: Assume que o camp `dest` foi preenchido corretamente,
                      // e que eh de tamanho 1 o simbolo
  }

  public string comp()
  {
    LogParserAction("fetch comp");

    var lexemes = curLine.ToCharArray();
    string comp = "";
    var builder = new StringBuilder();
    var found = false;
    if (hasDest())
    {
      for (var i = 0; i < lexemes.Length; i++)
      {
        if (lexemes[i] != '=' && !found) continue;
        found = true;
        if (lexemes[i] == '=') continue;
        builder.Append(lexemes[i]);
        if (i + 1 < lexemes.Length && lexemes[i + 1] == ';') break;
      }
    } 
    else
    {
      for (var i = 0; i < lexemes.Length; i++)
      {
        builder.Append(lexemes[i]);
        if (i + 1 < lexemes.Length && lexemes[i + 1] == ';') break;
      }
    }
    comp = builder.ToString();
    return comp; // WARN: Assume que o camp `dest` foi preenchido corretamente,
                      // e que eh de tamanho 1 o simbolo

    // var tokens = getTokens(curLine);
    // var builder = new StringBuilder();
    // for (int i = 0; i < tokens.Length; i++)
    // {
    //   DebugPrinter.Log($"Seeing if tokens[i] equals ;");
    //   var reachedJump = tokens[i].Equals(";");
    //   if (reachedJump) break;
    //   DebugPrinter.Log($"tokens[i] does not equals ;");
    //   DebugPrinter.Log($"Appending tokens[i] ({tokens[i]})");
    //   builder.Append(tokens[i]);
    // }
    // var returning = builder.ToString();
    // DebugPrinter.Log($"Returning returining ({returning}");
    // return returning; // WARN: Assume que o camp `dest` foi preenchido corretamente,
    //                   // e que eh de tamanho 1 o simbolo
  }

  public bool hasDest() 
  {
    var line = curLine;
    for (int i = 0; i < line.Length; i++)
    {
      if (line[i] == '=') return true;
    }
    return false;
  }

  private string[] getTokens(string line)
  {
    DebugPrinter.LogState($"line tokenized: {line}");
    var tokens = line.Split();
    return tokens;
  }

  public string jump()
  {
    LogParserAction("fetch Jump");

    var lexemes = curLine.ToCharArray();
    string jump = "";
    var builder = new StringBuilder();
    var found = false;
    int i = 0;
    for (i = 0; i < lexemes.Length; i++)
    {
      if (lexemes[i] != ';' && !found) 
      {
        LogParserAction("';' not found, continuing");
        continue;
      }
      found = true;
      if (lexemes[i] == ';')
      {
        LogParserAction("';' fond, continuing");
        continue;
      }
      builder.Append(lexemes[i]);
      // DebugPrinter.Log($"i + 1 = {lexemes[i+1]}");
      DebugPrinter.LogState($"lexemes: {curLine} length: {lexemes.Length} i: {i} lexemes[i]: {lexemes[i]}");
      if (i + 1 > lexemes.Length) break;
    }
    DebugPrinter.LogState($"(finished loop) lexemes: {curLine} length: {lexemes.Length} i: {i} lexemes[i]: {lexemes[i-1]}");
    jump = builder.ToString();
    return jump; // WARN: Assume que o camp `dest` foi preenchido corretamente,
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
      DebugPrinter.LogState($"instruction: {instruction}");
    }
  }

  public Parser Clone()
  {
    LogParserAction("clone parser");
    return new Parser(this.Path);
  }
}

public class CInstruction(string dest, string comp, string? jump) {
  public string dest { get; init; }
  public string comp { get; init; }
  public string? jump { get; init; }
}
