namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated {
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "NativeColorArrayCamera"
                                           + SensorComponentMenuPath._Postfix)]
  public class NativeColorFloatArrayCameraSensor : Sensor,
                                                   droid.Runtime.Interfaces.IHasFloatArray {
    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.Camera _camera = null;

    [UnityEngine.SerializeField] UnityEngine.Texture2D _texture = null;

    bool _grab = true;

    droid.Runtime.Interfaces.IManager _manager = null;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return ""; } }

    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get { return null; } //this.ObservationArray; }
    }

    /// <summary>
    /// </summary>
    protected virtual void OnPostRender() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        this._grab = true;
      }
      #endif
      if (this._camera.targetTexture) {
        this.UpdateArray();
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        //Graphics.DrawTexture(new Rect(new Vector2(0, 0), new Vector2(256, 256)), this._texture);
      }
      #endif
    }

    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("Observation", order = 103)]
    public float[] ObservationArray { get; private set; }

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space1[] ObservationSpace {
      get { return new[] {droid.Runtime.Structs.Space.Space1.ZeroOne}; }
    }

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
        this.ObservationArray = new float[target_texture.width * target_texture.height * 4];
      } else {
        #if NEODROID_DEBUG
        UnityEngine.Debug.LogWarning("Texture not available!");
        #endif
        this._texture =
            new UnityEngine.Texture2D(width : droid.Runtime.Utilities.NeodroidConstants._Default_Width,
                                      height : droid.Runtime.Utilities.NeodroidConstants._Default_Height,
                                      textureFormat : droid.Runtime.Utilities.NeodroidConstants
                                                           ._Default_TextureFormat,
                                      false);
        this.ObservationArray = new float[this._texture.width * this._texture.height * 4];
      }
    }

    /// <summary>
    /// </summary>
    protected virtual void UpdateArray() {
      if (!this._grab) {
        return;
      }

      this._grab = false;

      if (this._camera) {
        var current_render_texture = UnityEngine.RenderTexture.active;
        UnityEngine.RenderTexture.active = this._camera.targetTexture;

        if (this._texture
            && this._camera.targetTexture.width == this._texture.width
            && this._camera.targetTexture.height == this._texture.height) {
          this._texture.ReadPixels(source : new UnityEngine.Rect(0,
                                                                 0,
                                                                 width : this._texture.width,
                                                                 height : this._texture.height),
                                   0,
                                   0);
          //this._texture.Apply();
        } else {
          #if NEODROID_DEBUG
          UnityEngine.Debug.LogWarning("Texture not available!");
          #endif
          this._texture = new UnityEngine.Texture2D(width : this._camera.targetTexture.width,
                                                    height : this._camera.targetTexture.height,
                                                    textureFormat : droid.Runtime.Utilities.NeodroidConstants
                                                        ._Default_TextureFormat,
                                                    false);
          this.ObservationArray = new float[this._texture.width * this._texture.height * 4];
        }

        //Texture2D texCopy = new Texture2D(_texture.width, _texture.height, _texture.format, _texture.mipmapCount > 1);
        //texCopy.LoadRawTextureData(_texture.GetRawTextureData());
        //texCopy.Apply();
        //var a = texCopy.GetRawTextureData<Color>();
        var a = this._texture.GetRawTextureData<UnityEngine.Color>();

        #if NEODROID_DEBUG
        var min = a[0];
        var max = a[0];

        #endif

        var i = 0;
/*
        foreach (var b in a) {
          this.flat_float_array[i] = b.r;
          this.flat_float_array[i + 1] = b.g;
          this.flat_float_array[i + 2] = b.b;
          this.flat_float_array[i + 3] = b.a;
          i += 4;
        }
*/
        for (var index = 0; index < a.Length; index++) {
          var b = a[index : index];
          //i = index*4;
          this.ObservationArray[i] = b.r;
          this.ObservationArray[i + 1] = b.g;
          this.ObservationArray[i + 2] = b.b;
          this.ObservationArray[i + 3] = b.a;
          i += 4;

          #if NEODROID_DEBUG
          if (this.Debugging) {
            max[0] = UnityEngine.Mathf.Max(a : b[0], b : max[0]);
            min[0] = UnityEngine.Mathf.Min(a : b[0], b : min[0]);
            max[1] = UnityEngine.Mathf.Max(a : b[1], b : max[1]);
            min[1] = UnityEngine.Mathf.Min(a : b[1], b : min[1]);
            max[2] = UnityEngine.Mathf.Max(a : b[2], b : max[2]);
            min[2] = UnityEngine.Mathf.Min(a : b[2], b : min[2]);
            max[3] = UnityEngine.Mathf.Max(a : b[3], b : max[3]);
            min[3] = UnityEngine.Mathf.Min(a : b[3], b : min[3]);
          }
          #endif
        }

        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"len(a):{a.Length}, min:{min}, max:{max}");
        }
        #endif

        UnityEngine.RenderTexture.active = current_render_texture;
      } else {
        UnityEngine.Debug.LogWarning(message : $"No camera found on {this}");
      }
    }

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

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      var rep = $"Float Array (Length: {this.ObservationArray.Length}), "
                + $"Sample [{UnityEngine.Mathf.Clamp01(value : this.ObservationArray[0])}.."
                + $"{UnityEngine.Mathf.Clamp01(value : this.ObservationArray[this.ObservationArray.Length - 1])}]";

      return rep;
    }
  }
}