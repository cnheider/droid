namespace droid.Runtime.Structs.Space.Sample {
  /// <inheritdoc cref="Interfaces.ISpace" />
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct SampleSpace1 : droid.Runtime.Interfaces.ISamplable {
    public SampleSpace1(string unused = null) {
      this._space = Space1.ZeroOne;
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
      switch (this._space.Normalised) {
        case droid.Runtime.Enums.ProjectionEnum.None_:
          return this._space.Round(v : this.DistributionSampler.Range(min : this._space.Min,
                                                                      max : this._space.Max,
                                                                      granularity : this._space
                                                                          .DecimalGranularity));
        case droid.Runtime.Enums.ProjectionEnum.Zero_one_:
          return this.DistributionSampler.Range(0, 1);
        case droid.Runtime.Enums.ProjectionEnum.Minus_one_one_:
          return this.DistributionSampler.Range(-1, 1);
        default: throw new System.ArgumentOutOfRangeException();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Interfaces.ISpace Space {
      get { return this._space; }
      set { this._space = (Space1)value; }
    }

    #region Fields

    [UnityEngine.HeaderAttribute("Sampling", order = 103)]
    [UnityEngine.SerializeField]
    internal Space1 _space;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    internal droid.Runtime.Sampling.DistributionSampler _distribution_sampler;

    #endregion
  }
}