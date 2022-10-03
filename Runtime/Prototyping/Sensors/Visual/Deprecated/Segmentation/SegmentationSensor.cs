namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated.Segmentation {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Segmentation"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent :
                                 typeof(droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Segmenter))]
  public class SegmentationSensor : droid.Runtime.Prototyping.Sensors.Strings.StringSensor {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Segmenter _segmenter = null;

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable { get { return null; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._segmenter != null
                                  ? System.Linq.Enumerable.Aggregate(source :
                                                                     System.Linq.Enumerable
                                                                         .Select(source : this._segmenter
                                                                               .ColorsDict,
                                                                           c =>
                                                                               $"{c.Key}: {c.Value.ToString()}"),
                                                                     "",
                                                                     (current, next) =>
                                                                         current != ""
                                                                             ? $"{current}, {next}"
                                                                             : $"{next}")
                                  : "Nothing";
      //TODO:ADD this Type(COLOR) and ColorDict as serialisation option instead of a string
/*      if (this._segmenter != null) {
        this.ObservationValue += $", Outline: {this._segmenter.OutlineColor.ToString()}";
      }
      */
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return this.ObservationValue; }
  }
}