using Neodroid.Messaging.Messages;
using UnityEngine;

namespace Neodroid.Models.Motors {
  [RequireComponent(typeof(WheelCollider))]
  public class SteeringMotor : Motor {

    WheelCollider _wheel_collider;

    private void Start() {
      _wheel_collider = GetComponent<WheelCollider>();
      RegisterComponent();
    }

    private void FixedUpdate() {
      ApplyLocalPositionToVisuals(_wheel_collider);
    }

    public override void ApplyMotion(MotorMotion motion) {
      if (_debug) Debug.Log("Applying " + motion._strength.ToString() + " To " + name);
      if (!_bidirectional && motion._strength < 0) {
        Debug.Log("Motor is not bi-directional. It does not accept negative input.");
        return; // Do nothing
      }
      _wheel_collider.steerAngle = motion._strength;
      _energy_spend_since_reset += _energy_cost * motion._strength;
    }

    public override string GetMotorIdentifier() {
      return "Steering";
    }

    public void ApplyLocalPositionToVisuals(WheelCollider collider) {
      if (collider.transform.childCount == 0) {
        return;
      }

      Transform visualWheel = collider.transform.GetChild(0);

      Vector3 position;
      Quaternion rotation;
      collider.GetWorldPose(out position, out rotation);

      visualWheel.transform.position = position;
      visualWheel.transform.rotation = rotation;
    }
  }
}
