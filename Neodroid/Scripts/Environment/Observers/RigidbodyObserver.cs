using UnityEngine;
using Neodroid.Utilities;
using System.Text;

namespace Neodroid.Models.Observers {

  [ExecuteInEditMode]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyObserver : Observer {

    Rigidbody _rigidbody;

    void Start() {
      AddToAgent();
      _rigidbody = this.GetComponent<Rigidbody> ();
    }

    public override byte[] GetData() {
      var str_rep = "Velocity: " +  _rigidbody.velocity.ToString();
      str_rep += ", AngularVelocity: " + _rigidbody.angularVelocity.ToString();
      _data = Encoding.ASCII.GetBytes(str_rep);
      return _data;
    }
  }
}