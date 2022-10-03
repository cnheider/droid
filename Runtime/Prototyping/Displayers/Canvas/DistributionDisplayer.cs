namespace droid.Runtime.Prototyping.Displayers.Canvas {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                                           + "Canvas/CanvasBar"
                                           + DisplayerComponentMenuPath._Postfix)]
  public class DistributionDisplayer : Displayer {
    [UnityEngine.SerializeField] UnityEngine.UI.Image[] _images;

    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0.0f, 1.0f)]
    float _value;

    /// <summary>
    /// </summary>
    public float Value {
      get { return this._value; }
      set {
        this._value = value;
        this.SetFillAmount(amount : value);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      if (this._images == null || this._images.Length == 0) {
        this._images = this.GetComponentsInChildren<UnityEngine.UI.Image>();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="amount"></param>
    public void SetFillAmount(float amount) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Setting amount to {amount}");
      }
      #endif

      if (this._images[0]) {
        this._images[0].fillAmount = amount;
      }
    }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + value + " To " + this.name);
      }
      #endif

      this.Value = value;

      this.SetFillAmount(amount : value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(double value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying " + value + " To " + this.name);
      }
      #endif

      this.Value = (float)value;

      this.SetFillAmount(amount : (float)value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float[] values) { throw new System.NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(string value) { throw new System.NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(UnityEngine.Vector3 value) { throw new System.NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(UnityEngine.Vector3[] value) { throw new System.NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.ValuePoint points) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.ValuePoint[] points) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.StringPoint point) {
      throw new System.NotImplementedException();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.StringPoint[] points) {
      throw new System.NotImplementedException();
    }

    public override void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points) {
      throw new System.NotImplementedException();
    }
  }
}