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
    private Object thisLock_ = new Object();
    bool stop_thread_ = false;

    ResponseSocket _socket;
    string _ip_address;
    int _port;

    public MessageServer(string ip_address = "127.0.0.1", int port = 5555) {
      _ip_address = ip_address;
      _port = port;
      AsyncIO.ForceDotNet.Force();
      _socket = new ResponseSocket();
    }

    public void ListenForClientToConnect(Action callback) {
      Thread _wait_for_client_thread = new Thread(unused_param => WaitForClientToConnect(callback));
      _wait_for_client_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _wait_for_client_thread.Start();
    }

    void WaitForClientToConnect(Action callback) {
      _socket.Bind("tcp://" + _ip_address + ":" + _port.ToString());
      callback();
    }

    public void StartReceiving(Action<Reaction> cmd_callback, Action disconnect_callback, Action<String> error_callback) {
      _polling_thread = new Thread(unused_param => PollingThread(cmd_callback, disconnect_callback, error_callback));
      _polling_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _polling_thread.Start();
    }

    void PollingThread(Action<Reaction> receive_callback, Action disconnect_callback, Action<String> error_callback) {
      byte[] msg;
      while (stop_thread_ == false) {
        try {
          msg = _socket.ReceiveFrameBytes();
          var flat_reaction = FlatBufferReaction.GetRootAsFlatBufferReaction(new FlatBuffers.ByteBuffer(msg));
          var reaction = FlatBufferUtilities.create_reaction(flat_reaction);
          receive_callback(reaction);
        } catch (Exception err) {
          error_callback(err.ToString());
        }

        Thread.Sleep(1000);
      }

      _socket.Close();
      NetMQConfig.Cleanup();
    }

    byte[] byte_buffer;

    public void SendEnvironmentState(EnvironmentState environment_state) {
      byte_buffer = FlatBufferUtilities.build_state(environment_state);
      _socket.SendFrame(byte_buffer);
    }

    public void Destroy() {
      _socket.Close();
      NetMQConfig.Cleanup();
      KillPollingThread();
    }

    public void KillPollingThread() {
      lock (thisLock_) stop_thread_ = true;
      _socket.Close();
      NetMQConfig.Cleanup();
      KillPollingThread();
      if (_polling_thread != null) {
        _polling_thread.Abort();
        _polling_thread.Join();
      }
    }
  }
}
