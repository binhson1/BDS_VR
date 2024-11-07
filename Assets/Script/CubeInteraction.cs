//using UnityEngine;

//public class CubeInteraction : MonoBehaviour
//{
//    public string apartmentId;  // ID của căn hộ, đặt giá trị này trong Inspector
//    private SocketConnectionManager socketManager;

//    void Start()
//    {
//        // Tìm và gán đối tượng SocketConnectionManager từ trong scene
//        socketManager = FindObjectOfType<SocketConnectionManager>();

//        if (socketManager == null)
//        {
//            Debug.LogError("Không tìm thấy SocketConnectionManager trong scene.");
//        }
//    }

//    // Gọi khi người dùng kích hoạt sự kiện "mua" qua trigger
//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Player"))  // Giả sử người dùng là đối tượng có tag "Player"
//        {
//            if (socketManager != null)
//            {
//                // Gửi yêu cầu "mua" với ID của căn hộ lên server
//                socketManager.EmitHandleConfirm(apartmentId);
//            }
//        }
//    }
//}
