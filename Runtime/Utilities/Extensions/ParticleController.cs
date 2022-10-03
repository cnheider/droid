//[ExecuteInEditMode]

namespace droid.Runtime.Utilities.Extensions {
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.ParticleSystem))]
  public class ParticleController : UnityEngine.MonoBehaviour {
    UnityEngine.ParticleSystem _particle_system;

    // Use this for initialization
    void Start() { this._particle_system = this.GetComponent<UnityEngine.ParticleSystem>(); }

    // Update is called once per frame
    void Update() {
      if (UnityEngine.Input.GetKey(key : UnityEngine.KeyCode.Space)) {
        if (this._particle_system.isPlaying) {
          return;
        }

        this._particle_system.Play(true);
      } else {
        //_particle_system.Pause (true);
        this._particle_system.Stop(true);
      }
    }
  }
}