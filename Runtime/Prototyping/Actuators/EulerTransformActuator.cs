#if UNITY_EDITOR

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "EulerTransform"
                                           + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransformActuator : Actuator {
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
      if (motion.ActuatorName == this._x) {
        this.transform.Translate(translation : UnityEngine.Vector3.right * motion.Strength,
                                 relativeTo : this._Relative_To);
      } else if (motion.ActuatorName == this._y) {
        this.transform.Translate(translation : -UnityEngine.Vector3.up * motion.Strength,
                                 relativeTo : this._Relative_To);
      } else if (motion.ActuatorName == this._z) {
        this.transform.Translate(translation : -UnityEngine.Vector3.forward * motion.Strength,
                                 relativeTo : this._Relative_To);
      } else if (motion.ActuatorName == this._rot_x) {
        this.transform.Rotate(axis : UnityEngine.Vector3.right,
                              angle : motion.Strength,
                              relativeTo : this._Relative_To);
      } else if (motion.ActuatorName == this._rot_y) {
        this.transform.Rotate(axis : UnityEngine.Vector3.up,
                              angle : motion.Strength,
                              relativeTo : this._Relative_To);
      } else if (motion.ActuatorName == this._rot_z) {
        this.transform.Rotate(axis : UnityEngine.Vector3.forward,
                              angle : motion.Strength,
                              relativeTo : this._Relative_To);
      }
    }
  }
}