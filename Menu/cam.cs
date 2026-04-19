using UnityEngine;

namespace urukpanel.Menu
{
    internal class CameraController : MonoBehaviour
    {
        public static CameraController instance;

        private Camera cam;
        private Transform playerHead;

        private static bool thirdPerson = false;
        private static float fov = 70f; // default FOV
        private static Vector3 thirdPersonOffset = new Vector3(0, 0.3f, -1.5f);

        void Awake()
        {
            instance = this;
            //cam = GorillaTagger.Instance.mainCamera;
            playerHead = GorillaTagger.Instance.headCollider.transform;
        }

        void LateUpdate()
        {
            if (!thirdPerson)
            {
                // FIRST PERSON
                cam.transform.position = playerHead.position;
                cam.transform.rotation = playerHead.rotation;
            }
            else
            {
                // THIRD PERSON
                cam.transform.position = playerHead.position +
                                         playerHead.transform.TransformDirection(thirdPersonOffset);

                cam.transform.LookAt(playerHead);
            }

            // Apply FOV
            cam.fieldOfView = fov;
        }

        // Toggle third person
        public static void ToggleThirdPerson()
        {
            thirdPerson = !thirdPerson;
            Console.Console.SendNotification(thirdPerson ? "Third Person Enabled" : "First Person Enabled");
        }

        // Increase FOV
        public static void IncreaseFOV()
        {
            fov += 5f;
            if (fov > 120f) fov = 120f;
            Console.Console.SendNotification($"FOV: {fov}");
        }

        // Decrease FOV
        public static void DecreaseFOV()
        {
            fov -= 5f;
            if (fov < 40f) fov = 40f;
            Console.Console.SendNotification($"FOV: {fov}");
        }
    }
}
