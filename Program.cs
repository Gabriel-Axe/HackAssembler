var file = new FileInfo("./TestFiles/add/Add.asm");
Console.WriteLine(file.FullName);
var lines = File.ReadAllText(file.FullName);
Console.WriteLine(lines);
