using MsgPack.Serialization;
using Neodroid.Models;
using Neodroid.Models.Observers;
using System;
using System.Collections.Generic;

namespace Neodroid.Messaging.Messages {
  [Serializable]
  public class EnvironmentState {
    public float _time_since_reset;
    public float _total_energy_spent_since_reset;

    //[MessagePackKnownCollectionItemType("Actor", typeof(Actor))]
    [MessagePackRuntimeType]
    public Dictionary<string, Actor> _actors;

    //[MessagePackKnownCollectionItemType("DepthObserver", typeof(DepthObserver))]
    //[MessagePackKnownCollectionItemType("LightMaskObserver", typeof(LightMaskObserver))]
    //[MessagePackKnownCollectionItemType("Observer", typeof(Observer))]
    [MessagePackRuntimeType]
    public Dictionary<string, Observer> _observers;

    public float _reward_for_last_step;

    public EnvironmentState(float time_since_reset, float total_energy_spent_since_reset, Dictionary<string, Actor> actors, Dictionary<string, Observer> observers, float reward_for_last_step) {
      _time_since_reset = time_since_reset;
      _total_energy_spent_since_reset = total_energy_spent_since_reset;
      _actors = actors;
      _observers = observers;
      _reward_for_last_step = reward_for_last_step;
    }

    public EnvironmentState() {

    }
  }
}
