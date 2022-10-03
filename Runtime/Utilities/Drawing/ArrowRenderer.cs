namespace droid.Runtime.Utilities.Drawing {
  // Put this script on a Camera
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class ArrowRenderer : UnityEngine.MonoBehaviour {
    // Fill/drag these in from the editor

    // Choose the Unlit/Color shader in the Material Settings
    // You can change that color, to change the color of the connecting lines
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Material _line_mat = null;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.GameObject _main_point = null;

    [UnityEngine.SerializeField] float _offset = 0.3f;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.GameObject[] _points = null;

    [UnityEngine.SerializeField] UnityEngine.Vector3[] _vec3_points = null;

    // To show the lines in the editor
    void OnDrawGizmos() {
      this._vec3_points =
          System.Linq.Enumerable.ToArray(source : System.Linq.Enumerable.Select(source : this._points,
                                           v => v.transform.position));
      var s =
          System.Linq.Enumerable.ToArray(source : System.Linq.Enumerable.Select(source : this._vec3_points,
                                           v =>
                                               new System.Tuple<UnityEngine.Vector3, UnityEngine.Vector3
                                               >(item1 : this._main_point.transform.position, item2 : v)));
      this.DrawConnectingLines(vec_pairs : s);
    }

    // To show the lines in the game window whne it is running
    void OnPostRender() {
      this._vec3_points =
          System.Linq.Enumerable.ToArray(source : System.Linq.Enumerable.Select(source : this._points,
                                           v => v.transform.position));
      var s =
          System.Linq.Enumerable.ToArray(source : System.Linq.Enumerable.Select(source : this._vec3_points,
                                           v =>
                                               new System.Tuple<UnityEngine.Vector3, UnityEngine.Vector3
                                               >(item1 : this._main_point.transform.position, item2 : v)));
      this.DrawConnectingLines(vec_pairs : s);
    }

    // Connect all of the `points` to the `main_point_pos`
    void DrawConnectingLines(
        System.Collections.Generic.IReadOnlyCollection<System.Tuple<UnityEngine.Vector3, UnityEngine.Vector3>>
            vec_pairs) {
      if (vec_pairs.Count > 0) {
        // Loop through each point to connect to the mainPoint
        foreach (var point in vec_pairs) {
          var main_point_pos = point.Item1;
          var point_pos = point.Item2;

          UnityEngine.GL.Begin(mode : UnityEngine.GL.LINES);
          this._line_mat.SetPass(0);
          UnityEngine.GL.Color(c : new UnityEngine.Color(r : this._line_mat.color.r,
                                                         g : this._line_mat.color.g,
                                                         b : this._line_mat.color.b,
                                                         a : this._line_mat.color.a));
          UnityEngine.GL.Vertex3(x : main_point_pos.x, y : main_point_pos.y, z : main_point_pos.z);
          UnityEngine.GL.Vertex3(x : point_pos.x, y : point_pos.y, z : point_pos.z);
          //
          UnityEngine.GL.Vertex3(x : point_pos.x - this._offset, y : point_pos.y, z : point_pos.z);
          UnityEngine.GL.Vertex3(x : point_pos.x, y : point_pos.y - this._offset, z : point_pos.z);
          UnityEngine.GL.Vertex3(x : point_pos.x, y : point_pos.y, z : point_pos.z - this._offset);
          UnityEngine.GL.Vertex3(x : point_pos.x + this._offset, y : point_pos.y, z : point_pos.z);
          UnityEngine.GL.Vertex3(x : point_pos.x, y : point_pos.y + this._offset, z : point_pos.z);
          UnityEngine.GL.Vertex3(x : point_pos.x, y : point_pos.y, z : point_pos.z + this._offset);
          //
          UnityEngine.GL.End();
        }
      }
    }
  }
}