namespace droid.Samples.MultiArmedBandit.Displayers {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : droid.Runtime.Prototyping.Displayers.DisplayerComponentMenuPath
                                                ._ComponentMenuPath
                                           + "TextBarPlot"
                                           + droid.Runtime.Prototyping.Displayers.DisplayerComponentMenuPath
                                                  ._Postfix)]
  public class TextBarPlotDisplayer : droid.Runtime.Prototyping.EnvironmentListener.EnvironmentListener {
    [UnityEngine.SerializeField]
    droid.Runtime.Prototyping.Displayers.Canvas.CanvasBarDisplayer[] _canvas_bars = { };

    [UnityEngine.SerializeField]
    droid.Runtime.Prototyping.Displayers.Canvas.CanvasTextDisplayer[] _canvas_text = { };

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "TextBarPlot"; } }

    void Update() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        //this.Display(this._values);
      }
      #endif
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PrototypingReset() {
      var canvas_bar_displayers = this._canvas_bars;
      if (canvas_bar_displayers != null) {
        foreach (var bar in canvas_bar_displayers) {
          bar.Display(0.5);
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    public void Display(float[] values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Displaying {values} at {this.Identifier}");
      }
      #endif

      var canvas_bar_displayers = this._canvas_bars;
      var canvas_text_displayers = this._canvas_text;

      if (canvas_bar_displayers != null) {
        for (var i = 0; i < canvas_bar_displayers.Length; i++) {
          if (i < values.Length) {
            var bar = canvas_bar_displayers[i];
            bar?.Display(value : values[i]);

            if (canvas_text_displayers != null) {
              var text = canvas_text_displayers[i];
              text?.Display(value : values[i]);
            }
          }
        }
      }
    }
  }
}