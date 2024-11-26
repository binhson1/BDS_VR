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
    // Public materials for each status, set these in the Unity Inspector
    public Material reservedMaterial;  // "Đặt cọc"
    public Material soldMaterial;      // "Đã bán"
    public Material waitingMaterial;   // "Chờ bán"

    // Queue to store apartment data updates
    private ConcurrentQueue<ApartmentData[]> apartmentDataQueue = new ConcurrentQueue<ApartmentData[]>();

    private Dictionary<string, ApartmentData> apartmentDataDict = new Dictionary<string, ApartmentData>();

    void Update()
    {
        // Process queued data and update colors as before
        while (apartmentDataQueue.TryDequeue(out ApartmentData[] apartments))
        {
            foreach (var apartment in apartments)
            {
                apartmentDataDict[apartment.id] = apartment; // Update apartment data in dictionary
            }

            UpdateCubeMaterials(apartments);
        }
    }

    // Thêm phương thức để lấy căn hộ theo ID
    public ApartmentData GetApartmentById(string id)
    {
        // Return ApartmentData if it exists in the dictionary, otherwise return null
        if (apartmentDataDict.TryGetValue(id, out ApartmentData apartment))
        {
            return apartment;
        }
        return null;
    }

    public void EnqueueApartmentData(ApartmentData[] apartments)
    {
        apartmentDataQueue.Enqueue(apartments);
    }

    private void UpdateCubeMaterials(ApartmentData[] apartments)
    {
        Debug.Log("Updating cube materials based on apartment data.");
        foreach (var apartment in apartments)
        {
            Material materialToApply = null;

            // Determine which material to apply based on apartment status
            switch (apartment.status)
            {
                case "Đắt cọc":
                    materialToApply = reservedMaterial;
                    break;
                case "Đã bán":
                    materialToApply = soldMaterial;
                    break;
                case "Chờ bán":
                    materialToApply = waitingMaterial;
                    break;
            }

            if (materialToApply != null)
            {
                GameObject cube = GameObject.Find(apartment.id);
                if (cube != null)
                {
                    MeshRenderer renderer = cube.GetComponent<MeshRenderer>();
                    renderer.material = materialToApply; // Apply the selected material
                }
                else
                {
                    Debug.Log($"Cube with ID {apartment.id} not found.");
                }
            }
        }
    }
}
