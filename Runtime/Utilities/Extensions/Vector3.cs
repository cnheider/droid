namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  /// </summary>
  public static partial class NeodroidUtilities {
    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 Divide(this UnityEngine.Vector3 a, UnityEngine.Vector3 b) {
      return new UnityEngine.Vector3(x : a.x / b.x, y : a.y / b.y, z : a.z / b.z);
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 Multiply(this UnityEngine.Vector3 a, UnityEngine.Vector3 b) {
      return new UnityEngine.Vector3(x : a.x * b.x, y : a.y * b.y, z : a.z * b.z);
    }

    /// <summary>
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="min_point"></param>
    /// <param name="max_point"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 Clamp(this ref UnityEngine.Vector3 vec,
                                            UnityEngine.Vector3 min_point,
                                            UnityEngine.Vector3 max_point) {
      vec.x = UnityEngine.Mathf.Clamp(value : vec.x, min : min_point.x, max : max_point.x);
      vec.y = UnityEngine.Mathf.Clamp(value : vec.y, min : min_point.y, max : max_point.y);
      vec.z = UnityEngine.Mathf.Clamp(value : vec.z, min : min_point.z, max : max_point.z);
      return vec;
    }
  }
}