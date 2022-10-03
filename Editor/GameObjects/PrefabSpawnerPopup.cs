#if UNITY_EDITOR
namespace droid.Editor.GameObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PrefabSpawnerPopup : UnityEditor.EditorWindow {
    static UnityEngine.Rect _rect = new UnityEngine.Rect(0,
                                                         0,
                                                         0,
                                                         0);

    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath + "SpawnPrefab", false, 10)]
    static void Init2() {
      try {
        UnityEditor.PopupWindow.Show(activatorRect : _rect, windowContent : new PrefabsPopup());
      } catch (UnityEngine.ExitGUIException) {
        //Debug.Log(e);
      }
    }
  }

  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class PrefabsPopup : UnityEditor.PopupWindowContent {
    UnityEngine.Vector2 _scroll_position;
    bool _updated_pos;
    int _x_size = 300;
    int _y_size = 200;

    public override UnityEngine.Vector2 GetWindowSize() {
      return new UnityEngine.Vector2(x : this._x_size, y : this._y_size);
    }

    public override void OnGUI(UnityEngine.Rect rect) {
      if (!this._updated_pos) {
        var mp = UnityEngine.Event.current.mousePosition;
        rect.x = mp.x;
        rect.y = mp.y;

        this.editorWindow.position = rect;
        this._updated_pos = true;
      }

      UnityEngine.GUILayout.Label("Spawn Prefab", style : UnityEditor.EditorStyles.boldLabel);

      // Supports the following syntax:
      // 't:type' syntax (e.g 't:Texture2D' will show Texture2D objects)
      // 'l:assetlabel' syntax (e.g 'l:architecture' will show assets with AssetLabel 'architecture')
      // 'ref[:id]:path' syntax (e.g 'ref:1234' will show objects that references the object with instanceID 1234)
      // 'v:versionState' syntax (e.g 'v:modified' will show objects that are modified locally)
      // 's:softLockState' syntax (e.g 's:inprogress' will show objects that are modified by anyone (except you))
      // 'a:area' syntax (e.g 'a:all' will s search in all assets, 'a:assets' will s search in assets folder only and 'a:packages' will s search in packages folder only)

      var prefabs = UnityEditor.AssetDatabase.FindAssets("t:Prefab a:all");

      this._scroll_position =
          UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
      UnityEditor.EditorGUILayout.BeginVertical();
      foreach (var prefab in prefabs) {
        var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid : prefab);
        //Debug.Log(path);
        var go = UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath : path,
                                                           type : typeof(UnityEngine.GameObject));
        if (path.Contains("Neodroid")) {
          if (UnityEngine.GUILayout.Button(text : go.name)) {
            UnityEngine.Object.Instantiate(original : go, parent : UnityEditor.Selection.activeTransform);
          }
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
      UnityEditor.EditorGUILayout.EndScrollView();
    }

    public override void OnOpen() {
      //Debug.Log("Popup opened: " + this);
    }

    public override void OnClose() {
      //Debug.Log("Popup closed: " + this);
    }
  }
}
#endif