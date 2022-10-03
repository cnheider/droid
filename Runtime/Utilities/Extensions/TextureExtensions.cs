namespace droid.Runtime.Utilities.Extensions {
  /// <summary>
  /// </summary>
  public static class TextureExtentions {
    /// <summary>
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static UnityEngine.Texture2D ToTexture2D(this UnityEngine.WebCamTexture texture) {
      var color_array = new Color32Array {colors = new UnityEngine.Color32[texture.width * texture.height]};
      texture.GetPixels32(colors : color_array.colors);
      var tex = new UnityEngine.Texture2D(2, 2);
      tex.LoadRawTextureData(data : color_array.byteArray);
      tex.Apply();

      return tex;

      /*
      var ntv_p = texture.GetNativeTexturePtr();

      return Texture2D.CreateExternalTexture(texture.width,
                                             texture.height,
                                             TextureFormat.RGBAFloat,
                                             false,
                                             true,
                                             ntv_p);
    */
    }

    #region Nested type: Color32Array

    /// <summary>
    /// </summary>
    [System.Runtime.InteropServices.StructLayoutAttribute(layoutKind : System.Runtime.InteropServices
                                                                             .LayoutKind.Explicit)]
    public struct Color32Array {
      /// <summary>
      /// </summary>
      [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
      public byte[] byteArray;

      /// <summary>
      /// </summary>
      [System.Runtime.InteropServices.FieldOffsetAttribute(0)]
      public UnityEngine.Color32[] colors;
    }

    #endregion
  }
}