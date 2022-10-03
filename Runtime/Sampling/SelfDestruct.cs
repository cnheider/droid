namespace droid.Runtime.Sampling {
  public class SelfDestruct : UnityEngine.MonoBehaviour {
    float _spawn_time;
    public float LifeTime { get; set; } = 10f;

    void Awake() { this._spawn_time = UnityEngine.Time.time; }

    void Update() {
      if (this._spawn_time + this.LifeTime < UnityEngine.Time.time) {
        Destroy(obj : this.gameObject);
      }
    }
  }
}