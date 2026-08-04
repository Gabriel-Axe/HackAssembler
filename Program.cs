using System.Text;

string readContents;
var file = new FileInfo("./TestFiles/add/Add.asm");

using (StreamReader streamReader = new StreamReader(file.FullName, Encoding.UTF8))
{
  readContents = streamReader.ReadToEnd();
}

Console.WriteLine(readContents);
