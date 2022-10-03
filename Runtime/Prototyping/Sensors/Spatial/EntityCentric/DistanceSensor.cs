namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  public class DistanceSensor : Sensor,
                                droid.Runtime.Interfaces.IHasSingle {
    [UnityEngine.SerializeField] UnityEngine.Transform t1 = null;
    [UnityEngine.SerializeField] UnityEngine.Transform t2 = null;
    [UnityEngine.SerializeField] float _observationValue = 0;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space1 _single_space = droid.Runtime.Structs.Space.Space1.MinusOneOne * 4;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this.ObservationValue; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public float ObservationValue {
      get { return this._observationValue; }
      private set { this._observationValue = this._single_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._single_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = UnityEngine.Vector3.Distance(a : this.t1.position, b : this.t2.position);
    }
  }
}