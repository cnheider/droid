﻿using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers.BoundingBox {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ObserverComponentMenuPath._ComponentMenuPath
                    + "Experimental/BoundingBox"
                    + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [RequireComponent(typeof(Utilities.BoundingBoxes.BoundingBox))]
  public class BoundingBoxObserver : Observer,
                                     IHasString {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "BoundingBox"; } }

    Utilities.BoundingBoxes.BoundingBox _bounding_box;
    [SerializeField] string _observationValue;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._bounding_box = this.GetComponent<Utilities.BoundingBoxes.BoundingBox>();
    }

    public override IEnumerable<float> FloatEnumerable { get { return new List<float>(); } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._bounding_box.BoundingBoxCoordinatesWorldSpaceAsJson;
    }

    /// <summary>
    ///
    /// </summary>
    public string ObservationValue {
      get { return this._observationValue; }
      set { this._observationValue = value; }
    }

    public override string ToString() { return this.ObservationValue; }
  }
}
