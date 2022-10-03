#if UNITY_EDITOR
namespace droid.Editor.ScriptableObjects {
  /// <summary>
  /// </summary>
  public static class CreatePlayerMotions {
    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorScriptableObjectMenuPath._ScriptableObjectMenuPath
                                     + "PlayerMotions")]
    public static void CreatePlayerMotionsAsset() {
      var asset = UnityEngine.ScriptableObject
                             .CreateInstance<droid.Runtime.ScriptableObjects.PlayerMotions>();

      UnityEditor.AssetDatabase.CreateAsset(asset : asset,
                                            path :
                                            $"{droid.Editor.Windows.EditorWindowMenuPath._NewAssetPath}NewPlayerMotions.asset");
      UnityEditor.AssetDatabase.SaveAssets();

      UnityEditor.EditorUtility.FocusProjectWindow();

      UnityEditor.Selection.activeObject = asset;
    }

    #region Nested type: CreatePlayerMotionsWizard

    /// <summary>
    /// </summary>
    public class CreatePlayerMotionsWizard : UnityEditor.ScriptableWizard {
      const float WINDOW_WIDTH = 260, WINDOW_HEIGHT = 500;

      /// <summary>
      /// </summary>
      [UnityEngine.HeaderAttribute("Actuators to generate motions for")]
      public droid.Runtime.Prototyping.Actors.Actor[] actors;

      void Awake() {
        var icon =
            UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(assetPath :
              $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/table.png");
        this.minSize = this.maxSize = new UnityEngine.Vector2(x : WINDOW_WIDTH, y : WINDOW_HEIGHT);
        this.titleContent = new UnityEngine.GUIContent(text : this.titleContent.text, image : icon);
      }

      void OnWizardCreate() {
        var asset = CreateInstance<droid.Runtime.ScriptableObjects.PlayerMotions>();
        var motionCount = 0;

        foreach (var actor in this.actors) {
          foreach (var actuator in actor.Actuators) {
            motionCount += ((droid.Runtime.Prototyping.Actuators.Actuator)actuator.Value).InnerMotionNames
                .Length;
          }
        }

        asset._Motions = new droid.Runtime.ScriptableObjects.PlayerMotion[motionCount];
        var i = 0;
        foreach (var actor in this.actors) {
          foreach (var actuator in actor.Actuators) {
            for (var j = 0;
                 j < ((droid.Runtime.Prototyping.Actuators.Actuator)actuator.Value).InnerMotionNames.Length;
                 j++, i++) {
              asset._Motions[i] = new droid.Runtime.ScriptableObjects.PlayerMotion {
                                      _Actor = actor.Identifier,
                                      _Actuator =
                                          ((droid.Runtime.Prototyping.Actuators.Actuator)actuator.Value)
                                          .InnerMotionNames[j]
                                  };
            }
          }
        }

        UnityEditor.AssetDatabase.CreateAsset(asset : asset,
                                              path :
                                              $"{droid.Editor.Windows.EditorWindowMenuPath._NewAssetPath}NewPlayerMotions.asset");
        UnityEditor.AssetDatabase.SaveAssets();

        UnityEditor.EditorUtility.FocusProjectWindow();

        UnityEditor.Selection.activeObject = asset;
      }

      [UnityEditor.MenuItem(itemName : EditorScriptableObjectMenuPath._ScriptableObjectMenuPath
                                       + "PlayerMotions (Wizard)")]
      static void Init() {
        var window = CreateWindow<CreatePlayerMotionsWizard>("Create Player Motions...");
        window.Show();
      }
    }

    #endregion
  }
}
#endif