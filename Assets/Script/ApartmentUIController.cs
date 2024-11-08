using UnityEngine;
using TMPro;

public class ApartmentUIController : MonoBehaviour
{
    // Tham chiếu đến TextMeshPro để hiển thị tên của căn hộ
    public TextMeshProUGUI apartmentText;

    public GameObject menu;    

    // Hàm kiểm tra trạng thái căn hộ và cập nhật UI
    public void CheckAndDisplayApartmentInfo()
    {                     
        apartmentText.text = $"Buy {gameObject.name}";          
        menu.SetActive(true);
    }

    // Gọi hàm này khi cần kiểm tra trạng thái, ví dụ trong Start hoặc khi có sự kiện thay đổi trạng thái
    void Start()
    {
        CheckAndDisplayApartmentInfo();
    }
}
