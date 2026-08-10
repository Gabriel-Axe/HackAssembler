static class DebugPrinter
{
  const bool DEBUG = true;

  public static void DebugPrintLine(string line)
  {
    if (DEBUG)
    {
      Print($"{line}");
    }
  }

  public static void Print(object? value)
  {
    Console.Write(value);
  }

  public static void NewLine()
  {
    Console.WriteLine();
  }
}
