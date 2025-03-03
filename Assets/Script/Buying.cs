using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buying : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject menu;
    public GameObject menu360;
    public SocketConnectionScript socketConnectionScript;

    private ApartmentData apartmentToBuy;  // Lưu trữ căn hộ cần mua

    // Hàm chuẩn bị mua, chỉ lưu lại thông tin căn hộ
    public void PrepareForPurchase(ApartmentData apartment)
    {
        if (apartment != null && apartment.status == "Chờ bán")
        {
            apartmentToBuy = apartment;
            Debug.Log($"Prepared to buy apartment: {apartment.id}");
        }
        else
        {
            Debug.LogWarning("Apartment is not available for preparation.");
        }
    }

    // Hàm thực hiện mua căn hộ dựa trên thông tin đã lưu
    public void ExecutePurchase()
    {
        if (apartmentToBuy != null)
        {
            Debug.Log($"Executing purchase for apartment: {apartmentToBuy.id}");
            socketConnectionScript.EmitHandleConfirm(apartmentToBuy.id);
            apartmentToBuy = null;  // Reset sau khi mua xong
        }
        else
        {
            Debug.LogWarning("No apartment prepared for purchase.");
        }
    }
    public void Cancel()
    {
        menu.SetActive(false);
        menu360.SetActive(false);
    }
}
