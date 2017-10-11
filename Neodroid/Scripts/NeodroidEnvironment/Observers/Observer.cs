using UnityEngine;
using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Agents;
using System;
using Neodroid.NeodroidEnvironment.Managers;

namespace Neodroid.NeodroidEnvironment.Observers {
  [Serializable]
  public class Observer : MonoBehaviour {

    public NeodroidAgent _agent_game_object;
    public EnvironmentManager _environment_manager;

    public bool _debug = false;

    public byte[] _data;

    protected virtual void Start () {
      Setup ();
    }

    protected virtual void Setup () {
      if (!_environment_manager) {
        _environment_manager = FindObjectOfType<EnvironmentManager> ();
      }
    }

    protected void AddToAgent () {
      NeodroidFunctions.MaybeRegisterComponent (_agent_game_object, this);
    }

    public virtual byte[] GetData () { 
      if (_data != null)
        return _data;
      else
        return new byte[]{ };
    }

    public float[] _position;
    public float[] _rotation;
    public float[] _direction;


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

    public virtual void Reset () {

    }
  }
}
