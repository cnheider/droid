#if UNITY_EDITOR

namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  ///   Editor for a scene reference that can display error prompts and offer
  ///   solutions when the scene is not valid.
  /// </summary>
  [UnityEditor.CustomPropertyDrawer(type : typeof(SceneReference))]
  public class SceneReferenceEditor : UnityEditor.PropertyDrawer {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <param name="property"></param>
    /// <param name="label"></param>
    public override void OnGUI(UnityEngine.Rect position,
                               UnityEditor.SerializedProperty property,
                               UnityEngine.GUIContent label) {
      label = UnityEditor.EditorGUI.BeginProperty(totalPosition : position,
                                                  label : label,
                                                  property : property);
      position = UnityEditor.EditorGUI.PrefixLabel(totalPosition : position, label : label);

      this.CacheProperties(property : property);
      this.UpdateSceneState();

      position = this.DisplayErrorsIfNecessary(position : position);

      UnityEditor.EditorGUI.BeginChangeCheck();
      UnityEditor.EditorGUI.PropertyField(position : position,
                                          property : this._scene,
                                          label : UnityEngine.GUIContent.none,
                                          false);
      if (UnityEditor.EditorGUI.EndChangeCheck()) {
        property.serializedObject.ApplyModifiedProperties();
        this.CacheProperties(property : property);
        this.UpdateSceneState();
        this.Validate();
      }

      UnityEditor.EditorGUI.EndProperty();
    }

    /// <summary>
    ///   Cache all used properties as local variables so that they can be
    ///   used by other methods. This needs to be called every frame since a
    ///   PropertyDrawer can be reused on different properties.
    /// </summary>
    /// <param name="property">Property to search through.</param>
    void CacheProperties(UnityEditor.SerializedProperty property) {
      this._scene = property.FindPropertyRelative("Scene");
      this._scene_name = property.FindPropertyRelative("SceneName");
      this._scene_index = property.FindPropertyRelative("sceneIndex");
      this._scene_enabled = property.FindPropertyRelative("sceneEnabled");
      this._scene_asset = this._scene.objectReferenceValue as UnityEditor.SceneAsset;

      if (this._scene_asset != null) {
        this._scene_asset_path =
            UnityEditor.AssetDatabase.GetAssetPath(assetObject : this._scene.objectReferenceValue);
        if (this._scene_asset_path != null) {
          this._scene_asset_guid = UnityEditor.AssetDatabase.AssetPathToGUID(path : this._scene_asset_path);
        }
      } else {
        this._scene_asset_path = null;
        this._scene_asset_guid = null;
      }
    }

    /// <summary>
    ///   Updates the scene index and enabled flags of a scene property by
    ///   scanning through the scenes in EditorBuildSettings.
    /// </summary>
    void UpdateSceneState() {
      if (this._scene_asset != null) {
        var scenes = UnityEditor.EditorBuildSettings.scenes;

        this._scene_index.intValue = -1;
        for (var i = 0; i < scenes.Length; i++) {
          if (scenes[i].guid.ToString() == this._scene_asset_guid) {
            if (this._scene_index.intValue != i) {
              this._scene_index.intValue = i;
            }

            this._scene_enabled.boolValue = scenes[i].enabled;
            if (scenes[i].enabled) {
              if (this._scene_name.stringValue != this._scene_asset.name) {
                this._scene_name.stringValue = this._scene_asset.name;
              }
            }

            break;
          }
        }
      } else {
        this._scene_name.stringValue = "";
      }
    }

    /// <summary>
    ///   Display a popup error message about the selected scene and respond
    ///   to the user choice by either fixing the issue in the build
    ///   settings, doing nothing, or opening the build settings.
    /// </summary>
    /// <param name="message">Message to display.</param>
    void DisplaySceneErrorPrompt(string message) {
      var scenes = UnityEditor.EditorBuildSettings.scenes;

      var choice = UnityEditor.EditorUtility.DisplayDialogComplex("Scene Not In Build",
                                                                  message : message,
                                                                  "Yes",
                                                                  "No",
                                                                  "Open Build Settings");

      if (choice == 0) {
        var new_count = this._scene_index.intValue < 0 ? scenes.Length + 1 : scenes.Length;
        var new_scenes = new UnityEditor.EditorBuildSettingsScene[new_count];
        System.Array.Copy(sourceArray : scenes, destinationArray : new_scenes, length : scenes.Length);

        if (this._scene_index.intValue < 0) {
          new_scenes[scenes.Length] =
              new UnityEditor.EditorBuildSettingsScene(path : this._scene_asset_path, true);
          this._scene_index.intValue = scenes.Length;
        }

        new_scenes[this._scene_index.intValue].enabled = true;

        UnityEditor.EditorBuildSettings.scenes = new_scenes;
      } else if (choice == 2) {
        UnityEditor.EditorApplication.ExecuteMenuItem("File/Build Settings...");
      }
    }

    /// <summary>
    ///   If there is anything wrong with the selected scene, this will
    ///   display an error icon that the user can click on for more info.
    /// </summary>
    /// <param name="position">
    ///   Full rect that will be used to draw the property.
    /// </param>
    /// <returns>
    ///   The rect that should be used to draw the rest of the property. If
    ///   there are no errors, this is the same as the input position Rect.
    ///   Otherwise, it will be the input rect adjusted to fit the error.
    /// </returns>
    UnityEngine.Rect DisplayErrorsIfNecessary(UnityEngine.Rect position) {
      if (this._error_style == null) {
        this._error_style = "CN EntryErrorIconSmall";
        this._error_tooltip = new UnityEngine.GUIContent("", "error");
      }

      if (this._scene_asset == null) {
        return position;
      }

      var warning_rect = new UnityEngine.Rect(source : position) {width = this._error_style.fixedWidth + 4};

      if (this._scene_index.intValue < 0) {
        this._error_tooltip.tooltip = _tooltip_scene_missing;
        position.xMin = warning_rect.xMax;
        if (UnityEngine.GUI.Button(position : warning_rect,
                                   content : this._error_tooltip,
                                   style : this._error_style)) {
          this.DisplaySceneErrorPrompt(message : _error_scene_missing);
        }
      } else if (!this._scene_enabled.boolValue) {
        this._error_tooltip.tooltip = _tooltip_scene_disabled;
        position.xMin = warning_rect.xMax;
        if (UnityEngine.GUI.Button(position : warning_rect,
                                   content : this._error_tooltip,
                                   style : this._error_style)) {
          this.DisplaySceneErrorPrompt(message : _error_scene_disabled);
        }
      }

      return position;
    }

    /// <summary>
    ///   Validate any new values in the scene property. This will display
    ///   popup errors if there are issues with the current value.
    /// </summary>
    void Validate() {
      if (this._scene_asset != null) {
        var scenes = UnityEditor.EditorBuildSettings.scenes;

        this._scene_index.intValue = -1;
        for (var i = 0; i < scenes.Length; i++) {
          if (scenes[i].guid.ToString() == this._scene_asset_guid) {
            if (this._scene_index.intValue != i) {
              this._scene_index.intValue = i;
            }

            if (scenes[i].enabled) {
              if (this._scene_name.stringValue != this._scene_asset.name) {
                this._scene_name.stringValue = this._scene_asset.name;
              }

              return;
            }

            break;
          }
        }

        if (this._scene_index.intValue >= 0) {
          this.DisplaySceneErrorPrompt(message : _error_scene_disabled);
        } else {
          this.DisplaySceneErrorPrompt(message : _error_scene_missing);
        }
      } else {
        this._scene_name.stringValue = "";
      }
    }

    #region -- Constants --------------------------------------------------

    const string _tooltip_scene_missing = "Scene is not in build settings.";

    const string _error_scene_missing =
        "You are refencing a scene that is not added to the build. Add it to the editor build settings now?";

    const string _tooltip_scene_disabled = "Scene is not enebled in build settings.";

    const string _error_scene_disabled =
        "You are refencing a scene that is not active the build. Enable it in the build settings now?";

    #endregion -- Constants -----------------------------------------------

    #region -- Private Variables ------------------------------------------

    UnityEditor.SerializedProperty _scene;
    UnityEditor.SerializedProperty _scene_name;
    UnityEditor.SerializedProperty _scene_index;
    UnityEditor.SerializedProperty _scene_enabled;
    UnityEditor.SceneAsset _scene_asset;
    string _scene_asset_path;
    string _scene_asset_guid;

    UnityEngine.GUIContent _error_tooltip;
    UnityEngine.GUIStyle _error_style;

    #endregion -- Private Variables ---------------------------------------
  }
}
#endif