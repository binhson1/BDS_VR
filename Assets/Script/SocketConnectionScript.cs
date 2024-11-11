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
            client.On("isChangeStatus", response =>
            {
                if (response != null)
                {
                    client.EmitAsync(GetAllApartmentsEvent);
                }
            });
            client.On("updateStatusToPending", response =>
            {
                if (response.ToString() == "[1]")
                {
                    Debug.Log("Success");
                    ApartmentManagerScript.Qu
                }
                if (response.ToString() == "[0]")
                {
                    Debug.Log("Fail");
                }
            });
            await client.ConnectAsync();            
        }
        else
        {
            Debug.Log("SocketIO client already exists; reusing the existing instance.");
        }
    }
    public async void EmitHandleConfirm(string apartmentId)
    {
        if (client != null && client.Connected)
        {
            // Tạo dữ liệu JSON gửi đến server
            var apartmentUpdate = new
            {
                id = apartmentId,
                idEmployee = "nv3",
                status = "Chờ bán"
            };

            // Phát sự kiện "handleConfirmPendingButton" kèm theo dữ liệu
            await client.EmitAsync(updateStatusToPending, apartmentUpdate);
            //Debug.Log($"Phát sự kiện mua cho căn hộ ID: {apartmentId}, Employee ID: {employeeId}");
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
