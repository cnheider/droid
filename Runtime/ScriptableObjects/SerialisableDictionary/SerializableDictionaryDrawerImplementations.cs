#if UNITY_EDITOR

namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  [UnityEditor.CustomPropertyDrawer(type : typeof(StringIntDictionary))]
  public class StringIntDictionaryDrawer : SerializableDictionaryDrawer<string, int> {
    protected override SerializableKeyValueTemplate<string, int> GetTemplate() {
      return this.GetGenericTemplate<SerializableStringIntTemplate>();
    }
  }

  class SerializableStringIntTemplate : SerializableKeyValueTemplate<string, int> { }

  [UnityEditor.CustomPropertyDrawer(type : typeof(GameObjectFloatDictionary))]
  public class GameObjectFloatDictionaryDrawer : SerializableDictionaryDrawer<UnityEngine.GameObject, float> {
    protected override SerializableKeyValueTemplate<UnityEngine.GameObject, float> GetTemplate() {
      return this.GetGenericTemplate<SerializableGameObjectFloatTemplate>();
    }
  }

  class SerializableGameObjectFloatTemplate : SerializableKeyValueTemplate<UnityEngine.GameObject, float> { }

  [UnityEditor.CustomPropertyDrawer(type : typeof(StringGameObjectDictionary))]
  public class StringGameObjectDictionaryDrawer :
      SerializableDictionaryDrawer<string, UnityEngine.GameObject> {
    protected override SerializableKeyValueTemplate<string, UnityEngine.GameObject> GetTemplate() {
      return this.GetGenericTemplate<SerializableStringGameObjectTemplate>();
    }
  }

  class SerializableStringGameObjectTemplate :
      SerializableKeyValueTemplate<string, UnityEngine.GameObject> { }
}
#endif