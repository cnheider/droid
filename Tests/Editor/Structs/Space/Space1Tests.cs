namespace droid.Tests.Editor.Structs.Space {
  /// <summary>
  /// </summary>
  [NUnit.Framework.TestFixtureAttribute]
  public class Space1Tests {
    /// <summary>
    /// </summary>
    [NUnit.Framework.TestCaseAttribute(0,
                                       0,
                                       0,
                                       ExpectedResult = 0f)]
    [NUnit.Framework.TestCaseAttribute(1,
                                       0,
                                       0,
                                       ExpectedResult = 0f)]
    [NUnit.Framework.TestCaseAttribute(0,
                                       0,
                                       10,
                                       ExpectedResult = 0f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       0,
                                       1,
                                       ExpectedResult = 0.5f)]
    [NUnit.Framework.TestCaseAttribute(0.1f,
                                       0,
                                       10,
                                       ExpectedResult = 1f)]
    [NUnit.Framework.TestCaseAttribute(1.0f,
                                       0,
                                       10,
                                       ExpectedResult = 10f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       5,
                                       10,
                                       ExpectedResult = 7.5f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       -10,
                                       10,
                                       ExpectedResult = 0f)]
    [NUnit.Framework.TestCaseAttribute(0.75f,
                                       -10,
                                       10,
                                       ExpectedResult = 5f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       -10,
                                       -5,
                                       ExpectedResult = -7.5f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       -10,
                                       0,
                                       ExpectedResult = -5f)]
    public float TestDenormalise01(float v, float min_value, float max_value) {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = min_value,
                                                             Max = max_value,
                                                             Normalised =
                                                                 droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                                                             DecimalGranularity = 2
                                                         };

      return space.Reproject(v : v);
    }

    /// <summary>
    /// </summary>
    [NUnit.Framework.TestCaseAttribute(5,
                                       0,
                                       10,
                                       ExpectedResult = 0.5f)]
    [NUnit.Framework.TestCaseAttribute(0.2f,
                                       0,
                                       1,
                                       ExpectedResult = 0.2f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       -1,
                                       1,
                                       ExpectedResult = 0.75f)]
    [NUnit.Framework.TestCaseAttribute(-0.8f,
                                       -1,
                                       0,
                                       ExpectedResult = 0.2f)]
    public float TestNormalise01(float v, float min_value, float max_value) {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = min_value,
                                                             Max = max_value,
                                                             DecimalGranularity = 2,
                                                             Normalised = droid.Runtime.Enums.ProjectionEnum
                                                                 .Zero_one_
                                                         };

      //Assert.AreEqual(expected, result, tolerance);
      return space.Project(v : v);
    }

    [NUnit.Framework.TestCaseAttribute(-1)]
    [NUnit.Framework.TestCaseAttribute(11)]
    public void TestDenormalise010Throws(float v) {
      var space =
          new droid.Runtime.Structs.Space.Space1 {
                                                     Min = 0,
                                                     Max = 10,
                                                     Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_
                                                 };

      NUnit.Framework.Assert.That(() => space.Reproject(v : v),
                                  expr : NUnit.Framework.Throws.TypeOf<System.ArgumentException>());
    }

    [NUnit.Framework.TestCaseAttribute(-1)]
    [NUnit.Framework.TestCaseAttribute(10.1f)]
    public void TestNormalise010Throws(float v) {
      var space =
          new droid.Runtime.Structs.Space.Space1 {
                                                     Min = 0,
                                                     Max = 10,
                                                     Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_
                                                 };

      NUnit.Framework.Assert.That(() => space.Project(v : v),
                                  expr : NUnit.Framework.Throws.TypeOf<System.ArgumentException>());
    }

    [NUnit.Framework.TestCaseAttribute(-2)]
    [NUnit.Framework.TestCaseAttribute(2)]
    public void TestDenormaliseMinusOneOne010Throws(float v) {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = 0,
                                                             Max = 10,
                                                             Normalised = droid.Runtime.Enums.ProjectionEnum
                                                                 .Minus_one_one_
                                                         };

      NUnit.Framework.Assert.That(() => space.Reproject(v : v),
                                  expr : NUnit.Framework.Throws.TypeOf<System.ArgumentException>());
    }

    [NUnit.Framework.TestCaseAttribute(-1)]
    [NUnit.Framework.TestCaseAttribute(10.1f)]
    public void TestNormaliseMinusOneOne010Throws(float v) {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = 0,
                                                             Max = 10,
                                                             Normalised = droid.Runtime.Enums.ProjectionEnum
                                                                 .Minus_one_one_
                                                         };

      NUnit.Framework.Assert.That(() => space.Project(v : v),
                                  expr : NUnit.Framework.Throws.TypeOf<System.ArgumentException>());
    }

    /// <summary>
    /// </summary>
    [NUnit.Framework.TestCaseAttribute(0,
                                       0,
                                       0,
                                       ExpectedResult = 0f)]
    [NUnit.Framework.TestCaseAttribute(1,
                                       0,
                                       0,
                                       ExpectedResult = 0f)]
    [NUnit.Framework.TestCaseAttribute(-1.0f,
                                       0,
                                       10,
                                       ExpectedResult = 0)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       0,
                                       1,
                                       ExpectedResult = 0.75f)]
    [NUnit.Framework.TestCaseAttribute(0.1f,
                                       0,
                                       10,
                                       ExpectedResult = 5.5f)]
    [NUnit.Framework.TestCaseAttribute(1.0f,
                                       0,
                                       10,
                                       ExpectedResult = 10f)]
    [NUnit.Framework.TestCaseAttribute(0f,
                                       5,
                                       10,
                                       ExpectedResult = 7.5f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       -10,
                                       10,
                                       ExpectedResult = 5.0f)]
    [NUnit.Framework.TestCaseAttribute(0.75f,
                                       -10,
                                       10,
                                       ExpectedResult = 7.5f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       -10,
                                       -5,
                                       ExpectedResult = -6.25f)]
    [NUnit.Framework.TestCaseAttribute(0.5f,
                                       -10,
                                       0,
                                       ExpectedResult = -2.5f)]
    public float TestDenormaliseMinusOneOne(float v, float min_value, float max_value) {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = min_value,
                                                             Max = max_value,
                                                             Normalised =
                                                                 droid.Runtime.Enums.ProjectionEnum
                                                                      .Minus_one_one_,
                                                             DecimalGranularity = 2
                                                         };

      return space.Reproject(v : v);
    }

    /// <summary>
    /// </summary>
    [NUnit.Framework.TestCaseAttribute(5,
                                       0,
                                       10,
                                       ExpectedResult = 0.0f)]
    [NUnit.Framework.TestCaseAttribute(0f,
                                       0,
                                       1,
                                       ExpectedResult = -1.0f)]
    [NUnit.Framework.TestCaseAttribute(1f,
                                       -1,
                                       1,
                                       ExpectedResult = 1.0f)]
    [NUnit.Framework.TestCaseAttribute(-0.5f,
                                       -1,
                                       0,
                                       ExpectedResult = 0f)]
    public float TestNormaliseMinusOneOne(float v, float min_value, float max_value) {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = min_value,
                                                             Max = max_value,
                                                             DecimalGranularity = 2,
                                                             Normalised = droid.Runtime.Enums.ProjectionEnum
                                                                 .Minus_one_one_
                                                         };

      //Assert.AreEqual(expected, result, tolerance);
      return space.Project(v : v);
    }

    [NUnit.Framework.TestAttribute]
    public void TestClipDenormalise01RoundClip() {
      var space =
          new droid.Runtime.Structs.Space.Space1 {
                                                     Min = -1,
                                                     Max = 1,
                                                     Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_
                                                 };

      NUnit.Framework.Assert.That(System.Math.Abs(space.Reproject(0.5f) - 0.0f),
                                  NUnit.Framework.Is.LessThanOrEqualTo(expected : float.Epsilon));
    }

    /// <summary>
    /// </summary>
    [NUnit.Framework.TestAttribute]
    public void TestDenormaliseMinus11() {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = -1f,
                                                             Max = 1f,
                                                             Normalised =
                                                                 droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                                                             DecimalGranularity = 1
                                                         };

      var d = space.Reproject(0.75f);
      NUnit.Framework.Assert.That(System.Math.Abs(d - 0.5f),
                                  NUnit.Framework.Is.LessThanOrEqualTo(expected : float.Epsilon));
    }

    /// <summary>
    /// </summary>
    [NUnit.Framework.TestAttribute]
    public void TestNormaliseMinus11() {
      var space = new droid.Runtime.Structs.Space.Space1 {
                                                             Min = -1,
                                                             Max = 1,
                                                             Normalised =
                                                                 droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                                                             DecimalGranularity = 2
                                                         };

      NUnit.Framework.Assert.That(System.Math.Abs(space.Project(0.5f) - 0.75f),
                                  NUnit.Framework.Is.LessThanOrEqualTo(expected : float.Epsilon));
    }
  }
}