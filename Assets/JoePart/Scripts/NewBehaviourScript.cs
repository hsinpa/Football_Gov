using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour {

    public int[] Ax;//空幾格
    public int[] Ay;//有幾個
    public int[] Az;//第幾個再生一個
    //在外面輸入時這三個的長度要一樣

    public float Rota;//旋轉
    
    public GameObject box;//要生的物件
	// Use this for initialization
	void Start () {

        for (int y = 0; y < Ay.Length; y++)//目前在第幾列
        {

            for (int x = 0; x < Ay[y]; x++)//目前在第幾個
            {
                Instantiate(box, new Vector3(transform.position.x + x + Ax[y], transform.position.y - y, transform.position.z), transform.rotation, transform);
                //         物件               位置                                                                                 角度                父物件   
            }
            if (Az[y] > 0)//如果此欄不為0則在該位置生成一個
            {
                Instantiate(box, new Vector3(transform.position.x + Az[y], transform.position.y - y, transform.position.z), transform.rotation, transform);
            }

        }

        transform.Rotate(new Vector3(0, 0, Rota));//旋轉

	}
	
}
