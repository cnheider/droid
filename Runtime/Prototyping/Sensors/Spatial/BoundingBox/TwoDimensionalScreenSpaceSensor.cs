namespace droid.Runtime.Prototyping.Sensors.Spatial.BoundingBox {
  public class TwoDimensionalScreenSpaceSensor : Sensor,
                                                 droid.Runtime.Interfaces.IHasDouble {
    [UnityEngine.SerializeField] UnityEngine.Vector2 _observation_value = UnityEngine.Vector2.zero;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space2 _observation_space2_d = droid.Runtime.Structs.Space.Space2.ZeroOne;

    [UnityEngine.SerializeField] UnityEngine.Camera _reference_camera = null;

    [UnityEngine.SerializeField] bool _use_viewport = true; // Already normalised between 0 and 1

    // Update is called once per frame
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this._observation_value.x;
        yield return this.ObservationValue.y;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector2 ObservationValue { get { return this._observation_value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space2 DoubleSpace { get { return this._observation_space2_d; } }

    public override void UpdateObservation() {
      if (this._reference_camera) {
        UnityEngine.Vector3 point;
        if (this._use_viewport) {
          point = this._reference_camera.WorldToViewportPoint(position : this.transform.position);
        } else {
          point = this._reference_camera.WorldToScreenPoint(position : this.transform.position);
        }

        this._observation_value = point;
      }
    }
  }
}