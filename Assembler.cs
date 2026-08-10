public class Assembler
{

  private InstructionType readingInstruction { get; set; }
  private Int16 address { get; set; }

  /// <summary>
  /// Reads the specified file and changes the internal Assembler state
  /// </summary>
  void interpret(FileInfo file) {
    using (StreamReader streamReader = new StreamReader(file.FullName, Encoding.UTF8))
    {
      Print($"Interpreting {file.FullName}\n");
      while (!streamReader.EndOfStream) 
      {
        var line = streamReader.ReadLine();
        // Print($"line: {line}");
        if (!line.Any() || line.StartsWith("//")) continue;
        if (line.StartsWith("@")) {
          interpret_a_instruction(line);
        }
        else {
          interpret_c_instruction(line);
        }
        debug_line(line);
        NewLine();
        this.interpret()
      }
    }
  }

  private enum InstructionType
  {
    C_INSTRUCTION,
    A_INSTRUCTION,
  }
}
