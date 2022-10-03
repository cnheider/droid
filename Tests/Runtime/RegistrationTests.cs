namespace droid.Tests.Runtime {
  /// <summary>
  /// </summary>
  [NUnit.Framework.TestFixtureAttribute]
  public class RegistrationTests {
    /// <summary>
    /// </summary>
    [NUnit.Framework.TestAttribute]
    public void RegistrationNameCheck() {
      const string go_name = "MyGameObject";
      var go = new UnityEngine.GameObject(name : go_name);
      NUnit.Framework.Assert.AreEqual(expected : go_name, actual : go.name);
    }
  }
}