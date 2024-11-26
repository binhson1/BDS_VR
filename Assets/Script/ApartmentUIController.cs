using UnityEngine;
using TMPro;

public class ApartmentUIController : MonoBehaviour
{
    public TextMeshProUGUI apartmentText;
    public GameObject menu;
    public GameObject menu360;
    // Tham chiếu đến ApartmentManagerScript để lấy dữ liệu căn hộ
    public ApartmentManagerScript apartmentManager;
    public SocketConnectionScript socketConnectionScript;
    public Buying buyingScript;
    
    // Hàm kiểm tra trạng thái căn hộ và cập nhật UI
    public void CheckAndDisplayApartmentInfo()
    {
        Debug.Log("CHECK");
        ApartmentData currentApartment = apartmentManager.GetApartmentById(gameObject.name);

        if (currentApartment != null && currentApartment.status == "Chờ bán" && menu.activeSelf == false)
        {
            apartmentText.text = $"Buy {currentApartment.id}";
            menu.SetActive(true);
            menu360.SetActive(true);
            // Gửi thông tin căn hộ sang BuyingScript để lưu lại cho việc mua sau
            buyingScript.PrepareForPurchase(currentApartment);
        }
        else
        {
            Debug.Log("Apartment is not available for display or purchase.");
        }
    }
}
