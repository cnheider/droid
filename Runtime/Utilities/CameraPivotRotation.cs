namespace droid.Runtime.Utilities {
  /// <summary>
  /// </summary>
  public class CameraPivotRotation : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    public float rotateSpeed = 1f;

    /// <summary>
    /// </summary>
    public float scrollSpeed = 200f;

    /// <summary>
    /// </summary>
    public UnityEngine.Transform pivot;

    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.SphericalSpace _sphericalSpace;

    void Start() {
      this._sphericalSpace =
          droid.Runtime.Structs.Space.SphericalSpace.FromCartesian(cartesian_coordinate :
                                                                   this.transform.position,
                                                                   3f,
                                                                   10f,
                                                                   0f,
                                                                   max_polar : UnityEngine.Mathf.PI * 2f,
                                                                   0f,
                                                                   max_elevation : UnityEngine.Mathf.PI / 4f);
      // Initialize position
      this.transform.position = this._sphericalSpace.ToCartesian() + this.pivot.position;
    }

    void Update() {
      var kh = UnityEngine.Input.GetAxis("Horizontal");
      var kv = UnityEngine.Input.GetAxis("Vertical");

      var any_mouse_button = UnityEngine.Input.GetMouseButton(0)
                             | UnityEngine.Input.GetMouseButton(1)
                             | UnityEngine.Input.GetMouseButton(2);
      var mh = any_mouse_button ? UnityEngine.Input.GetAxis("Mouse X") : 0f;
      var mv = any_mouse_button ? UnityEngine.Input.GetAxis("Mouse Y") : 0f;

      var h = kh * kh > mh * mh ? kh : mh;
      var v = kv * kv > mv * mv ? kv : mv;

      if (h * h > .1f || v * v > .1f) {
        this.transform.position =
            this._sphericalSpace.Rotate(polar_delta : h * this.rotateSpeed * UnityEngine.Time.deltaTime,
                                        elevation_delta : v * this.rotateSpeed * UnityEngine.Time.deltaTime)
                .ToCartesian()
            + this.pivot.position;
      }

      var sw = -UnityEngine.Input.GetAxis("Mouse ScrollWheel");
      if (sw * sw > UnityEngine.Mathf.Epsilon) {
        this.transform.position =
            this._sphericalSpace
                .TranslateRadius(scroll_speed : sw * UnityEngine.Time.deltaTime * this.scrollSpeed)
                .ToCartesian()
            + this.pivot.position;
      }

      this.transform.LookAt(worldPosition : this.pivot.position);
    }
  }
}