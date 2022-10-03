namespace droid.Runtime.Sampling {
  public class ProjectileSpammer : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] string _assigned_tag = "Obstruction";

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(.0f, 3.0f)]
    float _life_time = 2f;

    [UnityEngine.SerializeField] UnityEngine.Vector2 _mass_range = new UnityEngine.Vector2(1f, 4f);
    [UnityEngine.SerializeField] float _projectile_multiplier = 100f;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(.0f, 1.0f)]
    float _scale_modifier = 0.2f;

    [UnityEngine.SerializeField] float _spawn_radius = 20f;
    [UnityEngine.SerializeField] float _spawn_rate = 0.5f;
    [UnityEngine.SerializeField] UnityEngine.Transform _target = null;
    float _last_spawn = 0f;

    void Update() {
      if (this._last_spawn + 1 / this._spawn_rate < UnityEngine.Time.time) {
        if (this._spawn_rate > 1) {
          for (var i = 0; i < this._spawn_rate; i++) {
            this.SpawnRandomProjectile();
          }
        } else {
          this.SpawnRandomProjectile();
        }
      }
    }

    void SpawnRandomProjectile() {
      var cube = UnityEngine.GameObject.CreatePrimitive(type : UnityEngine.PrimitiveType.Cube);
      cube.tag = this._assigned_tag;
      cube.transform.position =
          this._target.transform.position + UnityEngine.Random.onUnitSphere * this._spawn_radius;
      cube.transform.rotation = UnityEngine.Random.rotation;
      cube.transform.localScale = (UnityEngine.Vector3.one - UnityEngine.Random.insideUnitSphere)
                                  / 2
                                  * this._scale_modifier;
      var rb = cube.AddComponent<UnityEngine.Rigidbody>();
      rb.AddForce(force : (this._target.position - cube.transform.position) * this._projectile_multiplier);
      rb.AddTorque(torque : UnityEngine.Random.insideUnitSphere);
      rb.mass = UnityEngine.Random.Range(minInclusive : this._mass_range.x, maxInclusive : this._mass_range.y);
      var sf = cube.AddComponent<SelfDestruct>();
      sf.LifeTime = this._life_time;
    }
  }
}