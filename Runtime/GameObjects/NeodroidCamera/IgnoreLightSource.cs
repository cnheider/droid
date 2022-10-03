namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class IgnoreLightSource : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] bool _automatically_add_lights_without_infrared_component = false;
    [UnityEngine.SerializeField] bool _ignore_infrared_if_empty = true;

    [UnityEngine.SerializeField] UnityEngine.Light[] _lights_to_ignore = null;

    // Use this for initialization
    void Start() {
      if (this._lights_to_ignore == null
          || this._lights_to_ignore.Length == 0 && this._ignore_infrared_if_empty) {
        var infrared_light_sources = FindObjectsOfType<InfraredLightSource>();
        var lights = new System.Collections.Generic.List<UnityEngine.Light>();
        for (var index = 0; index < infrared_light_sources.Length; index++) {
          var ils = infrared_light_sources[index];
          lights.Add(item : ils.GetComponent<UnityEngine.Light>());
        }

        this._lights_to_ignore = lights.ToArray();
      } else if (this._automatically_add_lights_without_infrared_component) {
        var lights = System.Linq.Enumerable.ToList(source : this._lights_to_ignore);
        var d = FindObjectsOfType<UnityEngine.Light>();
        for (var index = 0; index < d.Length; index++) {
          var light1 = d[index];
          if (!light1.gameObject.GetComponent<InfraredLightSource>()) {
            if (!lights.Exists(l => l != null && light1.GetHashCode() == l.GetHashCode())) {
              lights.Add(item : light1);
            }
          }
        }

        this._lights_to_ignore = lights.ToArray();
      }
    }

    // Update is called once per frame
    void Update() { }

    void OnPostRender() {
      if (this._lights_to_ignore != null) {
        for (var index = 0; index < this._lights_to_ignore.Length; index++) {
          var l = this._lights_to_ignore[index];
          if (l) {
            l.enabled = true;
          }
        }
      }
    }

    void OnPreCull() {
      if (this._lights_to_ignore != null) {
        for (var index = 0; index < this._lights_to_ignore.Length; index++) {
          var l = this._lights_to_ignore[index];
          if (l) {
            l.enabled = false;
          }
        }
      }
    }

    void OnPreRender() {
      if (this._lights_to_ignore != null) {
        for (var index = 0; index < this._lights_to_ignore.Length; index++) {
          var l = this._lights_to_ignore[index];
          if (l) {
            l.enabled = false;
          }
        }
      }
    }
  }
}