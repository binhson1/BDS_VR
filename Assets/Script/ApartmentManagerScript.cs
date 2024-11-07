using System.Collections.Concurrent;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class ApartmentData
{
    public string id;
    public string idEmployee;
    public string block;
    public string floor;
    public string apartment;
    public string location;
    public string status;
    public string note;
    public string createdAt;
    public string updatedAt;
    public string deletedAt;
}

public class ApartmentManagerScript : MonoBehaviour
{
    private Dictionary<string, Color> statusColors = new Dictionary<string, Color>()
    {
        { "Đắt cọc", Color.yellow },
        { "Đã bán", Color.red },
        { "Chờ bán", Color.green },
    };

    // Queue to store apartment data updates
    private ConcurrentQueue<ApartmentData[]> apartmentDataQueue = new ConcurrentQueue<ApartmentData[]>();

    void Update()
    {
        // Process all queued apartment data updates
        while (apartmentDataQueue.TryDequeue(out ApartmentData[] apartments))
        {
            Debug.Log("OKEY");
            UpdateCubeColors(apartments);
        }
    }

    public void EnqueueApartmentData(ApartmentData[] apartments)
    {
        //Debug.Log("OKEY");
        apartmentDataQueue.Enqueue(apartments);
    }

    private void UpdateCubeColors(ApartmentData[] apartments)
    {
        Debug.Log("Updating cube colors based on apartment data.");
        foreach (var apartment in apartments)
        {
            if (statusColors.ContainsKey(apartment.status))
            {
                GameObject cube = GameObject.Find(apartment.id);
                if (cube != null)
                {
                    Debug.Log("OKEY");
                    MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
                    renderer.material.color = statusColors[apartment.status];
                }
                else
                {
                    //Debug.Log($"Cube with ID {apartment.id} not found.");
                }
            }
        }
    }
}
