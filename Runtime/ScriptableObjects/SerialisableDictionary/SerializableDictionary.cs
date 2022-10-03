namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  public abstract class SerializableDictionary<TK, TV> : UnityEngine.ISerializationCallbackReceiver {
    public System.Collections.Generic.Dictionary<TK, TV> _Dict;
    [UnityEngine.SerializeField] TK[] _keys;

    [UnityEngine.SerializeField] TV[] _values;

    #region ISerializationCallbackReceiver Members

    public void OnAfterDeserialize() {
      var c = this._keys.Length;
      this._Dict = new System.Collections.Generic.Dictionary<TK, TV>(capacity : c);
      for (var i = 0; i < c; i++) {
        this._Dict[key : this._keys[i]] = this._values[i];
      }

      this._keys = null;
      this._values = null;
    }

    public void OnBeforeSerialize() {
      var c = this._Dict.Count;
      this._keys = new TK[c];
      this._values = new TV[c];
      var i = 0;
      using (var e = this._Dict.GetEnumerator()) {
        while (e.MoveNext()) {
          var kvp = e.Current;
          this._keys[i] = kvp.Key;
          this._values[i] = kvp.Value;
          i++;
        }
      }
    }

    #endregion

    public static T New<T>() where T : SerializableDictionary<TK, TV>, new() {
      var result = new T {_Dict = new System.Collections.Generic.Dictionary<TK, TV>()};
      return result;
    }
  }
}