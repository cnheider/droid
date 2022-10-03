namespace droid.Runtime.Structs {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class Points : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    public struct StringPoint {
      /// <summary>
      /// </summary>
      public UnityEngine.Vector3 _Pos;

      /// <summary>
      /// </summary>
      public float _Size;

      /// <summary>
      /// </summary>
      public string _Val;

      public StringPoint(UnityEngine.Vector3 pos, string val, float size) {
        this._Pos = pos;
        this._Val = val;
        this._Size = size;
      }
    }

    /// <summary>
    /// </summary>
    public struct ValuePoint {
      /// <summary>
      /// </summary>
      public UnityEngine.Vector3 _Pos;

      /// <summary>
      /// </summary>
      public float _Size;

      /// <summary>
      /// </summary>
      public float _Val;

      public ValuePoint(UnityEngine.Vector3 pos, float val, float size) {
        this._Pos = pos;
        this._Val = val;
        this._Size = size;
      }
    }
  }
}