using System.Text;


if (args.Length <= 0)
{
  DebugPrinter.Print("No file path was provided");
}

var assembler = new Assembler();
assembler.ReadFile(args[0]);

enum Type
{
  EQUAL,
  SYMBOL,
  VALUE
}

// List<Tuple<string, Type>> Parse(string line)
// {
//   var alist = new List<Tuple<string, Type>>();
//   var text = line.Split('');
//   foreach (var idk in text)
//   {
//     // var val = ToSymbolAndType(idk);
//     // alist.Add(val);
//   }
//
//   return alist;
// }
