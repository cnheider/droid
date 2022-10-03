#if UNITY_EDITOR

namespace droid.Editor.GameObjects {
  public class PrebuiltSpawner : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    /// <param name="menu_command"></param>
    [UnityEditor.MenuItem(itemName : EditorGameObjectMenuPath._GameObjectMenuPath
                                     + "Prebuilt/SimpleEnvironment",
                          false,
                          10)]
    static void CreateSingleEnvironmentGameObject(UnityEditor.MenuCommand menu_command) {
      var go = new UnityEngine.GameObject("SimpleEnvironment");
      go.AddComponent<droid.Runtime.Managers.NeodroidManager>();
      var env = go.AddComponent<droid.Runtime.Environments.Prototyping.PrototypingEnvironment>();
      go.AddComponent<UnityEngine.BoxCollider>();
      var bounding_box = go.AddComponent<droid.Runtime.GameObjects.BoundingBoxes.NeodroidBoundingBox>();
      env.PlayableArea = bounding_box;

      var plane = UnityEngine.GameObject.CreatePrimitive(type : UnityEngine.PrimitiveType.Plane);
      plane.transform.parent = go.transform;

      var actor = new UnityEngine.GameObject("Actor");
      actor.AddComponent<droid.Runtime.Prototyping.Actors.Actor>();
      actor.AddComponent<droid.Runtime.Prototyping.Actuators.EulerTransform3DofActuator>();
      actor.AddComponent<droid.Runtime.Prototyping.Sensors.Spatial.Transform.EulerTransformSensor>();
      actor.AddComponent<droid.Runtime.Prototyping.Configurables.Transforms.PositionConfigurable>();
      actor.transform.parent = go.transform;

      var capsule = UnityEngine.GameObject.CreatePrimitive(type : UnityEngine.PrimitiveType.Capsule);
      capsule.transform.parent = actor.transform;
      capsule.transform.localPosition = UnityEngine.Vector3.up;

      UnityEditor.GameObjectUtility.SetParentAndAlign(child : go,
                                                      parent : menu_command
                                                                       .context as
                                                                   UnityEngine.
                                                                   GameObject); // Ensure it gets reparented if this was a context click (otherwise does nothing)
      UnityEditor.Undo.RegisterCreatedObjectUndo(objectToUndo : go,
                                                 name : $"Create {go.name}"); // Register the creation in the undo system
      UnityEditor.Selection.activeObject = go;
    }
  }
}
#endif