namespace droid.Runtime.Utilities.Procedural {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class GameObjectCloner : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.GameObject[] _clones = null;
    [UnityEngine.SerializeField] UnityEngine.Vector3 _initial_offset = new UnityEngine.Vector3(0, 0, 0);

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 99)]
    int _num_clones = 0;

    [UnityEngine.SerializeField] UnityEngine.Vector3 _offset = new UnityEngine.Vector3(20, 0, 20);
    [UnityEngine.SerializeField] UnityEngine.GameObject _prefab = null;

    void Start() { this.InstantiateClones(); }

    void Update() {
      if (this._num_clones != this._clones.Length) {
        this.InstantiateClones();
      }
    }

    void InstantiateClones() {
      if (this._clones.Length > 0) {
        this.ClearClones();
      }

      var clone_id = 0;
      this._clones = new UnityEngine.GameObject[this._num_clones];
      if (this._prefab) {
        var clone_coords = NeodroidUtilities.SnakeSpaceFillingGenerator(length : this._num_clones);
        foreach (var c in clone_coords) {
          var go = Instantiate(original : this._prefab,
                               position : this._initial_offset
                                          + UnityEngine.Vector3.Scale(a : this._offset, b : c),
                               rotation : UnityEngine.Quaternion.identity,
                               parent : this.transform);
          go.name = $"{go.name}{clone_id}";
          this._clones[clone_id] = go;
          clone_id++;
        }
      }
    }

    void ClearClones() {
      foreach (var clone in this._clones) {
        Destroy(obj : clone);
      }
    }
  }
}