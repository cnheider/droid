namespace droid.Runtime.Prototyping.Actuators.WheelColliderActuator {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "WheelCollider/Torque"
                                           + ActuatorComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.WheelCollider))]
  public class TorqueActuator : Actuator {
    [UnityEngine.SerializeField] UnityEngine.WheelCollider _wheel_collider;

    public override string[] InnerMotionNames { get { return new[] {"motorTorque"}; } }

    void FixedUpdate() { ApplyLocalPositionToVisuals(col : this._wheel_collider); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._wheel_collider = this.GetComponent<UnityEngine.WheelCollider>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      this._wheel_collider.motorTorque = motion.Strength;
    }

    /// <summary>
    /// </summary>
    static void ApplyLocalPositionToVisuals(UnityEngine.WheelCollider col) {
      if (col.transform.childCount == 0) {
        return;
      }

      var visual_wheel = col.transform.GetChild(0);

      col.GetWorldPose(pos : out var position, quat : out var rotation);

      var transform1 = visual_wheel.transform;
      transform1.position = position;
      transform1.rotation = rotation;
    }
  }
}