namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="droid.Runtime.Interfaces.IHasQuaternionTransform" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "QuaternionTransform"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class QuaternionTransformSensor : Sensor,
                                           droid.Runtime.Interfaces.IHasQuaternionTransform {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _position;

    [UnityEngine.SerializeField] UnityEngine.Quaternion _rotation;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    droid.Runtime.Enums.CoordinateSpaceEnum _spaceEnum = droid.Runtime.Enums.CoordinateSpaceEnum.Environment_;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _position_space = droid.Runtime.Structs.Space.Space3.MinusOneOne;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space4 _rotation_space = droid.Runtime.Structs.Space.Space4.MinusOneOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this._position.x;
        yield return this._position.y;
        yield return this._position.z;
        yield return this._rotation.x;
        yield return this._rotation.y;
        yield return this._rotation.z;
        yield return this._rotation.w;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Position { get { return this._position; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Quaternion Rotation { get { return this._rotation; } }

    public droid.Runtime.Structs.Space.Space3
        PositionSpace { get { return this._position_space; } } //TODO: Implement

    public droid.Runtime.Structs.Space.Space4
        RotationSpace { get { return this._rotation_space; } } //TODO: Implement

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      var transform1 = this.transform;
      if (this.ParentEnvironment != null
          && this._spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        this._position = this.ParentEnvironment.TransformPoint(point : transform1.position);
        this._rotation =
            UnityEngine.Quaternion.Euler(euler : this.ParentEnvironment
                                                     .TransformDirection(direction : transform1.up));
      } else {
        this._position = transform1.position;
        this._rotation = transform1.rotation;
      }
    }
  }
}