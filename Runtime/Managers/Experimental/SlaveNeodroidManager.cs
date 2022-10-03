#if UNITY_EDITOR

#endif

namespace droid.Runtime.Managers.Experimental {
  /// <inheritdoc cref="UnityEngine.MonoBehaviour" />
  /// <summary>
  /// </summary>
  [UnityEngine.DisallowMultipleComponent]
  [UnityEngine.AddComponentMenu("Neodroid/Managers/SlaveNeodroidManager")]
  public class SlaveNeodroidManager : UnityEngine.MonoBehaviour,
                                      droid.Runtime.Interfaces.IManager {
    /// <summary>
    /// </summary>
    public static SlaveNeodroidManager Instance { get; private set; }

    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISimulatorConfiguration Configuration {
      get {
        if (this._configuration == null) {
          this._configuration = UnityEngine.ScriptableObject
                                           .CreateInstance<droid.Runtime.Structs.SimulatorConfiguration>();
        }

        return this._configuration;
      }
      set { this._configuration = (droid.Runtime.Structs.SimulatorConfiguration)value; }
    }

    /// <summary>
    ///   Can be subscribed to for pre fixed update events (Will be called before any FixedUpdate on any script)
    /// </summary>
    public event System.Action EarlyFixedUpdateEvent;

    /// <summary>
    /// </summary>
    public event System.Action FixedUpdateEvent;

    /// <summary>
    /// </summary>
    public event System.Action LateFixedUpdateEvent;

    /// <summary>
    ///   Can be subscribed to for pre update events (Will be called before any Update on any script)
    /// </summary>
    public event System.Action EarlyUpdateEvent;

    /// <summary>
    /// </summary>
    public event System.Action UpdateEvent;

    /// <summary>
    /// </summary>
    public event System.Action LateUpdateEvent;

    /// <summary>
    /// </summary>
    public event System.Action OnPostRenderEvent;

    /// <summary>
    /// </summary>
    public event System.Action OnRenderImageEvent;

    /// <summary>
    /// </summary>
    public event System.Action OnEndOfFrameEvent;

    /// <summary>
    /// </summary>
    public event System.Action OnReceiveEvent;

    /// <summary>
    /// </summary>
    void FetchCommandLineArguments() {
      var arguments = System.Environment.GetCommandLineArgs();

      for (var i = 0; i < arguments.Length; i++) {
        if (arguments[i] == "-ip") {
          this.Configuration.IpAddress = arguments[i + 1];
        }

        if (arguments[i] == "-port") {
          this.Configuration.Port = int.Parse(s : arguments[i + 1]);
        }
      }
    }

    /// <summary>
    /// </summary>
    void CreateMessagingClient() {
      try {
        if (this.Configuration.IpAddress != "" || this.Configuration.Port != 0) {
          this._Message_Client =
              new droid.Runtime.Messaging.Experimental.MessageClient(ip_address :
                                                                     this.Configuration.IpAddress,
                                                                     port : this.Configuration.Port,
                                                                     false,
                                                                     #if NEODROID_DEBUG
                                                                     debug : this.Debugging
                                                                     #else
                                                   false
                                                                     #endif
                                                                    );
        } else {
          this._Message_Client = new droid.Runtime.Messaging.Experimental.MessageClient(
             #if NEODROID_DEBUG
             debug : this.Debugging
             #else
                                                   false
             #endif
            );
        }
      } catch (System.Exception exception) {
        UnityEngine.Debug.Log(message : exception);
        throw;

        //TODO: close application is port is already in use.
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="threaded"></param>
    void StartMessagingServer(bool threaded = false) {
      this._Message_Client.ListenForClientToConnect(debug_callback : this.OnDebugCallback);
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(" Messaging Server is listening for clients");
      }
      #endif

      if (threaded) {
        this.OnListeningCallback();
      }

      //}
    }

    /// <summary>
    /// </summary>
    /// <param name="recipient"></param>
    public void StatusString(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.GetStatus());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var c = this.SimulatorConfiguration.ToString();
      var e = System.Linq.Enumerable.FirstOrDefault(source : this._Environments).Value.ToString();

      return $"{c}, {e}";
    }

    #region PrivateFields

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Development", order = 110)]
    [UnityEngine.SerializeField]
    bool _debugging;

    /// <summary>
    /// </summary>
    object _send_lock = new object();

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _testing_Actuators;

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    const int _script_execution_order = -1000;
    #endif

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Simulation", order = 80)]
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.SimulatorConfiguration _configuration;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    int _skip_frame_i;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _syncing_environments;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _awaiting_reply;

    [UnityEngine.SerializeField] bool _step;

    UnityEngine.WaitForEndOfFrame _wait_for_end_of_frame = new UnityEngine.WaitForEndOfFrame();
    UnityEngine.WaitForFixedUpdate _wait_for_fixed_update = new UnityEngine.WaitForFixedUpdate();

    #endregion

    #region Getter And Setters

    /// <summary>
    /// </summary>
    public droid.Runtime.Messaging.Messages.Reaction[] CurrentReactions {
      get {
        lock (this._send_lock) {
          return this._Current_Reactions;
        }
      }
      set {
        lock (this._send_lock) {
          this._Current_Reactions = value;
        }
      }
    }

    /// <summary>
    /// </summary>
    public float SimulationTimeScale {
      get { return UnityEngine.Time.timeScale; }
      set {
        #if UNITY_EDITOR
        UnityEngine.Time.timeScale = System.Math.Min(val1 : value, 99);
        this._last_simulation_time = System.Math.Min(val1 : value, 99);
        #else
        Time.timeScale = value;
        this._last_simulation_time = value;
        #endif

        if (this.Configuration.UpdateFixedTimeScale) {
          UnityEngine.Time.fixedDeltaTime = 0.02F * UnityEngine.Time.timeScale;
        }
      }
    }

    [UnityEngine.SerializeField] float _last_simulation_time;

    /// <summary>
    /// </summary>
    public bool HasStepped { get { return this._has_stepped; } set { this._has_stepped = value; } }

    /// <summary>
    /// </summary>
    public bool TestActuators {
      get { return this._testing_Actuators; }
      set { this._testing_Actuators = value; }
    }

    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>
    public bool Debugging {
      get { return this._debugging; }
      set {
        if (this._Message_Client != null) {
          this._Message_Client.Debugging = value;
        }

        this._debugging = value;
      }
    }
    #endif

    /// <summary>
    /// </summary>
    public bool AwaitingReply {
      get {
        lock (this._send_lock) {
          return this._awaiting_reply;
        }
      }
      set {
        lock (this._send_lock) {
          this._awaiting_reply = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISimulatorConfiguration SimulatorConfiguration {
      get { return this._configuration; }
    }

    /// <summary>
    /// </summary>
    public bool IsSyncingEnvironments {
      get { return this._syncing_environments; }
      set { this._syncing_environments = value; }
    }

    /// <summary>
    /// </summary>
    public bool Stepping { get { return this._step; } }

    #endregion

    #region PrivateMembers

    /// <summary>
    /// </summary>
    protected System.Collections.Generic.Dictionary<string, droid.Runtime.Interfaces.IEnvironment>
        _Environments =
            new System.Collections.Generic.Dictionary<string, droid.Runtime.Interfaces.IEnvironment>();

    /// <summary>
    /// </summary>
    public void Clear() { this._Environments.Clear(); }

    /// <summary>
    /// </summary>
    protected droid.Runtime.Messaging.Experimental.MessageClient _Message_Client;

    /// <summary>
    /// </summary>
    protected droid.Runtime.Messaging.Messages.Reaction[] _Current_Reactions = { };

    [UnityEngine.SerializeField] bool _has_stepped;

    #endregion

    #region UnityCallbacks

    /// <summary>
    /// </summary>
    protected void Awake() {
      if (Instance == null) {
        Instance = this;
      } else if (Instance == this) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : "Using " + Instance);
        }
        #endif
      } else {
        UnityEngine.Debug.LogWarning(message :
                                     "WARNING! There are multiple SimulationManagers in the scene! Only using "
                                     + Instance);
      }

      this.Setup();

      #if UNITY_EDITOR
      if (!UnityEngine.Application.isPlaying) {
        var manager_script = UnityEditor.MonoScript.FromMonoBehaviour(behaviour : this);
        if (UnityEditor.MonoImporter.GetExecutionOrder(script : manager_script) != _script_execution_order) {
          UnityEditor.MonoImporter.SetExecutionOrder(script : manager_script,
                                                     order :
                                                     _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          UnityEngine.Debug
                     .LogWarning("Execution Order changed, you will need to press play again to make everything function correctly!");
          UnityEditor.EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif
    }

    public virtual void Setup() { }

    /// <summary>
    /// </summary>
    protected void Start() {
      this.FetchCommandLineArguments();

      if (this.Configuration == null) {
        this.Configuration = UnityEngine.ScriptableObject
                                        .CreateInstance<droid.Runtime.Structs.SimulatorConfiguration>();
      }

      this.ApplyConfigurationToUnity(configuration : this.Configuration);

      if (this.Configuration.SimulationType == droid.Runtime.Enums.SimulationTypeEnum.Physics_dependent_) {
        this.EarlyFixedUpdateEvent += this.OnPreTick;
        this.FixedUpdateEvent += this.OnTick;
        this.FixedUpdateEvent += this.Tick;
        this.LateFixedUpdateEvent += this.OnPostTick;
        this.StartCoroutine(routine : this.LateFixedUpdateEventGenerator());
      } else {
        this.EarlyUpdateEvent += this.OnPreTick;
        this.UpdateEvent += this.OnTick;
        this.UpdateEvent += this.Tick;
        switch (this.Configuration.FrameFinishes) {
          case droid.Runtime.Enums.FrameFinishesEnum.Late_update_:
            this.LateUpdateEvent += this.OnPostTick;
            break;
          case droid.Runtime.Enums.FrameFinishesEnum.On_post_render_:
            this.OnPostRenderEvent += this.OnPostTick;
            break;
          case droid.Runtime.Enums.FrameFinishesEnum.On_render_image_:
            if (!this.GetComponent<UnityEngine.Camera>()) {
              throw new
                  UnityEngine.MissingComponentException("Missing a camera component on Managers gameobject");
            }

            this.OnRenderImageEvent += this.OnPostTick;
            break;
          case droid.Runtime.Enums.FrameFinishesEnum.End_of_frame_:
            this.StartCoroutine(routine : this.EndOfFrameEventGenerator());
            this.OnEndOfFrameEvent += this.OnPostTick;
            break;
          default: throw new System.ArgumentOutOfRangeException();
        }
      }

      this.CreateMessagingClient();
      if (this.Configuration.SimulationType == droid.Runtime.Enums.SimulationTypeEnum.Physics_dependent_) {
        this.StartMessagingServer(); // Remember to manually bind receive to an event in a derivation
      } else {
        this.StartMessagingServer(true);
      }
    }

    /// <summary>
    /// </summary>
    public void ApplyConfigurationToUnity(droid.Runtime.Interfaces.ISimulatorConfiguration configuration) {
      if (configuration.ApplyQualitySettings) {
        UnityEngine.QualitySettings.SetQualityLevel(index : configuration.QualityLevel, true);
        UnityEngine.QualitySettings.vSyncCount = configuration.VSyncCount;
      }

      this.SimulationTimeScale = configuration.TimeScale;
      UnityEngine.Application.targetFrameRate = configuration.TargetFrameRate;

      if (this._configuration.OptimiseWindowForSpeed) {
        UnityEngine.Screen.SetResolution(1, 1, false);
      }
      #if !UNITY_EDITOR
      else if( configuration.ApplyResolutionSettings ){
      Screen.SetResolution(
          width : configuration.Width,
          height : configuration.Height,
          fullscreen : configuration.FullScreen);
        }
      #else

      UnityEditor.PlayerSettings.resizableWindow = configuration.ResizableWindow;
      UnityEditor.PlayerSettings.colorSpace = configuration.UnityColorSpace;
      UnityEditor.PlayerSettings.displayResolutionDialog = UnityEditor.ResolutionDialogSetting.Disabled;
      //PlayerSettings.use32BitDisplayBuffer
      #endif
    }

    /// <summary>
    /// </summary>
    void OnPostRender() { this.OnPostRenderEvent?.Invoke(); }

    /// <summary>
    /// </summary>
    /// <param name="src"></param>
    /// <param name="dest"></param>
    void OnRenderImage(UnityEngine.RenderTexture src, UnityEngine.RenderTexture dest) {
      this.OnRenderImageEvent?.Invoke(); //TODO: May not work
    }

    /// <summary>
    /// </summary>
    protected void FixedUpdate() {
      this.EarlyFixedUpdateEvent?.Invoke();
      this.FixedUpdateEvent?.Invoke();
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    System.Collections.IEnumerator LateFixedUpdateEventGenerator() {
      while (true) {
        yield return this._wait_for_fixed_update;
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("LateFixedUpdate");
        }
        #endif
        this.LateFixedUpdateEvent?.Invoke();
      }

      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    protected System.Collections.IEnumerator EndOfFrameEventGenerator() {
      while (true) {
        yield return this._wait_for_end_of_frame;
        //yield return new WaitForEndOfFrame();
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("EndOfFrameEvent");
        }
        #endif
        this.OnEndOfFrameEvent?.Invoke();
      }
      #pragma warning disable 162
      // ReSharper disable once HeuristicUnreachableCode
      yield return null;
      #pragma warning restore 162
      // ReSharper disable once IteratorNeverReturns
    }

    /// <summary>
    /// </summary>
    protected void Update() {
      this.EarlyUpdateEvent?.Invoke();
      this.UpdateEvent?.Invoke();
    }

    /// <summary>
    /// </summary>
    protected void LateUpdate() { this.LateUpdateEvent?.Invoke(); }

    #endregion

    #region PrivateMethods

    /// <summary>
    /// </summary>
    protected void OnPreTick() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("OnPreTick");
      }
      #endif

      if (this.Configuration.StepExecutionPhase
          == droid.Runtime.Messaging.Messages.ExecutionPhaseEnum.Before_tick_) {
        this.ExecuteStep();
      }
    }

    /// <summary>
    /// </summary>
    protected void OnTick() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("OnTick");
      }
      #endif
      if (this.Configuration.StepExecutionPhase
          == droid.Runtime.Messaging.Messages.ExecutionPhaseEnum.On_tick_) {
        this.ExecuteStep();
      }
    }

    /// <summary>
    /// </summary>
    protected void OnPostTick() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("OnPostTick");
      }
      #endif

      foreach (var environment in this._Environments.Values) {
        environment.PostStep();
      }

      if (this.Configuration.StepExecutionPhase
          == droid.Runtime.Messaging.Messages.ExecutionPhaseEnum.After_tick_) {
        this.ExecuteStep();
      }

      this.CurrentReactions = new droid.Runtime.Messaging.Messages.Reaction[] { };
    }

    /// <summary>
    /// </summary>
    void ExecuteStep() {
      if (!this.HasStepped) {
        if (!this._syncing_environments) {
          this.React(reactions : this.CurrentReactions);
        }

        if (this.AwaitingReply) {
          var states = this.CollectStates();
          this.PostReact(states : states);
        }

        this.HasStepped = true;
      }
    }

    /// <summary>
    /// </summary>
    protected void Tick() {
      if (this.TestActuators) {
        this.React(reactions : this.SampleRandomReactions());
        this.CollectStates();
      }

      foreach (var environment in this._Environments.Values) {
        environment.Tick();
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("Tick");
      }
      #endif
    }

    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    protected void PostReact(droid.Runtime.Messaging.Messages.EnvironmentSnapshot[] states) {
      lock (this._send_lock) {
        foreach (var env in this._Environments.Values) {
          if (env.IsResetting) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log(message : $"Environment {env} is resetting");
            }
            #endif

            this._syncing_environments = true;
            return;
          }
        }

        this._syncing_environments = false;
        if (this._skip_frame_i >= this.Configuration.FrameSkips) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log("Not skipping frame, replying...");
          }
          #endif

          this.Reply(states : states);
          this.AwaitingReply = false;
          this._skip_frame_i = 0;
        } else {
          this._skip_frame_i += 1;
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log(message :
                                  $"Skipping frame, {this._skip_frame_i}/{this.Configuration.FrameSkips}");
          }
          #endif
          if (this.Configuration.ReplayReactionInSkips) { }
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    protected droid.Runtime.Messaging.Messages.Reaction[] SampleRandomReactions() {
      var sample_reactions = new System.Collections.Generic.List<droid.Runtime.Messaging.Messages.Reaction>();
      foreach (var environment in this._Environments.Values) {
        sample_reactions.Add(item : environment.SampleReaction());
      }

      return sample_reactions.ToArray();
    }

    //TODO: Maybe add EnvironmentState[][] states for aggregation of states in unity side buffer, when using skips?
    /// <summary>
    /// </summary>
    /// <param name="states"></param>
    void Reply(droid.Runtime.Messaging.Messages.EnvironmentSnapshot[] states) {
      lock (this._send_lock) {
        var configuration_message =
            new droid.Runtime.Messaging.Messages.SimulatorConfigurationMessage(simulator_configuration : this
                .Configuration);
        var describe = false;
        if (this.CurrentReactions != null) {
          foreach (var reaction in this.CurrentReactions) {
            if (reaction.Parameters.Describe) {
              describe = true;
            }
          }
        }

        this._Message_Client.SendStates(environment_states : states,
                                        simulator_configuration_message : configuration_message,
                                        do_serialise_unobservables :
                                        describe || this.Configuration.SerialiseUnobservables,
                                        serialise_individual_observables :
                                        describe || this.Configuration.SerialiseIndividualObservables,
                                        do_serialise_observables : describe
                                                                   || this._configuration
                                                                       .SerialiseAggregatedFloatArray);
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("Replying");
        }

        #endif
      }
    }

    #endregion

    #region PublicMethods

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public void React(droid.Runtime.Messaging.Messages.Reaction reaction) {
      this.React(reactions : new[] {reaction});
    }

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    /// <returns></returns>
    public void React(droid.Runtime.Messaging.Messages.Reaction[] reactions) {
      this.SetStepping(reactions : reactions);
      foreach (var reaction in reactions) {
        if (this._Environments.ContainsKey(key : reaction.RecipientEnvironment)) {
          this._Environments[key : reaction.RecipientEnvironment].Step(reaction : reaction);
        } else if (reaction.RecipientEnvironment == "all") {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log("Applying to all environments");
          }
          #endif

          foreach (var environment in this._Environments.Values) {
            environment.Step(reaction : reaction);
          }
        } else {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log(message :
                                  $"Could not find an environment with the identifier: {reaction.RecipientEnvironment}");
          }
          #endif
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public droid.Runtime.Messaging.Messages.EnvironmentSnapshot[] CollectStates() {
      var environments = this._Environments.Values;
      var states = new droid.Runtime.Messaging.Messages.EnvironmentSnapshot[environments.Count];
      var i = 0;
      foreach (var environment in environments) {
        states[i++] = environment.Snapshot();
      }

      return states;
    }

    void SetStepping(bool step) {
      if (step) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("Stepping from Reaction");
        }
        #endif

        this._step = true;
      } else {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log("Not stepping from Reaction");
        }
        #endif
        if (this.HasStepped) {
          this._step = false;
        }
      }
    }

    void SetStepping(droid.Runtime.Messaging.Messages.Reaction[] reactions) {
      for (var index = 0; index < reactions.Length; index++) {
        var reaction = reactions[index];
        if (reaction.Parameters.ReactionType == droid.Runtime.Messaging.Messages.ReactionTypeEnum.Step_) {
          this.SetStepping(true);
          break;
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="arg0"></param>
    public void SetTesting(bool arg0) { this.TestActuators = arg0; }

    /// <summary>
    /// </summary>
    public void ResetAllEnvironments() {
      this.React(reaction : new droid.Runtime.Messaging.Messages.Reaction(parameters :
                   new droid.Runtime.Messaging.Messages.ReactionParameters(reaction_type : droid.Runtime
                         .Messaging.Messages.ReactionTypeEnum.Reset_,
                     false,
                     true),
                   null,
                   null,
                   null,
                   null,
                   ""));
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public string GetStatus() {
      if (this._Message_Client != null) {
        return this._Message_Client._Listening_For_Clients ? "Connected" : "Not Connected";
      }

      return "No server";
    }

    #endregion

    #region Registration

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    public void Register(droid.Runtime.Interfaces.IEnvironment environment) {
      this.Register(environment : environment, identifier : environment.Identifier);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void Register(droid.Runtime.Interfaces.IEnvironment environment, string identifier) {
      if (!this._Environments.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message :
                                $"Manager {this.name} already has an environment with the identifier: {identifier}");
        }
        #endif

        this._Environments.Add(key : identifier, value : environment);
      } else {
        UnityEngine.Debug.LogWarning(message :
                                     $"WARNING! Please check for duplicates, SimulationManager {this.name} "
                                     + $"already has environment {identifier} registered");
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="environment"></param>
    /// <param name="identifier"></param>
    public void UnRegister(droid.Runtime.Interfaces.IEnvironment environment, string identifier) {
      if (this._Environments.ContainsKey(key : identifier)) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message :
                                $"SimulationManager {this.name} unregistered Environment {identifier}");
        }
        #endif

        this._Environments.Remove(key : identifier);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="neodroid_environment"></param>
    public void UnRegister(droid.Runtime.Interfaces.IEnvironment neodroid_environment) {
      this.UnRegister(environment : neodroid_environment, identifier : neodroid_environment.Identifier);
    }

    #endregion

    #region MessageServerCallbacks

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    void OnReceiveCallback(droid.Runtime.Messaging.Messages.Reaction[] reactions) {
      lock (this._send_lock) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message :
                                $"Received: {System.Linq.Enumerable.Aggregate(source : System.Linq.Enumerable.Select(source : reactions, r => r.ToString()), (current, next) => $"{current}, {next}")}");
        }
        #endif

        this.SetReactionsFromExternalSource(reactions : reactions);

        this.OnReceiveEvent?.Invoke();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="reactions"></param>
    protected void SetReactionsFromExternalSource(droid.Runtime.Messaging.Messages.Reaction[] reactions) {
      lock (this._send_lock) {
        if (reactions != null) {
          if (this.AwaitingReply || !this.HasStepped) {
            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log(message :
                                    $"Got new reaction while not having stepped({!this.HasStepped}) or replied({this.AwaitingReply})");
            }
            #endif
          }

          this.CurrentReactions = reactions;

          this.Configuration.StepExecutionPhase = this.CurrentReactions[0].Parameters.PhaseEnum;
          this.AwaitingReply = true;
          this.HasStepped = false;
        } else {
          UnityEngine.Debug.LogWarning("Reaction was null");
        }
      }
    }

    /// <summary>
    /// </summary>
    void OnDisconnectCallback() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("Client disconnected.");
      }
      #endif
    }

    /// <summary>
    /// </summary>
    /// <param name="error"></param>
    void OnDebugCallback(string error) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "DebugCallback: " + error);
      }
      #endif
    }

    /// <summary>
    /// </summary>
    void OnListeningCallback() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("Client connected");
      }
      #endif

      this._Message_Client.StartReceiving(cmd_callback : this.OnReceiveCallback,
                                          disconnect_callback : this.OnDisconnectCallback,
                                          debug_callback : this.OnDebugCallback);
    }

    #endregion

    #region Deconstruction

    /// <summary>
    /// </summary>
    void OnApplicationQuit() { this._Message_Client.CleanUp(); }

    /// <summary>
    /// </summary>
    void OnDestroy() { this._Message_Client.Destroy(); }

    #endregion
  }
}