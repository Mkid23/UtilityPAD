using GorillaLocomotion;
using Photon.Pun;
using TvvPancke.Classes;
using UnityEngine;
namespace PanckeUtilityPad.LegalMods

{
    internal class ModFunction
    {
        public static int flySpeedIndex = 2;
        public static float flySpeed = 15f;

        public static void ChangeFlySpeed()
        {
            string[] speedNames = new string[] { "Very Slow", "Slow", "Normal", "Fast", "Very Fast", "Extreme" };
            float[] speedValues = new float[] { 5f, 10f, 15f, 20f, 30f, 50f };

            flySpeedIndex++;
            flySpeedIndex %= speedNames.Length;

        }
        public static void Fly()
        {
            if (ControllerInputPoller.instance.rightControllerPrimaryButton)
            {
                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
            }
        }
        public static void speedboost()
        {
            {
                GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = 8.5f;
            }
        }
        public static void longarms()
        {
            {
                GorillaLocomotion.GTPlayer.Instance.maxArmLength = 8.5f;
            }
        }

        public static void speedboostRG()
        {
            if (ControllerInputPoller.instance.rightGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.maxJumpSpeed = 8.5f;
            }
        }
        public static void longarmsLG()
        {
            if (ControllerInputPoller.instance.leftGrab)
            {
                GorillaLocomotion.GTPlayer.Instance.maxArmLength = 8.5f;
            }
        }

        public static GameObject platL, platR;
        public static int platMode = 1;
        public static int platInput = 0;

        public static void Platforms()
        {
            bool grip = platInput == 0;

            void Handle(ref GameObject plat, Transform hand, bool input)
            {
                if (input)
                {
                    if (plat == null)
                    {
                        plat = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        plat.transform.localScale = new Vector3(0.025f, 0.3f, 0.4f);
                        plat.transform.position = hand.position;
                        plat.transform.rotation = hand.rotation;
                        plat.GetComponent<Renderer>().material.shader = Shader.Find("Sprites/Default");
                    }

                    Renderer r = plat.GetComponent<Renderer>();
                    if (platMode == 0) r.enabled = false;
                    else
                    {
                        r.enabled = true;
                        r.material.color = new Color(0.13f, 0.55f, 0.13f);
                    }
                }
                else if (plat != null)
                {
                    UnityEngine.Object.Destroy(plat);
                    plat = null;
                }
            }

            Handle(ref platL, GorillaTagger.Instance.leftHandTransform, (ControllerInputPoller.instance.rightGrab));
            Handle(ref platR, GorillaTagger.Instance.rightHandTransform, (ControllerInputPoller.instance.leftGrab));
        }
        public static Vector3? checkpointPos = null;
        public static GameObject orb = null;
        public static void Checkpoint()
        {
            if (ControllerInputPoller.instance.rightControllerTriggerButton)
            {
                checkpointPos = GorillaTagger.Instance.rightHandTransform.transform.position;
                if (orb == null)
                {
                    orb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    orb.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    orb.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");
                    UnityEngine.Object.Destroy(orb.GetComponent<SphereCollider>());
                }
                orb.transform.position = checkpointPos.Value;
            }

            if (orb != null)
                orb.GetComponent<Renderer>().material.color = new Color(0.13f, 0.55f, 0.13f);

            if (ControllerInputPoller.instance.rightControllerSecondaryButton && checkpointPos != null)
                TeleportPlayer(checkpointPos.Value);
        }
        public static void TeleportPlayer(Vector3 pos)
        {
            GTPlayer.Instance.TeleportTo(World2Player(pos), GTPlayer.Instance.transform.rotation, false);
        }
        public static Vector3 World2Player(Vector3 world)
        {
            return world - GorillaTagger.Instance.bodyCollider.transform.position + GorillaTagger.Instance.transform.position;
        }

        public static void DestroyCheckpoint()
        {
            UnityEngine.Object.Destroy(orb);
            orb = null;
            checkpointPos = null;
        }
        public static float wallAssistAmount = 0.56f;

        public static void WallAssist()
        {

            if (GTPlayer.Instance.RightHand.wasColliding)
            {
                GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity += -GTPlayer.Instance.RightHand.controllerTransform.up * wallAssistAmount;
                RaycastHit raycastHit;
                Physics.Raycast(GTPlayer.Instance.RightHand.controllerTransform.position, -GTPlayer.Instance.RightHand.controllerTransform.up, out raycastHit);
            }

            if (GTPlayer.Instance.LeftHand.wasColliding)
            {
                GTPlayer.Instance.GetComponent<Rigidbody>().linearVelocity += -GTPlayer.Instance.LeftHand.controllerTransform.up * wallAssistAmount;
                RaycastHit raycastHit2;
                Physics.Raycast(GTPlayer.Instance.LeftHand.controllerTransform.position, -GTPlayer.Instance.LeftHand.controllerTransform.up, out raycastHit2);
            }

        }

        private static int wallForceIndex = 2;
        private static float activeWallForce = 9.81f;
        private static readonly float[] wallForces = { 2f, 5f, 9.81f, 15f, 50f };
        private static readonly string[] wallForceNames = { "Feeble", "Soft", "Default", "Firm", "Intense" };
        private static Vector3 currentWallNormal = Vector3.up;
        private static bool contactSaved = false;

        public static void AdjustWallWalkStrength(bool increase)
        {
            if (increase)
                wallForceIndex = (wallForceIndex + 1) % wallForces.Length;
            else
                wallForceIndex = (wallForceIndex - 1 + wallForces.Length) % wallForces.Length;

            activeWallForce = wallForces[wallForceIndex];
        }

        public static void WallWalk()
        {
            bool grabInput = ControllerInputPoller.instance.rightControllerTriggerButton || ControllerInputPoller.instance.leftControllerTriggerButton;

            if (GTPlayer.Instance.RightHand.wasColliding || GTPlayer.Instance.LeftHand.wasColliding)
            {
                Transform hand = GTPlayer.Instance.RightHand.wasColliding ? GTPlayer.Instance.RightHand.controllerTransform : GTPlayer.Instance.LeftHand.controllerTransform;
                if (Physics.Raycast(hand.position, -hand.up, out RaycastHit hit, 0.5f) ||
                    Physics.Raycast(hand.position, hand.forward, out hit, 0.5f))
                {
                    currentWallNormal = hit.normal;
                    contactSaved = true;
                }
            }

            if (!grabInput)
                contactSaved = false;

            if (contactSaved && grabInput)
            {
                GorillaTagger.Instance.rigidbody.AddForce(-currentWallNormal * activeWallForce, ForceMode.Acceleration);
                ZeroGravity();
            }
        }

        public static void LegitimateWallWalk()
        {
            float maxRange = 0.25f;
            float legitPull = 2.5f;

            if (ControllerInputPoller.instance.leftControllerTriggerButton)
            {
                Transform leftHand = GTPlayer.Instance.LeftHand.controllerTransform;
                if (Physics.Raycast(leftHand.position, -leftHand.up, out RaycastHit hitL, maxRange))
                {
                    GorillaTagger.Instance.rigidbody.AddForce(-hitL.normal * legitPull, ForceMode.Acceleration);
                }
            }

            if (ControllerInputPoller.instance.rightControllerTriggerButton)
            {
                Transform rightHand = GTPlayer.Instance.RightHand.controllerTransform;
                if (Physics.Raycast(rightHand.position, -rightHand.up, out RaycastHit hitR, maxRange))
                {
                    GorillaTagger.Instance.rigidbody.AddForce(-hitR.normal * legitPull, ForceMode.Acceleration);
                }
            }
        }
        public static void NoclipFly()
        {

            if (ControllerInputPoller.instance.leftControllerTriggerButton)
            {

                GTPlayer.Instance.transform.position += GorillaTagger.Instance.headCollider.transform.forward * Time.deltaTime * flySpeed;
                GorillaTagger.Instance.rigidbody.linearVelocity = Vector3.zero;
                foreach (MeshCollider v in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    v.enabled = false;
            }
            else
            {
                foreach (MeshCollider v in Resources.FindObjectsOfTypeAll<MeshCollider>())
                    v.enabled = true;
            }
        }
        public static void ZeroGravity()
        {
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * 9.81f, ForceMode.Acceleration);
        }
        public static void UpAndDown()
        {

            if (ControllerInputPoller.instance.rightGrab)
                GorillaTagger.Instance.rigidbody.AddForce(Vector3.up * 6.66f, ForceMode.Acceleration);

            if (ControllerInputPoller.instance.leftGrab)
                GorillaTagger.Instance.rigidbody.AddForce(Vector3.down * 6.66f, ForceMode.Acceleration);
        }
        public static void LeftandRight()
        {

            if (ControllerInputPoller.instance.rightGrab)
                GorillaTagger.Instance.rigidbody.AddForce(Vector3.right * 6.66f, ForceMode.Acceleration);

            if (ControllerInputPoller.instance.leftGrab)
                GorillaTagger.Instance.rigidbody.AddForce(Vector3.left * 6.66f, ForceMode.Acceleration);
        }
        public static void BF()
        {

            if (ControllerInputPoller.instance.rightGrab)
                GorillaTagger.Instance.rigidbody.AddForce(Vector3.forward * 6.66f, ForceMode.Acceleration);

            if (ControllerInputPoller.instance.leftGrab)
                GorillaTagger.Instance.rigidbody.AddForce(Vector3.back * 6.66f, ForceMode.Acceleration);
        }

        public static void LowGravity()
        {
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.up * 6.66f, ForceMode.Acceleration);
        }

        public static void HighGravity()
        {
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.down * 7.77f, ForceMode.Acceleration);
        }
        public static void ToHighGravity()
        {
            GTPlayer.Instance.bodyCollider.attachedRigidbody.AddForce(Vector3.down * 7000.77f, ForceMode.Acceleration);
        }

        public static void RPCProtection()
        {
            if (!PhotonNetwork.InRoom)
                return;

            try
            {
                MonkeAgent.instance.rpcErrorMax = int.MaxValue;
                MonkeAgent.instance.rpcCallLimit = int.MaxValue;
                MonkeAgent.instance.logErrorMax = int.MaxValue;

                PhotonNetwork.MaxResendsBeforeDisconnect = int.MaxValue;
                PhotonNetwork.QuickResends = int.MaxValue;

                PhotonNetwork.SendAllOutgoingCommands();
            }
            catch { }
        }
        internal class ModKeybindManager : MonoBehaviour
        {
            // Toggle states for every mod
            public static bool fly;
            public static bool speedBoost;
            public static bool longArms;
            public static bool platforms;
            public static bool checkpoint;
            public static bool noclip;
            public static bool wallWalk;
            public static bool wallAssist;
            public static bool upDown;
            public static bool leftRight;
            public static bool bf;
            public static bool lowGravity;
            public static bool highGravity;
            public static bool rpcProtection;

            void Update()
            {
                // -------------------------
                // KEYBIND TOGGLES
                // -------------------------
                if (Input.GetKeyDown(KeyCode.F)) fly = !fly;
                if (Input.GetKeyDown(KeyCode.G)) speedBoost = !speedBoost;
                if (Input.GetKeyDown(KeyCode.H)) longArms = !longArms;
                if (Input.GetKeyDown(KeyCode.J)) platforms = !platforms;
                if (Input.GetKeyDown(KeyCode.K)) checkpoint = !checkpoint;
                if (Input.GetKeyDown(KeyCode.N)) noclip = !noclip;
                if (Input.GetKeyDown(KeyCode.M)) wallWalk = !wallWalk;
                if (Input.GetKeyDown(KeyCode.B)) wallAssist = !wallAssist;
                if (Input.GetKeyDown(KeyCode.U)) upDown = !upDown;
                if (Input.GetKeyDown(KeyCode.L)) leftRight = !leftRight;
                if (Input.GetKeyDown(KeyCode.V)) bf = !bf;
                if (Input.GetKeyDown(KeyCode.Z)) lowGravity = !lowGravity;
                if (Input.GetKeyDown(KeyCode.X)) highGravity = !highGravity;
                if (Input.GetKeyDown(KeyCode.P)) rpcProtection = !rpcProtection;

                // -------------------------
                // ACTIVE MOD EXECUTION
                // -------------------------
                if (fly) ModFunction.Fly();
                if (speedBoost) ModFunction.speedboost();
                if (longArms) ModFunction.longarms();
                if (platforms) ModFunction.Platforms();
                if (checkpoint) ModFunction.Checkpoint();
                if (noclip) ModFunction.NoclipFly();
                if (wallWalk) ModFunction.WallWalk();
                if (wallAssist) ModFunction.WallAssist();
                if (upDown) ModFunction.UpAndDown();
                if (leftRight) ModFunction.LeftandRight();
                if (bf) ModFunction.BF();
                if (lowGravity) ModFunction.LowGravity();
                if (highGravity) ModFunction.HighGravity();
                if (rpcProtection) ModFunction.RPCProtection();
            }
        }
    }

}

