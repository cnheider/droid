namespace droid.Runtime.ScriptableObjects {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.CreateAssetMenuAttribute(fileName = "PlayerMotions",
                                        menuName = ScriptableObjectMenuPath._ScriptableObjectMenuPath
                                                   + "PlayerMotions",
                                        order = 1)]
  public class PlayerMotions : UnityEngine.ScriptableObject {
    /// <summary>
    /// </summary>
    public PlayerMotion[] _Motions;

    void OnValidate() {
/*
      foreach (var motion in this._Motions) {
        foreach (var motion2 in this._Motions) {
          if (motion._Key == motion2._Key) {
            Debug.LogWarning($"{motion} and {motion2} has the same Key");
          }
        }
      }
  */
      var copy = this._Motions;
      if (copy != null) {
        for (var i = 0; i < copy.Length; i++) {
          var actor = copy[i]._Actor;
          if (actor != null) {
            copy[i]._Actor = System.Text.RegularExpressions.Regex.Replace(input : actor, "[^\\w\\._]", "");
          }

          var actuator = copy[i]._Actuator;
          if (actuator != null) {
            copy[i]._Actuator =
                System.Text.RegularExpressions.Regex.Replace(input : actuator, "[^\\w\\._]", "");
          }
        }
      }

      this._Motions = copy;
    }
  }

  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct PlayerMotion {
    /// <summary>
    /// </summary>
    [droid.Runtime.Utilities.SearchableEnumAttribute]
    public UnityEngine.KeyCode _Key;

    /// <summary>
    /// </summary>
    public string _Actor;

    /// <summary>
    /// </summary>
    public string _Actuator;

    /// <summary>
    /// </summary>
    public float _Strength;
  }
}