namespace droid.Runtime.Enums {
  /// <summary>
  ///   Determines the discrete time steps of the simulation environment.
  /// </summary>
  public enum SimulationTypeEnum {
    /// <summary>
    ///   Waiting for frame instead means stable physics(Multiple fixed updates) and camera has updated their
    ///   rendertextures. Pauses the game after every reaction until next reaction is received.
    /// </summary>
    Frame_dependent_, // TODO: Sometimes some frame seems to be dropped with the frame dependent configuration at high frame rates. // BROKEN NOT SYNCED!!

    /// <summary>
    ///   Camera sensors should be manually rendered to ensure validity and freshness with camera.Render()
    /// </summary>
    Physics_dependent_,

    /// <summary>
    ///   Camera sensors should be manually rendered to ensure validity and freshness with camera.Render()
    /// </summary>
    Event_dependent_,
    
    //TODO: MAKE MANUAL PHYSICS CALL VARIANT!

    /// <summary>
    ///   Continue simulation
    /// </summary>
    Independent_
  }
}