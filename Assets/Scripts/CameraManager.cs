using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private List<CameraObject> cameras;

    public List<CameraObject> Cameras => cameras;

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
        oldCamera.SetActive(false);
        newCamera.SetActive(true);
        return newCamera;
    }
}