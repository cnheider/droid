namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.CreateAssetMenuAttribute(fileName = "Example Asset",
                                        menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath
                                                   + "Example Asset",
                                        order = 1)]
  public class Example : UnityEngine.ScriptableObject {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    GameObjectFloatDictionary _game_object_float_store =
        GameObjectFloatDictionary.New<GameObjectFloatDictionary>();

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    StringIntDictionary _string_integer_store = StringIntDictionary.New<StringIntDictionary>();

    /// <summary>
    /// </summary>
    System.Collections.Generic.Dictionary<string, int> StringIntegers {
      get { return this._string_integer_store._Dict; }
    }

    /// <summary>
    /// </summary>
    System.Collections.Generic.Dictionary<UnityEngine.GameObject, float> Screenshots {
      get { return this._game_object_float_store._Dict; }
    }
  }
}