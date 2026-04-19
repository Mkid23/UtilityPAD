using GorillaNetworking;
using Photon.Pun;
using System;
using System.Collections.Generic;
using System.Text;

namespace TvvPancke.Classes
{
    internal class RoomManager /// Leave alone, only add if needed
    {
        public static void Join(string roomToJoin)
        {
            PhotonNetworkController.Instance.AttemptToJoinSpecificRoom(roomToJoin, JoinType.Solo);
        }

    }
}
