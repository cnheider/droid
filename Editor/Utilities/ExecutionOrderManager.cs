#if UNITY_EDITOR
namespace droid.Editor.Utilities {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEditor.InitializeOnLoadAttribute]
  public class ExecutionOrderManager : UnityEditor.Editor {
    static ExecutionOrderManager() {
      foreach (var mono_script in UnityEditor.MonoImporter.GetAllRuntimeMonoScripts()) {
        var type = mono_script.GetClass();
        if (type == null) {
          continue;
        }

        var attributes =
            type.GetCustomAttributes(attributeType : typeof(ScriptExecutionOrderAttribute), true);

        if (attributes.Length == 0) {
          continue;
        }

        var attribute = (ScriptExecutionOrderAttribute)attributes[0];
        if (UnityEditor.MonoImporter.GetExecutionOrder(script : mono_script) != attribute.GetOrder()) {
          UnityEditor.MonoImporter.SetExecutionOrder(script : mono_script, order : attribute.GetOrder());
        }
      }
    }
  }
}
#endif