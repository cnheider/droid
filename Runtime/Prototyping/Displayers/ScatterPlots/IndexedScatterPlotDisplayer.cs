namespace droid.Runtime.Prototyping.Displayers.ScatterPlots {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                                           + "IndexedScatterPlot"
                                           + DisplayerComponentMenuPath._Postfix)]
  public class IndexedScatterPlotDisplayer : Displayer {
    [UnityEngine.SerializeField] UnityEngine.GameObject[] _designs = null;
    [UnityEngine.SerializeField] System.Collections.Generic.List<UnityEngine.GameObject> _instances = null;

    void Update() {
      if (this._RetainLastPlot) {
        if (this._Values != null) {
          PlotSeries(points : this._Values);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { }

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
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

    //public override void Display(Object o) { throw new NotImplementedException(); }
    public override void Display(float values) { }

    void SpawnDesign(UnityEngine.GameObject design,
                     UnityEngine.Vector3 position,
                     UnityEngine.Quaternion rotation) {
      //var go = Instantiate(design, position, rotation,this.transform);
      var go = Instantiate(original : design,
                           position : position,
                           rotation : design.transform.rotation,
                           parent : this.transform);
      this._instances.Add(item : go);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="immediately"></param>
    protected override void Clean() {
      if (UnityEngine.Application.isPlaying) {
        for (var index = 0; index < this._instances.Count; index++) {
          var game_object = this._instances[index : index];
          Destroy(obj : game_object);
        }
      } else {
        for (var index = 0; index < this._instances.Count; index++) {
          var game_object = this._instances[index : index];
          DestroyImmediate(obj : game_object);
        }
      }

      this._instances.Clear();

      base.Clean();
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(UnityEngine.Vector3[] points) { }

    /*public override void PlotSeries(float[] points) {

    }*/

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log("Plotting value points");
      }
      #endif

      this._Values = points;
      this.Clean();

      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
        var game_objects = this._designs;
        if (game_objects != null && point._Val >= game_objects.Length) {
          continue;
        }

        this.SpawnDesign(design : this._designs[(int)point._Val],
                         position : point._Pos,
                         rotation : UnityEngine.Quaternion.identity);
      }
    }
  }
}