namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "StandardShaderMaterial"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Renderer))]
  public class StandardShaderMaterialConfigurable : Configurable,
                                                    droid.Runtime.Interfaces.IHasTArray {
    static readonly int _glossiness = UnityEngine.Shader.PropertyToID("_Glossiness");
    static readonly int _glossy_reflections = UnityEngine.Shader.PropertyToID("_GlossyReflections");
    static readonly int _main_tex = UnityEngine.Shader.PropertyToID("_MainTex");

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace2 _tiling_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace2 {
                                                                Space = droid.Runtime.Structs.Space.Space2
                                                                    .TwentyEighty
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace2 _offset_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace2 {
                                                                Space = droid.Runtime.Structs.Space.Space2
                                                                    .TwentyEighty
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace4 _color_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace4 {
                                                                Space = droid.Runtime.Structs.Space.Space4
                                                                    .TwentyEighty
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _smoothness_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                Space = droid.Runtime.Structs.Space.Space1
                                                                    .TwentyEighty
                                                            };

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _reflection_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1 {
                                                                Space = droid.Runtime.Structs.Space.Space1
                                                                    .TwentyEighty
                                                            };

    [UnityEngine.SerializeField] bool _use_shared = false;
    string _a;
    string _b;
    string _g;
    string _offset_x;
    string _offset_y;
    string _r;
    string _reflection;

    /// <summary>
    /// </summary>
    UnityEngine.Renderer _renderer;

    string _smoothness;
    string _tiling_x;
    string _tiling_y;

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get { return this._tiling_space; } }

    /// <summary>
    /// </summary>
    public dynamic[] ObservationArray { get { return new dynamic[] { }; } }

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISpace[] ObservationSpace {
      get { return new[] {this._tiling_space.Space}; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._r = this.Identifier + "R";
      this._g = this.Identifier + "G";
      this._b = this.Identifier + "B";
      this._a = this.Identifier + "A";
      this._reflection = this.Identifier + "Reflection";
      this._smoothness = this.Identifier + "Smoothness";
      this._tiling_x = this.Identifier + "TilingX";
      this._tiling_y = this.Identifier + "TilingY";
      this._offset_x = this.Identifier + "OffsetX";
      this._offset_y = this.Identifier + "OffsetY";

      this._renderer = this.GetComponent<UnityEngine.Renderer>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._r);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._g);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._b);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._a);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._reflection);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._smoothness);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._offset_x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._offset_y);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._tiling_x);
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : (Configurable)this,
            identifier : this._tiling_y);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._r);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._g);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._b);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._a);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._reflection);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._smoothness);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._offset_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._offset_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._tiling_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._tiling_y);
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

      if (!this._use_shared) {
        for (var index = 0; index < this._renderer.materials.Length; index++) {
          var mat = this._renderer.materials[index];
          var c = mat.color;

          if (configuration.ConfigurableName.Equals(value : this._r,
                                                    comparisonType : System.StringComparison.Ordinal)) {
            c.r = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._g,
                                   comparisonType : System.StringComparison.Ordinal)) {
            c.g = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._b,
                                   comparisonType : System.StringComparison.Ordinal)) {
            c.b = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._a,
                                   comparisonType : System.StringComparison.Ordinal)) {
            c.a = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._smoothness,
                                   comparisonType : System.StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossiness, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._reflection,
                                   comparisonType : System.StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossy_reflections, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_x,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_y,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_x,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_y,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          }

          mat.color = c;
        }
      } else {
        foreach (var mat in this._renderer.sharedMaterials) {
          var c = mat.color;

          if (string.Equals(a : configuration.ConfigurableName,
                            b : this._r,
                            comparisonType : System.StringComparison.Ordinal)) {
            c.r = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._g,
                                   comparisonType : System.StringComparison.Ordinal)) {
            c.g = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._b,
                                   comparisonType : System.StringComparison.Ordinal)) {
            c.b = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._a,
                                   comparisonType : System.StringComparison.Ordinal)) {
            c.a = configuration.ConfigurableValue;
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._smoothness,
                                   comparisonType : System.StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossiness, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._reflection,
                                   comparisonType : System.StringComparison.Ordinal)) {
            mat.SetFloat(nameID : _glossy_reflections, value : configuration.ConfigurableValue);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_x,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._offset_y,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureOffset(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureOffset(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_x,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.x = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          } else if (string.Equals(a : configuration.ConfigurableName,
                                   b : this._tiling_y,
                                   comparisonType : System.StringComparison.Ordinal)) {
            var a = mat.GetTextureScale(nameID : _main_tex);
            a.y = configuration.ConfigurableValue;
            mat.SetTextureScale(nameID : _main_tex, value : a);
          }

          mat.color = c;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      var cs1 = this._color_space.Sample();
      var tl1 = this._tiling_space.Sample();
      var os1 = this._offset_space.Sample();
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._r,
                         configurable_value : cs1.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._g,
                         configurable_value : cs1.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._b,
                         configurable_value : cs1.z),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._a,
                         configurable_value : cs1.w),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name :
                         this._reflection,
                         configurable_value : this._reflection_space.Sample()),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name :
                         this._smoothness,
                         configurable_value : this._smoothness_space.Sample()),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._tiling_x,
                         configurable_value : tl1.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._tiling_y,
                         configurable_value : tl1.y),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._offset_x,
                         configurable_value : os1.x),
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this._offset_y,
                         configurable_value : os1.y)
                   };
    }

    /// <summary>
    /// </summary>
    protected override void Randomise() {
      UnityEngine.Material[] materials;
      if (this._use_shared) {
        materials = this._renderer.sharedMaterials;
      } else {
        materials = this._renderer.materials;
      }

      foreach (var mat in materials) {
        if (mat) {
          mat.color = this._color_space.Sample();
          mat.SetTextureScale(nameID : _main_tex, value : this._tiling_space.Sample());
          mat.SetTextureOffset(nameID : _main_tex, value : this._offset_space.Sample());
          mat.SetFloat(nameID : _glossiness, value : this._smoothness_space.Sample());
          mat.SetFloat(nameID : _glossy_reflections, value : this._reflection_space.Sample());
        }
      }
    }
  }
}