namespace droid.Runtime.Prototyping.Sensors.Spatial.Occupancy {
  /// <summary>
  /// </summary>
  enum IntersectionType {
    Cone_sphere_,
    Sphere_sphere_
  }

  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Occupancy3d"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  public class Occupancy3dSensor : Sensor,
                                   droid.Runtime.Interfaces.IHasTripleArray {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3[] _observation_value;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _observation_value_space = droid.Runtime.Structs.Space.Space1.ZeroOne;

    UnityEngine.Light _light;
    System.Collections.Generic.IEnumerable<UnityEngine.Transform> _transforms;

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._observation_value_space; } }

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        var a = new float[this.ObservationArray.Length * 3];
        for (var i = 0; i < this.ObservationArray.Length * 3; i += 3) {
          a[i] = this.ObservationArray[i].x;
          a[i + 1] = this.ObservationArray[i].y;
          a[i + 2] = this.ObservationArray[i].z;
        }

        return a;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3[] ObservationArray {
      get { return this._observation_value; }
      set { this._observation_value = this.SingleSpace.Project(v : value); }
    }

    public droid.Runtime.Structs.Space.Space1[] ObservationSpace { get; } =
      new droid.Runtime.Structs.Space.Space1[1];

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._light = this.GetComponent<UnityEngine.Light>();
      this._transforms =
          System.Linq.Enumerable.Select(source : FindObjectsOfType<UnityEngine.MeshFilter>(),
                                        o => o.transform);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      var a = new System.Collections.Generic.List<UnityEngine.Vector3>();
      foreach (var transform1 in this._transforms) {
        if (droid.Runtime.Utilities.IntersectionUtilities.ConeSphereIntersection(spot_light : this._light,
          dynamic_object : transform1)) {
          a.Add(item : transform1.position);
          
        }
      }

      this._observation_value = a.ToArray();
    }
  }
}