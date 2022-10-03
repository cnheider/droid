namespace droid.Runtime.GameObjects.NeodroidCamera.Experimental {
  /// <summary>
  /// </summary>
  public class CreateStereoCubemaps : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.RenderTexture cubemapLeftEye;

    [UnityEngine.SerializeField] UnityEngine.RenderTexture cubemapRightEye;
    [UnityEngine.SerializeField] UnityEngine.RenderTexture cubemapEquirect;
    [UnityEngine.SerializeField] bool renderStereo = false;
    [UnityEngine.SerializeField] float stereoSeparation = 0.064f;

    [UnityEngine.SerializeField] UnityEngine.Camera _cam;

    void Start() {
      this._cam = this.GetComponent<UnityEngine.Camera>();

      if (this._cam == null) {
        this._cam = this.GetComponentInParent<UnityEngine.Camera>();
      }
    }

    void LateUpdate() {
      if (this._cam == null) {
        UnityEngine.Debug.Log("stereo 360 capture node has no camera or parent camera");
      }

      if (this.renderStereo) {
        this._cam.stereoSeparation = this.stereoSeparation;
        this._cam.RenderToCubemap(cubemap : this.cubemapLeftEye,
                                  63,
                                  stereoEye : UnityEngine.Camera.MonoOrStereoscopicEye.Left);
        this._cam.RenderToCubemap(cubemap : this.cubemapRightEye,
                                  63,
                                  stereoEye : UnityEngine.Camera.MonoOrStereoscopicEye.Right);
      } else {
        this._cam.RenderToCubemap(cubemap : this.cubemapLeftEye,
                                  63,
                                  stereoEye : UnityEngine.Camera.MonoOrStereoscopicEye.Mono);
      }

      //optional: convert cubemaps to equirect
      if (this.cubemapEquirect != null) {
        if (this.renderStereo) {
          this.cubemapLeftEye.ConvertToEquirect(equirect : this.cubemapEquirect,
                                                eye : UnityEngine.Camera.MonoOrStereoscopicEye.Left);
          this.cubemapRightEye.ConvertToEquirect(equirect : this.cubemapEquirect,
                                                 eye : UnityEngine.Camera.MonoOrStereoscopicEye.Right);
        } else {
          this.cubemapLeftEye.ConvertToEquirect(equirect : this.cubemapEquirect);
        }
      }
    }
  }
}