using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnLoad : MonoBehaviour
{
    public GameObject building;

    public void Disable()
    {
        building.SetActive(false);
    }

    public void Enable()
    {
        building.SetActive(true);
    }
}
