namespace droid.Runtime.Prototyping.Sensors.Spatial.BoundingBox {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Experimental/ScreenSpaceBoundingBox"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  //[ExecuteAlways]
  [UnityEngine.RequireComponent(requiredComponent :
                                 typeof(droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox))]
  public class ScreenSpaceBoundingBoxSensor : Sensor,
                                              droid.Runtime.Interfaces.IHasString {
    [UnityEngine.SerializeField] UnityEngine.Camera _camera = null;
    [UnityEngine.SerializeField] UnityEngine.Rect _out_rect = new UnityEngine.Rect();

    [UnityEngine.SerializeField] bool NormaliseObservation = true;
    droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox _neodroid_bounding_box = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable { get { return null; } }

    /// <summary>
    /// </summary>
    public string ObservationValue { get; set; }

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
      if (this._camera) {
        var rect = this._neodroid_bounding_box.ScreenSpaceBoundingRect(a_camera : this._camera);

        if (this.NormaliseObservation) {
          float w;
          float h;
          var target = this._camera.targetTexture;

          if (target) {
            w = target.width;
            h = target.height;
          } else {
            var r = this._camera.pixelRect;
            w = r.width;
            h = r.height;
          }

          this._out_rect =
              droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities.Normalise(rect : rect,
                width : w,
                height : h);
        } else {
          this._out_rect = rect;
        }
      }

      var str_rep =
          $"{{\"x\":{this._out_rect.x},\n\"y\":{this._out_rect.y},\n\"w\":{this._out_rect.width},\n\"h\":{this._out_rect.height}}}";

      this.ObservationValue = str_rep;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.ObservationValue; }
  }
}