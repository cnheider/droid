﻿namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public abstract class SpatialObjective : EpisodicObjective {
    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox Box { get; set; }

    /// <summary>
    ///   // TODO: Look at how to simplify a way to describe which objects should be in this list
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected UnityEngine.Transform[] TerminatingTransforms { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      base.RemotePostSetup();

      if (this.Box == null) {
        if (this.ParentEnvironment.PlayableArea) {
          this.Box = this.ParentEnvironment.PlayableArea;
        } else {
          this.Box =
              this.gameObject.GetComponent<droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox>();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float Evaluate() {
      var signal = 0.0f;

      if (!this.ParentEnvironment.Terminated) {
        signal += this.InternalEvaluate();
      }

      if (this.EpisodeLength > 0 && this.ParentEnvironment?.StepI >= this.EpisodeLength) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"Maximum episode length reached, Length {this.ParentEnvironment.StepI}");
        }
        #endif

        signal = this.FailedSignal;

        this.ParentEnvironment.Terminate("Maximum episode length reached");
      }

      if (this.Box) {
        foreach (var t in this.TerminatingTransforms) {
          if (!this.Box.Bounds.Contains(point : t.position)) {
            signal = this.FailedSignal;

            #if NEODROID_DEBUG
            if (this.Debugging) {
              UnityEngine.Debug.Log(message : $"The transform {t} outside bounds, terminating {this.ParentEnvironment}");
            }
            #endif

            this.ParentEnvironment?.Terminate(reason : $"The transform {t} is not inside {this.Box.gameObject} bounds");
            break;
          }
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : signal);
      }
      #endif

      this.LastSignal = signal;

      this.EpisodeReturn += signal;

      return signal;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public abstract override void InternalReset();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public abstract override float InternalEvaluate();
  }
}