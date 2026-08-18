using HackAssembler;
using Level = HackAssembler.DebugPrinter.LogLevels;

public class Code
{
  private Int16 CurAddress { get; set; } = 0;
  private Int16 CurOutput { get; set; } = 0;
  private List<Int16> Outputs  { get; set; } = new();
  private bool InMemory {get; set;} = false;

  public void outputFile(List<string> binList)
  {
    using (StreamWriter sw = File.CreateText("./Output.hack"))
    {
      // DebugPrinter.PrintValue(binList.Any());
      foreach (var bin in binList)
      {
        DebugPrinter.Log($"output bin: {bin}", Level.TRACE);
        sw.WriteLine(bin);
      }
    }
  }

  public string dest(string val)
  {
    DebugPrinter.Log($"received dest {val} as val", Level.DEBUG);
    switch (val)
    {
      case "M":
        return "001";
      case "AM":
        return "101";
      case "D":
        return "010";
      case "DM":
      case "MD":
        return "011";
      case "AD":
        return "110";
      case "A":
        return "100";
      case "ADM":
        return "111";
      default:
        return "000";
    }
  }

  public string comp(string val)
  {
    if (val == "0") return "0101010";
    if (val == "1") return "0111111";
    if (val == "-1") return "0111010";
    if (val == "D") return "0001100";
    if (val == "A") return "0110000";
    if (val == "M") return "1110000";
    if (val == "!A") return "0110001";
    if (val == "!D") return "0001101";
    if (val == "!M") return "1110001";
    if (val == "-D") return "0001111";
    if (val == "-A") return "0110011";
    if (val == "-M") return "1110011";
    if (val == "D+1") return "0011111";
    if (val == "A+1") return "0110111";
    if (val == "M+1") return "1110111";
    if (val == "D-1") return "0001110";
    if (val == "A-1") return "0110010";
    if (val == "M-1") return "1110010";
    if (val == "D+A") return "0000010";
    if (val == "D+M") return "1000010";
    if (val == "D-A") return "0010011";
    if (val == "D-M") return "1010011";
    if (val == "A-D") return "0000111";
    if (val == "M-D") return "1000111";
    if (val == "D&A") return "0000000";
    if (val == "D&M") return "1000000";
    if (val == "D|A") return "0010101";
    if (val == "D|M") return "1010101";
    return "000000";
  }
  public string jump(string val)
  {
    DebugPrinter.Log($"received jump {val} as val", Level.DEBUG);
    switch (val)
    {
      case "JGT":
        return "001";
      case "JEQ":
        return "010";
      case "JGE":
        return "011";
      case "JLT":
        return "100";
      case "JNE":
        return "101";
      case "JLE":
        return "110";
      case "JMP":
        return "111";
      default:
        return "000";
    }
  }
}
