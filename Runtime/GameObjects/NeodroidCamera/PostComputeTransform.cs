namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  [UnityEngine.ExecuteInEditMode]
  public class PostComputeTransform : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    public UnityEngine.ComputeShader _TransformationComputeShader;

    //[SerializeField] Material _post_material;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    UnityEngine.Camera _camera = null;

    UnityEngine.Rendering.CommandBuffer _transformation_command_buffer;

    UnityEngine.ComputeBuffer _transformation_compute_buffer;

    /// <summary>
    /// </summary>
    public UnityEngine.RenderTexture MyRenderTexture { get; set; }

    void Awake() {
      if (this._camera == null) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      this.MyRenderTexture = new UnityEngine.RenderTexture(256, 256, 0) {enableRandomWrite = true};
      this.MyRenderTexture.Create();

      if (this._TransformationComputeShader) {
        var kernel_id = this._TransformationComputeShader.FindKernel("CSMain");

        this._transformation_command_buffer = new UnityEngine.Rendering.CommandBuffer();

        /*
      var target_texture = this._camera.targetTexture;
      this._TransformationComputeShader.SetTexture(kernel_id,
                                                   "Result",
                                                   target_texture);

      this._transformation_compute_buffer = new ComputeBuffer(target_texture.width * target_texture.height * target_texture.depth,
                                        sizeof(float)) {name = "My Buffer"};

*/

        //Graphics.SetRandomWriteTarget(1, my_buffer);

        //myBuffer.SetData(minMaxHeight);
        //this.my_buffer.targetTexture.GetNativeDepthBufferPtr().GetBuffer(0, "minMax", minMaxBuffer);

        //_TransformationComputeShader.SetFloat("gamma", 2.2);
        //_TransformationComputeShader.Dispatch(0, map.Length, 1, 1);

        //my_buffer.SetData(*target_texture.GetNativeTexturePtr());

//      this._TransformationComputeShader.SetTexture(kernelHandle, "Result", target_texture);

        this._transformation_command_buffer.SetComputeTextureParam(computeShader :
                                                                   this._TransformationComputeShader,
                                                                   kernelIndex : kernel_id,
                                                                   "Result",
                                                                   rt : this.MyRenderTexture);
        //this._transformation_command_buffer.SetComputeBufferParam(this._TransformationComputeShader, kernel_id,"Result",this._transformation_compute_buffer);
        this._transformation_command_buffer.DispatchCompute(computeShader : this._TransformationComputeShader,
                                                            kernelIndex : kernel_id,
                                                            threadGroupsX : 256 / 32,
                                                            threadGroupsY : 256 / 32,
                                                            1);
        //this._camera.AddCommandBuffer(CameraEvent.AfterEverything, this._transformation_command_buffer);
      }
    }

    void Update() {
      if (this._TransformationComputeShader) {
        this._TransformationComputeShader.SetTexture(0, "Result", texture : this.MyRenderTexture);
        //this._TransformationComputeShader.SetBuffer(0,"",this._transformation_compute_buffer);
        this._TransformationComputeShader.Dispatch(0,
                                                   threadGroupsX : 256 / 32,
                                                   threadGroupsY : 256 / 32,
                                                   1);
      }
    }

// Postprocess the image
/*
  void OnRenderImage(RenderTexture source, RenderTexture destination) {



    Graphics.Blit(source, destination);
  }
*/

/// <summary>
/// </summary>
public void OnDisable() { this.Cleanup(); }

    void OnDestroy() {
      this.Cleanup();

//DestroyImmediate(this._GammaCommandBuffer);
    }

    void Cleanup() {
      if (this._transformation_command_buffer != null) {
        this._camera.RemoveCommandBuffer(evt : UnityEngine.Rendering.CameraEvent.AfterEverything,
                                         buffer : this._transformation_command_buffer);
      }
    }
  }
}