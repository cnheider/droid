namespace droid.Runtime.Prototyping.Sensors.Spatial.EntityCentric.Rays.Experimental {
  /// <inheritdoc />
  /// <summary>
  ///   Ray perception component. Attach this to agents to enable "local perception"
  ///   via the use of ray casts directed outward from the agent.
  /// </summary>
  public class Ray : UnityEngine.MonoBehaviour {
    UnityEngine.Vector3 _end_position;
    UnityEngine.RaycastHit _hit;
    System.Collections.Generic.List<float> _perception_buffer = new System.Collections.Generic.List<float>();

    /// <summary>
    ///   Creates perception vector to be used as part of an observation of an agent.
    /// </summary>
    /// <returns>The partial vector observation corresponding to the set of rays</returns>
    /// <param name="ray_distance">Radius of rays</param>
    /// <param name="ray_angles">Anlges of rays (starting from (1,0) on unit circle).</param>
    /// <param name="detectable_objects">List of tags which correspond to object types agent can see</param>
    /// <param name="start_offset">Starting height offset of ray from center of agent.</param>
    /// <param name="end_offset">Ending height offset of ray from center of agent.</param>
    public System.Collections.Generic.List<float> Perceive(float ray_distance,
                                                           System.Collections.Generic.IEnumerable<float>
                                                               ray_angles,
                                                           string[] detectable_objects,
                                                           float start_offset,
                                                           float end_offset) {
      this._perception_buffer.Clear();
      // For each ray sublist stores categorial information on detected object
      // along with object distance.
      foreach (var angle in ray_angles) {
        this._end_position =
            this.transform.TransformDirection(direction : PolarToCartesian(radius : ray_distance,
                                                angle : angle));
        this._end_position.y = end_offset;
        if (UnityEngine.Application.isEditor) {
          UnityEngine.Debug.DrawRay(start : this.transform.position
                                            + new UnityEngine.Vector3(0f, y : start_offset, 0f),
                                    dir : this._end_position,
                                    color : UnityEngine.Color.black,
                                    0.01f,
                                    true);
        }

        var sub_list = new float[detectable_objects.Length + 2];
        if (UnityEngine.Physics.SphereCast(origin : this.transform.position
                                                    + new UnityEngine.Vector3(0f, y : start_offset, 0f),
                                           0.5f,
                                           direction : this._end_position,
                                           hitInfo : out this._hit,
                                           maxDistance : ray_distance)) {
          for (var i = 0; i < detectable_objects.Length; i++) {
            if (this._hit.collider.gameObject.CompareTag(tag : detectable_objects[i])) {
              sub_list[i] = 1;
              sub_list[detectable_objects.Length + 1] = this._hit.distance / ray_distance;
              break;
            }
          }
        } else {
          sub_list[detectable_objects.Length] = 1f;
        }

        this._perception_buffer.AddRange(collection : sub_list);
      }

      return this._perception_buffer;
    }

    /// <summary>
    ///   Converts polar coordinate to cartesian coordinate.
    /// </summary>
    public static UnityEngine.Vector3 PolarToCartesian(float radius, float angle) {
      var x = radius * UnityEngine.Mathf.Cos(f : DegreeToRadian(degree : angle));
      var z = radius * UnityEngine.Mathf.Sin(f : DegreeToRadian(degree : angle));
      return new UnityEngine.Vector3(x : x, 0f, z : z);
    }

    /// <summary>
    ///   Converts degrees to radians.
    /// </summary>
    public static float DegreeToRadian(float degree) { return degree * UnityEngine.Mathf.PI / 180f; }
  }
}