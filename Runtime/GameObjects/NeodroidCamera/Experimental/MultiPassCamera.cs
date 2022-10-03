namespace droid.Runtime.GameObjects.NeodroidCamera.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class MultiPassCamera : UnityEngine.MonoBehaviour {
    const int _size = 100;
    const int _margin = 20;

    [UnityEngine.SerializeField] UnityEngine.RenderTexture depthRenderTexture = null;
    [UnityEngine.SerializeField] UnityEngine.RenderTexture objectIdRenderTexture = null;
    [UnityEngine.SerializeField] UnityEngine.RenderTexture tagIdRenderTexture = null;
    [UnityEngine.SerializeField] UnityEngine.RenderTexture flowRenderTexture = null;

    [UnityEngine.SerializeField] CapturePassMaterial[] _capture_passes;

    [UnityEngine.SerializeField] UnityEngine.Camera _camera;
    [UnityEngine.SerializeField] bool debug = true;
    [UnityEngine.SerializeField] bool always_re = true;
    [UnityEngine.SerializeField] UnityEngine.Mesh m_quad;
    [UnityEngine.SerializeField] UnityEngine.GUISkin gui_style = null;

    /// <summary>
    /// </summary>
    UnityEngine.Renderer[] _all_renders = null;

    TextureFlipper _asf;

    /// <summary>
    /// </summary>
    UnityEngine.MaterialPropertyBlock _block = null;

    void Awake() {
      //this._asf= new TextureFlipper();
    }

    /// <summary>
    /// </summary>
    void Start() { this.Setup(); }

    void OnGUI() {
      if (this.debug) {
        var index = 0;

        foreach (var pass in this._capture_passes) {
          var xi = (_size + _margin) * index++;
          var x = xi % (UnityEngine.Screen.width - _size);
          var y = (_size + _margin) * (xi / (UnityEngine.Screen.width - _size));
          var r = new UnityEngine.Rect(x : _margin + x,
                                       y : _margin + y,
                                       width : _size,
                                       height : _size);
          //this._asf?.Flip(pass._RenderTexture);

          UnityEngine.GUI.DrawTexture(position : r,
                                      image : pass._RenderTexture,
                                      scaleMode : UnityEngine.ScaleMode.ScaleToFit);
          UnityEngine.GUI.TextField(position : r, text : pass.Source.ToString(), style : this.gui_style.box);
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
    /// <returns></returns>
    public static UnityEngine.Mesh CreateFullscreenQuad() {
      var r = new UnityEngine.Mesh {
                                       vertices = new[] {
                                                            new UnityEngine.Vector3(1.0f, 1.0f, 0.0f),
                                                            new UnityEngine.Vector3(-1.0f, 1.0f, 0.0f),
                                                            new UnityEngine.Vector3(-1.0f, -1.0f, 0.0f),
                                                            new UnityEngine.Vector3(1.0f, -1.0f, 0.0f)
                                                        },
                                       triangles = new[] {
                                                             0,
                                                             1,
                                                             2,
                                                             2,
                                                             3,
                                                             0
                                                         }
                                   };
      r.UploadMeshData(true);
      return r;
    }

    /// <summary>
    /// </summary>
    void Setup() {
      if (!this.gui_style) {
        this.gui_style =
            System.Linq.Enumerable.First(source : UnityEngine.Resources
                                                             .FindObjectsOfTypeAll<UnityEngine.GUISkin>(),
                                         a => a.name == "BoundingBox");
      }

      this._all_renders = FindObjectsOfType<UnityEngine.Renderer>();
      if (this._capture_passes == null || this._capture_passes.Length == 0 || this.always_re) {
        this._capture_passes = new[] {
                                         new CapturePassMaterial(when : UnityEngine.Rendering.CameraEvent
                                                                     .AfterDepthTexture,
                                                                 source : UnityEngine.Rendering
                                                                     .BuiltinRenderTextureType.Depth) {
                                             _SupportsAntialiasing = false,
                                             _RenderTexture = this.depthRenderTexture
                                         },
                                         new CapturePassMaterial(when : UnityEngine.Rendering.CameraEvent
                                                                     .AfterForwardAlpha,
                                                                 source : UnityEngine.Rendering
                                                                     .BuiltinRenderTextureType
                                                                     .MotionVectors) {
                                             _SupportsAntialiasing = false,
                                             _RenderTexture = this.flowRenderTexture
                                         },
                                         new CapturePassMaterial(when : UnityEngine.Rendering.CameraEvent
                                                                     .AfterForwardAlpha,
                                                                 source : UnityEngine.Rendering
                                                                     .BuiltinRenderTextureType.None) {
                                             _SupportsAntialiasing = false,
                                             _RenderTexture = this.objectIdRenderTexture,
                                             _TextureId = UnityEngine.Shader.PropertyToID("_TmpFrameBuffer")
                                         },
                                         new CapturePassMaterial(when : UnityEngine.Rendering.CameraEvent
                                                                     .AfterDepthTexture,
                                                                 source : UnityEngine.Rendering
                                                                     .BuiltinRenderTextureType.None) {
                                             _SupportsAntialiasing = false,
                                             _RenderTexture = this.tagIdRenderTexture,
                                             _TextureId =
                                                 UnityEngine.Shader.PropertyToID("_CameraDepthTexture")
                                         }
                                     };
      }

      if (this.m_quad == null) {
        this.m_quad = CreateFullscreenQuad();
      }

      this._camera = this.GetComponent<UnityEngine.Camera>();
      //this._camera.SetReplacementShader(this.uberMaterial.shader,"");

      this._camera.RemoveAllCommandBuffers(); // cleanup capturing camera

      this._camera.depthTextureMode =
          UnityEngine.DepthTextureMode.Depth | UnityEngine.DepthTextureMode.MotionVectors;

      foreach (var capture_pass in this._capture_passes) {
        var cb = new UnityEngine.Rendering.CommandBuffer {name = capture_pass.Source.ToString()};

        cb.Clear();

        if (capture_pass._Material) {
          cb.GetTemporaryRT(nameID : capture_pass._TextureId,
                            -1,
                            -1,
                            0,
                            filter : UnityEngine.FilterMode.Point);
          //cb.Blit(capture_pass.Source, capture_pass._RenderTexture, capture_pass._Material);
          cb.Blit(source : capture_pass.Source, dest : capture_pass._TextureId);
          cb.SetRenderTarget(colors : new UnityEngine.Rendering.RenderTargetIdentifier[] {
                                          capture_pass._RenderTexture
                                      },
                             depth : capture_pass._RenderTexture);
          cb.DrawMesh(mesh : this.m_quad,
                      matrix : UnityEngine.Matrix4x4.identity,
                      material : capture_pass._Material,
                      0,
                      0);
          cb.ReleaseTemporaryRT(nameID : capture_pass._TextureId);
        } else {
          cb.Blit(source : capture_pass.Source, dest : capture_pass._RenderTexture);
        }

        this._camera.AddCommandBuffer(evt : capture_pass.When, buffer : cb);
      }

      this.CheckBlock();
      foreach (var r in this._all_renders) {
        r.GetPropertyBlock(properties : this._block);
        var sm = r.sharedMaterial;
        if (sm) {
          var id = sm.GetInstanceID();
          var color =
              droid.Runtime.GameObjects.NeodroidCamera.Synthesis.ColorEncoding
                   .EncodeIdAsColor(instance_id : id);

          this._block.SetColor(name : droid.Runtime.GameObjects.NeodroidCamera.Synthesis.SynthesisUtilities
                                           ._Shader_MaterialId_Color_Name,
                               value : color);
          r.SetPropertyBlock(properties : this._block);
        }
      }
    }
  }

  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct CapturePassMaterial {
    public bool _SupportsAntialiasing;
    public bool _NeedsRescale;
    public UnityEngine.Material _Material;
    public UnityEngine.RenderTexture _RenderTexture;
    public UnityEngine.Rendering.CameraEvent When;
    public UnityEngine.Rendering.BuiltinRenderTextureType Source;
    public int _TextureId;

    public CapturePassMaterial(UnityEngine.Rendering.CameraEvent when =
                                   UnityEngine.Rendering.CameraEvent.AfterEverything,
                               UnityEngine.Rendering.BuiltinRenderTextureType source =
                                   UnityEngine.Rendering.BuiltinRenderTextureType.CurrentActive) {
      this.When = when;
      this.Source = source;
      this._Material = null;
      this._RenderTexture = null;
      this._SupportsAntialiasing = false;
      this._NeedsRescale = false;
      this._TextureId = 0;
    }
  }

  public class TextureFlipper : System.IDisposable {
    UnityEngine.Shader _m_sh_v_flip;
    UnityEngine.Material _m_vf_lip_material;
    UnityEngine.RenderTexture _m_work_texture;

    public TextureFlipper() {
      this._m_sh_v_flip = UnityEngine.Shader.Find("Neodroid/Experimental/VerticalFlipper");
      if (this._m_sh_v_flip) {
        this._m_vf_lip_material = new UnityEngine.Material(shader : this._m_sh_v_flip);
      }
    }

    #region IDisposable Members

    public void Dispose() {
      UnityHelpers.Destroy(obj : this._m_work_texture);
      this._m_work_texture = null;
      if (this._m_vf_lip_material) {
        UnityHelpers.Destroy(obj : this._m_vf_lip_material);
        this._m_vf_lip_material = null;
      }
    }

    #endregion

    public void Flip(UnityEngine.RenderTexture target) {
      if (this._m_work_texture == null
          || this._m_work_texture.width != target.width
          || this._m_work_texture.height != target.height) {
        UnityHelpers.Destroy(obj : this._m_work_texture);
        this._m_work_texture = new UnityEngine.RenderTexture(width : target.width,
                                                             height : target.height,
                                                             depth : target.depth,
                                                             format : target.format,
                                                             readWrite : UnityEngine.RenderTextureReadWrite
                                                                 .Linear);
      }

      if (this._m_vf_lip_material) {
        UnityEngine.Graphics.Blit(source : target,
                                  dest : this._m_work_texture,
                                  mat : this._m_vf_lip_material);
        UnityEngine.Graphics.Blit(source : this._m_work_texture, dest : target);
      }
    }
  }

  /// <summary>
  ///   What is this:
  ///   Motivation  :
  ///   Notes:
  /// </summary>
  public static class UnityHelpers {
    public static void Destroy(UnityEngine.Object obj, bool allow_destroying_assets = false) {
      if (obj == null) {
        return;
      }
      #if UNITY_EDITOR
      if (UnityEditor.EditorApplication.isPlaying) {
        UnityEngine.Object.Destroy(obj : obj);
      } else {
        UnityEngine.Object.DestroyImmediate(obj : obj, allowDestroyingAssets : allow_destroying_assets);
      }
      #else
            Object.Destroy(obj);
      #endif
      obj = null;
    }

    public static bool IsPlaying() {
      #if UNITY_EDITOR
      return UnityEditor.EditorApplication.isPlaying;
      #else
            return true;
      #endif
    }
  }
}