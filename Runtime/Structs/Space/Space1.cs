//#define ALWAYS_PRE_CLIP_PROJECTIONS
//#define ZERO_RETURN_NEGATIVE_SPAN

namespace droid.Runtime.Structs.Space {
  /// <inheritdoc cref="Interfaces.ISpace" />
  ///  <summary>
  ///  </summary>
  [System.SerializableAttribute]
  public struct Space1 : droid.Runtime.Interfaces.ISpace {
    /// <summary>
    ///
    /// </summary>
    public float Span { get { return this._max - this._min; } }

    /// <summary>
    ///
    /// </summary>
    public static Space1 TwentyEighty {
      get {
        return new Space1 {
                              _min = 0.2f,
                              _max = 0.8f,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 ZeroOne {
      get {
        return new Space1 {
                              _min = 0,
                              _max = 1,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 MinusOneOne {
      get {
        return new Space1 {
                              _min = -1,
                              _max = 1,
                              DecimalGranularity = 4,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                              Clipped = true
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public bool NormalisedBool {
      get { return this._projection == droid.Runtime.Enums.ProjectionEnum.Zero_one_; }
      set { this._projection = value ? droid.Runtime.Enums.ProjectionEnum.Zero_one_ : droid.Runtime.Enums.ProjectionEnum.None_; }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 DiscreteMinusOneOne {
      get {
        return new Space1 {
                              _min = -1,
                              _max = 1,
                              DecimalGranularity = 0,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.None_,
                              Clipped = false
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public static Space1 DiscreteZeroOne {
      get {
        return new Space1 {
                              _min = 0,
                              _max = 1,
                              DecimalGranularity = 0,
                              Normalised = droid.Runtime.Enums.ProjectionEnum.None_,
                              Clipped = false
                          };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public dynamic Precision {
      get {
        if (this._decimal_granularity < 0) {
          return float.PositiveInfinity;
        }

        return 1.0f / (this._decimal_granularity + 1.0f);
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="v"></param>
    ///  <returns></returns>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Project(dynamic v) {
      if (this.Clipped) {
        v = this.Clip(v : v);
      }

      switch (this.Normalised) {
        case droid.Runtime.Enums.ProjectionEnum.None_:
          return v;
        case droid.Runtime.Enums.ProjectionEnum.Zero_one_:
          return ClipNormaliseRound(v : v);
        case droid.Runtime.Enums.ProjectionEnum.Minus_one_one_:
          return ClipNormaliseMinusOneOneRound(v : v);
        default: throw new System.ArgumentOutOfRangeException();
      }
    }

    public bool Clipped { get { return this._clipped; } set { this._clipped = value; } }

    public dynamic Mean { get { return (this.Max + this.Min) * 0.5f; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="v"></param>
    ///  <returns></returns>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public dynamic Reproject(dynamic v) {
      switch (this.Normalised) {
        case droid.Runtime.Enums.ProjectionEnum.Zero_one_:
          v = ClipRoundDenormalise01Clip(v : v);
          break;
        case droid.Runtime.Enums.ProjectionEnum.Minus_one_one_:
          v = ClipRoundDenormaliseMinusOneOneClip(v : v);
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
    ///  <summary>
    ///  </summary>
    public int DecimalGranularity {
      get { return this._decimal_granularity; }
      set { this._decimal_granularity = value; }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Min { get { return this._min; } set { this._min = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public dynamic Max { get { return this._max; } set { this._max = value; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public droid.Runtime.Enums.ProjectionEnum Normalised { get { return this._projection; } set { this._projection = value; } }

    /// <summary>
    /// If max is less than min, no clipping is performed.
    /// </summary>
    /// <param name="v"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    static float Clip(float v, float min, float max) {
      #if ZERO_RETURN_NEGATIVE_SPAN
        return max < min ? 0 : Mathf.Clamp(value : v, min : min, max : max);
      #else
        return max < min ? v : UnityEngine.Mathf.Clamp(value : v, min : min, max : max);
      #endif
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float Clip(float v) { return Clip(v : v, min : this._min, max : this._max); }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float RoundClip(float v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(v : v);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float ClipRound(float v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v);
      #endif

      return this.Round(v : v);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormaliseRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = this.Clip(v);
      #endif

      return this.Round(v : this.Normalise01(v : v));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipNormaliseMinusOneOneRound(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = this.Clip( v);
      #endif

      return this.Round(v : this.NormaliseMinusOneOne(v : v));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public UnityEngine.Vector2 ToVector2() { return new UnityEngine.Vector2(x : this._min, y : this._max); }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public UnityEngine.Vector3 ToVector3() {
      return new UnityEngine.Vector3(x : this._min, y : this._max, z : this._decimal_granularity);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipRoundDenormalise01Clip(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v, min : 0, max : 1);
      #endif

      return this.Clip(v : this.Round(v : this.Denormalise01(v : v)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    dynamic ClipRoundDenormaliseMinusOneOneClip(dynamic v) {
      #if ALWAYS_PRE_CLIP_PROJECTIONS
      v = Clip(v : v, min : -1, max : 1);
      #endif

      return this.Clip(v : this.Round(v : this.DenormaliseMinusOneOne(v : v)));
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public static string Vector3Description() { return "Space (min, max, granularity)"; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    public float Round(float v) {
      return this.DecimalGranularity >= 0
                 ? (float)System.Math.Round(value : v, digits : this._decimal_granularity)
                 : v;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="vector3_field"></param>
    public void FromVector3(UnityEngine.Vector3 vector3_field) {
      this._decimal_granularity = (int)vector3_field.z;
      this._max = vector3_field.y;
      this._min = vector3_field.x;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static Space1 operator*(Space1 a, float b) {
      a.Max *= b;
      a.Min *= b;
      return a;
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="extent"></param>
    /// <param name="projection_enum"></param>
    /// <param name="decimal_granularity"></param>
    /// <returns></returns>
    public static Space1 FromCenterExtent(float extent,
                                          droid.Runtime.Enums.ProjectionEnum projection_enum = droid.Runtime.Enums.ProjectionEnum.Zero_one_,
                                          int decimal_granularity = 4) {
      return new Space1 {
                            _min = -extent,
                            Max = extent,
                            _projection = projection_enum,
                            DecimalGranularity = decimal_granularity
                        };
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float Normalise01(float v) {
      if (v > this._max || v < this._min) {
        throw new System.ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.Normalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float NormaliseMinusOneOne(float v) {
      if (v > this._max || v < this._min) {
        throw new System.ArgumentException(message : $"Value was {v}, min:{this._min}, max:{this._max}");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.NormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float Denormalise01(float v) {
      if (v > 1 || v < 0) {
        throw new System.ArgumentException(message : $"Value was {v}, min:0, max:1");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.Denormalise01_(v : v, min : this._min, span : this.Span);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    float DenormaliseMinusOneOne(float v) {
      if (v > 1 || v < -1) {
        throw new System.ArgumentException(message : $"Value was {v}, min:-1, max:1");
      }

      if (this.Span <= 0) {
        return 0;
      }

      return Normalisation.DenormaliseMinusOneOne_(v : v, min : this._min, span : this.Span);
    }

    #region Fields

    /// <summary>
    ///
    /// </summary>
    [UnityEngine.HeaderAttribute("Space", order = 103)]
    [UnityEngine.SerializeField]
    float _min;

    /// <summary>
    ///
    /// </summary>
    [UnityEngine.SerializeField]
    float _max;

    /// <summary>
    ///
    /// </summary>
    ///
    [UnityEngine.RangeAttribute(-1, 15)]
    [UnityEngine.SerializeField]
    int _decimal_granularity;

    [UnityEngine.SerializeField] droid.Runtime.Enums.ProjectionEnum _projection;
    [UnityEngine.SerializeField] bool _clipped;

    #endregion
  }
}