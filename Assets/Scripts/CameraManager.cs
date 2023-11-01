using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private List<CameraController> allCameras;
    
    [SerializeField]
    private List<CameraObject> cameraObjects;


    public List<CameraController> AllCameras => allCameras;

    public List<CameraObject> CameraObjects => cameraObjects;

    private void Awake()
    {
        allCameras = new List<CameraController>(FindObjectsByType<CameraController>(FindObjectsSortMode.None));
        cameraObjects = new List<CameraObject>(GetComponentsInChildren<CameraObject>());
    }

    // Start is called before the first frame update
    void Start()
    {
        if (allCameras.Count > 0)
        {
            allCameras[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < allCameras.Count - 1; i++)
        {
            Gizmos.DrawLine(allCameras[i].Position, allCameras[i + 1].Position);
        }

        if (allCameras.Count > 1)
        {
            Gizmos.DrawLine(allCameras[0].Position, allCameras[^1].Position);
        }
    }

    public CameraController GetNextCamera(CameraController currentCamera)
    {
        currentCamera.SetActive(false);
        currentCamera = allCameras[(allCameras.FindLastIndex(a => a.Equals(currentCamera)) + 1) % allCameras.Count];
        currentCamera.SetActive(true);

        return currentCamera;
    }

    public CameraController GetPrevCamera(CameraController currentCamera)
    {
        currentCamera.SetActive(false);
        currentCamera =
            allCameras[(allCameras.FindLastIndex(a => a.Equals(currentCamera)) - 1 + allCameras.Count) % allCameras.Count];
        currentCamera.SetActive(true);
        return currentCamera;
    }

    /// <summary>
    /// Change Cameras
    /// </summary>
    /// <param name="newCamera"></param>
    /// <param name="oldCamera"></param>
    /// <returns></returns>
    public CameraController ChangeCamera(CameraController newCamera, CameraController oldCamera)
    {
        // if (newCamera.IsLocked)
        // {
        //     return oldCamera;
        // }
        oldCamera.SetActive(false);
        newCamera.SetActive(true);
        GlobalKnowledgeSystem.UpdatePlayerCamera(newCamera);
        return newCamera;
    }

    public void ActiveThroughWalls(Vector3 position, float range)
    {
        List<CameraObject> temp = new List<CameraObject>();
        foreach (CameraObject cameraObject in cameraObjects)
        {
            if (Vector3.Distance(cameraObject.Position, position) < range)
            {
                temp.Add(cameraObject);
            }
        }

        StartCoroutine(ThroughWallAnimation(temp.OrderBy((d) => (Vector3.Distance(d.Position, position))).ToArray()));
    }

    private IEnumerator ThroughWallAnimation(CameraObject[] cameras)
    {
        foreach (CameraObject cameraObject in cameraObjects)
        {
            cameraObject.CameraController.ThroughWallEffect_Activate();
            yield return new WaitForSeconds(.1f);
        }
    }
}