using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buying : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu;

    public void Buy()
    {
        menu.SetActive(true);
    }

    public void Cancel()
    {
        menu.SetActive(false);
    }
}
