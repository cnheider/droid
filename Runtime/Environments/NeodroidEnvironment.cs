namespace droid.Runtime.Environments {
  /// <inheritdoc cref="GameObjects.PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class NeodroidEnvironment : droid.Runtime.GameObjects.PrototypingGameObject,
                                              droid.Runtime.Interfaces.IEnvironment {
    #if UNITY_EDITOR
    const int _script_execution_order = -20;
    #endif
    /// <summary>
    /// </summary>
    protected UnityEngine.WaitForFixedUpdate _Wait_For_Fixed_Update = new UnityEngine.WaitForFixedUpdate();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override string PrototypingTypeName { get; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract void PostStep();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract droid.Runtime.Messaging.Messages.Reaction SampleReaction();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract void Step(droid.Runtime.Messaging.Messages.Reaction reaction);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract void Reset();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    /// <returns></returns>
    public abstract void Configure(droid.Runtime.Messaging.Messages.Reaction reaction);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract droid.Runtime.Messaging.Messages.EnvironmentSnapshot Snapshot();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      base.Setup();

      if (this.SimulationManager == null) {
        this.SimulationManager = FindObjectOfType<droid.Runtime.Managers.AbstractNeodroidManager>();
      }

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

      this.StartCoroutine(routine : this.RemotePostSetupIe());
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void IdentifierString(
        droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : this.Identifier);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void TerminatedBoolean(
        droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      if (this.Terminated) {
        recipient.PollData(true);
      }

      recipient.PollData(false);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    System.Collections.IEnumerator RemotePostSetupIe() {
      yield return this._Wait_For_Fixed_Update;
      this.RemotePostSetup();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      if (this.SimulationManager != null) {
        this.SimulationManager =
            droid.Runtime.Utilities.NeodroidRegistrationUtilities
                 .RegisterComponent(r : (droid.Runtime.Managers.NeodroidManager)this.SimulationManager,
                                    c : this);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.SimulationManager?.UnRegister(obj : this); }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract void ObservationsString(
        droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient);

    #region Public Methods

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void FrameString(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data : $"{this.StepI}");
    }

    #endregion

    #region Properties

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public int StepI { get; protected internal set; }

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected float LastResetTime { get; set; }

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected bool Terminable { get; set; } = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public bool Terminated { get; set; } = false;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public droid.Runtime.Messaging.Messages.Reaction LastReaction { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public bool IsResetting { get; set; }

    /// <summary>
    /// </summary>

    [field : UnityEngine.SerializeField]
    public bool ProvideFullDescription { get; set; } = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public string LastTerminationReason { get; set; } = "None";

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected bool ShouldConfigure { get; set; }

    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("Environment", order = 100)]
    [field : UnityEngine.SerializeField]
    protected droid.Runtime.Interfaces.IManager SimulationManager { get; set; }

    #endregion
  }
}