namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                                           + "Position2D"
                                           + SensorComponentMenuPath._Postfix)]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class Position2DSensor : Sensor,
                                  droid.Runtime.Interfaces.IHasDouble {
    [UnityEngine.HeaderAttribute("Observation", order = 103)]
    [UnityEngine.SerializeField]
    UnityEngine.Vector2 _2_d_position;

    [UnityEngine.SerializeField]
    [droid.Runtime.Utilities.SearchableEnumAttribute]
    droid.Runtime.Enums.Dimension2DCombinationEnum _dim_combinationEnum =
        droid.Runtime.Enums.Dimension2DCombinationEnum.Xz_;

    [UnityEngine.SerializeField]
    droid.Runtime.Structs.Space.Space2 _position_space = droid.Runtime.Structs.Space.Space2.ZeroOne;

    [UnityEngine.HeaderAttribute("Specific", order = 102)]
    [UnityEngine.SerializeField]
    droid.Runtime.Enums.CoordinateSpaceEnum _coordinate_spaceEnum =
        droid.Runtime.Enums.CoordinateSpaceEnum.Environment_;

    [UnityEngine.SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override System.Collections.Generic.IEnumerable<float> FloatEnumerable {
      get {
        yield return this._2_d_position.x;
        yield return this._2_d_position.y;
      }
    }

    #if UNITY_EDITOR

    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._dim_combinationEnum) {
          case droid.Runtime.Enums.Dimension2DCombinationEnum.Xy_:
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.right * 2,
                                       color : UnityEngine.Color.green);
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.up * 2,
                                       color : UnityEngine.Color.red);
            break;
          case droid.Runtime.Enums.Dimension2DCombinationEnum.Xz_:
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.right * 2,
                                       color : UnityEngine.Color.green);
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.forward * 2,
                                       color : UnityEngine.Color.red);
            break;
          case droid.Runtime.Enums.Dimension2DCombinationEnum.Yz_:

            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.up * 2,
                                       color : UnityEngine.Color.green);
            UnityEngine.Debug.DrawLine(start : position,
                                       end : position + UnityEngine.Vector3.forward * 2,
                                       color : UnityEngine.Color.red);
            break;
          default: //TODO add the Direction cases
            UnityEngine.Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }
    #endif

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public UnityEngine.Vector2 ObservationValue {
      get { return this._2_d_position; }
      set { this._2_d_position = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public droid.Runtime.Structs.Space.Space2 DoubleSpace { get { return this._position_space; } }

    /// <summary>
    /// </summary>
    /// <param name="position"></param>
    /// <exception cref="System.ArgumentOutOfRangeException"></exception>
    public void SetPosition(UnityEngine.Vector3 position) {
      UnityEngine.Vector2 vector2_pos;
      switch (this._dim_combinationEnum) {
        case droid.Runtime.Enums.Dimension2DCombinationEnum.Xy_:
          vector2_pos = new UnityEngine.Vector2(x : position.x, y : position.y);
          break;
        case droid.Runtime.Enums.Dimension2DCombinationEnum.Xz_:
          vector2_pos = new UnityEngine.Vector2(x : position.x, y : position.z);
          break;
        case droid.Runtime.Enums.Dimension2DCombinationEnum.Yz_:
          vector2_pos = new UnityEngine.Vector2(x : position.y, y : position.z);
          break;
        default: throw new System.ArgumentOutOfRangeException();
      }

      this._2_d_position = this._position_space.Project(v : vector2_pos);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null
          && this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Environment_) {
        this.SetPosition(position : this.ParentEnvironment.TransformPoint(point : this.transform.position));
      } else if (this._coordinate_spaceEnum == droid.Runtime.Enums.CoordinateSpaceEnum.Local_) {
        this.SetPosition(position : this.transform.localPosition);
      } else {
        this.SetPosition(position : this.transform.position);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      base.RemotePostSetup();
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment && this.ParentEnvironment.PlayableArea != null) {
          var ex = this.ParentEnvironment.PlayableArea.Bounds.extents;
          switch (this._dim_combinationEnum) {
            case droid.Runtime.Enums.Dimension2DCombinationEnum.Xy_:
              this._position_space =
                  droid.Runtime.Structs.Space.Space2.FromCenterExtents(bounds_extents :
                                                                       new UnityEngine.Vector2(x : ex.x,
                                                                         y : ex.y));
              break;
            case droid.Runtime.Enums.Dimension2DCombinationEnum.Xz_:
              this._position_space =
                  droid.Runtime.Structs.Space.Space2.FromCenterExtents(bounds_extents :
                                                                       new UnityEngine.Vector2(x : ex.x,
                                                                         y : ex.z));
              break;
            case droid.Runtime.Enums.Dimension2DCombinationEnum.Yz_:
              this._position_space =
                  droid.Runtime.Structs.Space.Space2.FromCenterExtents(bounds_extents :
                                                                       new UnityEngine.Vector2(x : ex.y,
                                                                         y : ex.z));
              break;
            default: throw new System.ArgumentOutOfRangeException();
          }
        }
      }
    }
  }
}