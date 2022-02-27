using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class SwitchCamera 
{
    static List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    static CinemachineVirtualCamera ActiveCamera = null;
    public static bool IsActiveCamera (CinemachineVirtualCamera camera)
    {
        return camera == ActiveCamera;
    }
    public static void CameraSwitch(CinemachineVirtualCamera camera)
    {
        camera.Priority = 10;
        ActiveCamera = camera;

        foreach (CinemachineVirtualCamera c in cameras)
        {
            if(c != camera)
            {
                c.Priority = 0;
            }
        }
    }
    public static void Rigister(CinemachineVirtualCamera camera)
    {
        cameras.Add(camera);
    }
    public static void UnRigister(CinemachineVirtualCamera camera)
    {
        cameras.Remove(camera);
    }
}
