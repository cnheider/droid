namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  /// </summary>
  public static partial class NeodroidUtilities {
    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static UnityEngine.Vector4 Divide(this UnityEngine.Vector4 a, UnityEngine.Vector4 b) {
      return new UnityEngine.Vector4(x : a.x / b.x,
                                     y : a.y / b.y,
                                     z : a.z / b.z,
                                     w : a.w / b.w);
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static UnityEngine.Vector4 Multiply(this UnityEngine.Vector4 a, UnityEngine.Vector4 b) {
      return new UnityEngine.Vector4(x : a.x * b.x,
                                     y : a.y * b.y,
                                     z : a.z * b.z,
                                     w : a.w * b.w);
    }
  }
}