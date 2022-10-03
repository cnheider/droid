﻿namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class VelocityObjective : SpatialObjective {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Rigidbody _rigidbody = null;

    void OnDrawGizmosSelected() {
      var rb_pos = this._rigidbody.position;
      UnityEngine.Debug.DrawLine(start : rb_pos, end : rb_pos + this._rigidbody.velocity);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void InternalReset() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override float InternalEvaluate() {
      var vel_mag = this._rigidbody.velocity.magnitude;

      this.IsOutsideBound();

      return vel_mag;
    }

    /// <summary>
    /// </summary>
    void IsOutsideBound() {
      if (this.ParentEnvironment.PlayableArea && this._rigidbody) {
        var env_bounds = this.ParentEnvironment.PlayableArea.Bounds;
        var rb_bounds = this._rigidbody.GetComponent<UnityEngine.Collider>().bounds;
        var intersects = env_bounds.Intersects(bounds : rb_bounds);

        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"{this.ParentEnvironment.Identifier} - {env_bounds}");
          UnityEngine.Debug.Log(message : $"{this._rigidbody.name} - {rb_bounds}");
          UnityEngine.Debug.Log(message : $"Is intersecting - {intersects}");
        }
        #endif

        if (!intersects) {
          this.ParentEnvironment.Terminate("Actor is outside playable area");
        }
      }
    }
  }
}