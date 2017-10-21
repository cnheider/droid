using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.NeodroidEnvironment.Motors {
  public class SingleAxisMotor : Motor {
    public MotorAxis _axis_of_motion;
    public Space _space = Space.Self;

    public override void ApplyMotion (MotorMotion motion) {
      if (_debug)
        Debug.Log ("Applying " + motion._strength.ToString () + " To " + name);
      if (!_bidirectional && motion._strength < 0) {
        Debug.Log ("Motor is not bi-directional. It does not accept negative input.");
        return; // Do nothing
      }
      switch (_axis_of_motion) {
      case MotorAxis.X:
        transform.Translate (Vector3.left * motion._strength, _space);
        break;
      case MotorAxis.Y:
        transform.Translate (-Vector3.up * motion._strength, _space);
        break;
      case MotorAxis.Z:
        transform.Translate (-Vector3.forward * motion._strength, _space);
        break;
      case MotorAxis.Rot_X:
        transform.Rotate (Vector3.left, motion._strength, _space);
        break;
      case MotorAxis.Rot_Y:
        transform.Rotate (Vector3.up, motion._strength, _space);
        break;
      case MotorAxis.Rot_Z:
        transform.Rotate (Vector3.forward, motion._strength, _space);
        break;
      default:
        break;
      }
      _energy_spend_since_reset += _energy_cost * motion._strength;
    }

    public override string GetMotorIdentifier () {
      return name + "SingleAxis" + _axis_of_motion.ToString ();
    }
  }
}
