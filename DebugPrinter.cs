namespace HackAssembler;

static class DebugPrinter
{

  private enum LogLevels {
    INFO = 3,
    DEBUG = 2,
    TRACE = 1,
    VERBOSE = 0,
  }
  const LogLevels LOG_LEVEL = LogLevels.TRACE;

  static void LogPattern(string logLevel)
  {
    Console.Write($"{logLevel} > ");
  }

  public static void LogState(string message)
  {
    Log(message, LogLevels.DEBUG);
  }

  public static void LogAction(string scope, string what)
  {
    Log($"{scope}: {what}", LogLevels.TRACE);
  }

  public static void LogError(string why)
  {
    Log($"[error] {why}", LogLevels.DEBUG);
}

  private static void Log(string message, LogLevels level) 
  {
    if (LOG_LEVEL.Equals(level) || LOG_LEVEL <= level) 
    {

    switch (level)
    {
      case LogLevels.INFO: { LogPattern("INFO"); break; }
      case LogLevels.DEBUG: { LogPattern("DEBUG"); break; }
      case LogLevels.TRACE: { LogPattern("TRACE"); break; }
      case LogLevels.VERBOSE: { LogPattern("VERBOSE"); break; }
    }

    Console.Write($"{message} \n");

    }
  }

}
