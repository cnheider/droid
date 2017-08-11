using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Models.Observers {

  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class LightMaskObserver : Observer {

    void Start() {
      AddToAgent();
      //Debug.Log(NeodroidFunctions.ColorArrayToString(NeodroidFunctions.RenderTextureImage(this.GetComponent<Camera>()).GetPixels(1, 1, 4, 4)));
    }


    public override byte[] GetData() {
      _data = NeodroidFunctions.RenderTextureImage(this.GetComponent<Camera>()).EncodeToPNG();
      return _data;
    }
  }
}
