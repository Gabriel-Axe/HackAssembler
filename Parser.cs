public class Parser
{

  private FileInfo currentFile { get; set; } = null;
  private int Line { get; set; } = 0;
  private int Col { get; set; } = 0;

  /// <summary>
  /// Reads the specified file and changes the internal Assembler state
  /// </summary>
  private void Parse(FileInfo file) {
    using (StreamReader streamReader = new StreamReader(file.FullName, Encoding.UTF8))
    {
      DebugPrinter.Print($"Interpreting {currentFile.FullName}\n");
      while (!streamReader.EndOfStream) 
      {
        var line = streamReader.ReadLine();
        if (!line.Any() || line.StartsWith("//")) continue;

        if (line.StartsWith("@")) ReadAInstruction(line);
        else ReadCInstruction(line);
      }
    }
  }
  private void ReadFile(string path)
  {
    this.currentFile = new FileInfo(path);
    if (!currentFile.Exists)
    {
      DebugPrinter.Print($"The file {currentFile.FullName} doesn't exist");
      Environment.Exit(1);
    }
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
