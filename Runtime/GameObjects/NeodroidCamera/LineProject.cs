namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.LineRenderer))]
  [UnityEngine.ExecuteInEditMode]
  public class LineProject : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.Vector3 _direction = UnityEngine.Vector3.down;
    [UnityEngine.SerializeField] float _length = 30f;
    UnityEngine.LineRenderer _line_renderer = null;

    UnityEngine.Vector3 _old_pos = UnityEngine.Vector3.zero;

    void Awake() {
      this._line_renderer = this.GetComponent<UnityEngine.LineRenderer>();
      this.Project();
    }

    void Update() {
      if (UnityEngine.Application.isPlaying) {
        if (this.transform.position != this._old_pos) {
          this.Project();
        }
      }
    }

    void OnEnable() { this.Project(); }

    void Project() {
      var position = this.transform.position;
      if (UnityEngine.Physics.Raycast(origin : position,
                                      direction : this._direction,
                                      hitInfo : out var ray,
                                      maxDistance : this._length)) {
        this._line_renderer.SetPositions(positions : new[] {position, ray.point});
      }
    }
  }
}