using System.Text;

public class Program
{
  public static void Main(string[] args)
  {
    if (args.Length <= 0)
    {
      DebugPrinter.PrintValue("No file path was provided");
    }

    var assembler = new Assembler(args[0]);
    assembler.Execute();
  }
}
