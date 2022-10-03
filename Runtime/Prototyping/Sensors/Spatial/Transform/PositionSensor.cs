namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Position"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class PositionSensor : Sensor,
                                droid.Runtime.Interfaces.IHasTriple {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _position;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _position_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    droid.Runtime.Enums.CoordinateSpaceEnum _spaceEnum = droid.Runtime.Enums.CoordinateSpaceEnum.Environment_;

    [UnityEngine.SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
        yield return this.ObservationValue.z;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 ObservationValue {
      get { return this._position; }
      set { this._position = this._position_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 TripleSpace { get { return this._position_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment) {
          this._position_space =
              droid.Runtime.Structs.Space.Space3.FromCenterExtents(bounds_extents : this.ParentEnvironment
                                                                       .PlayableArea.Bounds.extents);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null
          && this._spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        this.ObservationValue = this.ParentEnvironment.TransformPoint(point : this.transform.position);
      } else if (this._spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        this.ObservationValue = this.transform.localPosition;
      } else {
        this.ObservationValue = this.transform.position;
      }
    }
  }
}