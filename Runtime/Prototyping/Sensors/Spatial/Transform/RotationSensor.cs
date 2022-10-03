namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  [UnityEngine.AddComponentMenu(menuName : PrototypingComponentMenuPath._ComponentMenuPath + "Rotation")]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class RotationSensor : Sensor,
                                droid.Runtime.Interfaces.IHasQuadruple {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Quaternion _rotation;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    droid.Runtime.Enums.CoordinateSpaceEnum _spaceEnum = droid.Runtime.Enums.CoordinateSpaceEnum.Environment_;

    /// <summary>
    /// </summary>
    public droid.Runtime.Enums.CoordinateSpaceEnum SpaceEnum { get { return this._spaceEnum; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
        yield return this.ObservationValue.z;
        yield return this.ObservationValue.w;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Quaternion ObservationValue {
      get { return this._rotation; }
      set { this._rotation = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space4 QuadSpace { get; } = new droid.Runtime.Structs.Space.Space4();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null
          && this._spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        this.ObservationValue =
            this.ParentEnvironment.TransformRotation(quaternion : this.transform.rotation);
      } else if (this._spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        this.ObservationValue = this.transform.localRotation;
      } else {
        this.ObservationValue = this.transform.rotation;
      }
    }
  }
}