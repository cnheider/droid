namespace droid.Runtime.Prototyping.Sensors.Spatial.Rigidbody {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  public class SingleVelocitySensor : Sensor,
                                      droid.Runtime.Interfaces.IHasSingle {
    [UnityEngine.SerializeField] UnityEngine.Rigidbody _rigidbody;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _single_space = droid.Runtime.Structs.Space.Space1.MinusOneOne * 10;

    [UnityEngine.SerializeField] float _observation_value;

    [UnityEngine.SerializeField] VelocityAxisEnum _velocity_axis = VelocityAxisEnum.X_vel_;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return base.PrototypingTypeName + this._velocity_axis; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this._observation_value; }
    }

    void OnDrawGizmosSelected() {
      var rb_pos = this._rigidbody.position;
      switch (this._velocity_axis) {
        case VelocityAxisEnum.X_vel_:
          UnityEngine.Debug.DrawLine(start : rb_pos,
                                     end : rb_pos + UnityEngine.Vector3.right * this._rigidbody.velocity.x);
          break;
        case VelocityAxisEnum.Y_vel_:
          UnityEngine.Debug.DrawLine(start : rb_pos,
                                     end : rb_pos + UnityEngine.Vector3.up * this._rigidbody.velocity.y);
          break;
        case VelocityAxisEnum.Z_vel_:
          UnityEngine.Debug.DrawLine(start : rb_pos,
                                     end : rb_pos + UnityEngine.Vector3.forward * this._rigidbody.velocity.z);
          break;
        case VelocityAxisEnum.X_ang_vel_:
          UnityEngine.Debug.DrawLine(start : rb_pos,
                                     end : rb_pos
                                           + UnityEngine.Vector3.up * this._rigidbody.angularVelocity.x);
          break;
        case VelocityAxisEnum.Y_ang_vel_:
          UnityEngine.Debug.DrawLine(start : rb_pos,
                                     end : rb_pos
                                           + UnityEngine.Vector3.right * this._rigidbody.angularVelocity.y);
          break;
        case VelocityAxisEnum.Z_ang_vel_:
          UnityEngine.Debug.DrawLine(start : rb_pos,
                                     end : rb_pos
                                           + UnityEngine.Vector3.forward * this._rigidbody.angularVelocity.z);
          break;
        default:
          throw new System.ArgumentOutOfRangeException();
      }
    }

    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._single_space; } }

    float droid.Runtime.Interfaces.IHasSingle.ObservationValue { get { return this._observation_value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      float val;
      switch (this._velocity_axis) {
        case VelocityAxisEnum.X_vel_:
          val = this._rigidbody.velocity.x;
          break;
        case VelocityAxisEnum.Y_vel_:
          val = this._rigidbody.velocity.y;
          break;
        case VelocityAxisEnum.Z_vel_:
          val = this._rigidbody.velocity.z;
          break;
        case VelocityAxisEnum.X_ang_vel_:
          val = this._rigidbody.angularVelocity.x;
          break;
        case VelocityAxisEnum.Y_ang_vel_:
          val = this._rigidbody.angularVelocity.y;
          break;
        case VelocityAxisEnum.Z_ang_vel_:
          val = this._rigidbody.angularVelocity.z;
          break;
        default:
          throw new System.ArgumentOutOfRangeException();
      }

      this._observation_value = this._single_space.Project(v : val);
    }

    /// <summary>
    /// </summary>
    enum VelocityAxisEnum {
      X_vel_,
      Y_vel_,
      Z_vel_,
      X_ang_vel_,
      Y_ang_vel_,
      Z_ang_vel_
    }
  }
}