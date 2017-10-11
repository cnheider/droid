using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Neodroid.Evaluation;
using Neodroid.Utilities;
using Neodroid.NeodroidEnvironment.Observers;
using Neodroid.NeodroidEnvironment.Motors;
using Neodroid.Messaging;
using Neodroid.Messaging.Messages;

namespace Neodroid.NeodroidEnvironment {
  public class NeodroidAgent : MonoBehaviour, HasRegister<Actor>, HasRegister<Observer> {

    #region PublicMembers

    public string _ip_address = "127.0.0.1";
    public int _port = 5555;
    public bool _continue_lastest_reaction_on_disconnect = false;

    public int _episode_length = 0;
    //infinite
    public ObjectiveFunction _objective_function;
    public EnvironmentManager _environment_manager;
    public bool _debug = false;

    #endregion

    #region PrivateMembers

    Dictionary<string, Actor> _actors = new Dictionary<string, Actor> ();
    Dictionary<string, Observer> _observers = new Dictionary<string, Observer> ();
    MessageServer _message_server;
    bool _waiting_for_reaction = true;
    bool _client_connected = false;

    int _current_episode_frame = 0;
    Reaction _lastest_reaction = null;
    float energy_spent = 0f;

    #endregion

    #region UnityCallbacks

    void Start () {
      FetchCommmandLineArguments ();
      FindMissingMembers ();
      StartMessagingServer ();
    }

    void Update () { // Update is called once per frame, updates like actor position needs to be done on the main thread

      if (_episode_length > 0 && _current_episode_frame > _episode_length) {
        Debug.Log ("Maximum episode length reached, resetting");
        ResetRegisteredObjects ();
        _environment_manager.ResetEnvironment ();
        _current_episode_frame = 0;
        return;
      }

      if (_lastest_reaction != null && _lastest_reaction._reset) {
        if (_environment_manager) {
          ResetRegisteredObjects ();
          _environment_manager.ResetEnvironment ();
          _environment_manager.Configure ("IncreaseDifficulty");
          return;
        }
      }

      if (_lastest_reaction != null && !_waiting_for_reaction) {
        _current_episode_frame++;
        ExecuteReaction (_lastest_reaction);
      }

      if (!_continue_lastest_reaction_on_disconnect) {
        _lastest_reaction = null;
      }
    }

    void LateUpdate () {
      if (!_waiting_for_reaction) {
        UpdateObserversData ();
        _message_server.SendEnvironmentState (GetCurrentState ());
        _environment_manager.ResumeEnvironment ();
        _waiting_for_reaction = true;
      }
    }

    void FixedUpdate () {
      _environment_manager.PauseEnviroment ();
    }

    #endregion

    #region Helpers

    void FetchCommmandLineArguments () {
      string[] arguments = Environment.GetCommandLineArgs ();

      for (int i = 0; i < arguments.Length; i++) {
        if (arguments [i] == "-ip") {
          _ip_address = arguments [i + 1];
        }
        if (arguments [i] == "-port") {
          _port = int.Parse (arguments [i + 1]);
        }
      }
    }

    void FindMissingMembers () {
      if (!_environment_manager) {
        _environment_manager = FindObjectOfType<EnvironmentManager> ();
      }
      if (!_objective_function) {
        _objective_function = FindObjectOfType<ObjectiveFunction> ();
      }
    }

    void StartMessagingServer () {
      if (_ip_address != "" || _port != 0)
        _message_server = new MessageServer (_ip_address, _port);
      else
        _message_server = new MessageServer ();

      _message_server.ListenForClientToConnect (OnConnectCallback);
    }

    void UpdateObserversData () {
      foreach (Observer obs in GetObservers().Values) {
        obs.GetComponent<Observer> ().GetData ();
      }
    }

    EnvironmentState GetCurrentState () {
      foreach (Actor a in _actors.Values) {
        foreach (Motor m in a.GetMotors().Values) {
          energy_spent += m.GetEnergySpend ();
        }
      }
      var reward = 0f;
      if (_objective_function != null)
        reward = _objective_function.Evaluate ();

      return new EnvironmentState (Time.realtimeSinceStartup, energy_spent, _actors, _observers, reward);
    }

    void ExecuteReaction (Reaction reaction) {
      var actors = GetActors ();
      if (reaction != null && reaction.GetMotions ().Length > 0)
        foreach (MotorMotion motion in reaction.GetMotions()) {
          var motion_actor_name = motion.GetActorName ();
          var motion_motor_name = motion.GetMotorName ();
          if (actors.ContainsKey (motion_actor_name)) {
            var motors = actors [motion_actor_name].GetMotors ();
            if (motors.ContainsKey (motion_motor_name)) {
              motors [motion_motor_name].ApplyMotion (motion);
            } else {
              Debug.Log ("Could find not motor with the specified name: " + motion_motor_name);
            }
          } else {
            Debug.Log ("Could find not actor with the specified name: " + motion_actor_name);
          }
        }
    }

    void AddActor (Actor actor) {
      if (_debug)
        Debug.Log ("Agent " + name + " has actor " + actor.name);
      _actors.Add (actor.name, actor);
    }

    void AddObserver (Observer observer) {
      if (_debug)
        Debug.Log ("Agent " + name + " has observer " + observer.name);
      _observers.Add (observer.name, observer);
    }

    void ResetRegisteredObjects () {
      foreach (var actor in _actors.Values) {
        actor.Reset ();
      }
      foreach (var observer in _observers.Values) {
        observer.Reset ();
      }
    }

    #endregion

    #region PublicMethods

    public Dictionary<string, Actor> GetActors () {
      return _actors;
    }

    public Dictionary<string, Observer> GetObservers () {
      return _observers;
    }

    public string GetStatus () {
      if (_client_connected)
        return "Connected";
      else
        return "Not Connected";
    }

    #endregion

    #region Callbacks

    void OnReceiveCallback (Reaction reaction) {
      _client_connected = true;
      if (_debug)
        Debug.Log ("Received: " + reaction.ToString ());
      _lastest_reaction = reaction;
      _waiting_for_reaction = false;
    }

    void OnDisconnectCallback () {
      _client_connected = false;
      if (_debug)
        Debug.Log ("Client disconnected.");
    }

    void OnErrorCallback (string error) {
      if (_debug)
        Debug.Log ("ErrorCallback: " + error);
    }

    void OnConnectCallback () {
      if (_debug)
        Debug.Log ("Client connected.");
      _message_server.StartReceiving (OnReceiveCallback, OnDisconnectCallback, OnErrorCallback);
    }

    void OnInterruptCallback () {

    }

    #endregion

    #region Registration

    public void Register (Actor obj) {
      AddActor (obj);
    }

    public void Register (Observer obj) {
      AddObserver (obj);
    }

    #endregion

    #region Deconstruction

    private void OnApplicationQuit () {
      _message_server.KillPollingAndListenerThread ();
    }

    private void OnDestroy () { //Deconstructor
      _message_server.Destroy ();
    }

    #endregion
  }
}
