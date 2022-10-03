//------------------------------//
//  ProceduralCapsule.cs        //
//  Written by Jay Kay          //
//  2016/05/27                  //
//------------------------------//

namespace droid.Runtime.Utilities.Procedural {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.MeshFilter),
                                requiredComponent2 : typeof(UnityEngine.MeshRenderer))]
  [UnityEngine.ExecuteInEditMode]
  public class ProceduralCapsule : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    public float _Height = 2f;

    /// <summary>
    /// </summary>
    public float _Radius = 0.5f;

    /// <summary>
    /// </summary>
    public int _Segments = 24;

    void Start() {
      var mesh_filter = this.GetComponent<UnityEngine.MeshFilter>();
      if (!mesh_filter.sharedMesh
          || mesh_filter.sharedMesh.vertexCount == 0
          || mesh_filter.sharedMesh.name != "ProceduralCapsule") {
        this.CreateMesh();
      }
    }
    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    [UnityEngine.ContextMenu("Generate Procedural Capsule")]
    public void GenerateProceduralCapsule() { this.CreateMesh(); }
    #endif

    /// <summary>
    /// </summary>
    public void CreateMesh() {
      // make segments an even number
      if (this._Segments % 2 != 0) {
        this._Segments++;
      }

      // extra vertex on the seam
      var points = this._Segments + 1;

      // calculate points around a circle
      var p_x = new float[points];
      var p_z = new float[points];
      var p_y = new float[points];
      var p_r = new float[points];

      var calc_h = 0f;
      var calc_v = 0f;

      for (var i = 0; i < points; i++) {
        p_x[i] = UnityEngine.Mathf.Sin(f : calc_h * UnityEngine.Mathf.Deg2Rad);
        p_z[i] = UnityEngine.Mathf.Cos(f : calc_h * UnityEngine.Mathf.Deg2Rad);
        p_y[i] = UnityEngine.Mathf.Cos(f : calc_v * UnityEngine.Mathf.Deg2Rad);
        p_r[i] = UnityEngine.Mathf.Sin(f : calc_v * UnityEngine.Mathf.Deg2Rad);

        calc_h += 360f / this._Segments;
        calc_v += 180f / this._Segments;
      }

      // - Vertices and UVs -

      var vertices = new UnityEngine.Vector3[points * (points + 1)];
      var uvs = new UnityEngine.Vector2[vertices.Length];
      var ind = 0;

      // Y-offset is half the height minus the diameter
      // float yOff = ( height - ( radius * 2f ) ) * 0.5f;
      var y_off = (this._Height - this._Radius) * 0.5f;
      if (y_off < 0) {
        y_off = 0;
      }

      // uv calculations
      var step_x = 1f / (points - 1);
      float uv_x, uv_y;

      // Top Hemisphere
      var top = UnityEngine.Mathf.CeilToInt(f : points * 0.5f);

      for (var y = 0; y < top; y++) {
        for (var x = 0; x < points; x++) {
          vertices[ind] = new UnityEngine.Vector3(x : p_x[x] * p_r[y], y : p_y[y], z : p_z[x] * p_r[y])
                          * this._Radius;
          vertices[ind].y = y_off + vertices[ind].y;

          uv_x = 1f - step_x * x;
          uv_y = (vertices[ind].y + this._Height * 0.5f) / this._Height;
          uvs[ind] = new UnityEngine.Vector2(x : uv_x, y : uv_y);

          ind++;
        }
      }

      // Bottom Hemisphere
      var btm = UnityEngine.Mathf.FloorToInt(f : points * 0.5f);

      for (var y = btm; y < points; y++) {
        for (var x = 0; x < points; x++) {
          vertices[ind] = new UnityEngine.Vector3(x : p_x[x] * p_r[y], y : p_y[y], z : p_z[x] * p_r[y])
                          * this._Radius;
          vertices[ind].y = -y_off + vertices[ind].y;

          uv_x = 1f - step_x * x;
          uv_y = (vertices[ind].y + this._Height * 0.5f) / this._Height;
          uvs[ind] = new UnityEngine.Vector2(x : uv_x, y : uv_y);

          ind++;
        }
      }

      // - Triangles -

      var triangles = new int[this._Segments * (this._Segments + 1) * 2 * 3];

      for (int y = 0, t = 0; y < this._Segments + 1; y++) {
        for (var x = 0; x < this._Segments; x++, t += 6) {
          triangles[t + 0] = (y + 0) * (this._Segments + 1) + x + 0;
          triangles[t + 1] = (y + 1) * (this._Segments + 1) + x + 0;
          triangles[t + 2] = (y + 1) * (this._Segments + 1) + x + 1;

          triangles[t + 3] = (y + 0) * (this._Segments + 1) + x + 1;
          triangles[t + 4] = (y + 0) * (this._Segments + 1) + x + 0;
          triangles[t + 5] = (y + 1) * (this._Segments + 1) + x + 1;
        }
      }

      // - Assign Mesh -

      var mf = this.gameObject.GetComponent<UnityEngine.MeshFilter>();
      var mesh = mf.sharedMesh;
      if (!mesh) {
        mesh = new UnityEngine.Mesh();
        mf.sharedMesh = mesh;
      }

      mesh.Clear();

      mesh.name = "ProceduralCapsule";

      mesh.vertices = vertices;
      mesh.uv = uvs;
      mesh.triangles = triangles;

      mesh.RecalculateBounds();
      mesh.RecalculateNormals();
    }
  }
}