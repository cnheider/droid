namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated.Segmentation {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "SegmentationCamera"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera),
                                requiredComponent2 :
                                typeof(droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Segmenter))]
  public class SegmentationEncodedCameraSensor : StringAugmentedEncodedCameraSensor {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Segmenter _segmenter = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      base.UpdateObservation();

      UnityEngine.Debug.LogWarning(message : UnityEngine.JsonUtility
                                                        .ToJson(obj : this._segmenter.ColorsDict));
      this.serialised_string = this._segmenter != null
                                   ? System.Linq.Enumerable.Aggregate(source :
                                                                      System.Linq.Enumerable
                                                                          .Select(source : this._segmenter
                                                                                .ColorsDict,
                                                                            c =>
                                                                                $"{c.Key}: {c.Value.ToString()}"),
                                                                      "",
                                                                      (current, next) => $"{current}, {next}")
                                   : "Nothing";
    }
  }
}