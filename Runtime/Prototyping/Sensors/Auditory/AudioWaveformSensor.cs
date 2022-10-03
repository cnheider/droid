namespace droid.Runtime.Prototyping.Sensors.Auditory {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "AudioWaveform"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.AudioListener))]
  public class AudioWaveformSensor : Sensor,
                                     droid.Runtime.Interfaces.IHasFloatArray {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { return this.ObservationArray; }
    }

    #if NEODROID_DEBUG
    void LateUpdate() {
      if (this.Debugging) {
        var samples = new float[256];
        UnityEngine.AudioListener.GetOutputData(samples : samples, 0);

        for (var i = 1; i < samples.Length - 1; i++) {
          var prev = samples[i - 1] * 3;
          var cur = samples[i] * 3;
          var next = samples[i + 1] * 3;
          UnityEngine.Debug.DrawLine(start : new UnityEngine.Vector3(x : i - 1, y : cur, 0),
                                     end : new UnityEngine.Vector3(x : i, y : next, 0),
                                     color : UnityEngine.Color.red);
        }
      }
    }
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public float[] ObservationArray { get; set; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1[] ObservationSpace { get; } =
      new droid.Runtime.Structs.Space.Space1[1];

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      var samples = new float[256];
      UnityEngine.AudioListener.GetOutputData(samples : samples, 0);
      this.ObservationArray = samples;
    }
  }
}