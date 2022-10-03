#if UNITY_EDITOR

#endif

namespace droid.Runtime.GameObjects.BoundingBoxes {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class NeodroidBoundingBox : droid.Runtime.Prototyping.EnvironmentListener.EnvironmentListener {
    /// <summary>
    /// </summary>
    public void FixedUpdate() {
      if (this.freezeAfterFirstCalculation) {
        return;
      }

      if (this.includeChildren && !this.cacheChildren) {
        if (this._children_meshes != this.GetComponentsInChildren<UnityEngine.MeshFilter>()) {
          this.BoundingBoxReset();
        }

        if (this._children_colliders != this.GetComponentsInChildren<UnityEngine.Collider>()) {
          this.BoundingBoxReset();
        }
      } else {
        this.CalculateBoundingBox();
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="a_camera"></param>
    /// <param name="margin"></param>
    /// <returns></returns>
    public UnityEngine.Rect ScreenSpaceBoundingRect(UnityEngine.Camera a_camera, float margin = 0f) {
      if (this._basedOnEnum == droid.Runtime.Enums.BoundingBox.BasedOnEnum.Collider_) {
        var a = this._local_collider as UnityEngine.MeshCollider;
        if (a) {
          return droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                      .GetCameraMinMaxRect(mesh : a.sharedMesh,
                                           t : this.transform,
                                           cam : a_camera,
                                           margin : this.bb_margin - margin);
        }
      }

      if (this._local_mesh) {
        if (this._use_shared_mesh || !UnityEngine.Application.isPlaying) {
          var a = droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                       .GetCameraMinMaxPoints(mesh : this._local_mesh.sharedMesh,
                                              t : this.transform,
                                              cam : a_camera);
          if (this.includeChildren) {
            for (var index = 0; index < this._children_meshes.Length; index++) {
              var children_mesh = this._children_meshes[index];
              a = droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                       .GetCameraMinMaxPoints(mesh : children_mesh.sharedMesh,
                                              t : children_mesh.transform,
                                              cam : a_camera,
                                              min : a[0],
                                              max : a[1]);
            }

            return droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                        .GetMinMaxRect(min : a[0], max : a[1], margin : this.bb_margin - margin);
          }
        } else {
          var a = droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                       .GetCameraMinMaxPoints(mesh : this._local_mesh.mesh,
                                              t : this.transform,
                                              cam : a_camera);
          if (this.includeChildren) {
            for (var index = 0; index < this._children_meshes.Length; index++) {
              var children_mesh = this._children_meshes[index];
              a = droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                       .GetCameraMinMaxPoints(mesh : children_mesh.mesh,
                                              t : children_mesh.transform,
                                              cam : a_camera,
                                              min : a[0],
                                              max : a[1]);
            }

            return droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                        .GetMinMaxRect(min : a[0], max : a[1], margin : this.bb_margin - margin);
          }
        }
      } else {
        if (this._use_shared_mesh || !UnityEngine.Application.isPlaying) {
          if (this._children_meshes != null && this._children_meshes.Length > 0) {
            var a =
                droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                     .GetCameraMinMaxPoints(mesh : this._children_meshes[0].sharedMesh,
                                            t : this._children_meshes[0].transform,
                                            cam : a_camera);
            if (this.includeChildren) {
              for (var index = 1; index < this._children_meshes.Length; index++) {
                var children_mesh = this._children_meshes[index];
                a = droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                         .GetCameraMinMaxPoints(mesh : children_mesh.sharedMesh,
                                                t : children_mesh.transform,
                                                cam : a_camera,
                                                min : a[0],
                                                max : a[1]);
              }

              return droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                          .GetMinMaxRect(min : a[0], max : a[1], margin : this.bb_margin - margin);
            }
          }
        } else {
          if (this._children_meshes != null && this._children_meshes.Length > 0) {
            var a = droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                         .GetCameraMinMaxPoints(mesh : this._children_meshes[0].mesh,
                                                t : this._children_meshes[0].transform,
                                                cam : a_camera);
            if (this.includeChildren) {
              for (var index = 1; index < this._children_meshes.Length; index++) {
                var children_mesh = this._children_meshes[index];
                a = droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                         .GetCameraMinMaxPoints(mesh : children_mesh.mesh,
                                                t : children_mesh.transform,
                                                cam : a_camera,
                                                min : a[0],
                                                max : a[1]);
              }

              return droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                          .GetMinMaxRect(min : a[0], max : a[1], margin : this.bb_margin - margin);
            }
          }
        }
      }

      return new UnityEngine.Rect();
    }

    /// <summary>
    /// </summary>
    /// <param name="vec"></param>
    /// <returns></returns>
    static string JsonifyVec3(UnityEngine.Vector3 vec) { return $"[{vec.x},{vec.y},{vec.z}]"; }

    /// <summary>
    /// </summary>
    void BoundingBoxReset() {
      this.Awake();
      this.Start();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      base.Setup();
      base.PreSetup();
      if (!this.enabled) {
        return;
      }

      if (!this.RunInEditModeSetup && !UnityEngine.Application.isPlaying) {
        return;
      }

      if (this.environment == null) {
        this.environment =
            FindObjectOfType<droid.Runtime.Environments.Prototyping.AbstractPrototypingEnvironment>();
      }

      if (!this._bb_transform) {
        this._empty_go = new UnityEngine.GameObject {hideFlags = UnityEngine.HideFlags.HideAndDontSave};
        this._bb_transform = this._empty_go.transform;
      }

      if (this.includeSelf) {
        this._local_collider = this.GetComponent<UnityEngine.BoxCollider>();
        this._local_mesh = this.GetComponent<UnityEngine.MeshFilter>();
      }

      if (this.includeChildren) {
        this._children_meshes = this.GetComponentsInChildren<UnityEngine.MeshFilter>();
        this._children_colliders = this.GetComponentsInChildren<UnityEngine.Collider>();
      }

      this.CalculateBoundingBox();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Tick() {
      base.Tick();
      if (this.freezeAfterFirstCalculation) {
        return;
      }

      if (this.includeChildren && !this.cacheChildren) {
        if (this._children_meshes != this.GetComponentsInChildren<UnityEngine.MeshFilter>()) {
          this.BoundingBoxReset();
        }

        if (this._children_colliders != this.GetComponentsInChildren<UnityEngine.Collider>()) {
          this.BoundingBoxReset();
        }
      } else {
        this.CalculateBoundingBox();
      }
    }

    /// <summary>
    /// </summary>
    void FitCollidersAabb() {
      var transform1 = this.transform;
      this._bb_transform.rotation = transform1.rotation;
      this._bb_transform.position = transform1.position;

      var bounds =
          new UnityEngine.Bounds(center : this._bb_transform.position, size : UnityEngine.Vector3.zero);

      if (this.includeSelf && this._local_collider) {
        this._bb_transform.position = this._local_collider.bounds.center;
        bounds = this._local_collider.bounds;
      }

      if (this.includeChildren && this._children_colliders != null) {
        for (var index = 0; index < this._children_colliders.Length; index++) {
          var a_collider = this._children_colliders[index];
          if (a_collider && a_collider != this._local_collider) {
            if (this._only_active_children) {
              if (a_collider.gameObject.activeInHierarchy
                  && a_collider.gameObject.activeSelf
                  && a_collider.enabled) {
                if (bounds.size == UnityEngine.Vector3.zero) {
                  this._bb_transform.rotation = a_collider.transform.rotation;
                  this._bb_transform.position = a_collider.bounds.center;
                  bounds = a_collider.bounds;
                } else {
                  bounds.Encapsulate(bounds : a_collider.bounds);
                }
              }
            } else {
              if (bounds.size == UnityEngine.Vector3.zero) {
                this._bb_transform.rotation = a_collider.transform.rotation;
                this._bb_transform.position = a_collider.bounds.center;
                bounds = a_collider.bounds;
              } else {
                bounds.Encapsulate(bounds : a_collider.bounds);
              }
            }
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - this._bb_transform.position;
    }

    /// <summary>
    /// </summary>
    void FitRenderersAabb() {
      var transform1 = this.transform;
      var position = transform1.position;
      this._bb_transform.position = position;
      this._bb_transform.rotation = transform1.rotation;

      var bounds = new UnityEngine.Bounds(center : position, size : UnityEngine.Vector3.zero);

      if (this.includeSelf && this._local_mesh) {
        UnityEngine.Mesh a_mesh;

        if (this._use_shared_mesh) {
          a_mesh = this._local_mesh.sharedMesh;
        } else {
          a_mesh = this._local_mesh.mesh;
        }

        if (a_mesh.isReadable) {
          var vc = a_mesh.vertexCount;
          for (var i = 0; i < vc; i++) {
            bounds.Encapsulate(point : this._local_mesh.transform
                                           .TransformPoint(position : a_mesh.vertices[i]));
          }
        } else {
          UnityEngine.Debug.LogWarning("Make sure mesh is marked as readable when imported!");
        }
      }

      if (this.includeChildren && this._children_meshes != null) {
        for (var index = 0; index < this._children_meshes.Length; index++) {
          var t = this._children_meshes[index];
          if (t) {
            if (this._only_active_children) {
              if (t.gameObject.activeInHierarchy && t.gameObject.activeSelf) {
                if (bounds.size == UnityEngine.Vector3.zero) {
                  var transform2 = t.transform;
                  position = transform2.position;
                  this._bb_transform.position = position;
                  this._bb_transform.rotation = transform2.rotation;
                  bounds = new UnityEngine.Bounds(center : position, size : UnityEngine.Vector3.zero);
                }

                UnityEngine.Mesh a_mesh;

                if (this._use_shared_mesh) {
                  a_mesh = t.sharedMesh;
                } else {
                  a_mesh = t.mesh;
                }

                if (a_mesh) {
                  if (a_mesh.isReadable) {
                    var vc = a_mesh.vertexCount;
                    for (var j = 0; j < vc; j++) {
                      bounds.Encapsulate(point : t.transform.TransformPoint(position : a_mesh.vertices[j]));
                    }
                  } else {
                    UnityEngine.Debug.LogWarning("Make sure mesh is marked as readable when imported!");
                  }
                }
              }
            } else {
              if (bounds.size == UnityEngine.Vector3.zero) {
                bounds = new UnityEngine.Bounds(center : t.transform.position,
                                                size : UnityEngine.Vector3.zero);
              }

              UnityEngine.Mesh a_mesh;

              if (this._use_shared_mesh) {
                a_mesh = t.sharedMesh;
              } else {
                a_mesh = t.mesh;
              }

              if (a_mesh) {
                var vc = a_mesh.vertexCount;
                for (var j = 0; j < vc; j++) {
                  bounds.Encapsulate(point : t.transform.TransformPoint(position : a_mesh.vertices[j]));
                }
              }
            }
          }
        }
      }

      this._Bounds = bounds;
      this._Bounds_Offset = this._Bounds.center - position;
    }

    /// <summary>
    /// </summary>
    void CalculateBoundingBox() {
      if (!this.RunInEditModeSetup && !UnityEngine.Application.isPlaying || this._bb_transform == null) {
        return;
      }

      if (this._basedOnEnum == droid.Runtime.Enums.BoundingBox.BasedOnEnum.Collider_) {
        switch (this.BbAligning) {
          case droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum.Axis_aligned_:
            this.FitCollidersAabb();
            this.RecalculatePoints();
            this.RecalculateLines();
            break;
          case droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum.Object_oriented_:
            this.FitCollidersOobb();
            break;
          case droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum.Camera_oriented_:
            this.FitRenderersCabb();
            break;
          default: throw new System.ArgumentOutOfRangeException();
        }
      } else {
        switch (this.BbAligning) {
          case droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum.Axis_aligned_:
            this.FitRenderersAabb();
            this.RecalculatePoints();
            this.RecalculateLines();
            break;
          case droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum.Object_oriented_:
            this.FitRenderersOobb();
            break;
          case droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum.Camera_oriented_:
            this.FitRenderersCabb();

            break;
          default: throw new System.ArgumentOutOfRangeException();
        }
      }
    }

    void FitRenderersCabb() {
      throw new System.NotImplementedException();

      /*
      var transform1 = this.transform;
      var position = transform1.position;
      this._bb_transform.position = position;
      this._bb_transform.rotation = transform1.rotation;

      var a = this._local_mesh.sharedMesh.GetCameraMinMaxPoints(this._bb_transform,
                                                                this._camera,
                                                                this.use_view_port);
      var min = a[0];
      var max = a[1];
      var extent = a[2];

      if (this.use_view_port) {
        min = this._camera.ViewportToWorldPoint(min);
        max = this._camera.ViewportToWorldPoint(max);
      } else {
        min = this._camera.ScreenToWorldPoint(min);
        max = this._camera.ScreenToWorldPoint(max);
        extent = max - min;
      }

      var cobb_extent = extent;
      var cobb_center =
          new Vector3(min.x + extent.x / 2.0f, min.y + extent.y / 2.0f, min.z + extent.z / 2.0f);

      this._point_tfr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Front_Right);
      this._point_tfl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Front_Left);
      this._point_tbl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Back_Left);
      this._point_tbr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Top_Back_Right);
      this._point_bfr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Front_Right);
      this._point_bfl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Front_Left);
      this._point_bbl = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Back_Left);
      this._point_bbr = cobb_center + Vector3.Scale(cobb_extent, BoundingBoxUtilities._Bottom_Back_Right);

      this._Bounds.center = cobb_center;
      this._Bounds.extents = cobb_extent;

      this._points = new[] {
                               this._point_tfr,
                               this._point_tfl,
                               this._point_tbl,
                               this._point_tbr,
                               this._point_bfr,
                               this._point_bfl,
                               this._point_bbl,
                               this._point_bbr
                           };

      var rot = Quaternion.identity;
      var pos = Vector3.zero;

      //rot = transform1.rotation;
      //pos = transform1.position;

      this._lines_list.Clear();

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 1] + pos};
        this._lines_list.Add(line);

        //height
        line = new[] {rot * this.Points[i] + pos, rot * this.Points[i + 4] + pos};
        this._lines_list.Add(line);

        //depth
        line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 3 - 4 * (i % 2)] + pos};
        this._lines_list.Add(line);
      }

      this._lines = new Vector3[BoundingBoxUtilities._Num_Lines, BoundingBoxUtilities._Num_Points_Per_Line];
      for (var j = 0; j < BoundingBoxUtilities._Num_Lines; j++) {
        this.Lines[j, 0] = this._lines_list[j][0];
        this.Lines[j, 1] = this._lines_list[j][1];
      }
      */
    }

    void FitRenderersOobb() { throw new System.NotImplementedException(); }

    void FitCollidersOobb() { throw new System.NotImplementedException(); }

    /// <summary>
    /// </summary>
    void RecalculatePoints() {
      this._point_tfr = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Top_Front_Right);
      this._point_tfl = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Top_Front_Left);
      this._point_tbl = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Top_Back_Left);
      this._point_tbr = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Top_Back_Right);
      this._point_bfr = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Bottom_Front_Right);
      this._point_bfl = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Bottom_Front_Left);
      this._point_bbl = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Bottom_Back_Left);
      this._point_bbr = this._Bounds_Offset
                        + UnityEngine.Vector3.Scale(a : this._Bounds.extents,
                                                    b : droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                             .BoundingBoxUtilities._Bottom_Back_Right);

      this._points = new[] {
                               this._point_tfr,
                               this._point_tfl,
                               this._point_tbl,
                               this._point_tbr,
                               this._point_bfr,
                               this._point_bfl,
                               this._point_bbl,
                               this._point_bbr
                           };
    }

    /// <summary>
    /// </summary>
    void RecalculateLines() {
      var transform1 = this.transform;
      if (this._bb_transform) {
        transform1 = this._bb_transform;
      }

      var rot = UnityEngine.Quaternion.identity;
      var pos = UnityEngine.Vector3.zero;
      if (this._use_bb_transform) {
        rot = transform1.rotation;
      }

      pos = transform1.position;

      this._lines_list.Clear();

      for (var i = 0; i < 4; i++) {
        //width
        var line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 1] + pos};
        this._lines_list.Add(item : line);

        //height
        line = new[] {rot * this.Points[i] + pos, rot * this.Points[i + 4] + pos};
        this._lines_list.Add(item : line);

        //depth
        line = new[] {rot * this.Points[2 * i] + pos, rot * this.Points[2 * i + 3 - 4 * (i % 2)] + pos};
        this._lines_list.Add(item : line);
      }

      this._lines =
          new UnityEngine.Vector3[droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities
                                       ._Num_Lines, droid.Runtime.GameObjects.BoundingBoxes.Experimental
                                                         .BoundingBoxUtilities._Num_Points_Per_Line];
      for (var j = 0;
           j < droid.Runtime.GameObjects.BoundingBoxes.Experimental.BoundingBoxUtilities._Num_Lines;
           j++) {
        this.Lines[j, 0] = this._lines_list[index : j][0];
        this.Lines[j, 1] = this._lines_list[index : j][1];
      }
    }

    #region fields

    /// <summary>
    /// </summary>
    UnityEngine.Transform _bb_transform = null;

    /// <summary>
    /// </summary>
    protected UnityEngine.Bounds _Bounds = new UnityEngine.Bounds();

    /// <summary>
    /// </summary>
    protected UnityEngine.Vector3 _Bounds_Offset = UnityEngine.Vector3.zero;

    /// <summary>
    /// </summary>
    UnityEngine.Collider[] _children_colliders = null;

    /// <summary>
    /// </summary>
    UnityEngine.MeshFilter[] _children_meshes = null;

    UnityEngine.GameObject _empty_go = null;

    /// <summary>
    /// </summary>
    UnityEngine.Vector3[,] _lines = null;

    System.Collections.Generic.List<UnityEngine.Vector3[]> _lines_list =
        new System.Collections.Generic.List<UnityEngine.Vector3[]>();

    UnityEngine.Collider _local_collider;

    UnityEngine.MeshFilter _local_mesh;

    UnityEngine.Vector3 _point_bbl;
    UnityEngine.Vector3 _point_bbr;
    UnityEngine.Vector3 _point_bfl;
    UnityEngine.Vector3 _point_bfr;
    UnityEngine.Vector3 _point_tbl;
    UnityEngine.Vector3 _point_tbr;
    UnityEngine.Vector3 _point_tfl;
    UnityEngine.Vector3 _point_tfr;

    /// <summary>
    /// </summary>
    UnityEngine.Vector3[] _points = null;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    public bool _use_bb_transform = false;

    [UnityEngine.SerializeField] bool _use_shared_mesh = false;

    /// <summary>
    /// </summary>
    [droid.Runtime.Utilities.SearchableEnumAttribute]
    public droid.Runtime.Enums.BoundingBox.BasedOnEnum _basedOnEnum =
        droid.Runtime.Enums.BoundingBox.BasedOnEnum.Collider_;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    float bb_margin = 0;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum BbAligning =
        droid.Runtime.Enums.BoundingBox.BoundingBoxOrientationEnum.Axis_aligned_;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool cacheChildren = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    UnityEngine.Color editorPreviewLineColor = new UnityEngine.Color(1f,
                                                                     0.36f,
                                                                     0.38f,
                                                                     0.74f);

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    droid.Runtime.Interfaces.IAbstractPrototypingEnvironment environment = null;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool freezeAfterFirstCalculation = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool includeChildren = false;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _only_active_children = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool includeSelf = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool OnAwakeSetup = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool RunInEditModeSetup = false;

    #endregion

    #region Properties

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3[] BoundingBoxCoordinates {
      get {
        return new[] {
                         this._point_tfl,
                         this._point_tfr,
                         this._point_tbl,
                         this._point_tbr,
                         this._point_bfl,
                         this._point_bfr,
                         this._point_bbl,
                         this._point_bbr
                     };
      }
    }

    /// <summary>
    /// </summary>
    public UnityEngine.Bounds Bounds { get { return this._Bounds; } }

    public UnityEngine.Vector3 Max { get { return this._Bounds.max; } }

    public UnityEngine.Vector3 Min { get { return this._Bounds.min; } }

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesAsString {
      get {
        var str_rep = "";
        str_rep += $"\"_top_front_left\":{this.BoundingBoxCoordinates[0]}, ";
        str_rep += $"\"_top_front_right\":{this.BoundingBoxCoordinates[1]}, ";
        str_rep += $"\"_top_back_left\":{this.BoundingBoxCoordinates[2]}, ";
        str_rep += $"\"_top_back_right\":{this.BoundingBoxCoordinates[3]}, ";
        str_rep += $"\"_bottom_front_left\":{this.BoundingBoxCoordinates[4]}, ";
        str_rep += $"\"_bottom_front_right\":{this.BoundingBoxCoordinates[5]}, ";
        str_rep += $"\"_bottom_back_left\":{this.BoundingBoxCoordinates[6]}, ";
        str_rep += $"\"_bottom_back_right\":{this.BoundingBoxCoordinates[7]}";
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public string BoundingBoxCoordinatesWorldSpaceAsJson {
      get {
        var str_rep = "{";
        var transform1 = this.transform;
        if (this._use_bb_transform) {
          transform1 = this._bb_transform;
        }

        var rotation = transform1.rotation;
        var position = transform1.position;
        if (this.environment != null) {
          str_rep +=
              $"\"top_front_left\":{JsonifyVec3(vec : this.environment.TransformPoint(point : rotation * this._point_tfl + position))}, ";
          str_rep +=
              $"\"bottom_back_right\":{JsonifyVec3(vec : this.environment.TransformPoint(point : rotation * this._point_bbr + position))}";
        }

        str_rep += "}";
        return str_rep;
      }
    }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3[,] Lines { get { return this._lines; } }

    /// <summary>
    /// </summary>
    public UnityEngine.Vector3[] Points { get { return this._points; } }

    /// <summary>
    /// </summary>
    public UnityEngine.Color EditorPreviewLineColor {
      get { return this.editorPreviewLineColor; }
      set { this.editorPreviewLineColor = value; }
    }

    #endregion

    #if UNITY_EDITOR
    /// <summary>
    /// </summary>
    void OnValidate() {
      if (!this.enabled) {
        return;
      }

      if (UnityEditor.EditorApplication.isPlaying) {
        return;
      }

      this.CalculateBoundingBox();
    }

    /// <summary>
    /// </summary>
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        UnityEngine.Gizmos.color = this.editorPreviewLineColor;

        if (this.Lines != null) {
          for (var i = 0; i < this.Lines.GetLength(0); i++) {
            UnityEngine.Gizmos.DrawLine(from : this.Lines[i, 0], to : this.Lines[i, 1]);
          }
        } else {
          UnityEngine.Gizmos.DrawWireCube(center : this.Bounds.center, size : this.Bounds.size);
        }

        if (this._bb_transform) {
          UnityEditor.Handles.Label(position : this._bb_transform.position, text : this.name);
        } else {
          UnityEditor.Handles.Label(position : this.transform.position, text : this.name);
        }
      }
    }
    #endif
  }
}