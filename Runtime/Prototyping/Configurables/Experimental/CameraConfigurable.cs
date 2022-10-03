namespace droid.Runtime.Prototyping.Configurables.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Camera"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  public class CameraConfigurable : Configurable {
    [UnityEngine.SerializeField] UnityEngine.Camera _camera;
    [UnityEngine.SerializeField] droid.Runtime.GameObjects.NeodroidCamera.SynchroniseCameraProperties _syncer;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _fov_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space =
                                                                    new droid.Runtime.Structs.Space.Space1 {
                                                                        Min = 60f, Max = 90f
                                                                    }
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _focal_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space =
                                                                    new droid.Runtime.Structs.Space.Space1 {
                                                                        Min = 2f, Max = 3f
                                                                    }
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace2 _sensor_size_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace2 {
                                                                _space =
                                                                    new droid.Runtime.Structs.Space.Space2 {
                                                                        Min =
                                                                            new UnityEngine.Vector2(2.5f,
                                                                              2.5f),
                                                                        Max = new UnityEngine.Vector2(5, 5)
                                                                    }
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace2 _lens_shift_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace2 {
                                                                _space =
                                                                    new droid.Runtime.Structs.Space.Space2 {
                                                                        Min = new UnityEngine.Vector2(-0.1f,
                                                                          -0.1f),
                                                                        Max = new UnityEngine.Vector2(0.1f,
                                                                          0.1f)
                                                                    }
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _gate_fit_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                _space = droid.Runtime.Structs.Space.Space1
                                                                      .DiscreteZeroOne
                                                                  * 4
                                                            };

    /// <summary>
    ///   Red
    /// </summary>
    string _focal_str;

    /// <summary>
    ///   Red
    /// </summary>
    string _fov_str;

    string _gate_fit_str;

    /// <summary>
    /// </summary>
    string _lens_shift_x_str;

    /// <summary>
    /// </summary>
    string _lens_shift_y_str;

    /// <summary>
    /// </summary>
    string _sensor_height_str;

    /// <summary>
    ///   Red
    /// </summary>
    string _sensor_width_str;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return this._sensor_size_space; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._fov_str = this.Identifier + "Fov";
      this._focal_str = this.Identifier + "Focal";
      this._sensor_width_str = this.Identifier + "SensorWidth";
      this._sensor_height_str = this.Identifier + "SensorHeight";
      this._lens_shift_x_str = this.Identifier + "LensShiftX";
      this._lens_shift_y_str = this.Identifier + "LensShiftY";
      this._gate_fit_str = this.Identifier + "GateFit";
      if (!this._camera) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      if (!this._syncer) {
        this._syncer =
            this.GetComponent<droid.Runtime.GameObjects.NeodroidCamera.SynchroniseCameraProperties>();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      if (!this._camera.usePhysicalProperties) {
        this.ParentEnvironment =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : this.ParentEnvironment,
                                    c : (Configurable)this,
                                    identifier : this._fov_str);
      } else {
        this.ParentEnvironment =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : this.ParentEnvironment,
                                    c : (Configurable)this,
                                    identifier : this._focal_str);
        this.ParentEnvironment =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : this.ParentEnvironment,
                                    c : (Configurable)this,
                                    identifier : this._sensor_width_str);
        this.ParentEnvironment =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : this.ParentEnvironment,
                                    c : (Configurable)this,
                                    identifier : this._sensor_height_str);
        this.ParentEnvironment =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : this.ParentEnvironment,
                                    c : (Configurable)this,
                                    identifier : this._lens_shift_x_str);
        this.ParentEnvironment =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : this.ParentEnvironment,
                                    c : (Configurable)this,
                                    identifier : this._lens_shift_y_str);
        this.ParentEnvironment =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : this.ParentEnvironment,
                                    c : (Configurable)this,
                                    identifier : this._gate_fit_str);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// n
    protected override void UnRegisterComponent() {
      if (!this._camera.usePhysicalProperties) {
        this.ParentEnvironment?.UnRegister(t : this, identifier : this._fov_str);
      } else {
        this.ParentEnvironment?.UnRegister(t : this, identifier : this._focal_str);
        this.ParentEnvironment?.UnRegister(t : this, identifier : this._sensor_width_str);
        this.ParentEnvironment?.UnRegister(t : this, identifier : this._sensor_height_str);
        this.ParentEnvironment?.UnRegister(t : this, identifier : this._lens_shift_x_str);
        this.ParentEnvironment?.UnRegister(t : this, identifier : this._lens_shift_y_str);
        this.ParentEnvironment?.UnRegister(t : this, identifier : this._gate_fit_str);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        droid.Runtime.Utilities.DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                                         configuration : configuration,
                                                         identifier : this.Identifier);
      }
      #endif

      if (configuration.ConfigurableName == this._fov_str) {
        this._camera.fieldOfView = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._focal_str) {
        this._camera.focalLength = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._sensor_width_str) {
        var a = this._camera.sensorSize;
        a.x = configuration.ConfigurableValue;
        this._camera.sensorSize = a;
      } else if (configuration.ConfigurableName == this._sensor_height_str) {
        var a = this._camera.sensorSize;
        a.y = configuration.ConfigurableValue;
        this._camera.sensorSize = a;
      } else if (configuration.ConfigurableName == this._lens_shift_x_str) {
        var a = this._camera.lensShift;
        a.x = configuration.ConfigurableValue;
        this._camera.lensShift = a;
      } else if (configuration.ConfigurableName == this._lens_shift_y_str) {
        var a = this._camera.lensShift;
        a.y = configuration.ConfigurableValue;
        this._camera.lensShift = a;
      } else if (configuration.ConfigurableName == this._gate_fit_str) {
        System.Enum.TryParse(value : ((int)configuration.ConfigurableValue).ToString(),
                             result : out UnityEngine.Camera.GateFitMode gate_fit_mode);
        this._camera.gateFit = gate_fit_mode;
      }

      if (this._syncer) {
        this._syncer.Sync_Cameras();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.IndexOutOfRangeException"></exception>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      if (!this._camera.usePhysicalProperties) {
        return new[] {
                         new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._fov_str,
                           configurable_value : this._fov_space.Sample())
                     };
      }

      var r = UnityEngine.Random.Range(0, 6);
      switch (r) {
        case 0:
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._focal_str,
                             configurable_value : this._focal_space.Sample())
                       };
        case 1:
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._sensor_width_str,
                             configurable_value : this._sensor_size_space.Sample().x)
                       };
        case 2:
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._sensor_height_str,
                             configurable_value : this._sensor_size_space.Sample().y)
                       };
        case 3:
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._lens_shift_x_str,
                             configurable_value : this._lens_shift_space.Sample().x)
                       };
        case 4:
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._lens_shift_y_str,
                             configurable_value : this._lens_shift_space.Sample().y)
                       };
        case 5:
          return new[] {
                           new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._gate_fit_str,
                             configurable_value : this._gate_fit_space.Sample())
                       };
        default: throw new System.IndexOutOfRangeException();
      }
    }
  }
}