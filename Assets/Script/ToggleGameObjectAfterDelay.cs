using System.Collections;
using UnityEngine;

public class ToggleGameObjectAfterDelay : MonoBehaviour
{
    public GameObject targetObject; // Đối tượng cần bật/tắt
    private bool isTriggered = false; // Trạng thái kiểm tra có đang chờ để bật/tắt không
    private Coroutine toggleCoroutine;

    void Start()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); // Đảm bảo GameObject tắt ban đầu
        }
    }

    public void Trigger()
    {
        if (!isTriggered)
        {
            // Nếu chưa có Coroutine đang chạy, bắt đầu Coroutine bật sau 8 giây
            toggleCoroutine = StartCoroutine(ToggleAfterDelay(6f, true));
        }
        else
        {
            // Nếu đã bật, ngừng Coroutine và tắt GameObject ngay lập tức
            StopCoroutine(toggleCoroutine);
            targetObject.SetActive(false);
            isTriggered = false;
        }
    }

    private IEnumerator ToggleAfterDelay(float delay, bool state)
    {
        isTriggered = true;
        yield return new WaitForSeconds(delay);

        targetObject.SetActive(state);
        isTriggered = false;
    }
}
