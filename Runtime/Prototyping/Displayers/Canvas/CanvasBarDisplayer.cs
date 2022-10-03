namespace droid.Runtime.Prototyping.Displayers.Canvas {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.UI.Image))]
  [UnityEngine.AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                                           + "Canvas/CanvasBar"
                                           + DisplayerComponentMenuPath._Postfix)]
  public class CanvasBarDisplayer : Displayer {
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0.0f, 1.0f)]
    float _value;

    UnityEngine.UI.Image _image;

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
    public override void Setup() { this._image = this.GetComponent<UnityEngine.UI.Image>(); }

    /// <summary>
    /// </summary>
    /// <param name="amount"></param>
    public void SetFillAmount(float amount) {
      if (this._image) {
        this._image.fillAmount = amount;
      }
    }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float value) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : value,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetFillAmount(amount : value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(double value) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : value,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetFillAmount(amount : (float)value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(float[] values) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : values[0],
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetFillAmount(amount : values[0]);
    }

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