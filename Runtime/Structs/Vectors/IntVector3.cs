namespace droid.Runtime.Structs.Vectors {
  [System.SerializableAttribute]
  public struct IntVector3 {
    [UnityEngine.SerializeField] public int _X;
    [UnityEngine.SerializeField] public int _Y;
    [UnityEngine.SerializeField] public int _Z;

    public IntVector3(UnityEngine.Vector3 vec3) {
      this._X = UnityEngine.Mathf.RoundToInt(f : vec3.x);
      this._Y = UnityEngine.Mathf.RoundToInt(f : vec3.y);
      this._Z = UnityEngine.Mathf.RoundToInt(f : vec3.z);
    }

    public IntVector3(int x, int y, int z) {
      this._X = x;
      this._Y = y;
      this._Z = z;
    }

    public int X { get { return this._X; } set { this._X = value; } }

    public int Y { get { return this._Y; } set { this._Y = value; } }

    public int Z { get { return this._Z; } set { this._Z = value; } }

    public static IntVector3 operator+(IntVector3 a, IntVector3 b) {
      a._X += b._X;
      a._Y += b._Y;
      a._Z += b._Z;
      return a;
    }
  }
}