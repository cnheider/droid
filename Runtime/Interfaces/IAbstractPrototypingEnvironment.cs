namespace droid.Runtime.Interfaces {
  /// <inheritdoc cref="IEnvironment" />
  /// <summary>
  /// </summary>
  public interface IAbstractPrototypingEnvironment : IEnvironment,
                                                     IHasRegister<ISensor>,
                                                     IHasRegister<IConfigurable>,
                                                     IHasRegister<IUnobservable>,
                                                     IHasRegister<IDisplayer> {
    /// <summary>
    /// </summary>
    UnityEngine.Transform Transform { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox PlayableArea { get; }

    /// <summary>
    /// </summary>
    droid.Runtime.Prototyping.ObjectiveFunctions.EpisodicObjective ObjectiveFunction { get; }

    /// <summary>
    /// </summary>
    /// <param name="transform_forward"></param>
    /// <returns></returns>
    UnityEngine.Vector3 TransformDirection(UnityEngine.Vector3 transform_forward);

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    UnityEngine.Vector3 TransformPoint(UnityEngine.Vector3 point);

    /// <summary>
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    UnityEngine.Vector3 InverseTransformPoint(UnityEngine.Vector3 point);

    /// <summary>
    /// </summary>
    /// <param name="inv_dir"></param>
    /// <returns></returns>
    UnityEngine.Vector3 InverseTransformDirection(UnityEngine.Vector3 inv_dir);

    /// <summary>
    /// </summary>
    /// <param name="transform_rotation"></param>
    /// <returns></returns>
    UnityEngine.Quaternion TransformRotation(UnityEngine.Quaternion transform_rotation);

    /// <summary>
    /// </summary>
    /// <param name="transform_rotation"></param>
    /// <returns></returns>
    UnityEngine.Quaternion InverseTransformRotation(UnityEngine.Quaternion transform_rotation);

    /// <summary>
    /// </summary>
    event System.Action PreStepEvent;

    /// <summary>
    /// </summary>
    event System.Action StepEvent;

    /// <summary>
    /// </summary>
    event System.Action PostStepEvent;

    /// <summary>
    /// </summary>
    /// <param name="reason"></param>
    void Terminate(string reason);
  }
}