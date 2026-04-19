using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using BepInEx;
using static Colorlib.ColorLib;

namespace Gunlib
{
   

    public class Gunlib
    {
        /// <summary>
        /// Checks if a controller grab button is pressed enough (75% threshold)
        /// </summary>
        /// <param name="grabValue">The grab input value (0-1)</param>
        /// <returns>True if grab value is 75% or more</returns>
        public static bool GetInputValue(float grabValue)
        {
            return grabValue >= 0.75f;
        }
    }

    // MAIN GUN SYSTEM CLASS

    public class GunTemplate : MonoBehaviour
    {
        #region ========== VISUAL SETTINGS (Safe to modify) ==========

        // Line Settings
        public static int LineCurve = 1500;                      // Number of points in curved line (higher = smoother)
        private const float LineWidth = 0.025f;                 // Thickness of the line
        private const float LineSmoothFactor = 6f;              // How smoothly line follows movement

        // Pointer Settings
        private const float PointerScale = 0.15f;               // Size of the pointer sphere
        private const float PulseSpeed = 2f;                    // Speed of pointer pulsing animation
        private const float PulseAmplitude = 0.03f;             // How much pointer grows/shrinks

        // Timing
        private const float DestroyDelay = 0.02f;               // How long before line segments are destroyed

        // Colors
        public static Color32 PointerColor = SkyBlue;           // Pointer color when aiming
        public static Color32 TriggeredPointerColor = SkyBlue;           // Pointer color when trigger pressed
        public static Color32 LineColor = SkyBlue;              // Line color when aiming  
        public static Color32 TriggeredLineColor = Black;     // Line color when trigger pressed

        #endregion

        #region ========== STATE VARIABLES (Don't modify directly) ==========

        // Gun Objects
        public static GameObject spherepointer;     // The visible pointer sphere
        public static VRRig LockedPlayer;          // Currently locked-on player (if any)
        public static Vector3 lr;                  // Lerp position for smooth line movement

        // Input State
        public static bool trigger = false;        // Whether trigger is currently pressed
        public static RaycastHit raycastHit;       // Result of last raycast

        #endregion

        #region ========== MATH FUNCTIONS - Bezier Curve Calculations ==========

        /// <summary>
        /// Calculate a point on a quadratic Bezier curve
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="mid">Control point (determines curve shape)</param>
        /// <param name="end">End point</param>
        /// <param name="t">Position along curve (0 to 1)</param>
        /// <returns>Calculated point position</returns>
        private static Vector3 CalculateBezierPoint(Vector3 start, Vector3 mid, Vector3 end, float t)
        {
            return Mathf.Pow(1 - t, 2) * start + 2 * (1 - t) * t * mid + Mathf.Pow(t, 2) * end;
        }

        /// <summary>
        /// Apply Bezier curve to a LineRenderer
        /// </summary>
        private static void CurveLineRenderer(LineRenderer lineRenderer, Vector3 start, Vector3 mid, Vector3 end)
        {
            lineRenderer.positionCount = LineCurve;
            for (int i = 0; i < LineCurve; i++)
            {
                float t = (float)i / (LineCurve - 1);
                lineRenderer.SetPosition(i, CalculateBezierPoint(start, mid, end, t));
            }
        }

        #endregion

        #region ========== ANIMATION COROUTINES ==========

        /// <summary>
        /// Continuously updates the curved line renderer with smooth animation
        /// </summary>
        private static IEnumerator StartCurvyLineRenderer(LineRenderer lineRenderer, Vector3 start, Vector3 mid, Vector3 end)
        {
            while (true)
            {
                // Update curve shape
                CurveLineRenderer(lineRenderer, start, mid, end);

                // Animate line opacity
                lineRenderer.startColor = Color.Lerp(
                    lineRenderer.startColor,
                    new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, Mathf.PingPong(Time.time, 1)),
                    0.5f
                );
                lineRenderer.endColor = lineRenderer.startColor;

                yield return null;
            }
        }

        /// <summary>
        /// Makes the pointer sphere pulse in and out smoothly
        /// </summary>
        private static IEnumerator PulsePointer(GameObject pointer)
        {
            Vector3 originalScale = pointer.transform.localScale;

            while (true)
            {
                float scaleFactor = 1 + Mathf.Sin(Time.time * PulseSpeed) * PulseAmplitude;
                pointer.transform.localScale = originalScale * scaleFactor;
                yield return null;
            }
        }

        #endregion

        #region ========== VR GUN SYSTEM ==========

        /// <summary>
        /// Main VR gun system - call this in your Update loop
        /// </summary>
        /// <param name="action">Function to call when trigger is pressed</param>
        /// <param name="LockOn">Whether to lock onto players when aiming at them</param>
        public static void StartVrGun(Action action, bool LockOn)
        {
            // Check if right grab button is held
            if (ControllerInputPoller.instance.rightGrab)
            {
                // Cast ray from right hand downward
                Physics.Raycast(
                    GorillaTagger.Instance.rightHandTransform.position,
                    -GorillaTagger.Instance.rightHandTransform.up,
                    out raycastHit,
                    float.MaxValue
                );

                // Create pointer sphere if it doesn't exist
                if (spherepointer == null)
                {
                    CreatePointerSphere();
                }

                // Update pointer position
                UpdatePointerPosition();

                // Smooth lerp for line midpoint
                lr = Vector3.Lerp(
                    lr,
                    (GorillaTagger.Instance.rightHandTransform.position + spherepointer.transform.position) / 2f,
                    Time.deltaTime * LineSmoothFactor
                );

                // Create and configure line renderer
                CreateGunLine();

                // Handle trigger press
                if (ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f)
                {
                    HandleTriggerPress(action, LockOn);
                    return;
                }
                else if (LockedPlayer != null)
                {
                    // Release lock when trigger released
                    LockedPlayer = null;
                    return;
                }
            }
            else if (spherepointer != null)
            {
                // Clean up when grab released
                DestroyGunObjects();
            }
        }

        #endregion

        #region ========== PC GUN SYSTEM ==========

        /// <summary>
        /// PC gun system using mouse - call this in your Update loop
        /// </summary>
        /// <param name="action">Function to call when left mouse clicked</param>
        /// <param name="LockOn">Whether to lock onto players when aiming at them</param>
        public static void StartPcGun(Action action, bool LockOn)
        {
            // Get camera ray from mouse position
            Ray ray = GameObject.Find("Shoulder Camera").activeSelf
                ? GameObject.Find("Shoulder Camera").GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition)
                : GorillaTagger.Instance.mainCamera.GetComponent<Camera>().ScreenPointToRay(UnityInput.Current.mousePosition);

            // Check if right mouse button is held
            if (Mouse.current.rightButton.isPressed)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(ray.origin, ray.direction, out raycastHit, float.PositiveInfinity, -32777) && spherepointer == null)
                {
                    // Create pointer sphere if it doesn't exist
                    if (spherepointer == null)
                    {
                        CreatePointerSphere();
                    }
                }

                // Update pointer position
                if (LockedPlayer == null)
                {
                    spherepointer.transform.position = raycastHit.point;
                    spherepointer.GetComponent<Renderer>().material.color = PointerColor;
                }
                else
                {
                    spherepointer.transform.position = LockedPlayer.transform.position;
                }

                // Smooth lerp for line midpoint
                lr = Vector3.Lerp(
                    lr,
                    (GorillaTagger.Instance.rightHandTransform.position + spherepointer.transform.position) / 2f,
                    Time.deltaTime * LineSmoothFactor
                );

                // Create and configure line renderer
                CreateGunLine();

                // Handle left mouse click (trigger)
                if (Mouse.current.leftButton.isPressed)
                {
                    HandlePcTriggerPress(action, LockOn, raycastHit);
                    return;
                }
                else if (LockedPlayer != null)
                {
                    // Release lock when trigger released
                    LockedPlayer = null;
                    return;
                }
            }
            else if (spherepointer != null)
            {
                // Clean up when right mouse released
                DestroyGunObjects();
            }
        }

        #endregion

        #region ========== UNIFIED GUN SYSTEM ==========

        /// <summary>
        /// Automatically detects VR or PC mode and uses appropriate gun system
        /// </summary>
        /// <param name="action">Function to call when trigger pressed</param>
        /// <param name="locko">Whether to enable player lock-on</param>
        public static void StartBothGuns(Action action, bool locko)
        {
            if (XRSettings.isDeviceActive)
            {
                StartVrGun(action, locko);
            }
            else
            {
                StartPcGun(action, locko);
            }
        }

        #endregion

        #region ========== HELPER FUNCTIONS - Object Creation & Management ==========

        /// <summary>
        /// Creates the pointer sphere with all necessary components
        /// </summary>
        private static void CreatePointerSphere()
        {
            spherepointer = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spherepointer.AddComponent<Renderer>();
            spherepointer.transform.localScale = new Vector3(0.040f, 0.040f, 0.040f);
            spherepointer.GetComponent<Renderer>().material.shader = Shader.Find("GUI/Text Shader");

            // Remove physics components
            GameObject.Destroy(spherepointer.GetComponent<BoxCollider>());
            GameObject.Destroy(spherepointer.GetComponent<Rigidbody>());
            GameObject.Destroy(spherepointer.GetComponent<Collider>());

            // Initialize lerp position
            lr = GorillaTagger.Instance.offlineVRRig.rightHandTransform.position;

            // Start pulsing animation
            spherepointer.AddComponent<GunTemplate>().StartCoroutine(PulsePointer(spherepointer));
        }

        /// <summary>
        /// Updates pointer position based on lock-on state
        /// </summary>
        private static void UpdatePointerPosition()
        {
            if (LockedPlayer == null)
            {
                spherepointer.transform.position = raycastHit.point;
                spherepointer.GetComponent<Renderer>().material.color = PointerColor;
            }
            else
            {
                spherepointer.transform.position = LockedPlayer.transform.position;
            }
        }

        /// <summary>
        /// Creates the visual line connecting hand to pointer
        /// </summary>
        private static void CreateGunLine()
        {
            GameObject gameObject = new GameObject("Line");
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            lineRenderer.startColor = LineColor;
            lineRenderer.endColor = LineColor;
            lineRenderer.useWorldSpace = true;
            lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));

            // Start curve animation
            gameObject.AddComponent<GunTemplate>().StartCoroutine(
                StartCurvyLineRenderer(
                    lineRenderer,
                    GorillaTagger.Instance.rightHandTransform.position,
                    lr,
                    spherepointer.transform.position
                )
            );

            // Auto-destroy after frame
            GameObject.Destroy(lineRenderer, Time.deltaTime);
        }

        /// <summary>
        /// Handles VR trigger press logic
        /// </summary>
        private static void HandleTriggerPress(Action action, bool LockOn)
        {
            trigger = true;

            
            GameObject gameObject = new GameObject("Line");
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            lineRenderer.startColor = LineColor;
            lineRenderer.endColor = LineColor;
            lineRenderer.useWorldSpace = true;
            lineRenderer.material = new Material(Shader.Find("GUI/Text Shader"));

            spherepointer.GetComponent<Renderer>().material.color = PointerColor;

            if (LockOn)
            {
                // Lock onto player if not already locked
                if (LockedPlayer == null)
                {
                    LockedPlayer = raycastHit.collider.GetComponentInParent<VRRig>();
                }

                // Execute action if locked onto valid player
                if (LockedPlayer != null)
                {
                    spherepointer.transform.position = LockedPlayer.transform.position;
                    action();
                }
                return;
            }

            // Execute action without lock-on
            action();
        }

        /// <summary>
        /// Handles PC trigger press logic
        /// </summary>
        private static void HandlePcTriggerPress(Action action, bool LockOn, RaycastHit hit)
        {
            trigger = true;

            // Change colors when triggered
            GameObject gameObject = new GameObject("Line");
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startWidth = LineWidth;
            lineRenderer.endWidth = LineWidth;
            lineRenderer.startColor = TriggeredLineColor;
            lineRenderer.endColor = TriggeredLineColor;

            spherepointer.GetComponent<Renderer>().material.color = PointerColor;

            if (LockOn)
            {
                // Lock onto player if not already locked
                if (LockedPlayer == null)
                {
                    LockedPlayer = hit.collider.GetComponentInParent<VRRig>();
                }

                // Execute action if locked onto valid player
                if (LockedPlayer != null)
                {
                    spherepointer.transform.position = LockedPlayer.transform.position;
                    action();
                }
                return;
            }

            // Execute action without lock-on
            action();
        }

        /// <summary>
        /// Cleans up all gun-related objects
        /// </summary>
        private static void DestroyGunObjects()
        {
            GameObject.Destroy(spherepointer);
            spherepointer = null;
            LockedPlayer = null;
        }

        #endregion
    }
}