using UnityEngine;

public class ToggleGameObjectAfterDelay : MonoBehaviour
{
    public GameObject targetObject; // Đối tượng cần bật/tắt
    private bool isTriggered = false; // Trạng thái có đang chờ bật/tắt không
    private Vector3 originalScale; // Kích thước ban đầu của đối tượng
    private float delayTime = 3f; // Thời gian trễ 3 giây
    private float timer = 0f; // Bộ đếm thời gian

    void Start()
    {
        if (targetObject != null)
        {
            originalScale = targetObject.transform.localScale;
            targetObject.transform.localScale = Vector3.zero; // Đặt đối tượng ban đầu thành vô hình
        }
    }

    void Update()
    {
        // Nếu đang chờ hết thời gian và cờ đang bật, tăng bộ đếm thời gian
        if (isTriggered)
        {
            timer += Time.deltaTime;

            // Khi hết 3 giây, bật lại kích thước và đặt cờ về false
            if (timer >= delayTime)
            {
                targetObject.transform.localScale = originalScale;
                isTriggered = false;
                timer = 0f; // Reset bộ đếm
            }
        }
    }

    public void Trigger()
    {
        if (!isTriggered)
        {
            // Nếu chưa được kích hoạt, bắt đầu chế độ chờ và thu nhỏ đối tượng
            targetObject.transform.localScale = Vector3.zero;
            isTriggered = true; // Bắt đầu đếm thời gian 3 giây
        }
        else
        {
            // Nếu đã được kích hoạt, quay về kích thước ban đầu ngay lập tức và tắt chế độ chờ
            targetObject.transform.localScale = originalScale;
            isTriggered = false;
            timer = 0f; // Reset bộ đếm
        }
    }
}
