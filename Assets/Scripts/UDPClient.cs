using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using TMPro;

public class UDPClient : MonoBehaviour
{
    private UdpClient udpClient;
    private Thread udpThread;
    private int port = 8888; //9876; // The server port number
    private string serverIP = "192.168.50.138"; // The server IP address
    private int counter = 0;

    public TextMeshProUGUI logText;
    private UDPLogger uDPLogger;


    void Start ()
    {
        GameObject targetObject = GameObject.Find("Canvas");

        if (targetObject != null)
        {
            // Get the ScriptB component and call the method
            uDPLogger = targetObject.GetComponent<UDPLogger>();
        }
        else
        {
            Debug.LogWarning("uDPLogger is null!");
        }

        logText.text = "Start";
    }
/*
    void Start()
    {
        udpClient = new UdpClient();

        udpThread = new Thread(new ThreadStart(UDPReceive));
        udpThread.IsBackground = true;
        udpThread.Start();

     //   SendMessageToServer("Hello from Unity UDP Client!");
    }

    private void UDPReceive()
    {
        IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

        try
        {
            while (true)
            {
                // Listen for incoming data
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string message = Encoding.UTF8.GetString(data);

                logText.text = message;

                Debug.Log("Received from server: " + message);
            }
        }
        catch (SocketException ex)
        {
            Debug.LogError("SocketException: " + ex.Message);
        }
        finally
        {
            udpClient.Close();
        }
    }

    public void SendMessageToServer(string message)
    {
        try
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            udpClient.Send(data, data.Length, serverIP, port);
            Debug.Log("Sent: " + message);
        }
        catch (SocketException ex)
        {
            Debug.LogError("SocketException: " + ex.Message);
        }
    }
*/
    private void Execute ()
    {
        string recStr = "";
        if (ExecuteSendAndReceive("Unity data request", ref recStr))
        {
            logText.text = recStr;
            Debug.Log("ExecuteSendAndReceive: " + counter.ToString ());
        }
    }

    public void QuitApplication()
    {
        Debug.Log("Application is quitting...");

        // Exit the application
        Application.Quit();

        // If running in the editor, stop play mode (optional)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OnClick()
    {
        Debug.Log("UDPClient button clicked ");
        Execute ();
    }
    
    public void OnTrigger()
    {
        Debug.Log("UDPClient Triggered");
        Execute();
    }

    public bool ExecuteSendAndReceive(string sendStr, ref string recStr)
    {
 
        UdpClient udpClient = new UdpClient(port);
        try
        {
            udpClient.Connect(serverIP, port);

            // Sends a message to the host to which you have connected.
            byte[] sendBytes = Encoding.ASCII.GetBytes(sendStr);

            udpClient.Send(sendBytes, sendBytes.Length);

            // Sends a message to a different host using optional hostname and port parameters.
            //UdpClient udpClientB = new UdpClient();
            //udpClientB.Send(sendBytes, sendBytes.Length, "AlternateHostMachineName", 11000);

            //IPEndPoint object will allow us to read datagrams sent from any source.
            IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

            // Blocks until a message returns on this socket from a remote host.
            byte[] receiveBytes = udpClient.Receive(ref RemoteIpEndPoint);
            string returnData = Encoding.ASCII.GetString(receiveBytes);

            // Uses the IPEndPoint object to determine which of these two hosts responded.
            Debug.Log("This is the message you received " +
                                         returnData.ToString());
            Debug.Log("This message was sent from " +
                                        RemoteIpEndPoint.Address.ToString() +
                                        " on their port number " +
                                        RemoteIpEndPoint.Port.ToString());
            if (uDPLogger != null)
                uDPLogger.LogToFile(returnData);

            recStr = returnData;
            counter++;

            udpClient.Close();
            return true;
            //udpClientB.Close();
        }
        catch (SocketException ex)
        {
            Debug.LogError("SocketException: " + ex.Message);
        }
        return false;
    }

    private void OnApplicationQuit()
    {
        if (udpThread != null)
        {
            udpThread.Abort();
        }

        if (udpClient != null)
        {
            udpClient.Close();
        }
    }
}
