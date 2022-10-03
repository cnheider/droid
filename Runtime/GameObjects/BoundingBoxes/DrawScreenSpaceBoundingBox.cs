namespace droid.Runtime.GameObjects.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  [UnityEngine.ExecuteInEditMode]
  public class DrawScreenSpaceBoundingBox : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] bool _draw_label = true;
    [UnityEngine.SerializeField] NeodroidBoundingBox[] bounding_boxes = null;
    [UnityEngine.SerializeField] bool _cache_bounding_boxes = false;
    [UnityEngine.SerializeField] UnityEngine.GUISkin gui_style = null;
    [UnityEngine.SerializeField] bool _draw_coords = false;
    UnityEngine.Camera _camera = null;
    System.Collections.Generic.List<string> _names = new System.Collections.Generic.List<string>();

    System.Collections.Generic.List<UnityEngine.Rect> _rects =
        new System.Collections.Generic.List<UnityEngine.Rect>();

    void Awake() {
      if (!this._camera) {
        this._camera = this.GetComponent<UnityEngine.Camera>();
      }

      if (!this.gui_style) {
        this.gui_style =
            System.Linq.Enumerable.First(source : UnityEngine.Resources
                                                             .FindObjectsOfTypeAll<UnityEngine.GUISkin>(),
                                         a => a.name == "BoundingBox");
      }

      this.bounding_boxes = FindObjectsOfType<NeodroidBoundingBox>();
    }

    void OnGUI() {
      if (this.gui_style) {
        UnityEngine.GUI.skin = this.gui_style;
      }

      this.Draw();
    }

    void Compute() {
      this._rects.Clear();
      this._names.Clear();

      if (!this._cache_bounding_boxes) {
        this.bounding_boxes = FindObjectsOfType<NeodroidBoundingBox>();
      }

      for (var index = 0; index < this.bounding_boxes.Length; index++) {
        var bb = this.bounding_boxes[index];
        if (this._camera.WorldToScreenPoint(position : bb.Bounds.center).z < 0) {
          return;
        }

        var a = bb.ScreenSpaceBoundingRect(a_camera : this._camera);

        this._rects.Add(item : a);
        this._names.Add(item : bb.name);
      }
    }

    void Draw() {
      this.Compute();
      var i = 0;
      for (var index = 0; index < this._rects.Count; index++) {
        var rect = this._rects[index : index];
        var text = "";
        if (this._draw_label) {
          text += $"{this._names[index : i]}";
        }

        if (this._draw_coords) {
          text += $"\n{rect}\n{rect.center}";
        }

        var a = rect;
        a.y = UnityEngine.Screen.height - (a.y + a.height);

        UnityEngine.GUI.Box(position : a, text : text);

        i++;
      }
    }
  }
}