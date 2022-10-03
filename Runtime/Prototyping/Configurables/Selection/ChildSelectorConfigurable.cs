namespace droid.Runtime.Prototyping.Configurables.Selection {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "ChildSelector"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public class ChildSelectorConfigurable : Configurable,
                                           droid.Runtime.Interfaces.ICategoryProvider {
    [UnityEngine.SerializeField] UnityEngine.Renderer active;
    [UnityEngine.SerializeField] UnityEngine.Renderer[] children;
    [UnityEngine.SerializeField] int len;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Sample.SampleSpace1 _configurable_value_space =
        new droid.Runtime.Structs.Space.Sample.SampleSpace1();

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISamplable ConfigurableValueSpace {
      get { return this._configurable_value_space; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public int CurrentCategoryValue { get; set; }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 Space1 { get { return this._configurable_value_space._space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      base.Setup();

      if (!UnityEngine.Application.isPlaying) {
        return;
      }

      var la = new System.Collections.Generic.List<UnityEngine.Renderer>();
      foreach (UnityEngine.Transform child in this.transform) {
        var o = child.gameObject.GetComponent<UnityEngine.Renderer>();
        o.enabled = false;
        this.active = o;
        la.Add(item : o);
      }

      this.children = la.ToArray();

      this.len = this.children.Length;

      if (this.active) {
        this.active.enabled = true;
      }

      this._configurable_value_space._space.DecimalGranularity = 0;
      this._configurable_value_space._space.Max = this.len - 1;
    }

    public override void UpdateCurrentConfiguration() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration) {
      if (!UnityEngine.Application.isPlaying) {
        return;
      }

      for (var index = 0; index < this.children.Length; index++) {
        var c = this.children[index];
        c.enabled = false;
      }

      if (this.children != null && (int)configuration.ConfigurableValue < this.len) {
        this.CurrentCategoryValue = UnityEngine.Mathf.RoundToInt(f : configuration.ConfigurableValue);
        this.active = this.children[this.CurrentCategoryValue];
      }

      if (this.active) {
        this.active.enabled = true;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations() {
      return new[] {
                       new droid.Runtime.Messaging.Messages.Configuration(configurable_name : this.Identifier,
                         configurable_value : this._configurable_value_space.Sample())
                   };
    }
  }
}