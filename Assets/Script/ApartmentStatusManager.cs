using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using UnityEngine;

[Serializable]
public class Apartment
{
    public string idEmployee;
    public string block;
    public string floor;
    public string apartment;
    public string location;
    public string status; // Trạng thái (ví dụ: "sold" hoặc "available")
    public string note;
    public string createdAt;
    public string updatedAt;
    public string deletedAt;
}


public class ApartmentStatusManager : MonoBehaviour
{
    private ClientWebSocket client;
    public GameObject[] houses; // Mảng chứa 4 GameObject đại diện cho 4 căn nhà

    private async void Start()
    {
        client = new ClientWebSocket();
        Uri serverUri = new Uri("ws://server_address"); // Thay "server_address" bằng địa chỉ WebSocket server

        // Kết nối tới WebSocket server
        await client.ConnectAsync(serverUri, CancellationToken.None);

        // Bắt đầu lắng nghe dữ liệu từ server
        StartCoroutine(ReceiveMessages());
    }

    private IEnumerator ReceiveMessages()
    {
        var buffer = new byte[1024];
        
        while (client.State == WebSocketState.Open)
        {
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            else
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                UpdateApartmentStatus(message);
            }

            yield return null;
        }
    }

    private void UpdateApartmentStatus(string data)
    {
        // Parse JSON dữ liệu nhận được
        Apartment[] apartments = JsonUtility.FromJson<ApartmentData>($"{{\"apartments\":{data}}}").apartments;

        for (int i = 0; i < houses.Length && i < apartments.Length; i++)
        {
            // Kiểm tra trạng thái và đổi màu Cube
            if (apartments[i].status == "sold")
            {
                houses[i].GetComponent<Renderer>().material.color = Color.red;
            }
            else
            {
                houses[i].GetComponent<Renderer>().material.color = Color.green;
            }
        }
    }

    private async void OnDestroy()
    {
        // Đảm bảo đóng kết nối khi hủy đối tượng
        if (client != null && client.State == WebSocketState.Open)
        {
            await client.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", CancellationToken.None);
            client.Dispose();
        }
    }
}

[Serializable]
public class ApartmentData
{
    public Apartment[] apartments;
}
