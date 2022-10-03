#if UNITY_POST_PROCESSING_STACK_V2
namespace droid.Editor.Utilities.PostProcessing {
  [UnityEditor.Rendering.PostProcessing.PostProcessEditorAttribute(settingsType :
                                                                    typeof(droid.Runtime.Utilities.
                                                                        PostProcessesEffects.Grayscale))]
  public sealed class GrayscaleEditor : UnityEditor.Rendering.PostProcessing.PostProcessEffectEditor<
      droid.Runtime.Utilities.PostProcessesEffects.Grayscale> {
    UnityEditor.Rendering.PostProcessing.SerializedParameterOverride _m_blend;

    public override void OnEnable() { this._m_blend = this.FindParameterOverride(x => x.blend); }

    public override void OnInspectorGUI() { this.PropertyField(property : this._m_blend); }
  }
}
#endif