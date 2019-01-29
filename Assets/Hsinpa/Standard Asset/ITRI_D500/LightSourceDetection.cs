//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using System.Runtime.InteropServices;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Linq;

public class LightSourceDetection : MonoBehaviour
{

    List<GameObject> lights = new List<GameObject>();

    TcpListener server = null;
    // Buffer for reading data
    Byte[] bytes = new Byte[512];
    Socket listener;
    string data = null;
    Thread SocketThread;

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;
        SocketThread = new Thread(SocketListener);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    void SocketListener()
    {
        TcpListener server = null;
        try
        {
            // Set the TcpListener on port 27015.
            Int32 port = 27015;
            IPAddress localAddr = IPAddress.Parse("192.168.8.100");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Enter the listening loop.
            //Monitor.Enter(this);
            while (true)
            {
                //Debug.Log("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also user server.AcceptSocket() here.
                TcpClient client = server.AcceptTcpClient();
                //Debug.Log("Connected!");
                //if (data != null) Monitor.Wait(this);
                data = null;

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    //Debug.Log("Received: "+ data);

                    //// Process the data sent by the client.
                    //data = data.ToUpper();

                    //byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                    //// Send back a response.
                    //stream.Write(msg, 0, msg.Length);
                    //Debug.Log("Sent: "+ data);
                }
                // Shutdown and end connection
                client.Close();
                //if (data != null) Monitor.Pulse(this);
                //Monitor.Exit(this);
            }
        }
        catch (SocketException e)
        {
            Debug.Log("SocketException: " + e);
        }
        finally
        {
            // Stop listening for new clients.
            server.Stop();
        }


        //Console.WriteLine("\nHit enter to continue...");
        //Console.Read();
    }


    void Update()
    {
        int lightAmount = 0;
        int[] lightSourceInfo = null;
        if (data != null)
            lightSourceInfo = data.Split(',').Select(Int32.Parse).ToArray();

        if (lightSourceInfo != null)
            lightAmount = lightSourceInfo.Length / 3;

        int tmp = lightAmount - lights.Count;
        if (tmp != 0)
        {
            if (tmp > 0)
            {
                for (int i = 0; i < tmp; i++)
                {
                    GameObject lightGameObject = new GameObject();
                    Light lightComp = lightGameObject.AddComponent<Light>();
                    lightComp.transform.Rotate(Vector3.right, 90, Space.World);
                    lightComp.type = LightType.Spot;
                    lightComp.spotAngle = 160;
                    lightComp.intensity = 0.8f;
                    lightComp.shadows = LightShadows.Soft;
                    lightComp.shadowBias = 0;
                    lightComp.shadowNormalBias = 0;
                    lights.Add(lightGameObject);
                }
            }
            else
            {
                for (int i = 0; i < -tmp; i++)
                {
                    Destroy(lights[0]);
                    lights.RemoveAt(0);
                }
            }
        }
        for (int i = 0; i < lights.Count; i++)
        {
            lights[i].transform.position = setLightPosition(lightSourceInfo[3 * i], lightSourceInfo[3 * i + 1], lightSourceInfo[3 * i + 2],
                                                    107, 150, 640, 480);
        }
    }

    Vector3 setLightPosition(
        // int light_ID,
        float pixel_X,
        float pixel_Y,
        float pixel_depth,
        // int light_area,
        // int light_intensity,
        float center_depth,
        float center_distance,
        float pano_width,
        float pano_height
        )
    {
        Vector3 newPosition, newDirection;
        float distance_per_depth = center_distance / center_depth;

        pixel_X = pano_width - pixel_X;
        pixel_Y = pano_height - pixel_Y;

        float tmp_X = (pixel_X / pano_width - 0.5f) * Mathf.PI * 2;
        float tmp_Y = (pixel_Y / pano_height - 0.5f) * Mathf.PI;

        // Unity use Y axis as vertical
        newDirection = new Vector3(Mathf.Sin(tmp_X) * Mathf.Cos(tmp_Y), Mathf.Sin(tmp_Y), -Mathf.Cos(tmp_X) * Mathf.Cos(tmp_Y)).normalized;
        newPosition = newDirection * pixel_depth * distance_per_depth * 0.01f;

        Vector3 adjustVector = new Vector3(-0.2f, 0.9f, 1.6f);
        newPosition = newPosition + adjustVector;

        return newPosition;
    }
}
