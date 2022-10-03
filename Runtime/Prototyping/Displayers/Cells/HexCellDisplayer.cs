namespace droid.Runtime.Prototyping.Displayers.Cells {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                                           + "VectorField"
                                           + DisplayerComponentMenuPath._Postfix)]
  public class HexCellDisplayer : QuadCellDisplayer {
    [UnityEngine.SerializeField] bool _plot_random_series = false;
    [UnityEngine.SerializeField] bool _retain_last_plot = true;

    public bool PlotRandomSeries {
      get { return this._plot_random_series; }
      set { this._plot_random_series = value; }
    }

    void Update() {
      if (this._retain_last_plot) {
        if (this._Values != null) {
          PlotSeries(points : this._Values);
        }
      }
    }

    /*public override void PlotSeries(float[] points) {

    }*/

    #if UNITY_EDITOR
    void OnDrawGizmos() {
      if (this.enabled) {
        if (this._Values == null || this._Values.Length == 0) {
          if (this._plot_random_series) {
            var vs = droid.Runtime.GameObjects.Plotting.PlotFunctions.SampleRandomSeries(9);
            this._Values =
                System.Linq.Enumerable.ToArray(source : System.Linq.Enumerable.Select(source : vs,
                                                 v => v._Val));
            this.PlotSeries(points : vs);
          }
        }
      }
    }
    #endif

    /// <summary>
    /// </summary>
    public override void Setup() { }

    public override void Display(double value) { }

    public override void Display(float[] values) { }

    public override void Display(string values) { }

    public override void Display(UnityEngine.Vector3 value) { throw new System.NotImplementedException(); }
    public override void Display(UnityEngine.Vector3[] value) { this.ScatterPlot(points : value); }

    public override void Display(droid.Runtime.Structs.Points.ValuePoint points) {
      this.PlotSeries(points : new[] {points});
    }

    public override void Display(droid.Runtime.Structs.Points.ValuePoint[] points) { }

    public override void Display(droid.Runtime.Structs.Points.StringPoint point) {
      throw new System.NotImplementedException();
    }

    public override void Display(droid.Runtime.Structs.Points.StringPoint[] points) {
      throw new System.NotImplementedException();
    }

    public override void Display(float values) { }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public new void ScatterPlot(UnityEngine.Vector3[] points) { }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public new void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("Plotting value points");
      }
      #endif

      this._Values = points;

      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
//point._Size
        switch ((int)point._Val) {
          case 0:
            droid.Runtime.Utilities.Drawing.NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
              direction : UnityEngine.Vector3.forward,
              color : UnityEngine.Color.cyan);
            break;
          case 1:
            droid.Runtime.Utilities.Drawing.NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
              direction : UnityEngine.Vector3.back,
              color : UnityEngine.Color.cyan);
            break;
          case 2:
            droid.Runtime.Utilities.Drawing.NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
              direction : UnityEngine.Vector3.up,
              color : UnityEngine.Color.cyan);
            break;
          case 3:
            droid.Runtime.Utilities.Drawing.NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
              direction : UnityEngine.Vector3.down,
              color : UnityEngine.Color.cyan);
            break;
          case 4:
            droid.Runtime.Utilities.Drawing.NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
              direction : UnityEngine.Vector3.left,
              color : UnityEngine.Color.cyan);
            break;
          case 5:
            droid.Runtime.Utilities.Drawing.NeodroidDrawingUtilities.ForDebug(pos : point._Pos,
              direction : UnityEngine.Vector3.right,
              color : UnityEngine.Color.cyan);
            break;
        }
      }
    }
  }
}