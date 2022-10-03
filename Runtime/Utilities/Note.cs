#if UNITY_EDITOR

namespace droid.Runtime.Utilities {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public class Note : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    [UnityEngine.TextAreaAttribute]
    [UnityEngine.TooltipAttribute("A component for holding notes or comments")]
    [UnityEngine.SerializeField]
    public string _Text;

    /// <summary>
    /// </summary>
    [System.NonSerializedAttribute]
    public bool _Editing;

    void Start() { this.enabled = false; }

    /// <summary>
    /// </summary>
    public void EditToggle() { this._Editing = !this._Editing; }
  }
}
#endif