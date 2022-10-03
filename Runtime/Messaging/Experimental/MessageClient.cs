namespace droid.Runtime.Messaging.Experimental {
  /// <summary>
  /// </summary>
  [System.SerializableAttribute]
  public class MessageClient {
    #region PublicMembers

    /// <summary>
    /// </summary>
    public bool _Listening_For_Clients;

    #endregion

    #region PrivateMembers

    /// <summary>
    /// </summary>
    System.Threading.Thread _polling_thread;
    #if NEODROID_DEBUG
    int _last_send_frame_number;

    float _last_send_time;
    #endif

    /// <summary>
    /// </summary>
    System.Threading.Thread _wait_for_client_thread;

    /// <summary>
    /// </summary>
    object _stop_lock = new object();

    object _thread_lock = new object();

    /// <summary>
    /// </summary>
    bool _stop_thread;

    /// <summary>
    /// </summary>
    bool _waiting_for_main_loop_to_send;

    /// <summary>
    /// </summary>
    bool _use_inter_process_communication;

    /// <summary>
    /// </summary>
    bool _debugging;

    /// <summary>
    /// </summary>
    NetMQ.Sockets.ResponseSocket _socket;

    //PairSocket _socket;
    /// <summary>
    /// </summary>
    string _ip_address;

    /// <summary>
    /// </summary>
    int _port;

    /// <summary>
    /// </summary>
    byte[] _byte_buffer;

    /// <summary>
    /// </summary>
    double _wait_time_seconds;

    #endregion

    #region PrivateMethods

    #region Threads

    /// <summary>
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="debug_callback"></param>
    void BindSocket(System.Action callback, System.Action<string> debug_callback) {
      if (this._debugging) {
        debug_callback?.Invoke("Start listening for clients");
      }

      try {
        if (this._use_inter_process_communication) {
          this._socket.Bind("ipc:///tmp/neodroid/messages");
        } else {
          this._socket.Bind(address : "tcp://" + this._ip_address + ":" + this._port);
        }

        callback?.Invoke();
        if (this._debugging) {
          debug_callback?.Invoke("Now listening for clients");
        }

        this._Listening_For_Clients = true;
      } catch (System.Exception exception) {
        if (this._debugging) {
          debug_callback?.Invoke(obj : $"BindSocket threw exception: {exception}");
        }
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="wait_time"></param>
    /// <returns></returns>
    public droid.Runtime.Messaging.Messages.Reaction[] Receive(System.TimeSpan wait_time) {
      //this._socket.Poll(); // TODO: MAYBE WAIT FOR CLIENT TO SEND

      droid.Runtime.Messaging.Messages.Reaction[] reactions = null;
      lock (this._thread_lock) {
        try {
          byte[] msg;

          if (wait_time > System.TimeSpan.Zero) {
            #if NEODROID_DEBUG
            var received =
                NetMQ.ReceivingSocketExtensions.TryReceiveFrameBytes(socket : this._socket,
                                                                     timeout : wait_time,
                                                                     bytes : out msg);
            if (this.Debugging) {
              if (received) {
                UnityEngine.Debug.Log("Received frame bytes");
              } else {
                UnityEngine.Debug.Log(message : $"Received nothing in {wait_time} seconds");
              }
            }
            #else
            this._socket.TryReceiveFrameBytes(wait_time, out msg);
            #endif
          } else {
            try {
              msg = NetMQ.ReceivingSocketExtensions.ReceiveFrameBytes(socket : this._socket);
            } catch (System.ArgumentNullException e) {
              msg = null;
              UnityEngine.Debug.Log(message : e);
            }
          }

          if (msg != null) { //&& msg.Length >= 4) {
            var flat_reaction =
                droid.Runtime.Messaging.FBS.FReactions
                     .GetRootAsFReactions(_bb : new FlatBuffers.ByteBuffer(buffer : msg));
            var tuple = FbsReactionUtilities.deserialise_reactions(reactions : flat_reaction);
            reactions = tuple.Item1; //TODO: Change tuple to the Reactions class
            var close = tuple.Item2;
            var api_version = tuple.Item3;
            var simulator_configuration = tuple.Item4;
          }
        } catch (System.Exception exception) {
          if (exception is NetMQ.TerminatingException) {
            return reactions;
          }

          UnityEngine.Debug.Log(message : exception);
        }
      }

      return reactions;
    }

    /// <summary>
    /// </summary>
    /// <param name="receive_callback"></param>
    /// <param name="disconnect_callback"></param>
    /// <param name="debug_callback"></param>
    void PollingThread(System.Action<droid.Runtime.Messaging.Messages.Reaction[]> receive_callback,
                       System.Action disconnect_callback,
                       System.Action<string> debug_callback) {
      while (this._stop_thread == false) {
        lock (this._thread_lock) {
          if (!this._waiting_for_main_loop_to_send) {
            var reactions =
                this.Receive(wait_time : System.TimeSpan.FromSeconds(value : this._wait_time_seconds));
            if (reactions != null) {
              receive_callback(obj : reactions);
              this._waiting_for_main_loop_to_send = true;
            }
          } else {
            if (this._debugging) {
              debug_callback("Waiting for main loop to send reply");
            }
          }
        }
      }

      disconnect_callback();
      if (!this._socket.IsDisposed) {
        if (this._use_inter_process_communication) {
          this._socket.Disconnect("inproc://neodroid");
        } else {
          this._socket.Disconnect(address : "tcp://" + this._ip_address + ":" + this._port);
        }
      }

      try {
        this._socket.Dispose();
        this._socket.Close();
      } finally {
        NetMQ.NetMQConfig.Cleanup(false);
      }
    }

    #endregion

    #endregion

    #region PublicMethods

    /// <summary>
    /// </summary>
    /// <param name="environment_states"></param>
    /// <param name="do_serialise_unobservables"></param>
    /// <param name="serialise_individual_observables"></param>
    /// <param name="do_serialise_observables"></param>
    /// <param name="simulator_configuration_message"></param>
    /// <param name="api_version"></param>
    public void SendStates(droid.Runtime.Messaging.Messages.EnvironmentSnapshot[] environment_states,
                           bool do_serialise_unobservables = false,
                           bool serialise_individual_observables = false,
                           bool do_serialise_observables = false,
                           droid.Runtime.Messaging.Messages.SimulatorConfigurationMessage
                               simulator_configuration_message = null,
                           string api_version = NeodroidRuntimeInfo._Version) {
      lock (this._thread_lock) {
        #if NEODROID_DEBUG
        if (this.Debugging) {
          var environment_state = System.Linq.Enumerable.ToArray(source : environment_states);
          if (environment_state.Length > 0) {
            if (environment_state[0] != null) {
              var frame_number = environment_state[0].FrameNumber;
              var time = environment_state[0].Time;
              var frame_number_duplicate = this._last_send_frame_number == frame_number;
              if (frame_number_duplicate && frame_number > 0) {
                UnityEngine.Debug.LogWarning(message :
                                             $"Sending duplicate frame! Frame number: {frame_number}");
              }

              if (frame_number <= this._last_send_frame_number) {
                UnityEngine.Debug.LogWarning(message :
                                             $"The current frame number {frame_number} is less or equal the last {this._last_send_frame_number}, SINCE AWAKE ({UnityEngine.Time.frameCount})");
              }

              if (time <= this._last_send_time) {
                UnityEngine.Debug.LogWarning(message :
                                             $"The current time {time} is less or equal the last {this._last_send_time}");
              }

              if (environment_state[0].Description != null) {
                UnityEngine.Debug.Log(message : $"State has description: {environment_state[0].Description}");
              }

              this._last_send_frame_number = frame_number;
              this._last_send_time = time;
            }
          } else {
            UnityEngine.Debug.LogWarning("No environment states where send.");
          }
        }
        #endif

        this._byte_buffer = FbsStateUtilities.Serialise(states : environment_states,
                                                        do_serialise_unobservables :
                                                        do_serialise_unobservables,
                                                        simulator_configuration :
                                                        simulator_configuration_message,
                                                        do_serialise_observables : do_serialise_observables,
                                                        api_version : api_version);
        NetMQ.OutgoingSocketExtensions.SendFrame(socket : this._socket, data : this._byte_buffer);
        this._waiting_for_main_loop_to_send = false;
      }
    }

    /// <summary>
    /// </summary>
    /// <param name="debug_callback"></param>
    public void ListenForClientToConnect(System.Action<string> debug_callback) {
      this.BindSocket(null, debug_callback : debug_callback);
    }

    /// <summary>
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="debug_callback"></param>
    public void ListenForClientToConnect(System.Action callback, System.Action<string> debug_callback) {
      this._wait_for_client_thread =
          new System.Threading.Thread(unused_param =>
                                          this.BindSocket(callback : callback,
                                                          debug_callback : debug_callback)) {
              IsBackground = true
          };
      // Is terminated with foreground threads, when they terminate
      this._wait_for_client_thread.Start();
    }

    /// <summary>
    /// </summary>
    /// <param name="cmd_callback"></param>
    /// <param name="disconnect_callback"></param>
    /// <param name="debug_callback"></param>
    public void StartReceiving(System.Action<droid.Runtime.Messaging.Messages.Reaction[]> cmd_callback,
                               System.Action disconnect_callback,
                               System.Action<string> debug_callback) {
      this._polling_thread =
          new System.Threading.Thread(unused_param => this.PollingThread(receive_callback : cmd_callback,
                                                                           disconnect_callback :
                                                                           disconnect_callback,
                                                                           debug_callback : debug_callback)) {
              IsBackground = true
          };
      // Is terminated with foreground threads, when they terminate
      this._polling_thread.Start();
    }

    #region Contstruction

    public MessageClient(string ip_address = "127.0.0.1",
                         int port = 6969,
                         bool use_inter_process_communication = false,
                         bool debug = false,
                         double wait_time_seconds = 2) {
      this._wait_time_seconds = wait_time_seconds;
      this.Debugging = debug;
      this._ip_address = ip_address;
      this._port = port;
      this._use_inter_process_communication = use_inter_process_communication;

      #if NEODROID_DEBUG
      if (this.Debugging) {
        UnityEngine.Debug.Log(message : $"Starting a message server at address:port {ip_address}:{port}");
      }
      #endif

      if (!this._use_inter_process_communication) {
        AsyncIO.ForceDotNet.Force();
      }

      this._socket = new NetMQ.Sockets.ResponseSocket();
    }

    public MessageClient(bool debug = false) : this("127.0.0.1",
                                                    6969,
                                                    false,
                                                    debug : debug) { }

    #endregion

    #region Getters

    /// <summary>
    /// </summary>
    public bool Debugging { get { return this._debugging; } set { this._debugging = value; } }

    #endregion

    #endregion

    #region Deconstruction

    /// <summary>
    /// </summary>
    public void Destroy() { this.CleanUp(); }

    /// <summary>
    /// </summary>
    public void CleanUp() {
      try {
        lock (this._stop_lock) {
          this._stop_thread = true;
        }

        if (this._use_inter_process_communication) {
          this._socket.Disconnect("ipc:///tmp/neodroid/messages");
        } else {
          this._socket.Disconnect(address : "tcp://" + this._ip_address + ":" + this._port);
        }

        try {
          this._socket.Dispose();
          this._socket.Close();
        } finally {
          NetMQ.NetMQConfig.Cleanup(false);
        }

        this._wait_for_client_thread?.Join();
        this._polling_thread?.Join();
      } catch {
        System.Console.WriteLine("Exception thrown while killing threads");
      }
    }

    #endregion
  }

  /// <summary>
  /// </summary>
  public class DepthPrediction : UnityEngine.MonoBehaviour {
    /// <summary>
    /// </summary>
    public bool recording;

    [UnityEngine.SerializeField] UnityEngine.UI.Text textBox;
    [UnityEngine.SerializeField] UnityEngine.Texture2D _tex;
    NetMQ.Sockets.RequestSocket _client = new NetMQ.Sockets.RequestSocket();

    System.Collections.Generic.List<UnityEngine.Vector3> _feature_points;

    bool _server_working = false;

    void Start() {
      this._client.Connect("tcp://10.24.11.87:8989");
      UnityEngine.Debug.Log("connected");
    }

    void FixedUpdate() {
      if (!this._server_working && this.recording) {
        this.GrabFrame();
      }
    }

    void GrabFrame() {
      try {
        var bytes = UnityEngine.ImageConversion.EncodeToJPG(tex : this._tex);

        this.StartCoroutine(routine : this.SendZmqRequest(bytes : bytes));
      } catch (System.Exception e) {
        var text = $"{this.textBox.text}{e}\n{e.Message}\n";
        this.textBox.text = text;
      }
    }

    System.Collections.IEnumerator SendZmqRequest(byte[] bytes) {
      var task =
          new System.Threading.Tasks.Task(() =>
                                              NetMQ.OutgoingSocketExtensions.SendFrame(socket : this._client,
                                                data : bytes));
      task.Start();

      while (!task.IsCompleted && !task.IsCanceled) {
        UnityEngine.Debug.Log("sending a frame");
        yield return new UnityEngine.WaitForEndOfFrame();
      }

      var response = "";
      var task2 =
          new System.Threading.Tasks.Task(() => response =
                                                    NetMQ.ReceivingSocketExtensions
                                                         .ReceiveFrameString(socket : this._client));
      task2.Start();

      while (!task2.IsCompleted) {
        UnityEngine.Debug.Log("waiting for response");
        yield return new UnityEngine.WaitForEndOfFrame();
      }

      this.textBox.text += response + "\n";

      try {
        var l = Newtonsoft.Json.JsonConvert
                          .DeserializeObject<
                              System.Collections.Generic.List<
                                  System.Collections.Generic.Dictionary<string, int[]>>>(value : response);

        this.textBox.text += l.Count + " in list\n";

        var num = 0;

        for (var index = 0; index < l.Count; index++) {
          var dict = l[index : index];
          var prefix = "person_" + num;

          this.textBox.text += dict.Count + " in dict\n";

          var point_dict = new System.Collections.Generic.Dictionary<string, UnityEngine.Vector3>();

          foreach (var kvp in dict) {
            var text = this.textBox.text;
            text += kvp.Key + " added\n";

            //hardcoded 240x426px
            var val = kvp.Value;

            text += "after val\n";

            var scale_x = UnityEngine.Screen.width / 240f;
            var scale_y = UnityEngine.Screen.height / 426f;

            text += "after scale\n";

            var scaled_vec = new UnityEngine.Vector3(x : val[0] * scale_x, y : val[1] * scale_y, z : val[2]);

            text += "after applying scale\n";
            this.textBox.text = text;

            point_dict.Add(key : kvp.Key, value : scaled_vec);

            this.textBox.text += "after dict add\n";
          }
        }
      } catch (System.Exception e) {
        this.textBox.text += e.ToString();
      }

      yield return new UnityEngine.WaitForEndOfFrame();

      this._server_working = false;
    }
  }
}