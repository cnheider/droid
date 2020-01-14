﻿using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using UnityEngine;

namespace droid.Runtime.Structs.Space.Sample {
  /// <inheritdoc />
  ///  <summary>
  ///  </summary>
  [Serializable]
  public struct SampleSpace2 : ISamplable {
    #region Fields

    [Header("Sampling", order = 103)]
    [SerializeField]
    internal Space2 _space;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    internal DistributionSampler _distribution_sampler;

    #endregion

    /// <summary>
    ///
    /// </summary>
    public DistributionSampler DistributionSampler {
      get { return this._distribution_sampler; }
      set { this._distribution_sampler = value; }
    }

    public SampleSpace2(string unused = null) {
      this._space = Space2.ZeroOne;
      this._distribution_sampler = new DistributionSampler();
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public dynamic Sample() {
      Single x;
      Single y;
      switch(this._space.Normalised) {
        case NormalisationEnum.None_:
          x = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.x, max : this._space.Max.x,granularity : this._space.DecimalGranularity));
          y = this._space.Round(this.DistributionSampler.Range(min : this._space.Min.y, max : this._space.Max.y,granularity : this._space.DecimalGranularity));
          break;
        case NormalisationEnum.Zero_one_:
          x = this.DistributionSampler.Range(0, 1);
          y = this.DistributionSampler.Range(0, 1);
          break;
        case NormalisationEnum.Minus_one_one_:
          x = this.DistributionSampler.Range(-1, 1);
          y = this.DistributionSampler.Range(-1, 1);
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      return new Vector2(x : x, y : y);
    }

    public ISpace Space { get { return this._space; } set { this._space = (Space2)value; } }
  }
}
