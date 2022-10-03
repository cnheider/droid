namespace droid.Runtime.GameObjects.Plotting {
  public static class PlotFunctions {
    static System.Collections.Generic.List<droid.Runtime.Structs.Points.ValuePoint> _points =
        new System.Collections.Generic.List<droid.Runtime.Structs.Points.ValuePoint>();

    /// <summary>
    /// </summary>
    /// <param name="size"></param>
    /// <param name="min_val"></param>
    /// <param name="max_val"></param>
    /// <param name="particle_size_min"></param>
    /// <param name="particle_size_max"></param>
    /// <returns></returns>
    public static droid.Runtime.Structs.Points.ValuePoint[] SampleRandomSeries(int size,
                                                                               float min_val = 0,
                                                                               float max_val = 5,
                                                                               float particle_size_min = 0.2f,
                                                                               float particle_size_max = 1.8f) {
      var s = new droid.Runtime.Structs.Space.Sample.SampleSpace3 {
                                                                      _space = droid.Runtime.Structs.Space
                                                                          .Space3.MinusOneOne
                                                                  };
      _points.Clear();
      for (var j = 0; j < size; j++) {
        var point = s.Sample() * max_val;
        var vp = new droid.Runtime.Structs.Points.ValuePoint(pos : point,
                                                             val : UnityEngine.Random.Range(minInclusive : min_val,
                                                                                            maxInclusive : max_val),
                                                             size : UnityEngine.Random
                                                                               .Range(minInclusive : particle_size_min,
                                                                                      maxInclusive : particle_size_max));
        _points.Add(item : vp);
      }

      var points = _points.ToArray();
      return points;
    }
  }
}