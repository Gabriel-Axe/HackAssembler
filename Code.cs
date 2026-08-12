public class Code
{
  private Int16 CurAddress { get; set; } = 0;
  private Int16 CurOutput { get; set; } = 0;
  private List<Int16> Outputs  { get; set; } = new();
  private bool InMemory {get; set;} = false;

  public void outputFile(List<string> binList)
  {
    using (StreamWriter sw = File.CreateText("./Output.asm"))
    {
      // DebugPrinter.PrintValue(binList.Any());
      foreach (var bin in binList)
      {
        DebugPrinter.PrintValue($": Outputing {bin}");
        sw.WriteLine(bin);
      }
    }
  }

  // public string address(string val)
  // {
  // }

  public string dest(string val)
  {
    switch (val)
    {
      case "M":
        return "001";
      case "D":
        return "010";
      case "A":
        return "100";
      default:
        return "000";
    }
  }

  public string comp(string val)
  {
    if (val == "0") return "101010";
    if (val == "1") return "111111";
    if (val == "D") return "001100";
    if (val == "A") return "110000";
    if (val == "!A") return "110001";
    if (val == "!D") return "001101";
    if (val == "-D") return "001111";
    if (val == "-A") return "110011";
    if (val == "D+1") return "011111";
    if (val == "A+1") return "110111";
    if (val == "D-1") return "001110";
    if (val == "A-1") return "110010";
    if (val == "D+A") return "000010";
    if (val == "D-A") return "010011";
    if (val == "A-D") return "000111";
    if (val == "D&A") return "000000";
    if (val == "D|A") return "010101";
    return "000000";
  }
  public string jump(string val)
  {
    switch (val)
    {
      case "JGT":
        return "001";
      case "JEQ":
        return "010";
      case "JGE":
        return "100";
      case "JLT":
        return "101";
      case "JNE":
        return "110";
      case "JMP":
        return "111";
      default:
        return "000";
    }
  }
}
