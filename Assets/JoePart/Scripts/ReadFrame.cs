using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

//筆記
//打擊點加速
public class ReadFrame : MonoBehaviour {

    public static ReadFrame MainReadFrame;
    public TextAsset objectsInfoListText;
    public TextAsset textAsset;
    public List<FrameInfo> frames = new List<FrameInfo>();
    public List<Transform> trs;
    public GameObject gameobjk;
    public static Transform svf ;
    public Transform svfobj;
    string text ;
    string[] objInfoArray ;
    public int x = 0;
    public Vector3 pos;
    public int cd =3;
    int cdi = 3;
    public float smoothSpeed = 1;

    public bool nowRead;

    public FrameInfo framebig;
    int fvi = 0;

    public string b;
    int c = 0;

    public GameObject ball;
    string[] nowInfoArrat;
    int nowFrameNum = 0;
    private void Awake()
    {
        MainReadFrame = this;
        calibrationTime = 5;
    }

    public void UI_GatBall()
    {
        GameObject g = Instantiate(ball, new Vector3(0f,-1.1f,0.4f), transform.rotation);
        Destroy(g, 20);
    }

    public void UI_SetSmoothSpeed(Slider slider)
    {
        smoothSpeed = slider.value;
    }

    public void UI_SetNowFrame(bool b)
    {
        nowRead = b;
        x = 0;        
        c = 0;
    }

    public void UI_calibration()
    {
        calibrationTime = 1;
    }

    public void UI_ReloadTxT()
    {
        //text = objectsInfoListText.text;
        string sPath = Application.streamingAssetsPath + "/FrameData.txt";
        WWW www = new WWW(sPath);        
        text = www.text;
        Debug.Log(text);
        objInfoArray = text.Split('\n');
        x = 0;
        c = 0;
        Debug.Log(objInfoArray.Length);
    }

    void Start () {
        svf = svfobj;
        StartCoroutine(LoadTXT());

        /*for(int i = 0; i<28; i++)
        {
            GameObject geObject = Instantiate(gameobjk, gameObject.transform);
            geObject.name = "obj" + i;
            trs.Add(geObject.transform);
        }*/

    }
   
	
	void Update () {
        if (!nowRead) {
            if (nowInfoArrat.Length > 0)
            {

                TxTReadInfo();
            }
        }

       /* if (nowRead)
        {
            StartCoroutine(LoadTXT());
        }
        else
        {
            ReadInfo();

            for (int i = 0; i < 28; i++)
            {
                if (cdi >= cd)
                {
                    pos = frames[c].v3s[i];
                    cdi = 0;
                }

                trs[i].localPosition = Vector3.Lerp(trs[i].localPosition, pos, smoothSpeed);
            }
            cdi++;
        }
        */
    }
   
    void ReadInfo()
    {

        for (int i = x; i < x+60; i++)
        {
            //Debug.Log(i);
            //Debug.Log(objInfoArray[i]);
            if(i>= objInfoArray.Length)
            {
                x = 0;
                i = 0;
                c = 0;
            }
            if (objInfoArray[i] == "" || objInfoArray[i] == b)
            {
                //Debug.Log(objInfoArray[i] + "jj");
                continue;

            }

            string[] Framev1 = objInfoArray[i].Split(' ');

            if (Framev1[0] == "Frame")
            {
                FrameInfo frame = new FrameInfo();

                frame.Framemun = Framev1[1];
               // Debug.Log(frame.Framemun);
                c = int.Parse(frame.Framemun);
                frames.Add(frame);
                framebig = frame;
                fvi = 0;
                continue;
            }

            if (Framev1[0] != " ")
            {
                //Debug.Log(Framev1[0]);
                float[] f3 = new float[3];
                int fi = 0;
                foreach (string str2 in Framev1)
                {
                    //Debug.Log("ppp" + str2);
                    if (str2 != " " && str2 != "" && str2 != "\n")
                    {
                        f3[fi] = float.Parse(str2);
                        fi++;
                    }

                    //Debug.Log(str2);
                }
                framebig.v3s[fvi] = new Vector3(f3[0], f3[1], f3[2]);
                fvi++;
                if (fvi == 28)
                {
                    fvi = 0;
                   // Debug.Log("enter" + i);
                    x = i;
                    break;
                }
            }

        }
    }

    public string TxTName;

    IEnumerator LoadTXT()
    {

        string sPath = System.IO.Path.Combine(Application.streamingAssetsPath, "dump1.txt");

        #if UNITY_STANDALONE_OSX
            sPath = "file://"+sPath;
        #endif

        WWW www = new WWW(sPath);
        yield return www;
        text = www.text;
        nowInfoArrat = text.Split('\n');
       
    }

    IEnumerator LoadData()
    {
        string sPath = Application.streamingAssetsPath + "/FrameData.txt";
        WWW www = new WWW(sPath);
        yield return www;
        text = www.text;
        //NowReadInfo();
    }

   
    void NowReadInfo()
    {



        nowInfoArrat = text.Split('\n');

        for (int i = 0; i < 60; i++)
        {

            
            if (nowInfoArrat[i] == "" || nowInfoArrat[i] == b)
            {
                //Debug.Log(objInfoArray[i] + "jj");
                continue;

            }

            string[] Framev1 = nowInfoArrat[i].Split(' ');

            if (Framev1[0] == "Frame")
            {
                FrameInfo frame = new FrameInfo();

                frame.Framemun = Framev1[1];

                framebig = frame;
                fvi = 0;
                continue;
            }

            if (Framev1[0] != " ")
            {
                //Debug.Log(Framev1[0]);
                float[] f3 = new float[3];
                int fi = 0;
                foreach (string str2 in Framev1)
                {
                    //Debug.Log("ppp" + str2);
                    if (str2 != " " && str2 != "" && str2 != "\n")
                    {
                        f3[fi] = float.Parse(str2);
                        fi++;
                    }

                    //Debug.Log(str2);
                }
                framebig.v3s[fvi] = new Vector3(f3[0], f3[1], f3[2]);
                fvi++;
                //Debug.Log("fvi" + fvi);
                if (fvi == 28)
                {
                    fvi = 0;
                    Debug.Log("enter" + i);

                    break;
                }
            }

        }

        for (int i = 0; i < 28; i++)
        {
            if (cdi >= cd)
            {
                pos = framebig.v3s[i];
                cdi = 0;
            }

            trs[i].localPosition = Vector3.Lerp(trs[i].localPosition, pos, smoothSpeed);
        }
        cdi++;
    }
    List<JSONObject> jsonVector3;
    List<JSONObject> js;

     

    public void JsonNowReadInfo(string n)
    {

        JSONObject jSON = new JSONObject(n);
       
        //Debug.Log(n);
        //Debug.Log(jSON.list[0]);

        List<JSONObject> js = new JSONObject(n).list;
        //Debug.Log(js[0].list[0]);
       
        


        
        for (int i = 0; i < js[0].list.Count; i++)
        {
            if (cdi >= cd)
            {
                jsonVector3 = js[0].list[i].list;
                pos = new Vector3(jsonVector3[0].list[0].n, jsonVector3[1].list[0].n, jsonVector3[2].list[0].n*0.001f);
                cdi = 0;
            }
            Points[i] = pos;
        }
        if(modify)
        pointCorrection(Points);
       

        for (int i = 0; i < js[0].list.Count; i++)
        {

            trs[i].localPosition = Vector3.Lerp(trs[i].localPosition, Points[i], smoothSpeed);
        }

        cdi++;
        calibrationTime -= Time.deltaTime;
    }
    public List<Vector3> Points = new List<Vector3>(); 
    public float posD;
    public float calibrationTime;
    public string Slash_N;
   
    public string n_ull;
    public string FirstRemove;
    public float footD = 0.5f;

    public bool modify;
    bool pointCorrection(List<Vector3> p)
    {
        if (p[2].z > p[1].z)
        {
            p[2] = new Vector3(p[2].x, p[2].y, p[1].z-footD);
            p[3] = new Vector3(p[3].x, p[3].y, p[1].z - footD);
        }

        if (p[6].z > p[5].z)
        {
            p[6] = new Vector3(p[6].x, p[6].y, p[5].z - footD);
            p[7] = new Vector3(p[7].x, p[7].y, p[5].z - footD);
        }

        if (p[3].y > p[2].y)
        {
            p[3] = new Vector3(p[3].x, p[2].y - 0.2f, p[3].z);
            
        }

        if (p[7].y > p[6].y)
        {
            p[7] = new Vector3(p[7].x, p[6].y - 0.2f, p[7].z);
           
        }

        return true;
    }

    public void TxTReadInfo(){
        //Debug.Log(nowInfoArrat[nowFrameNum].ToString());
        JsonNowReadInfo(nowInfoArrat[nowFrameNum].Replace(Slash_N, ",").Replace('"', ' ').Replace("frames", n_ull).Replace(FirstRemove,"").Replace("'",""));

        nowFrameNum++;
        if (nowFrameNum >= nowInfoArrat.Length)
        {
            nowFrameNum = 0;
        }
        //Debug.Log(nowInfoArrat[nowFrameNum].Replace(Slash_N, ",").Replace('"', "".ToCharArray()[0]).Replace("frames", n_ull).Remove(4));
        //st.Replace(p, "").Replace('"', 'l').Replace("l", "").Replace("'", "").Replace(k, u)


    }


    [System.Serializable]
    public class FrameInfo
    {
        public string Framemun;
        public Vector3[] v3s = new Vector3[28];
    }

}
