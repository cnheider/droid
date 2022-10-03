﻿namespace droid.Runtime.Prototyping.Configurables.Rigidbody {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Rigidbody"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class RigidbodyConfigurable : droid.Runtime.Prototyping.Configurables.Transforms.SpatialConfigurable,
                                       droid.Runtime.Interfaces.IHasRigidbody {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _angular_velocity = UnityEngine.Vector3.zero;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 _angular_velocity_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                _space = droid.Runtime.Structs.Space.Space3
                                                                    .ZeroOne
                                                            };

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Observation", order = 110)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _velocity = UnityEngine.Vector3.zero;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace3 _velocity_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                _space = droid.Runtime.Structs.Space.Space3
                                                                    .ZeroOne
                                                            };

    /// <summary>
    /// </summary>
    string _ang_x;

    /// <summary>
    /// </summary>
    string _ang_y;

    /// <summary>
    /// </summary>
    string _ang_z;

    /// <summary>
    /// </summary>
    UnityEngine.Rigidbody _rigidbody = null;

    /// <summary>
    /// </summary>
    string _vel_x;

    /// <summary>
    /// </summary>
    string _vel_y;

    /// <summary>
    /// </summary>
    string _vel_z;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "RigidbodyConfigurable"; } }

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Velocity { get { return this._velocity; } set { this._velocity = value; } }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      private set { this._angular_velocity = value; }
    }

    public droid.Runtime.Structs.Space.Space3 VelocitySpace { get { return this._velocity_space._space; } }

    public droid.Runtime.Structs.Space.Space3 AngularSpace {
      get { return this._angular_velocity_space._space; }
    }

    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      this.Velocity = this._rigidbody.velocity;
      this.AngularVelocity = this._rigidbody.angularVelocity;
    }

    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this._vel_x = this.Identifier + "VelX";
      this._vel_y = this.Identifier + "VelY";
      this._vel_z = this.Identifier + "VelZ";
      this._ang_x = this.Identifier + "AngX";
      this._ang_y = this.Identifier + "AngY";
      this._ang_z = this.Identifier + "AngZ";
    }

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._vel_x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._vel_y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._vel_z);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._ang_x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._ang_y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._ang_z);
    }

    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._vel_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._vel_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._vel_z);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._ang_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._ang_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._ang_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(
        droid.Runtime.Interfaces.IConfigurableConfiguration simulator_configuration) {
      var vel = this._rigidbody.velocity;
      var ang = this._rigidbody.velocity;

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._vel_x) {
          var v = this.VelocitySpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).x;
          vel.Set(newX : v - vel.x, newY : vel.y, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_y) {
          var v = this.VelocitySpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).y;
          vel.Set(newX : vel.x, newY : v - vel.y, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_z) {
          var v = this.VelocitySpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).z;
          vel.Set(newX : vel.x, newY : vel.y, newZ : v - vel.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_x) {
          var v = this.AngularSpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).x;
          ang.Set(newX : v - ang.x, newY : ang.y, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_y) {
          var v = this.AngularSpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).y;
          ang.Set(newX : ang.x, newY : v - ang.y, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_z) {
          var v = this.AngularSpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).z;
          ang.Set(newX : ang.x, newY : ang.y, newZ : v - ang.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._vel_x) {
          var v = this.VelocitySpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).x;
          vel.Set(newX : v, newY : vel.y, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_y) {
          var v = this.VelocitySpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).y;
          vel.Set(newX : vel.x, newY : v, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_z) {
          var v = this.VelocitySpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).z;
          vel.Set(newX : vel.x, newY : vel.y, newZ : v);
        } else if (simulator_configuration.ConfigurableName == this._ang_x) {
          var v = this.AngularSpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).x;
          ang.Set(newX : v, newY : ang.y, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_y) {
          var v = this.AngularSpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).y;
          ang.Set(newX : ang.x, newY : v, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_z) {
          var v = this.AngularSpace
                      .Reproject(v : UnityEngine.Vector3.one * simulator_configuration.ConfigurableValue).z;
          ang.Set(newX : ang.x, newY : ang.y, newZ : v);
        }
      }

      this._rigidbody.velocity = vel;
      this._rigidbody.angularVelocity = ang;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._ang_z,
                         configurable_value : this._angular_velocity_space.Sample().z)
                   };
    }
  }
}