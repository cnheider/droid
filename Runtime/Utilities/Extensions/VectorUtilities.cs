namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  /// </summary>
  public static class NeodroidVectorUtilities {
    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3 BroadcastVector3(this float a) {
      return new UnityEngine.Vector3 {x = a, y = a, z = a};
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static UnityEngine.Vector2 BroadcastVector2(this float a) {
      return new UnityEngine.Vector2 {x = a, y = a};
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static UnityEngine.Vector4 BroadcastVector4(this float a) {
      return new UnityEngine.Vector4 {x = a, y = a, z = a};
    }
  }
}