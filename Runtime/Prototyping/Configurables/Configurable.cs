namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="GameObjects.PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public abstract class Configurable : droid.Runtime.GameObjects.PrototypingGameObject,
                                       droid.Runtime.Interfaces.IConfigurable {
    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("References", order = 20)]
    [field : UnityEngine.SerializeField]
    public droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment ParentEnvironment {
      get;
      set;
    } = null;

    void Update() {
      if (this.RandomSamplingPhaseEnum == droid.Runtime.Enums.RandomSamplingPhaseEnum.On_update_
          && UnityEngine.Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Random reconfiguring {this} Update");
        }
        #endif
        this.Randomise();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public droid.Runtime.Enums.RandomSamplingPhaseEnum RandomSamplingPhaseEnum { get; set; } =
      droid.Runtime.Enums.RandomSamplingPhaseEnum.Disabled_;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract droid.Runtime.Messaging.Messages.Configuration[] SampleConfigurations();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void UpdateCurrentConfiguration();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public abstract void
        ApplyConfiguration(droid.Runtime.Interfaces.IConfigurableConfiguration configuration);

    /// <inheritdoc />
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      if (this.RandomSamplingPhaseEnum == droid.Runtime.Enums.RandomSamplingPhaseEnum.On_reset_
          && UnityEngine.Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Random reconfiguring {this} Reset");
        }
        #endif
        this.Randomise();
      }
    }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      this.UpdateCurrentConfiguration();

      if (this.ParentEnvironment != null) {
        this.ParentEnvironment.PreTickEvent += this.PreTick;
        this.ParentEnvironment.PostTickEvent += this.PostTick;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Tick() {
      if (this.RandomSamplingPhaseEnum == droid.Runtime.Enums.RandomSamplingPhaseEnum.On_tick_
          && UnityEngine.Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this);
    }

    void PostTick() {
      if (this.RandomSamplingPhaseEnum == droid.Runtime.Enums.RandomSamplingPhaseEnum.On_post_tick_
          && UnityEngine.Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    void PreTick() {
      if (this.RandomSamplingPhaseEnum == droid.Runtime.Enums.RandomSamplingPhaseEnum.On_pre_tick_
          && UnityEngine.Application.isPlaying) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Random reconfiguring {this} Tick");
        }
        #endif
        this.Randomise();
      }
    }

    /// <summary>
    /// </summary>
    protected virtual void Randomise() {
      var vs = this.SampleConfigurations();
      for (var index = 0; index < vs.Length; index++) {
        var v = vs[index];
        this.ApplyConfiguration(configuration : v);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(configurable : this); }

    #region Fields

    #endregion
  }
}