namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Value"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  public abstract class SingleValueSensor : Sensor,
                                            droid.Runtime.Interfaces.IHasSingle {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    float _observation_value;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected droid.Runtime.Structs.Space.Space1 _observation_value_space =
        droid.Runtime.Structs.Space.Space1.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { yield return this.ObservationValue; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1 SingleSpace { get { return this._observation_value_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = this.SingleSpace.Project(v : value); }
    }
  }
}