using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Models.Motors {
  public enum MotorAxis { X, Y, Z, rot_X, rot_Y, rot_Z, rot_W }

  public class SingleAxisMotor : Motor {
    public MotorAxis _axis_of_motion;



    public override void ApplyMotion(MotorMotion motion) {
      if (_debug) Debug.Log("Applying " + motion._strength.ToString() + " To " + name);
      if (!_bidirectional && motion._strength < 0) {
        return; // Do nothing
      }
      var new_pos = transform.localPosition;
      var new_rot = transform.localRotation;
      switch (_axis_of_motion) {
        case MotorAxis.X:
          new_pos.Set(new_pos.x + motion._strength, new_pos.y, new_pos.z);
          break;
        case MotorAxis.Y:
          new_pos.Set(new_pos.x, new_pos.y + motion._strength, new_pos.z);
          break;
        case MotorAxis.Z:
          new_pos.Set(new_pos.x, new_pos.y, new_pos.z + motion._strength);
          break;
        case MotorAxis.rot_X:
          new_rot.Set(new_rot.x + motion._strength, new_rot.y, new_rot.z, new_rot.w);
          break;
        case MotorAxis.rot_Y:
          new_rot.Set(new_rot.x, new_rot.y + motion._strength, new_rot.z, new_rot.w);
          break;
        case MotorAxis.rot_Z:
          new_rot.Set(new_rot.x, new_rot.y, new_rot.z + motion._strength, new_rot.w);
          break;
        case MotorAxis.rot_W:
          new_rot.Set(new_rot.x, new_rot.y, new_rot.z, new_rot.w + motion._strength);
          break;
        default:
          break;
      }
      transform.localPosition = new_pos;
      transform.localRotation = new_rot;
      _energy_spend_since_reset += _energy_cost * motion._strength;
    }

    public override string GetMotorIdentifier() {
      return _axis_of_motion.ToString();
    }
  }
}
