using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SegmentationWindow : EditorWindow {

  [MenuItem ("Neodroid/SegmentationWindow")]
  public static void ShowWindow () {
    EditorWindow.GetWindow (typeof(SegmentationWindow));      //Show existing window instance. If one doesn't exist, make one.
  }

  public SegmentationColorByTag[] _segmentation_colors_by_tag;
  public SegmentationColorByInstance[] _segmentation_colors_by_instance;

  void OnGUI () {
    GUILayout.Label ("Segmentation Colors", EditorStyles.boldLabel);
    SerializedObject serialised_object = new SerializedObject(this);
   
    GUILayout.Label ("By Tag");
    var material_changers_by_tag = FindObjectsOfType<ChangeMaterialOnRenderByTag> ();
    foreach(ChangeMaterialOnRenderByTag material_changer_by_tag in material_changers_by_tag){
      _segmentation_colors_by_tag = material_changer_by_tag.SegmentationColorsByTag;
      SerializedProperty tag_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_tag");
      EditorGUILayout.PropertyField(tag_colors_property, new GUIContent(material_changer_by_tag.name), true); // True means show children
      material_changer_by_tag._use_shared_materials = EditorGUILayout.Toggle ("  -  Use Shared Materials",material_changer_by_tag._use_shared_materials);
    }

    /*var material_changer = FindObjectOfType<ChangeMaterialOnRenderByTag> ();
    if(material_changer){
      _segmentation_colors_by_game_object = material_changer.SegmentationColors;
      SerializedProperty game_object_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_game_object");
      EditorGUILayout.PropertyField(tag_colors_property, true); // True means show children
    }*/

    GUILayout.Label ("By Instance (Not changable, only for inspection) ");
    var material_changers_by_instance = FindObjectsOfType<ChangeMaterialOnRenderByInstance> ();
    foreach(ChangeMaterialOnRenderByInstance material_changer_by_instance in material_changers_by_instance){
      _segmentation_colors_by_instance = material_changer_by_instance.InstanceColors;
      SerializedProperty instance_colors_property = serialised_object.FindProperty ("_segmentation_colors_by_instance");
      EditorGUILayout.PropertyField(instance_colors_property, new GUIContent(material_changer_by_instance.name), true); // True means show children
      material_changer_by_instance._use_shared_materials = EditorGUILayout.Toggle ("  -  Use Shared Materials",material_changer_by_instance._use_shared_materials);
    }

    serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
  }
}
