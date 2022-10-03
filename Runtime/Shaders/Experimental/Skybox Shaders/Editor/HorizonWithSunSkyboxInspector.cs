#if UNITY_EDITOR
namespace droid.Runtime.Shaders.Experimental.Skybox_Shaders.Editor {
  public class HorizonWithSunSkyboxInspector : UnityEditor.MaterialEditor {
    public override void OnInspectorGUI() {
      this.serializedObject.Update();

      if (this.isVisible) {
        UnityEditor.EditorGUI.BeginChangeCheck();

        UnityEngine.GUILayout.Label("Background Parameters");

        UnityEditor.EditorGUILayout.Space();

        this.ColorProperty(prop : GetMaterialProperty(mats : this.targets, "_SkyColor1"), "Top Color");
        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_SkyExponent1"),
                           "Exponential Factor");

        UnityEditor.EditorGUILayout.Space();

        this.ColorProperty(prop : GetMaterialProperty(mats : this.targets, "_SkyColor2"), "Horizon Color");

        UnityEditor.EditorGUILayout.Space();

        this.ColorProperty(prop : GetMaterialProperty(mats : this.targets, "_SkyColor3"), "Bottom Color");
        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_SkyExponent2"),
                           "Exponential Factor");

        UnityEditor.EditorGUILayout.Space();

        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_SkyIntensity"), "Intensity");

        UnityEditor.EditorGUILayout.Space();

        UnityEngine.GUILayout.Label("Sun Parameters");

        UnityEditor.EditorGUILayout.Space();

        this.ColorProperty(prop : GetMaterialProperty(mats : this.targets, "_SunColor"), "Color");
        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_SunIntensity"), "Intensity");

        UnityEditor.EditorGUILayout.Space();

        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_SunAlpha"), "Alpha");
        this.FloatProperty(prop : GetMaterialProperty(mats : this.targets, "_SunBeta"), "Beta");

        UnityEditor.EditorGUILayout.Space();

        var az = GetMaterialProperty(mats : this.targets, "_SunAzimuth");
        var al = GetMaterialProperty(mats : this.targets, "_SunAltitude");

        if (az.hasMixedValue || al.hasMixedValue) {
          UnityEditor.EditorGUILayout.HelpBox("Editing angles is disabled because they have mixed values.",
                                              type : UnityEditor.MessageType.Warning);
        } else {
          this.FloatProperty(prop : az, "Azimuth");
          this.FloatProperty(prop : al, "Altitude");
        }

        if (UnityEditor.EditorGUI.EndChangeCheck()) {
          var raz = az.floatValue * UnityEngine.Mathf.Deg2Rad;
          var ral = al.floatValue * UnityEngine.Mathf.Deg2Rad;

          var up_vector =
              new UnityEngine.Vector4(x : UnityEngine.Mathf.Cos(f : ral) * UnityEngine.Mathf.Sin(f : raz),
                                      y : UnityEngine.Mathf.Sin(f : ral),
                                      z : UnityEngine.Mathf.Cos(f : ral) * UnityEngine.Mathf.Cos(f : raz),
                                      0.0f);
          GetMaterialProperty(mats : this.targets, "_SunVector").vectorValue = up_vector;

          this.PropertiesChanged();
        }
      }
    }
  }
}
#endif