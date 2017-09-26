using MsgPack.Serialization;
using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.Models.Observers {

  [ExecuteInEditMode]
  [RequireComponent(typeof(Camera))]
  public class DepthObserver : Observer {

    [MessagePackIgnore]
    public Material _material;

    void Start() {
      AddToAgent();
      GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
      //Debug.Log(NeodroidFunctions.ColorArrayToString(NeodroidFunctions.RenderTextureImage(this.GetComponent<Camera>()).GetPixels(1,1,4,4)));
    }

    void LateUpdate() {
    }

    public override byte[] GetData() {
      _data = NeodroidFunctions.RenderTextureImage(this.GetComponent<Camera>()).EncodeToPNG();
      return _data;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
      Graphics.Blit(source, destination, _material);
    }
  }
}
