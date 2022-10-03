namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated {
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "FloatArrayCamera"
                                           + SensorComponentMenuPath._Postfix)]
  public class FloatArrayCameraSensor : Sensor,
                                        droid.Runtime.Interfaces.IHasFloatArray {
    [UnityEngine.SerializeField] bool _black_white = false;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.Camera _camera = null;

    [UnityEngine.SerializeField] UnityEngine.Texture2D _texture = null;

    bool _grab = true;

    droid.Runtime.Interfaces.IManager _manager = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { return this.ObservationArray; }
    }

    /// <summary>
    /// </summary>
    protected virtual void OnPostRender() {
      if (this._camera.targetTexture) {
        this.UpdateArray();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("Observation", order = 103)]
    public float[] ObservationArray { get; private set; } = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1[] ObservationSpace {
      get { return new[] {droid.Runtime.Structs.Space.Space1.ZeroOne}; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      if (this._manager == null) {
        this._manager = FindObjectOfType<droid.Runtime.Managers.AbstractNeodroidManager>();
      }

      if (this._camera == null) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      var target_texture = this._camera.targetTexture;
      if (target_texture) {
        this._texture =
            new UnityEngine.Texture2D(width : target_texture.width, height : target_texture.height);
        if (this._black_white) {
          this.ObservationArray = new float[this._texture.width * this._texture.height * 1]; // *1 for clarity
        } else {
          this.ObservationArray = new float[this._texture.width * this._texture.height * 3];
        }
      } else {
        this.ObservationArray = new float[0];
      }
    }

    /// <summary>
    /// </summary>
    protected virtual void UpdateArray() {
      if (!this._grab) {
        return;
      }

      this._grab = false;

      var current_render_texture = UnityEngine.RenderTexture.active;
      var target_texture = this._camera.targetTexture;
      UnityEngine.RenderTexture.active = target_texture;

      this._texture.ReadPixels(source : new UnityEngine.Rect(0,
                                                             0,
                                                             width : target_texture.width,
                                                             height : target_texture.height),
                               0,
                               0);
      this._texture.Apply();

      if (!this._black_white) {
        for (var w = 0; w < this._texture.width; w++) {
          for (var h = 0; h < this._texture.height; h++) {
            var c = this._texture.GetPixel(x : w, y : h);
            this.ObservationArray[this._texture.width * w + h * 3] = c.r;
            this.ObservationArray[this._texture.width * w + h * 3 + 1] = c.g;
            this.ObservationArray[this._texture.width * w + h * 3 + 2] = c.b;
          }
        }
      } else {
        for (var w = 0; w < this._texture.width; w++) {
          for (var h = 0; h < this._texture.height; h++) {
            var c = this._texture.GetPixel(x : w, y : h);
            this.ObservationArray[this._texture.width * w + h] = (c.r + c.g + c.b) / 3;
          }
        }
      }

      UnityEngine.RenderTexture.active = current_render_texture;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        if (this._manager?.SimulatorConfiguration != null) {
          if (this._manager.SimulatorConfiguration.SimulationType
              != droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_) {
            UnityEngine.Debug.LogWarning("WARNING! Camera Observations may be out of sync other data");
          }
        }
      }
      #endif

      this._grab = true;
      var manager = this._manager;
      if (manager != null
          && manager.SimulatorConfiguration.SimulationType
          != droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_) {
        this.UpdateArray();
      }
    }
  }
}