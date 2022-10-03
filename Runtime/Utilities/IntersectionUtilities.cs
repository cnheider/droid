namespace droid.Runtime.Utilities {
  public static class IntersectionUtilities {
    // https://www.geometrictools.com/Documentation/IntersectionSphereCone.pdf
    public static bool ConeSphereIntersection(UnityEngine.Light spot_light,
                                              UnityEngine.Transform dynamic_object,
                                              float bounding_sphere_radius = 1) {
      // FIXME Optimize computations of boundingSphereRadius^2, 1.0f / Mathf.Sin(angle * 0.5f), Mathf.Sin(angle * 0.5f)^2 and  Mathf.Cos(angle * 0.5f)^2
      var sin = UnityEngine.Mathf.Sin(f : spot_light.spotAngle * 0.5f * UnityEngine.Mathf.Deg2Rad);
      var cos = UnityEngine.Mathf.Cos(f : spot_light.spotAngle * 0.5f * UnityEngine.Mathf.Deg2Rad);
      var offset = bounding_sphere_radius / sin;
      var transform = spot_light.transform;
      var forward = transform.forward;
      var u = transform.position - offset * forward; // Assumes unit length forward.
      var d = dynamic_object.transform.position - u;

      var distance = UnityEngine.Vector3.Dot(lhs : forward, rhs : d); // Assumes unit length forward.

      return distance >= d.magnitude * cos && distance <= offset + spot_light.range + bounding_sphere_radius;
    }

    public static bool SphereSphereIntersection(UnityEngine.Light point_light,
                                                UnityEngine.Transform dynamic_object,
                                                float bounding_sphere_radius = 1) {
      var diff = dynamic_object.transform.position - point_light.transform.position;
      return diff.magnitude - bounding_sphere_radius <= point_light.range;
    }
  }
}