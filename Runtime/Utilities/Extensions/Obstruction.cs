namespace droid.Runtime.Utilities.Extensions {
  /// <inheritdoc cref="droid.Runtime.Utilities.Orientation.IMotionTracker" />
  /// <summary>
  /// </summary>
  public class Obstruction : UnityEngine.MonoBehaviour,
                             droid.Runtime.Utilities.Orientation.IMotionTracker {
    UnityEngine.Vector3 _last_recorded_move;
    UnityEngine.Quaternion _last_recorded_rotation;
    UnityEngine.Vector3 _previous_position;
    UnityEngine.Quaternion _previous_rotation;

    void Start() {
      this.UpdatePreviousTransform();
      this.UpdateLastRecordedTransform();
    }

    void Update() { this.UpdatePreviousTransform(); }

    public bool IsInMotion() {
      var transform1 = this.transform;
      return transform1.position != this._previous_position
             || transform1.rotation != this._previous_rotation;
    }

    public bool IsInMotion(float sensitivity) {
      var distance_moved =
          UnityEngine.Vector3.Distance(a : this.transform.position, b : this._last_recorded_move);
      var angle_rotated =
          UnityEngine.Quaternion.Angle(a : this.transform.rotation, b : this._last_recorded_rotation);
      if (distance_moved > sensitivity || angle_rotated > sensitivity) {
        this.UpdateLastRecordedTransform();
        return true;
      }

      return false;
    }

    void UpdatePreviousTransform() {
      var transform1 = this.transform;
      this._previous_position = transform1.position;
      this._previous_rotation = transform1.rotation;
    }

    void UpdateLastRecordedTransform() {
      var transform1 = this.transform;
      this._last_recorded_move = transform1.position;
      this._last_recorded_rotation = transform1.rotation;
    }
  }
}