using System.Text;

public class Program
{
  public static void Main(string[] args)
  {
    if (args.Length <= 0)
    {
      DebugPrinter.Print("No file path was provided");
    }

    var assembler = new Assembler();
    assembler.Execute(args[0]);
  }
}
