using System.Text;

const bool DEBUG = true;

string readContents;
var file = new FileInfo("./TestFiles/add/Add.asm");

using (StreamReader streamReader = new StreamReader(file.FullName, Encoding.UTF8))
{
  while (!streamReader.EndOfStream) 
  {
    var line = streamReader.ReadLine();
    interpret(line);
  }
}

void interpret(string line) {
  if (!line.Any() || line.StartsWith("//")) return;
  if (line.StartsWith("@"))
  {
    int address = Int32.Parse(line.Substring(1));
    Print(Convert.ToString(address, 2).PadLeft(16, '0'));
  }
  if (DEBUG)
  {
    Print($": {line}");
  }
  NewLine();
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
