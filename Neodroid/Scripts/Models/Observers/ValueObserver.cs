using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Models.Observers {

  [ExecuteInEditMode]
  public class ValueObserver : Observer {

    void Start() {
      AddToAgent();
      _data = new byte[1]{123};
    }
      
    public override byte[] GetData() {
      return _data;
    }
  }
}
