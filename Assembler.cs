using System.Text;

public class Assembler
{
  private InstructionType readingInstruction { get; set; } = 0;

  private Int16 CurAddress { get; set; } = 0;
  private Int16 CurOutput { get; set; } = 0;
  private List<Int16> Outputs  { get; set; } = new();

  private JumpType _JumpType { get; set; } = null;
  private RegisterType Register { get; set; } = null;

  private FileInfo currentFile { get; set; } = null;
  private int Line { get; set; } = 0;
  private int Col { get; set; } = 0;

  private void ReadFile(string path)
  {
    this.currentFile = new FileInfo(path);
    if (!currentFile.Exists)
    {
      DebugPrinter.Print($"The file {currentFile.FullName} doesn't exist");
      Environment.Exit(1);
    }
  }

  public void Execute(string path)
  {
    currentFile = ReadFile(path);
    Interpret(currentFile);
    foreach (var output in Outputs)
    {
      Console.WriteLine(output);
    }
  }

  /// <summary>
  /// Reads the specified file and changes the internal Assembler state
  /// </summary>
  private void Interpret(FileInfo file) {
    using (StreamReader streamReader = new StreamReader(file.FullName, Encoding.UTF8))
    {
      DebugPrinter.Print($"Interpreting {currentFile.FullName}\n");
      while (!streamReader.EndOfStream) 
      {
        var line = streamReader.ReadLine();
        // DebugPrinter.Print($"line: {line}");
        if (!line.Any() || line.StartsWith("//")) continue;
        if (line.StartsWith("@")) {
          ReadAInstruction(line);
        }
        else {
          ReadCInstruction(line);
        }
        DebugPrinter.DebugPrintLine(line);
        DebugPrinter.NewLine();
        this.interpret(currentFile);
      }
    }
  }

  void ReadCInstruction(string line)
  {
    int c_initial = 0b1110_0000_0000_0000;
    for (int col = Col; col < line.Length; col++)
    {
      if (!Char.IsLetter(line[Col+1]))
      {
        // get_token(line.Substring(Col));
      }
    }
    string dest = line[0].ToString();
    // string comp = line[2].ToString();

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

  void ReadAInstruction(string line)
  {
    Int16 address = Int16.Parse(line.Substring(1));
    CurOutput = address;
    Outputs.Add(address);
  }

  private enum InstructionType
  {
    C_INSTRUCTION,
    A_INSTRUCTION,
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
}
