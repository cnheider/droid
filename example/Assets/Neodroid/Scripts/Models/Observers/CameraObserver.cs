using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Models.Observers {

  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class CameraObserver : Observer {

    void Start() {
      AddToAgent();
    }
      
    public override byte[] GetData() {
      _data = NeodroidFunctions.RenderTextureImage(this.GetComponent<Camera>()).EncodeToPNG();
      return _data;
    }
  }
}
