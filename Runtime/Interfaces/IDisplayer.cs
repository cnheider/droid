namespace droid.Runtime.Interfaces {
  public interface IDisplayer : IRegisterable {
    //void Display(dynamic o);
    //void Display(object o);

    void Display(float value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    void Display(double value);

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
    void Display(float[] values);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    void Display(string value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    void Display(UnityEngine.Vector3 value);

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    void Display(UnityEngine.Vector3[] value);

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    void Display(droid.Runtime.Structs.Points.ValuePoint point);

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    void Display(droid.Runtime.Structs.Points.ValuePoint[] points);

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    void Display(droid.Runtime.Structs.Points.StringPoint point);

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    void Display(droid.Runtime.Structs.Points.StringPoint[] points);

    // void PlotSeries(dynamic points);
    void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points);
  }
}