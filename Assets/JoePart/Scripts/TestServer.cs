using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;


public class TestServer
{


   
    Socket[] SckSs;  
    Thread SckSAcceptTd;
    int SckCIndex;    
    string LocalIP = "127.0.0.1"; 
    int SPort = 9999;

    int RDataLen = 5; 

  

    string onText;
    open open;
    
    public void Listen(open open1)
    {
        open = open1;
        

        Array.Resize(ref SckSs, 1);

        SckSs[0] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        SckSs[0].Bind(new IPEndPoint(IPAddress.Parse(LocalIP), SPort));



      
        SckSs[0].Listen(10);

        SckSWaitAccept();   

    }



    // 等待Client連線

    private void SckSWaitAccept()

    {


        bool FlagFinded = false;

        for (int i = 1; i < SckSs.Length; i++)

        {

            

            if (SckSs[i] != null)

            {
                

               
                if (SckSs[i].Connected == false)

                {
                    
                    SckCIndex = i;

                    FlagFinded = true;

                    break;

                }

            }

        }

       

        if (FlagFinded == false)

        {


            SckCIndex = SckSs.Length;

            Array.Resize(ref SckSs, SckCIndex + 1);

        }



       

        SckSAcceptTd = new Thread(SckSAcceptProc);

        SckSAcceptTd.Start();  

    }





    // 接收來自Client的連線與Client傳來的資料

    private void SckSAcceptProc()

    {


        try

        {

            SckSs[SckCIndex] = SckSs[0].Accept();  // 等待Client 端連線


            int Scki = SckCIndex;
            try

            {
                JSONObject jSON = new JSONObject();
                jSON.AddField("Type", "PlayerID");
                jSON.AddField("PlayerID", Convert.ToInt32(Math.Pow(2,SckCIndex)));
                string SendS = jSON.ToString();      
                UnityEngine.Debug.Log(Math.Pow(2, SckCIndex));
                SckSs[SckCIndex].Send(Encoding.ASCII.GetBytes(SendS));
                Console.WriteLine(SendS);
            }

            catch

            {
                // Debug.Log("catch");
                Console.WriteLine("dwo");
                

            }


            SckSWaitAccept();



            long dataLength;

            
            int DataLen = 2048, IntAcceptData;
            byte[] clientData = new byte[RDataLen];  
            byte[] bytes = new byte[DataLen];
            int iii = 0;
            while (true)

            {

  
               

                dataLength = SckSs[Scki].Receive(bytes);

                

              

                string S = Encoding.Default.GetString(bytes);
                Console.WriteLine(S);
                UnityEngine.Debug.Log(S);
                open.poi(S);
                //    SckSSend(S);

            }

        }

        catch

        {

           

        }

    }



    // Server 傳送資料給所有Client

    private void SckSSend(string st)

    {

        for (int Scki = 1; Scki < SckSs.Length; Scki++)

        {

            if (null != SckSs[Scki] && SckSs[Scki].Connected == true)

            {

                try

                {

                    string SendS = st;      

                    SckSs[Scki].Send(Encoding.ASCII.GetBytes(SendS));

                }

                catch

                {
                    // Debug.Log("catch");
                 

                }

            }

        }

    }

    public void Quit()
    {
        

        for (int Scki = 1; Scki < SckSs.Length; Scki++)

        {

            if (null != SckSs[Scki] && SckSs[Scki].Connected == true)

            {

                try

                {

                    SckSAcceptTd.Abort();
                    
                    SckSs[Scki].Close();
                    
                }

                catch

                {
                    // Debug.Log("catch");
                    

                }

            }

        }
    }

}
