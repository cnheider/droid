namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "EulerRotation"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class EulerRotationConfigurable : SpatialConfigurable,
                                           droid.Runtime.Interfaces.IHasTriple {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Quaternion _euler_rotation = UnityEngine.Quaternion.identity;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 _euler_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                _space =
                                                                    new droid.Runtime.Structs.Space.Space3 {
                                                                        Min = UnityEngine.Vector3.zero,
                                                                        Max = new UnityEngine.Vector3(360f,
                                                                          360f,
                                                                          360f)
                                                                    }
                                                            };

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
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get { return this._euler_space; } }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 TripleSpace {
      get { return (droid.Runtime.Structs.Space.Space3)this._euler_space.Space; }
    }

    UnityEngine.Vector3 droid.Runtime.Interfaces.IHasTriple.ObservationValue {
      get { return this._euler_rotation.eulerAngles; }
    }

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
            c : (Configurable)this);
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

      this.ParentEnvironment.UnRegister(configurable : this);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._z);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._w);
    }

    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      switch (this._coordinate_spaceEnum) {
        case droid.Runtime.Enums.CoordinateSpaceEnum.Environment_ when this.ParentEnvironment != null:
          this._euler_rotation =
              this.ParentEnvironment.TransformRotation(quaternion : this.transform.rotation);
          break;
        case droid.Runtime.Enums.CoordinateSpaceEnum.Global_:
          this._euler_rotation = this.transform.rotation;
          break;
        case droid.Runtime.Enums.CoordinateSpaceEnum.Local_:
          this._euler_rotation = this.transform.localRotation;
          break;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(
        droid.Runtime.Interfaces.IConfigurableConfiguration simulator_configuration) {
      UnityEngine.Quaternion rot;
      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        rot = this.transform.localRotation;
      } else {
        rot = this.transform.rotation;
      }

      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        rot = this.ParentEnvironment.TransformRotation(quaternion : this.transform.rotation);
      }

      var v = simulator_configuration.ConfigurableValue;
      if (this.TripleSpace.DecimalGranularity >= 0) {
        v = (int)System.Math.Round(value : v, digits : this.TripleSpace.DecimalGranularity);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message :
                              $"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif
      var rote = rot.eulerAngles;

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          if (this.TripleSpace.Min[0].CompareTo(this.TripleSpace.Max[0]) != 0) {
            #if NEODROID_DEBUG
            if (v < this.TripleSpace.Min[0] || v > this.TripleSpace.Max[0]) {
              UnityEngine.Debug.Log(message :
                                    $"Configurable does not accept input {v}, outside allowed range {this.TripleSpace.Min[0]} to {this.TripleSpace.Max[0]}");
              return; // Do nothing
            }
            #endif
          }

          rot.eulerAngles = new UnityEngine.Vector3(x : v + rote.x, y : rote.y, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          rot.eulerAngles = new UnityEngine.Vector3(x : rote.x, y : v + rote.y, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          rot.eulerAngles = new UnityEngine.Vector3(x : rote.x, y : rote.y, z : v + rote.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          rot.eulerAngles = new UnityEngine.Vector3(x : v, y : rote.y, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          rot.eulerAngles = new UnityEngine.Vector3(x : rote.x, y : v, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          rot.eulerAngles = new UnityEngine.Vector3(x : rote.x, y : rote.y, z : v);
        }
      }

      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        rot = this.ParentEnvironment.InverseTransformRotation(quaternion : rot);
      }

      if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        this.transform.localRotation = rot;
      } else {
        this.transform.rotation = rot;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var sample = this._euler_space.Sample();

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._x,
                         configurable_value : sample.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._y,
                         configurable_value : sample.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._z,
                         configurable_value : sample.z)
                   };
    }
  }
}