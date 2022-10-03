#if UNITY_POST_PROCESSING_STACK_V2
namespace droid.Runtime.Utilities.PostProcessesEffects {
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  [UnityEngine.Rendering.PostProcessing.PostProcessAttribute(renderer : typeof(FlipperRenderer),
                                                             eventType : UnityEngine.Rendering.PostProcessing
                                                                 .PostProcessEvent.AfterStack,
                                                             "Neodroid/Flip")]
  public sealed class Flipper : UnityEngine.Rendering.PostProcessing.PostProcessEffectSettings {
    /// <summary>
    /// </summary>
    public UnityEngine.Rendering.PostProcessing.BoolParameter flip_x =
        new UnityEngine.Rendering.PostProcessing.BoolParameter {value = false};

    /// <summary>
    /// </summary>
    public UnityEngine.Rendering.PostProcessing.BoolParameter flip_y =
        new UnityEngine.Rendering.PostProcessing.BoolParameter {value = false};
  }

  public sealed class FlipperRenderer :
      UnityEngine.Rendering.PostProcessing.PostProcessEffectRenderer<Flipper> {
    static readonly int _flip_x = UnityEngine.Shader.PropertyToID("_Flip_x");
    static readonly int _flip_y = UnityEngine.Shader.PropertyToID("_Flip_y");
    static readonly UnityEngine.Shader _s = UnityEngine.Shader.Find("Neodroid/PostProcessing/Flip");

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public override void Render(UnityEngine.Rendering.PostProcessing.PostProcessRenderContext context) {
      var sheet = context.propertySheets.Get(shader : _s);
      sheet.properties.SetFloat(nameID : _flip_x, value : this.settings.flip_x ? -1.0f : 1.0f);
      sheet.properties.SetFloat(nameID : _flip_y, value : this.settings.flip_y ? -1.0f : 1.0f);
      UnityEngine.Rendering.PostProcessing.RuntimeUtilities.BlitFullscreenTriangle(cmd : context.command,
        source : context.source,
        destination : context.destination,
        propertySheet : sheet,
        0);
    }
  }
}
#endif