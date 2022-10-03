namespace droid.Runtime.GameObjects {
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  public class SimulationRenderCamera : UnityEngine.MonoBehaviour {
    UnityEngine.Camera _camera;

    void Awake() { this._camera = this.GetComponent<UnityEngine.Camera>(); }

    internal void DisableCamera() {
      if (this._camera.enabled) {
        this._camera.enabled = false;
      }
    }

    internal void EnableCamera() {
      if (!this._camera.enabled) {
        this._camera.enabled = true;
      }
    }

    internal void Render() { this._camera.Render(); }
  }
}