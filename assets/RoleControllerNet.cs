using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;

public class RoleControllerNet : MonoBehaviour {

    private bool IsConnected = false;
    public static bool IsLeftTurning = false, IsRightTurning = false, IsKinectPause = false;
    private Rect rectNetConnection = new Rect(5, Screen.height - 35, 180, 30);
    private Socket socket;
	// Use this for initialization
	void Start () {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Any, 10001));
        socket.Listen(0);

        socket.BeginAccept(OnSocketAccepted, socket);
	}

    public void OnApplicationQuit()
    {
        socket.Close();
    // socket.Dispose();
    }

    

    private void OnSocketAccepted(IAsyncResult ar)
    {
        ThreadPool.QueueUserWorkItem((object action) =>
        {
            Socket server = (Socket)ar.AsyncState;
            Socket client = server.EndAccept(ar);

            IsConnected = true;

            byte[] buffer = new byte[8];
            client.BeginReceive(buffer, 0, 8, SocketFlags.None, OnReceive,
                new ReceiveState { socket = client, buffer = buffer });
            server.BeginAccept(OnSocketAccepted, server);
        });

    }

    private void OnReceive(IAsyncResult ar)
    {
        ThreadPool.QueueUserWorkItem((object action) =>
        {
            ReceiveState state = (ReceiveState)ar.AsyncState;
            int bytes = state.socket.EndReceive(ar);

            if (bytes > 0)
            {
                byte[] buffer = state.buffer;
                //Debug.Log(BitConverter.ToString(buffer));
                switch (buffer[0])
                { 
                    case 0:
                        IsRightTurning = false;
                        IsLeftTurning = true;
                        break;
                    case 1:
                        IsLeftTurning = false;
                        break;
                    case 2:
                        IsLeftTurning = false;
                        IsRightTurning = true;
                        break;
                    case 3:
                        IsRightTurning = false;
                        break;
                    case 4:
                        IsKinectPause = !IsKinectPause;
                        break;
                }
            }

            Socket client = state.socket;
            byte[] buffernew = new byte[8];
            client.BeginReceive(buffernew, 0, 8, SocketFlags.None, OnReceive,
                new ReceiveState { socket = client, buffer = buffernew });
        });
    }

    //public void OnGUI()
    //{
    //    GUI.skin.label.fontSize = 20;
    //    GUI.skin.label.alignment = TextAnchor.MiddleLeft;
    //    GUI.Label(rectNetConnection, String.Format("Controller: {0}", IsConnected ? "已连接" : "未连接"));
    //}

	// Update is called once per frame
	void Update () {
	
	}
}
