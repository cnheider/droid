using UnityEngine;
using Neodroid.Utilities;

namespace Neodroid.NeodroidEnvironment.Observers {

  [ExecuteInEditMode]
  [RequireComponent (typeof(Camera))]
  public class CameraObserver : Observer {

    Camera _camera;

    protected override void Start () {
      Setup ();
      AddToAgent ();
      _camera = this.GetComponent<Camera> ();
    }

    public override byte[] GetData () {
      _data = NeodroidFunctions.RenderTextureImage (_camera).EncodeToPNG ();
      return _data;
    }
  }
}
