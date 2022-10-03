namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Rotation"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class RotationConfigurable : SpatialConfigurable,
                                      droid.Runtime.Interfaces.IHasQuadruple {
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace4 _quad_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace4 {
                                                                _space = droid.Runtime.Structs.Space.Space4
                                                                    .MinusOneOne
                                                            };

    [UnityEngine.SerializeField] bool _use_environments_space = false;

    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Quaternion observation_value = UnityEngine.Quaternion.identity;

    /// <summary>
    /// </summary>
    string _w;

    /// <summary>
    /// </summary>
    string _x;

    /// <summary>
    /// </summary>
    string _y;

    /// <summary>
    /// </summary>
    string _z;

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get { return this._quad_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Quaternion ObservationValue { get { return this.observation_value; } }

    public droid.Runtime.Structs.Space.Space4 QuadSpace { get { return this._quad_space._space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      this._w = this.Identifier + "W_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._z);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._w);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._z);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._w);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space && this.ParentEnvironment != null) {
        this.observation_value =
            this.ParentEnvironment.TransformRotation(quaternion : this.transform.rotation);
      } else {
        this.observation_value = this.transform.rotation;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(
        droid.Runtime.Interfaces.IConfigurableConfiguration simulator_configuration) {
      //TODO: Denormalize configuration if space is marked as normalised

      var rot = this.transform.rotation;
      if (this.ParentEnvironment && this._use_environments_space) {
        rot = this.ParentEnvironment.TransformRotation(quaternion : this.transform.rotation);
      }

      var v = simulator_configuration.ConfigurableValue;
      if (this.QuadSpace.DecimalGranularity >= 0) {
        v = (int)System.Math.Round(value : v, digits : this.QuadSpace.DecimalGranularity);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message :
                              $"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          if (this.QuadSpace.Min.x.CompareTo(this.QuadSpace.Max.x) != 0) {
            if (v < this.QuadSpace.Min.x || v > this.QuadSpace.Max.x) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableX does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.x} to {this.QuadSpace.Max.x}");
              return; // Do nothing
            }
          }

          rot.Set(newX : rot.x - v,
                  newY : rot.y,
                  newZ : rot.z,
                  newW : rot.w);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          if (this.QuadSpace.Min.y.CompareTo(this.QuadSpace.Max.y) != 0) {
            if (v < this.QuadSpace.Min.y || v > this.QuadSpace.Max.y) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableY does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.y} to {this.QuadSpace.Max.y}");
              return; // Do nothing
            }
          }

          rot.Set(newX : rot.x,
                  newY : rot.y - v,
                  newZ : rot.z,
                  newW : rot.w);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          if (this.QuadSpace.Min.z.CompareTo(this.QuadSpace.Max.z) != 0) {
            if (v < this.QuadSpace.Min.z || v > this.QuadSpace.Max.z) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableZ does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.z} to {this.QuadSpace.Max.z}");
              return; // Do nothing
            }
          }

          rot.Set(newX : rot.x,
                  newY : rot.y,
                  newZ : rot.z - v,
                  newW : rot.w);
        } else if (simulator_configuration.ConfigurableName == this._w) {
          if (this.QuadSpace.Min.w.CompareTo(this.QuadSpace.Max.w) != 0) {
            if (v < this.QuadSpace.Min.w || v > this.QuadSpace.Max.w) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableW does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.w} to {this.QuadSpace.Max.w}");
              return; // Do nothing
            }
          }

          rot.Set(newX : rot.x,
                  newY : rot.y,
                  newZ : rot.z,
                  newW : rot.w - v);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          if (this.QuadSpace.Min.x.CompareTo(this.QuadSpace.Max.x) != 0) {
            if (v < this.QuadSpace.Min.x || v > this.QuadSpace.Max.x) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableX does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.x} to {this.QuadSpace.Max.x}");
              return; // Do nothing
            }
          }

          rot.Set(newX : v,
                  newY : rot.y,
                  newZ : rot.z,
                  newW : rot.w);
          //rot.x = v;
        } else if (simulator_configuration.ConfigurableName == this._y) {
          if (this.QuadSpace.Min.y.CompareTo(this.QuadSpace.Max.y) != 0) {
            if (v < this.QuadSpace.Min.y || v > this.QuadSpace.Max.y) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableY does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.y} to {this.QuadSpace.Max.y}");
              return; // Do nothing
            }
          }

          rot.Set(newX : rot.x,
                  newY : v,
                  newZ : rot.z,
                  newW : rot.w);
          //rot.y = v;
        } else if (simulator_configuration.ConfigurableName == this._z) {
          if (this.QuadSpace.Min.z.CompareTo(this.QuadSpace.Max.z) != 0) {
            if (v < this.QuadSpace.Min.z || v > this.QuadSpace.Max.z) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableZ does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.z} to {this.QuadSpace.Max.z}");
              return; // Do nothing
            }
          }

          rot.Set(newX : rot.x,
                  newY : rot.y,
                  newZ : v,
                  newW : rot.w);
          //rot.z = v;
        } else if (simulator_configuration.ConfigurableName == this._w) {
          if (this.QuadSpace.Min.w.CompareTo(this.QuadSpace.Max.w) != 0) {
            if (v < this.QuadSpace.Min.w || v > this.QuadSpace.Max.w) {
              UnityEngine.Debug.Log(message :
                                    $"ConfigurableW does not accept input {v}, outside allowed range "
                                    + $"{this.QuadSpace.Min.w} to {this.QuadSpace.Max.w}");
              return; // Do nothing
            }
          }

          rot.Set(newX : rot.x,
                  newY : rot.y,
                  newZ : rot.z,
                  newW : v);
          //rot.w = v;
        }
      }

      if (this.ParentEnvironment && this._use_environments_space) {
        rot = this.ParentEnvironment.InverseTransformRotation(quaternion : rot);
      }

      this.transform.rotation = rot;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var sample = this._quad_space.Sample();
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._x,
                         configurable_value : sample.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._y,
                         configurable_value : sample.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._z,
                         configurable_value : sample.z),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._w,
                         configurable_value : sample.w)
                   };
    }
  }
}