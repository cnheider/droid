namespace droid.Runtime.Structs {
  /// <inheritdoc cref="UnityEngine.ScriptableObject" />
  /// <summary>
  ///   Contains everything relevant to configuring simulation environments engine specific settings
  /// </summary>
  [UnityEngine.CreateAssetMenuAttribute(fileName = "SimulatorConfiguration",
                                        menuName = droid.Runtime.ScriptableObjects.ScriptableObjectMenuPath
                                                        ._ScriptableObjectMenuPath
                                                   + "SimulatorConfiguration",
                                        order = 1)]
  [System.SerializableAttribute]
  public class SimulatorConfiguration : UnityEngine.ScriptableObject,
                                        droid.Runtime.Interfaces.ISimulatorConfiguration {
    [UnityEngine.HeaderAttribute("Performance (Disable for faster serialisation, but with loss of functionality)")]
    [UnityEngine.SerializeField]
    bool _always_serialise_unobservables = true;

    [UnityEngine.SerializeField] bool _always_serialise_individual_observables = true;

    [UnityEngine.SerializeField] bool _always_serialise_aggregated_float_array = true;

    [UnityEngine.HeaderAttribute("Graphics")]
    [UnityEngine.SerializeField]
    bool _apply_resolution_settings = false;

    [UnityEngine.SerializeField] bool _optimiseWindow_for_speed = false;

    [UnityEngine.SerializeField] bool _apply_quality_settings = false;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(1, 9999)]
    int _height = 500;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(1, 9999)]
    int _width = 500;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(1, 4)]
    int _quality_level = 1;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 4)]
    int _v_sync_count = 0;

    [UnityEngine.SerializeField] bool _full_screen = false;
    [UnityEngine.SerializeField] bool _resizable_window = true;
    [UnityEngine.SerializeField] UnityEngine.ColorSpace _unityColor_space = UnityEngine.ColorSpace.Linear;

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Simulation")]
    [UnityEngine.SerializeField]
    [droid.Runtime.Utilities.SearchableEnumAttribute]
    droid.Runtime.Enums.FrameFinishesEnum
        _frame_finishes = droid.Runtime.Enums.FrameFinishesEnum.Late_update_;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 99)]
    int _frame_skips = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Connection")]
    [UnityEngine.SerializeField]
    string _ip_address = "localhost";

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 9999)]
    float _max_reply_interval = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 999)]
    int _num_of_environments = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _port = 6969;

    [UnityEngine.SerializeField] bool _replay_reaction_in_skips;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    [droid.Runtime.Utilities.SearchableEnumAttribute]
    droid.Runtime.Enums.SimulationTypeEnum _simulation_type =
        droid.Runtime.Enums.SimulationTypeEnum.Independent_;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    //[SearchableEnum]
    droid.Runtime.Messaging.Messages.ExecutionPhaseEnum _step_execution_phase =
        droid.Runtime.Messaging.Messages.ExecutionPhaseEnum.On_tick_;

    /// <summary>
    ///   Target frame rate = -1 means that no waiting/v-syncing is done and the simulation can run as fast as
    ///   possible.
    /// </summary>
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(-1, 9999)]
    int _target_frame_rate = -1;

    /// <summary>
    ///   Allows physics loop to be run more often than frame loop
    /// </summary>
    [UnityEngine.HeaderAttribute("Time")]
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0f, 100f)]
    float _time_scale = 1;

    /// <summary>
    ///   WARNING When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    /// </summary>
    [UnityEngine.HeaderAttribute("Experimental (Warning, it is important to read docs before use!)")]
    [UnityEngine.SerializeField]
    bool _update_fixed_time_scale = false;

    [UnityEngine.SerializeField] bool _manual_render = false;

    /// <summary>
    /// </summary>
    public void SetToDefault() {
      this.Width = 500;
      this.Height = 500;
      this.FullScreen = false;
      this.QualityLevel = 1;
      this.TimeScale = 1;
      this.TargetFrameRate = -1;
      this.SimulationType = droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_;
      this.FrameFinishes = droid.Runtime.Enums.FrameFinishesEnum.Late_update_;
      this.FrameSkips = 0;
      this.MaxReplyInterval = 0;
      this.NumOfEnvironments = 1;
      this.ResizableWindow = true;
      this.UnityColorSpace = UnityEngine.ColorSpace.Linear;
      this.VSyncCount = 0;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var e = "";

      e += this.name;
      e += ", ";
      e += this.SimulationType.ToString();

      return e;
    }

    #region Getter Setters

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int FrameSkips {
      get { return this._frame_skips; }
      set {
        if (value >= 0) {
          this._frame_skips = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Enums.SimulationTypeEnum SimulationType {
      get { return this._simulation_type; }
      set { this._simulation_type = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool ApplyResolutionSettings {
      get { return this._apply_resolution_settings; }
      set { this._apply_resolution_settings = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool ApplyQualitySettings {
      get { return this._apply_quality_settings; }
      set { this._apply_quality_settings = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool ResizableWindow {
      get { return this._resizable_window; }
      set { this._resizable_window = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.ColorSpace UnityColorSpace {
      get { return this._unityColor_space; }
      set { this._unityColor_space = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int VSyncCount { get { return this._v_sync_count = 0; } set { this._v_sync_count = value; } }

    public bool ManualRender { get { return this._manual_render; } set { this._manual_render = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int Width {
      get { return this._width; }
      set {
        if (value >= 0) {
          this._width = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int Height {
      get { return this._height; }
      set {
        if (value >= 0) {
          this._height = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool FullScreen { get { return this._full_screen; } set { this._full_screen = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int NumOfEnvironments {
      get { return this._num_of_environments; }
      set { this._num_of_environments = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int TargetFrameRate {
      get { return this._target_frame_rate; }
      set {
        if (value >= -1) {
          this._target_frame_rate = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int QualityLevel {
      get { return this._quality_level; }
      set {
        if (value >= 1 && value <= 4) {
          this._quality_level = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public float TimeScale {
      get { return this._time_scale; }
      set {
        if (value >= 0) {
          this._time_scale = value;
        }
      }
    }

    /// <summary>
    /// </summary>
    public float MaxReplyInterval {
      get { return this._max_reply_interval; }
      set { this._max_reply_interval = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Enums.FrameFinishesEnum FrameFinishes {
      get { return this._frame_finishes; }
      set { this._frame_finishes = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Messaging.Messages.ExecutionPhaseEnum StepExecutionPhase {
      get { return this._step_execution_phase; }
      set { this._step_execution_phase = value; }
    }

    /// <inheritdoc />
    /// <summary>
    ///   WARNING When _update_fixed_time_scale is true, MAJOR slow downs due to PHYSX updates on change.
    /// </summary>
    public bool UpdateFixedTimeScale {
      get { return this._update_fixed_time_scale; }
      set { this._update_fixed_time_scale = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool SerialiseAggregatedFloatArray {
      get { return this._always_serialise_aggregated_float_array; }
      set { this._always_serialise_aggregated_float_array = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool SerialiseUnobservables {
      get { return this._always_serialise_unobservables; }
      set { this._always_serialise_unobservables = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool SerialiseIndividualObservables {
      get { return this._always_serialise_individual_observables; }
      set { this._always_serialise_individual_observables = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public bool ReplayReactionInSkips {
      get { return this._replay_reaction_in_skips; }
      set { this._replay_reaction_in_skips = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int Port { get { return this._port; } set { this._port = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string IpAddress { get { return this._ip_address; } set { this._ip_address = value; } }

    /// <summary>
    /// </summary>
    public bool OptimiseWindowForSpeed {
      get { return this._optimiseWindow_for_speed; }
      set { this._optimiseWindow_for_speed = value; }
    }

    #endregion
  }
}