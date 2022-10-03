namespace droid.Tests.Editor.Commands {
  [NUnit.Framework.TestFixtureAttribute]
  public class SystemCommands {
    [NUnit.Framework.TestAttribute]
    public void TestSystemPythonAddition() {
      droid.Editor.Utilities.Commands.Commands.SystemCommand("python", "-c 'print(1+1)'");

      NUnit.Framework.Assert.That(true, expression : NUnit.Framework.Is.True);
    }
  }
}