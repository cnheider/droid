namespace droid.Runtime.GameObjects.BoundingBoxes.Experimental {
  /// <summary>
  /// </summary>
  public static class VectorSpace {
    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [System.Runtime.CompilerServices.MethodImplAttribute(methodImplOptions : System.Runtime.CompilerServices
                                                                                   .MethodImplOptions.AggressiveInlining)]
    public static void GetMinMax(this UnityEngine.Vector2 point,
                                 ref UnityEngine.Vector2 min,
                                 ref UnityEngine.Vector2 max) {
      min = new UnityEngine.Vector2(x : min.x >= point.x ? point.x : min.x,
                                    y : min.y >= point.y ? point.y : min.y);
      max = new UnityEngine.Vector2(x : max.x <= point.x ? point.x : max.x,
                                    y : max.y <= point.y ? point.y : max.y);
    }

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    [System.Runtime.CompilerServices.MethodImplAttribute(methodImplOptions : System.Runtime.CompilerServices
                                                                                   .MethodImplOptions.AggressiveInlining)]
    public static void GetMinMax(this UnityEngine.Vector3 point,
                                 ref UnityEngine.Vector3 min,
                                 ref UnityEngine.Vector3 max) {
      min = new UnityEngine.Vector3(x : min.x >= point.x ? point.x : min.x,
                                    y : min.y >= point.y ? point.y : min.y,
                                    z : min.z >= point.z ? point.z : min.z);
      max = new UnityEngine.Vector3(x : max.x <= point.x ? point.x : max.x,
                                    y : max.y <= point.y ? point.y : max.y,
                                    z : max.z <= point.z ? point.z : max.z);
    }
  }
}