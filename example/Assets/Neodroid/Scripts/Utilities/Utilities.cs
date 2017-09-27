using UnityEngine;

namespace Neodroid.Utilities {
  public static class NeodroidFunctions {
    
    public static Texture2D RenderTextureImage(Camera camera) { // From unity documentation, https://docs.unity3d.com/ScriptReference/Camera.Render.html
      RenderTexture current_render_texture = RenderTexture.active;
      RenderTexture.active = camera.targetTexture;
      camera.Render();
      Texture2D texture = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
      texture.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
      texture.Apply();
      RenderTexture.active = current_render_texture;
      return texture;
    }

    public static string ColorArrayToString(Color[] colors) {
      string s = "";
      foreach(Color color in colors) {
        s += color.ToString();
      }
      return s;
    }

    public static void MaybeRegisterComponent<Recipient, Caller>(Recipient r, Caller c) where Recipient : Object, HasRegister<Caller> where Caller : Component {
      Recipient component;
      if (r != null) {
        component = r;  //.GetComponent<Recipient>();
      } else if (c.GetComponentInParent<Recipient>() != null) {
        component = c.GetComponentInParent<Recipient>();
      } else {
        component = Object.FindObjectOfType<Recipient>();
      }
      if (component != null)
        component.Register(c);
    }


    /** Contains logic for coverting a camera component into a Texture2D. */
    /*public Texture2D ObservationToTex(Camera camera, int width, int height)
    {
      Camera cam = camera;
      Rect oldRec = camera.rect;
      cam.rect = new Rect(0f, 0f, 1f, 1f);
      bool supportsAntialiasing = false;
      bool needsRescale = false;
      var depth = 24;
      var format = RenderTextureFormat.Default;
      var readWrite = RenderTextureReadWrite.Default;
      var antiAliasing = (supportsAntialiasing) ? Mathf.Max(1, QualitySettings.antiAliasing) : 1;

      var finalRT =
        RenderTexture.GetTemporary(width, height, depth, format, readWrite, antiAliasing);
      var renderRT = (!needsRescale) ? finalRT :
        RenderTexture.GetTemporary(width, height, depth, format, readWrite, antiAliasing);
      var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

      var prevActiveRT = RenderTexture.active;
      var prevCameraRT = cam.targetTexture;

      // render to offscreen texture (readonly from CPU side)
      RenderTexture.active = renderRT;
      cam.targetTexture = renderRT;

      cam.Render();

      if (needsRescale)
      {
        RenderTexture.active = finalRT;
        Graphics.Blit(renderRT, finalRT);
        RenderTexture.ReleaseTemporary(renderRT);
      }

      tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
      tex.Apply();
      cam.targetTexture = prevCameraRT;
      cam.rect = oldRec;
      RenderTexture.active = prevActiveRT;
      RenderTexture.ReleaseTemporary(finalRT);
      return tex;
    }

    /// Contains logic to convert the agent's cameras into observation list
    ///  (as list of float arrays)
    public List<float[,,,]> GetObservationMatrixList(List<int> agent_keys)
    {
      List<float[,,,]> observation_matrix_list = new List<float[,,,]>();
      Dictionary<int, List<Camera>> observations = CollectObservations();
      for (int obs_number = 0; obs_number < brainParameters.cameraResolutions.Length; obs_number++)
      {
        int width = brainParameters.cameraResolutions[obs_number].width;
        int height = brainParameters.cameraResolutions[obs_number].height;
        bool bw = brainParameters.cameraResolutions[obs_number].blackAndWhite;
        int pixels = 0;
        if (bw)
          pixels = 1;
        else
          pixels = 3;
        float[,,,] observation_matrix = new float[agent_keys.Count
          , height
          , width
          , pixels];
        int i = 0;
        foreach (int k in agent_keys)
        {
          Camera agent_obs = observations[k][obs_number];
          Texture2D tex = ObservationToTex(agent_obs, width, height);
          for (int w = 0; w < width; w++)
          {
            for (int h = 0; h < height; h++)
            {
              Color c = tex.GetPixel(w, h);
              if (!bw)
              {
                observation_matrix[i, tex.height - h - 1, w, 0] = c.r;
                observation_matrix[i, tex.height - h - 1, w, 1] = c.g;
                observation_matrix[i, tex.height - h - 1, w, 2] = c.b;
              }
              else
              {
                observation_matrix[i, tex.height - h - 1, w, 0] = (c.r + c.g + c.b) / 3;
              }
            }
          }
          UnityEngine.Object.DestroyImmediate(tex);
          Resources.UnloadUnusedAssets();
          i++;
        }
        observation_matrix_list.Add(observation_matrix);
      }
      return observation_matrix_list;
    }*/
  }

}
