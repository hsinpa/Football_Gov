using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class open : MonoBehaviour {
    TestServer ts = new TestServer();
    public Text tt;
    
    public string st;
   

    public string Slash;

    public string n_ull;
    public string FirstRemove;
    // Use this for initialization
    void Start () {
        ts.Listen(this);
        ReadFrame.MainReadFrame.JsonNowReadInfo(st.Replace('n', ',').Replace(Slash, "").Replace('"', ' ').Replace("'", "").Replace("frames", n_ull));
        st = "";
        //ReadFrame.MainReadFrame.JsonNowReadInfo(st.Replace("\n", ","));
       
        
    }

    public void poi(string n)
    {
        st = n;
        //Debug.Log(st);
        
       
    }
	// Update is called once per frame
	void Update () {
        tt.text = st;
        if (st !="")
        {
            Debug.Log(st);
            ReadFrame.MainReadFrame.JsonNowReadInfo(st.Replace('n', ',').Replace(Slash, "").Replace('"',' ').Replace("'","").Replace("frames", n_ull));
            st = "";
        }
	}
}
