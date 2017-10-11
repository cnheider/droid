using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Configuration;

namespace Neodroid.NeodroidEnvironment.Managers {
  public class EnvironmentManager : MonoBehaviour {

    #region PublicMembers

    public int _frames_spent_resetting = 10;
    public bool _wait_for_reaction_every_frame = false;
    public CoordinateSystem _coordinate_system = CoordinateSystem.GlobalCoordinates;
    public Transform _coordinate_reference_point;

    #endregion

    #region PrivateMembers

    Vector3[] _reset_positions;
    Quaternion[] _reset_rotations;
    GameObject[] _game_objects;
    Configurable[] _configurables;

    #endregion

    #region UnityCallbacks

    void Start () {
      _configurables = FindObjectsOfType<Configurable> ();
      _game_objects = FindObjectsOfType<GameObject> ();
      _reset_positions = new Vector3[_game_objects.Length];
      _reset_rotations = new Quaternion[_game_objects.Length];
      for (int i = 0; i < _game_objects.Length; i++) {
        _reset_positions [i] = _game_objects [i].transform.position;
        _reset_rotations [i] = _game_objects [i].transform.rotation;
      }
    }

    void FixedUpdate () {
      if (_wait_for_reaction_every_frame) {
        PauseEnviroment ();
      }
    }

    #endregion

    #region PublicMethods

    public Vector3 TransformPosition (Vector3 position) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        return _coordinate_reference_point.transform.InverseTransformPoint (position);
      } else {
        return position;
      }
    }

    public Vector3 TransformDirection (Vector3 direction) {
      if (_coordinate_system == CoordinateSystem.RelativeToReferencePoint) {
        return _coordinate_reference_point.transform.InverseTransformDirection (direction);
      } else {
        return direction;
      }
    }

    public void Step () {
      ResumeEnvironment ();
    }

    public void ResetEnvironment () {
      for (int resets = 0; resets < _frames_spent_resetting; resets++) { 
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

    public void Configure (string configuration) {
      foreach (var configurable in _configurables) {
        configurable.Configure (configuration);
      }
    }

    public bool IsEnvironmentPaused () {
      return Time.timeScale == 0;
    }

    void PauseEnviroment () {
      Time.timeScale = 0;
    }

    void ResumeEnvironment () {
      Time.timeScale = 1;
    }

    #endregion
  }
}
