namespace droid.Runtime.ScriptableObjects.SerialisableDictionary {
  [System.SerializableAttribute] public class StringIntDictionary : SerializableDictionary<string, int> { }

  [System.SerializableAttribute]
  public class GameObjectFloatDictionary : SerializableDictionary<UnityEngine.GameObject, float> { }

  [System.SerializableAttribute]
  public class StringGameObjectDictionary : SerializableDictionary<string, UnityEngine.GameObject> { }
}