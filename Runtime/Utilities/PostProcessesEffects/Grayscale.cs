#if UNITY_POST_PROCESSING_STACK_V2
namespace droid.Runtime.Utilities.PostProcessesEffects {
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  [UnityEngine.Rendering.PostProcessing.PostProcessAttribute(renderer : typeof(GrayscaleRenderer),
                                                             eventType : UnityEngine.Rendering.PostProcessing
                                                                 .PostProcessEvent.AfterStack,
                                                             "Neodroid/Grayscale")]
  public sealed class Grayscale : UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings {
    /// <summary>
    /// </summary>
    [UnityEngine.RangeAttribute(0f, 1f)]
    [UnityEngine.TooltipAttribute("Grayscale effect intensity.")]
    public UnityEngine.Rendering.PostProcessing.FloatParameter blend =
        new UnityEngine.Rendering.PostProcessing.FloatParameter {value = 0.5f};
  }

  /// <summary>
  /// </summary>
  public sealed class GrayscaleRenderer :
      UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<Grayscale> {
    static readonly int _blend = UnityEngine.Shader.PropertyToID("_Blend");
    static readonly UnityEngine.Shader _s = UnityEngine.Shader.Find("Neodroid/PostProcessing/Grayscale");

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public override void Render(UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context) {
      var sheet = context.propertySheets.Get(shader : _s);
      sheet.properties.SetFloat(nameID : _blend, value : this.settings.blend);
      UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(cmd : context.command,
        source : context.source,
        destination : context.destination,
        propertySheet : sheet,
        0);
    }
  }
}
#endif