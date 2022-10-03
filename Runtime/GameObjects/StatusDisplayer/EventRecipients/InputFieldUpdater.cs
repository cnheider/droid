namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc cref="DataPoller" />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.UI.InputField))]
  [UnityEngine.ExecuteInEditMode]
  public class InputFieldUpdater : DataPoller {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.UI.InputField _input_field;

    /// <summary>
    /// </summary>
    void Start() { this._input_field = this.GetComponent<UnityEngine.UI.InputField>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public override void PollData(dynamic data) {
      if (data is string) {
        if (this._input_field) {
          this._input_field.text = data;
        }
      }
    }
  }
}