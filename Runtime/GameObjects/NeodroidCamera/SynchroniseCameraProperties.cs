﻿namespace droid.Runtime.GameObjects.NeodroidCamera {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.RequireComponent(requiredComponent : typeof(UnityEngine.Camera))]
  [UnityEngine.ExecuteInEditMode]
  [System.SerializableAttribute]
  public class SynchroniseCameraProperties : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    SynchroniseCameraProperties[] _cameras = null;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _run_only_in_edit_mode = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _only_run_on_awake = false;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_culling_mask = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_far_clip_plane = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_fov = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_near_clip_plane = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_orthographic_projection = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_orthographic_size = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_physicality = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_sensor_size = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_lens_shift = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_gate_fit = true;

    /// <summary>
    /// </summary>
    [UnityEngine.SerializeField]
    bool _sync_focal_length = true;

/*
    /// <summary>
    /// </summary>
    [SerializeField]
    bool _sync_sensor_type = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    bool _sync_fov_axis = true;
*/
    [UnityEngine.SerializeField] UnityEngine.Camera _camera;

    /// <summary>
    /// </summary>
    public float Foc {
      get { return this._camera.focalLength; }
      set {
        if (this._sync_focal_length) {
          this._camera.focalLength = value;
          this._old_foc = value;
        }
      }
    }

    /// <summary>
    /// </summary>
    public float OrtSize {
      get { return this._camera.orthographicSize; }
      set {
        if (this._sync_orthographic_size) {
          this._camera.orthographicSize = value;
          this._old_orthographic_size = value;
        }
      }
    }

    /// <summary>
    /// </summary>
    public float Near {
      get { return this._camera.nearClipPlane; }
      set {
        if (this._sync_far_clip_plane) {
          this._camera.nearClipPlane = value;
          this._old_near_clip_plane = value;
        }
      }
    }

    public float Fov {
      get { return this._camera.fieldOfView; }
      set {
        if (this._sync_fov) {
          this._camera.fieldOfView = value;
          this._old_fov = value;
        }
      }
    }

    public bool Ort {
      get { return this._camera.orthographic; }
      set {
        if (this._sync_orthographic_projection) {
          this._camera.orthographic = value;
          this._old_orthographic_projection = value;
        }
      }
    }

    public int Mask {
      get { return this._camera.cullingMask; }
      set {
        if (this._sync_culling_mask) {
          this._camera.cullingMask = value;
          this._old_culling_mask = value;
        }
      }
    }

    public float Far {
      get { return this._camera.farClipPlane; }
      set {
        if (this._sync_far_clip_plane) {
          this._camera.farClipPlane = value;
          this._old_far_clip_plane = value;
        }
      }
    }

    public bool Physicality {
      get { return this._camera.usePhysicalProperties; }
      set {
        if (this._sync_physicality) {
          this._camera.usePhysicalProperties = value;
          this._old_physicality = value;
        }
      }
    }

    public UnityEngine.Camera.GateFitMode Gate {
      get { return this._camera.gateFit; }
      set {
        if (this._sync_gate_fit) {
          this._camera.gateFit = value;
          this._old_gate_fit = value;
        }
      }
    }

    public UnityEngine.Vector2 SensSize {
      get { return this._camera.sensorSize; }
      set {
        if (this._sync_sensor_size) {
          this._camera.sensorSize = value;
          this._old_sensor_size = value;
        }
      }
    }

    public UnityEngine.Vector2 Shift {
      get { return this._camera.lensShift; }
      set {
        if (this._sync_lens_shift) {
          this._camera.lensShift = value;
          this._old_lens_shift = value;
        }
      }
    }

    /// <summary>
    /// </summary>
    public void Awake() {
      this._camera = this.GetComponent<UnityEngine.Camera>();
      if (this._camera) {
        this.OrtSize = this._camera.orthographicSize;
        this.Near = this._camera.nearClipPlane;
        this.Far = this._camera.farClipPlane;
        this.Mask = this._camera.cullingMask;
        this.Ort = this._camera.orthographic;
        this.Fov = this._camera.fieldOfView;
        this.Foc = this._camera.focalLength;
        this.Physicality = this._camera.usePhysicalProperties;
        this.Gate = this._camera.gateFit;
        this.SensSize = this._camera.sensorSize;
        this.Shift = this._camera.lensShift;

        this._cameras = FindObjectsOfType<SynchroniseCameraProperties>();
      } else {
        UnityEngine.Debug.Log("No camera component found on GameObject");
      }

      this.Sync_Cameras();
    }

    /// <summary>
    /// </summary>
    public void Update() {
      if (!this._only_run_on_awake) {
        if (this._run_only_in_edit_mode) {
          #if UNITY_EDITOR
          if (!UnityEngine.Application.isPlaying) {
            this.Sync_Cameras();
          }
          #endif
        } else {
          this.Sync_Cameras();
        }
      }
    }

    public void Sync_Cameras() {
      if (this.enabled) {
        if (this._camera) {
          if (this._sync_orthographic_size) {
            var orthographic_size = this.OrtSize;
            if (System.Math.Abs(value : this._old_orthographic_size - orthographic_size)
                > droid.Runtime.Utilities.NeodroidConstants._Double_Tolerance) {
              this.OrtSize = orthographic_size;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.OrtSize = orthographic_size;
                }
              }
            }
          }

          if (this._sync_near_clip_plane) {
            var near_clip_plane = this._camera.nearClipPlane;
            if (System.Math.Abs(value : this._old_near_clip_plane - near_clip_plane)
                > droid.Runtime.Utilities.NeodroidConstants._Double_Tolerance) {
              this._old_near_clip_plane = near_clip_plane;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Near = near_clip_plane;
                }
              }
            }
          }

          if (this._sync_far_clip_plane) {
            var far_clip_plane = this._camera.farClipPlane;
            if (System.Math.Abs(value : this._old_far_clip_plane - far_clip_plane)
                > droid.Runtime.Utilities.NeodroidConstants._Double_Tolerance) {
              this._old_far_clip_plane = far_clip_plane;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Far = far_clip_plane;
                }
              }
            }
          }

          if (this._sync_culling_mask) {
            var culling_mask = this._camera.cullingMask;
            if (this._old_culling_mask != culling_mask) {
              this._old_culling_mask = culling_mask;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Mask = culling_mask;
                }
              }
            }

            if (this._sync_orthographic_projection) {
              var orthographic = this._camera.orthographic;
              if (this._old_orthographic_projection != orthographic) {
                this._old_orthographic_projection = orthographic;
                for (var index = 0; index < this._cameras.Length; index++) {
                  var cam = this._cameras[index];
                  if (cam != this && cam != null) {
                    cam.Ort = orthographic;
                  }
                }
              }
            }
          }

          if (this._sync_fov) {
            var fov = this._camera.fieldOfView;
            if (System.Math.Abs(value : this._old_fov - fov)
                > droid.Runtime.Utilities.NeodroidConstants._Double_Tolerance) {
              this._old_fov = fov;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Fov = fov;
                }
              }
            }
          }

          if (this._sync_focal_length) {
            var foc = this._camera.focalLength;
            if (System.Math.Abs(value : this._old_foc - foc)
                > droid.Runtime.Utilities.NeodroidConstants._Double_Tolerance) {
              this._old_foc = foc;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Foc = foc;
                }
              }
            }
          }

          if (this._sync_physicality) {
            var physicality = this._camera.usePhysicalProperties;
            if (this._old_physicality != physicality) {
              this._old_physicality = physicality;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Physicality = physicality;
                }
              }
            }
          }

          if (this._sync_sensor_size) {
            var a = this._camera.sensorSize;
            if (this._old_sensor_size != a) {
              this._old_sensor_size = a;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.SensSize = a;
                }
              }
            }
          }

          if (this._sync_lens_shift) {
            var a = this._camera.lensShift;
            if (this._old_lens_shift != a) {
              this._old_lens_shift = a;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Shift = a;
                }
              }
            }
          }

          if (this._sync_gate_fit) {
            var a = this._camera.gateFit;
            if (this._old_gate_fit != a) {
              this._old_gate_fit = a;
              for (var index = 0; index < this._cameras.Length; index++) {
                var cam = this._cameras[index];
                if (cam != this && cam != null) {
                  cam.Gate = a;
                }
              }
            }
          }
        } else {
          #if NEODROID_DEBUG
          UnityEngine.Debug.Log(message : $"No Camera component found on {this.name} GameObject");
          #endif
        }
      } else {
        #if NEODROID_DEBUG
        //Debug.Log($"No SyncCameraProperties component found on {this.name} GameObject");

        #endif
      }
    }

    #region old

    int _old_culling_mask = 0;

    float _old_far_clip_plane = 0;

    float _old_fov = 0;

    float _old_near_clip_plane = 0;

    bool _old_orthographic_projection = false;

    float _old_orthographic_size = 0;

    float _old_foc;
    bool _old_physicality = false;
    UnityEngine.Camera.GateFitMode _old_gate_fit = UnityEngine.Camera.GateFitMode.Fill;
    UnityEngine.Vector2 _old_sensor_size = UnityEngine.Vector2.one;
    UnityEngine.Vector2 _old_lens_shift = UnityEngine.Vector2.zero;

    #endregion
  }
}