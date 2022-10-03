namespace droid.Runtime.Prototyping.Displayers.ScatterPlots {
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                                           + "GameObjectScatterPlotDisplayer"
                                           + DisplayerComponentMenuPath._Postfix)]
  public class GameObjectScatterPlotDisplayer : Displayer {
    [UnityEngine.SerializeField] UnityEngine.Gradient _gradient;

    [UnityEngine.SerializeField]
    UnityEngine.ParticleSystemSimulationSpace _particle_system_simulation_space =
        UnityEngine.ParticleSystemSimulationSpace.World;

    [UnityEngine.SerializeField] float _size = 0.6f;
    UnityEngine.ParticleSystem _particle_system;

    UnityEngine.ParticleSystem.MainModule _particle_system_main_module;
    UnityEngine.ParticleSystemRenderer _particle_system_renderer;

    UnityEngine.ParticleSystem.Particle[] _particles;

    System.Collections.Generic.List<float> _vs = new System.Collections.Generic.List<float>();

    public override void Setup() {
      this._particle_system = this.GetComponent<UnityEngine.ParticleSystem>();
      var em = this._particle_system.emission;
      em.enabled = false;
      em.rateOverTime = 0;
      var sh = this._particle_system.shape;
      sh.enabled = false;

      this._particle_system_main_module = this._particle_system.main;
      this._particle_system_main_module.loop = false;
      this._particle_system_main_module.playOnAwake = false;
      this._particle_system_main_module.simulationSpace = this._particle_system_simulation_space;
      this._particle_system_main_module.simulationSpeed = 0;
      this._particle_system_main_module.startSize = this._size;

      this._particle_system_renderer = this.GetComponent<UnityEngine.ParticleSystemRenderer>();
      //this._particle_system_renderer.renderMode = ParticleSystemRenderMode.Mesh;
      this._particle_system_renderer.alignment = UnityEngine.ParticleSystemRenderSpace.World;

      if (this._gradient == null) {
        this._gradient = new UnityEngine.Gradient {
                                                      colorKeys = new[] {
                                                                            new UnityEngine.
                                                                                GradientColorKey(col : new
                                                                                      UnityEngine.Color(1,
                                                                                        0,
                                                                                        0),
                                                                                  0f),
                                                                            new UnityEngine.
                                                                                GradientColorKey(col : new
                                                                                      UnityEngine.Color(0,
                                                                                        1,
                                                                                        0),
                                                                                  1f)
                                                                        }
                                                  };
      }
    }

    public override void Display(double value) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying the double " + value + " To " + this.name);
      }
      #endif

      this._Values = new[] {(float)value};
      this.PlotSeries(this._Values);
    }

    public override void Display(float[] values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        var s = "";
        foreach (var value in values) {
          s += $"{value},";
        }

        UnityEngine.Debug.Log(message : "Applying the float array " + s + " To " + this.name);
      }
      #endif
      this._Values = values;
      this.PlotSeries(points : values);
    }

    public override void Display(string values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying the float array " + values + " To " + this.name);
      }
      #endif

      this._vs.Clear();
      foreach (var value in values.Split(',')) {
        this._vs.Add(item : float.Parse(s : value));
      }

      this._Values = this._vs.ToArray();
      this.PlotSeries(this._Values);
    }

    public override void Display(UnityEngine.Vector3 value) { throw new System.NotImplementedException(); }
    public override void Display(UnityEngine.Vector3[] value) { this.ScatterPlot(points : value); }

    public override void Display(droid.Runtime.Structs.Points.ValuePoint points) {
      this.PlotSeries(points : new[] {points});
    }

    public override void Display(droid.Runtime.Structs.Points.ValuePoint[] points) {
      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new UnityEngine.ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        var points_str = System.Linq.Enumerable.Aggregate(source : points,
                                                          "",
                                                          (current, point) =>
                                                              current
                                                              + $"({point._Pos.ToString()}, {point._Val},{point._Size})"
                                                              + ", ");
        UnityEngine.Debug.Log(message : "Applying the points " + points_str + " to " + this.name);
      }
      #endif

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        var clamped = System.Math.Min(val1 : System.Math.Max(0.0f, val2 : point._Val), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = point._Size;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    public override void Display(droid.Runtime.Structs.Points.StringPoint point) {
      throw new System.NotImplementedException();
    }

    public override void Display(droid.Runtime.Structs.Points.StringPoint[] points) {
      throw new System.NotImplementedException();
    }

    //public override void Display(Object o) { throw new NotImplementedException(); }

    public override void Display(float values) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying the float " + values + " To " + this.name);
      }
      #endif

      this._Values = new[] {values};
      this.PlotSeries(this._Values);
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public void ScatterPlot(UnityEngine.Vector3[] points) {
      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new UnityEngine.ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        var points_str =
            System.Linq.Enumerable.Aggregate(source : points, "", (current, point) => current + point + ", ");
        UnityEngine.Debug.Log(message : "Applying the points " + points_str + " To " + this.name);
      }
      #endif

      var i = 0;
      var l = (float)points.Length;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point;
        var clamped = System.Math.Min(val1 : System.Math.Max(0.0f, val2 : i / l), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = this._size;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    public void PlotSeries(float[] points) {
      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new UnityEngine.ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying the series " + points + " To " + this.name);
      }
      #endif

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = UnityEngine.Vector3.one * i;
        var clamped = System.Math.Min(val1 : System.Math.Max(0.0f, val2 : point), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = this._size;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points) {
      var alive = this._particle_system.GetParticles(particles : this._particles);
      if (alive < points.Length) {
        this._particles = new UnityEngine.ParticleSystem.Particle[points.Length];
      }

      var i = 0;
      foreach (var point in points) {
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        this._particles[i].startColor = this._gradient.Evaluate(time : point._Val);
        this._particles[i].startSize = point._Size;
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }
  }
}