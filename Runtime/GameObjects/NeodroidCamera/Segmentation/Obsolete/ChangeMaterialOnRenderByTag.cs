namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Obsolete {
  /// <inheritdoc cref="UnityEngine.MonoBehaviour" />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class ChangeMaterialOnRenderByTag : ObsoleteSegmenter {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected droid.Runtime.Structs.ColorByCategory[] _colors_by_category;

    /// <summary>
    /// </summary>
    public bool _Replace_Untagged_Color = true;

    [UnityEngine.SerializeField] droid.Runtime.ScriptableObjects.Segmentation _segmentation = null;

    /// <summary>
    /// </summary>
    public UnityEngine.Color _Untagged_Color = UnityEngine.Color.black;

    /// <summary>
    /// </summary>
    UnityEngine.Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    UnityEngine.MaterialPropertyBlock _block;

    /// <summary>
    /// </summary>
    System.Collections.Generic.LinkedList<UnityEngine.Color>[] _original_colors;

    /// <summary>
    /// </summary>
    System.Collections.Generic.Dictionary<string, UnityEngine.Color> _tag_colors_dict =
        new System.Collections.Generic.Dictionary<string, UnityEngine.Color>();

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.ColorByCategory[] ColorsByCategory {
      get { return this._colors_by_category; }
    }

    /// <summary>
    /// </summary>
    public override System.Collections.Generic.Dictionary<string, UnityEngine.Color> ColorsDict {
      get { return this._tag_colors_dict; }
    }

    /// <summary>
    /// </summary>
    void Awake() {
      this._block = new UnityEngine.MaterialPropertyBlock();
      this._tag_colors_dict.Clear();
      var colors_by_tag = this._colors_by_category;
      if (colors_by_tag != null && colors_by_tag.Length > 0) {
        for (var index = 0; index < this._colors_by_category.Length; index++) {
          var tag_color = this._colors_by_category[index];
          if (!this._tag_colors_dict.ContainsKey(key : tag_color._Category_Name)) {
            this._tag_colors_dict.Add(key : tag_color._Category_Name, value : tag_color._Color);
          }
        }
      }

      if (this._segmentation) {
        var segmentation_color_by_tags = this._segmentation._color_by_categories;
        if (segmentation_color_by_tags != null) {
          for (var index = 0; index < segmentation_color_by_tags.Length; index++) {
            var tag_color = segmentation_color_by_tags[index];
            if (!this._tag_colors_dict.ContainsKey(key : tag_color._Category_Name)) {
              this._tag_colors_dict.Add(key : tag_color._Category_Name, value : tag_color._Color);
            }
          }
        }
      }

      this.Setup();
    }

    /// <summary>
    /// </summary>
    void Update() {
      this.Setup(); // Renderers maybe be disable and enabled, that is why every update we find all renderers again
    }

    /// <summary>
    /// </summary>
    void Setup() {
      this.CheckBlock();

      this._all_renders = FindObjectsOfType<UnityEngine.Renderer>();
    }

    /// <summary>
    /// </summary>
    protected override void Change() {
      this._original_colors =
          new System.Collections.Generic.LinkedList<UnityEngine.Color>[this._all_renders.Length];
      for (var i = 0; i < this._original_colors.Length; i++) {
        this._original_colors[i] = new System.Collections.Generic.LinkedList<UnityEngine.Color>();
      }

      this.CheckBlock();

      for (var i = 0; i < this._all_renders.Length; i++) {
        var c_renderer = this._all_renders[i];
        if (c_renderer) {
          if (this._tag_colors_dict != null
              && this._tag_colors_dict.ContainsKey(key : this._all_renders[i].tag)) {
            for (var index = 0; index < this._all_renders[i].sharedMaterials.Length; index++) {
              var mat = this._all_renders[i].sharedMaterials[index];
              if (mat != null && mat.HasProperty(nameID : this._Default_Color_Tag)) {
                this._original_colors[i].AddFirst(value : mat.color);
              }

              this._block.SetColor(nameID : this._Segmentation_Color_Tag,
                                   value : this._tag_colors_dict[key : this._all_renders[i].tag]);

              this._block.SetColor(nameID : this._Outline_Color_Tag, value : this._Outline_Color);
              this._block.SetFloat(nameID : this._Outline_Width_Factor_Tag,
                                   value : this._Outline_Width_Factor);
              this._all_renders[i].SetPropertyBlock(properties : this._block);
            }
          } else if (this._Replace_Untagged_Color) {
            for (var index = 0; index < this._all_renders[i].sharedMaterials.Length; index++) {
              var mat = this._all_renders[i].sharedMaterials[index];
              if (mat != null && mat.HasProperty(nameID : this._Default_Color_Tag)) {
                this._original_colors[i].AddFirst(value : mat.color);
              }

              this._block.SetColor(nameID : this._Segmentation_Color_Tag, value : this._Untagged_Color);

              this._block.SetColor(nameID : this._Outline_Color_Tag, value : this._Outline_Color);
              this._block.SetFloat(nameID : this._Outline_Width_Factor_Tag,
                                   value : this._Outline_Width_Factor);
              this._all_renders[i].SetPropertyBlock(properties : this._block);
            }
          }
        }
      }
    }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new UnityEngine.MaterialPropertyBlock();
      }
    }

    /// <summary>
    /// </summary>
    protected override void Restore() {
      this.CheckBlock();

      for (var i = 0; i < this._all_renders.Length; i++) {
        if (this._all_renders[i]) {
          for (var index = 0; index < this._all_renders[i].sharedMaterials.Length; index++) {
            var mat = this._all_renders[i].sharedMaterials[index];
            if (mat != null && this._original_colors != null && i < this._original_colors.Length) {
              var c = this._original_colors[i];
              var last = c?.Last;
              if (last != null) {
                var last_val = last.Value;
                this._block.SetColor(nameID : this._Default_Color_Tag, value : last_val);
                this._original_colors[i].RemoveLast();
                this._all_renders[i].SetPropertyBlock(properties : this._block);
              }
            }
          }
        }
      }
    }
  }
}