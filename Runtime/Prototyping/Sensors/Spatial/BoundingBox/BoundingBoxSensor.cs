namespace droid.Runtime.Prototyping.Sensors.Spatial.BoundingBox {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Experimental/BoundingBox"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent :
                                 typeof(droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox))]
  public class BoundingBoxSensor : Sensor,
                                   droid.Runtime.Interfaces.IHasString {
    [UnityEngine.SerializeField] string _observationValue;
    droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox _neodroid_bounding_box;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable { get { return null; } }

    /// <summary>
    /// </summary>
    public string ObservationValue {
      get { return this._observationValue; }
      set { this._observationValue = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._neodroid_bounding_box =
          this.GetComponent<droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._neodroid_bounding_box.BoundingBoxCoordinatesWorldSpaceAsJson;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.ObservationValue; }
  }
}