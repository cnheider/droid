namespace droid.Runtime.Prototyping.Sensors.Auditory {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "AudioSpectrum"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.AudioListener))]
  public class AudioSpectrumSensor : Sensor,
                                     droid.Runtime.Interfaces.IHasFloatArray {
    [UnityEngine.SerializeField] readonly float[] _observation_array = new float[256];

    UnityEngine.FFTWindow _fft_window = UnityEngine.FFTWindow.Rectangular;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { return this.ObservationArray; }
    }
    #if NEODROID_DEBUG
    void LateUpdate() {
      if (this.Debugging) {
        var spectrum = new float[256];
        UnityEngine.AudioListener.GetSpectrumData(samples : spectrum,
                                                  0,
                                                  window : UnityEngine.FFTWindow.Rectangular);

        for (var i = 1; i < spectrum.Length - 1; i++) {
          var prev = spectrum[i - 1];
          var cur = spectrum[i];
          var next = spectrum[i + 1];
          UnityEngine.Debug.DrawLine(start : new UnityEngine.Vector3(x : i - 1, y : cur + 10, 0),
                                     end : new UnityEngine.Vector3(x : i, y : next + 10, 0),
                                     color : UnityEngine.Color.red);
          UnityEngine.Debug.DrawLine(start : new UnityEngine.Vector3(x : i - 1,
                                                                     y : UnityEngine.Mathf.Log(f : prev) + 10,
                                                                     2),
                                     end : new UnityEngine.Vector3(x : i,
                                                                   y : UnityEngine.Mathf.Log(f : cur) + 10,
                                                                   2),
                                     color : UnityEngine.Color.cyan);
          UnityEngine.Debug.DrawLine(start : new UnityEngine.Vector3(x : UnityEngine.Mathf.Log(f : i - 1),
                                                                     y : prev - 10,
                                                                     1),
                                     end : new UnityEngine.Vector3(x : UnityEngine.Mathf.Log(f : i),
                                                                   y : cur - 10,
                                                                   1),
                                     color : UnityEngine.Color.green);
          UnityEngine.Debug.DrawLine(start : new UnityEngine.Vector3(x : UnityEngine.Mathf.Log(f : i - 1),
                                                                     y : UnityEngine.Mathf.Log(f : prev),
                                                                     3),
                                     end : new UnityEngine.Vector3(x : UnityEngine.Mathf.Log(f : i),
                                                                   y : UnityEngine.Mathf.Log(f : cur),
                                                                   3),
                                     color : UnityEngine.Color.blue);
        }
      }
    }
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public float[] ObservationArray { get { return this._observation_array; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1[] ObservationSpace { get; } =
      new droid.Runtime.Structs.Space.Space1[1];

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      UnityEngine.AudioListener.GetSpectrumData(samples : this._observation_array,
                                                0,
                                                window : this._fft_window);
    }
  }
}