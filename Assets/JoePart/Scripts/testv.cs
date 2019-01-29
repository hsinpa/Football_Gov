using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testv : MonoBehaviour {
    public open open;
    public string j;
    public string y;
    public string k;
    public string text;
    public string[] nowInfoArrat;

    public string Slash_N;
   
    
    public string n_ull;
    
    // Use this for initialization
    void Start () {
        
        StartCoroutine(LoadTXT());
    }
	
	// Update is called once per frame
	void Update () {
       //open.poi(j);
	}

    IEnumerator LoadTXT()
    {
        string sPath = Application.streamingAssetsPath + "/dump2.txt";
        WWW www = new WWW(sPath);
        yield return www;
        text = www.text;
        nowInfoArrat = text.Split('\n');
        string n = nowInfoArrat[0].Replace(Slash_N, ",").Replace('"', ' ').Replace("frames", n_ull).Remove(0, 4);
        Debug.Log(n);
        JSONObject jSON = new JSONObject(n);
        List<JSONObject> js = new JSONObject(n).list;
        Debug.Log(js[0].list[0]);
    }
}
