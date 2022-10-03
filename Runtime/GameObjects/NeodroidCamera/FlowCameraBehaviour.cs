namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  public class FlowCameraBehaviour : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Color _background_color = UnityEngine.Color.white;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 1)]
    float _blending = 0.5f;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 100)]
    float _overlay_amplitude = 60;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Shader _shader = null;

    /// <summary>
    /// </summary>
    UnityEngine.Material _material;

    /// <summary>
    /// </summary>
    void Awake() {
      this.GetComponent<UnityEngine.Camera>().depthTextureMode |=
          UnityEngine.DepthTextureMode.Depth | UnityEngine.DepthTextureMode.MotionVectors;
    }

    /// <summary>
    /// </summary>
    void OnDestroy() {
      /*if (this._material != null) {
          if (Application.isPlaying) {
            Destroy(this._material);
          } else {
            DestroyImmediate(this._material);
          }
      }*/
    }

    /// <summary>
    /// </summary>
    /// <param name="source"></param>
    /// <param name="destination"></param>
    void OnRenderImage(UnityEngine.RenderTexture source, UnityEngine.RenderTexture destination) {
      if (this._material == null) {
        var shader = this._shader;
        if (shader != null) {
          this._material =
              new UnityEngine.Material(shader : shader) {hideFlags = UnityEngine.HideFlags.DontSave};
        }
      }

      var material = this._material;
      if (material != null) {
        material.SetColor("_BackgroundColor", value : this._background_color);
        material.SetFloat("_Blending", value : this._blending);
        material.SetFloat("_Amplitude", value : this._overlay_amplitude);
        UnityEngine.Graphics.Blit(source : source, dest : destination, mat : material);
      }
    }
  }
}