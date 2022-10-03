namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "TransformConfigurable1Dof"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class TransformConfigurable1Dof : SpatialConfigurable,
                                           droid.Runtime.Interfaces.IHasSingle {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Transform" + this._axisEnum_of_configuration + "Configurable"; }
    }

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return this._single_value_space; }
    }

    /// <summary>
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      private set { this._observation_value = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 SingleSpace {
      get { return (droid.Runtime.Structs.Space.Space1)this._single_value_space.Space; }
      set { this._single_value_space.Space = value; }
    }

    /// <summary>
    /// </summary>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public override void UpdateCurrentConfiguration() {
      var transform1 = this.transform;
      UnityEngine.Vector3 pos;
      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        pos = transform1.localPosition;
      } else {
        pos = transform1.position;
      }

      var dir = transform1.forward;
      var rot = transform1.up;

      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        if (this.ParentEnvironment != null) {
          pos = this.ParentEnvironment.TransformPoint(point : pos);
          dir = this.ParentEnvironment.TransformDirection(direction : dir);
          rot = this.ParentEnvironment.TransformDirection(direction : rot);
        } else {
          UnityEngine.Debug.LogWarning("ParentEnvironment not found!");
        }
      }

      switch (this._axisEnum_of_configuration) {
        case droid.Runtime.Enums.AxisEnum.X_:
          this._observation_value = pos.x;
          break;
        case droid.Runtime.Enums.AxisEnum.Y_:
          this._observation_value = pos.y;
          break;
        case droid.Runtime.Enums.AxisEnum.Z_:
          this._observation_value = pos.z;
          break;
        case droid.Runtime.Enums.AxisEnum.Dir_x_:
          this._observation_value = dir.x;
          break;
        case droid.Runtime.Enums.AxisEnum.Dir_y_:
          this._observation_value = dir.y;
          break;
        case droid.Runtime.Enums.AxisEnum.Dir_z_:
          this._observation_value = dir.z;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_x_:
          this._observation_value = rot.x;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_y_:
          this._observation_value = rot.y;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_z_:
          this._observation_value = rot.z;
          break;
        default:
          throw new System.ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        var dec_gran = 4;
        if (this._single_value_space.Space != null && this.ParentEnvironment.PlayableArea) {
          dec_gran = this._single_value_space.Space.DecimalGranularity;
        }

        if (this.ParentEnvironment) {
          switch (this._axisEnum_of_configuration) {
            case droid.Runtime.Enums.AxisEnum.X_:
              this.SingleSpace =
                  droid.Runtime.Structs.Space.Space1.FromCenterExtent(extent : this.ParentEnvironment
                                                                          .PlayableArea.Bounds.extents.x,
                                                                      decimal_granularity : dec_gran);
              break;
            case droid.Runtime.Enums.AxisEnum.Y_:
              this.SingleSpace =
                  droid.Runtime.Structs.Space.Space1.FromCenterExtent(extent : this.ParentEnvironment
                                                                          .PlayableArea.Bounds.extents.y,
                                                                      decimal_granularity : dec_gran);
              break;
            case droid.Runtime.Enums.AxisEnum.Z_:
              this.SingleSpace =
                  droid.Runtime.Structs.Space.Space1.FromCenterExtent(extent : this.ParentEnvironment
                                                                          .PlayableArea.Bounds.extents.z,
                                                                      decimal_granularity : dec_gran);
              break;
          }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public override void ApplyConfiguration(
        droid.Runtime.Interfaces.IConfigurableConfiguration simulator_configuration) {
      float cv = this.SingleSpace.Reproject(v : simulator_configuration.ConfigurableValue);

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + simulator_configuration + " To " + this.Identifier);
      }
      #endif

      var transform1 = this.transform;
      UnityEngine.Vector3 pos;
      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        pos = transform1.localPosition;
      } else {
        pos = transform1.position;
      }

      var dir = transform1.forward;
      var rot = transform1.up;
      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        if (this.ParentEnvironment != null) {
          this.ParentEnvironment.TransformPoint(point : ref pos);
          this.ParentEnvironment.TransformDirection(direction : ref dir);
          this.ParentEnvironment.TransformDirection(direction : ref rot);
        } else {
          UnityEngine.Debug.LogWarning("ParentEnvironment not found!");
        }
      }

      switch (this._axisEnum_of_configuration) {
        case droid.Runtime.Enums.AxisEnum.X_:
          if (this.RelativeToExistingValue) {
            pos.Set(newX : cv + pos.x, newY : pos.y, newZ : pos.z);
          } else {
            pos.Set(newX : cv, newY : pos.y, newZ : pos.z);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Y_:
          if (this.RelativeToExistingValue) {
            pos.Set(newX : pos.x, newY : cv + pos.y, newZ : pos.z);
          } else {
            pos.Set(newX : pos.x, newY : cv, newZ : pos.z);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Z_:
          if (this.RelativeToExistingValue) {
            pos.Set(newX : pos.x, newY : pos.y, newZ : cv + pos.z);
          } else {
            pos.Set(newX : pos.x, newY : pos.y, newZ : cv);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Dir_x_:
          if (this.RelativeToExistingValue) {
            dir.Set(newX : cv + dir.x, newY : dir.y, newZ : dir.z);
          } else {
            dir.Set(newX : cv, newY : dir.y, newZ : dir.z);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Dir_y_:
          if (this.RelativeToExistingValue) {
            dir.Set(newX : dir.x, newY : cv + dir.y, newZ : dir.z);
          } else {
            dir.Set(newX : dir.x, newY : cv, newZ : dir.z);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Dir_z_:
          if (this.RelativeToExistingValue) {
            dir.Set(newX : dir.x, newY : dir.y, newZ : cv + dir.z);
          } else {
            dir.Set(newX : dir.x, newY : dir.y, newZ : cv);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_x_:
          if (this.RelativeToExistingValue) {
            rot.Set(newX : cv + rot.x, newY : rot.y, newZ : rot.z);
          } else {
            rot.Set(newX : cv, newY : rot.y, newZ : rot.z);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_y_:
          if (this.RelativeToExistingValue) {
            rot.Set(newX : rot.x, newY : cv + rot.y, newZ : rot.z);
          } else {
            rot.Set(newX : rot.x, newY : cv, newZ : rot.z);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_z_:
          if (this.RelativeToExistingValue) {
            rot.Set(newX : rot.x, newY : rot.y, newZ : cv + rot.z);
          } else {
            rot.Set(newX : rot.x, newY : rot.y, newZ : cv);
          }

          break;
        default:
          throw new System.ArgumentOutOfRangeException();
      }

      var inv_pos = pos;
      var inv_dir = dir;
      var inv_rot = rot;

      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        if (this.ParentEnvironment != null) {
          this.ParentEnvironment.InverseTransformPoint(point : ref inv_pos);
          this.ParentEnvironment.InverseTransformDirection(direction : ref inv_dir);
          this.ParentEnvironment.InverseTransformDirection(direction : ref inv_rot);
        } else {
          UnityEngine.Debug.LogWarning("ParentEnvironment not found!");
        }
      }

      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        transform1.localPosition = inv_pos;
      } else {
        this.transform.position = inv_pos;
      }

      this.transform.rotation = UnityEngine.Quaternion.identity;
      this.transform.rotation = UnityEngine.Quaternion.LookRotation(forward : inv_dir, upwards : inv_rot);
    }

    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this.Identifier,
                         configurable_value : this._single_value_space.Sample())
                   };
    }

    #region Fields

    [UnityEngine.SerializeField]
    droid.Runtime.Enums.AxisEnum _axisEnum_of_configuration = droid.Runtime.Enums.AxisEnum.X_;

    [UnityEngine.SerializeField] float _observation_value = 0;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _single_value_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space = droid.Runtime.Structs.Space.Space1
                                                                    .ZeroOne
                                                            };

    [UnityEngine.SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    #endregion
  }
}