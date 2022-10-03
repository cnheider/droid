namespace droid.Runtime.Prototyping.EnvironmentListener {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  public class CameraFitter : EnvironmentListener {
    [UnityEngine.SerializeField] droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox bb;
    [UnityEngine.SerializeField] UnityEngine.Collider _collider;
    [UnityEngine.SerializeField] SourceEnum _source_enum = SourceEnum.Collider_;
    [UnityEngine.SerializeField] float margin = 0f;
    [UnityEngine.SerializeField] FitModeEnum _fit_mode_enum = FitModeEnum.Zoom_;
    UnityEngine.Camera _camera;

    public override void PreSetup() {
      base.PreSetup();
      if (!this.bb) {
        this.bb = FindObjectOfType<droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox>();
      }

      if (!this._collider) {
        this._collider = FindObjectOfType<UnityEngine.Collider>();
      }

      if (!this._camera) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }
    }

    /// <summary>
    /// </summary>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public override void PostTick() {
      base.PostTick();
      switch (this._source_enum) {
        case SourceEnum.Bb_:
          if (this.bb) {
            this._camera.transform.LookAt(target : this.bb.transform);
            var radius =
                droid.Runtime.GameObjects.BoundingBoxes.Experimental.Unused.BoundingBoxUtilities
                     .MaxDim(this.bb.Bounds.extents);
            switch (this._fit_mode_enum) {
              case FitModeEnum.Zoom_:
                this._camera.LookAtZoomToInstant(radius, this.bb.transform.position, this.margin);
                break;
              case FitModeEnum.Move_:
                this._camera.MoveToInstant(radius, this.bb.transform.position, this.margin);
                break;
              default: throw new System.ArgumentOutOfRangeException();
            }
          }

          break;
        case SourceEnum.Collider_:
          if (this._collider) {
            this._camera.transform.LookAt(target : this.bb.transform);
            var radius =
                droid.Runtime.GameObjects.BoundingBoxes.Experimental.Unused.BoundingBoxUtilities
                     .MaxDim(this._collider.bounds.extents);
            switch (this._fit_mode_enum) {
              case FitModeEnum.Zoom_:
                this._camera.LookAtZoomToInstant(radius, this._collider.transform.position, this.margin);
                break;
              case FitModeEnum.Move_:
                this._camera.MoveToInstant(radius, this._collider.transform.position, this.margin);
                break;
              default: throw new System.ArgumentOutOfRangeException();
            }
          }

          break;
        default: throw new System.ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// </summary>
    enum FitModeEnum {
      Zoom_,
      Move_
    }

    /// <summary>
    /// </summary>
    enum SourceEnum {
      Bb_,
      Collider_
    }
  }

  /// <summary>
  /// </summary>
  public static class CameraFittingUtilities {
    // cam - camera to use
    // center - screen pixel center
    // pixelHeight - height of the rectangle in pixels
    // time - time to take zooming
    static System.Collections.IEnumerator ZoomToLerp(this UnityEngine.Camera cam,
                                                     UnityEngine.Vector3 center,
                                                     float pixel_height,
                                                     float time) {
      var cam_tran = cam.transform;
      var ray = cam.ScreenPointToRay(pos : center);
      var end_rotation = UnityEngine.Quaternion.LookRotation(forward : ray.direction);
      var position = cam_tran.position;
      var end_position = ProjectPointOnPlane(plane_normal : cam_tran.forward,
                                             plane_point : position,
                                             point : ray.origin);
      var opp = UnityEngine.Mathf.Tan(f : cam.fieldOfView * 0.5f * UnityEngine.Mathf.Deg2Rad);
      opp *= pixel_height / UnityEngine.Screen.height;
      var end_fov = UnityEngine.Mathf.Atan(f : opp) * 2.0f * UnityEngine.Mathf.Rad2Deg;

      var timer = 0.0f;
      var start_rotation = cam_tran.rotation;
      var start_fov = cam.fieldOfView;
      var start_position = position;

      while (timer <= 1.0f) {
        var t = UnityEngine.Mathf.Sin(f : timer * UnityEngine.Mathf.PI * 0.5f);
        cam_tran.rotation = UnityEngine.Quaternion.Slerp(a : start_rotation, b : end_rotation, t : t);
        cam_tran.position = UnityEngine.Vector3.Lerp(a : start_position, b : end_position, t : t);
        cam.fieldOfView = UnityEngine.Mathf.Lerp(a : start_fov, b : end_fov, t : t);
        timer += UnityEngine.Time.deltaTime / time;
        yield return null;
      }

      cam_tran.rotation = end_rotation;
      cam_tran.position = end_position;
      cam.fieldOfView = end_fov;
    }

    // cam - camera to use
    // center - screen pixel center
    // pixelHeight - height of the rectangle in pixels
    public static void ZoomToInstant(this UnityEngine.Camera cam,
                                     UnityEngine.Vector2 center,
                                     float pixel_height) {
      var cam_tran = cam.transform;
      var ray = cam.ScreenPointToRay(pos : center);
      var end_rotation = UnityEngine.Quaternion.LookRotation(forward : ray.direction);
      var end_position = ProjectPointOnPlane(plane_normal : cam_tran.forward,
                                             plane_point : cam_tran.position,
                                             point : ray.origin);

      var opp = UnityEngine.Mathf.Tan(f : cam.fieldOfView * 0.5f * UnityEngine.Mathf.Deg2Rad);
      opp *= pixel_height / UnityEngine.Screen.height;
      var end_fov = UnityEngine.Mathf.Atan(f : opp) * 2.0f * UnityEngine.Mathf.Rad2Deg;

      cam_tran.rotation = end_rotation;
      cam_tran.position = end_position;
      cam.fieldOfView = end_fov;
    }

    public static void LookAtZoomToInstant(this UnityEngine.Camera cam,
                                           float radius,
                                           UnityEngine.Vector3 center,
                                           float margin = 1f) {
      cam.transform.LookAt(worldPosition : center, worldUp : UnityEngine.Vector3.up);
      var bound_sphere_radius = radius + margin;
      var distance = UnityEngine.Vector3.Distance(a : center, b : cam.transform.position);
      var end_fov = UnityEngine.Mathf.Atan(f : bound_sphere_radius * 0.5f / distance)
                    * 2.0f
                    * UnityEngine.Mathf.Rad2Deg;

      cam.fieldOfView = end_fov;
    }

    // cam - camera to use
    // center - screen pixel center
    // pixelHeight - height of the rectangle in pixels
    /// <summary>
    /// </summary>
    /// <param name="cam"></param>
    /// <param name="rect"></param>
    /// <param name="bb_position"></param>
    /// <param name="margin"></param>
    public static void MoveToInstant(this UnityEngine.Camera cam,
                                     float radius,
                                     UnityEngine.Vector3 bb_position,
                                     float margin = 1f) {
      var bound_sphere_radius = radius + margin;
      var fov = UnityEngine.Mathf.Deg2Rad * cam.fieldOfView;
      var cam_distance = bound_sphere_radius * .5f / UnityEngine.Mathf.Tan(f : fov * .5f);

      var transform = cam.transform;
      transform.position = bb_position - transform.forward * cam_distance;
    }

    public static UnityEngine.Vector3 ProjectPointOnPlane(UnityEngine.Vector3 plane_normal,
                                                          UnityEngine.Vector3 plane_point,
                                                          UnityEngine.Vector3 point) {
      plane_normal.Normalize();
      var distance = -UnityEngine.Vector3.Dot(lhs : plane_normal.normalized, point - plane_point);
      return point + plane_normal * distance;
    }
  }
}