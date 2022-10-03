namespace droid.Tests.Runtime {
  /// <summary>
  /// </summary>
  [NUnit.Framework.TestFixtureAttribute]
  public class SanityTests {
    /// <summary>
    /// </summary>
    [NUnit.Framework.TestAttribute]
    public void Sanity() { NUnit.Framework.Assert.That(true, expression : NUnit.Framework.Is.True); }
  }
}