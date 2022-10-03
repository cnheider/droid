namespace droid.Runtime.Structs.Space.Sample {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct SampleSpace2 : droid.Runtime.Interfaces.ISamplable {
    public SampleSpace2(string unused = null) {
      this._space = Space2.ZeroOne;
      this._distribution_sampler = new droid.Runtime.Sampling.DistributionSampler();
    }

    /// <summary>
    /// </summary>
    public droid.Runtime.Sampling.DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public dynamic Sample() {
      float x;
      float y;
      switch (this._space.Normalised) {
        case droid.Runtime.Enums.ProjectionEnum.None_:
          x = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.x,
                                                               max : this._space.Max.x,
                                                               granularity : this._space.DecimalGranularity));
          y = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.y,
                                                               max : this._space.Max.y,
                                                               granularity : this._space.DecimalGranularity));
          break;
        case droid.Runtime.Enums.ProjectionEnum.Zero_one_:
          x = this.DistributionSampler.Range(0, 1);
          y = this.DistributionSampler.Range(0, 1);
          break;
        case droid.Runtime.Enums.ProjectionEnum.Minus_one_one_:
          x = this.DistributionSampler.Range(-1, 1);
          y = this.DistributionSampler.Range(-1, 1);
          break;
        default: throw new System.ArgumentOutOfRangeException();
      }

      return new UnityEngine.Vector2(x : x, y : y);
    }

    public droid.Runtime.Interfaces.ISpace Space {
      get { return this._space; }
      set { this._space = (Space2)value; }
    }

    #region Fields

    [UnityEngine.HeaderAttribute("Sampling", order = 103)]
    [UnityEngine.SerializeField]
    internal Space2 _space;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    internal droid.Runtime.Sampling.DistributionSampler _distribution_sampler;

    #endregion
  }
}