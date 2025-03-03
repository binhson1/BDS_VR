using UnityEngine;
using SocketIOClient;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections;
using TMPro;
using SocketIOClient.Newtonsoft.Json;

public class SocketConnectionScript : MonoBehaviour
{
    private static SocketIO client;
    public TextMeshProUGUI menuUI;
    public ApartmentManagerScript apartmentManager;
    public GameObject buyingMenu;
    public GameObject buttonYes;
    public GameObject buttonNo;
    public GameObject menu360;
    private const string getAllEmployee = "getAllEmployee";
    private const string GetAllApartmentsEvent = "getAllApartment";
    private const string getApartmentByBlock = "getApartmentByBlock";
    private const string updateStatusToPending = "updateStatusToPending";
    private const string updateStatusToSold = "updateStatusToSold";    
    private ConcurrentQueue<string> responseQueue = new ConcurrentQueue<string>();
    void Start()
    {
        InitializeSocketClient();        
    }
    private async void InitializeSocketClient()
    {
        if (client == null)
        {
            client = new SocketIO("ws://192.168.1.8:3000");
            client.JsonSerializer = new NewtonsoftJsonSerializer();
            client.OnConnected += async (sender, e) =>
            {
                Debug.Log("Connected to the server.");
                await client.EmitAsync(GetAllApartmentsEvent);                
            };
            client.On("apartments", response =>
            {
                Debug.Log(response);
                Debug.Log("Received apartment update data.");
                ApartmentData[][] apartmentsNested = JsonConvert.DeserializeObject<ApartmentData[][]>(response.ToString());
                ApartmentData[] apartments = apartmentsNested.Length > 0 ? apartmentsNested[0] : new ApartmentData[0];

                // Enqueue data to be processed on the main thread
                if (apartmentManager != null)
                {
                    //Debug.Log("OKEY");
                    apartmentManager.EnqueueApartmentData(apartments);
                }
            });
            client.On("isChangeStatus", response =>
            {
                if (response != null)
                {
                    client.EmitAsync(GetAllApartmentsEvent);
                }
            });
            client.On(updateStatusToPending, response =>
            {
                //Debug.Log("Du lieu chuoi" + response.ToString().Trim());
                //if (response.ToString() == "[[1]]")
                //{
                //    Debug.Log("Success");
                //    client.EmitAsync(GetAllApartmentsEvent);
                //    buttonNo.SetActive(false);
                //    buttonYes.SetActive(false);
                //    menuUI.text = $"Buy Success !";
                //    Invoke("DeactivateBuyingMenu", 2f); // Gọi hàm sau 2 giây
                //}
                //else if (response.ToString() == "[[0]]")
                //{
                //    buttonNo.SetActive(false);
                //    buttonYes.SetActive(false);
                //    menuUI.text = $"Try Again Later !";
                //    Debug.Log("Fail");
                //    Invoke("DeactivateBuyingMenu", 2f); // Gọi hàm sau 2 giây
                //}
                responseQueue.Enqueue(response.ToString().Trim());

            });
            await client.ConnectAsync();
        }
        else
        {
            Debug.Log("SocketIO client already exists; reusing the existing instance.");
        }
    }

    void Update()
    {
        // Xử lý các phản hồi từ hàng đợi
        while (responseQueue.TryDequeue(out string response))
        {
            ProcessResponse(response);
        }
    }

    // Hàm xử lý phản hồi, dựa trên giá trị của response
    private void ProcessResponse(string response)
    {
        Debug.Log("Processing response: " + response);

        if (response == "[[1]]")
        {
            Debug.Log("Success");
            client.EmitAsync(GetAllApartmentsEvent);
            buttonNo.SetActive(false);
            buttonYes.SetActive(false);
            menuUI.text = "Success !";
            menu360.SetActive(false);
            Invoke("DeactivateBuyingMenu", 2f); // Gọi hàm sau 2 giây
        }
        else if (response == "[[0]]")
        {
            buttonNo.SetActive(false);
            buttonYes.SetActive(false);
            menuUI.text = "Try Again Later !";
            Debug.Log("Fail");
            menu360.SetActive(false);
            Invoke("DeactivateBuyingMenu", 2f); // Gọi hàm sau 2 giây
        }
    }
    void DeactivateBuyingMenu()
    {
        buyingMenu.SetActive(false);
        buttonNo.SetActive(true);
        buttonYes.SetActive(true);
    }
    public async void EmitHandleConfirm(string apartmentId)
    {
        Debug.Log("OKEY");
        //if (client != null && client.Connected)
        {
            // Tạo dữ liệu JSON gửi đến server
            var apartmentUpdate = new
            {
                id = apartmentId,
                idEmployee = "nv3",
                status = "Chờ bán"
            };

            // Chuyển đổi thành JSON string nếu cần
            var jsonData = JsonConvert.SerializeObject(apartmentUpdate);
            Debug.Log(jsonData);
            // Phát sự kiện "handleConfirmPendingButton" kèm theo dữ liệu JSON
            await client.EmitAsync(updateStatusToPending, apartmentUpdate);
        }
    }   

    public async void EmitSoldConfirm(string apartmentId)
    {
        if (client != null && client.Connected)
        {
            var apartmentUpdate = new
            {
                id = apartmentId,
                idEmployee = "nv3",
                status = "Đặt cọc"
            };
            await client.EmitAsync(updateStatusToSold, apartmentUpdate);
        }
    }

    public async void EmitGetEmployee()
    {
        if(client != null && client.Connected)
        {
            await client.EmitAsync("getAllEmployee");
        }
        else
        {
            Debug.Log("Can't get Employee");
        }
    }
    private async void OnDestroy()
    {
        if (client != null && client.Connected)
        {
            await client.DisconnectAsync();
            Debug.Log("Disconnected from the server.");
        }
    }
}
