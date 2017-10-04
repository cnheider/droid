using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentConfigurator : MonoBehaviour {

  public int _reset_frames = 10;

  Vector3[] _reset_positions;
  Quaternion[] _reset_rotations;
  GameObject[] _game_objects;
  Configurable[] _configurables;


	// Use this for initialization
	void Start () {
    _configurables = FindObjectsOfType<Configurable> ();
    _game_objects = FindObjectsOfType<GameObject> ();
    _reset_positions = new Vector3[_game_objects.Length];
    _reset_rotations = new Quaternion[_game_objects.Length];
    for (int i = 0; i < _game_objects.Length; i++) {
      _reset_positions[i] = _game_objects [i].transform.position;
      _reset_rotations[i] = _game_objects [i].transform.rotation;
    }
  }
	
  public void ResetEnvironment(){
    for (int resets = 0; resets < _reset_frames; resets++) { 
      for (int i = 0; i < _game_objects.Length; i++) {
        var rigid_body = _game_objects [i].GetComponent<Rigidbody> ();
        if (rigid_body)
          rigid_body.Sleep ();
        _game_objects [i].transform.position = _reset_positions [i];
        _game_objects [i].transform.rotation = _reset_rotations [i];
        if (rigid_body)
          rigid_body.WakeUp ();
      }
    }
  }

  public void Configure(string configuration){
    foreach (var configurable in _configurables) {
      configurable.Configure (configuration);
    }
  }
}
