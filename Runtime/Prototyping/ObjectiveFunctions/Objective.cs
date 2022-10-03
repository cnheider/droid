namespace droid.Runtime.Prototyping.ObjectiveFunctions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public abstract class ObjectiveFunction : droid.Runtime.GameObjects.PrototypingGameObject,
                                            //IHasRegister<Term>,
                                            droid.Runtime.Interfaces.IObjectiveFunction {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public virtual float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Signal for this step: {signal}");
      }
      #endif

      this.LastSignal = signal;

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      this.LastSignal = 0;
      this.InternalReset();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public sealed override void Setup() {
      this.PreSetup();

      if (this.ParentEnvironment == null) {
        this.ParentEnvironment = droid.Runtime.Utilities.NeodroidSceneUtilities
                                      .RecursiveFirstSelfSiblingParentGetComponent<
                                          droid.Runtime.Environments.Prototyping.
                                          AbstractPrototypingEnvironment>(child : this.transform);
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void SignalString(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data :
                         $"{this.LastSignal.ToString(provider : System.Globalization.CultureInfo.InvariantCulture)}");
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public void EpisodeLengthString(
        droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData("");
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract float InternalEvaluate();

    /// <summary>
    /// </summary>
    public abstract void InternalReset();

    #region Fields

    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("References", order = 100)]
    [field : UnityEngine.SerializeField]
    public droid.Runtime.Interfaces.IAbstractPrototypingEnvironment ParentEnvironment { get; set; } = null;

    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("General", order = 101)]
    [field : UnityEngine.SerializeField]
    public float LastSignal { get; protected set; } = 0f;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public droid.Runtime.Structs.Space.Space1 SignalSpace { get; set; }

    #endregion
  }
}