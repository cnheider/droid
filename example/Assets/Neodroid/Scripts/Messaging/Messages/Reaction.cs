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
      return "<Reaction> " + _reset + " </Reaction>";
    }
  }
}
