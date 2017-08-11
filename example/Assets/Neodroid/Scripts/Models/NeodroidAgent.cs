using Neodroid.Messaging;
using Neodroid.Messaging.Messages;
using Neodroid.Models.Motors;
using Neodroid.Models.Observers;
using Neodroid.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Neodroid.Evaluation;

namespace Neodroid.Models {
  public class NeodroidAgent : MonoBehaviour, HasRegister<Actor>, HasRegister<Observer> {

    private Dictionary<string, Actor> _actors = new Dictionary<string, Actor>();
    private Dictionary<string, Observer> _observers = new Dictionary<string, Observer>();

    // Public unity parameters
    public string _ip_address;
    public int _port;
    public bool _continue_lastest_reaction_on_disconnect = false;

    public ObjectiveFunction _objective_function;
    public bool _debug = false;

    // Private
    MessageServer _message_server;
    bool _waiting_for_reaction = true;

    private Reaction _lastest_reaction = null;

    private void Start() {

      string[] arguments = Environment.GetCommandLineArgs();

      for (int i = 0; i < arguments.Length; i++) {
        if (arguments[i] == "-ip") {
          _ip_address = arguments[i + 1];
        }
        if (arguments[i] == "-port") {
          _port = int.Parse(arguments[i + 1]);
        }
      }

      if (_ip_address != "" || _port != 0)
        _message_server = new MessageServer(_ip_address, _port);
      else
        _message_server = new MessageServer();
      _message_server.ListenForClientToConnect(OnConnectCallback);
    }

    public string GetStatus() {
      return true.ToString();
    }

    public Dictionary<string, Actor> GetActors() {
      return _actors;
    }

    public void AddActor(Actor actor) {
      if (_debug) Debug.Log("Agent " + name + " has actor " + actor.name);
      _actors.Add(actor.name, actor);
    }

    public Dictionary<string, Observer> GetObservers() {
      return _observers;
    }

    public void AddObserver(Observer observer) {
      if (_debug) Debug.Log("Agent " + name + " has observer " + observer.name);
      _observers.Add(observer.name, observer);
    }

    void Update() { // Update is called once per frame, updates like actor position needs to be done on the main thread

      if (_lastest_reaction != null && _lastest_reaction._reset) {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Start();
      }

      if (_lastest_reaction != null && !_waiting_for_reaction ) {
        ExecuteReaction(_lastest_reaction);
        if (!_continue_lastest_reaction_on_disconnect)
          _lastest_reaction = null;
      }
    }

    private void LateUpdate() {
      if (!_waiting_for_reaction) {
        foreach (Observer obs in GetObservers().Values) {
          obs.GetComponent<Observer>().GetData();
        }
        _message_server.SendEnvironmentState(GetCurrentState());
        ResumeGame();
        _waiting_for_reaction = true;
      }
    }

    void FixedUpdate() {
        PauseGame();
    }

    private void OnDestroy() { //Deconstructor
      _message_server.KillPollingThread();
      _message_server.Destroy();
    }

    float energy_spent = 0f;

    EnvironmentState GetCurrentState() {
      foreach (Actor a in _actors.Values) {
        foreach (Motor m in a.GetMotors().Values) {
          energy_spent += m.GetEnergySpend();
        }
      }
      var reward = 0f;
      if (_objective_function != null)
        reward = _objective_function.Evaluate();

      return new EnvironmentState(Time.realtimeSinceStartup, energy_spent, _actors, _observers, reward);
    }

    void PauseGame() {
      Time.timeScale = 0;
    }

    void ResumeGame() {
      Time.timeScale = 1;
    }

    bool IsGamePaused() {
      return Time.timeScale == 0;
    }

    void ExecuteReaction(Reaction reaction) {
      var actors = GetActors();
      if(reaction != null && reaction._actor_motor_motions.Count>0)
      foreach (MotorMotion motion in reaction.GetMotions()) {
        var motion_actor_name = motion.GetActorName();
        var motion_motor_name = motion.GetMotorName();
        if (actors.ContainsKey(motion_actor_name)) {
          var motors = actors[motion_actor_name].GetMotors();
          if (motors.ContainsKey(motion_motor_name)) {
            motors[motion_motor_name].ApplyMotion(motion);
          } else {
            Debug.Log("Could find not motor with the specified name: " + motion_motor_name);
          }
        } else {
          Debug.Log("Could find not actor with the specified name: " + motion_actor_name);
        }
      }
    }

    //Callbacks
    void OnReceiveCallback(Reaction reaction) {
      if (_debug) Debug.Log("Received: " + reaction.ToString());
      _lastest_reaction = reaction;
      _waiting_for_reaction = false;
    }

    void OnDisconnectCallback() {
      if (_debug) Debug.Log("Client disconnected.");
    }

    void OnErrorCallback(string error) {
      if (_debug) Debug.Log("ErrorCallback: " + error);
    }

    void OnConnectCallback() {
      if (_debug) Debug.Log("Client connected.");
      _message_server.StartReceiving(OnReceiveCallback, OnDisconnectCallback, OnErrorCallback);
      //_message_server.StartReceiving(OnReceiveCallback, OnDisconnectCallback, OnErrorCallback);
      //_header_requested = true;
    }

    public void Register(Actor obj) {
      AddActor(obj);
    }

    public void Register(Observer obj) {
      AddObserver(obj);
    }

    private void OnApplicationQuit() {
      _message_server.KillPollingThread();
      _message_server.Destroy();
    }
  }
}
