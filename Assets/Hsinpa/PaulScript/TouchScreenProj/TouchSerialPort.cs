using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.Text;
using System.Threading;

public class TouchSerialPort {
	private SerialPort _sp;
    private const float speed_1 = 0.5f, speed_2 = 4.5f, speed_3 = 15f, speed_4 = 25f;
	private string lastBinaryRecord;

	public static bool isPress;

	public TouchSerialPort(int port_num, bool isDebugMode) {
		_sp = null;
		isPress = isDebugMode;

		_sp = new SerialPort("\\\\.\\COM"+port_num, 115200);
		_sp.ReadTimeout = 100;

		try
		{
			_sp.Open();             //開啟SerialPort連線
            Thread readThread = new Thread(new ThreadStart(ReadMsgFromSensor)); //實例化執行緒與指派呼叫函式
            readThread.Start();           //開啟執行緒
            Debug.Log("SerialPort開啟連接");
		}
		catch
		{
			Debug.Log("SerialPort連接失敗");
		}
	}

	public void OnTouch(string p_field, float p_velocity) {

        //Debug.Log("Velocity " + p_velocity);
        //Send something only moving velocity higher than speed_1
        if (p_velocity < speed_1) {
			OnTouchExist();
		} else {
			string velocity = EncodeVelocity(p_velocity);
			string field = EncodeField(p_field);
			string build_binary = "01" + field + velocity + "01";

			// Debug.Log("build_binary : " + build_binary);
			SendBinary(build_binary);
		}
	}

	public void OnTouchExist() {
		SendBinary("01000000");
	}

	private string EncodeVelocity(float p_velocity) {

        if (p_velocity >= speed_1 && p_velocity < speed_2) {
			return "00";
		} else if (p_velocity >= speed_2 && p_velocity < speed_3) {
            return "01";
        } else if (p_velocity >= speed_3 && p_velocity < speed_4) {
            return "10";
		} else if (p_velocity >= speed_4) {
			return "11";
		}

		return "00";
	}

	private string EncodeField(string p_field) {
		switch (p_field) {
			case "grass" :
			return "10";
			case "concrete" :
			return "01";
		}

		return "00";
	}
	

	private void ReadMsgFromSensor()
    {
        while (_sp.IsOpen)
        {
            try
            {
                CheckIsPress(_sp.ReadLine()); //讀取藍芽資料並裝入readMessage
            } catch (System.Exception e) {
                //Debug.LogWarning(e.Message);
            }
        }
    }

	private void CheckIsPress(string p_raw_message) {
		isPress = (p_raw_message == "PressLv3" || p_raw_message == "PressLv4");
	}

	private void SendBinary(string binary) {
		if (_sp == null || !_sp.IsOpen || lastBinaryRecord == binary) return;
			
			lastBinaryRecord = binary;
			// Translate binary to ASCII
			StringBuilder decodedBinary = new StringBuilder();
			for (int i = 0; i < binary.Length; i += 8)
			{
				decodedBinary.Append(Convert.ToChar(Convert.ToByte(binary.Substring(i, 8), 2)));
			}
			string str1 = decodedBinary.ToString();
			_sp.Write(str1);
			//Debug.Log(str1);	
		}
	

    public void Disconnect()  //關閉連接埠
    {
        if (_sp != null)
        {
            if (_sp.IsOpen)
            {
                _sp.Close();
            }
        }
    }

}
