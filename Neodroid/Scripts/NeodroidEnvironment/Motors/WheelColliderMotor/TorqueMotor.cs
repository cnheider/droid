using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.NeodroidEnvironment.Motors {
  [RequireComponent (typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    public override void ApplyMotion (MotorMotion motion) {
      if (_debug)
        Debug.Log ("Applying " + motion._strength.ToString () + " To " + name);
      if (!_bidirectional && motion._strength < 0) {
        Debug.Log ("Motor is not bi-directional. It does not accept negative input.");
        return; // Do nothing
      }
      GetComponent<WheelCollider> ().motorTorque = motion._strength;
      _energy_spend_since_reset += _energy_cost * motion._strength;
    }

    public override string GetMotorIdentifier () {
      return name + "Torque";
    }
  }
}
