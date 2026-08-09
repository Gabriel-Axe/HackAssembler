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
  if (DEBUG)
  {
    Print(line);
  }
}

void Print(object? value)
{
  Console.WriteLine(value);
}

// foreach (var line in readContents)
// {
//   Console.WriteLine($"{line} \n");
// }
// Console.WriteLine(readContents);
