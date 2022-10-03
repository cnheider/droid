﻿#if UNITY_EDITOR
namespace droid.Editor.Windows {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class CameraSynchronisationWindow : UnityEditor.EditorWindow {
    droid.Runtime.GameObjects.NeodroidCamera.SynchroniseCameraProperties[] _cameras;

    UnityEngine.Texture _icon;
    UnityEngine.Vector2 _scroll_position;
    bool[] _show_camera_properties;

    /// <summary>
    /// </summary>
    void OnEnable() {
      this._cameras =
          FindObjectsOfType<droid.Runtime.GameObjects.NeodroidCamera.SynchroniseCameraProperties>();
      this.Setup();
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/arrow_refresh.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent = new UnityEngine.GUIContent("Neo:Sync",
                                                     image : this._icon,
                                                     "Window for controlling synchronisation of cameras");
    }

    /// <summary>
    /// </summary>
    void OnGUI() {
      this._cameras =
          FindObjectsOfType<droid.Runtime.GameObjects.NeodroidCamera.SynchroniseCameraProperties>();
      if (this._cameras.Length > 0) {
        var serialised_object = new UnityEditor.SerializedObject(obj : this);
        this._scroll_position =
            UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
        if (this._show_camera_properties != null) {
          for (var i = 0; i < this._show_camera_properties.Length; i++) {
            this._show_camera_properties[i] =
                UnityEditor.EditorGUILayout.Foldout(foldout : this._show_camera_properties[i],
                                                    content : this._cameras[i].name);
            if (this._show_camera_properties[i]) {
              UnityEditor.EditorGUILayout.BeginVertical("Box");
              /*
              this._cameras[i].SyncOrthographicSize =
                  EditorGUILayout.Toggle("Synchronise Orthographic Size",
                                         this._cameras[i].SyncOrthographicSize);
              this._cameras[i].SyncNearClipPlane =
                  EditorGUILayout.Toggle("Synchronise Near Clip Plane", this._cameras[i].SyncNearClipPlane);
              this._cameras[i].SyncFarClipPlane =
                  EditorGUILayout.Toggle("Synchronise Far Clip Plane", this._cameras[i].SyncFarClipPlane);
              this._cameras[i].SyncCullingMask =
                  EditorGUILayout.Toggle("Synchronise Culling Mask", this._cameras[i].SyncCullingMask);
              */
              UnityEditor.EditorGUILayout.EndVertical();
            }
          }
        }

        UnityEditor.EditorGUILayout.EndScrollView();
        serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
      }

      /*if (GUI.changed) {
      EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
      // Unity not tracking changes to properties of gameobject made through this window automatically and
      // are not saved unless other changes are made from a working inpector window
      }*/
    }

    /// <summary>
    /// </summary>
    public void OnInspectorUpdate() { this.Repaint(); }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "CameraSynchronisationWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "CameraSynchronisationWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(CameraSynchronisationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }

    /// <summary>
    /// </summary>
    void Setup() {
      this._show_camera_properties = new bool[this._cameras.Length];
      for (var i = 0; i < this._cameras.Length; i++) {
        this._show_camera_properties[i] = false;
      }
    }
  }
}

#endif