using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NowFrameMove : MonoBehaviour {
    
    public TextAsset textAsset;
    //public List<FrameInfo> frames = new List<FrameInfo>();
    public List<Transform> trs;
    public GameObject gameobjk;
    public static Transform svf ;
    public Transform svfobj;
    string text ;
    string[] objInfoArray ;
    //public int x = 0;
    Vector3 pos;
    public int cd =3;
    int cdi = 3;
    public float smoothSpeed = 1;
    public string b;
    int c = 0;
    

    public FrameInfo framebig;
    int fvi = 0;

    void Start () {
        svf = svfobj;
      
    }
    
	void Update () {

        

        

      
    }
   
    void NowReadInfo()
    {
        textAsset = Resources.Load("NoeFrame") as TextAsset;

        text = textAsset.text;

        objInfoArray = text.Split('\n');

        for (int i = 0; i < 60; i++)
        {
           

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

    [System.Serializable]
    public class FrameInfo
    {
        public string Framemun;
        public Vector3[] v3s = new Vector3[28];
    }

}
