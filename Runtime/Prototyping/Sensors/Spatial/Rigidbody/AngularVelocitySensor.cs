namespace droid.Runtime.Prototyping.Sensors.Spatial.Rigidbody {
  public class AngularVelocitySensor : Sensor,
                                       droid.Runtime.Interfaces.IHasTriple {
    [UnityEngine.SerializeField] UnityEngine.Vector3 _angular_velocity;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _angular_velocity_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

    [UnityEngine.SerializeField] UnityEngine.Rigidbody _rigidbody;

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
        yield return this.ObservationValue.z;
      }
    }

    void OnDrawGizmosSelected() {
      var rb_pos = this._rigidbody.position;
      UnityEngine.Debug.DrawLine(start : rb_pos, end : rb_pos + this._rigidbody.angularVelocity);
    }

    public UnityEngine.Vector3 ObservationValue {
      get { return this._angular_velocity; }
      set { this._angular_velocity = value; }
    }

    public droid.Runtime.Structs.Space.Space3 TripleSpace { get { return this._angular_velocity_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() { this.ObservationValue = this._rigidbody.angularVelocity; }
  }
}