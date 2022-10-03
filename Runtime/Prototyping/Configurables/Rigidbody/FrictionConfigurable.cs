namespace droid.Runtime.Prototyping.Configurables.Rigidbody {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Friction"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class FrictionConfigurable : droid.Runtime.Prototyping.Configurables.Transforms.SpatialConfigurable,
                                      droid.Runtime.Interfaces.IHasSingle {
    /// <summary>
    /// </summary>
    droid.Runtime.Interfaces.ISamplable _friction_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space = droid.Runtime.Structs.Space.Space1
                                                                    .ZeroOne
                                                            };

    UnityEngine.Rigidbody _rigidbody;

    /// <summary>
    /// </summary>
    string _vel_x;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "RigidbodyConfigurable"; } }

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    public float ObservationValue { get { return this._friction_space.Sample(); } }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 SingleSpace {
      get { return (droid.Runtime.Structs.Space.Space1)this._friction_space.Space; }
    }

    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      //this.Velocity = this._rigidbody.velocity;
    }

    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this._vel_x = this.Identifier + "VelX";
    }

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._vel_x);
    }

    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._vel_x);
    }

    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(
        droid.Runtime.Interfaces.IConfigurableConfiguration simulator_configuration) {
      //var vel = this._rigidbody.velocity;

      var v = simulator_configuration.ConfigurableValue;
      if (this.SingleSpace.DecimalGranularity >= 0) {
        v = (int)System.Math.Round(value : v, digits : this.SingleSpace.DecimalGranularity);
      }

      if (this.SingleSpace.Min.CompareTo(this.SingleSpace.Max) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, VelocitySpace.MinValues == VelocitySpace.MaxValues
        if (v < this.SingleSpace.Min || v > this.SingleSpace.Max) {
          UnityEngine.Debug.Log(message :
                                string
                                    .Format("Configurable does not accept input{2}, outside allowed range {0} to {1}",
                                            this.SingleSpace.Min,
                                            this.SingleSpace.Max,
                                            v));
          return; // Do nothing
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + v + " To " + this.Identifier);
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._vel_x) {
          //vel.Set(v - vel.x, vel.y, vel.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._vel_x) {
          //vel.Set(v, vel.y, vel.z);
        }
      }

      //this._rigidbody.angularVelocity = ang;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._vel_x,
                         configurable_value : this._friction_space.Sample())
                   };
    }
  }
}