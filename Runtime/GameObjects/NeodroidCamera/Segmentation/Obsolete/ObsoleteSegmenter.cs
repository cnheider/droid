namespace droid.Runtime.GameObjects.NeodroidCamera.Segmentation.Obsolete {
  /// <inheritdoc cref="UnityEngine.MonoBehaviour" />
  /// <summary>
  /// </summary>
  public abstract class ObsoleteSegmenter : Segmenter {
    [UnityEngine.SerializeField]
    [UnityEngine.RangeAttribute(0, 2)]
    protected float _Outline_Width_Factor = 0.05f;

    [UnityEngine.SerializeField] protected UnityEngine.Color _Outline_Color = UnityEngine.Color.magenta;

    /// <summary>
    /// </summary>
    protected int _Default_Color_Tag = UnityEngine.Shader.PropertyToID("_Color");

    protected int _Outline_Color_Tag = UnityEngine.Shader.PropertyToID("_OutlineColor");
    protected int _Outline_Width_Factor_Tag = UnityEngine.Shader.PropertyToID("_OutlineWidthFactor");

    protected int _Segmentation_Color_Tag = UnityEngine.Shader.PropertyToID("_SegmentationColor");

    public UnityEngine.Color OutlineColor { get { return this._Outline_Color; } }

    /*void OnPreCull() {
  // change
}*/
    /// <summary>
    /// </summary>
    void OnPostRender() {
      // change back
      this.Restore();
    }

    void OnPreRender() {
      // change
      this.Change();
    }

    protected abstract void Change();

    protected abstract void Restore();
  }
}