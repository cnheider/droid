namespace droid.Runtime.Utilities.Drawing {
  public class CylinderLineRenderer : UnityEngine.MonoBehaviour {
    // Fill in this with the default Unity Cylinder mesh
    // We will account for the cylinder pivot/origin being in the middle.
    /// <summary>
    /// </summary>
    public UnityEngine.Mesh _CylinderMesh;

    // Material used for the connecting lines
    public UnityEngine.Material _LineMat;

    // Connect all of the `points` to the `mainPoint`
    public UnityEngine.GameObject _MainPoint;
    public UnityEngine.GameObject[] _Points;

    public float _Radius = 0.05f;

    UnityEngine.GameObject[] _ring_game_objects;

    // Use this for initialization
    void Start() {
      this._ring_game_objects = new UnityEngine.GameObject[this._Points.Length];
      //this.connectingRings = new ProceduralRing[points.Length];
      for (var i = 0; i < this._Points.Length; i++) {
        // Make a game_object that we will put the ring on
        // And then put it as a child on the game_object that has this Command and Control script
        this._ring_game_objects[i] = new UnityEngine.GameObject {name = "Connecting ring #" + i};
        this._ring_game_objects[i].transform.parent = this.gameObject.transform;

        // We make a offset game_object to counteract the default cylindermesh pivot/origin being in the middle
        var ring_offset_cylinder_mesh_object = new UnityEngine.GameObject();
        ring_offset_cylinder_mesh_object.transform.parent = this._ring_game_objects[i].transform;

        // Offset the cylinder so that the pivot/origin is at the bottom in relation to the outer ring game_object.
        ring_offset_cylinder_mesh_object.transform.localPosition = new UnityEngine.Vector3(0f, 1f, 0f);
        // Set the radius
        ring_offset_cylinder_mesh_object.transform.localScale =
            new UnityEngine.Vector3(x : this._Radius, 1f, z : this._Radius);

        // Create the the Mesh and renderer to show the connecting ring
        var ring_mesh = ring_offset_cylinder_mesh_object.AddComponent<UnityEngine.MeshFilter>();
        ring_mesh.mesh = this._CylinderMesh;

        var ring_renderer = ring_offset_cylinder_mesh_object.AddComponent<UnityEngine.MeshRenderer>();
        ring_renderer.material = this._LineMat;
      }
    }

    // Update is called once per frame
    void Update() {
      for (var i = 0; i < this._Points.Length; i++) {
        // Move the ring to the point
        this._ring_game_objects[i].transform.position = this._Points[i].transform.position;

        // Match the scale to the distance
        var cylinder_distance =
            0.5f
            * UnityEngine.Vector3.Distance(a : this._Points[i].transform.position,
                                           b : this._MainPoint.transform.position);
        this._ring_game_objects[i].transform.localScale =
            new UnityEngine.Vector3(x : this._ring_game_objects[i].transform.localScale.x,
                                    y : cylinder_distance,
                                    z : this._ring_game_objects[i].transform.localScale.z);

        // Make the cylinder look at the main point.
        // Since the cylinder is pointing up(y) and the forward is z, we need to offset by 90 degrees.
        this._ring_game_objects[i].transform
            .LookAt(target : this._MainPoint.transform, worldUp : UnityEngine.Vector3.up);
        this._ring_game_objects[i].transform.rotation *= UnityEngine.Quaternion.Euler(90, 0, 0);
      }
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ConnectPointsWithCubeMesh : UnityEngine.MonoBehaviour {
    // Fill in this with the default Unity Cube mesh
    // We will account for the cube pivot/origin being in the middle.
    public UnityEngine.Mesh _CubeMesh;

    // Material used for the connecting lines
    public UnityEngine.Material _LineMat;

    // Connect all of the `points` to the `mainPoint`
    public UnityEngine.GameObject _MainPoint;
    public UnityEngine.GameObject[] _Points;

    public float _Radius = 0.05f;

    UnityEngine.GameObject[] _ring_game_objects;

    // Use this for initialization
    void Start() {
      this._ring_game_objects = new UnityEngine.GameObject[this._Points.Length];
      //this.connectingRings = new ProceduralRing[points.Length];
      for (var i = 0; i < this._Points.Length; i++) {
        // Make a game_object that we will put the ring on
        // And then put it as a child on the game_object that has this Command and Control script
        this._ring_game_objects[i] = new UnityEngine.GameObject();
        this._ring_game_objects[i].name = "Connecting ring #" + i;
        this._ring_game_objects[i].transform.parent = this.gameObject.transform;

        // We make a offset game_object to counteract the default cubemesh pivot/origin being in the middle
        var ring_offset_cube_mesh_object = new UnityEngine.GameObject();
        ring_offset_cube_mesh_object.transform.parent = this._ring_game_objects[i].transform;

        // Offset the cube so that the pivot/origin is at the bottom in relation to the outer ring     game_object.
        ring_offset_cube_mesh_object.transform.localPosition = new UnityEngine.Vector3(0f, 1f, 0f);
        // Set the radius
        ring_offset_cube_mesh_object.transform.localScale =
            new UnityEngine.Vector3(x : this._Radius, 1f, z : this._Radius);

        // Create the the Mesh and renderer to show the connecting ring
        var ring_mesh = ring_offset_cube_mesh_object.AddComponent<UnityEngine.MeshFilter>();
        ring_mesh.mesh = this._CubeMesh;

        var ring_renderer = ring_offset_cube_mesh_object.AddComponent<UnityEngine.MeshRenderer>();
        ring_renderer.material = this._LineMat;
      }
    }

    // Update is called once per frame
    void Update() {
      for (var i = 0; i < this._Points.Length; i++) {
        // Move the ring to the point
        this._ring_game_objects[i].transform.position = this._Points[i].transform.position;

        this._ring_game_objects[i].transform.position =
            0.5f * (this._Points[i].transform.position + this._MainPoint.transform.position);
        var delta = this._Points[i].transform.position - this._MainPoint.transform.position;
        this._ring_game_objects[i].transform.position += delta;

        // Match the scale to the distance
        var cube_distance = UnityEngine.Vector3.Distance(a : this._Points[i].transform.position,
                                                         b : this._MainPoint.transform.position);
        this._ring_game_objects[i].transform.localScale =
            new UnityEngine.Vector3(x : this._ring_game_objects[i].transform.localScale.x,
                                    y : cube_distance,
                                    z : this._ring_game_objects[i].transform.localScale.z);

        // Make the cube look at the main point.
        // Since the cube is pointing up(y) and the forward is z, we need to offset by 90 degrees.
        this._ring_game_objects[i].transform
            .LookAt(target : this._MainPoint.transform, worldUp : UnityEngine.Vector3.up);
        this._ring_game_objects[i].transform.rotation *= UnityEngine.Quaternion.Euler(90, 0, 0);
      }
    }
  }
}