namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Scale"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class ScaleSensor : Sensor,
                             droid.Runtime.Interfaces.IHasTriple {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _scale;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space3 _scale_space = droid.Runtime.Structs.Space.Space3.ZeroOne;

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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 ObservationValue { //TODO: IMPLEMENT LOCAL SPACE
      get { return this._scale; }
      set { this._scale = this._scale_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space3 TripleSpace { get { return this._scale_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() { }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() { this.ObservationValue = this.transform.localScale; }
  }
}