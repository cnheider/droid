namespace droid.Runtime.GameObjects.ChildSensors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class ChildCollider3DSensor : ChildColliderSensor<UnityEngine.Collider, UnityEngine.Collision> {
    void OnCollisionEnter(UnityEngine.Collision collision) {
      this._on_collision_enter_delegate?.Invoke(child_sensor_game_object : this.gameObject,
                                                collision : collision);
    }

    void OnCollisionExit(UnityEngine.Collision collision) {
      this._on_collision_exit_delegate?.Invoke(child_sensor_game_object : this.gameObject,
                                               collision : collision);
    }

    void OnCollisionStay(UnityEngine.Collision collision) {
      this._on_collision_stay_delegate?.Invoke(child_sensor_game_object : this.gameObject,
                                               collision : collision);
    }

    void OnTriggerEnter(UnityEngine.Collider other) {
      this._on_trigger_enter_delegate?.Invoke(child_sensor_game_object : this.gameObject, collider : other);
    }

    void OnTriggerExit(UnityEngine.Collider other) {
      this._on_trigger_exit_delegate?.Invoke(child_sensor_game_object : this.gameObject, collider : other);
    }

    void OnTriggerStay(UnityEngine.Collider other) {
      this._on_trigger_stay_delegate?.Invoke(child_sensor_game_object : this.gameObject, collider : other);
    }
  }
}