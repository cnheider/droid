namespace droid.Runtime.Prototyping.Sensors.Visual {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "ByteArrayCamera"
                                           + SensorComponentMenuPath._Postfix)]
  public class ByteArrayCameraSensor : Sensor,
                                       droid.Runtime.Interfaces.IHasByteArray {
    const UnityEngine.Experimental.Rendering.TextureCreationFlags _flags =
        UnityEngine.Experimental.Rendering.TextureCreationFlags.None;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.Camera _camera = null;

    [UnityEngine.SerializeField] bool linear_space;
    [UnityEngine.SerializeField] UnityEngine.Camera disable_camera_when_unused;

    byte[] _bytes = { };
    bool _grab = true;
    droid.Runtime.Interfaces.IManager _manager = null;
    UnityEngine.RenderTexture _rt;
    UnityEngine.Texture2D _texture = null;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable { get { return null; } }

    void OnDestroy() {
      if (this._rt) {
        this._rt.Release();
      }
    }

    /// <summary>
    /// </summary>
    protected virtual void OnPostRender() {
      if (this._manager?.SimulatorConfiguration?.SimulationType
          == droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_) {
        this.UpdateArray();
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        //Graphics.DrawTexture(new Rect(new Vector2(0, 0), new Vector2(256, 256)), this._texture);
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public byte[] Bytes {
      get { return this._bytes; }
      private set {
        if (value != null) {
          this._bytes = value;
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int[] Shape {
      get {
        int channels;
        switch (this._texture.graphicsFormat) {
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R8_UNorm:
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R16_SFloat:
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R32_SFloat:
            channels = 1;
            break;
          //case GraphicsFormat.R32G32B32A32_SFloat:
          //case GraphicsFormat.B8G8R8A8_UNorm:
          //case GraphicsFormat.R16G16B16A16_SFloat:
          default:
            channels = 4;
            break;
        }

        return new[] {this._texture.width, this._texture.height, channels};
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string ArrayEncoding {
      get {
        string s;
        switch (this._texture.graphicsFormat) {
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R8G8B8A8_UNorm:
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R8_UInt:
            s = "UINT8";
            break;
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R16_SFloat:
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R16G16B16A16_SFloat:
            s = "FLOAT16";
            break;
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R32_SFloat:
          case UnityEngine.Experimental.Rendering.GraphicsFormat.R32G32B32A32_SFloat:
            s = "FLOAT32";
            break;
          default:
            s = "Unknown";
            break;
        }

        return s;
      }
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

      if (this._texture) {
        droid.Runtime.GameObjects.NeodroidCamera.Experimental.UnityHelpers.Destroy(obj : this._texture);
      }

      if (this._manager?.SimulatorConfiguration?.SimulationType
          != droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_) {
        if (this.disable_camera_when_unused) {
          this._camera.enabled = false;
        }
      }

      var target_texture = this._camera.targetTexture;
      if (!target_texture) {
        #if NEODROID_DEBUG
        UnityEngine.Debug.LogWarning(message :
                                     $"RenderTexture target not available on {this.Identifier} not available, allocating a default!");
        #endif
        this._rt =
            new UnityEngine.RenderTexture(width : droid.Runtime.Utilities.NeodroidConstants._Default_Width,
                                          height : droid.Runtime.Utilities.NeodroidConstants._Default_Height,
                                          0,
                                          format : UnityEngine.RenderTextureFormat.ARGBFloat) {
                filterMode = UnityEngine.FilterMode.Point,
                name = $"rt_{this.Identifier}",
                enableRandomWrite = true
            };
        this._rt.Create();
        this._camera.targetTexture = this._rt;
        this._texture =
            new UnityEngine.Texture2D(width : droid.Runtime.Utilities.NeodroidConstants._Default_Width,
                                      height : droid.Runtime.Utilities.NeodroidConstants._Default_Height,
                                      textureFormat : droid.Runtime.Utilities.NeodroidConstants
                                                           ._Default_TextureFormat,
                                      false,
                                      linear : this.linear_space);
      } else {
        this._texture = new UnityEngine.Texture2D(width : target_texture.width,
                                                  height : target_texture.height,
                                                  format : target_texture.graphicsFormat,
                                                  flags : _flags);
      }
    }

    /// <summary>
    /// </summary>
    protected virtual void UpdateArray() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        this._grab = true;
      }
      #endif

      if (!this._grab && this._camera.targetTexture) {
        return;
      }

      this._grab = false;

      if (this._camera) {
        var current_render_texture = UnityEngine.RenderTexture.active;
        var texture = this._camera.targetTexture;
        UnityEngine.RenderTexture.active = texture;

        this._texture.ReadPixels(source : new UnityEngine.Rect(0,
                                                               0,
                                                               width : this._texture.width,
                                                               height : this._texture.height),
                                 0,
                                 0,
                                 false);
        this.Bytes = this._texture.GetRawTextureData();
        UnityEngine.RenderTexture.active = current_render_texture;
      } else {
        UnityEngine.Debug.LogWarning(message : $"No camera found on {this}");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this._grab = true;
      if (this._manager?.SimulatorConfiguration?.SimulationType
          != droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_) {
        if (UnityEngine.Application.isPlaying) {
          this._camera.Render();
        }

        this.UpdateArray();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      if (this.Bytes != null) {
        var rep = $"Byte Array (Length: {this.Bytes.Length}), ";
        if (this.Bytes.Length > 1) {
          rep += $"Sample [{this.Bytes[0]}.." + $"{this.Bytes[this.Bytes.Length - 1]}]";
        }

        return rep;
      }

      return "No data";
    }
  }
}