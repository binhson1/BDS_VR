using UnityEngine;
using SocketIOClient;
using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Collections;

public class SocketConnectionScript : MonoBehaviour
{
    private static SocketIO client;
    public ApartmentManagerScript apartmentManager;
    private const string getAllEmployee = "getAllEmployee";
    private const string GetAllApartmentsEvent = "getAllApartment";
    private const string getApartmentByBlock = "getApartmentByBlock";
    private const string updateStatusToPending = "updateStatusToPending";
    private const string updateStatusToSold = "updateStatusToSold";
    void Start()
    {
        InitializeSocketClient();
    }
    private async void InitializeSocketClient()
    {
        if (client == null)
        {
            client = new SocketIO("ws://192.168.1.100:9000");
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

            await client.ConnectAsync();
        }
        else
        {
            Debug.Log("SocketIO client already exists; reusing the existing instance.");
        }
    }
    public async void EmitHandleConfirm()
    {
        if (client != null && client.Connected)
        {
            // Tạo dữ liệu JSON gửi đến server
            var apartmentUpdate = new
            {
                id = "A1004",
                idEmployee = "nv3",
                status = "pending"
            };

            // Phát sự kiện "handleConfirmPendingButton" kèm theo dữ liệu
            await client.EmitAsync(updateStatusToPending, apartmentUpdate);
            //Debug.Log($"Phát sự kiện mua cho căn hộ ID: {apartmentId}, Employee ID: {employeeId}");
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
