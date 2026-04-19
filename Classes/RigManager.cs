using Photon.Realtime;
using Photon.Pun;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace TvvPancke.Classes /// You can add, but not delete
{
    internal class RigManager : BaseUnityPlugin
    {
        public static bool RigIsInfected(VRRig vrrig)
        {
            bool result;
            if (vrrig == null || vrrig.mainSkin == null || vrrig.mainSkin.material == null)
            {
                result = false;
            }
            else
            {
                string name = vrrig.mainSkin.material.name;
                result = (name.Contains("fected") || name.Contains("It"));
            }
            return result;
        }

        public static bool IsOtherPlayer(VRRig rig)
        {
            return rig != null && rig != GorillaTagger.Instance.offlineVRRig && !rig.isOfflineVRRig && !rig.isMyPlayer;
        }

        public static PhotonView rig2view(VRRig p) =>
            p.GetComponent<NetworkView>().GetView;

        public static Player NetPlayerToPlayer(NetPlayer np) =>
            np.GetPlayerRef();

        public static NetPlayer PlayerToNetPlayer(Player np)
        {
            foreach (NetPlayer p in NetworkSystem.Instance.AllNetPlayers)
            {
                if (np.UserId == p.UserId)
                    return p;
            }
            return null;
        }


        public static NetworkView GetNetViewFromVRRig(VRRig p) =>
            p.GetComponent<NetworkView>();

        public static VRRig GetRigFromPlayer(Player p) =>
            GorillaGameManager.instance.FindPlayerVRRig(p);

        public static PhotonView GetViewFromPlayer(Player p) =>
            rig2view(GorillaGameManager.instance.FindPlayerVRRig(p));

        public static VRRig GetOwnVRRig() =>
            GorillaTagger.Instance.offlineVRRig;

        public static PhotonView GetViewFromRig(VRRig rig) =>
            rig2view(rig);

        public static NetworkView GetNetViewFromRig(VRRig rig) =>
            rig2netview(rig);

        public static NetPlayer GetPlayerFromID(string id)
        {
            NetPlayer found = null;
            foreach (Player target in PhotonNetwork.PlayerList)
            {
                if (target.UserId == id)
                {
                    found = target;
                    break;
                }
            }
            return found;
        }

        public static NetworkView rig2netview(VRRig p)
        {
            return p.GetComponent<NetworkView>();
        }

        public static Player GetPlayerFromRig(VRRig rig)
        {
            return rig.OwningNetPlayer.GetPlayerRef();
        }

        public static NetPlayer GetNetPlayerFromRig(VRRig rig)
        {
            return rig.OwningNetPlayer;
        }

        private float Distance2D(Vector3 a, Vector3 b)
        {
            Vector2 a2 = new Vector2(a.x, a.z);
            Vector2 b2 = new Vector2(b.x, b.z);
            return Vector2.Distance(a2, b2);
        }

        private RaycastHit[] rayResults = new RaycastHit[1];
        private bool PlayerNear(VRRig rig, float dist, out float playerDist)
        {
            if (rig == null)
            {
                playerDist = float.PositiveInfinity;
                return false;
            }
            playerDist = Distance2D(rig.transform.position, transform.position);
            return playerDist < dist && Physics.RaycastNonAlloc(new Ray(transform.position, rig.transform.position - transform.position), rayResults, playerDist, UnityLayer.Default.ToLayerMask() | UnityLayer.GorillaObject.ToLayerMask()) <= 0;
        }



        public static bool battleIsOnCooldown(VRRig rig) =>
            rig.mainSkin.material.name.Contains("hit");

        public static Player GetRandomPlayer(bool includeSelf) =>
            includeSelf ?
            PhotonNetwork.PlayerList[Random.Range(0, PhotonNetwork.PlayerList.Length)] :
            PhotonNetwork.PlayerListOthers[Random.Range(0, PhotonNetwork.PlayerListOthers.Length)];

        public static NetPlayer GetRandomNetPlayer(bool includeSelf) =>
            includeSelf ?
            NetworkSystem.Instance.AllNetPlayers[Random.Range(0, NetworkSystem.Instance.AllNetPlayers.Length)] :
            NetworkSystem.Instance.PlayerListOthers[Random.Range(0, NetworkSystem.Instance.PlayerListOthers.Length)];

        public static VRRig GetRandomVRRig(bool includeSelf) =>
            GetRigFromPlayer(GetRandomPlayer(includeSelf));
    }
}