namespace droid.Runtime.Utilities.Orientation {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class PointInDirection : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _direction = UnityEngine.Vector3.down;

    /// <summary>
    /// </summary>
    void Update() {
      this.transform.rotation = UnityEngine.Quaternion.LookRotation(forward : this._direction);
    }
  }
}