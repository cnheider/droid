﻿using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Managers;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Camera {
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
    Tga_,
    Bmp_,
    Raw_
  }

  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "Camera"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(UnityEngine.Camera))]
  public class CameraSensor : Sensor,
                              IHasByteArray {
    /// <summary>
    /// </summary>
    [Header("Observation", order = 103)]
    //[SerializeField]
    byte[] _bytes = { };

    /// <summary>
    /// </summary>
    [Header("Specific", order = 102)]
    [SerializeField]
    protected UnityEngine.Camera _Camera;

    /// <summary>
    /// </summary>
    protected bool _Grab = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    ImageFormat imageFormat = ImageFormat.Jpg_;

    /// <summary>
    /// </summary>
    [SerializeField]
    [Range(0, 100)]
    int jpegQuality = 75;

    /// <summary>
    /// </summary>
    protected IManager _Manager = null;

    public override String PrototypingTypeName { get { return ""; } }

    /// <summary>
    /// </summary>
    Texture2D _texture = null;

    [SerializeField] bool disable_encoding = false;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public byte[] Bytes { get { return this._bytes; } private set { this._bytes = value; } }

    public String DataType { get { return this.imageFormat.ToString(); } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._Manager = FindObjectOfType<NeodroidManager>();
      this._Camera = this.GetComponent<UnityEngine.Camera>();
      if (this._Camera) {
        var target_texture = this._Camera.targetTexture;
        if (!target_texture) {
          Debug.LogWarning($"No targetTexture defaulting to a texture of size ({NeodroidConstants._Default_Width}, {NeodroidConstants._Default_Height})");

          this._texture = new Texture2D(NeodroidConstants._Default_Width, NeodroidConstants._Default_Height);
        } else {
          var texture_format_str = target_texture.format.ToString();
          if (Enum.TryParse(texture_format_str, out TextureFormat texture_format)) {
            this._texture = new Texture2D(target_texture.width,
                                          target_texture.height,
                                          texture_format,
                                          target_texture.useMipMap,
                                          !target_texture.sRGB);
          } else {
            #if NEODROID_DEBUG
            Debug.LogWarning($"Texture format {texture_format_str} is not a valid TextureFormat for Texture2D for observer {this.Identifier}");
            #endif
          }
        }
      }
      #if NEODROID_DEBUG
      if (this._Manager?.SimulatorConfiguration != null) {
        if (this._Manager.SimulatorConfiguration.SimulationType != SimulationType.Frame_dependent_
            && Application.isEditor) {
          //Debug.Log("Notice that camera observations may be out of sync with other observation data, because simulation configuration is not frame dependent");
        }
      }
      #endif
    }

    /// <summary>
    /// </summary>
    protected virtual void OnPostRender() { this.UpdateBytes(); }

    /// <summary>
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected void UpdateBytes() {
      if (!this._Grab) {
        return;
      }

      this._Grab = false;

      if (this._Camera) {
        var current_render_texture = RenderTexture.active;
        RenderTexture.active = this._Camera.targetTexture;

        if (this._texture) {
          this._texture.ReadPixels(new Rect(0, 0, this._texture.width, this._texture.height), 0, 0);
          this._texture.Apply();
        } else {
          #if NEODROID_DEBUG
            Debug.LogWarning("Texture not available!");
          #endif
          this._texture = new Texture2D(NeodroidConstants._Default_Width, NeodroidConstants._Default_Height);
        }

        if (!this.disable_encoding) {
          switch (this.imageFormat) {
            case ImageFormat.Jpg_:
              this.Bytes = this._texture.EncodeToJPG(this.jpegQuality);
              break;
            case ImageFormat.Png_:
              this.Bytes = this._texture.EncodeToPNG();
              break;
            case ImageFormat.Exr_:
              this.Bytes = this._texture.EncodeToEXR();
              break;
            case ImageFormat.Tga_:
              this.Bytes = this._texture.EncodeToTGA();
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
            default: throw new ArgumentOutOfRangeException();
          }
        }

        RenderTexture.active = current_render_texture;
      } else {
        Debug.LogWarning($"No camera found on {this}");
      }
    }

    public override IEnumerable<float> FloatEnumerable { get { return new List<float>(); } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this._Grab = true;
      if (this._Manager?.SimulatorConfiguration?.SimulationType != SimulationType.Frame_dependent_) {
        if (Application.isPlaying) {
          if (UnityEngine.Camera.current) {
            this._Camera.Render();
          }
        }

        this.UpdateBytes();
      }
    }

    public override string ToString() { return $"Rendered {this.imageFormat} image"; }
  }
}
