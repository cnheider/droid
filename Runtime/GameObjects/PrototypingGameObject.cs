namespace droid.Runtime.GameObjects {
  /// <inheritdoc cref="Interfaces.IRegisterable" />
  /// <summary>
  /// </summary>
  public abstract class PrototypingGameObject : UnityEngine.MonoBehaviour,
                                                droid.Runtime.Interfaces.IRegisterable {
    #if NEODROID_DEBUG
    /// <summary>
    /// </summary>

    [field : UnityEngine.SerializeField]
    public bool Debugging { get; set; }
    #endif

    ///
    public virtual string PrototypingTypeName { get { return this.GetType().Name; } }

    /// <summary>
    /// </summary>
    [field : UnityEngine.HeaderAttribute("Development", order = 90)]
    [field : UnityEngine.SerializeField]
    public bool DisablesChildren { get; set; } = false;

    [field : UnityEngine.SerializeField] public bool UnregisterAtDisable { get; set; } = false;

    /// <summary>
    /// </summary>
    [field : UnityEngine.SerializeField]
    protected string CustomName { get; set; } = "";

    /// <summary>
    /// </summary>
    protected void Awake() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Clear();
      }
    }

    /// <summary>
    /// </summary>
    protected void Start() { this.ReRegister(); }

    /// <summary>
    /// </summary>
    void OnEnable() {
      if (this.DisablesChildren) {
        foreach (UnityEngine.Transform child in this.transform) {
          if (child != this.transform) {
            child.gameObject.SetActive(true);
            child.GetComponent<PrototypingGameObject>()?.CallOnEnable();
          }
        }

        var children = this.GetComponentsInChildren<PrototypingGameObject>();
        for (var index = 0; index < children.Length; index++) {
          var child = children[index];
          if (child.gameObject != this.gameObject) {
            child.enabled = true;
            child.gameObject.SetActive(true);
          }
        }
      }

      #if UNITY_EDITOR
      if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
        return;
      }
      #endif

      this.ReRegister();
    }

    /// <summary>
    /// </summary>
    void OnDisable() {
      if (this.DisablesChildren) {
        var children = this.GetComponentsInChildren<PrototypingGameObject>();
        for (var index = 0; index < children.Length; index++) {
          var child = children[index];
          if (child.gameObject != this.gameObject) {
            child.enabled = false;
            child.gameObject.SetActive(false);
          }
        }

        foreach (UnityEngine.Transform child in this.transform) {
          if (child != this.transform) {
            child.GetComponent<PrototypingGameObject>()?.CallOnDisable();
            child.gameObject.SetActive(false);
          }
        }
      }

      if (this.UnregisterAtDisable) {
        this.UnRegisterComponent();
      }
    }

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    void OnValidate() { // Only called in the editor
      if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode) {
        this.ReRegister();
      }
    }
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public string Identifier {
      get {
        if (!string.IsNullOrWhiteSpace(value : this.CustomName)) {
          return this.CustomName.Trim();
        }

        return $"{this.name}{this.PrototypingTypeName}{this.GetInstanceID()}";
      }
    }

    /// <summary>
    /// </summary>
    public virtual void Setup() { this.PreSetup(); }

    /// <summary>
    /// </summary>
    public virtual void PreSetup() { }

    /// <summary>
    /// </summary>
    public virtual void RemotePostSetup() { }

    /// <summary>
    /// </summary>
    public virtual void Tick() { }

    /// <summary>
    /// </summary>
    public virtual void PrototypingReset() { }

    /// <summary>
    /// </summary>
    void ReRegister() {
      try {
        if (this.enabled && this.isActiveAndEnabled) {
          this.Setup();
          this.UnRegisterComponent();
          this.RegisterComponent();
        }
      } catch (System.ArgumentNullException e) {
        UnityEngine.Debug.LogWarning(message : e);
        UnityEngine.Debug.Log(message :
                              $"You must override RegisterComponent and UnRegisterComponent for component {this.GetType()} for gameobject {this.Identifier} in order to Re-register component on every 'OnValidate' while in edit-mode");
      }
    }

    /// <summary>
    /// </summary>
    void CallOnDisable() { this.OnDisable(); }

    /// <summary>
    /// </summary>
    void CallOnEnable() { this.OnEnable(); }

    /// <summary>
    /// </summary>
    protected virtual void Clear() { }

    /// <summary>
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected abstract void UnRegisterComponent();

    /// <summary>
    /// </summary>
    /// <exception cref="System.NotImplementedException"></exception>
    protected abstract void RegisterComponent();

    /// <summary>
    /// </summary>
    public void RefreshStart() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Start();
      }
    }

    /// <summary>
    /// </summary>
    public void RefreshAwake() {
      if (this.enabled && this.isActiveAndEnabled) {
        this.Awake();
      }
    }
  }
}