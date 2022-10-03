namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc cref="DataPoller" />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.UI.Toggle))]
  [UnityEngine.ExecuteInEditMode]
  public class ToggleUpdater : DataPoller {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.UI.Toggle _toggle;

    // Use this for initialization
    /// <summary>
    /// </summary>
    void Start() { this._toggle = this.GetComponent<UnityEngine.UI.Toggle>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public override void PollData(dynamic data) {
      if (data is bool) {
        if (this._toggle) {
          this._toggle.isOn = data;
        }
      }
    }
  }
}