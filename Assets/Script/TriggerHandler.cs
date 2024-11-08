using UnityEngine;
using UnityEngine.UI;

public class TriggerHandler : MonoBehaviour
{
    // Tham chiếu đến Text UI mà bạn muốn cập nhật tên đối tượng
    public GameObject menu;

    // Phương thức này sẽ được gọi khi có đối tượng đi vào Trigger Collider
    private void OnTriggerEnter(Collider other)
    {
        // Gửi tên của đối tượng vào Text UI
        if (menu != null)
        {
            menu.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Target Text UI is not assigned.");
        }
    }
}
