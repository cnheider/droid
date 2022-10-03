namespace droid.Runtime.ScriptableObjects.Deprecated {
  [UnityEngine.CreateAssetMenuAttribute(fileName = "Curriculum",
                                        menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath
                                                   + "Curriculum",
                                        order = 1)]
  public class Curriculum : UnityEngine.ScriptableObject {
    public Level[] _Levels;
  }

  [System.SerializableAttribute]
  public struct Level {
    public ConfigurableEntry[] _Configurable_Entries;
    public float _Min_Reward;
    public float _Max_Reward;
  }

  [System.SerializableAttribute]
  public struct ConfigurableEntry {
    public string _Configurable_Name;
    public float _Min_Value;
    public float _Max_Value;
  }
}