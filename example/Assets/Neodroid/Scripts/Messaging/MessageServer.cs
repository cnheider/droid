using System;
using System.Threading;

using Neodroid.Messaging.Messages;
using NetMQ;
using NetMQ.Sockets;

using Assets.Neodroid.Scripts.Messaging;
using Neodroid.Messaging.Models.Reaction;

namespace Neodroid.Messaging {
  class MessageServer {
    Thread _polling_thread;
    Thread _wait_for_client_thread;
    private Object _thread_lock = new Object ();
    bool _stop_thread_ = false;
    bool _waiting_for_main_loop_to_send =false;

    ResponseSocket _socket;
    string _ip_address;
    int _port;

    public MessageServer (string ip_address = "127.0.0.1", int port = 5555) {
      _ip_address = ip_address;
      _port = port;
      AsyncIO.ForceDotNet.Force ();
      _socket = new ResponseSocket ();
    }

    public void ListenForClientToConnect (Action callback) {
      _wait_for_client_thread = new Thread (unused_param => WaitForClientToConnect (callback));
      _wait_for_client_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _wait_for_client_thread.Start ();
    }

    void WaitForClientToConnect (Action callback) {
      _socket.Bind ("tcp://" + _ip_address + ":" + _port.ToString ());
      callback ();
    }

    public void StartReceiving (Action<Reaction> cmd_callback, Action disconnect_callback, Action<String> error_callback) {
      _polling_thread = new Thread (unused_param => PollingThread (cmd_callback, disconnect_callback, error_callback));
      _polling_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _polling_thread.Start ();
    }

    void PollingThread (Action<Reaction> receive_callback, Action disconnect_callback, Action<String> error_callback) {
      byte[] msg;
      while (_stop_thread_ == false) {
        if (!_waiting_for_main_loop_to_send) {
          try {
            //msg = _socket.TryReceiveFrameBytes ();
            _socket.TryReceiveFrameBytes (TimeSpan.FromSeconds (2), out msg);
            var flat_reaction = FlatBufferReaction.GetRootAsFlatBufferReaction (new FlatBuffers.ByteBuffer (msg));
            var reaction = FlatBufferUtilities.create_reaction (flat_reaction);
            receive_callback (reaction);
            _waiting_for_main_loop_to_send=true;
          } catch (Exception err) {
            error_callback (err.ToString ());
          }
        }
          
      }
        
      _socket.Disconnect(("tcp://" + _ip_address + ":" + _port.ToString ()));
      _socket.Close ();
      NetMQConfig.Cleanup ();
    }

    byte[] byte_buffer;

    public void SendEnvironmentState (EnvironmentState environment_state) {
      byte_buffer = FlatBufferUtilities.build_state (environment_state);
      _socket.SendFrame (byte_buffer);
      _waiting_for_main_loop_to_send = false;
    }

    public void Destroy () {
        KillPollingAndListenerThread ();
    }

    public void KillPollingAndListenerThread () {
      try {
        lock (_thread_lock) _stop_thread_ = true;
        _socket.Disconnect(("tcp://" + _ip_address + ":" + _port.ToString ()));
        _socket.Close ();
        NetMQConfig.Cleanup ();
        if (_wait_for_client_thread != null) {
          _wait_for_client_thread.Join();
        }
        if (_polling_thread != null) {
        //  _polling_thread.Abort ();
          _polling_thread.Join ();
        }
      } catch {
        System.Console.WriteLine ("Exception thrown while killing threads");
      }
    }
  }
}
