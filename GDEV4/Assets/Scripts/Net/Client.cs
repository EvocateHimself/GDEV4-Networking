using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine;

public class Client : MonoBehaviour {
    
    #region Singleton implementation
    public static Client Instance { set; get; }

    private void Awake() {
        Instance = this;
    }
    #endregion

    public NetworkDriver driver;
    private NetworkConnection connection;

    private bool isActive = false;

    public Action connectionDropped;

    // Methods

    // When a server is created
    public void Init(string ip, ushort port) {
        driver = NetworkDriver.Create();
        NetworkEndPoint endpoint = NetworkEndPoint.Parse(ip, port);

        connection = driver.Connect(endpoint);

        Debug.Log("Attempting to connect to server on " + endpoint.Address);

        isActive = true;

        RegisterToEvent();
    }


    // When the server is shutdown
    public void Shutdown() {
        if(isActive) {
            UnregisterToEvent();
            driver.Dispose();
            isActive = false;
            connection = default(NetworkConnection);
        }
    }


    public void OnDestroy() {
        Shutdown();
    }


    public void Update() {
        if (!isActive) {
            return;
        }

        driver.ScheduleUpdate().Complete();
        CheckAlive();

        UpdateMessagePump();

    }


    private void CheckAlive() {
        if(!connection.IsCreated && isActive) {
            Debug.Log("Something went wrong, lost connection to server");
            connectionDropped?.Invoke();
            Shutdown();
        }
    }


    private void UpdateMessagePump() {

        DataStreamReader stream;

        // Create network event
        NetworkEvent.Type cmd;

        // If the network command is not empty
        while ((cmd = connection.PopEvent(driver, out stream)) != NetworkEvent.Type.Empty) {
            if (cmd == NetworkEvent.Type.Connect) {
                SendToServer(new NetWelcome());
                Debug.Log("We're connected!");
            } else if (cmd == NetworkEvent.Type.Data) {
                NetUtility.OnData(stream, default(NetworkConnection));
            } else if (cmd == NetworkEvent.Type.Disconnect) {
                Debug.Log("Client disconnected from server");
                connection = default(NetworkConnection);
                connectionDropped?.Invoke();
            }
        }
    }


    public void SendToServer(NetMessage msg) {
        DataStreamWriter writer;
        driver.BeginSend(connection, out writer);
        msg.Serialize(ref writer);
        driver.EndSend(writer);
    }


    // Event parsing
    
    private void RegisterToEvent() {
        NetUtility.C_KEEP_ALIVE += OnKeepAlive;
    }


    private void UnregisterToEvent() {
        NetUtility.C_KEEP_ALIVE -= OnKeepAlive;
    }


    private void OnKeepAlive(NetMessage nm) {
        // Send it back, to keep both sides alive
        SendToServer(nm);
    }
}


