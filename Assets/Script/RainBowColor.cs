using UnityEngine;

public class RainbowColor : MonoBehaviour
{
    public Material targetMaterial; // Material cần thay đổi màu sắc
    public float colorChangeSpeed = 1.0f; // Tốc độ thay đổi màu

    private float hue; // Giá trị hue trong HSV

    void Update()
    {
        // Tăng giá trị hue theo thời gian và sử dụng hàm Mathf.Repeat để giữ giá trị trong khoảng 0-1
        hue += Time.deltaTime * colorChangeSpeed;
        hue = Mathf.Repeat(hue, 1.0f);

        // Chuyển đổi từ HSV sang RGB
        Color rainbowColor = Color.HSVToRGB(hue, 1.0f, 1.0f);

        // Gán màu cho material
        if (targetMaterial != null)
        {
            targetMaterial.color = rainbowColor;
        }
    }
}
