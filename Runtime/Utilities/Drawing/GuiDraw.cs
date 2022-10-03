namespace droid.Runtime.Utilities.Drawing {
  /// <summary>
  /// </summary>
  public static partial class NeodroidUtilities {
    static UnityEngine.Texture2D _s_line_tex;

    static NeodroidUtilities() {
      _s_line_tex = new UnityEngine.Texture2D(1,
                                              3,
                                              textureFormat : UnityEngine.TextureFormat.ARGB32,
                                              true);
      _s_line_tex.SetPixel(0,
                           0,
                           color : new UnityEngine.Color(1,
                                                         1,
                                                         1,
                                                         0));
      _s_line_tex.SetPixel(0, 1, color : UnityEngine.Color.white);
      _s_line_tex.SetPixel(0,
                           2,
                           color : new UnityEngine.Color(1,
                                                         1,
                                                         1,
                                                         0));
      _s_line_tex.Apply();
    }

    /// <summary>
    /// </summary>
    /// <param name="p_point_a"></param>
    /// <param name="p_point_b"></param>
    /// <param name="p_width"></param>
    public static void DrawLine(UnityEngine.Vector2 p_point_a, UnityEngine.Vector2 p_point_b, float p_width) {
      var save_matrix = UnityEngine.GUI.matrix;
      var save_color = UnityEngine.GUI.color;

      var delta = p_point_b - p_point_a;
      UnityEngine.GUIUtility.ScaleAroundPivot(scale : new UnityEngine.Vector2(x : delta.magnitude,
                                                y : p_width),
                                              pivotPoint : UnityEngine.Vector2.zero);
      UnityEngine.GUIUtility.RotateAroundPivot(angle : UnityEngine.Vector2.Angle(from : delta,
                                                         to : UnityEngine.Vector2.right)
                                                       * UnityEngine.Mathf.Sign(f : delta.y),
                                               pivotPoint : UnityEngine.Vector2.zero);
      UnityEngine.GUI.matrix =
          UnityEngine.Matrix4x4.TRS(pos : p_point_a,
                                    q : UnityEngine.Quaternion.identity,
                                    s : UnityEngine.Vector3.one)
          * UnityEngine.GUI.matrix;

      UnityEngine.GUI.DrawTexture(position : new UnityEngine.Rect(position : UnityEngine.Vector2.zero,
                                                                  size : UnityEngine.Vector2.one),
                                  image : _s_line_tex);

      UnityEngine.GUI.matrix = save_matrix;
      UnityEngine.GUI.color = save_color;
    }
  }
}