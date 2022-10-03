﻿namespace droid.Runtime.Utilities.Procedural {
  public static class NeodroidUtilities {
    /// <summary>
    /// </summary>
    /// <param name="length"></param>
    /// <returns></returns>
    public static System.Collections.Generic.IEnumerable<UnityEngine.Vector3> SnakeSpaceFillingGenerator(
        int length = 100) {
      var x = 0;
      var y = 0;
      var state = GeneratorState.Expand_x_;

      var out_vectors = new UnityEngine.Vector3[length];
      if (length == 0) {
        return out_vectors;
      }

      for (var i = 0; i < length; i++) {
        switch (state) {
          case GeneratorState.Expand_x_:
            x += 1;
            state = GeneratorState.Inc_y_;
            break;

          case GeneratorState.Inc_x_:
            x += 1;
            if (y == x) {
              state = GeneratorState.Dec_y_;
            }

            break;
          case GeneratorState.Dec_x_:
            x -= 1;
            if (x == 0) {
              state = GeneratorState.Expand_y_;
            }

            break;
          case GeneratorState.Expand_y_:
            y += 1;
            state = GeneratorState.Inc_x_;
            break;
          case GeneratorState.Inc_y_:
            y += 1;
            if (y == x) {
              state = GeneratorState.Dec_x_;
            }

            break;
          case GeneratorState.Dec_y_:
            y -= 1;
            if (y == 0) {
              state = GeneratorState.Expand_x_;
            }

            break;
          default: throw new System.ArgumentOutOfRangeException();
        }

        out_vectors[i] = new UnityEngine.Vector3(x : x, 0, z : y);
      }

      return out_vectors;
    }

    #region Nested type: GeneratorState

    /// <summary>
    /// </summary>
    enum GeneratorState {
      Expand_x_ = 0,
      Expand_y_ = 1,

      Inc_x_ = 2,
      Dec_x_ = 3,

      Inc_y_ = 4,
      Dec_y_ = 5
    }

    #endregion
  }
}