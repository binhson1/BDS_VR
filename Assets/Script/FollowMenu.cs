using UnityEngine;

public class FollowMenu : MonoBehaviour
{
    [SerializeField] private Transform handTransform; // GameObject bàn tay cần tham chiếu
    [SerializeField] private float smoothSpeed = 0.125f; // Tốc độ mượt cho chuyển động
    [SerializeField] private Vector3 offset; // Khoảng cách giữa menu và bàn tay

    void Update()
    {
        // Lấy vị trí mục tiêu của bàn tay và áp dụng offset
        Vector3 targetPosition = handTransform.position + offset;

        // Dùng Lerp để tạo chuyển động mượt mà cho menu đến vị trí mục tiêu
        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
    }
}
