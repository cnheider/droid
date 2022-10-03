namespace droid.Runtime.Prototyping.Sensors.Spatial.Rigidbody {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class VelocitySensor : Sensor,
                                droid.Runtime.Interfaces.IHasTriple {
    [UnityEngine.SerializeField] UnityEngine.Rigidbody _rigidbody;
    [UnityEngine.SerializeField] UnityEngine.Vector3 _velocity;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _velocity_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
        yield return this.ObservationValue.z;
      }
    }

    void OnDrawGizmosSelected() {
      var rb_pos = this._rigidbody.position;
      UnityEngine.Debug.DrawLine(start : rb_pos, end : rb_pos + this._rigidbody.velocity);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 ObservationValue {
      get { return this._velocity; }
      set { this._velocity = this._velocity_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 TripleSpace { get { return this._velocity_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() { this.ObservationValue = this._rigidbody.velocity; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }
  }
}