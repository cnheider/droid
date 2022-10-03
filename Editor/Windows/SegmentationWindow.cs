#if UNITY_EDITOR
namespace droid.Editor.Windows {
  /// <summary>
  /// </summary>
  public class SegmentationWindow : UnityEditor.EditorWindow {
    [UnityEngine.SerializeField] droid.Runtime.Structs.ColorByInstance[] colorsByInstance;

    [UnityEngine.SerializeField] droid.Runtime.Structs.ColorByCategory[] _colorsByCategory;
    UnityEngine.Texture _icon;

    UnityEngine.Vector2 _scroll_position;

    void OnEnable() {
      this._icon =
          (UnityEngine.Texture2D)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath :
            $"{NeodroidSettings.Current.NeodroidImportLocationProp}Gizmos/Icons/color_wheel.png",
            type : typeof(UnityEngine.Texture2D));
      this.titleContent =
          new UnityEngine.GUIContent("Neo:Seg", image : this._icon, "Window for segmentation");
    }

    void OnGUI() {
      UnityEngine.GUILayout.Label("Segmentation Colors", style : UnityEditor.EditorStyles.boldLabel);
      var serialised_object = new UnityEditor.SerializedObject(obj : this);
      this._scroll_position =
          UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition : this._scroll_position);
      UnityEditor.EditorGUILayout.BeginVertical("Box");
      UnityEngine.GUILayout.Label("By Tag");
      var material_changers_by_tag =
          FindObjectsOfType<droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Obsolete.
              ChangeMaterialOnRenderByTag>();
      foreach (var material_changer_by_tag in material_changers_by_tag) {
        this._colorsByCategory = material_changer_by_tag.ColorsByCategory;
        if (this._colorsByCategory != null) {
          var tag_colors_property = serialised_object.FindProperty("_segmentation_colors_by_tag");
          UnityEditor.EditorGUILayout.PropertyField(property : tag_colors_property,
                                                    label : new
                                                        UnityEngine.GUIContent(text : material_changer_by_tag
                                                            .name),
                                                    true); // True means show children
          material_changer_by_tag._Replace_Untagged_Color =
              UnityEditor.EditorGUILayout.Toggle("  -  Replace untagged colors",
                                                 value : material_changer_by_tag._Replace_Untagged_Color);
          material_changer_by_tag._Untagged_Color =
              UnityEditor.EditorGUILayout.ColorField("  -  Untagged color",
                                                     value : material_changer_by_tag._Untagged_Color);
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();

      /*var material_changer = FindObjectOfType<ChangeMaterialOnRenderByTag> ();
    if(material_changer){
      _segmentation_colors_by_game_object = material_changer.SegmentationColors;
      SerializedProperty game_object_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_game_object");
      EditorGUILayout.PropertyField(tag_colors_property, true); // True means show children
    }*/
      UnityEditor.EditorGUILayout.BeginVertical("Box");
      UnityEngine.GUILayout.Label("By Instance (Not changable, only for inspection) ");
      var material_changers_by_instance =
          FindObjectsOfType<droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Obsolete.
              ChangeMaterialOnRenderByInstance>();
      foreach (var material_changer_by_instance in material_changers_by_instance) {
        this.colorsByInstance = material_changer_by_instance.InstanceColors;
        if (this.colorsByInstance != null) {
          var instance_colors_property = serialised_object.FindProperty("_segmentation_colors_by_instance");
          UnityEditor.EditorGUILayout.PropertyField(property : instance_colors_property,
                                                    label : new
                                                        UnityEngine.GUIContent(text :
                                                          material_changer_by_instance.name),
                                                    true); // True means show children
        }
      }

      UnityEditor.EditorGUILayout.EndVertical();
      UnityEditor.EditorGUILayout.EndScrollView();
      serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
    }

    /// <summary>
    /// </summary>
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._WindowMenuPath + "SegmentationWindow")]
    [UnityEditor.MenuItem(itemName : EditorWindowMenuPath._ToolMenuPath + "SegmentationWindow")]
    public static void ShowWindow() {
      GetWindow(t : typeof(SegmentationWindow)); //Show existing window instance. If one doesn't exist, make one.
    }
  }
}
#endif