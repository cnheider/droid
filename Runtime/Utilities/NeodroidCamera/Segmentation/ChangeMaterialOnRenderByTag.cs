﻿using System;
using System.Collections.Generic;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Utilities.NeodroidCamera.Segmentation {
  /// <inheritdoc cref="MonoBehaviour" />
  /// <summary>
  /// </summary>
  [ExecuteInEditMode]
  public class ChangeMaterialOnRenderByTag : Segmenter {
    /// <summary>
    /// </summary>
    Renderer[] _all_renders;

    /// <summary>
    /// </summary>
    MaterialPropertyBlock _block;

    /// <summary>
    /// </summary>
    [SerializeField] protected ColorByTag[] _Colors_By_Tag;

    /// <summary>
    /// </summary>
    LinkedList<Color>[] _original_colors;

    /// <summary>
    /// </summary>
    public bool _Replace_Untagged_Color = true;

    [SerializeField] ScriptableObjects.Segmentation _segmentation;

    /// <summary>
    /// </summary>
    Dictionary<string, Color> _tag_colors_dict = new Dictionary<string, Color>();

    /// <summary>
    /// </summary>
    public Color _Untagged_Color = Color.black;

    /// <summary>
    /// </summary>
    public ColorByTag[] ColorsByTag { get { return this._Colors_By_Tag; } }

    /// <summary>
    /// </summary>
    public override Dictionary<String, Color> ColorsDict { get { return this._tag_colors_dict; } }

    /// <summary>
    /// </summary>
    void Awake() {
      this._block = new MaterialPropertyBlock();
      this._tag_colors_dict.Clear();
      var colors_by_tag = this._Colors_By_Tag;
      if (colors_by_tag != null && colors_by_tag.Length > 0) {
        foreach (var tag_color in this._Colors_By_Tag) {
          if (!this._tag_colors_dict.ContainsKey(tag_color._Tag)) {
            this._tag_colors_dict.Add(tag_color._Tag, tag_color._Col);
          }
        }
      }

      if (this._segmentation) {
        var segmentation_color_by_tags = this._segmentation._Color_By_Tags;
        if (segmentation_color_by_tags != null) {
          foreach (var tag_color in segmentation_color_by_tags) {
            if (!this._tag_colors_dict.ContainsKey(tag_color._Tag)) {
              this._tag_colors_dict.Add(tag_color._Tag, tag_color._Col);
            }
          }
        }
      }

      this.Setup();
    }

    /// <summary>
    /// </summary>
    void Update() {
      this.Setup(); // Renderers maybe be disable and enabled, that is why every update we find all renderers again
    }

    /// <summary>
    /// </summary>
    void Setup() {
      this.CheckBlock();

      this._all_renders = FindObjectsOfType<Renderer>();
    }

    /// <summary>
    /// </summary>
    void Change() {
      this._original_colors = new LinkedList<Color>[this._all_renders.Length];
      for (var i = 0; i < this._original_colors.Length; i++) {
        this._original_colors[i] = new LinkedList<Color>();
      }

      this.CheckBlock();

      for (var i = 0; i < this._all_renders.Length; i++) {
        var c_renderer = this._all_renders[i];
        if (c_renderer) {
          if (this._tag_colors_dict != null && this._tag_colors_dict.ContainsKey(this._all_renders[i].tag)) {
            foreach (var mat in this._all_renders[i].sharedMaterials) {
              if (mat != null && mat.HasProperty(this._Default_Color_Tag)) {
                this._original_colors[i].AddFirst(mat.color);
              }


              this._block.SetColor(this._Segmentation_Color_Tag, this._tag_colors_dict[this._all_renders[i].tag]);

              this._block.SetColor(this._Outline_Color_Tag, this._Outline_Color);
              this._block.SetFloat(this._Outline_Width_Factor_Tag, this._Outline_Width_Factor);
              this._all_renders[i].SetPropertyBlock(this._block);
            }
          } else if (this._Replace_Untagged_Color) {
            foreach (var mat in this._all_renders[i].sharedMaterials) {
              if (mat != null && mat.HasProperty(this._Default_Color_Tag)) {
                this._original_colors[i].AddFirst(mat.color);
              }

              this._block.SetColor(this._Segmentation_Color_Tag, this._Untagged_Color);

              this._block.SetColor(this._Outline_Color_Tag, this._Outline_Color);
              this._block.SetFloat(this._Outline_Width_Factor_Tag, this._Outline_Width_Factor);
              this._all_renders[i].SetPropertyBlock(this._block);
            }
          }
        }
      }
    }

    void CheckBlock() {
      if (this._block == null) {
        this._block = new MaterialPropertyBlock();
      }
    }

    /// <summary>
    /// </summary>
    void Restore() {
      this.CheckBlock();

      for (var i = 0; i < this._all_renders.Length; i++) {
        if (this._all_renders[i]) {
          foreach (var mat in this._all_renders[i].sharedMaterials) {
            if (mat != null && this._original_colors != null && i < this._original_colors.Length) {
              var c = this._original_colors[i];
              var last = c?.Last;
              if (last != null) {
                var last_val = last.Value;
                this._block.SetColor(this._Default_Color_Tag, last_val);
                this._original_colors[i].RemoveLast();
                this._all_renders[i].SetPropertyBlock(this._block);
              }
            }
          }
        }
      }
    }

    /*void OnPreCull() {
      // change
    }*/

    void OnPreRender() {
      // change
      this.Change();
    }

    void OnPostRender() {
      // change back
      this.Restore();
    }
  }
}
