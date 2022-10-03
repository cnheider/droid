﻿namespace droid.Runtime.Sampling {
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public enum DistributionEnum {
    /// <summary>
    /// </summary>
    Uniform_,

    /// <summary>
    /// </summary>
    Normal_,

    /// <summary>
    /// </summary>
    Sloped_,

    /// <summary>
    /// </summary>
    Exponential_,

    /// <summary>
    /// </summary>
    Linear_
  }

  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public struct DistributionSampler {
    [UnityEngine.SerializeField] DistributionEnum _de;
    Distributions.ConfidenceLevel _conf_level;

    public DistributionSampler(DistributionEnum distribution_enum = DistributionEnum.Uniform_,
                               Distributions.DirectionE d = Distributions.DirectionE.Left_) {
      this._de = distribution_enum;
      this._conf_level = Distributions.ConfidenceLevel._95;
      this.Direction = d;
      this.DistributionParameter = 1.267291f;
    }

    /// <summary>
    /// </summary>
    public float DistributionParameter { get; set; }

    /// <summary>
    /// </summary>
    public Distributions.DirectionE Direction { get; set; }

    /// <summary>
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="granularity"></param>
    /// <returns></returns>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public float Range(float min, float max, int granularity = 0) {
      switch (this._de) {
        case DistributionEnum.Uniform_:
          if (granularity == 0) {
            return UnityEngine.Random.Range(minInclusive : (int)min, maxExclusive : (int)max + 1);
          } else {
            return UnityEngine.Random.Range(minInclusive : min, maxInclusive : max);
          }
        case DistributionEnum.Normal_:
          return Distributions.RandomRangeNormalDistribution(min : min,
                                                             max : max,
                                                             confidence_level_cutoff : this._conf_level);
        case DistributionEnum.Sloped_:
          return Distributions.RandomRangeSlope(min : min,
                                                max : max,
                                                skew : this.DistributionParameter,
                                                direction : this.Direction);
        case DistributionEnum.Exponential_:
          return Distributions.RandomRangeExponential(min : min,
                                                      max : max,
                                                      exponent : this.DistributionParameter,
                                                      direction : this.Direction);
        case DistributionEnum.Linear_: return Distributions.RandomLinear(slope : this.DistributionParameter);
        default:
          if (granularity == 0) {
            return UnityEngine.Random.Range(minInclusive : (int)min, maxExclusive : (int)max + 1);
          } else {
            return UnityEngine.Random.Range(minInclusive : min, maxInclusive : max);
          }
      }
    }
  }

  /// <summary>
  /// </summary>
  public static class Distributions {
    #region ConfidenceLevel enum

    //--------------------------------------------------------------------------------------------
    // Normal Distribution
    //--------------------------------------------------------------------------------------------

    public enum ConfidenceLevel {
      _60,
      _80,
      _90,
      _95,
      _98,
      _99,
      _998,
      _999
    }

    #endregion

    #region DirectionE enum

    //--------------------------------------------------------------------------------------------
    // Sloped Distribution (sec^2 distribution)
    //--------------------------------------------------------------------------------------------

    /// <summary>
    /// </summary>
    public enum DirectionE {
      /// <summary>
      /// </summary>
      Right_,

      /// <summary>
      /// </summary>
      Left_
    }

    #endregion

    static float[] _confidence_to_z_score = {
                                                0.84162123f,
                                                1.28155156f,
                                                1.64485363f,
                                                1.95996399f,
                                                2.32634787f,
                                                2.57582931f,
                                                3.0902323f,
                                                3.29052673f
                                            };

    /// <summary>
    ///   Get a random number from a normal distribution in [min,max].
    /// </summary>
    /// <description>
    ///   Get a random number between min [inclusive] and max [inclusive] with probability matching
    ///   a normal distribution along this range. The width of the distribution is described by the
    ///   confidence_level_cutoff, which describes what percentage of the bell curve should be over
    ///   the provided range. For example, a confidence level cutoff of 0.999 will result in a bell
    ///   curve from min to max that contains 99.9% of the area under the complete curve. 0.80 gives
    ///   a curve with 80% of the distribution's area.
    ///   Because a normal distribution flattens of towards the ends, this means that 0.80 will have
    ///   more even distribution between min and max than 0.999.
    /// </description>
    /// <returns>
    ///   A random number between min [inclusive] and max [inclusive], with probability described
    ///   by the distribution.
    /// </returns>
    /// <param name="min">The min value returned [inclusive].</param>
    /// <param name="max">The max min value returned [inclusive].</param>
    /// <param name="confidence_level_cutoff">
    ///   The percentage of a standard normal distribution that should be represented in the range.
    /// </param>
    public static float RandomRangeNormalDistribution(float min,
                                                      float max,
                                                      ConfidenceLevel
                                                          confidence_level_cutoff /*, float confidence_level_cutoff*/) {
      var mean = 0.5f * (min + max);

      // TODO formula for this?
      var z_score_cutoff = _confidence_to_z_score[(int)confidence_level_cutoff];

      var new_width = (max - min) / 2.0f;
      var sigma = new_width / z_score_cutoff;

      // Get random normal from Normal Distribution that's within the confidence level cutoff requested
      float random_normal_num;
      do {
        random_normal_num = RandomNormalDistribution(mean : mean, std_dev : sigma);
      } while (random_normal_num > max || random_normal_num < min);

      // now you have a number selected from a bell curve stretching from min to max!
      return random_normal_num;
    }

    /// <summary>
    ///   Get a random number from a normal distribution with given mean and standard deviation.
    /// </summary>
    /// <description>
    ///   Get a random number with probability following a normal distribution with given mean
    ///   and standard deviation. The likelihood of getting any given number corresponds to
    ///   its value along the y-axis in the distribution described by the parameters.
    /// </description>
    /// <returns>
    ///   A random number between -infinity and infinity, with probability described by the distribution.
    /// </returns>
    /// <param name="mean">The Mean (or center) of the normal distribution.</param>
    /// <param name="std_dev">The Standard Deviation (or Sigma) of the normal distribution.</param>
    public static float RandomNormalDistribution(float mean, float std_dev) {
      // Get random normal from Standard Normal Distribution
      var random_normal_num = RandomFromStandardNormalDistribution();

      // Stretch distribution to the requested sigma variance
      random_normal_num *= std_dev;

      // Shift mean to requested mean:
      random_normal_num += mean;

      // now you have a number selected from a normal distribution with requested mean and sigma!
      return random_normal_num;
    }

    /// <summary>
    ///   Get a random number from the standard normal distribution.
    /// </summary>
    /// <returns>
    ///   A random number in range [-inf, inf] from the standard normal distribution (mean == 1, stand deviation ==
    ///   1).
    /// </returns>
    public static float RandomFromStandardNormalDistribution() {
      // This code follows the polar form of the muller transform:
      // https://en.wikipedia.org/wiki/Box%E2%80%93Muller_transform#Polar_form
      // also known as Marsaglia polar method
      // https://en.wikipedia.org/wiki/Marsaglia_polar_method

      // calculate points on a circle
      float u, v;

      float s; // this is the hypotenuse squared.
      do {
        u = UnityEngine.Random.Range(-1f, 1f);
        v = UnityEngine.Random.Range(-1f, 1f);
        s = u * u + v * v;
      } while (!(s != 0 && s < 1)); // keep going until s is nonzero and less than one

      // TODO allow a user to specify how many random numbers they want!
      // choose between u and v for seed (z0 vs z1)
      float seed;
      if (UnityEngine.Random.Range(0, 2) == 0) {
        seed = u;
      } else {
        seed = v;
      }

      // create normally distributed number.
      var z = seed * UnityEngine.Mathf.Sqrt(f : -2.0f * UnityEngine.Mathf.Log(f : s) / s);

      return z;
    }

    /// <summary>
    ///   Returns a random number in range [min,max] from a curved slope following sec^2(x).
    /// </summary>
    /// <returns>Random in range [min,max] from a curved left slope.</returns>
    /// <param name="max"></param>
    /// <param name="skew">The difference in height between max and min of curve.</param>
    /// <param name="min"></param>
    /// <param name="direction"></param>
    public static float RandomRangeSlope(float min, float max, float skew, DirectionE direction) {
      return min + RandomFromSlopedDistribution(skew : skew, direction : direction) * (max - min);
    }

    /// <summary>
    ///   Returns random in range [0,1] from a curved right slope.
    /// </summary>
    /// <returns>Random in range [0,1] from a curved right slope.</returns>
    /// <param name="skew">The difference in height between max and min of curve.</param>
    /// <param name="direction"></param>
    public static float RandomFromSlopedDistribution(float skew, DirectionE direction) {
      // the difference in scale is just the same as the max y-value..
      var max_y = skew;

      // our curve will go from 0 to max_x.
      var max_x = Inverse_Sec_Squared(y : max_y);

      var max_cdf = Sec_Squared_CumulativeDistribution(x : max_x);

      var u = UnityEngine.Random.Range(0.0f, maxInclusive : max_cdf);
      var x_val = Sec_Squared_InverseCumulativeDistribution(x : u);

      // scale to [0,1]
      var value = x_val / max_x;

      if (direction == DirectionE.Left_) {
        value = 1.0f - value;
      }

      return value;
    }

    /// <summary>
    ///   The inverse of the sec^2 function.
    /// </summary>
    /// <param name="y">
    ///   The y coordinate. if y
    ///   < 1, returns NaN.
    /// </param>
    static float Inverse_Sec_Squared(float y) {
      // Note: arcsec(x) = arccos(1/x)

      // return arcsec(sqrt(y))
      return UnityEngine.Mathf.Acos(f : 1.0f / UnityEngine.Mathf.Sqrt(f : y));
    }

    // The integral of sec^2
    static float Sec_Squared_CumulativeDistribution(float x) {
      // The cumulative distribution function for sec^2 is just the definite integral of sec^2(x) = tan(x) - tan(0) = tan(x)

      return UnityEngine.Mathf.Tan(f : x);
    }

    // The inverse of the integral of sec^2
    static float Sec_Squared_InverseCumulativeDistribution(float x) {
      // The cumulative distribution function for sec^2 is just the definite integral of sec^2(x) = tan(x) - tan(0) = tan(x)
      // Then the Inverse cumulative distribution function is just atan(x)

      return UnityEngine.Mathf.Atan(f : x);
    }

    //--------------------------------------------------------------------------------------------
    // Linear Distribution
    //--------------------------------------------------------------------------------------------

    // Returns random in range [min, max] with linear distribution of given slope.
    public static float RandomRangeLinear(float min, float max, float slope) {
      if (slope == 0) {
        return UnityEngine.Random.Range(minInclusive : min, maxInclusive : max);
      }

      var val = RandomLinear(slope : slope);

      return min + (max - min) * val;
    }

    // Returns random in range [0,1] with linear distribution of given slope.
    public static float RandomLinear(float slope) {
      var abs_value = RandomFromLinearWithPositiveSlope(slope : UnityEngine.Mathf.Abs(f : slope));
      if (slope < 0) {
        return 1 - abs_value;
      }

      return abs_value;
    }

    // Returns random in range [0,1] with linear distribution of given slope.
    static float RandomFromLinearWithPositiveSlope(float slope) {
      if (slope == 0) {
        return UnityEngine.Random.Range(0.0f, 1.0f);
      }

      float x, y;
      do {
        x = UnityEngine.Random.Range(0.0f, 1.0f);
        y = UnityEngine.Random.Range(0.0f, 1.0f);
        if (slope < 1) {
          y -= (1 - slope) / 2.0f;
        }
      } while (y > x * slope);

      return x;
    }

    //--------------------------------------------------------------------------------------------
    // Exponential Distribution
    //--------------------------------------------------------------------------------------------

    /// <summary>
    ///   Returns a random number in range [min,max] from an exponential distribution.
    /// </summary>
    /// <returns>Random number in range [min,max] from given exponential distribution.</returns>
    /// <param name="min">Minimum random number (inclusive).</param>
    /// <param name="max">Maximum random number (inclusive).</param>
    /// <param name="exponent">
    ///   Exponent for distribution. Must be >= 0.
    ///   0 will be uniform distribution; 1 will be linear distribution w/ slope 1.
    /// </param>
    /// <param name="direction">The direction for the curve (right/left).</param>
    public static float RandomRangeExponential(float min, float max, float exponent, DirectionE direction) {
      return min
             + RandomFromExponentialDistribution(exponent : exponent, direction : direction) * (max - min);
    }

    /// <summary>
    ///   Returns a random number in range [0,1] from an exponential distribution.
    /// </summary>
    /// <returns>Random number in range [0,1] from given exponential distribution.</returns>
    /// <param name="exponent">
    ///   Exponent for distribution. Must be >= 0.
    ///   0 will be uniform distribution; 1 will be linear distribution w/ slope 1.
    /// </param>
    /// <param name="direction">The direction for the curve (right/left).</param>
    public static float RandomFromExponentialDistribution(float exponent, DirectionE direction) {
      // our curve will go from 0 to 1.
      var max_cdf = ExponentialRightCdf(1.0f, exponent : exponent);

      var u = UnityEngine.Random.Range(0.0f, maxInclusive : max_cdf);
      var x_val = ExponentialRightInverseCdf(x : u, exponent : exponent);

      if (direction == DirectionE.Left_) {
        x_val = 1.0f - x_val;
      }

      return x_val;
    }

    // The inverse of the curve.
    static float ExponentialRightInverse(float y, float exponent) {
      return UnityEngine.Mathf.Pow(f : y, p : 1.0f / exponent);
    }

    // The integral of the exponent curve.
    static float ExponentialRightCdf(float x, float exponent) {
      var integral_exp = exponent + 1.0f;
      return UnityEngine.Mathf.Pow(f : x, p : integral_exp) / integral_exp;
    }

    // The inverse of the integral of the exponent curve.
    static float ExponentialRightInverseCdf(float x, float exponent) {
      var integral_exp = exponent + 1.0f;
      return UnityEngine.Mathf.Pow(f : integral_exp * x, p : 1.0f / integral_exp);
    }

    //--------------------------------------------------------------------------------------------
    // User-Defined Probability Distribution Function
    //--------------------------------------------------------------------------------------------

    /// <summary>
    ///   Return an index randomly chosen following the distribution specified by a list of probabilities
    /// </summary>
    /// <returns>
    ///   An index in range [0, probabilities.Length) following the distribution specified in probabilites.
    /// </returns>
    /// <param name="probabilities">
    ///   A list of probabilities from which to choose an index. All values must be >= 0!
    /// </param>
    public static int
        RandomChoiceFollowingDistribution(System.Collections.Generic.List<float> probabilities) {
      // Sum to create CDF:
      var cdf = new float[probabilities.Count];
      float sum = 0;
      for (var i = 0; i < probabilities.Count; ++i) {
        cdf[i] = sum + probabilities[index : i];
        sum = cdf[i];
      }

      // Choose from CDF:
      var cdf_value = UnityEngine.Random.Range(0.0f, maxInclusive : cdf[probabilities.Count - 1]);
      var index = System.Array.BinarySearch(array : cdf, value : cdf_value);

      if (index < 0) {
        index = ~index; // if not found (probably won't be) BinarySearch returns bitwise complement of next-highest index.
      }

      return index;
    }
  }
}