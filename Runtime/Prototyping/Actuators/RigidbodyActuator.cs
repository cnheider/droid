#if UNITY_EDITOR

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "Rigidbody"
                                           + ActuatorComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class RigidbodyActuator : Actuator {
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

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    public override string[] InnerMotionNames {
      get {
        return new[] {
                         this._x,
                         this._y,
                         this._z,
                         this._rot_x,
                         this._rot_y,
                         this._rot_z
                     };
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;

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
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._Rigidbody = this.GetComponent<UnityEngine.Rigidbody>();

      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      this._rot_x = this.Identifier + "RotX_";
      this._rot_y = this.Identifier + "RotY_";
      this._rot_z = this.Identifier + "RotZ_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      //this.ParentActor = NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.ParentActor, (Actuator)this);

      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._x);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._y);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._z);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._rot_x);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._rot_y);
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(t : this, obj : this._x);
      this.Parent?.UnRegister(t : this, obj : this._y);
      this.Parent?.UnRegister(t : this, obj : this._z);

      this.Parent?.UnRegister(t : this, obj : this._rot_x);
      this.Parent?.UnRegister(t : this, obj : this._rot_y);
      this.Parent?.UnRegister(t : this, obj : this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      if (this._Relative_To == UnityEngine.Space.World) {
        if (motion.ActuatorName == this._x) {
          this._Rigidbody.AddForce(force : UnityEngine.Vector3.right * motion.Strength,
                                   mode : this._ForceMode);
        } else if (motion.ActuatorName == this._y) {
          this._Rigidbody.AddForce(force : UnityEngine.Vector3.up * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._z) {
          this._Rigidbody.AddForce(force : UnityEngine.Vector3.forward * motion.Strength,
                                   mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_x) {
          this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.right * motion.Strength,
                                    mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_y) {
          this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.up * motion.Strength,
                                    mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_z) {
          this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.forward * motion.Strength,
                                    mode : this._ForceMode);
        }
      } else if (this._Relative_To == UnityEngine.Space.Self) {
        if (motion.ActuatorName == this._x) {
          this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.right * motion.Strength,
                                           mode : this._ForceMode);
        } else if (motion.ActuatorName == this._y) {
          this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.up * motion.Strength,
                                           mode : this._ForceMode);
        } else if (motion.ActuatorName == this._z) {
          this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.forward * motion.Strength,
                                           mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_x) {
          this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.right * motion.Strength,
                                            mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_y) {
          this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.up * motion.Strength,
                                            mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_z) {
          this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.forward * motion.Strength,
                                            mode : this._ForceMode);
        }
      } else {
        UnityEngine.Debug.LogWarning(message : $"Not applying force in space {this._Relative_To}");
      }
    }
  }
}