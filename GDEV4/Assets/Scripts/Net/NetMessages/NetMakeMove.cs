using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine;

public class NetMakeMove : NetMessage {
    
    public FixedString64 playerElementNW; // WriteString is outdated
    public FixedString64 playerName; // WriteString is outdated
    public int teamId;

    // Making the data
    public NetMakeMove() {
        Code = OpCode.MAKE_MOVE;
    }

    // Receiving the data
    public NetMakeMove(DataStreamReader reader) {
        Code = OpCode.MAKE_MOVE;
        Deserialize(reader);
    }


    // Pack the data
    public override void Serialize(ref DataStreamWriter writer) {
        writer.WriteByte((byte)Code);
        writer.WriteFixedString64(playerElementNW);
        writer.WriteFixedString64(playerName);
        writer.WriteInt(teamId);
    }

    // Unpack the data
    public override void Deserialize(DataStreamReader reader) {
        playerElementNW = reader.ReadFixedString64();
        playerName = reader.ReadFixedString64();
        teamId = reader.ReadInt();
    }


    // If data received on the client
    public override void ReceivedOnClient() {
        NetUtility.C_MAKE_MOVE?.Invoke(this);
    }

    // If data received on the server
    public override void ReceivedOnServer(NetworkConnection cnn) {
        NetUtility.S_MAKE_MOVE?.Invoke(this, cnn);
    }
}
