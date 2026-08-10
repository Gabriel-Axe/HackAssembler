using System.Text;


if (args.Length <= 0)
{
  DebugPrinter.Print("No file path was provided");
}

var assembler = new Assembler();
assembler.ReadFile(args[0]);
