namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  public class TargetRigidbodySensor : Sensor,
                                       droid.Runtime.Interfaces.IHasDouble {
    [UnityEngine.SerializeField] droid.Runtime.Prototyping.Actuators.TargetRigidbodyActuator _actuator = null;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space2 _observation_space2_d = droid.Runtime.Structs.Space.Space2.ZeroOne;

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector2 ObservationValue {
      get {
        return new UnityEngine.Vector2(x : this._actuator.MovementSpeed, y : this._actuator.RotationSpeed);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space2 DoubleSpace { get { return this._observation_space2_d; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      base.PreSetup();
      if (!this._actuator) {
        this._actuator = this.GetComponent<droid.Runtime.Prototyping.Actuators.TargetRigidbodyActuator>();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() { }
  }
}