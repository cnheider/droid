namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "EulerTransform"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class EulerTransformSensor : Sensor,
                                      droid.Runtime.Interfaces.IHasEulerTransform {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _position;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _position_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.SerializeField] UnityEngine.Vector3 _rotation;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _rotation_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.SerializeField] UnityEngine.Vector3 _direction;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _direction_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    [droid.Runtime.Utilities.SearchableEnumAttribute]
    droid.Runtime.Enums.CoordinateSpaceEnum _spaceEnum = droid.Runtime.Enums.CoordinateSpaceEnum.Environment_;

    [UnityEngine.SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Rotation {
      get { return this._rotation; }
      set { this._rotation = this._rotation_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 PositionSpace { get { return this._position_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 DirectionSpace { get { return this._direction_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 RotationSpace { get { return this._rotation_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Direction {
      get { return this._direction; }
      set { this._direction = this._direction_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      var transform1 = this.transform;
      if (this.ParentEnvironment != null
          && this._spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        this.Position = this.ParentEnvironment.TransformPoint(point : transform1.position);
        this.Direction = this.ParentEnvironment.TransformDirection(direction : transform1.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(direction : transform1.up);
      } else if (this._spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        this.Position = transform1.localPosition;
        this.Direction = transform1.forward;
        this.Rotation = transform1.up;
      } else {
        this.Position = transform1.position;
        this.Direction = transform1.forward;
        this.Rotation = transform1.up;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment != null && this.ParentEnvironment.PlayableArea != null) {
          this._position_space =
              droid.Runtime.Structs.Space.Space3.FromCenterExtents(bounds_extents : this.ParentEnvironment
                                                                       .PlayableArea.Bounds.extents);
        }
      }
    }
  }
}