using UnityEngine;
using System.Net.Sockets;
using System.Text;
using System;

public class NetworkTimeChecker : MonoBehaviour
{
    private string ntpServer = "time.google.com"; // Бесплатный NTP-сервер

    public void GetNetworkTime(Action<DateTime> onTimeReceived)
    {
        StartCoroutine(FetchNTPTime(ntpServer, onTimeReceived));
    }
    //public void WriteTimeStartGame()
    //{
    //    GetNetworkTime((nowTime) =>
    //    {
    //        PlayerPrefs.SetString("EnterToTheGameTime", nowTime.ToString());
    //        Debug.Log(nowTime.ToString()+" Now");
    //        PlayerPrefs.Save();
    //    });
    //}
    public void WriteTimeEnterMap()
    {
        GetNetworkTime((nowTime) =>
        {
            PlayerPrefs.SetString("TimeEnterMap", nowTime.ToString());
            Debug.Log(nowTime.ToString() + " Now");
            PlayerPrefs.Save();
        });
    }
    public void WriteTimeLastGiveTime(string numShop)
    {
        GetNetworkTime((nowTime) =>
        {
            PlayerPrefs.SetString("LastGiveTime" + numShop, nowTime.ToString());
            Debug.Log(nowTime.ToString() + " Now "+numShop);
            PlayerPrefs.Save();
        });
    }
    private System.Collections.IEnumerator FetchNTPTime(string server, Action<DateTime> callback)
    {
        var ntpData = new byte[48];
        ntpData[0] = 0x1B; 

        using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            socket.ReceiveTimeout = 2000;
            try
            {
                var addresses = System.Net.Dns.GetHostEntry(server).AddressList;
                var ipEndPoint = new System.Net.IPEndPoint(addresses[0], 123);
                socket.Connect(ipEndPoint);
                socket.Send(ntpData);
                socket.Receive(ntpData);
            }
            catch (Exception e)
            {
                Debug.LogError("NTP Error: " + e.Message);
                yield break;
            }
        }

        ulong intPart = (ulong)ntpData[40] << 24 | (ulong)ntpData[41] << 16 | (ulong)ntpData[42] << 8 | ntpData[43];
        ulong fractPart = (ulong)ntpData[44] << 24 | (ulong)ntpData[45] << 16 | (ulong)ntpData[46] << 8 | ntpData[47];
        var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
        var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

        callback?.Invoke(networkDateTime.ToLocalTime());
    }
}