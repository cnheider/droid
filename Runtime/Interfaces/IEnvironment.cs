namespace droid.Runtime.Interfaces {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public interface IEnvironment : IRegisterable {
    /// <summary>
    /// </summary>
    droid.Runtime.Messaging.Messages.Reaction LastReaction { get; }

    /// <summary>
    /// </summary>
    int StepI { get; }

    /// <summary>
    /// </summary>
    bool Terminated { get; }

    /// <summary>
    /// </summary>
    string LastTerminationReason { get; }

    /// <summary>
    /// </summary>
    bool IsResetting { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    droid.Runtime.Messaging.Messages.EnvironmentSnapshot Snapshot();

    /// <summary>
    /// </summary>
    /// <param name="reaction"></param>
    void Step(droid.Runtime.Messaging.Messages.Reaction reaction);

    /// <summary>
    /// </summary>
    /// <returns></returns>
    droid.Runtime.Messaging.Messages.Reaction SampleReaction();

    /// <summary>
    /// </summary>
    void PostStep();

    /// <summary>
    /// </summary>
    void Configure(droid.Runtime.Messaging.Messages.Reaction reaction);

    /// <summary>
    /// </summary>
    void Reset();
  }
}