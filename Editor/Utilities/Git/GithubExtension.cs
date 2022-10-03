#if UNITY_2019_1_OR_NEWER && UNITY_EDITOR && USE_GITHUB_EXTENSION
namespace droid.Editor.Utilities.Git {
  using Enumerable = System.Linq.Enumerable;

  /// <summary>
  /// </summary>
  public static class GithubExtension {
    const string _k_display_none = "display-none";

    /// <summary>
    /// </summary>
    /// <param name="element"></param>
    /// <param name="value"></param>
    public static void SetElementDisplay(UnityEngine.UIElements.VisualElement element, bool value) {
      if (element == null) {
        return;
      }

      SetElementClass(element : element, class_name : _k_display_none, value : !value);
      element.visible = value;
    }

    /// <summary>
    /// </summary>
    /// <param name="element"></param>
    /// <returns></returns>
    public static bool IsElementDisplay(UnityEngine.UIElements.VisualElement element) {
      return !HasElementClass(element : element, class_name : _k_display_none);
    }

    /// <summary>
    /// </summary>
    /// <param name="element"></param>
    /// <param name="class_name"></param>
    /// <param name="value"></param>
    public static void SetElementClass(UnityEngine.UIElements.VisualElement element,
                                       string class_name,
                                       bool value) {
      if (element == null) {
        return;
      }

      if (value) {
        element.AddToClassList(className : class_name);
      } else {
        element.RemoveFromClassList(className : class_name);
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="element"></param>
    /// <param name="class_name"></param>
    /// <returns></returns>
    public static bool HasElementClass(UnityEngine.UIElements.VisualElement element, string class_name) {
      if (element == null) {
        return false;
      }

      return element.ClassListContains(cls : class_name);
    }

    /// <summary>
    /// </summary>
    /// <param name="package_info"></param>
    /// <returns></returns>
    public static string GetRepoUrl(UnityEditor.PackageManager.PackageInfo package_info) {
      return GetRepoUrl(package_id : package_info != null ? package_info.packageId : "");
    }

    /// <summary>
    /// </summary>
    /// <param name="package_id"></param>
    /// <returns></returns>
    public static string GetRepoUrl(string package_id) {
      var m = System.Text.RegularExpressions.Regex.Match(input : package_id, "^[^@]+@([^#]+)(#.+)?$");
      if (m.Success) {
        var repo_url = m.Groups[1].Value;
        repo_url = System.Text.RegularExpressions.Regex.Replace(input : repo_url,
                                                                "(git:)?git@([^:]+):",
                                                                "https://$2/");
        //repoUrl = repoUrl.Replace ("github.com:", "https://github.com/");
        repo_url = repo_url.Replace("ssh://", "https://");
        repo_url = repo_url.Replace("git@", "");
        repo_url = System.Text.RegularExpressions.Regex.Replace(input : repo_url, "\\.git$", "");

        return repo_url;
      }

      return "";
    }

    /// <summary>
    /// </summary>
    /// <param name="package_info"></param>
    /// <returns></returns>
    public static string GetRepoId(UnityEditor.PackageManager.PackageInfo package_info) {
      return GetRepoId(package_id : package_info != null ? package_info.packageId : "");
    }

    /// <summary>
    /// </summary>
    /// <param name="package_id"></param>
    /// <returns></returns>
    public static string GetRepoId(string package_id) {
      var m = System.Text.RegularExpressions.Regex.Match(input : GetRepoUrl(package_id : package_id),
                                                         "/([^/]+/[^/]+)$");
      if (m.Success) {
        return m.Groups[1].Value;
      }

      return "";
    }

    /// <summary>
    /// </summary>
    /// <param name="package_id"></param>
    /// <param name="method_path"></param>
    /// <returns></returns>
    public static string GetApiRequestUrl(string package_id, string method_path) {
      var repo_id = GetRepoId(package_id : package_id);
      if (package_id.Contains("github.com")) {
        return $"https://api.github.com/repos/{repo_id}/{method_path}";
      }

      return "";
    }

    public static UnityEngine.AsyncOperation
        RequestTags(string package_id, System.Collections.Generic.List<string> result) {
      return Request(url : GetApiRequestUrl(package_id : package_id, "tags"),
                     x => FillRefNamesFromResponse(res : x, result : result));
    }

    public static UnityEngine.AsyncOperation
        RequestBranches(string package_id, System.Collections.Generic.List<string> result) {
      return Request(url : GetApiRequestUrl(package_id : package_id, "branches"),
                     x => FillRefNamesFromResponse(res : x, result : result));
    }

    public static void FillRefNamesFromResponse(string res, System.Collections.Generic.List<string> result) {
      result.Clear();
      result.AddRange(collection :
                      Enumerable.Select(source : Enumerable.Cast<System.Text.RegularExpressions.Match>(source : System.Text
                                                                                                                      .RegularExpressions.Regex
                                                                                                                      .Matches(input : res, "\\s*\"name\": \"(.+)\",")),
                                        x => x.Groups[1].Value));
    }

    public static string GetRevisionHash(UnityEditor.PackageManager.PackageInfo package_info) {
      return GetRevisionHash(resolved_path : package_info != null ? package_info.resolvedPath : "");
    }

    public static string GetRevisionHash(string resolved_path) {
      var m = System.Text.RegularExpressions.Regex.Match(input : resolved_path, "@([^@]+)$");
      if (m.Success) {
        return m.Groups[1].Value;
      }

      return "";
    }

    public static string GetFileUrl(UnityEditor.PackageManager.PackageInfo package_info, string file_path) {
      return package_info != null
                 ? GetFileUrl(package_id : package_info.packageId,
                              resolved_path : package_info.resolvedPath,
                              file_path : file_path)
                 : "";
    }

    public static string GetFileUrl(string package_id, string resolved_path, string file_path) {
      if (string.IsNullOrEmpty(value : package_id)
          || string.IsNullOrEmpty(value : resolved_path)
          || string.IsNullOrEmpty(value : file_path)) {
        return "";
      }

      var repo_url = GetRepoUrl(package_id : package_id);
      var hash = GetRevisionHash(resolved_path : resolved_path);
      var blob = "blob";

      return $"{repo_url}/{blob}/{hash}/{file_path}";
    }

    public static string GetSpecificPackageId(string package_id, string tag) {
      if (string.IsNullOrEmpty(value : package_id)) {
        return "";
      }

      var m = System.Text.RegularExpressions.Regex.Match(input : package_id, "^([^#]+)(#.+)?$");
      if (m.Success) {
        var id = m.Groups[1].Value;
        return string.IsNullOrEmpty(value : tag) ? id : $"{id}#{tag}";
      }

      return "";
    }

    public static string GetRequestCache(string url) {
      var path = GetRequestCachePath(url : url);
      return System.IO.File.Exists(path : path)
             && (System.DateTime.UtcNow - System.IO.File.GetLastWriteTimeUtc(path : path)).TotalSeconds < 300
                 ? System.IO.File.ReadAllText(path : path)
                 : null;
    }

    public static string GetRequestCachePath(string url) { return $"Temp/RequestCache_{url.GetHashCode()}"; }

    public static UnityEngine.AsyncOperation Request(string url, System.Action<string> on_success) {
      if (string.IsNullOrEmpty(value : url)) {
        return null;
      }

      var cache = GetRequestCache(url : url);
      if (!string.IsNullOrEmpty(value : cache)) {
        on_success(obj : cache);
        return null;
      }

      var www = UnityEngine.Networking.UnityWebRequest.Get(uri : url);
      var op = www.SendWebRequest();
      op.completed += _ => {
                        if (www.isHttpError || www.isHttpError || !string.IsNullOrEmpty(value : www.error)) {
                          UnityEngine.Debug.LogError(message : www.error);
                          www.Dispose();
                          return;
                        }

                        var path = GetRequestCachePath(url : url);
                        System.IO.File.WriteAllText(path : path, contents : www.downloadHandler.text);
                        on_success(obj : www.downloadHandler.text);
                        www.Dispose();
                      };
      return op;
    }
  }
}
#endif