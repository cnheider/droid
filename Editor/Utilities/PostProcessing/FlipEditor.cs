#if UNITY_POST_PROCESSING_STACK_V2
namespace droid.Editor.Utilities.PostProcessing {
  [UnityEditor.Rendering.PostProcessing.PostProcessEditorAttribute(settingsType :
                                                                    typeof(droid.Runtime.Utilities.
                                                                        PostProcessesEffects.Flipper))]
  public sealed class FlipEditor : UnityEditor.Rendering.PostProcessing.PostProcessEffectEditor<
      droid.Runtime.Utilities.PostProcessesEffects.Flipper> {
    UnityEditor.Rendering.PostProcessing.SerializedParameterOverride _m_flip_x;
    UnityEditor.Rendering.PostProcessing.SerializedParameterOverride _m_flip_y;

    public override void OnEnable() {
      this._m_flip_x = this.FindParameterOverride(x => x.flip_x);
      this._m_flip_y = this.FindParameterOverride(x => x.flip_y);
    }

    public override void OnInspectorGUI() {
      this.PropertyField(property : this._m_flip_x);
      this.PropertyField(property : this._m_flip_y);
    }
  }
}
#endif