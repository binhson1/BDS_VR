using UnityEngine;

public class Play : MonoBehaviour
{
    public Animator animator;
    private float targetTime = 8f; // Giây dừng lại
    public bool isPlaying = false;
    private bool oneTime = false;

    // void Start()
    // {
    //     animator.Play("Animation_2", 0, 0); // Bắt đầu animation từ đầu
    //     animator.speed = 0; // Dừng animation ở đầu
    // }

    public void OnButtonClick()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            animator.Play("Animation_2", 0, 0);
            oneTime = false;
        }
        if (!isPlaying)
        {
            // Bắt đầu chạy animation từ vị trí hiện tại
            animator.speed = 1;
            isPlaying = true;
        }
        else
        {
            // Nếu animation đã dừng lại ở targetTime, tiếp tục đến cuối
            animator.speed = 1;
        }
    }

    void FixedUpdate()
    {
        if (!oneTime)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (isPlaying && stateInfo.IsName("Animation_2"))
            {
                // Kiểm tra thời gian animation trong trạng thái này
                if (stateInfo.normalizedTime * stateInfo.length >= targetTime)
                {
                    animator.speed = 0; // Dừng animation
                    isPlaying = false;
                    oneTime = true;
                }
            }
        }
    }
}
