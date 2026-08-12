public class Code
{
  private InstructionType readingInstruction { get; set; } = 0;

  private Int16 CurAddress { get; set; } = 0;
  private Int16 CurOutput { get; set; } = 0;
  private List<Int16> Outputs  { get; set; } = new();
  private bool InMemory {get; set;} = false;
  private JumpType? _JumpType { get; set; } = null;
  private RegisterType? _RegisterType { get; set; } = null;

  private void OutputFile(string path)
  {
    using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(path)))
    {
      foreach (var instruction in Outputs)
      {
        writer.Write(instruction);
      }
    }
  }

  string dest(string val)
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
  string comp(string val)
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
  }
  string jump(string val)
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

  void BuildCInstruction()
  {
    int c_initial = 0b1110_0000_0000_0000;
    if (InMemory) c_initial = c_initial | 4096;
    switch (_RegisterType)
    {
      case RegisterType.D:
        c_initial = c_initial | 16;
        break;
      case RegisterType.A:
        c_initial = c_initial | 32;
        break;
      case RegisterType.M:
        c_initial = c_initial | 8;
        break;
    }
    CurOutput = (short) c_initial;
    Outputs.Add((short) c_initial);
  }

  void ReadAInstruction(string line)
  {
    Int16 address = Int16.Parse(line.Substring(1));
    CurOutput = address;
    Outputs.Add(address);
  }
}
