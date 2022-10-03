namespace droid.Runtime.Prototyping.Sensors.Visual.Deprecated {
  /// <summary>
  /// </summary>
  enum ImageFormat {
    /// <summary>
    /// </summary>
    Jpg_,

    /// <summary>
    /// </summary>
    Png_,

    /// <summary>
    /// </summary>
    Exr_,

    /// <summary>
    /// </summary>
    Tga_,

    /// <summary>
    /// </summary>
    Raw_
  }

  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "EncodedCamera"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.DisallowMultipleComponent]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  public class EncodedCameraSensor : Sensor,
                                     droid.Runtime.Interfaces.IHasByteArray {
    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    protected UnityEngine.Camera _Camera;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    ImageFormat imageFormat = ImageFormat.Png_;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 100)]
    int jpegQuality = 75;

    [UnityEngine.SerializeField] bool disable_encoding = false;

    /// <summary>
    /// </summary>
    protected bool _Grab = true;

    /// <summary>
    /// </summary>
    protected droid.Runtime.Interfaces.IManager _Manager = null;

    /// <summary>
    /// </summary>
    UnityEngine.Texture2D _texture = null;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return ""; } }

    /// <summary>
    /// </summary>
    public UnityEngine.Experimental.Rendering.GraphicsFormat DataType {
      get {
        return UnityEngine.Experimental.Rendering.GraphicsFormat.None; //this.imageFormat;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable { get { return null; } }

    /// <summary>
    /// </summary>
    protected virtual void OnPostRender() { this.UpdateBytes(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("Observation", order = 103)]
    public byte[] Bytes { get; private set; } = { };

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int[] Shape {
      get {
        var channels = 4;
        if (this.imageFormat == ImageFormat.Jpg_) {
          channels = 3;
        }

        return new[] {this._texture.width, this._texture.height, channels};
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public string ArrayEncoding {
      get {
        switch (this.imageFormat) {
          case ImageFormat.Jpg_:
            return "JPEG";
          case ImageFormat.Png_:
            return "PNG";
          case ImageFormat.Exr_:
            return "EXR";
          case ImageFormat.Tga_:
            return "TGA";
          case ImageFormat.Raw_:
            return "RAW";
          default: throw new System.ArgumentOutOfRangeException();
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      if (this._Manager == null) {
        this._Manager = FindObjectOfType<droid.Runtime.Managers.AbstractNeodroidManager>();
      }

      if (this._Camera == null) {
        this._Camera = this.GetComponent<UnityEngine.Camera>();
      }

      if (this._Camera) {
        var target_texture = this._Camera.targetTexture;
        if (!target_texture) {
          UnityEngine.Debug.LogWarning(message :
                                       $"No targetTexture defaulting to a texture of size ({droid.Runtime.Utilities.NeodroidConstants._Default_Width}, {droid.Runtime.Utilities.NeodroidConstants._Default_Height})");

          this._texture =
              new UnityEngine.Texture2D(width : droid.Runtime.Utilities.NeodroidConstants._Default_Width,
                                        height : droid.Runtime.Utilities.NeodroidConstants._Default_Height);
        } else {
          var texture_format_str = target_texture.format.ToString();
          if (System.Enum.TryParse(value : texture_format_str,
                                   result : out UnityEngine.TextureFormat texture_format)) {
            this._texture = new UnityEngine.Texture2D(width : target_texture.width,
                                                      height : target_texture.height,
                                                      textureFormat : texture_format,
                                                      mipChain : target_texture.useMipMap,
                                                      linear : !target_texture.sRGB);
          } else {
            #if NEODROID_DEBUG
            UnityEngine.Debug.LogWarning(message :
                                         $"Texture format {texture_format_str} is not a valid TextureFormat for Texture2D for sensor {this.Identifier}");
            #endif
          }
        }
      }
      #if NEODROID_DEBUG
      if (this._Manager?.SimulatorConfiguration != null) {
        if (this._Manager.SimulatorConfiguration.SimulationType
            != droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_
            && UnityEngine.Application.isEditor) {
          //Debug.Log("Notice that camera observations may be out of sync with other observation data, because simulation configuration is not frame dependent");
        }
      }
      #endif
    }

    /// <summary>
    /// </summary>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    protected void UpdateBytes() {
      if (!this._Grab) {
        return;
      }

      this._Grab = false;

      if (this._Camera) {
        var current_render_texture = UnityEngine.RenderTexture.active;
        UnityEngine.RenderTexture.active = this._Camera.targetTexture;

        if (this._texture) {
          this._texture.ReadPixels(source : new UnityEngine.Rect(0,
                                                                 0,
                                                                 width : this._texture.width,
                                                                 height : this._texture.height),
                                   0,
                                   0);
          this._texture.Apply();
        } else {
          #if NEODROID_DEBUG
          UnityEngine.Debug.LogWarning("Texture not available!");
          #endif
          var target_texture = this._Camera.targetTexture;
          this._texture = new UnityEngine.Texture2D(width : target_texture.width,
                                                    height : target_texture.height,
                                                    textureFormat : droid.Runtime.Utilities.NeodroidConstants
                                                        ._Default_TextureFormat,
                                                    false);
        }

        if (!this.disable_encoding) {
          switch (this.imageFormat) {
            case ImageFormat.Jpg_:
              this.Bytes =
                  UnityEngine.ImageConversion.EncodeToJPG(tex : this._texture, quality : this.jpegQuality);
              break;
            case ImageFormat.Png_:
              this.Bytes = UnityEngine.ImageConversion.EncodeToPNG(tex : this._texture);
              break;
            case ImageFormat.Exr_:
              this.Bytes = UnityEngine.ImageConversion.EncodeToEXR(tex : this._texture);
              break;
            case ImageFormat.Tga_:
              this.Bytes = UnityEngine.ImageConversion.EncodeToTGA(tex : this._texture);
              break;
            case ImageFormat.Raw_:
              this.Bytes = this._texture.GetRawTextureData();
              break;
            /*case ImageFormat.Ppm_:
              // create a file header for ppm formatted file
              string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
              fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
              this.Bytes = _texture.GetRawTextureData();

                      new System.Threading.Thread(() =>
        {
            // create file and write optional header with image bytes
            var f = System.IO.File.Create(filename);
            if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
            f.Write(fileData, 0, fileData.Length);
            f.Close();
            Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
        }).Start();
            */
            default: throw new System.ArgumentOutOfRangeException();
          }
        }

        UnityEngine.RenderTexture.active = current_render_texture;
      } else {
        UnityEngine.Debug.LogWarning(message : $"No camera found on {this}");
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this._Grab = true;
      if (this._Manager?.SimulatorConfiguration?.SimulationType
          != droid.Runtime.Enums.SimulationTypeEnum.Frame_dependent_) {
        if (UnityEngine.Application.isPlaying) {
          this._Camera.Render();
        }

        this.UpdateBytes();
      }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() { return $"Rendered {this.imageFormat} image"; }
  }
}