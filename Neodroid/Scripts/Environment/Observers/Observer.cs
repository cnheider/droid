using UnityEngine;
using Neodroid.Utilities;
using System;

namespace Neodroid.Models.Observers {
  [Serializable]
  public class Observer : MonoBehaviour {

    public NeodroidAgent _agent_game_object; //Is not send

    public bool _debug = false;

    public byte[] _data;

    protected void AddToAgent() {
      NeodroidFunctions.MaybeRegisterComponent(_agent_game_object, this);
    }
      
    public virtual byte[] GetData() { 
      if(_data != null)
        return _data;
      else
        return new byte[]{};
    }

    public float[] _position;
    public float[] _rotation;

    private void Update() {
      _position = new float[] { transform.position.x, transform.position.y, transform.position.z };
      _rotation = new float[] { transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w };
    }
  }
}
