#if UNITY_EDITOR

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "Rigidbody3DofActuator"
                                           + ActuatorComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class Rigidbody3DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected bool _Angular_Actuators;

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

    /// <summary>
    /// </summary>
    string _x;

    /// <summary>
    /// </summary>
    string _y;

    /// <summary>
    /// </summary>
    string _z;

    public override string[] InnerMotionNames { get { return new[] {this._x, this._y, this._z}; } }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        if (this._Angular_Actuators) {
          UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                          normal : this.transform.right,
                                          from : -this.transform.forward,
                                          180,
                                          2);

          UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                          normal : this.transform.up,
                                          from : -this.transform.right,
                                          180,
                                          2);

          UnityEditor.Handles.DrawWireArc(center : this.transform.position,
                                          normal : this.transform.forward,
                                          from : -this.transform.right,
                                          180,
                                          2);
        } else {
          UnityEngine.Debug.DrawLine(start : position,
                                     end : position + UnityEngine.Vector3.right * 2,
                                     color : UnityEngine.Color.green);

          UnityEngine.Debug.DrawLine(start : position,
                                     end : position + UnityEngine.Vector3.forward * 2,
                                     color : UnityEngine.Color.green);

          UnityEngine.Debug.DrawLine(start : position,
                                     end : position + UnityEngine.Vector3.up * 2,
                                     color : UnityEngine.Color.green);
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
    protected override void RegisterComponent() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      if (this._Angular_Actuators) {
        this._x = this.Identifier + "RotX_";
        this._y = this.Identifier + "RotY_";
        this._z = this.Identifier + "RotZ_";
      }

      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : this,
            identifier : this._x);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : this,
            identifier : this._y);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : this,
            identifier : this._z);
    }

    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(t : this, obj : this._x);
      this.Parent?.UnRegister(t : this, obj : this._y);
      this.Parent?.UnRegister(t : this, obj : this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      if (!this._Angular_Actuators) {
        if (motion.ActuatorName == this._x) {
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.right * motion.Strength,
                                     mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.right * motion.Strength,
                                             mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._y) {
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.up * motion.Strength,
                                     mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.up * motion.Strength,
                                             mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._z) {
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.forward * motion.Strength,
                                     mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.forward * motion.Strength,
                                             mode : this._ForceMode);
          }
        }
      } else {
        if (motion.ActuatorName == this._x) {
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.right * motion.Strength,
                                      mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.right * motion.Strength,
                                              mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._y) {
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.up * motion.Strength,
                                      mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.up * motion.Strength,
                                              mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._z) {
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.forward * motion.Strength,
                                      mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.forward * motion.Strength,
                                              mode : this._ForceMode);
          }
        }
      }
    }
  }
}