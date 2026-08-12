using System.Text;
using System.Text.Unicode;

public class Assembler
{
  private InstructionType readingInstruction { get; set; } = 0;

  private Int16 CurAddress { get; set; } = 0;
  private Int16 CurOutput { get; set; } = 0;
  private List<Int16> Outputs  { get; set; } = new();

  public void Execute(string path)
  {
    ReadCompiledFile(path);
    // ReadFile(path);
    // Parse(currentFile);
    // foreach (var output in Outputs)
    // {
    //   Console.WriteLine(output);
    // }

    // OutputFile("./Output.asm");
  }
}
