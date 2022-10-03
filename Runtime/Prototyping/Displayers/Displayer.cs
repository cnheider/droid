namespace droid.Runtime.Prototyping.Displayers {
  /// <inheritdoc cref="GameObjects.PrototypingGameObject" />
  /// <summary>
  /// </summary>
  public abstract class Displayer : droid.Runtime.GameObjects.PrototypingGameObject,
                                    droid.Runtime.Interfaces.IDisplayer {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    protected bool _RetainLastPlot = true;

    [UnityEngine.SerializeField] bool clean_all_children = true;
    [UnityEngine.SerializeField] bool clean_before_every_plot = true;

    /// <summary>
    /// </summary>
    droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment _environment = null;

    /// <summary>
    /// </summary>
    protected dynamic _Values = null;

    /// <summary>
    /// </summary>
    public droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment ParentEnvironment {
      get { return this._environment; }
      set { this._environment = value; }
    }

    /// <summary>
    /// </summary>
    void Update() {
      if (this._RetainLastPlot) {
        if (this.clean_before_every_plot) {
          this.Clean();
        }

        if (this._Values != null) {
          PlotSeries(points : this._Values);
        }
      }
    }

    void OnEnable() { this.Clean(); }

    void OnDisable() { this.Clean(); }

    void OnDestroy() { this.Clean(); }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled && UnityEditor.Selection.activeGameObject == this.gameObject) {
        if (!this._PlotRandomSeries && !this._RetainLastPlot) {
          this.Clean();
        }

        if (this._Values == null || this._Values.Length == 0 || this.always_random_sample_new) {
          if (this._PlotRandomSeries) {
            this.Clean();
            var vs = droid.Runtime.GameObjects.Plotting.PlotFunctions.SampleRandomSeries(9);
            this._Values =
                System.Linq.Enumerable.ToArray(source : System.Linq.Enumerable.Select(source : vs,
                                                 v => v._Val));
            this.PlotSeries(points : vs);
          }
        }
      } else {
        this.Clean();
      }
    }

    #endif

    //void OnValidate() { this.Clean(); }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(float value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(double value);

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    public abstract void Display(float[] values);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(string value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(UnityEngine.Vector3 value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    public abstract void Display(UnityEngine.Vector3[] value);

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    public abstract void Display(droid.Runtime.Structs.Points.ValuePoint point);

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public abstract void Display(droid.Runtime.Structs.Points.ValuePoint[] points);

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    public abstract void Display(droid.Runtime.Structs.Points.StringPoint point);

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public abstract void Display(droid.Runtime.Structs.Points.StringPoint[] points);

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public abstract void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          droid.Runtime.Utilities.NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
            c : this);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() { this.ParentEnvironment?.UnRegister(displayer : this); }

    /// <summary>
    /// </summary>
    protected virtual void Clean() {
      if (this.clean_all_children) {
        foreach (UnityEngine.Transform child in this.transform) {
          if (UnityEngine.Application.isPlaying) {
            Destroy(obj : child.gameObject);
          } else {
            DestroyImmediate(obj : child.gameObject);
          }
        }
      }

      if (this._RetainLastPlot) {
        this._Values = null;
      }
    }

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("OnGizmo")]
    [UnityEngine.SerializeField]
    bool _PlotRandomSeries = false;

    [UnityEngine.SerializeField] bool always_random_sample_new = true;
    #endif
  }
}