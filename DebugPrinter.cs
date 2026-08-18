namespace HackAssembler;

static class DebugPrinter
{

  public enum LogLevels {
    INFO = 3,
    DEBUG = 2,
    TRACE = 1
  }
  const LogLevels LOG_LEVEL = LogLevels.TRACE;

  static void LogPattern(string logLevel)
  {
    Console.Write($"{logLevel} > ");
  }

  public static void Log(string message, LogLevels level) 
  {
    if (LOG_LEVEL.Equals(level) || LOG_LEVEL <= level) 
    {

    switch (level)
    {
      case LogLevels.INFO: { LogPattern("INFO"); break; }
      case LogLevels.DEBUG: { LogPattern("DEBUG"); break; }
      case LogLevels.TRACE: { LogPattern("TRACE"); break; }
    }

    Console.Write($"{message} \n");

    }
  }

}
