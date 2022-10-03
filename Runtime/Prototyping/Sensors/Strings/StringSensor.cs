namespace droid.Runtime.Prototyping.Sensors.Strings {
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "String"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  public class StringSensor : Sensor,
                              droid.Runtime.Interfaces.IHasString {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    string _observation_value;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable { get { return null; } }

    /// <summary>
    /// </summary>
    public string ObservationValue {
      get { return this._observation_value; }
      set { this._observation_value = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this._observation_value = this.ParentEnvironment.StepI.ToString();
    }
  }
}