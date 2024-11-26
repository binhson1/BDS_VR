using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    // Danh sách các Material dùng làm Skybox
    public List<Material> skyboxMaterials;

    // Chỉ số hiện tại của Skybox
    private int currentSkyboxIndex = 0;

    // Hàm để chuyển sang Skybox tiếp theo
    public void NextSkybox()
    {
        if (skyboxMaterials == null || skyboxMaterials.Count == 0)
        {
            Debug.LogWarning("Danh sách Skybox trống hoặc chưa được gán!");
            return;
        }

        // Tăng chỉ số Skybox
        currentSkyboxIndex = (currentSkyboxIndex + 1) % skyboxMaterials.Count;

        // Gán Skybox mới
        RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex];

        // Làm mới Skybox để hiển thị thay đổi (chỉ cần nếu sử dụng procedural skybox)
        DynamicGI.UpdateEnvironment();
    }
    
    public void PreviousSkybox()
    {
        if (skyboxMaterials == null || skyboxMaterials.Count == 0)
        {
            Debug.LogWarning("Danh sách Skybox trống hoặc chưa được gán!");
            return;
        }

        // Giảm chỉ số Skybox
        currentSkyboxIndex = (currentSkyboxIndex - 1 + skyboxMaterials.Count) % skyboxMaterials.Count;

        // Gán Skybox mới
        RenderSettings.skybox = skyboxMaterials[currentSkyboxIndex];

        // Làm mới Skybox
        DynamicGI.UpdateEnvironment();
    }

}
