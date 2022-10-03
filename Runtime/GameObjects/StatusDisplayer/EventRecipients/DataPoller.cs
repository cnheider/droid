namespace droid.Runtime.GameObjects.StatusDisplayer.EventRecipients {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public abstract class DataPoller : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] UnityEngine.Events.UnityEvent _poll_event;

    [UnityEngine.SerializeField] bool _invoke_on_validate = false;

    /// <summary>
    /// </summary>
    public UnityEngine.Events.UnityEvent PollEvent {
      get { return this._poll_event; }
      set { this._poll_event = value; }
    }

    // Update is called once per frame
    /// <summary>
    /// </summary>
    void Update() {
      if (!this._invoke_on_validate) {
        this._poll_event?.Invoke();
      }
    }

    void OnEnable() { this._poll_event?.Invoke(); }

    void OnValidate() {
      if (this._invoke_on_validate) {
        this._poll_event?.Invoke();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="data"></param>
    public abstract void PollData(dynamic data);
  }
}