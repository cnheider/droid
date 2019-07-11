﻿using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Structs;

namespace droid.Runtime.Interfaces {
  /// <summary>
  /// </summary>
  public interface IHasSingle {
    /// <summary>
    /// </summary>
    float ObservationValue { get; }

    /// <summary>
    /// </summary>
    Space1 SingleSpace { get; }
  }
}
