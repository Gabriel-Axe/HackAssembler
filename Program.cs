using System.Text;

string readContents;
var file = new FileInfo("./TestFiles/add/Add.asm");

using (StreamReader streamReader = new StreamReader(file.FullName, Encoding.UTF8))
{
  while (!streamReader.EndOfStream) 
  {
    // readContents = streamReader.ReadToEnd();
    var line = streamReader.ReadLine();
    Console.WriteLine($"{line}~");
  }
}

// foreach (var line in readContents)
// {
//   Console.WriteLine($"{line} \n");
// }
// Console.WriteLine(readContents);
