namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="GameObjects.PrototypingGameObject" />
  /// <summary>
  /// </summary>
  [UnityEngine.AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                                           + "Spatial"
                                           + ConfigurableComponentMenuPath._Postfix)]
  public abstract class SpatialConfigurable : Configurable {
    /// <summary>
    /// </summary>
    public bool RelativeToExistingValue { get { return this._relative_to_existing_value; } }

    #region Fields

    /// <summary>
    /// </summary>
    [UnityEngine.HeaderAttribute("Configurable", order = 30)]
    [UnityEngine.SerializeField]
    bool _relative_to_existing_value = false;

    [UnityEngine.SerializeField]
    protected droid.Runtime.Enums.CoordinateSpaceEnum _coordinate_spaceEnum =
        droid.Runtime.Enums.CoordinateSpaceEnum.Environment_;

    #endregion
  }
}