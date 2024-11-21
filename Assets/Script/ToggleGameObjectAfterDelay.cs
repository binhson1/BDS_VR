using UnityEngine;

public class ToggleGameObjectAfterDelay : MonoBehaviour
{
    public GameObject targetObject; // Đối tượng cần bật/tắt
    private bool isTriggered = false; // Cờ để theo dõi trạng thái
    private bool isWaitingToEnable = false; // Theo dõi trạng thái bật sau delay
    private Vector3 originalScale; // Kích thước ban đầu của đối tượng
    public float delayTime = 7f; // Thời gian trễ (có thể thay đổi từ Inspector)
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
        if (isWaitingToEnable)
        {
            timer += Time.deltaTime;

            // Khi hết thời gian chờ, bật đối tượng và reset trạng thái
            if (timer >= delayTime)
            {
                targetObject.transform.localScale = originalScale; // Bật đối tượng
                isWaitingToEnable = false;
                timer = 0f;
            }
        }
    }

    public void Trigger()
    {
        if (!isTriggered)
        {
            // Nếu đối tượng đang tắt, chuẩn bị bật sau delay
            if (!isWaitingToEnable)
            {
                isWaitingToEnable = true;
                timer = 0f;
                isTriggered = true; // Đánh dấu trạng thái bật
            }
        }
        else
        {
            // Nếu đối tượng đang bật hoặc đang chờ bật, tắt ngay lập tức
            targetObject.transform.localScale = Vector3.zero;
            isWaitingToEnable = false; // Hủy chế độ chờ
            timer = 0f;
            isTriggered = false; // Đánh dấu trạng thái tắt
        }
    }
}
