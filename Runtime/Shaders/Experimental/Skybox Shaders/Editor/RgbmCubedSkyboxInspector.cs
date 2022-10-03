#if UNITY_EDITOR
namespace droid.Runtime.Shaders.Experimental.Skybox_Shaders.Editor {
  public class RgbmCubedSkyboxInspector : UnityEditor.MaterialEditor {
    public override void OnInspectorGUI() {
      base.OnInspectorGUI();

      if (this.isVisible) {
        var material = this.target as UnityEngine.Material;

        var use_linear = false;
        for (var index = 0; index < material.shaderKeywords.Length; index++) {
          var keyword = material.shaderKeywords[index];
          if (keyword == "USE_LINEAR") {
            use_linear = true;
            break;
          }
        }

        UnityEditor.EditorGUI.BeginChangeCheck();

        use_linear = UnityEditor.EditorGUILayout.Toggle("Linear Space Lighting", value : use_linear);

        if (UnityEditor.EditorGUI.EndChangeCheck()) {
          if (use_linear) {
            material.EnableKeyword("USE_LINEAR");
            material.DisableKeyword("USE_GAMMA");
          } else {
            material.DisableKeyword("USE_LINEAR");
            material.EnableKeyword("USE_GAMMA");
          }

          UnityEditor.EditorUtility.SetDirty(target : this.target);
        }
      }
    }
  }
}
#endif