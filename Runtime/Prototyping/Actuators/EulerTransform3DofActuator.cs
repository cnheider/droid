#if UNITY_EDITOR

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "EulerTransform3Dof"
                                           + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransform3DofActuator : Actuator {
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

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected bool _angular_Actuators;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected bool _Use_Mask = true;

    /// <summary>
    ///   XAxisIdentifier
    /// </summary>
    string _x;

    /// <summary>
    ///   YAxisIdentifier
    /// </summary>
    string _y;

    /// <summary>
    ///   ZAxisIdentifier
    /// </summary>
    string _z;

    /// <summary>
    /// </summary>
    public override string[] InnerMotionNames { get { return new[] {this._x, this._y, this._z}; } }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        if (this._angular_Actuators) {
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
                                     end : position + UnityEngine.Vector3.up * 2,
                                     color : UnityEngine.Color.green);
          UnityEngine.Debug.DrawLine(start : position,
                                     end : position + UnityEngine.Vector3.forward * 2,
                                     color : UnityEngine.Color.green);
          UnityEngine.Debug.DrawLine(start : position,
                                     end : position + UnityEngine.Vector3.right * 2,
                                     color : UnityEngine.Color.green);
        }
      }
    }
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      if (!this._angular_Actuators) {
        this._x = this.Identifier + "X_";
        this._y = this.Identifier + "Y_";
        this._z = this.Identifier + "Z_";
      } else {
        this._x = this.Identifier + "RotX_";
        this._y = this.Identifier + "RotY_";
        this._z = this.Identifier + "RotZ_";
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
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

    /// <inheritdoc />
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
      var layer_mask = 1 << UnityEngine.LayerMask.NameToLayer(layerName : this._Layer_Mask);
      if (!this._angular_Actuators) {
        if (motion.ActuatorName == this._x) {
          var vec = UnityEngine.Vector3.right * motion.Strength;
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
        } else if (motion.ActuatorName == this._y) {
          var vec = -UnityEngine.Vector3.up * motion.Strength;
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
        } else if (motion.ActuatorName == this._z) {
          var vec = -UnityEngine.Vector3.forward * motion.Strength;
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
      } else {
        if (motion.ActuatorName == this._x) {
          this.transform.Rotate(axis : UnityEngine.Vector3.right,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
        } else if (motion.ActuatorName == this._y) {
          this.transform.Rotate(axis : UnityEngine.Vector3.up,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
        } else if (motion.ActuatorName == this._z) {
          this.transform.Rotate(axis : UnityEngine.Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
        }
      }
    }
  }
}