namespace droid.Runtime.GameObjects.NeodroidCamera.Synthesis {
  /// <summary>
  /// </summary>
  public static class SynthesisUtilities {
    #region ReplacementModes enum

    /// <summary>
    /// </summary>
    public enum ReplacementModes {
      /// <summary>
      /// </summary>
      Object_id_ = 0,

      /// <summary>
      /// </summary>
      Material_id_ = 1,

      /// <summary>
      /// </summary>
      Layer_id_ = 2,

      /// <summary>
      /// </summary>
      Tag_id_ = 3,

      /// <summary>
      /// </summary>
      Depth_compressed_ = 4,

      /// <summary>
      /// </summary>
      Depth_multichannel_ = 5,

      /// <summary>
      /// </summary>
      Normals_ = 6,

      /// <summary>
      /// </summary>
      None_ = 7,

      /// <summary>
      /// </summary>
      Flow_ = 8
    }

    #endregion

    public const string _Shader_Layer_Color_Name = "_LayerColor";
    public const string _Shader_Tag_Color_Name = "_TagColor";
    public const string _Shader_MaterialId_Color_Name = "_MaterialIdColor";
    public const string _Shader_ObjectId_Color_Name = "_ObjectIdColor";
    public const string _Shader_OutputMode_Name = "_OutputMode";
    static readonly int _sensitivity = UnityEngine.Shader.PropertyToID("_Sensitivity");

    public static CapturePass[] _Default_Capture_Passes = {
                                                              new CapturePass {
                                                                  _Name = "_img",
                                                                  _ReplacementMode = ReplacementModes.None_
                                                              },
                                                              new CapturePass {
                                                                  _Name = "_id",
                                                                  _SupportsAntialiasing = false,
                                                                  _ReplacementMode =
                                                                      ReplacementModes.Object_id_
                                                              },
                                                              new CapturePass {
                                                                  _Name = "_layer",
                                                                  _SupportsAntialiasing = false,
                                                                  _ReplacementMode =
                                                                      ReplacementModes.Layer_id_
                                                              },
                                                              new CapturePass {
                                                                  _Name = "_depth",
                                                                  _ReplacementMode =
                                                                      ReplacementModes.Depth_compressed_
                                                              },
                                                              new CapturePass {
                                                                  _Name = "_normals",
                                                                  _ReplacementMode =
                                                                      ReplacementModes.Normals_
                                                              },
                                                              new CapturePass {
                                                                  _Name = "_mat_id",
                                                                  _SupportsAntialiasing = false,
                                                                  _ReplacementMode =
                                                                      ReplacementModes.Material_id_
                                                              },
                                                              new CapturePass {
                                                                  _Name = "_tag",
                                                                  _SupportsAntialiasing = false,
                                                                  _ReplacementMode =
                                                                      ReplacementModes.Material_id_
                                                              },
                                                              new CapturePass {
                                                                  _Name = "_flow",
                                                                  _SupportsAntialiasing = false,
                                                                  _NeedsRescale = true,
                                                                  _ReplacementMode = ReplacementModes.Flow_
                                                              }
                                                          };

    /// <summary>
    /// </summary>
    public static void SetupCapturePassesFull(UnityEngine.Camera camera,
                                              UnityEngine.Shader replacement_shader,
                                              UnityEngine.Shader optical_flow_shader,
                                              UnityEngine.Material optical_flow_material,
                                              float optical_flow_sensitivity,
                                              ref CapturePass[] capture_passes) {
      SetupHiddenCapturePassCameras(camera : camera, capture_passes : ref capture_passes);
      CleanRefreshPassCameras(camera : camera, capture_passes : ref capture_passes);

      // cache materials and setup material properties
      if (!optical_flow_material || optical_flow_material.shader != optical_flow_shader) {
        optical_flow_material = new UnityEngine.Material(shader : optical_flow_shader);
      }

      optical_flow_material.SetFloat(nameID : _sensitivity, value : optical_flow_sensitivity);

      // setup command buffers and replacement shaders
      AddReplacementShaderCommandBufferOnCamera(cam : capture_passes[1]._Camera,
                                                shader : replacement_shader,
                                                mode : capture_passes[1]._ReplacementMode);
      AddReplacementShaderCommandBufferOnCamera(cam : capture_passes[2]._Camera,
                                                shader : replacement_shader,
                                                mode : capture_passes[2]._ReplacementMode);
      AddReplacementShaderCommandBufferOnCamera(cam : capture_passes[6]._Camera,
                                                shader : replacement_shader,
                                                mode : capture_passes[6]._ReplacementMode);

      AddReplacementShaderCommandBufferOnCamera(cam : capture_passes[5]._Camera,
                                                shader : replacement_shader,
                                                mode : capture_passes[5]._ReplacementMode);

      AddReplacementShaderCommandBufferOnCamera(camera : capture_passes[3]._Camera,
                                                shader : replacement_shader,
                                                mode : capture_passes[3]._ReplacementMode,
                                                clear_color : UnityEngine.Color.white);
      AddReplacementShaderCommandBufferOnCamera(cam : capture_passes[4]._Camera,
                                                shader : replacement_shader,
                                                mode : capture_passes[4]._ReplacementMode);
      SetupCameraWithPostShader(cam : capture_passes[8]._Camera,
                                material : optical_flow_material,
                                depth_texture_mode : UnityEngine.DepthTextureMode.Depth
                                                     | UnityEngine.DepthTextureMode.MotionVectors);
    }

    public static void SetupCapturePassesReplacementShader(UnityEngine.Camera camera,
                                                           UnityEngine.Shader replacement_shader,
                                                           ref CapturePass[] capture_passes) {
      SetupHiddenCapturePassCameras(camera : camera, capture_passes : ref capture_passes);
      CleanRefreshPassCameras(camera : camera, capture_passes : ref capture_passes);

      foreach (var capture_pass in capture_passes) {
        AddReplacementShaderCommandBufferOnCamera(cam : capture_pass._Camera,
                                                  shader : replacement_shader,
                                                  mode : capture_pass._ReplacementMode);
      }
    }

    static void CleanRefreshPassCameras(UnityEngine.Camera camera, ref CapturePass[] capture_passes) {
      var target_display = 1;
      foreach (var pass in capture_passes) {
        if (pass._Camera == camera) {
          continue;
        }

        pass._Camera.RemoveAllCommandBuffers(); // cleanup capturing camera
        pass._Camera.CopyFrom(other : camera); // copy all "main" camera parameters into capturing camera
        pass._Camera.targetDisplay =
            target_display++; // set targetDisplay here since it gets overriden by CopyFrom()
      }
    }

    static void
        AddReplacementShaderCommandBufferOnCamera(UnityEngine.Camera cam,
                                                  UnityEngine.Shader shader,
                                                  ReplacementModes mode) {
      AddReplacementShaderCommandBufferOnCamera(camera : cam,
                                                shader : shader,
                                                mode : mode,
                                                clear_color : UnityEngine.Color.black);
    }

    static void AddReplacementShaderCommandBufferOnCamera(UnityEngine.Camera camera,
                                                          UnityEngine.Shader shader,
                                                          ReplacementModes mode,
                                                          UnityEngine.Color clear_color) {
      var cb = new UnityEngine.Rendering.CommandBuffer {name = mode.ToString()};
      cb.SetGlobalInt(name : _Shader_OutputMode_Name, value : (int)mode);
      camera.AddCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeForwardOpaque, buffer : cb);
      camera.AddCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.BeforeFinalPass, buffer : cb);
      camera.SetReplacementShader(shader : shader, "");
      camera.backgroundColor = clear_color;
      camera.clearFlags = UnityEngine.CameraClearFlags.SolidColor;
    }

    static void SetupCameraWithPostShader(UnityEngine.Camera cam,
                                          UnityEngine.Material material,
                                          UnityEngine.DepthTextureMode depth_texture_mode =
                                              UnityEngine.DepthTextureMode.None) {
      var cb = new UnityEngine.Rendering.CommandBuffer {name = cam.name};
      cb.Blit(null, dest : UnityEngine.Rendering.BuiltinRenderTextureType.CurrentActive, mat : material);
      cam.AddCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.AfterEverything, buffer : cb);
      cam.depthTextureMode = depth_texture_mode;
    }

    /// <summary>
    /// </summary>
    /// <param name="camera"></param>
    /// <param name="capture_passes"></param>
    static void SetupHiddenCapturePassCameras(UnityEngine.Camera camera, ref CapturePass[] capture_passes) {
      capture_passes[0]._Camera = camera;
      for (var q = 1; q < capture_passes.Length; q++) {
        capture_passes[q]._Camera =
            CreateHiddenCamera(cam_name : capture_passes[q]._Name, parent : camera.transform);
      }
    }

    static UnityEngine.Camera CreateHiddenCamera(string cam_name, UnityEngine.Transform parent) {
      var go = new UnityEngine.GameObject(name : cam_name, typeof(UnityEngine.Camera)) {
                   hideFlags = UnityEngine.HideFlags.HideAndDontSave
               };
      go.transform.parent = parent;

      var new_camera = go.GetComponent<UnityEngine.Camera>();
      return new_camera;
    }

    #region Nested type: CapturePass

    /// <summary>
    /// </summary>
    [System.SerializableAttribute]
    public struct CapturePass {
      // configuration
      public string _Name;
      public bool _SupportsAntialiasing;
      public bool _NeedsRescale;
      public UnityEngine.Camera _Camera;
      public ReplacementModes _ReplacementMode;
    }

    #endregion
  }
}