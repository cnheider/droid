﻿#if UNITY_EDITOR

namespace droid.Editor.Utilities {
  /// <summary>
  ///   Class used to serialize a reference to a scene asset that can be used
  ///   at runtime in a build, when the asset can no longer be directly
  ///   referenced. This caches the scene name based on the SceneAsset to use
  ///   at runtime to load.
  /// </summary>
  [System.SerializableAttribute]
  public class SceneReference : UnityEngine.ISerializationCallbackReceiver {
    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    public UnityEditor.SceneAsset scene;
    #endif

    [UnityEngine.SerializeField] bool sceneEnabled;

    [UnityEngine.SerializeField] int sceneIndex = -1;

    /// <summary>
    /// </summary>
    [UnityEngine.TooltipAttribute("The name of the referenced scene. This may be used at runtime to load the scene.")]
    public string sceneName;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void OnBeforeSerialize() {
      #if UNITY_EDITOR
      if (this.scene != null) {
        var scene_asset_path = UnityEditor.AssetDatabase.GetAssetPath(assetObject : this.scene);
        var scene_asset_guid = UnityEditor.AssetDatabase.AssetPathToGUID(path : scene_asset_path);

        var scenes = UnityEditor.EditorBuildSettings.scenes;

        this.sceneIndex = -1;
        for (var i = 0; i < scenes.Length; i++) {
          if (scenes[i].guid.ToString() == scene_asset_guid) {
            this.sceneIndex = i;
            this.sceneEnabled = scenes[i].enabled;
            if (scenes[i].enabled) {
              this.sceneName = this.scene.name;
            }

            break;
          }
        }
      } else {
        this.sceneName = "";
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public void OnAfterDeserialize() { }

    void ValidateScene() {
      if (string.IsNullOrEmpty(value : this.sceneName)) {
        throw new SceneLoadException("No scene specified.");
      }

      if (this.sceneIndex < 0) {
        throw new SceneLoadException(message : $"Scene {this.sceneName} is not in the build settings");
      }

      if (!this.sceneEnabled) {
        throw new SceneLoadException(message : $"Scene {this.sceneName} is not enabled in the build settings");
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="mode"></param>
    public void LoadScene(UnityEngine.SceneManagement.LoadSceneMode mode =
                              UnityEngine.SceneManagement.LoadSceneMode.Single) {
      this.ValidateScene();
      UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName : this.sceneName, mode : mode);
    }

    /// <summary>
    ///   Exception that is raised when there is an issue resolving and
    ///   loading a scene reference.
    /// </summary>
    public class SceneLoadException : System.Exception {
      public SceneLoadException(string message) : base(message : message) { }
    }
  }
}
#endif