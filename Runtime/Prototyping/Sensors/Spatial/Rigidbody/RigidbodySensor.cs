namespace droid.Runtime.Prototyping.Sensors.Spatial.Rigidbody {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Rigidbody"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class RigidbodySensor : Sensor,
                                 droid.Runtime.Interfaces.IHasRigidbody {
    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _angular_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.HeaderAttribute("Observation", order = 100)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _angular_velocity = UnityEngine.Vector3.zero;

    [UnityEngine.SerializeField] bool _differential = false;

    [UnityEngine.SerializeField] float _last_update_time = 0;

    [UnityEngine.HeaderAttribute("Configuration", order = 110)]
    [UnityEngine.SerializeField]
    UnityEngine.Rigidbody _rigidbody = null;

    [UnityEngine.SerializeField] UnityEngine.Vector3 _velocity = UnityEngine.Vector3.zero;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _velocity_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get {
        if (this._differential) {
          return "RigidbodyDifferential";
        }

        return "Rigidbody";
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.Velocity.x;
        yield return this.Velocity.y;
        yield return this.Velocity.z;
        yield return this.AngularVelocity.x;
        yield return this.AngularVelocity.y;
        yield return this.AngularVelocity.z;
      }
    }

    void OnDrawGizmosSelected() {
      var rb_pos = this._rigidbody.position;
      UnityEngine.Debug.DrawLine(start : rb_pos, end : rb_pos + this._rigidbody.angularVelocity);
      UnityEngine.Debug.DrawLine(start : rb_pos,
                                 end : rb_pos + this._rigidbody.velocity,
                                 color : UnityEngine.Color.red);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Velocity {
      get { return this._velocity; }
      set { this._velocity = this._velocity_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      set { this._angular_velocity = this._angular_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 VelocitySpace { get { return this._velocity_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 AngularSpace { get { return this._angular_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      var update_time_difference = UnityEngine.Time.time - this._last_update_time;
      if (this._differential && update_time_difference > 0) {
        var vel_diff = this.Velocity - this._rigidbody.velocity;
        var ang_diff = this.AngularVelocity - this._rigidbody.angularVelocity;

        var vel_magnitude = vel_diff.magnitude;
        if (vel_magnitude > 0) {
          this.Velocity = vel_diff / (update_time_difference + float.Epsilon);
        } else {
          this.Velocity = vel_diff;
        }

        var ang_magnitude = ang_diff.magnitude;
        if (ang_magnitude > 0) {
          this.AngularVelocity = ang_diff / (update_time_difference + float.Epsilon);
        } else {
          this.AngularVelocity = ang_diff;
        }
      } else {
        this.Velocity = this._rigidbody.velocity;
        this.AngularVelocity = this._rigidbody.angularVelocity;
      }

      this._last_update_time = UnityEngine.Time.realtimeSinceStartup;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }
  }
}