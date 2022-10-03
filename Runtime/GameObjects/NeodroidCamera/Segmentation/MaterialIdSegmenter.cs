namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class MaterialIdSegmenter : Segmenter {
    [UnityEngine.SerializeField] UnityEngine.Shader segmentation_shader = null;
    [UnityEngine.SerializeField] UnityEngine.Camera _camera = null;

    /// <summary>
    /// </summary>
    UnityEngine.Renderer[] _all_renders = null;

    /// <summary>
    /// </summary>
    UnityEngine.MaterialPropertyBlock _block = null;

    droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities.CapturePass[] _capture_passes = {
        new droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities.CapturePass {
            _Name = "_material_id",
            _ReplacementMode =
                droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities.ReplacementModes
                     .Material_id_,
            _SupportsAntialiasing = false
        }
    };

    /// <summary>
    /// </summary>
    public System.Collections.Generic.Dictionary<UnityEngine.Material, UnityEngine.Color>
        ColorsDictGameObject { get; set; } =
      new System.Collections.Generic.Dictionary<UnityEngine.Material, UnityEngine.Color>();

    /// <summary>
    /// </summary>
    public override System.Collections.Generic.Dictionary<string, UnityEngine.Color> ColorsDict {
      get {
        var colors = new System.Collections.Generic.Dictionary<string, UnityEngine.Color>();
        foreach (var key_val in this.ColorsDictGameObject) {
          if (!colors.ContainsKey(key : key_val.Key.name)) {
            colors.Add(key : key_val.Key.name, value : key_val.Value);
          }
        }

        return colors;
      }
    }

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this.Setup(); }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new UnityEngine.MaterialPropertyBlock();
      }
    }

    /// <summary>
    /// </summary>
    void Setup() {
      this._all_renders = FindObjectsOfType<UnityEngine.Renderer>();

      this._camera = this.GetComponent<UnityEngine.Camera>();
      droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities
           .SetupCapturePassesReplacementShader(camera : this._camera,
                                                replacement_shader : this.segmentation_shader,
                                                capture_passes : ref this._capture_passes);

      this.ColorsDictGameObject =
          new System.Collections.Generic.Dictionary<UnityEngine.Material, UnityEngine.Color>();
      this.CheckBlock();
      for (var index = 0; index < this._all_renders.Length; index++) {
        var r = this._all_renders[index];
        r.GetPropertyBlock(properties : this._block);
        var sm = r.sharedMaterial;
        if (sm) {
          var id = sm.GetInstanceID();
          var color =
              droid.Runtime.GameObjects.NeodroidCamera.Synthesis.ColorEncoding
                   .EncodeIdAsColor(instance_id : id);
          if (!this.ColorsDictGameObject.ContainsKey(key : sm)) {
            this.ColorsDictGameObject.Add(key : sm, value : color);
          }

          this._block.SetColor(name : droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities
                                           ._Shader_MaterialId_Color_Name,
                               value : color);
          r.SetPropertyBlock(properties : this._block);
        }
      }
    }
  }
}