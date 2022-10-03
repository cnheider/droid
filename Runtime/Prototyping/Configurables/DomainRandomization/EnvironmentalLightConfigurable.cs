namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "EnvironmentalLight"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.DisallowMultipleComponent]
  public class EnvironmentalLightConfigurable : Configurable {
    string _color_b;
    string _color_g;
    string _color_r;

    [UnityEngine.SerializeField]
    droid.Runtime.Interfaces.ISamplable _color_space = new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                           _space = new droid.Runtime.Structs.Space.Space3 {
                                                                        Min = UnityEngine.Vector3.one
                                                                          * 0.6f,
                                                                        Max = UnityEngine.Vector3.one * 1f
                                                                    }
                                                       };

    string _intensity;

    [UnityEngine.SerializeField]
    droid.Runtime.Interfaces.ISamplable _intensity_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace2 {
                                                                Space =
                                                                    new droid.Runtime.Structs.Space.Space2 {
                                                                        Min =
                                                                            UnityEngine.Vector3.one * 0.0f,
                                                                        Max = UnityEngine.Vector3.one * 1f
                                                                    },
                                                                _distribution_sampler =
                                                                    new droid.Runtime.Sampling.
                                                                    DistributionSampler(distribution_enum :
                                                                      droid.Runtime.Sampling
                                                                          .DistributionEnum.Linear_) {
                                                                        DistributionParameter = -1
                                                                    }
                                                            };

    string _reflection_intensity;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._color_r = this.Identifier + "ColorR";
      this._color_g = this.Identifier + "ColorG";
      this._color_b = this.Identifier + "ColorB";
      this._intensity = this.Identifier + "Intensity";
      this._reflection_intensity = this.Identifier + "ReflectionIntensity";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._color_r);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._color_b);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._color_g);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._intensity);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._reflection_intensity);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._color_r);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._color_g);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._color_b);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._intensity);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._reflection_intensity);
    }

    public override void UpdateCurrentConfiguration() { }

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
      var c = UnityEngine.RenderSettings.ambientLight;
      if (configuration.ConfigurableName == this._color_r) {
        c.r = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._color_g) {
        c.g = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._color_b) {
        c.b = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._intensity) {
        //c.a = configuration.ConfigurableValue;
        UnityEngine.RenderSettings.ambientIntensity = configuration.ConfigurableValue;
        UnityEngine.RenderSettings.reflectionIntensity =
            UnityEngine.Mathf.Clamp01(value : configuration.ConfigurableValue);
        //RenderSettings.skybox.SetFloat("_Exposure", configuration.ConfigurableValue);
      } else if (configuration.ConfigurableName == this._reflection_intensity) {
        //c.a = configuration.ConfigurableValue;
//        RenderSettings.reflectionIntensity = configuration.ConfigurableValue;
        //RenderSettings.skybox.SetFloat("_Exposure", configuration.ConfigurableValue);
      }

      UnityEngine.RenderSettings.ambientLight = c;
      UnityEngine.DynamicGI.UpdateEnvironment();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var o = this._intensity_space.Sample();
      var v = this._color_space.Sample();

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._color_r,
                         configurable_value : v.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._color_g,
                         configurable_value : v.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._color_b,
                         configurable_value : v.z),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._intensity,
                         configurable_value : o.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name :
                         this._reflection_intensity,
                         configurable_value : o.y)
                   };
    }
  }
}