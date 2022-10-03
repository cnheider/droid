namespace droid.Runtime.Prototyping.ObjectiveFunctions.Spatial.Waypoint {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class Waypoint : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] float radius = 1.0f;

    public float Radius { get { return this.radius; } }
  }
}