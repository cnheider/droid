namespace droid.Runtime.Structs.Space {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct Space2 : droid.Runtime.Interfaces.ISpace {
    [UnityEngine.SerializeField] droid.Runtime.Enums.ProjectionEnum _projection; //TODO use!
    [UnityEngine.SerializeField] bool _clipped;

    /// <summary>
    /// </summary>
    public UnityEngine.Vector2 Span { get { return this._max - this._min; } }

    /// <summary>
    /// </summary>
    public Space1 Xspace {
      get {
        return new Space1 {
                              Min = this._min.x,
                              Max = this._max.x,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    /// <summary>
    /// </summary>
    public Space1 Yspace {
      get {
        return new Space1 {
                              Min = this._min.y,
                              Max = this._max.y,
                              DecimalGranularity = this.DecimalGranularity
                          };
      }
    }

    /// <summary>
    /// </summary>
    public static Space2 ZeroOne {
      get {
        return new Space2 {
                              _min = UnityEngine.Vector2.zero,
                              Max = UnityEngine.Vector2.one,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    /// </summary>
    public static Space2 TwentyEighty {
      get {
        return new Space2 {
                              _min = UnityEngine.Vector2.one * 0.2f,
                              Max = UnityEngine.Vector2.one * 0.8f,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    /// </summary>
    public static Space2 MinusOneOne {
      get {
        return new Space2 {
                              _min = -UnityEngine.Vector2.one,
                              Max = UnityEngine.Vector2.one,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    /// </summary>
    public bool NormalisedBool {
      get { return this._projection == droid.Runtime.Enums.ProjectionEnum.Zero_one_; }
      set {
        this._projection =
            value ? droid.Runtime.Enums.ProjectionEnum.Zero_one_ : droid.Runtime.Enums.ProjectionEnum.None_;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

    public bool Clipped { get { return this._clipped; } set { this._clipped = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Project(dynamic v) {
      if (this.Clipped) {
        v = this.Clip(v : v);
      }

      switch (this.Normalised) {
        case droid.Runtime.Enums.ProjectionEnum.None_:
          return v;
        case droid.Runtime.Enums.ProjectionEnum.Zero_one_:
          return ClipNormalise01Round(v : v);
        case droid.Runtime.Enums.ProjectionEnum.Minus_one_one_:
          return ClipNormaliseMinusOneOneRound(v : v);
        default: throw new System.ArgumentOutOfRangeException();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Reproject(dynamic v) {
      switch (this.Normalised) {
        case droid.Runtime.Enums.ProjectionEnum.Zero_one_:
          v = ClipRoundDenormalise01Clip(configuration_configurable_value : v);
          break;
        case droid.Runtime.Enums.ProjectionEnum.Minus_one_one_:
          v = ClipRoundDenormaliseMinusOneOneClip(configuration_configurable_value : v);
          break;
        case droid.Runtime.Enums.ProjectionEnum.None_: break;
        default: throw new System.ArgumentOutOfRangeException();
      }

      if (this.Clipped) {
        v = this.Clip(v : v);
      }

      return v;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public dynamic Min { get { return this._min; } set { this._min = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public dynamic Max { get { return this._max; } set { this._max = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

    /// <summary>
    ///   If max is less than min, no clipping is performed.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public UnityEngine.Vector2 Clip(UnityEngine.Vector2 v, UnityEngine.Vector2 min, UnityEngine.Vector2 max) {
      return new UnityEngine.Vector2(x : max.x < min.x
                                             ? v.x
                                             : UnityEngine.Mathf.Clamp(value : v.x, min : min.x, max : max.x),
                                     y : max.y < min.y
                                             ? v.y
                                             : UnityEngine.Mathf.Clamp(value : v.y,
                                                                       min : min.y,
                                                                       max : max.y));
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public UnityEngine.Vector2 Clip(UnityEngine.Vector2 v) {
      return this.Clip(v : v, min : this._min, max : this._max);
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    UnityEngine.Vector2 ClipRound(UnityEngine.Vector2 v) { return this.Clip(v : this.Round(v : v)); }

    dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : Vector2.zero,
                                                                  max : Vector2.one)
      #endif

      return this.Clip(v : this.Round(this.Denormalise01(v : configuration_configurable_value)));
    }

    dynamic ClipNormaliseMinusOneOneRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v)
      #endif

      return this.Round(this.NormaliseMinusOneOne(v : v));
    }

    dynamic ClipRoundDenormaliseMinusOneOneClip(dynamic configuration_configurable_value) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : -Vector2.one,
                                                                  max : Vector2.one)
      #endif

      return this.Clip(v : this.Round(this.DenormaliseMinusOneOne(v : configuration_configurable_value)));
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormalise01Round(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v)
      #endif

      return this.Round(Normalise01(v : v));
    }

    /// <summary>
    ///   if Decimal granularity is negative no rounding is performed
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) {
      return this.DecimalGranularity >= 0
                 ? (float)System.Math.Round(value : v, digits : this.DecimalGranularity)
                 : v;
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public UnityEngine.Vector2 Round(UnityEngine.Vector2 v) {
      v.x = this.Round(v : v.x);
      v.y = this.Round(v : v.y);
      return v;
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space2 operator*(Space2 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    /// </summary>
    /// <param name="bounds_extents"></param>
    /// <param name="normalised"></param>
    /// <param name="decimal_granularity"></param>
    /// <returns></returns>
    public static Space2 FromCenterExtents(UnityEngine.Vector2 bounds_extents,
                                           droid.Runtime.Enums.ProjectionEnum normalised =
                                               droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                                           int decimal_granularity = 4) {
      return new Space2 {
                            _min = -bounds_extents,
                            Max = bounds_extents,
                            normalised = normalised,
                            _decimal_granularity = decimal_granularity
                        };
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    UnityEngine.Vector2 Denormalise01(UnityEngine.Vector2 v) {
      if (v.x > 1 || v.y > 1 || v.x < 0 || v.y < 0) {
        throw new System.ArgumentException(message : $"Value was {v}, min:0, max:1");
      }

      if (this.Span.x <= 0) {
        if (this.Span.y <= 0) {
          return new UnityEngine.Vector2(0, 0);
        }

        return new UnityEngine.Vector2(0,
                                       y : Normalisation.Denormalise01_(v : v.y,
                                                                        min : this._min.y,
                                                                        span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new UnityEngine.Vector2(0, 0);
        }

        return new UnityEngine.Vector2(x : Normalisation.Denormalise01_(v : v.x,
                                                                        min : this._min.x,
                                                                        span : this.Span.x),
                                       0);
      }

      return Normalisation.Denormalise01_(v : v, min : this._min, span : this.Span);
    }

    UnityEngine.Vector2 DenormaliseMinusOneOne(UnityEngine.Vector2 v) {
      if (v.x > 1 || v.y > 1 || v.x < -1 || v.y < -1) {
        throw new System.ArgumentException(message : $"Value was {v}, min:-1, max:1");
      }

      if (this.Span.x <= 0) {
        if (this.Span.y <= 0) {
          return new UnityEngine.Vector2(0, 0);
        }

        return new UnityEngine.Vector2(0,
                                       y : Normalisation.DenormaliseMinusOneOne_(v : v.y,
                                         min : this._min.y,
                                         span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new UnityEngine.Vector2(0, 0);
        }

        return new UnityEngine.Vector2(x : Normalisation.DenormaliseMinusOneOne_(v : v.x,
                                         min : this._min.x,
                                         span : this.Span.x),
                                       0);
      }

      return Normalisation.DenormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    UnityEngine.Vector2 Normalise01(UnityEngine.Vector2 v) {
      if (v.x > this._max.x || v.y > this._max.y || v.x < this._min.x || v.y < this._min.y) {
        throw new System.ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span.x <= 0) {
        if (this.Span.y <= 0) {
          return new UnityEngine.Vector2(0, 0);
        }

        return new UnityEngine.Vector2(0,
                                       y : Normalisation.Normalise01_(v : v.y,
                                                                      min : this._min.y,
                                                                      span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new UnityEngine.Vector2(0, 0);
        }

        return new UnityEngine.Vector2(x : Normalisation.Normalise01_(v : v.x,
                                                                      min : this._min.x,
                                                                      span : this.Span.x),
                                       0);
      }

      return Normalisation.Normalise01_(v : v, min : this._min, span : this.Span);
    }

    dynamic NormaliseMinusOneOne(dynamic v) {
      if (v.x > this._max.x || v.y > this._max.y || v.x < this._min.x || v.y < this._min.y) {
        throw new System.ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span.x > 0 && this.Span.y > 0) {
        v = this.Round(Normalisation.NormaliseMinusOneOne_(v : v, min : this._min, span : this.Span));
      } else if (this.Span.x > 0 && this.Span.y <= 0) {
        v.x = this.Round(Normalisation.NormaliseMinusOneOne_(v : v.x, min : this._min.x, span : this.Span.x));
        v.y = 0;
      } else if (this.Span.x <= 0 && this.Span.y >= 0) {
        v.x = 0;
        v.y = this.Round(Normalisation.NormaliseMinusOneOne_(v : v.y, min : this._min.y, span : this.Span.y));
      } else {
        v.x = 0;
        v.y = 0;
      }

      return v;
    }

    #region Fields

    public droid.Runtime.Enums.ProjectionEnum Normalised {
      get { return this.normalised; }
      set { this.normalised = value; }
    }

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Space", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector2 _min;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Vector2 _max;

    [UnityEngine.RangeAttribute(-1, 15)]
    [UnityEngine.SerializeField]
    int _decimal_granularity;

    [UnityEngine.SerializeField] droid.Runtime.Enums.ProjectionEnum normalised;

    #endregion
  }
}