namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "SingleAxisTransform"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  public class SingleAxisTransformSensor : droid.Runtime.Prototyping.Sensors.Experimental.SingleValueSensor {
    [UnityEngine.SerializeField]
    [droid.Runtime.Utilities.SearchableEnumAttribute]
    droid.Runtime.Enums.AxisEnum _dim = droid.Runtime.Enums.AxisEnum.X_;

    [UnityEngine.SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return base.PrototypingTypeName + this._dim; } }

    void OnDrawGizmos() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._dim) {
          case droid.Runtime.Enums.AxisEnum.Rot_x_:
          case droid.Runtime.Enums.AxisEnum.X_:

            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.right * 2,
                                       color : UnityEngine.Color.green);
            break;
          case droid.Runtime.Enums.AxisEnum.Rot_y_:
          case droid.Runtime.Enums.AxisEnum.Y_:

            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.up * 2,
                                       color : UnityEngine.Color.green);
            break;
          case droid.Runtime.Enums.AxisEnum.Rot_z_:
          case droid.Runtime.Enums.AxisEnum.Z_:
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.forward * 2,
                                       color : UnityEngine.Color.green);
            break;
          case droid.Runtime.Enums.AxisEnum.Dir_x_: break;
          case droid.Runtime.Enums.AxisEnum.Dir_y_: break;
          case droid.Runtime.Enums.AxisEnum.Dir_z_: break;
          default: //TODO add the Direction cases
            UnityEngine.Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        switch (this._dim) {
          case droid.Runtime.Enums.AxisEnum.X_:
            if (this.ParentEnvironment != null && this.ParentEnvironment.PlayableArea != null) {
              this._observation_value_space =
                  droid.Runtime.Structs.Space.Space1.FromCenterExtent(extent : this.ParentEnvironment
                                                                          .PlayableArea.Bounds.extents.x);
            }

            break;
          case droid.Runtime.Enums.AxisEnum.Y_:
            if (this.ParentEnvironment != null && this.ParentEnvironment.PlayableArea != null) {
              this._observation_value_space =
                  droid.Runtime.Structs.Space.Space1.FromCenterExtent(extent : this.ParentEnvironment
                                                                          .PlayableArea.Bounds.extents.y);
            }

            break;
          case droid.Runtime.Enums.AxisEnum.Z_:
            if (this.ParentEnvironment != null && this.ParentEnvironment.PlayableArea != null) {
              this._observation_value_space =
                  droid.Runtime.Structs.Space.Space1.FromCenterExtent(extent : this.ParentEnvironment
                                                                          .PlayableArea.Bounds.extents.z);
            }

            break;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public override void UpdateObservation() { //TODO: IMPLEMENT LOCAL SPACE
      switch (this._dim) {
        case droid.Runtime.Enums.AxisEnum.X_:
          this.ObservationValue = this.transform.position.x;
          break;
        case droid.Runtime.Enums.AxisEnum.Y_:
          this.ObservationValue = this.transform.position.y;
          break;
        case droid.Runtime.Enums.AxisEnum.Z_:
          this.ObservationValue = this.transform.position.z;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_x_:
          this.ObservationValue = this.transform.rotation.eulerAngles.x;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_y_:
          this.ObservationValue = this.transform.rotation.eulerAngles.y;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_z_:
          this.ObservationValue = this.transform.rotation.eulerAngles.z;
          break;
        case droid.Runtime.Enums.AxisEnum.Dir_x_:
          this.ObservationValue = this.transform.forward.x;
          break;
        case droid.Runtime.Enums.AxisEnum.Dir_y_:
          this.ObservationValue = this.transform.forward.y;
          break;
        case droid.Runtime.Enums.AxisEnum.Dir_z_:
          this.ObservationValue = this.transform.forward.z;
          break;
        default: throw new System.ArgumentOutOfRangeException();
      }
    }
  }
}