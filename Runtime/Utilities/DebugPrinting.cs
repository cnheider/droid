namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static class DebugPrinting {
    /// <summary>
    /// </summary>
    /// <param name="debugging"></param>
    /// <param name="configuration"></param>
    /// <param name="identifier"></param>
    public static void ApplyPrint(bool debugging,
                                  droid.Runtime.Interfaces.IConfigurableConfiguration configuration,
                                  string identifier) {
      if (debugging) {
        UnityEngine.Debug.Log(message : "Applying " + configuration + " To " + identifier);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <param name="identifier"></param>
    /// <param name="debugging"></param>
    public static void DisplayPrint(dynamic value, string identifier, bool debugging) {
      if (debugging) {
        UnityEngine.Debug.Log(message : "Applying " + value + " To " + identifier);
      }
    }
  }
}