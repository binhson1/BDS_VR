using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;
using Newtonsoft.Json.Linq;

public class API : MonoBehaviour
{
    private SocketIO client;

    // Start is called before the first frame update
    async void Start()
    {
        // Initialize the SocketIO client
        client = new SocketIO("ws://192.168.1.100:9000");

        // Register for connected event
        client.OnConnected += (sender, e) =>
        {
            Debug.Log("Connected to the server.");
        };
        // Listen for the "employees" event from the server
        client.On("employees", response =>
        {
            Debug.Log(response);
            Debug.Log("Received employees data from server.");
            Debug.Log("OKEY");
            ParseEmployeeData(response.GetValue<JArray>());

        });

        // Connect to the WebSocket server
        await client.ConnectAsync();
    }

    // Method to parse and handle employee data
    private void ParseEmployeeData(JArray employeesArray)
    {
        foreach (var employee in employeesArray)
        {
            // Parse the employee data with the new structure
            string id = employee.Value<string>("id");
            string fullname = employee.Value<string>("fullname");
            string cccd = employee.Value<string>("cccd");
            string phone = employee.Value<string>("phone");
            string level = employee.Value<string>("level");
            string createdAt = employee.Value<string>("createdAt");
            string updatedAt = employee.Value<string>("updatedAt");
            string deletedAt = employee["deletedAt"]?.ToString(); // Handle nullable deletedAt

            Debug.Log($"ID: {id}, Full Name: {fullname}, CCCD: {cccd}, Phone: {phone}, Level: {level}");
            Debug.Log($"Created At: {createdAt}, Updated At: {updatedAt}, Deleted At: {deletedAt}");
        }
    }

    // Example method to request employee data from the server
    public async void RequestEmployeeData()
    {
        if (client != null && client.Connected)
        {
            await client.EmitAsync("employees", "abc");
            Debug.Log("Requested employee data.");
        }
        else
        {
            Debug.LogWarning("Client is not connected.");
        }
    }

    private async void OnApplicationQuit()
    {
        // Disconnect when the application quits
        if (client != null)
        {
            await client.DisconnectAsync();
            Debug.Log("Disconnected from the server.");
        }
    }
}
