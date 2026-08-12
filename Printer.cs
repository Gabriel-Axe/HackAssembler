static class DebugPrinter
{
  const bool DEBUG = true;

  public static void DebugPrintLine(string line)
  {
    if (DEBUG)
    {
      PrintValue($"{line}");
    }
  }

  public static void PrintValue(object? value)
  {
    Console.WriteLine(value);
  }

  public static void NewLine()
  {
    Console.WriteLine();
  }
}
