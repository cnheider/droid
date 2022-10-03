namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public static class NeodroidDefaultsUtilities {
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public static UnityEngine.AnimationCurve DefaultAnimationCurve() {
      return new UnityEngine.AnimationCurve(new UnityEngine.Keyframe(1, 1), new UnityEngine.Keyframe(0, 0));
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public static UnityEngine.Gradient DefaultGradient() {
      var gradient = new UnityEngine.Gradient {
                                                  // The number of keys must be specified in this array initialiser
                                                  colorKeys = new[] {
                                                                        // Add your colour and specify the stop point
                                                                        new UnityEngine.
                                                                            GradientColorKey(col : new
                                                                                  UnityEngine.Color(1,
                                                                                    1,
                                                                                    1),
                                                                              0),
                                                                        new UnityEngine.
                                                                            GradientColorKey(col : new
                                                                                  UnityEngine.Color(1,
                                                                                    1,
                                                                                    1),
                                                                              1f),
                                                                        new UnityEngine.
                                                                            GradientColorKey(col : new
                                                                                  UnityEngine.Color(1,
                                                                                    1,
                                                                                    1),
                                                                              0)
                                                                    },
                                                  // This sets the alpha to 1 at both ends of the gradient
                                                  alphaKeys = new[] {
                                                                        new UnityEngine.GradientAlphaKey(1,
                                                                          0),
                                                                        new UnityEngine.GradientAlphaKey(1,
                                                                          1),
                                                                        new UnityEngine.GradientAlphaKey(1, 0)
                                                                    }
                                              };

      return gradient;
    }
  }
}