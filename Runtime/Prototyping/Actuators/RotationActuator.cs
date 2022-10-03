namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "Rotation"
                                           + ActuatorComponentMenuPath._Postfix)]
  public class RotationActuator : Actuator {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.Space _Relative_To = UnityEngine.Space.Self;

    string _rot_w = "RotW";

    string _rot_x = "RotX";
    string _rot_y = "RotY";
    string _rot_z = "RotZ";

    public override string[] InnerMotionNames {
      get {
        return new[] {
                         this._rot_x,
                         this._rot_y,
                         this._rot_z,
                         this._rot_w
                     };
      }
    }
    //new Space1 MotionSpace = Space1.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._rot_x = this.Identifier + "RotX";
      this._rot_y = this.Identifier + "RotY";
      this._rot_z = this.Identifier + "RotZ";
      this._rot_w = this.Identifier + "RotW";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
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
      this.Parent =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : (droid.Runtime.
                Interfaces.IHasRegister<droid.Runtime.Interfaces.IActuator>)this.Parent,
            c : (Actuator)this,
            identifier : this._rot_w);
    }

    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(t : this, obj : this._rot_x);
      this.Parent?.UnRegister(t : this, obj : this._rot_y);
      this.Parent?.UnRegister(t : this, obj : this._rot_z);
      this.Parent?.UnRegister(t : this, obj : this._rot_w);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      var transform_rotation = this.transform.rotation;
      if (motion.ActuatorName == this._rot_x) {
        transform_rotation.x = motion.Strength;
      } else if (motion.ActuatorName == this._rot_y) {
        transform_rotation.y = motion.Strength;
      } else if (motion.ActuatorName == this._rot_z) {
        transform_rotation.z = motion.Strength;
      } else if (motion.ActuatorName == this._rot_w) {
        transform_rotation.z = motion.Strength;
      }

      this.transform.rotation = transform_rotation;
    }
  }
}