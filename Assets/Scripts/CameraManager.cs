using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private List<CameraObject> cameras;

    public List<CameraObject> Cameras => cameras;

    private void Awake()
    {
        cameras = new List<CameraObject>(GetComponentsInChildren<CameraObject>());
    }

    // Start is called before the first frame update
    void Start()
    {
        if (cameras.Count > 0)
        {
            cameras[0].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < cameras.Count - 1; i++)
        {
            Gizmos.DrawLine(cameras[i].Position, cameras[i + 1].Position);
        }

        if (cameras.Count > 1)
        {
            Gizmos.DrawLine(cameras[0].Position, cameras[^1].Position);
        }
    }

    public CameraObject GetNextCamera(CameraObject currentCamera)
    {
        currentCamera.SetActive(false);
        currentCamera = cameras[(cameras.FindLastIndex(a => a.Equals(currentCamera)) + 1) % cameras.Count];
        currentCamera.SetActive(true);

        return currentCamera;
    }

    public CameraObject GetPrevCamera(CameraObject currentCamera)
    {
        currentCamera.SetActive(false);
        currentCamera =
            cameras[(cameras.FindLastIndex(a => a.Equals(currentCamera)) - 1 + cameras.Count) % cameras.Count];
        currentCamera.SetActive(true);
        return currentCamera;
    }

    /// <summary>
    /// Change Cameras
    /// </summary>
    /// <param name="newCamera"></param>
    /// <param name="oldCamera"></param>
    /// <returns></returns>
    public CameraObject ChangeCamera(CameraObject newCamera, CameraObject oldCamera)
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
        foreach (CameraObject cameraObject in cameras)
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
        foreach (CameraObject cameraObject in this.cameras)
        {
            cameraObject.ThroughWallEffect_Activate();
            yield return new WaitForSeconds(.1f);
        }
    }
}