using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.AnimatedValues;

public class CameraSynchronisationWindow : EditorWindow {

  [MenuItem ("Neodroid/CameraSynchronisationWindow")]
  public static void ShowWindow () {
    EditorWindow.GetWindow (typeof(CameraSynchronisationWindow));      //Show existing window instance. If one doesn't exist, make one.
  }

  SynchroniseCameraProperties[] _cameras;
  bool[] _show_camera_properties;

  

  void OnEnable()    {
    _cameras = FindObjectsOfType<SynchroniseCameraProperties> ();
    Setup ();
  }

  void Setup(){
    _show_camera_properties = new bool[_cameras.Length];
    for(int i = 0; i < _cameras.Length; i++){
      _show_camera_properties[i] = false;
    }
  }

  void OnGUI () {
    _cameras = FindObjectsOfType<SynchroniseCameraProperties> ();
    SerializedObject serialised_object = new SerializedObject(this);
    if(_show_camera_properties != null){
    for(int i = 0; i < _show_camera_properties.Length; i++){
        _show_camera_properties[i] = EditorGUILayout.Foldout(_show_camera_properties[i],_cameras[i].name);
        if ( _show_camera_properties[i]){
          EditorGUILayout.BeginVertical("Box");
          _cameras[i]._orthographic_size = EditorGUILayout.Toggle ("Synchronise Orthographic Size", _cameras[i]._orthographic_size);
          _cameras[i]._near_clip_plane = EditorGUILayout.Toggle ("Synchronise Near Clip Plane", _cameras[i]._near_clip_plane);
          _cameras[i]._far_clip_plane = EditorGUILayout.Toggle ("Synchronise Far Clip Plane", _cameras[i]._far_clip_plane);
          EditorGUILayout.EndVertical ();
        }
    }
    }

    serialised_object.ApplyModifiedProperties(); // Remember to apply modified properties
  }

      public void OnInspectorUpdate()
    {
        this.Repaint();
  }
}

