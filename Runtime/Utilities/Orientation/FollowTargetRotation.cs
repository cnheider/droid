namespace droid.Runtime.Utilities.Orientation {
  [UnityEngine.ExecuteInEditMode]
  public class FollowTargetRotation : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.Vector3 _forward;

    public UnityEngine.Quaternion rot;

    /// <summary>
    /// </summary>
    public UnityEngine.Transform targetPose;

    void LateUpdate() {
      if (this.targetPose) {
        this.rot = this.targetPose.rotation;

        var projection_on_plane =
            UnityEngine.Vector3.ProjectOnPlane(vector : this.targetPose.up,
                                               planeNormal : UnityEngine.Vector3.up);

        var rot = this.transform.rotation;
        var normalised_proj = projection_on_plane.normalized;
        var view = UnityEngine.Quaternion.Euler(0, -90, 0) * normalised_proj;
        if (view != UnityEngine.Vector3.zero) {
          rot.SetLookRotation(view : view, up : UnityEngine.Vector3.down);
        }

        this.transform.rotation = rot;
      }
    }
  }
}