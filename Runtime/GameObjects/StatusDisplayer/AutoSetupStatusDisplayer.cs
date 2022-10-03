#if UNITY_EDITOR
namespace droid.Runtime.GameObjects.StatusDisplayer {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [UnityEngine.ExecuteInEditMode]
  public class AutoSetupStatusDisplayer : UnityEngine.MonoBehaviour {
    [UnityEngine.SerializeField] bool _clean_empty_no_target_events = true;

    [UnityEngine.SerializeField] droid.Runtime.Environments.NeodroidEnvironment _environment = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.TextUpdater _environment_frame = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.TextUpdater _environment_obs = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.TextUpdater _environment_text = null;

    [UnityEngine.SerializeField]
    droid.Runtime.Prototyping.ObjectiveFunctions.ObjectiveFunction _evaluation_function = null;

    [UnityEngine.SerializeField] droid.Runtime.Managers.AbstractNeodroidManager _manager = null;
    [UnityEngine.SerializeField] UnityEngine.UI.Button _reset_button = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.TextUpdater _signal = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.TextUpdater _episode_length = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.TextUpdater _status_text = null;

    [UnityEngine.SerializeField]
    droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.ToggleUpdater _terminated = null;

    [UnityEngine.SerializeField] UnityEngine.UI.Toggle _testing_toggle = null;

    [UnityEngine.SerializeField]
    UnityEngine.Events.UnityEventCallState _unity_event_call_state =
        UnityEngine.Events.UnityEventCallState.RuntimeOnly;

    void Start() {
      if (!this._environment) {
        this._environment = FindObjectOfType<droid.Runtime.Environments.NeodroidEnvironment>();
      }

      var neodroid_environment = this._environment;

      if (neodroid_environment != null) {
        this.TryRegister(poller : this._environment_text, f : neodroid_environment.IdentifierString);
        this.TryRegister(poller : this._environment_frame, f : neodroid_environment.FrameString);
        this.TryRegister(poller : this._environment_obs, f : neodroid_environment.ObservationsString);
        this.TryRegister(poller : this._terminated, f : neodroid_environment.TerminatedBoolean);
      }

      if (!this._evaluation_function) {
        this._evaluation_function =
            FindObjectOfType<droid.Runtime.Prototyping.ObjectiveFunctions.ObjectiveFunction>();
      }

      var evaluation_function = this._evaluation_function;
      if (evaluation_function != null) {
        this.TryRegister(poller : this._signal, f : evaluation_function.SignalString);
        this.TryRegister(poller : this._episode_length, f : evaluation_function.EpisodeLengthString);
      }

      if (!this._manager) {
        this._manager = FindObjectOfType<droid.Runtime.Managers.AbstractNeodroidManager>();
      }

      if (this._manager) {
        if (this._status_text) {
          this.TryRegister(poller : this._status_text, f : this._manager.StatusString);
        }

        if (this._testing_toggle) {
          this.TryRegisterProperty(poller : this._testing_toggle.onValueChanged,
                                   f : this._manager.SetTesting);
        }
      }

      if (this._reset_button) {
        this.TryRegisterVoid(poller : this._reset_button.onClick, f : this._manager.ResetAllEnvironments);
      }
    }

    void TryRegister(droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller poller,
                     UnityEngine.Events.UnityAction<
                         droid.Runtime.GameObjects.StatusDisplayer.EventRecipients.DataPoller> f) {
      if (poller) {
        var count = poller.PollEvent.GetPersistentEventCount();
        if (this._clean_empty_no_target_events && count > 0) {
          //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
          for (var i = 0; i < count; i++) {
            if (poller.PollEvent.GetPersistentTarget(index : i) == null
                || poller.PollEvent.GetPersistentMethodName(index : i) == null) {
              UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent : poller.PollEvent,
                index : i);
            }
          }
        }

        count = poller.PollEvent.GetPersistentEventCount();
        if (count == 0) {
          UnityEditor.Events.UnityEventTools.AddObjectPersistentListener(unityEvent : poller.PollEvent,
                                                                           call : f,
                                                                           argument : poller);
          poller.PollEvent.SetPersistentListenerState(0, state : this._unity_event_call_state);
        } else if (count > 0 && poller.PollEvent.GetPersistentTarget(0) != poller) {
          #if NEODROID_DEBUG
          if (this.Debugging) {
            UnityEngine.Debug.Log(message : $"PollEvent on {poller} already has a listeners");
          }
          #endif
        }
      }
    }

    void TryRegisterVoid(UnityEngine.Events.UnityEventBase poller, UnityEngine.Events.UnityAction f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistant listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(index : i) == null
              || poller.GetPersistentMethodName(index : i) == null) {
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent : poller, index : i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEditor.Events.UnityEventTools.AddVoidPersistentListener(unityEvent : poller, call : f);
        poller.SetPersistentListenerState(0, state : this._unity_event_call_state);
      } else if (count > 0) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"PollEvent on {poller} already has a listeners");
        }
        #endif
      }
    }

    void TryRegisterProperty(UnityEngine.UI.Toggle.ToggleEvent poller,
                             UnityEngine.Events.UnityAction<bool> f) {
      var count = poller.GetPersistentEventCount();
      if (this._clean_empty_no_target_events && count > 0) {
        //poller.PollEvent.RemoveAllListeners(); // Only non-persistent listeners.
        for (var i = 0; i < count; i++) {
          if (poller.GetPersistentTarget(index : i) == null
              || poller.GetPersistentMethodName(index : i) == null) {
            UnityEditor.Events.UnityEventTools.RemovePersistentListener(unityEvent : poller, index : i);
          }
        }
      }

      count = poller.GetPersistentEventCount();
      if (count == 0) {
        UnityEditor.Events.UnityEventTools.AddPersistentListener(unityEvent : poller, call : f);
        poller.SetPersistentListenerState(0, state : this._unity_event_call_state);
      } else if (count > 0) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          UnityEngine.Debug.Log(message : $"PollEvent on {poller} already has a listeners");
        }
        #endif
      }
    }

    #if NEODROID_DEBUG
    bool Debugging { get { return this._debugging; } set { this._debugging = value; } }
    [UnityEngine.SerializeField] bool _debugging;
    #endif
  }
}
#endif