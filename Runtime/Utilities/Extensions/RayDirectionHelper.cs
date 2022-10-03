namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  /// </summary>
  public static class RayDirectionHelper {
    const int _num_view_directions = 300;
    static readonly float _golden_ratio = (1 + UnityEngine.Mathf.Sqrt(5)) / 2;
    static readonly float _angle_increment = UnityEngine.Mathf.PI * 2 * _golden_ratio;

    /// <summary>
    /// </summary>
    public static readonly UnityEngine.Vector3[] _Directions;

    static RayDirectionHelper() {
      _Directions = new UnityEngine.Vector3[_num_view_directions];

      for (var i = 0; i < _num_view_directions; i++) {
        var t = (float)i / _num_view_directions;
        var inclination = UnityEngine.Mathf.Acos(f : 1 - 2 * t);
        var azimuth = _angle_increment * i;

        var x = UnityEngine.Mathf.Sin(f : inclination) * UnityEngine.Mathf.Cos(f : azimuth);
        var y = UnityEngine.Mathf.Sin(f : inclination) * UnityEngine.Mathf.Sin(f : azimuth);
        var z = UnityEngine.Mathf.Cos(f : inclination);
        _Directions[i] = new UnityEngine.Vector3(x : x, y : y, z : z);
      }
    }

    /*
   *     var rayDirections = RayDirectionHelper._Directions;

    foreach (var t in rayDirections) {
      var dir = this._cached_transform.TransformDirection(t);
      var ray = new Ray(this.position, dir);
      if (!Physics.SphereCast(ray,
                              this._settings.boundsRadius,
                              this._settings.collisionAvoidDst,
                              this._settings.obstacleMask)) {
        return dir;
      }
    }
   */
  }
}