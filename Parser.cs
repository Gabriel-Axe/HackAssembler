namespace HackAssembler;

using System.Text;

public class Parser
{
  private string Path { get; set; }
  private StreamReader FileStream { get; set; } 
  public int ParsedMeaningfulLines { get; private set; } = 0;
  public string CurLine { get; private set; } = "";

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
    FileStream = new StreamReader(file.FullName, Encoding.UTF8);
  }

  public void IncrementParsedMeaningfulLines() 
  {
    var old = ParsedMeaningfulLines;
    ParsedMeaningfulLines++;
    LogParserAction($"increment parsedLines, old: {old} new: {ParsedMeaningfulLines}");
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

  public CTokenType GetTokenType(string token)
  {
    JumpType[] jumpTypes = [JumpType.JGT, JumpType.JEQ, JumpType.JGE, JumpType.JLT, JumpType.JLE, JumpType.JMP];
    var jumps = new List<string>();
    foreach (var jump in jumpTypes)
    {
      jumps.Add(jump.ToString());
    }
    if (jumps.Contains(token)) return CTokenType.JUMP;
    return CTokenType.DEST;
  }

  public InstructionTypeEnum InstructionType() {

    InstructionTypeEnum nextType;

    var isAInstruction = 
      CurLine.StartsWith("@");

    var isLInstruction = 
      CurLine.StartsWith("(") ;

    if (isAInstruction) 
    {
      nextType = InstructionTypeEnum.A_INSTRUCTION;
    }
    else if (isLInstruction) 
    {
      nextType = InstructionTypeEnum.L_INSTRUCTION;
    }
    else 
    {
      nextType = InstructionTypeEnum.C_INSTRUCTION;
    }
    LogParserAction($"return instruction type {nextType}");
    return nextType;
  }

  private void LogParserAction(string what)
  {
    DebugPrinter.LogAction("parser", what);
  }

  public bool HasMoreLines()
  {
    var eof = FileStream.EndOfStream;
    DebugPrinter.LogState($"end of file? {eof}");
    return eof;
  }

  // WARN: I should not send inverted dependencies wtvr its convenient
  public void Advance(SymbolTable symbolTable)
  {
    LogParserAction("advance");
    CurLine = FileStream
        .ReadLine()
        .Trim();

    if (!CurLine.Any() || CurLine.StartsWith("//")) 
    {
      Advance(symbolTable);
    }
    else
    {
      DebugPrinter.LogState($"current line: {CurLine}");
    }
  }

  public string Symbol()
  {
    LogParserAction("fetch symbol");
    var curInstructionType = InstructionType();
    if (curInstructionType == InstructionTypeEnum.A_INSTRUCTION)
    {
      return CurLine[1..^0];
    }
    else if (curInstructionType == InstructionTypeEnum.L_INSTRUCTION)
    {
      LogParserAction($"L Instruction {CurLine} received");
      var last = CurLine.Length - 2;
      var returning = CurLine[1..last];
      LogParserAction($"returning {returning}");
      return returning;
    }
    return "";
    // else {
    //   // WARN: Retornar erro aqui
    // }
  }

  public string Dest()
  {
    if (!HasDest()) 
    {
      LogParserAction("no dest, skip");
      return "";
    }
    LogParserAction("fetch Dest");
    var lexemes = CurLine.ToCharArray();
    var builder = new StringBuilder();
    DebugPrinter.LogState($"curLine: {CurLine}");
    for (var i = 0; i < lexemes.Length; i++)
    {
      DebugPrinter.LogState($"lexeme length: {lexemes.Length}, i: {i}");
      builder.Append(lexemes[i]);
      if (lexemes.Length > i + 1 && (lexemes[i + 1] == '=' || lexemes[i + 1] == ';')) break;
    }
    var dest = builder.ToString();
    return dest; // WARN: Assume que o camp `dest` foi preenchido corretamente,
                      // e que eh de tamanho 1 o simbolo
  }

  public string Comp()
  {
    LogParserAction("fetch comp");

    var lexemes = CurLine.ToCharArray();
    var builder = new StringBuilder();
    var found = false;
    if (HasDest())
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
    var comp = builder.ToString();
    return comp; // WARN: Assume que o camp `dest` foi preenchido corretamente,
                      // e que eh de tamanho 1 o simbolo
  }

  public bool HasDest() 
  {
    var line = CurLine;
    for (int i = 0; i < line.Length; i++)
    {
      if (line[i] == '=') return true;
    }
    return false;
  }

  public string Jump()
  {
    LogParserAction("fetch Jump");

    var lexemes = CurLine.ToCharArray();
    var builder = new StringBuilder();
    var found = false;
    int i = 0;
    for (; i < lexemes.Length; i++)
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
      DebugPrinter.LogState($"lexemes: {CurLine} length: {lexemes.Length} i: {i} lexemes[i]: {lexemes[i]}");
      if (i + 1 > lexemes.Length) break;
    }
    DebugPrinter.LogState($"(finished loop) lexemes: {CurLine} length: {lexemes.Length} i: {i} lexemes[i]: {lexemes[i-1]}");
    var jump = builder.ToString();
    return jump; // WARN: Assume que o camp `dest` foi preenchido corretamente,
  }

  public Parser Clone()
  {
    LogParserAction("clone parser");
    return new Parser(Path);
  }
}
