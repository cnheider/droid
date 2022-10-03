namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "ScreenSpacePosition"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Renderer))]
  public class ScreenSpacePositionConfigurable : Configurable {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Camera _camera;

    [UnityEngine.SerializeField] droid.Runtime.Structs.Space.Sample.SampleSpace3 _configurable_value_space;
    string _rw;
    string _rx;
    string _ry;
    string _rz;
    string _x;
    string _y;
    string _z;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return this._configurable_value_space; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
      this._rx = this.Identifier + "RX";
      this._ry = this.Identifier + "RY";
      this._rz = this.Identifier + "RZ";
      this._rw = this.Identifier + "RW";

      if (!this._camera) {
        this._camera = FindObjectOfType<UnityEngine.Camera>();
      }
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
            identifier : this._rx);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._ry);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._rz);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._rw);
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
      this.ParentEnvironment.UnRegister(t : this, identifier : this._rx);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._ry);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._rz);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._rw);
    }

    public override void UpdateCurrentConfiguration() { }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      var cv =
          this._configurable_value_space.Space.Reproject(configuration_configurable_value : configuration
                                                             .ConfigurableValue);

      #if NEODROID_DEBUG
      if (this.Debugging) {
        droid.Runtime.Utilities.DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                                         configuration : configuration,
                                                         identifier : this.Identifier);
      }
      #endif

      var pos = this.transform.position;
      var rot = this.transform.rotation;

      if (configuration.ConfigurableName == this._x) {
        pos.x = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._y) {
        pos.y = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._z) {
        pos.z = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._rx) {
        rot.x = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._ry) {
        rot.y = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._rz) {
        rot.z = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._rw) {
        rot.w = configuration.ConfigurableValue;
      }

      this.transform.position = pos;
      this.transform.rotation = rot;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var x = this.ConfigurableValueSpace.Sample();
      var y = this.ConfigurableValueSpace.Sample();

      var a = new UnityEngine.Vector2(x : x, y : y);
      var bounded =
          UnityEngine.Vector2.Min(lhs : UnityEngine.Vector2.Max(lhs : a,
                                                                rhs : new UnityEngine.Vector2(0.2f, 0.2f)),
                                  rhs : new UnityEngine.Vector2(0.8f, 0.8f));

      //var z = Space1.ZeroOne.Sample() * this._camera.farClipPlane;
      var z = this._camera.nearClipPlane + 2;
      var bounded3 = new UnityEngine.Vector3(x : bounded.x, y : bounded.y, z : z);

      var c = this._camera.ViewportToWorldPoint(position : bounded3);

      var b = new UnityEngine.Quaternion(x : this.ConfigurableValueSpace.Sample(),
                                         y : this.ConfigurableValueSpace.Sample(),
                                         z : this.ConfigurableValueSpace.Sample(),
                                         w : this.ConfigurableValueSpace.Sample());
      var sample1 = this.ConfigurableValueSpace.Sample();
      var sample = this.ConfigurableValueSpace.Sample();

      if (sample1 > 0.5f) {
        if (sample < .33f) {
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._x,
                             configurable_value : c.x)
                       };
        }

        if (sample > .66f) {
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._y,
                             configurable_value : c.y)
                       };
        }

        return new[] {
                         new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._z,
                           configurable_value : c.z)
                     };
      }

      if (sample < .33f) {
        return new[] {
                         new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._rx,
                           configurable_value : b.x)
                     };
      }

      if (sample > .66f) {
        return new[] {
                         new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._ry,
                           configurable_value : b.y)
                     };
      }

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._rz,
                         configurable_value : b.z)
                   };
    }
  }
}