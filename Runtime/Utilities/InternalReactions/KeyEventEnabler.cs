namespace droid.Runtime.Utilities.InternalReactions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class KeyEventEnabler : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.GameObject _game_object = null;

    [UnityEngine.SerializeField]
    [SearchableEnum]
    UnityEngine.KeyCode _key = UnityEngine.KeyCode.None;

    /// <summary>
    /// </summary>
    void Update() {
      #if !INPUT_SYSTEM_EXISTS
      if (UnityEngine.Input.GetKeyDown(key : this._key)) {
        this._game_object?.SetActive(value : !this._game_object.activeSelf);
      }
      #endif
    }
  }
}