//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.IF
{
    using System.Collections.Generic;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.Rendering;
    using UnityEngine.SceneManagement;

#if UNITY_EDITOR
    using Input = InstantPreviewInput;
#endif
    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class TowerBuildController : MonoBehaviour
    {
        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject TrackedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject defenseStation_Prefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// A list to hold new planes ARCore began tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<TrackedPlane> m_NewPlanes = new List<TrackedPlane>();

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<TrackedPlane> m_AllPlanes = new List<TrackedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;

        /// <summary>
        /// 20180403 SangBin : Singletone Pattern
        /// </summary>
        public static TowerBuildController TBController = null;

        /// <summary>
        /// 20180403 SangBin : Tower Base Tracing State
        /// </summary>
        private enum TowerBaseTrackingState { tracking, built };

        /// <summary>
        /// 20180403 SangBin : Tower Base Present Tracing State
        /// </summary>
        private TowerBaseTrackingState myTowerBaseTrackingState = TowerBaseTrackingState.tracking;

        /// <summary>
        /// 20180403 SangBin : Defense Station Anchor Transform
        /// </summary>
        private Transform defenseStation_Anchor_Tr;

        /// <summary>
        /// 20180403 SangBin : Defense Station Anchor Transform Property
        /// </summary>
        public Transform DefenseStation_Anchor_Tr { get { return defenseStation_Anchor_Tr; } }

        /// <summary>
        /// 20180418 SangBin : Defense Station Transform
        /// </summary>
        //private Transform defenseStation_Tr;

        /// <summary>
        /// 20180403 SangBin : Defense Station Transform Property
        /// </summary>
        //public Transform DefenseStation_Tr { get { return defenseStation_Tr; } }

        GameObject planeGridObject;

        /// <summary>
        /// 20180509 SangBin : Defense Station Ground Prefab
        /// </summary>
        [SerializeField]
        private GameObject groundPlane;


        //---------------------------------------------------------------------------------------------

        void Awake()
        {
            TBController = this;
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {

            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            _QuitOnConnectionErrors();

            if (myTowerBaseTrackingState == TowerBaseTrackingState.tracking)
            {
                // Check that motion tracking is tracking.
                if (Session.Status != SessionStatus.Tracking)
                {
                    const int lostTrackingSleepTimeout = 15;
                    Screen.sleepTimeout = lostTrackingSleepTimeout;
                    if (!m_IsQuitting && Session.Status.IsValid())
                    {
                        SearchingForPlaneUI.SetActive(true);
                    }

                    return;
                }

                Screen.sleepTimeout = SleepTimeout.NeverSleep;

                // Iterate over planes found in this frame and instantiate corresponding GameObjects to visualize them.
                Session.GetTrackables<TrackedPlane>(m_NewPlanes, TrackableQueryFilter.New);
                for (int i = 0; i < m_NewPlanes.Count; i++)
                {
                    // Instantiate a plane visualization prefab and set it to track the new plane. The transform is set to
                    // the origin with an identity rotation since the mesh for our prefab is updated in Unity World
                    // coordinates.
                    planeGridObject = Instantiate(TrackedPlanePrefab, Vector3.zero, Quaternion.identity,
                        transform);
                    planeGridObject.GetComponent<TrackedPlaneVisualizer>().Initialize(m_NewPlanes[i]); //아마 이 planeobject가 없어지면 설치후 그릴 없엘수 잇겟다
                }
                
                // Disable the snackbar UI when no planes are valid.
                Session.GetTrackables<TrackedPlane>(m_AllPlanes); //여기서 현재 추적되어 있는 그릴을 찾는군
                bool showSearchingUI = true;
                for (int i = 0; i < m_AllPlanes.Count; i++)
                {
                    if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                    {
                        showSearchingUI = false;
                        break;
                    }
                }

                SearchingForPlaneUI.SetActive(showSearchingUI);

                // If the player has not touched the screen, we are done with this update.
                Touch touch;
                if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
                {
                    return;
                }

                // Raycast against the location the player touched to search for planes.
                TrackableHit hit;
                TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                    TrackableHitFlags.FeaturePointWithSurfaceNormal;


                if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
                {
                    var defenseStation = Instantiate(defenseStation_Prefab, hit.Pose.position, hit.Pose.rotation);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    var defenseStation_Anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Andy should look at the camera but still be flush with the plane.
                    if ((hit.Flags & TrackableHitFlags.PlaneWithinPolygon) != TrackableHitFlags.None)
                    {
                        // Get the camera position and match the y-component with the hit position.
                        Vector3 cameraPositionSameY = FirstPersonCamera.transform.position;
                        cameraPositionSameY.y = hit.Pose.position.y;

                        // Have Andy look toward the camera respecting his "up" perspective, which may be from ceiling.
                        defenseStation.transform.LookAt(cameraPositionSameY, defenseStation.transform.up);
                    }

                    // Make Andy model a child of the anchor.
                    defenseStation.transform.parent = defenseStation_Anchor.transform;

                    myTowerBaseTrackingState = TowerBaseTrackingState.built;
                    //defenseStation_Tr = defenseStation.transform;
                    defenseStation_Anchor_Tr = defenseStation_Anchor.transform;

                    planeGridObject.GetComponent<MeshRenderer>().enabled = false;
                    planeGridObject.GetComponent<TrackedPlaneVisualizer>().enabled = false;
                    SceneManager.LoadScene("GameScene B",LoadSceneMode.Additive);
                    GameObject ground = Instantiate(groundPlane, defenseStation.transform.position, Quaternion.identity);
                }
            }
        }































        /// <summary>
        /// Quit the application if there was a connection error for the ARCore session.
        /// </summary>
        private void _QuitOnConnectionErrors()
        {
            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
    }
}
