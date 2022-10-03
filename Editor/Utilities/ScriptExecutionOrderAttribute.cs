namespace droid.Editor.Utilities {
  /// <summary>
  /// </summary>
  [System.AttributeUsageAttribute(validOn : System.AttributeTargets.Class)]
  public class ScriptExecutionOrderAttribute : System.Attribute {
    int _order;

    public ScriptExecutionOrderAttribute(int order) { this._order = order; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public int GetOrder() { return this._order; }
  }
}