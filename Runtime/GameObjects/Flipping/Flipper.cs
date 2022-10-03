namespace droid.Runtime.GameObjects.Flipping {
  /// <summary>
  /// </summary>
  public class Flipper : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.ComputeShader _shader;
    [UnityEngine.SerializeField] UnityEngine.Texture2D _texture_2d;

    void Start() { this._texture_2d = new UnityEngine.Texture2D(256, 256); }

    void OnRenderImage(UnityEngine.RenderTexture src, UnityEngine.RenderTexture dest) {
      this.FlipImage(my_texture : src, result : this._texture_2d);

      UnityEngine.Graphics.Blit(source : this._texture_2d, dest : dest);
    }

    /// <summary>
    /// </summary>
    /// <param name="my_texture"></param>
    /// <param name="result"></param>
    public void FlipImage(UnityEngine.Texture my_texture, UnityEngine.Texture2D result) {
      var kernel_handle = this._shader.FindKernel("Flip");
      var tex =
          new UnityEngine.RenderTexture(width : my_texture.width, height : my_texture.height, 24) {
              enableRandomWrite = true
          };
      tex.Create();

      this._shader.SetTexture(kernelIndex : kernel_handle, "Result", texture : tex);
      this._shader.SetTexture(kernelIndex : kernel_handle, "ImageInput", texture : my_texture);
      this._shader.SetInt("width", val : my_texture.width);
      this._shader.SetInt("height", val : my_texture.height);
      this._shader.Dispatch(kernelIndex : kernel_handle,
                            threadGroupsX : my_texture.width / 8,
                            threadGroupsY : my_texture.height / 8,
                            1);

      UnityEngine.RenderTexture.active = tex;
      result.ReadPixels(source : new UnityEngine.Rect(0,
                                                      0,
                                                      width : tex.width,
                                                      height : tex.height),
                        0,
                        0);
      result.Apply();
    }
  }
}