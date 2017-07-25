using MsgPack.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Neodroid.Messaging.Messages {
  public class Reaction {
    public Dictionary<string, MotorMotion> _actor_motor_motions;
    public bool _reset;

    public Reaction() {

    }

    public Dictionary<string, MotorMotion> GetMotions() {
      return _actor_motor_motions;
    }

    public override string ToString() {
      var text = "";
      foreach (string key in _actor_motor_motions.Keys) {
        text += key;
      }

      foreach (MotorMotion key in _actor_motor_motions.Values) {
        text += key;
      }
      return "<Reaction> " + _reset + ", " + text + " </Reaction>";
    }
  }
}
