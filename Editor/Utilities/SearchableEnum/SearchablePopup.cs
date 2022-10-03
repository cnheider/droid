#if UNITY_EDITOR
namespace droid.Editor.Utilities.SearchableEnum {
  /// <inheritdoc />
  /// <summary>
  ///   A popup window that displays a list of options and may use a search
  ///   string to filter the displayed content.
  /// </summary>
  public class SearchablePopup : UnityEditor.PopupWindowContent {
    #region -- Initialization ---------------------------------------------

    SearchablePopup(string[] names, int current_index, System.Action<int> on_selection_made) {
      this._list = new FilteredList(items : names);
      this._current_index = current_index;
      this._on_selection_made = on_selection_made;

      this._hover_index = current_index;
      this._scroll_to_index = current_index;
      this._scroll_offset = this.GetWindowSize().y - _row_height * 2;
    }

    #endregion -- Initialization ------------------------------------------

    #region Nested type: FilteredList

    #region -- Helper Classes ---------------------------------------------

    /// <summary>
    ///   Stores a list of strings and can return a subset of that list that
    ///   matches a given filter string.
    /// </summary>
    class FilteredList {
      /// <summary> All posibile items in the list. </summary>
      readonly string[] _all_items;

      /// <summary> Create a new filtered list. </summary>
      /// <param name="items">All The items to filter.</param>
      public FilteredList(string[] items) {
        this._all_items = items;
        this.Entries = new System.Collections.Generic.List<Entry>();
        this.UpdateFilter("");
      }

      /// <summary> The current string filtering the list. </summary>
      public string Filter { get; private set; }

      /// <summary> All valid entries for the current filter. </summary>
      public System.Collections.Generic.List<Entry> Entries { get; }

      /// <summary> Total possible entries in the list. </summary>
      public int MaxLength { get { return this._all_items.Length; } }

      /// <summary>
      ///   Sets a new filter string and updates the Entries that match the
      ///   new filter if it has changed.
      /// </summary>
      /// <param name="filter">String to use to filter the list.</param>
      /// <returns>
      ///   True if the filter is updated, false if newFilter is the same
      ///   as the current Filter and no update is necessary.
      /// </returns>
      public bool UpdateFilter(string filter) {
        if (this.Filter == filter) {
          return false;
        }

        this.Filter = filter;
        this.Entries.Clear();

        for (var i = 0; i < this._all_items.Length; i++) {
          if (string.IsNullOrEmpty(value : this.Filter)
              || this._all_items[i].ToLower().Contains(value : this.Filter.ToLower())) {
            var entry = new Entry {_Index = i, _Text = this._all_items[i]};
            if (string.Equals(a : this._all_items[i],
                              b : this.Filter,
                              comparisonType : System.StringComparison.CurrentCultureIgnoreCase)) {
              this.Entries.Insert(0, item : entry);
            } else {
              this.Entries.Add(item : entry);
            }
          }
        }

        return true;
      }

      #region Nested type: Entry

      /// <summary>
      ///   An entry in the filtererd list, mapping the text to the
      ///   original index.
      /// </summary>
      public struct Entry {
        public int _Index;
        public string _Text;
      }

      #endregion
    }

    #endregion -- Helper Classes ------------------------------------------

    #endregion

    #region -- Constants --------------------------------------------------

    /// <summary> Height of each element in the popup list. </summary>
    const float _row_height = 16.0f;

    /// <summary> How far to indent list entries. </summary>
    const float _row_indent = 8.0f;

    /// <summary> Name to use for the text field for search. </summary>
    const string _search_control_name = "EnumSearchText";

    #endregion -- Constants -----------------------------------------------

    #region -- Static Functions -------------------------------------------

    /// <summary> Show a new SearchablePopup. </summary>
    /// <param name="activator_rect">
    ///   Rectangle of the button that triggered the popup.
    /// </param>
    /// <param name="options">List of strings to choose from.</param>
    /// <param name="current">
    ///   Index of the currently selected string.
    /// </param>
    /// <param name="on_selection_made">
    ///   Callback to trigger when a choice is made.
    /// </param>
    public static void Show(UnityEngine.Rect activator_rect,
                            string[] options,
                            int current,
                            System.Action<int> on_selection_made) {
      var win = new SearchablePopup(names : options,
                                    current_index : current,
                                    on_selection_made : on_selection_made);
      UnityEditor.PopupWindow.Show(activatorRect : activator_rect, windowContent : win);
    }

    /// <summary>
    ///   Force the focused window to redraw. This can be used to make the
    ///   popup more responsive to mouse movement.
    /// </summary>
    static void Repaint() {
      var window = UnityEditor.EditorWindow.focusedWindow;
      if (window) {
        window.Repaint();
      }
    }

    /// <summary> Draw a generic box. </summary>
    /// <param name="rect">Where to draw.</param>
    /// <param name="tint">Color to tint the box.</param>
    static void DrawBox(UnityEngine.Rect rect, UnityEngine.Color tint) {
      var c = UnityEngine.GUI.color;
      UnityEngine.GUI.color = tint;
      UnityEngine.GUI.Box(position : rect, "", style : _selection);
      UnityEngine.GUI.color = c;
    }

    #endregion -- Static Functions ----------------------------------------

    #region -- Private Variables ------------------------------------------

    /// <summary> Callback to trigger when an item is selected. </summary>
    readonly System.Action<int> _on_selection_made;

    /// <summary>
    ///   Index of the item that was selected when the list was opened.
    /// </summary>
    readonly int _current_index;

    /// <summary>
    ///   Container for all available options that does the actual string
    ///   filtering of the content.
    /// </summary>
    readonly FilteredList _list;

    /// <summary> Scroll offset for the vertical scroll area. </summary>
    UnityEngine.Vector2 _scroll;

    /// <summary>
    ///   Index of the item under the mouse or selected with the keyboard.
    /// </summary>
    int _hover_index;

    /// <summary>
    ///   An item index to scroll to on the next draw.
    /// </summary>
    int _scroll_to_index;

    /// <summary>
    ///   An offset to apply after scrolling to scrollToIndex. This can be
    ///   used to control if the selection appears at the top, bottom, or
    ///   center of the popup.
    /// </summary>
    float _scroll_offset;

    #endregion -- Private Variables ---------------------------------------

    #region -- GUI Styles -------------------------------------------------

    // GUIStyles implicitly cast from a string. This triggers a lookup into
    // the current skin which will be the editor skin and lets us get some
    // built-in styles.

    static UnityEngine.GUIStyle _search_box = "ToolbarSeachTextField";
    static UnityEngine.GUIStyle _cancel_button = "ToolbarSeachCancelButton";
    static UnityEngine.GUIStyle _disabled_cancel_button = "ToolbarSeachCancelButtonEmpty";
    static UnityEngine.GUIStyle _selection = "SelectionRect";

    #endregion -- GUI Styles ----------------------------------------------

    #region -- PopupWindowContent Overrides -------------------------------

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnOpen() {
      base.OnOpen();
      // Force a repaint every frame to be responsive to mouse hover.
      UnityEditor.EditorApplication.update += Repaint;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void OnClose() {
      base.OnClose();
      UnityEditor.EditorApplication.update -= Repaint;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override UnityEngine.Vector2 GetWindowSize() {
      return new UnityEngine.Vector2(x : base.GetWindowSize().x,
                                     y : UnityEngine.Mathf.Min(600,
                                                               b : this._list.MaxLength * _row_height
                                                                   + UnityEditor.EditorStyles.toolbar
                                                                       .fixedHeight));
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="rect"></param>
    public override void OnGUI(UnityEngine.Rect rect) {
      var search_rect = new UnityEngine.Rect(0,
                                             0,
                                             width : rect.width,
                                             height : UnityEditor.EditorStyles.toolbar.fixedHeight);
      var scroll_rect = UnityEngine.Rect.MinMaxRect(0,
                                                    ymin : search_rect.yMax,
                                                    xmax : rect.xMax,
                                                    ymax : rect.yMax);

      this.HandleKeyboard();
      this.DrawSearch(rect : search_rect);
      this.DrawSelectionArea(scroll_rect : scroll_rect);
    }

    #endregion -- PopupWindowContent Overrides ----------------------------

    #region -- GUI --------------------------------------------------------

    void DrawSearch(UnityEngine.Rect rect) {
      if (UnityEngine.Event.current.type == UnityEngine.EventType.Repaint) {
        UnityEditor.EditorStyles.toolbar.Draw(position : rect,
                                              false,
                                              false,
                                              false,
                                              false);
      }

      var search_rect = new UnityEngine.Rect(source : rect);
      search_rect.xMin += 6;
      search_rect.xMax -= 6;
      search_rect.y += 2;
      search_rect.width -= _cancel_button.fixedWidth;

      UnityEngine.GUI.FocusControl(name : _search_control_name);
      UnityEngine.GUI.SetNextControlName(name : _search_control_name);
      var new_text =
          UnityEngine.GUI.TextField(position : search_rect, text : this._list.Filter, style : _search_box);

      if (this._list.UpdateFilter(filter : new_text)) {
        this._hover_index = 0;
        this._scroll = UnityEngine.Vector2.zero;
      }

      search_rect.x = search_rect.xMax;
      search_rect.width = _cancel_button.fixedWidth;

      if (string.IsNullOrEmpty(value : this._list.Filter)) {
        UnityEngine.GUI.Box(position : search_rect,
                            content : UnityEngine.GUIContent.none,
                            style : _disabled_cancel_button);
      } else if (UnityEngine.GUI.Button(position : search_rect, "x", style : _cancel_button)) {
        this._list.UpdateFilter("");
        this._scroll = UnityEngine.Vector2.zero;
      }
    }

    void DrawSelectionArea(UnityEngine.Rect scroll_rect) {
      var content_rect = new UnityEngine.Rect(0,
                                              0,
                                              width : scroll_rect.width
                                                      - UnityEngine.GUI.skin.verticalScrollbar.fixedWidth,
                                              height : this._list.Entries.Count * _row_height);

      this._scroll = UnityEngine.GUI.BeginScrollView(position : scroll_rect,
                                                     scrollPosition : this._scroll,
                                                     viewRect : content_rect);

      var row_rect = new UnityEngine.Rect(0,
                                          0,
                                          width : scroll_rect.width,
                                          height : _row_height);

      for (var i = 0; i < this._list.Entries.Count; i++) {
        if (this._scroll_to_index == i
            && (UnityEngine.Event.current.type == UnityEngine.EventType.Repaint
                || UnityEngine.Event.current.type == UnityEngine.EventType.Layout)) {
          var r = new UnityEngine.Rect(source : row_rect);
          r.y += this._scroll_offset;
          UnityEngine.GUI.ScrollTo(position : r);
          this._scroll_to_index = -1;
          this._scroll.x = 0;
        }

        if (row_rect.Contains(point : UnityEngine.Event.current.mousePosition)) {
          if (UnityEngine.Event.current.type == UnityEngine.EventType.MouseMove
              || UnityEngine.Event.current.type == UnityEngine.EventType.ScrollWheel) {
            this._hover_index = i;
          }

          if (UnityEngine.Event.current.type == UnityEngine.EventType.MouseDown) {
            this._on_selection_made(obj : this._list.Entries[index : i]._Index);
            UnityEditor.EditorWindow.focusedWindow.Close();
          }
        }

        this.DrawRow(row_rect : row_rect, i : i);

        row_rect.y = row_rect.yMax;
      }

      UnityEngine.GUI.EndScrollView();
    }

    void DrawRow(UnityEngine.Rect row_rect, int i) {
      if (this._list.Entries[index : i]._Index == this._current_index) {
        DrawBox(rect : row_rect, tint : UnityEngine.Color.cyan);
      } else if (i == this._hover_index) {
        DrawBox(rect : row_rect, tint : UnityEngine.Color.white);
      }

      var label_rect = new UnityEngine.Rect(source : row_rect);
      label_rect.xMin += _row_indent;

      UnityEngine.GUI.Label(position : label_rect, text : this._list.Entries[index : i]._Text);
    }

    /// <summary>
    ///   Process keyboard input to navigate the choices or make a selection.
    /// </summary>
    void HandleKeyboard() {
      if (UnityEngine.Event.current.type == UnityEngine.EventType.KeyDown) {
        if (UnityEngine.Event.current.keyCode == UnityEngine.KeyCode.DownArrow) {
          this._hover_index =
              UnityEngine.Mathf.Min(a : this._list.Entries.Count - 1, b : this._hover_index + 1);
          UnityEngine.Event.current.Use();
          this._scroll_to_index = this._hover_index;
          this._scroll_offset = _row_height;
        }

        if (UnityEngine.Event.current.keyCode == UnityEngine.KeyCode.UpArrow) {
          this._hover_index = UnityEngine.Mathf.Max(0, b : this._hover_index - 1);
          UnityEngine.Event.current.Use();
          this._scroll_to_index = this._hover_index;
          this._scroll_offset = -_row_height;
        }

        if (UnityEngine.Event.current.keyCode == UnityEngine.KeyCode.Return) {
          if (this._hover_index >= 0 && this._hover_index < this._list.Entries.Count) {
            this._on_selection_made(obj : this._list.Entries[index : this._hover_index]._Index);
            UnityEditor.EditorWindow.focusedWindow.Close();
          }
        }

        if (UnityEngine.Event.current.keyCode == UnityEngine.KeyCode.Escape) {
          UnityEditor.EditorWindow.focusedWindow.Close();
        }
      }
    }

    #endregion -- GUI -----------------------------------------------------
  }
}
#endif