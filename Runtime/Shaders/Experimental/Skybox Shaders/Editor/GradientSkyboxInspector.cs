#if UNITY_EDITOR
namespace droid.Runtime.Shaders.Experimental.Skybox_Shaders.Editor {
  public class GradientSkyboxInspector : UnityEditor.MaterialEditor {
    public override void OnInspectorGUI() {
      this.serializedObject.Update();

      if (this.isVisible) {
        UnityEditor.EditorGUI.BeginChangeCheck();

        this.ColorProperty(prop : GetMaterialProperty(mats : this.targets, "_Color2"), "Top Color");
        this.ColorProperty(prop : GetMaterialProperty(mats : this.targets, "_Color1"), "Bottom Color");
        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_Intensity"), "Intensity");
        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_Exponent"), "Exponent");

        var dp = GetMaterialProperty(mats : this.targets, "_UpVectorPitch");
        var dy = GetMaterialProperty(mats : this.targets, "_UpVectorYaw");

        if (dp.hasMixedValue || dy.hasMixedValue) {
          UnityEditor.EditorGUILayout.HelpBox("Editing angles is disabled because they have mixed values.",
                                              type : UnityEditor.MessageType.Warning);
        } else {
          this.FloatProperty(prop : dp, "Pitch");
          this.FloatProperty(prop : dy, "Yaw");
        }

        if (UnityEditor.EditorGUI.EndChangeCheck()) {
          var rp = dp.floatValue * UnityEngine.Mathf.Deg2Rad;
          var ry = dy.floatValue * UnityEngine.Mathf.Deg2Rad;

          var up_vector =
              new UnityEngine.Vector4(x : UnityEngine.Mathf.Sin(f : rp) * UnityEngine.Mathf.Sin(f : ry),
                                      y : UnityEngine.Mathf.Cos(f : rp),
                                      z : UnityEngine.Mathf.Sin(f : rp) * UnityEngine.Mathf.Cos(f : ry),
                                      0.0f);
          GetMaterialProperty(mats : this.targets, "_UpVector").vectorValue = up_vector;

          this.PropertiesChanged();
        }
      }
    }
  }
}
#endif