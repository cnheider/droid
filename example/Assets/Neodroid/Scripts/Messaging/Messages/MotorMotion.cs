using System;

namespace Neodroid.Messaging.Messages {
  [Serializable]
  public class MotorMotion {
    public string _actor_name;
    public string _motor_name;
    public float _strength; // Has a possible direction given by the sign of the float


    public string GetActorName() {
      return _actor_name;
    }

    public string GetMotorName() {
      return _motor_name;
    }

    public override string ToString() {
      return "<MotorMotion> " + _actor_name + ", " + _motor_name + ", " + _strength + " </MotorMotion>";
    }
  }
}
