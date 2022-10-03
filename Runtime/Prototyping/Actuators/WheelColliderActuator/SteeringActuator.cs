namespace droid.Runtime.Prototyping.Actuators.WheelColliderActuator {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "WheelCollider/Steering"
                                           + ActuatorComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.WheelCollider))]
  public class SteeringActuator : Actuator {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.WheelCollider _wheel_collider;

    public override string[] InnerMotionNames { get { return new[] {"steerAngle"}; } }

    /// <summary>
    /// </summary>
    void FixedUpdate() { ApplyLocalPositionToVisuals(col : this._wheel_collider); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._wheel_collider = this.GetComponent<UnityEngine.WheelCollider>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      this._wheel_collider.steerAngle = motion.Strength;
    }

    /// <summary>
    /// </summary>
    /// <param name="col"></param>
    static void ApplyLocalPositionToVisuals(UnityEngine.WheelCollider col) {
      if (col.transform.childCount == 0) {
        return;
      }

      var visual_wheel = col.transform.GetChild(0);

      col.GetWorldPose(pos : out var position, quat : out var rotation);

      visual_wheel.transform.position = position;
      visual_wheel.transform.rotation = rotation;
    }
  }
}