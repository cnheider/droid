namespace droid.Runtime.GameObjects.Plotting {
  /// <summary>
  /// </summary>
  public class DrawSpaces : UnityEngine.MonoBehaviour {
    void OnDrawGizmos() {
      if (this.enabled) {
        var color = UnityEngine.Color.green;
        // local up
        this.DrawHelperAtCenter(direction : this.transform.up, color : color, 2f);

        color.g -= 0.5f;
        // global up
        this.DrawHelperAtCenter(direction : UnityEngine.Vector3.up, color : color, 1f);

        color = UnityEngine.Color.blue;
        // local forward
        this.DrawHelperAtCenter(direction : this.transform.forward, color : color, 2f);

        color.b -= 0.5f;
        // global forward
        this.DrawHelperAtCenter(direction : UnityEngine.Vector3.forward, color : color, 1f);

        color = UnityEngine.Color.red;
        // local right
        this.DrawHelperAtCenter(direction : this.transform.right, color : color, 2f);

        color.r -= 0.5f;
        // global right
        this.DrawHelperAtCenter(direction : UnityEngine.Vector3.right, color : color, 1f);
      }
    }

    void DrawHelperAtCenter(UnityEngine.Vector3 direction, UnityEngine.Color color, float scale) {
      UnityEngine.Gizmos.color = color;
      var position = this.transform.position;
      var destination = position + direction * scale;
      UnityEngine.Gizmos.DrawLine(from : position, to : destination);
    }
  }
}