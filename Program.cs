using System.Text;
using Level = HackAssembler.DebugPrinter.LogLevels;
using HackAssembler;

public class Program
{
  public static void Main(string[] args)
  {
    if (args.Length <= 0)
    {
      DebugPrinter.Log("No file path was provided", Level.INFO);
    }

    var assembler = new Assembler(args[0]);
    assembler.Execute();
  }
}
