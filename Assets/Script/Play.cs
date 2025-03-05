using System.Collections;
using UnityEngine;

public class Play : MonoBehaviour
{
    public Animator animator;
    public Animator UtilAnim;
    private float targetTime = 4f; // Thời gian dừng lại ở giây thứ targetTime
    private bool isPlayingForward = true; // Kiểm tra trạng thái hiện tại
    public GameObject regenVFX;
    public GameObject manaVFX;
    public float vfxScaleDuration = 1f; // Thời gian scale cho VFX

    private Vector3 regenVFXOriginalScale; // Scale ban đầu của regenVFX
    private Vector3 manaVFXOriginalScale; // Scale ban đầu của manaVFX
    private bool flag;
    private void Start()
    {
        // Lưu scale ban đầu của các VFX
        if (regenVFX != null)
            regenVFXOriginalScale = regenVFX.transform.localScale;
        if (manaVFX != null)
        {
            manaVFXOriginalScale = manaVFX.transform.localScale;
            manaVFX.transform.localScale = Vector3.zero; // Đặt manaVFX về scale 0 khi bắt đầu
        }
    }

    public void OnButtonClick()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            // Nếu đang ở trạng thái Idle, bắt đầu chạy Forward
            animator.Play("Forward", 0, 0);
            isPlayingForward = true;
            StopAllCoroutines(); // Dừng mọi hiệu ứng scale trước đó
            StartCoroutine(ScaleVFX(manaVFX, Vector3.zero, manaVFXOriginalScale, vfxScaleDuration)); // Scale manaVFX lên
            StartCoroutine(ScaleVFX(regenVFX, regenVFXOriginalScale, Vector3.zero, vfxScaleDuration)); // Scale regenVFX xuống
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Forward") && isPlayingForward)
        {
            animator.speed = 1;
            isPlayingForward = false;
            StopAllCoroutines(); // Dừng mọi hiệu ứng scale trước đó
            StartCoroutine(ScaleVFX(regenVFX, Vector3.zero, regenVFXOriginalScale, vfxScaleDuration)); // Scale regenVFX lên
            StartCoroutine(ScaleVFX(manaVFX, manaVFX.transform.localScale, Vector3.zero, vfxScaleDuration)); // Scale manaVFX xuống
        }
    }
    void FixedUpdate()
    {
        if (isPlayingForward && animator.GetCurrentAnimatorStateInfo(0).IsName("Forward"))
        {
            // Kiểm tra thời gian của animation Forward
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= targetTime / animator.GetCurrentAnimatorStateInfo(0).length)
            {
                // Nếu đã đạt đến thời điểm targetTime, tạm dừng animation
                animator.speed = 0;
                StopAllCoroutines(); // Dừng mọi hiệu ứng scale trước đó
                StartCoroutine(ScaleVFX(manaVFX, manaVFX.transform.localScale, Vector3.zero, vfxScaleDuration)); // Scale manaVFX xuống
            }
        }
    }

    private IEnumerator ScaleVFX(GameObject vfx, Vector3 startScale, Vector3 endScale, float duration)
    {
        if (vfx == null) yield break;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            vfx.transform.localScale = Vector3.Lerp(startScale, endScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        vfx.transform.localScale = endScale;
    }

    public void PlayAnimUtil()
    {
        if (UtilAnim.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            UtilAnim.Play("Start");
            UtilAnim.SetBool("IsEnd", false);
        }
        else
        {
            UtilAnim.SetBool("IsEnd", true);
        }
    }
}
