namespace droid.Runtime.Prototyping.Configurables.Selection {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Simulation"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(droid.Runtime.Managers.NeodroidManager))]
  public class SimulationConfigurable : Configurable {
    string _fullscreen;
    string _height;

    string _quality_level;
    string _target_frame_rate;
    string _time_scale;
    string _width;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "SimulationConfigurable"; } }

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._quality_level = this.Identifier + "QualityLevel";
      this._target_frame_rate = this.Identifier + "TargetFrameRate";
      this._time_scale = this.Identifier + "TimeScale";
      this._width = this.Identifier + "Width";
      this._height = this.Identifier + "Height";
      this._fullscreen = this.Identifier + "Fullscreen";
    }

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._quality_level);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._target_frame_rate);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._width);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._height);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._fullscreen);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._time_scale);
    }

    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._quality_level);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._target_frame_rate);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._time_scale);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._width);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._height);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._fullscreen);
    }

    public override void UpdateCurrentConfiguration() { }

    /// <summary>
    /// </summary>
    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(
        droid.Runtime.Interfaces.IConfigurableConfiguration simulator_configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + simulator_configuration + " To " + this.Identifier);
      }
      #endif

      if (simulator_configuration.ConfigurableName == this._quality_level) {
        UnityEngine.QualitySettings.SetQualityLevel(index : (int)simulator_configuration.ConfigurableValue,
                                                    true);
      } else if (simulator_configuration.ConfigurableName == this._target_frame_rate) {
        UnityEngine.Application.targetFrameRate = (int)simulator_configuration.ConfigurableValue;
      } else if (simulator_configuration.ConfigurableName == this._width) {
        UnityEngine.Screen.SetResolution(width : (int)simulator_configuration.ConfigurableValue,
                                         height : UnityEngine.Screen.height,
                                         false);
      } else if (simulator_configuration.ConfigurableName == this._height) {
        UnityEngine.Screen.SetResolution(width : UnityEngine.Screen.width,
                                         height : (int)simulator_configuration.ConfigurableValue,
                                         false);
      } else if (simulator_configuration.ConfigurableName == this._fullscreen) {
        UnityEngine.Screen.SetResolution(width : UnityEngine.Screen.width,
                                         height : UnityEngine.Screen.height,
                                         fullscreen : (int)simulator_configuration.ConfigurableValue != 0);
      } else if (simulator_configuration.ConfigurableName == this._time_scale) {
        UnityEngine.Time.timeScale = simulator_configuration.ConfigurableValue;
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="System.NotImplementedException"></exception>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name :
                         this._time_scale,
                         configurable_value : this.ConfigurableValueSpace.Sample())
                   };
    }
  }
}