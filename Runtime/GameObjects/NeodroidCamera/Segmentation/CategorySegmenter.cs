namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation {
  /// <summary>
  /// </summary>
  enum SegmentationMode {
    Tag_,
    Layer_
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class CategorySegmenter : Segmenter {
    [UnityEngine.SerializeField] UnityEngine.Shader segmentation_shader = null;
    [UnityEngine.SerializeField] UnityEngine.Camera _camera = null;

    [UnityEngine.SerializeField] protected droid.Runtime.Structs.ColorByCategory[] _colors_by_category = null;

    [UnityEngine.SerializeField] SegmentationMode _segmentation_mode = SegmentationMode.Tag_;

    [UnityEngine.SerializeField] droid.Runtime.ScriptableObjects.Segmentation _segmentation_preset = null;

    /// <summary>
    /// </summary>
    public bool _Replace_Untagged_Color = true;

    /// <summary>
    /// </summary>
    public UnityEngine.Color _Untagged_Color = new UnityEngine.Color(0.0f,
                                                                     0.0f,
                                                                     0.0f,
                                                                     0.0f);

    /// <summary>
    /// </summary>
    UnityEngine.Renderer[] _all_renders = null;

    /// <summary>
    /// </summary>
    UnityEngine.MaterialPropertyBlock _block = null;

    droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities.CapturePass[] _capture_passes;

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.ColorByCategory[] ColorsByCategory {
      get { return this._colors_by_category; }
    }

    /// <summary>
    /// </summary>
    public System.Collections.Generic.Dictionary<string, UnityEngine.Color> ColorsDictGameObject { get; set; }
      = new System.Collections.Generic.Dictionary<string, UnityEngine.Color>();

    /// <summary>
    /// </summary>
    public override System.Collections.Generic.Dictionary<string, UnityEngine.Color> ColorsDict {
      get {
        var colors = new System.Collections.Generic.Dictionary<string, UnityEngine.Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          colors.Add(key : key_val.Key, value : key_val.Value);
        }

        return colors;
      }
    }

    /// <summary>
    /// </summary>
    void Awake() { this.Setup(); }

    /// <summary>
    /// </summary>
    void Start() {
      //this.Setup();
    }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new UnityEngine.MaterialPropertyBlock();
      }
    }

    /// <summary>
    /// </summary>
    void Setup() {
      if (this._colors_by_category != null && this._colors_by_category.Length > 0) {
        for (var index = 0; index < this._colors_by_category.Length; index++) {
          var tag_color = this._colors_by_category[index];
          if (!this.ColorsDictGameObject.ContainsKey(key : tag_color._Category_Name)) {
            this.ColorsDictGameObject.Add(key : tag_color._Category_Name, value : tag_color._Color);
          }
        }
      }

      if (this._segmentation_preset) {
        var segmentation_color_by_tags = this._segmentation_preset._color_by_categories;
        if (segmentation_color_by_tags != null) {
          for (var index = 0; index < segmentation_color_by_tags.Length; index++) {
            var tag_color = segmentation_color_by_tags[index];
            if (!this.ColorsDictGameObject.ContainsKey(key : tag_color._Category_Name)) {
              this.ColorsDictGameObject.Add(key : tag_color._Category_Name, value : tag_color._Color);
            }
          }
        }
      }

      this._all_renders = FindObjectsOfType<UnityEngine.Renderer>();
      if (!this._camera) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      if (this.ColorsDictGameObject == null) {
        this.ColorsDictGameObject = new System.Collections.Generic.Dictionary<string, UnityEngine.Color>();
      }

      switch (this._segmentation_mode) {
        case SegmentationMode.Tag_:
          this._capture_passes = new[] {
                                           new droid.Runtime.GameObjects.NeodroidCamera.Synthesis.
                                           SynthesisUtilities.CapturePass {
                                               _Name = "_tag_id",
                                               _ReplacementMode =
                                                   droid.Runtime.GameObjects.NeodroidCamera.Synthesis
                                                        .SynthesisUtilities.ReplacementModes.Tag_id_,
                                               _SupportsAntialiasing = false
                                           }
                                       };
          break;
        case SegmentationMode.Layer_:
          this._capture_passes = new[] {
                                           new droid.Runtime.GameObjects.NeodroidCamera.Synthesis.
                                           SynthesisUtilities.CapturePass {
                                               _Name = "_layer_id",
                                               _ReplacementMode =
                                                   droid.Runtime.GameObjects.NeodroidCamera.Synthesis
                                                        .SynthesisUtilities.ReplacementModes.Layer_id_,
                                               _SupportsAntialiasing = false
                                           }
                                       };
          break;
        default: throw new System.ArgumentOutOfRangeException();
      }

      droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities
           .SetupCapturePassesReplacementShader(camera : this._camera,
                                                replacement_shader : this.segmentation_shader,
                                                capture_passes : ref this._capture_passes);

      this.CheckBlock();
      for (var index = 0; index < this._all_renders.Length; index++) {
        var a_renderer = this._all_renders[index];
        a_renderer.GetPropertyBlock(properties : this._block);
        string category_name;
        var category_int = 0;
        UnityEngine.Color color;
        string shader_data_name;
        switch (this._segmentation_mode) {
          case SegmentationMode.Tag_:
            category_name = a_renderer.tag;
            shader_data_name = droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities
                                    ._Shader_Tag_Color_Name;
            break;
          case SegmentationMode.Layer_:
            category_int = a_renderer.gameObject.layer;
            category_name = UnityEngine.LayerMask.LayerToName(layer : category_int);
            shader_data_name = droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities
                                    ._Shader_Layer_Color_Name;
            break;
          default: throw new System.ArgumentOutOfRangeException();
        }

        if (!this.ColorsDictGameObject.ContainsKey(key : category_name)) {
          if (!this._Replace_Untagged_Color) {
            switch (this._segmentation_mode) {
              case SegmentationMode.Tag_:
                category_int = category_name.GetHashCode();
                color = droid.Runtime.GameObjects.NeodroidCamera.Synthesis.ColorEncoding
                             .EncodeTagHashCodeAsColor(hash : category_int);
                //color = ColorEncoding.EncodeIdAsColor(category_int);
                break;
              case SegmentationMode.Layer_:
                color =
                    droid.Runtime.GameObjects.NeodroidCamera.Synthesis.ColorEncoding
                         .EncodeLayerAsColor(layer : category_int);
                break;
              default:
                //color = ColorEncoding.EncodeIdAsColor(category_int);
                throw new System.ArgumentOutOfRangeException();
            }
          } else {
            color = this._Untagged_Color;
          }

          this.ColorsDictGameObject.Add(key : category_name, value : color);
        } else {
          color = this.ColorsDictGameObject[key : category_name];
        }

        this._block.SetColor(name : shader_data_name, value : color);

        a_renderer.SetPropertyBlock(properties : this._block);
      }
    }
  }
}