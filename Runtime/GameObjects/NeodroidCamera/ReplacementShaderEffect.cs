namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class ReplacementShaderEffect : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] string _replace_render_type = "";

    [UnityEngine.SerializeField] UnityEngine.Shader _replacement_shader = null;

    void Start() {
      if (this._replacement_shader != null) {
        this.GetComponent<UnityEngine.Camera>()
            .SetReplacementShader(shader : this._replacement_shader,
                                  replacementTag : this._replace_render_type);
      }
    }

    void OnEnable() {
      if (this._replacement_shader != null) {
        this.GetComponent<UnityEngine.Camera>()
            .SetReplacementShader(shader : this._replacement_shader,
                                  replacementTag : this._replace_render_type);
      }
    }

    void OnDisable() { this.GetComponent<UnityEngine.Camera>().ResetReplacementShader(); }
  }
}