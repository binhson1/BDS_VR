using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectActivator : MonoBehaviour
{
    public List<GameObject> objects;

    private Dictionary<GameObject, Vector3> originalScales = new Dictionary<GameObject, Vector3>();
    public GameObject passThroughCamera;
    public Camera cam;
    private bool flag = true;
    public GameObject next;
    public GameObject previous;
    public GameObject exit;
   public GameObject button;
    private void Start()
    {
        // Lưu lại scale ban đầu của tất cả các đối tượng trong danh sách
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                originalScales[obj] = obj.transform.localScale;
            }
        }
    }
    
    // Hàm để đặt scale của tất cả các đối tượng về (0, 0, 0)
    public void ScaleDownObjects()
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null)
            {
                obj.transform.localScale = Vector3.zero;
            }
        }
        TriggerChangingScene();
    }

    // Hàm để khôi phục scale ban đầu của tất cả các đối tượng
    public void ResetScaleObjects()
    {
        foreach (GameObject obj in objects)
        {
            if (obj != null && originalScales.ContainsKey(obj))
            {
                obj.transform.localScale = originalScales[obj];
            }
        }
        TriggerChangingScene();
      
    }
    
    public void TriggerChangingScene()
    {
        if (flag == true)
        {
            passThroughCamera.SetActive(false);
            cam.clearFlags = CameraClearFlags.Skybox;
            flag = false;
            next.SetActive(true);
            previous.SetActive(true);
            exit.SetActive(true);
            button.SetActive(false);
            
        }
        else
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            passThroughCamera.SetActive(true);
            cam.backgroundColor = new Color(0, 0, 0, 0);
            flag = true;
            next.SetActive(false);
            previous.SetActive(false);
            exit.SetActive(false);
           button.SetActive(true);
        }

    }
}
