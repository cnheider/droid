namespace droid.Runtime.Utilities.Orientation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class FollowTarget : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 _Offset = new UnityEngine.Vector3(0f, 7.5f, 0f);

    /// <summary>
    /// </summary>
    public UnityEngine.Transform target;

    void LateUpdate() {
      if (this.target) {
        this.transform.position = this.target.position + this._Offset;
      }
    }
  }
}