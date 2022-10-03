namespace droid.Runtime.Prototyping.Configurables.DomainRandomization {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Texture"
                                           + ConfigurableComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Renderer))]
  public class TextureConfigurable : Configurable {
    static readonly int _main_tex = UnityEngine.Shader.PropertyToID("_MainTex");
    [UnityEngine.SerializeField] UnityEngine.Texture[] _textures = null;
    [UnityEngine.SerializeField] bool load_from_resources_if_empty = true;
    [UnityEngine.SerializeField] UnityEngine.Texture _texture = null;
    [UnityEngine.SerializeField] UnityEngine.Renderer _renderer = null;
    [UnityEngine.SerializeField] bool use_shared = false;
    [UnityEngine.SerializeField] int _last_sample;
    [UnityEngine.SerializeField] string load_path = "Textures";
    UnityEngine.Material _mat;

    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._renderer = this.GetComponent<UnityEngine.Renderer>();
      if (UnityEngine.Application.isPlaying) {
        if (this.use_shared) {
          this._mat = this._renderer?.sharedMaterial;
        } else {
          this._mat = this._renderer?.material;
        }
      }

      if (this.load_from_resources_if_empty) {
        if (this._textures == null || this._textures.Length == 0) {
          this._textures = UnityEngine.Resources.LoadAll<UnityEngine.Texture>(path : this.load_path);
        }
      }
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

      this._texture = this._textures[(int)configuration.ConfigurableValue];

      this._mat.SetTexture(nameID : _main_tex, value : this._texture);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Randomise() {
      this._texture = this._textures[UnityEngine.Random.Range(0, maxExclusive : this._textures.Length)];

      this._mat.SetTexture(nameID : _main_tex, value : this._texture);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      this._last_sample = UnityEngine.Random.Range(0, maxExclusive : this._textures.Length);

      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this.Identifier,
                         configurable_value : this._last_sample)
                   };
    }
  }
}