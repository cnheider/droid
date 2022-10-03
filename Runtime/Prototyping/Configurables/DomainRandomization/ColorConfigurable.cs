namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Color"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Renderer))]
  public class ColorConfigurable : Configurable {
    /// <summary>
    ///   Alpha
    /// </summary>
    const char _a = 'A';

    /// <summary>
    ///   Blue
    /// </summary>
    const char _b = 'B';

    /// <summary>
    ///   Green
    /// </summary>
    const char _g = 'G';

    /// <summary>
    ///   Red
    /// </summary>
    const char _r = 'R';

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace4 _space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace4 {
                                                                _space = droid.Runtime.Structs.Space.Space4
                                                                    .TwentyEighty
                                                            };

    [UnityEngine.SerializeField] bool use_shared = false;
    string _a_id;
    string _b_id;
    string _g_id;

    string _r_id;

    /// <summary>
    /// </summary>
    UnityEngine.Renderer _renderer;

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get { return this._space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._r_id = this.Identifier + _r;
      this._b_id = this.Identifier + _b;
      this._g_id = this.Identifier + _g;
      this._a_id = this.Identifier + _a;

      this._renderer = this.GetComponent<UnityEngine.Renderer>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._r_id);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._g_id);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._b_id);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this,
            identifier : this._a_id);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._r_id);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._b_id);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._g_id);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._a_id);
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

      if (this.use_shared) {
        for (var index = 0; index < this._renderer.sharedMaterials.Length; index++) {
          var mat = this._renderer.sharedMaterials[index];
          var c = mat.color;

          switch (configuration.ConfigurableName[index : configuration.ConfigurableName.Length - 1]) {
            case _r:
              c.r = configuration.ConfigurableValue;
              break;
            case _g:
              c.g = configuration.ConfigurableValue;
              break;
            case _b:
              c.b = configuration.ConfigurableValue;
              break;
            case _a:
              c.a = configuration.ConfigurableValue;
              break;
          }

          mat.color = c;
        }
      } else {
        for (var index = 0; index < this._renderer.materials.Length; index++) {
          var mat = this._renderer.materials[index];
          var c = mat.color;

          switch (configuration.ConfigurableName[index : configuration.ConfigurableName.Length - 1]) {
            case _r:
              c.r = configuration.ConfigurableValue;
              break;
            case _g:
              c.g = configuration.ConfigurableValue;
              break;
            case _b:
              c.b = configuration.ConfigurableValue;
              break;
            case _a:
              c.a = configuration.ConfigurableValue;
              break;
          }

          mat.color = c;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Randomise() {
      if (this.use_shared) {
        foreach (var mat in this._renderer.sharedMaterials) {
          mat.color = this._space.Sample();
        }
      } else {
        foreach (var mat in this._renderer.materials) {
          mat.color = this._space.Sample();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var v = this._space.Sample();

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._r_id,
                         configurable_value : v.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._g_id,
                         configurable_value : v.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._b_id,
                         configurable_value : v.z),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._a_id,
                         configurable_value : v.w)
                   };
    }
  }
}