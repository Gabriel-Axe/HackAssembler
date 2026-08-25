using HackAssembler;

public class Code
{
  public void OutputFile(List<string> binList)
  {
    using StreamWriter sw = File.CreateText("./Output.hack");
    foreach (var bin in binList)
    {
      DebugPrinter.LogState($"output bin: {bin}");
      sw.WriteLine(bin);
    }
  }

  public string Dest(string val)
  {
    LogInput("dest", val);
    return val switch
    {
      "M" => "001",
      "AM" => "101",
      "D" => "010",
      "DM" =>"011",
      "MD" => "011",
      "AD" => "110",
      "A" => "100",
      "ADM" => "111",
      _ => "000"
    };
  }
  public string Comp(string val)
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

  public string Jump(string val)
  {
    LogInput("jump", val);
    return val switch
    {
      "JGT" => "001",
      "JEQ" => "010",
      "JGE" => "011",
      "JLT" => "100",
      "JNE" => "101",
      "JLE" => "110",
      "JMP" => "111",
      _ => "000",
    };
  }

  private void LogInput(string instType, string val)
  {
    // instType refers to "dest", "comp" and "jump"...
    DebugPrinter.LogAction("code", $"receive {instType} {val} as input");
  }
}
