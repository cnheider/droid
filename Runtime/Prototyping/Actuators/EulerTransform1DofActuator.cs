#if UNITY_EDITOR

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "EulerTransformActuator1Dof"
                                           + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransform1DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected droid.Runtime.Enums.AxisEnum _axisEnum_of_motion;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected string _Layer_Mask = "Obstructions";

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected bool _No_Collisions = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Space _Relative_To = UnityEngine.Space.Self;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return base.PrototypingTypeName + this._axisEnum_of_motion; }
    }

    /// <summary>
    /// </summary>
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
            //Handles.DrawSolidArc
            //Handles.DrawSolidDisc

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
          case droid.Runtime.Enums.AxisEnum.Dir_x_:
            UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                            normal : this.transform.forward,
                                            from : -this.transform.right,
                                            180,
                                            2);
            break;
          case droid.Runtime.Enums.AxisEnum.Dir_y_:
            UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                            normal : this.transform.up,
                                            from : -this.transform.right,
                                            180,
                                            2);
            break;
          case droid.Runtime.Enums.AxisEnum.Dir_z_:
            UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                            normal : this.transform.right,
                                            from : -this.transform.forward,
                                            180,
                                            2);
            break;
          default:
            UnityEngine.Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }

    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      var layer_mask = 1 << UnityEngine.LayerMask.NameToLayer(layerName : this._Layer_Mask);
      var vec = UnityEngine.Vector3.zero;
      switch (this._axisEnum_of_motion) {
        case droid.Runtime.Enums.AxisEnum.X_: // Translational
          vec = UnityEngine.Vector3.right * motion.Strength;
          break;
        case droid.Runtime.Enums.AxisEnum.Y_: // Translational
          vec = -UnityEngine.Vector3.up * motion.Strength;
          break;
        case droid.Runtime.Enums.AxisEnum.Z_: // Translational
          vec = -UnityEngine.Vector3.forward * motion.Strength;
          break;
        case droid.Runtime.Enums.AxisEnum.Rot_x_: // Rotational
          this.transform.Rotate(axis : UnityEngine.Vector3.right,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case droid.Runtime.Enums.AxisEnum.Rot_y_: // Rotational
          this.transform.Rotate(axis : UnityEngine.Vector3.up,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case droid.Runtime.Enums.AxisEnum.Rot_z_: // Rotational
          this.transform.Rotate(axis : UnityEngine.Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case droid.Runtime.Enums.AxisEnum.Dir_x_:
          this.transform.Rotate(axis : UnityEngine.Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case droid.Runtime.Enums.AxisEnum.Dir_y_:
          this.transform.Rotate(axis : UnityEngine.Vector3.up,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case droid.Runtime.Enums.AxisEnum.Dir_z_:
          this.transform.Rotate(axis : UnityEngine.Vector3.right,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        default: throw new System.ArgumentOutOfRangeException();
      }

      if (this._No_Collisions) {
        if (!UnityEngine.Physics.Raycast(origin : this.transform.position,
                                         direction : vec,
                                         maxDistance : UnityEngine.Mathf.Abs(f : motion.Strength),
                                         layerMask : layer_mask)) {
          this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
        }
      } else {
        this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
      }
    }
  }
}