using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    public Color outlineColor = Color.black; // Màu đường viền
    public float outlineThickness = 0.05f;  // Độ dày đường viền

    private GameObject outlineObject;

    void Start()
    {
        // 1. Tạo object đường viền
        CreateOutline();
    }

    private void CreateOutline()
    {
        // Tạo bản sao của object gốc
        outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation);
        outlineObject.transform.localScale = transform.localScale + Vector3.one * outlineThickness;

        // Loại bỏ script OutlineObject trên object đường viền
        Destroy(outlineObject.GetComponent<OutlineObject>());

        // Đổi màu material của object đường viền
        Renderer renderer = outlineObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material outlineMaterial = new Material(Shader.Find("Standard"));
            outlineMaterial.color = outlineColor;
            renderer.material = outlineMaterial;
        }

        // Đảo normals để đường viền hiển thị đúng
        MeshFilter meshFilter = outlineObject.GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            ReverseNormals(meshFilter.mesh);
        }

        // Đặt object đường viền là con của object gốc
        outlineObject.transform.SetParent(this.transform);
    }

    private void ReverseNormals(Mesh mesh)
    {
        int[] triangles = mesh.triangles;
        Vector3[] normals = mesh.normals;

        // Đảo hướng các mặt
        for (int i = 0; i < triangles.Length; i += 3)
        {
            int temp = triangles[i];
            triangles[i] = triangles[i + 1];
            triangles[i + 1] = temp;
        }

        // Đảo normals
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }

        // Cập nhật lại mesh
        mesh.triangles = triangles;
        mesh.normals = normals;
    }
}
