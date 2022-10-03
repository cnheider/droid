namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc cref="DataPoller" />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.UI.Text))]
  [UnityEngine.ExecuteInEditMode]
  public class TextUpdater : DataPoller {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.UI.Text _text;

    /// <summary>
    /// </summary>
    void Start() { this._text = this.GetComponent<UnityEngine.UI.Text>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public override void PollData(dynamic data) {
      if (data is string) {
        if (this._text) {
          this._text.text = data;
        }
      }
    }
  }
}