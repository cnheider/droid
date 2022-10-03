namespace droid.Runtime.Prototyping.Actuators.Particles {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                                           + "Particles/Rocket"
                                           + ActuatorComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.ParticleSystem))]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class RocketActuator : Rigidbody1DofActuator {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _fired_this_step;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected UnityEngine.ParticleSystem _Particle_System;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return base.PrototypingTypeName + this._axisEnum_of_motion; }
    }

    /// <summary>
    /// </summary>
    void LateUpdate() {
      if (this._fired_this_step) {
        if (!this._Particle_System.isPlaying) {
          this._Particle_System.Play(true);
        }
      } else {
        this._Particle_System.Stop(true);
      }

      this._fired_this_step = false;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._Rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this._Particle_System = this.GetComponent<UnityEngine.ParticleSystem>();
      var valid_input = this.MotionSpace;
      valid_input.Min = 0;
      this.MotionSpace = valid_input;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(droid.Runtime.Interfaces.IMotion motion) {
      if (motion.Strength < this.MotionSpace.Min || motion.Strength > this.MotionSpace.Max) {
        UnityEngine.Debug.Log(message :
                              $"It does not accept input {motion.Strength}, outside allowed range {this.MotionSpace.Min} to {this.MotionSpace.Max}");
        return; // Do nothing
      }

      switch (this._axisEnum_of_motion) {
        case droid.Runtime.Enums.AxisEnum.X_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.right * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.right * motion.Strength);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Y_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.up * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.up * motion.Strength);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Z_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddForce(force : UnityEngine.Vector3.forward * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(force : UnityEngine.Vector3.forward * motion.Strength);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_x_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.right * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.right * motion.Strength);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_y_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.up * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.up * motion.Strength);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Rot_z_:
          if (this._Relative_To == UnityEngine.Space.World) {
            this._Rigidbody.AddTorque(torque : UnityEngine.Vector3.forward * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : UnityEngine.Vector3.forward * motion.Strength);
          }

          break;
        case droid.Runtime.Enums.AxisEnum.Dir_x_: break;
        case droid.Runtime.Enums.AxisEnum.Dir_y_: break;
        case droid.Runtime.Enums.AxisEnum.Dir_z_: break;
        default: throw new System.ArgumentOutOfRangeException();
      }

      this._fired_this_step = true;
    }
  }
}