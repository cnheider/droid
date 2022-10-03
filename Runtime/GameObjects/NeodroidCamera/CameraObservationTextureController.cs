namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class CameraObservationTextureController : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.Camera[] _cameras = null;

    [UnityEngine.SerializeField] UnityEngine.FilterMode _filter_mode = UnityEngine.FilterMode.Bilinear;
    //[SerializeField] GraphicsFormat _texture_format = GraphicsFormat.R8G8B8A8_UNorm;

    [UnityEngine.SerializeField] UnityEngine.Texture[] _textures = null;
    [UnityEngine.SerializeField] UnityEngine.TextureWrapMode _wrap_mode = UnityEngine.TextureWrapMode.Clamp;

    [UnityEngine.SerializeField]
    UnityEngine.Vector2Int _size =
        new UnityEngine.Vector2Int(x : droid.Runtime.Utilities.NeodroidConstants
                                            ._Default_Observation_Texture_Xy_Size,
                                   y : droid.Runtime.Utilities.NeodroidConstants
                                            ._Default_Observation_Texture_Xy_Size);

    void Awake() {
      this._cameras = FindObjectsOfType<UnityEngine.Camera>();

      var textures = new System.Collections.Generic.List<UnityEngine.Texture>();

      for (var index = 0; index < this._cameras.Length; index++) {
        var a_camera = this._cameras[index];
        var target = a_camera.targetTexture;
        if (target) {
          textures.Add(item : target);
        }
      }

      this._textures = textures.ToArray();

      for (var index = 0; index < this._textures.Length; index++) {
        var texture = this._textures[index];
        if (texture) {
          //texture.height = this._size.y;
          //texture.width = this._size.x;
          texture.filterMode = this._filter_mode;
          texture.wrapMode = this._wrap_mode;
          //texture.graphicsFormat = this._texture_format;
        }
      }
    }
  }
}