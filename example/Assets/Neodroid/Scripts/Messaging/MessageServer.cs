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
    }

    public void StartReceiving(Action<Reaction> cmd_callback, Action disconnect_callback, Action<String> error_callback) {
      _polling_thread = new Thread(unused_param => PollingThread(cmd_callback, disconnect_callback, error_callback));
      _polling_thread.IsBackground = true; // Is terminated with foreground threads, when they terminate
      _polling_thread.Start();
    }

    void PollingThread(Action<Reaction> receive_callback, Action disconnect_callback, Action<String> error_callback) {
      AsyncIO.ForceDotNet.Force();

      var timeout = new TimeSpan(0, 0, 1); //1sec
      byte[] msg;

      _socket = new ResponseSocket("@tcp://" + _ip_address + ":" + _port.ToString());
      while (stop_thread_ == false) {
        try {
          var message = _socket.TryReceiveFrameBytes(timeout, out msg);
          var reaction = FlatBufferReaction.GetRootAsFlatBufferReaction(new FlatBuffers.ByteBuffer(msg));
          error_callback(reaction.ToString());
          //receive_callback(action);
        } catch (Exception err) {
          error_callback(err.ToString());
        }

        _socket.Close();
        NetMQConfig.Cleanup();

        Thread.Sleep(10);
      }
    }

    public void SendEnvironmentState(EnvironmentState environment_state) {
      var byte_buffer = FlatBufferUtilities.build_state(environment_state);
      _socket.SendFrame(byte_buffer.Data);
    }

    public void Destroy() {
      KillPollingThread();
    }

    public void KillPollingThread() {
      lock (thisLock_) stop_thread_ = true;
      if (_polling_thread != null) {
        _polling_thread.Abort();
        _polling_thread.Join();
      }
    }
  }
}
