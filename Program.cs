using System.Text;

const bool DEBUG = true;

int i_line = 0;
int col = 0;

if (args.Length <= 0)
{
  Print("No file path was provided");
  return;
}
var file = new FileInfo(args[0]);
if (!file.Exists)
{
  Print($"The file {file.FullName} doesn't exist");
  return;
}

interpret(file);

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
    }
  }
}

enum Type
{
  EQUAL,
  SYMBOL,
  VALUE
}

Tuple(string, Type) Parse(string line)
{
  var alist = new List<Tuple<string, Type>>();
  var text = line.Split('');
  foreach (var idk in text)
  {
    var val = ToSymbolAndType(idk);
    alist.Add(val);
  }

  return val;
}

Tuple<string, Type> ToSymbolAndType(string text) 
{
}

void interpret_a_instruction(string line)
{
  int address = Int32.Parse(line.Substring(1));
  Print(Convert.ToString(address, 2).PadLeft(16, '0'));
}

void debug_line(string line)
{
  if (DEBUG)
  {
    Print($": {line}");
  }
}

void interpret_c_instruction(string line)
{
  int c_initial = 0b1110_0000_0000_0000;
  for (col; col < line.Length; col++)
  {
    if (!Char.IsLetter(line[col+1]))
    {
      get_token(line.Substring(col));
    }
  }
  // string dest = line[0].ToString();
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

void Print(object? value)
{
  Console.Write(value);
}

void NewLine()
{
  Console.WriteLine();
}

// foreach (var line in readContents)
// {
//   Console.WriteLine($"{line} \n");
// }
// Console.WriteLine(readContents);

