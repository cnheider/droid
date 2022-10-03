﻿namespace droid.Runtime.Prototyping.ObjectiveFunctions {
  /// <inheritdoc cref="ObjectiveFunction" />
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public abstract class EpisodicObjective : ObjectiveFunction,
                                            droid.Runtime.Interfaces.IEpisodicObjectiveFunction {
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public float EpisodeReturn { get; protected set; } = 0;

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected float SolvedSignal { get; set; } = 1.0f;

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected float FailedSignal { get; set; } = -1.0f;

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected float DefaultSignal { get; set; } = -0.001f;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      var signal = 0.0f;
      signal += this.InternalEvaluate();

      if (this.EpisodeLength > 0
          && this.ParentEnvironment.StepI >= this.EpisodeLength
          && !this.ParentEnvironment.Terminated) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message :
                                $"Maximum episode length reached, Length {this.ParentEnvironment.StepI}");
        }
        #endif

        signal = this.FailedSignal;

        this.ParentEnvironment.Terminate("Maximum episode length reached");
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Signal for this step: {signal}");
      }
      #endif

      this.LastSignal = signal;

      this.EpisodeReturn += signal;

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public new void PrototypingReset() {
      this.LastSignal = 0;
      this.EpisodeReturn = 0;
      this.InternalReset();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    public int EpisodeLength { get; set; } = 1000;

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new void
        SignalString(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data :
                         $"{this.LastSignal.ToString(provider : System.Globalization.CultureInfo.InvariantCulture)}, {this.EpisodeReturn}");
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public new void
        EpisodeLengthString(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller recipient) {
      recipient.PollData(data :
                         $"{this.EpisodeLength.ToString(provider : System.Globalization.CultureInfo.InvariantCulture)}");
    }
  }
}