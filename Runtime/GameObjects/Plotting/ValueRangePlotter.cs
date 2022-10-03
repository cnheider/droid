#if UNITY_2019_1_OR_NEWER

namespace droid.Runtime.GameObjects.Plotting {
  [UnityEngine.ExecuteInEditMode]
  public class ValueRangePlotter : UnityEngine.MonoBehaviour {
    static readonly int _range = UnityEngine.Shader.PropertyToID("_Range");
    [UnityEngine.SerializeField] UnityEngine.Shader _shader = null;
    UnityEngine.Material _material;

    [UnityEngine.SerializeField]
    UnityEngine.Bounds _value_range =
        new UnityEngine.Bounds(center : UnityEngine.Vector3.zero, size : UnityEngine.Vector3.one * 2);

    void OnDestroy() {
      if (this._material != null) {
        if (UnityEngine.Application.isPlaying) {
          Destroy(obj : this._material);
        } else {
          DestroyImmediate(obj : this._material);
        }
      }
    }

    public void OnRenderObject() {
      if (this._material == null) {
        this._material = new UnityEngine.Material(shader : this._shader);
        this._material.hideFlags = UnityEngine.HideFlags.DontSave;
      }

      this._material.SetVector(nameID : _range,
                               value : new UnityEngine.Vector4(x : this._value_range.min.x,
                                                               y : this._value_range.max.x,
                                                               z : this._value_range.center.y,
                                                               w : this._value_range.extents.y
                                                                   + this._value_range.center.y));

      this._material.SetPass(0);
      UnityEngine.Graphics.DrawProceduralNow(topology : UnityEngine.MeshTopology.LineStrip, 512);
    }
  }
}
#endif