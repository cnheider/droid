using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeMaterialOnRenderByInstance : MonoBehaviour {
 
  Dictionary<GameObject, Color> _instance_colors;
  LinkedList<Color>[] _original_colors;
  Renderer[] _all_renders;

  public Dictionary<GameObject, Color> InstanceColors{
    get{return _instance_colors;}
  }

  // Use this for initialization
  void Start () {
    Setup ();
  }

  void Awake(){
    Setup ();
  
  }

  // Update is called once per frame
  void Update () {
    if (_instance_colors == null)
      Setup ();
    else if (_instance_colors.Keys.Count != FindObjectsOfType<Renderer> ().Length) {
      Setup ();
    }
  }

  void Setup () {
    _all_renders = FindObjectsOfType<Renderer> ();

    _instance_colors = new Dictionary<GameObject, Color> (_all_renders.Length);
    foreach (Renderer renderer in _all_renders) {
      _instance_colors.Add(renderer.gameObject, Random.ColorHSV ());
    }
  }

  void Change () {
    _original_colors = new LinkedList<Color>[_all_renders.Length];
    for (int i = 0; i < _original_colors.Length; i++) {
      _original_colors [i] = new LinkedList<Color> ();
    }

    for (int i = 0; i < _all_renders.Length; i++) {
      foreach (var mat in _all_renders[i].materials) {
        _original_colors [i].AddFirst (mat.color);
        mat.color = _instance_colors [_all_renders [i].gameObject];
      }
      /*else if(true){
          int j = 0;
          foreach (var mat in _all_renders[i].sharedMaterials) {
            _original_colors [i].AddFirst (mat.color);
            var temporary_material = new Material (mat);
            temporary_material.color = _tag_colors [_all_renders [i].tag];
            _all_renders[i].sharedMaterials[j] = temporary_material;
            j++;
          }*/
    }
      
  }

  void Restore () {
    for (int i = 0; i < _all_renders.Length; i++) {
      foreach (var mat in _all_renders[i].materials) {
        mat.color = _original_colors [i].Last.Value;
        _original_colors [i].RemoveLast ();
      }
    }
        
      
  }

  void OnPreRender () { // change
    Change ();
  }

  void OnPostRender () { // change back
    Restore ();
  }



}
