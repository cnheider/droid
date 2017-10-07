using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.Models;
using Neodroid.Models.Motors;
using Neodroid.Models.Observers;

#if UNITY_EDITOR
using UnityEditor.AnimatedValues;
using UnityEditor;


public class DebugWindow : EditorWindow {

[MenuItem ("Neodroid/DebugWindow")]
public static void ShowWindow () {
EditorWindow.GetWindow (typeof(DebugWindow));      //Show existing window instance. If one doesn't exist, make one.
}

NeodroidAgent[] _agents;
bool _show_agents_debug = false;

Actor[] _actors;
bool _show_actors_debug= false;

Motor[] _motors;
bool _show_motors_debug= false;

Observer[] _observers;
bool _show_observers_debug= false;



void OnEnable() {
_agents = FindObjectsOfType<NeodroidAgent> ();
_actors = FindObjectsOfType<Actor> ();
_motors = FindObjectsOfType<Motor> ();
_observers = FindObjectsOfType<Observer> ();
}

void OnGUI () {
_agents = FindObjectsOfType<NeodroidAgent> ();
_actors = FindObjectsOfType<Actor> ();
_motors = FindObjectsOfType<Motor> ();
_observers = FindObjectsOfType<Observer> ();

_show_agents_debug = EditorGUILayout.Toggle ("Show agents debug messages", _show_agents_debug);
_show_actors_debug = EditorGUILayout.Toggle ("Show actors debug messages", _show_actors_debug);
_show_motors_debug = EditorGUILayout.Toggle ("Show motors debug messages", _show_motors_debug);
_show_observers_debug = EditorGUILayout.Toggle ("Show observers debug messages", _show_observers_debug);

    if(GUILayout.Button("Apply")){
        foreach(var agent in _agents){
        agent._debug = _show_agents_debug;
        }
        foreach(var actor in _actors){
        actor._debug = _show_actors_debug;
        }
        foreach(var motor in _motors){
        motor._debug = _show_motors_debug;
        }
        foreach (var observer in _observers) {
        observer._debug = _show_observers_debug;
        }
}
      
}

public void OnInspectorUpdate(){
this.Repaint();
}
}

#endif
