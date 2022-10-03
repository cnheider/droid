#if UNITY_EDITOR

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "Rigidbody1DofActuator"
                                           + ActuatorComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class Rigidbody1DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("General", order = 101)]
    [UnityEngine.SerializeField]
    protected droid.Runtime.Enums.AxisEnum _axisEnum_of_motion;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.ForceMode _ForceMode = UnityEngine.ForceMode.Force;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Space _Relative_To = UnityEngine.Space.Self;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Rigidbody _Rigidbody;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return base.PrototypingTypeName + this._axisEnum_of_motion; }
    }

    public override string[] InnerMotionNames { get { return new[] {this._axisEnum_of_motion.ToString()}; } }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._axisEnum_of_motion) {
          case droid.Runtime.Enums.AxisEnum.X_:
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.right * 2,
                                       color : UnityEngine.Color.green);
            break;
          case droid.Runtime.Enums.AxisEnum.Y_:
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.up * 2,
                                       color : UnityEngine.Color.green);
            break;
          case droid.Runtime.Enums.AxisEnum.Z_:
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.forward * 2,
                                       color : UnityEngine.Color.green);
            break;
          case droid.Runtime.Enums.AxisEnum.Rot_x_:
            UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                            normal : this.transform.right,
                                            from : -this.transform.forward,
                                            180,
                                            2);
            break;
          case droid.Runtime.Enums.AxisEnum.Rot_y_:
            UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                            normal : this.transform.up,
                                            from : -this.transform.right,
                                            180,
                                            2);
            break;
          case droid.Runtime.Enums.AxisEnum.Rot_z_:
            UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                            normal : this.transform.forward,
                                            from : -this.transform.right,
                                            180,
                                            2);
            break;
          case droid.Runtime.Enums.AxisEnum.Dir_x_: break;
          case droid.Runtime.Enums.AxisEnum.Dir_y_: break;
          case droid.Runtime.Enums.AxisEnum.Dir_z_: break;
          default: //TODO add the Direction cases
            UnityEngine.Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._Rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      switch (this._axisEnum_of_motion) {
        case droid.Runtime.Enums.AxisEnum.X_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.right * motion.Strength,
                                     mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.right * motion.Strength,
                                             mode : this._ForceMode);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Y_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.up * motion.Strength,
                                     mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.up * motion.Strength,
                                             mode : this._ForceMode);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Z_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.forward * motion.Strength,
                                     mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.forward * motion.Strength,
                                             mode : this._ForceMode);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_x_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.right * motion.Strength,
                                      mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.right * motion.Strength,
                                              mode : this._ForceMode);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_y_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.up * motion.Strength,
                                      mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.up * motion.Strength,
                                              mode : this._ForceMode);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_z_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.forward * motion.Strength,
                                      mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.forward * motion.Strength,
                                              mode : this._ForceMode);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Dir_x_: break;
        case droid.Runtime.Enums.AxisEnum.Dir_y_: break;
        case droid.Runtime.Enums.AxisEnum.Dir_z_: break;
        default:
          throw new System.ArgumentOutOfRangeException();
      }
    }
  }
}