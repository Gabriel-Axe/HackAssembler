public class Code
{
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

  private enum InstructionType
  {
    C_INSTRUCTION,
    A_INSTRUCTION,
  }

  private enum JumpType
  {
    JGT,
    JEQ,
    JGE,
    JLT,
    JLE,
    JMP
  }

  private enum RegisterType
  {
    D,
    A,
    M,
  }
}
