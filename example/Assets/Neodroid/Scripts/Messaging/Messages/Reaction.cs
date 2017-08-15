using System;
using System.Collections.Generic;

namespace Neodroid.Messaging.Messages {
  
  [Serializable]
  public class Reaction {
    public MotorMotion[]_motions;
    public bool _reset;

    public Reaction(MotorMotion[] motions, bool reset) {
      _motions = motions;
      _reset = reset;
    }

    public MotorMotion[] GetMotions() {
      return _motions;
    }

    public override string ToString() {
      string motions_str = "";
      foreach (MotorMotion motion in GetMotions()) {
        motions_str += motion.ToString ()+"\n";
      }
      return "<Reaction> " + _reset + ",\n "+ motions_str +"</Reaction>";
    }
  }
}
