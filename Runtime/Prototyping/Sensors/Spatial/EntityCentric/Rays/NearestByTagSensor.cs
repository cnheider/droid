namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "NearestByTag"
                                           + SensorComponentMenuPath._Postfix)]
  public class NearestByTagSensor : Sensor,
                                    droid.Runtime.Interfaces.IHasEulerTransform {
    [UnityEngine.SerializeField] UnityEngine.Vector3 _direction;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _direction_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.GameObject _nearest_object;

    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _position;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _position_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.SerializeField] UnityEngine.Vector3 _rotation;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _rotation_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.SerializeField] string _tag = "";

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return base.PrototypingTypeName + this._tag; } }

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.Position.x;
        yield return this.Position.y;
        yield return this.Position.z;
        yield return this.Direction.x;
        yield return this.Direction.y;
        yield return this.Direction.z;
        yield return this.Rotation.x;
        yield return this.Rotation.y;
        yield return this.Rotation.z;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Position {
      get { return this._position; }
      set { this._position = this._position_space.Project(v : value); }
    }

    public UnityEngine.Vector3 Rotation {
      get { return this._rotation; }
      set { this._rotation = this._rotation_space.Project(v : value); }
    }

    public droid.Runtime.Structs.Space.Space3 PositionSpace { get { return this._position_space; } }
    public droid.Runtime.Structs.Space.Space3 DirectionSpace { get { return this._direction_space; } }
    public droid.Runtime.Structs.Space.Space3 RotationSpace { get { return this._rotation_space; } }

    public UnityEngine.Vector3 Direction {
      get { return this._direction; }
      set { this._direction = this._direction_space.Project(v : value); }
    }

    public override void UpdateObservation() {
      this._nearest_object = this.FindNearest();

      if (this.ParentEnvironment != null) {
        this.Position =
            this.ParentEnvironment.TransformPoint(point : this._nearest_object.transform.position);
        this.Direction =
            this.ParentEnvironment.TransformDirection(direction : this._nearest_object.transform.forward);
        this.Rotation =
            this.ParentEnvironment.TransformDirection(direction : this._nearest_object.transform.up);
      } else {
        this.Position = this._nearest_object.transform.position;
        this.Direction = this._nearest_object.transform.forward;
        this.Rotation = this._nearest_object.transform.up;
      }
    }

    UnityEngine.GameObject FindNearest() {
      var candidates = FindObjectsOfType<UnityEngine.GameObject>();
      var nearest_object = this.gameObject;
      var nearest_distance = -1.0;
      for (var index = 0; index < candidates.Length; index++) {
        var candidate = candidates[index];
        if (candidate.CompareTag(tag : this._tag)) {
          var dist = UnityEngine.Vector3.Distance(a : this.transform.position,
                                                  b : candidate.transform.position);
          if (nearest_distance > dist || nearest_distance < 0) {
            nearest_distance = dist;
            nearest_object = candidate;
          }
        }
      }

      return nearest_object;
    }
  }
}