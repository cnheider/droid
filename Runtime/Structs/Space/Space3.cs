namespace droid.Runtime.Structs.Space {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct Space3 : droid.Runtime.Interfaces.ISpace {
    [UnityEngine.SerializeField] droid.Runtime.Enums.ProjectionEnum _projection; //TODO use!
    [UnityEngine.SerializeField] bool _clipped;

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3 Span { get { return this._max - this._min; } }

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
    public Space1 Zspace {
      get {
        return new Space1 {
                              Min = this._min.z,
                              Max = this._max.z,
                              DecimalGranularity = this.DecimalGranularity
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

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public static Space3 ZeroOne {
      get {
        return new Space3 {
                              _min = UnityEngine.Vector3.zero,
                              Max = UnityEngine.Vector3.one,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    /// </summary>
    public static Space3 TwentyEighty {
      get {
        return new Space3 {
                              Min = UnityEngine.Vector3.one * 0.2f,
                              _max = UnityEngine.Vector3.one * 0.8f,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    /// </summary>
    public static Space3 MinusOneOne {
      get {
        return new Space3 {
                              _min = -UnityEngine.Vector3.one,
                              Max = UnityEngine.Vector3.one,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

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

    public bool Clipped { get { return this._clipped; } set { this._clipped = value; } }

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

    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public dynamic Min { get { return this._min; } set { this._min = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public dynamic Max { get { return this._max; } set { this._max = value; } }

    dynamic ClipNormaliseMinusOneOneRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(NormaliseMinusOneOne(v : v));
    }

    dynamic ClipRoundDenormaliseMinusOneOneClip(dynamic configuration_configurable_value) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : Vector3.zero,
                                                                  max : Vector3.one)
      #endif

      return this.Clip(v : this.Round(this.DenormaliseMinusOneOne(v : configuration_configurable_value)));
    }

    /// <summary>
    ///   If max is  less than min, no clipping is performed.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static UnityEngine.Vector3
        Clip(UnityEngine.Vector3 v, UnityEngine.Vector3 min, UnityEngine.Vector3 max) {
      return new UnityEngine.Vector3(x : max.x < min.x
                                             ? v.x
                                             : UnityEngine.Mathf.Clamp(value : v.x, min : min.x, max : max.x),
                                     y : max.y < min.y
                                             ? v.y
                                             : UnityEngine.Mathf.Clamp(value : v.y, min : min.y, max : max.y),
                                     z : max.z < min.z
                                             ? v.z
                                             : UnityEngine.Mathf.Clamp(value : v.z,
                                                                       min : min.z,
                                                                       max : max.z));
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public UnityEngine.Vector3 Clip(UnityEngine.Vector3 v) {
      return Clip(v : v, min : this._min, max : this._max);
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    UnityEngine.Vector3 ClipRound(UnityEngine.Vector3 v) { return this.Clip(v : this.Round(v : v)); }

    dynamic ClipRoundDenormalise01Clip(dynamic configuration_configurable_value) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
configuration_configurable_value = Clip(v : configuration_configurable_value,
                                                                  min : Vector3.zero,
                                                                  max : Vector3.one)
      #endif

      return this.Clip(v : this.Round(this.Denormalise01(v : configuration_configurable_value)));
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormalise01Round(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
v = Clip(v : v);
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
    public UnityEngine.Vector3 Round(UnityEngine.Vector3 v) {
      v.x = this.Round(v : v.x);
      v.y = this.Round(v : v.y);
      v.z = this.Round(v : v.z);
      return v;
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator+(Space3 b, UnityEngine.Vector3 c) {
      b._min += c;
      b._max += c;
      return b;
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(Space3 b, UnityEngine.Vector3 c) {
      b._min -= c;
      b._max -= c;
      return b;
    }

    /// <summary>
    /// </summary>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns></returns>
    public static Space3 operator-(UnityEngine.Vector3 c, Space3 b) {
      b._min -= c;
      b._max -= c;
      return b;
    }

    /// <summary>
    /// </summary>
    /// <param name="c"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space3 operator+(UnityEngine.Vector3 c, Space3 b) {
      b._min += c;
      b._max += c;
      return b;
    }

    /// <summary>
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space3 operator*(Space3 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    ///   Return Space3 with the negative and positive extents respectively as min and max for each dimension
    /// </summary>
    /// <param name="bounds_extents"></param>
    /// <param name="normalised"></param>
    /// <param name="decimal_granularity"></param>
    /// <returns></returns>
    public static Space3 FromCenterExtents(UnityEngine.Vector3 bounds_extents,
                                           droid.Runtime.Enums.ProjectionEnum normalised =
                                               droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                                           int decimal_granularity = 4) {
      return new Space3 {
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
    dynamic NormaliseMinusOneOne(dynamic v) { // TODO: Finish cases
      if (v.x > this._max.x
          || v.y > this._max.y
          || v.z > this._max.z
          || v.x < this._min.x
          || v.y < this._min.y
          || v.z < this._min.z) {
        throw new System.ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span.x > 0 && this.Span.y > 0 && this.Span.z > 0) {
        v = Normalisation.NormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
      } else if (this.Span.x > 0 && this.Span.y <= 0) {
        v.x = Normalisation.NormaliseMinusOneOne_(v : v.x, min : this._min.x, span : this.Span.x);
        v.y = 0;
      } else if (this.Span.x <= 0 && this.Span.y >= 0) {
        v.x = 0;
        v.y = Normalisation.NormaliseMinusOneOne_(v : v.y, min : this._min.y, span : this.Span.y);
      } else {
        v.x = 0;
        v.y = 0;
        v.z = 0;
      }

      return v;
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    UnityEngine.Vector3 DenormaliseMinusOneOne(UnityEngine.Vector3 v) {
      if (v.x > 1 || v.y > 1 || v.z > 1 || v.x < -1 || v.y < -1 || v.z < -1) {
        throw new System.ArgumentException(message : $"Value was {v}, min:-1, max:1");
      }

      if (this.Span.x <= 0) { //TODO: FINISH cases
        if (this.Span.y <= 0) {
          return new UnityEngine.Vector3(0, 0);
        }

        return new UnityEngine.Vector3(0,
                                       y : Normalisation.DenormaliseMinusOneOne_(v : v.y,
                                         min : this._min.y,
                                         span : this.Span.y));
      }

      if (this.Span.y <= 0) {
        if (this.Span.x <= 0) {
          return new UnityEngine.Vector3(0, 0);
        }

        return new UnityEngine.Vector3(x : Normalisation.DenormaliseMinusOneOne_(v : v.x,
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
    UnityEngine.Vector3 Denormalise01(UnityEngine.Vector3 v) {
      if (v.x > 1 || v.y > 1 || v.z > 1 || v.x < 0 || v.y < 0 || v.z < 0) {
        throw new System.ArgumentException(message : $"Value was {v}, min:0, max:1");
      }

      return Normalisation.Denormalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    UnityEngine.Vector3 Normalise01(UnityEngine.Vector3 v) {
      if (v.x > this._max.x
          || v.y > this._max.y
          || v.z > this._max.z
          || v.x < this._min.x
          || v.y < this._min.y
          || v.z < this._min.z) {
        throw new System.ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span.x > 0 && this.Span.y > 0 && this.Span.z > 0) { //TODO: Complete variations
        v = this.Round(v : Normalisation.Normalise01_(v : v, min : this._min, span : this.Span));
      } else if (this.Span.x > 0 && this.Span.y > 0 && this.Span.z <= 0) {
        v.x = this.Round(v : Normalisation.Normalise01_(v : v.x, min : this._min.x, span : this.Span.x));
        v.y = this.Round(v : Normalisation.Normalise01_(v : v.y, min : this._min.y, span : this.Span.y));
        v.z = 0;
      } else if (this.Span.x > 0 && this.Span.y <= 0 && this.Span.z <= 0) {
        v.x = this.Round(v : Normalisation.Normalise01_(v : v.x, min : this._min.x, span : this.Span.x));
        v.y = 0;
        v.z = 0;
      } else {
        v.x = 0;
        v.y = 0;
        v.z = 0;
      }

      return v;
    }

    #region Fields

    public droid.Runtime.Enums.ProjectionEnum Normalised {
      get { return this.normalised; }
      set { this.normalised = value; }
    }

    [UnityEngine.HeaderAttribute("Space", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector3 _min;

    [UnityEngine.SerializeField] UnityEngine.Vector3 _max;

    [UnityEngine.RangeAttribute(-1, 15)]
    [UnityEngine.SerializeField]
    int _decimal_granularity;

    [UnityEngine.SerializeField] droid.Runtime.Enums.ProjectionEnum normalised;

    #endregion
  }
}