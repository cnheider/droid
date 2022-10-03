namespace droid.Runtime.Utilities.InternalReactions {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public abstract class ScriptedReactions : UnityEngine.MonoBehaviour {
    #if UNITY_EDITOR
    const int _script_execution_order = -10000;
    #endif

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _debugging;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected droid.Runtime.Managers.AbstractNeodroidManager _Manager;

    /// <summary>
    /// </summary>
    public static ScriptedReactions Instance { get; private set; }

    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endif
    /// <summary>
    /// </summary>
    void Awake() {
      if (Instance == null) {
        Instance = this;
      } else {
        UnityEngine.Debug.LogWarning(message : "WARNING! Multiple PlayerReactions in the scene! Only using "
                                               + Instance);
      }

      #if UNITY_EDITOR
      if (!UnityEngine.Application.isPlaying) {
        var manager_script = UnityEditor.MonoScript.FromMonoBehaviour(behaviour : this);
        if (UnityEditor.MonoImporter.GetExecutionOrder(script : manager_script) != _script_execution_order) {
          UnityEditor.MonoImporter.SetExecutionOrder(script : manager_script,
                                                     order :
                                                     _script_execution_order); // Ensures that PreStep is called first, before all other scripts.
          UnityEngine.Debug
                     .LogWarning("Execution Order changed, you will need to press play again to make everything function correctly!");
          UnityEditor.EditorApplication.isPlaying = false;
          //TODO: UnityEngine.Experimental.LowLevel.PlayerLoop.SetPlayerLoop(new UnityEngine.Experimental.LowLevel.PlayerLoopSystem());
        }
      }
      #endif
    }
  }
}