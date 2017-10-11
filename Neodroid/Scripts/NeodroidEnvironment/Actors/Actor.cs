using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Agents;
using Neodroid.NeodroidEnvironment.Motors;
using System.Collections.Generic;
using UnityEngine;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.NeodroidEnvironment.Actors {
  public class Actor : MonoBehaviour, HasRegister<Motor> {
    public float[] _position;
    public float[] _rotation;
    public float[] _direction;

    public Dictionary<string, Motor> _motors;
  
    public NeodroidAgent _agent_game_object;
    public EnvironmentManager _environment_manager;

    public bool _debug = false;

    protected void Start () {
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      NeodroidFunctions.MaybeRegisterComponent (_agent_game_object, this);
      Setup ();
    }

    protected virtual void Setup () {
      if (!_environment_manager) {
        _environment_manager = FindObjectOfType<EnvironmentManager> ();
      }
    }

    private void Update () {
      if (_environment_manager) {
        var position = _environment_manager.TransformPosition (this.transform.position);
        var direction = _environment_manager.TransformDirection (this.transform.rotation.eulerAngles);
        _position = new float[] { position.x, position.y, position.z };
        _direction = new float[] { direction.x, direction.y, direction.z };
        _rotation = new float[] {
          transform.rotation.x,
          transform.rotation.y,
          transform.rotation.z,
          transform.rotation.w
        };
      } else {
        _position = new float[] { this.transform.position.x, this.transform.position.y, this.transform.position.z };
        _direction = new float[] {
          this.transform.rotation.eulerAngles.x,
          this.transform.rotation.eulerAngles.y,
          this.transform.rotation.eulerAngles.z
        };
        _rotation = new float[] {
          transform.rotation.x,
          transform.rotation.y,
          transform.rotation.z,
          transform.rotation.w
        };
      }
    }

    public Dictionary<string, Motor> GetMotors () {
      return _motors;
    }

    public void AddMotor (Motor motor) {
      if (_motors == null)
        _motors = new Dictionary<string, Motor> ();
      if (_debug)
        Debug.Log ("Actor " + name + " has motor " + motor);
      _motors.Add (motor._motor_identifier, motor);
    }

    public void Register (Motor obj) {
      AddMotor (obj);
    }

    public virtual void Reset () {
      foreach (var motor in _motors.Values) {
        motor.Reset ();
      }
    }
  }
}
