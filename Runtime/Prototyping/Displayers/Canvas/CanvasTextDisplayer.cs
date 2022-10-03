namespace droid.Runtime.Prototyping.Displayers.Canvas {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.UI.Text))]
  [UnityEngine.AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                                           + "Canvas/CanvasText"
                                           + DisplayerComponentMenuPath._Postfix)]
  public class CanvasTextDisplayer : Displayer {
    UnityEngine.UI.Text _text_component;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._text_component = this.GetComponent<UnityEngine.UI.Text>(); }

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

      this.SetText(text : value.ToString(provider : System.Globalization.CultureInfo.InvariantCulture));
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

      this.SetText(text : value.ToString(provider : System.Globalization.CultureInfo.InvariantCulture));
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

      this.SetText(text : values[0].ToString(provider : System.Globalization.CultureInfo.InvariantCulture));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(string value) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : value,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : value);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(UnityEngine.Vector3 value) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : value,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : value.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(UnityEngine.Vector3[] value) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : value,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : value.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.ValuePoint points) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : points,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : points,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.StringPoint point) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : point,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : point.ToString());
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Display(droid.Runtime.Structs.Points.StringPoint[] points) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : points,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
    }

    public override void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      droid.Runtime.Utilities.DebugPrinting.DisplayPrint(value : points,
                                                         identifier : this.Identifier,
                                                         debugging : this.Debugging);
      #endif

      this.SetText(text : points.ToString());
    }

    /// <summary>
    /// </summary>
    /// <param name="text"></param>
    public void SetText(string text) {
      if (this._text_component) {
        this._text_component.text = text;
      }
    }
  }
}