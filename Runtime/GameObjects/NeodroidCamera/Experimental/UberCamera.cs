namespace droid.Runtime.GameObjects.NeodroidCamera.Experimental {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  [UnityEngine.ExecuteInEditMode]
  public class UberCamera : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    void Awake() {
      if (!this.gui_style) {
        this.gui_style =
            System.Linq.Enumerable.First(source : UnityEngine.Resources
                                                             .FindObjectsOfTypeAll<UnityEngine.GUISkin>(),
                                         a => a.name == "BoundingBox");
      }

      if (!this._copy_material) {
        this._copy_material = new UnityEngine.Material(shader : this.copy_shader);
      }

      if (!this._quad_mesh) {
        this._quad_mesh = CreateFullscreenQuad();
      }

      if (!this._camera) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      this.Dispose();

      if (this._fb_rts == null || this._fb_rts.Length != 2) {
        this._fb_rts = new UnityEngine.RenderTexture[2];
        for (var i = 0; i < this._fb_rts.Length; ++i) {
          this._fb_rts[i] = new UnityEngine.RenderTexture(width : _texture_wh.Item1,
                                                          height : _texture_wh.Item2,
                                                          0,
                                                          format : UnityEngine.RenderTextureFormat.ARGBHalf) {
                                filterMode = UnityEngine.FilterMode.Point, name = $"rt_fb{i}"
                            };
          this._fb_rts[i].Create();
        }
      }

      this._m_rt_gb_ids =
          new UnityEngine.Rendering.RenderTargetIdentifier[] {this._fb_rts[0], this._fb_rts[1]};

      if (this._gb_rts == null || this._gb_rts.Length != 8) {
        /*
 half4 albedo        : SV_Target0;
  half4 occlusion     : SV_Target1;
  half4 specular      : SV_Target2;
  half4 smoothness    : SV_Target3;
  half4 normal        : SV_Target4;
  half4 emission      : SV_Target5;
  half4 depth         : SV_Target6;
 */
        var names = new[] {
                              "albedo",
                              "occlusion",
                              "specular",
                              "smoothness",
                              "normal",
                              "emission",
                              "depth",
                              "velocity"
                          };
        this._gb_rts = new UnityEngine.RenderTexture[8];
        for (var i = 0; i < this._gb_rts.Length; ++i) {
          this._gb_rts[i] = new UnityEngine.RenderTexture(width : _texture_wh.Item1,
                                                          height : _texture_wh.Item2,
                                                          0,
                                                          format : UnityEngine.RenderTextureFormat.ARGBHalf) {
                                filterMode = UnityEngine.FilterMode.Point, name = $"{names[i]}"
                            };
          this._gb_rts[i].Create();
        }
      }

      this._m_rt_fb_ids = new UnityEngine.Rendering.RenderTargetIdentifier[] {
                              this._gb_rts[0],
                              this._gb_rts[1],
                              this._gb_rts[2],
                              this._gb_rts[3],
                              this._gb_rts[4],
                              this._gb_rts[5],
                              this._gb_rts[6]
                          };

      this.Setup();
    }

    void Update() {
      if (_texture_wh.Item1 == UnityEngine.Screen.width && _texture_wh.Item2 == UnityEngine.Screen.height) {
        return;
      }

      var xw = _texture_wh.Item1;
      var yh = _texture_wh.Item2;

      var x = UnityEngine.Screen.width / 2 - xw / 2;
      var y = UnityEngine.Screen.height / 2 - yh / 2;

      this._camera.pixelRect = new UnityEngine.Rect(x : x,
                                                    y : y,
                                                    width : xw,
                                                    height : yh);
    }

    void OnDestroy() {
      //this.Dispose();
    }

    void OnGUI() {
      if (this._debugging) {
        var index = 0;

        if (this._gb_rts != null) {
          for (var i = 0; i < this._gb_rts.Length; i++) {
            var pass = this._gb_rts[i];
            var xi = (_preview_size + _preview_margin) * index++;
            var x = xi % (UnityEngine.Screen.width - _preview_size);
            var y = (_preview_size + _preview_margin) * (xi / (UnityEngine.Screen.width - _preview_size));
            var r = new UnityEngine.Rect(x : _preview_margin + x,
                                         y : _preview_margin + y,
                                         width : _preview_size,
                                         height : _preview_size);
            //this._asf?.Flip(pass._RenderTexture);

            UnityEngine.GUI.DrawTexture(position : r,
                                        image : pass,
                                        scaleMode : UnityEngine.ScaleMode.ScaleToFit);
            UnityEngine.GUI.TextField(position : r, text : pass.name, style : this.gui_style.box);
          }
        }

        if (this._fb_rts != null) {
          for (var i = 0; i < this._fb_rts.Length; i++) {
            var pass = this._fb_rts[i];
            var xi = (_preview_size + _preview_margin) * index++;
            var x = xi % (UnityEngine.Screen.width - _preview_size);
            var y = (_preview_size + _preview_margin) * (xi / (UnityEngine.Screen.width - _preview_size));
            var r = new UnityEngine.Rect(x : _preview_margin + x,
                                         y : _preview_margin + y,
                                         width : _preview_size,
                                         height : _preview_size);
            //this._asf?.Flip(pass._RenderTexture);

            UnityEngine.GUI.DrawTexture(position : r,
                                        image : pass,
                                        scaleMode : UnityEngine.ScaleMode.ScaleToFit);
            UnityEngine.GUI.TextField(position : r, text : pass.name, style : this.gui_style.box);
          }
        }
      }
    }

    protected System.Tuple<int, int> GetCaptureResolutionFromCamera() {
      var w = this._camera.pixelWidth;
      var h = this._camera.pixelHeight;
      var aspect = (float)h / w;
      w = _texture_wh.Item1;
      h = (int)(w * aspect);
      return new System.Tuple<int, int>(item1 : w, item2 : h);
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
      if (this.copy_shader == null) {
        UnityEngine.Debug.LogError("Copy shader is missing!");
        return;
      }

      if (this._off_screen_mat != null) {
        if (this._camera.targetTexture != null) {
          this._off_screen_mat.EnableKeyword("OFFSCREEN");
        } else {
          this._off_screen_mat.DisableKeyword("OFFSCREEN");
        }
      }

      this._copy_fb_cb = new UnityEngine.Rendering.CommandBuffer {name = "Copy FrameBuffer"};
      this._copy_fb_cb.GetTemporaryRT(nameID : this._tmp_texture_id,
                                      -1,
                                      -1,
                                      0,
                                      filter : UnityEngine.FilterMode.Point);
      this._copy_fb_cb.Blit(source : UnityEngine.Rendering.BuiltinRenderTextureType.CurrentActive,
                            dest : this._tmp_texture_id);
      this._copy_fb_cb.SetRenderTarget(colors : this._m_rt_gb_ids, depth : this._fb_rts[0]);
      this._copy_fb_cb.DrawMesh(mesh : this._quad_mesh,
                                matrix : UnityEngine.Matrix4x4.identity,
                                material : this._copy_material,
                                0,
                                0);
      this._copy_fb_cb.ReleaseTemporaryRT(nameID : this._tmp_texture_id);
      this._camera.AddCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.AfterEverything,
                                    buffer : this._copy_fb_cb);

      this._clear_gb_cb =
          new UnityEngine.Rendering.CommandBuffer {
                                                      name = "Cleanup GBuffer"
                                                  }; // clear gbuffer (Unity doesn't clear emission buffer - it is not needed usually)
      if (this._camera.allowHDR) {
        this._clear_gb_cb.SetRenderTarget(rt : UnityEngine.Rendering.BuiltinRenderTextureType.CameraTarget);
      } else {
        this._clear_gb_cb.SetRenderTarget(rt : UnityEngine.Rendering.BuiltinRenderTextureType.GBuffer3);
      }

      this._clear_gb_cb.DrawMesh(mesh : this._quad_mesh,
                                 matrix : UnityEngine.Matrix4x4.identity,
                                 material : this._copy_material,
                                 0,
                                 3);
      this._copy_material.SetColor(nameID : _clear_color, value : this._camera.backgroundColor);

      this._copy_gb_cb = new UnityEngine.Rendering.CommandBuffer {name = "Copy GBuffer"}; // copy gbuffer
      this._copy_gb_cb.SetRenderTarget(colors : this._m_rt_fb_ids, depth : this._gb_rts[0]);
      this._copy_gb_cb.DrawMesh(mesh : this._quad_mesh,
                                matrix : UnityEngine.Matrix4x4.identity,
                                material : this._copy_material,
                                0,
                                2);
      this._camera.AddCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeGBuffer,
                                    buffer : this._clear_gb_cb);
      this._camera.AddCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeLighting,
                                    buffer : this._copy_gb_cb);

      this._copy_velocity_cb = new UnityEngine.Rendering.CommandBuffer {name = "Copy Velocity"};
      this._copy_velocity_cb.SetRenderTarget(rt : this._gb_rts[7]);
      this._copy_velocity_cb.DrawMesh(mesh : this._quad_mesh,
                                      matrix : UnityEngine.Matrix4x4.identity,
                                      material : this._copy_material,
                                      0,
                                      4);
      this._camera.AddCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeImageEffectsOpaque,
                                    buffer : this._copy_velocity_cb);
      this._camera.depthTextureMode =
          UnityEngine.DepthTextureMode.Depth | UnityEngine.DepthTextureMode.MotionVectors;

      this._copy_cbs = new[] {
                                 this._copy_fb_cb,
                                 this._clear_gb_cb,
                                 this._copy_gb_cb,
                                 this._copy_velocity_cb
                             };
    }

    void Dispose() {
      this._camera.RemoveAllCommandBuffers(); // cleanup capturing camera

      if (this._copy_fb_cb != null) {
        this._camera.RemoveCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.AfterEverything,
                                         buffer : this._copy_fb_cb);
        this._copy_fb_cb.Release();
        this._copy_fb_cb = null;
      }

      if (this._clear_gb_cb != null) {
        this._camera.RemoveCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeGBuffer,
                                         buffer : this._clear_gb_cb);
        this._clear_gb_cb.Release();
        this._clear_gb_cb = null;
      }

      if (this._copy_gb_cb != null) {
        this._camera.RemoveCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeLighting,
                                         buffer : this._copy_gb_cb);
        this._copy_gb_cb.Release();
        this._copy_gb_cb = null;
      }

      if (this._copy_velocity_cb != null) {
        this._camera.RemoveCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeImageEffectsOpaque,
                                         buffer : this._copy_velocity_cb);
        this._copy_velocity_cb.Release();
        this._copy_velocity_cb = null;
      }

      if (this._fb_rts != null) {
        for (var index = 0; index < this._fb_rts.Length; index++) {
          var rt = this._fb_rts[index];
          rt.Release();
        }

        this._fb_rts = null;
      }

      if (this._gb_rts != null) {
        for (var index = 0; index < this._gb_rts.Length; index++) {
          var rt = this._gb_rts[index];
          rt.Release();
        }

        this._gb_rts = null;
      }
    }

    #region fields

    [UnityEngine.SerializeField] UnityEngine.Shader copy_shader = null;
    [UnityEngine.SerializeField] UnityEngine.Material _copy_material = null;
    [UnityEngine.SerializeField] UnityEngine.Material _off_screen_mat = null;
    [UnityEngine.SerializeField] UnityEngine.Camera _camera = null;
    [UnityEngine.SerializeField] bool _debugging = true;
    [UnityEngine.SerializeField] UnityEngine.GUISkin gui_style = null;

    UnityEngine.Rendering.CommandBuffer[] _copy_cbs = null;
    UnityEngine.Rendering.CommandBuffer _copy_fb_cb = null;
    UnityEngine.Rendering.CommandBuffer _copy_gb_cb = null;
    UnityEngine.Rendering.CommandBuffer _clear_gb_cb = null;
    UnityEngine.Rendering.CommandBuffer _copy_velocity_cb = null;
    UnityEngine.RenderTexture[] _fb_rts = null;
    UnityEngine.RenderTexture[] _gb_rts = null;
    UnityEngine.Mesh _quad_mesh = null;

    UnityEngine.Rendering.RenderTargetIdentifier[] _m_rt_fb_ids = null;
    UnityEngine.Rendering.RenderTargetIdentifier[] _m_rt_gb_ids = null;
    int _tmp_texture_id = UnityEngine.Shader.PropertyToID("_TmpFrameBuffer");
    static readonly int _clear_color = UnityEngine.Shader.PropertyToID("_ClearColor");

    static readonly System.Tuple<int, int> _texture_wh = new System.Tuple<int, int>(256, 256);

    const int _preview_size = 100;
    const int _preview_margin = 20;

    #endregion
  }
}