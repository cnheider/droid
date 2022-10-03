namespace droid.Tests.Editor.Commands {
  [NUnit.Framework.TestFixtureAttribute]
  public class PythonCommands {
    [NUnit.Framework.TestAttribute]
    public void TestPythonAddition() {
      droid.Editor.Utilities.Commands.Commands.PythonCommand("print(1+1)");

      NUnit.Framework.Assert.That(true, expression : NUnit.Framework.Is.True);
    }
  }
}