namespace droid.Runtime.Prototyping.Displayers.ScatterPlots {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  [UnityEngine.AddComponentMenu(menuName : DisplayerComponentMenuPath._ComponentMenuPath
                                           + "ScatterPlot"
                                           + DisplayerComponentMenuPath._Postfix)]
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.ParticleSystem))]
  public class ScatterPlotDisplayer : Displayer {
    [UnityEngine.SerializeField] UnityEngine.Gradient _gradient;

    [UnityEngine.SerializeField]
    UnityEngine.ParticleSystemSimulationSpace _particle_system_simulation_space =
        UnityEngine.ParticleSystemSimulationSpace.World;

    [UnityEngine.SerializeField] float _default_start_size = 0.6f;
    UnityEngine.ParticleSystem _particle_system;

    UnityEngine.ParticleSystem.MainModule _particle_system_main_module;
    UnityEngine.ParticleSystemRenderer _particle_system_renderer;

    UnityEngine.ParticleSystem.Particle[] _particles;

    System.Collections.Generic.List<float> _vs = new System.Collections.Generic.List<float>();

    /// <inheritdoc />
    /// <summary>
    /// </summary>
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
      this._particle_system_main_module.startSize = this._default_start_size;

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
        for (var index = 0; index < values.Length; index++) {
          var value = values[index];
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
      for (var index = 0; index < values.Split(',').Length; index++) {
        var value = values.Split(',')[index];
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
      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        var clamped = System.Math.Min(val1 : System.Math.Max(0.0f, val2 : point._Val), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = point._Size;
        this._particles[i].startSize3D =
            droid.Runtime.Utilities.Extensions.NeodroidVectorUtilities
                 .BroadcastVector3(a : this._default_start_size);
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

    /// <summary>
    /// </summary>
    /// <param name="values"></param>
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
      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point;
        var clamped = System.Math.Min(val1 : System.Math.Max(0.0f, val2 : i / l), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = this._default_start_size;
        this._particles[i].startSize3D =
            droid.Runtime.Utilities.Extensions.NeodroidVectorUtilities
                 .BroadcastVector3(a : this._default_start_size);
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    public void PlotSeries(float[] points) {
      if (!this._particle_system) {
        return;
      }

      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new UnityEngine.ParticleSystem.Particle[points.Length];
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying the series " + points + " To " + this.name);
      }
      #endif

      var i = 0;
      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = UnityEngine.Vector3.one * i;
        var clamped = System.Math.Min(val1 : System.Math.Max(0.0f, val2 : point), 1.0f);
        this._particles[i].startColor = this._gradient.Evaluate(time : clamped);
        this._particles[i].startSize = this._default_start_size;
        this._particles[i].startSize3D =
            droid.Runtime.Utilities.Extensions.NeodroidVectorUtilities
                 .BroadcastVector3(a : this._default_start_size);
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Clean() {
      if (!this._RetainLastPlot) {
        if (this._particle_system) {
          this._particle_system.Clear(true);
        }
      }

      base.Clean();
    }

    /// <summary>
    /// </summary>
    /// <param name="points"></param>
    public override void PlotSeries(droid.Runtime.Structs.Points.ValuePoint[] points) {
      if (!this._particle_system) {
        return;
      }

      if (this._particles == null || this._particles.Length != points.Length) {
        this._particles = new UnityEngine.ParticleSystem.Particle[points.Length];
      }
      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : "Applying the series " + points + " To " + this.name);
      }
      #endif

      var alive = this._particle_system.GetParticles(particles : this._particles);
      if (alive < points.Length) {
        this._particles = new UnityEngine.ParticleSystem.Particle[points.Length];
      }

      var i = 0;
      for (var index = 0; index < points.Length; index++) {
        var point = points[index];
        this._particles[i].remainingLifetime = 100000;
        this._particles[i].position = point._Pos;
        this._particles[i].startColor = this._gradient.Evaluate(time : point._Val);
        this._particles[i].startSize = point._Size;
        this._particles[i].startSize3D =
            droid.Runtime.Utilities.Extensions.NeodroidVectorUtilities.BroadcastVector3(a : point._Size);
        i++;
      }

      this._particle_system.SetParticles(particles : this._particles, size : points.Length);
    }
  }
}