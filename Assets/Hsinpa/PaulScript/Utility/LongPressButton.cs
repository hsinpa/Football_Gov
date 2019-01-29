using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;


public class LongPressButton : MonoBehaviour, IPointerEnterHandler, IPointerUpHandler, IPointerExitHandler {

	[SerializeField]
	private UnityEvent pressEvent;

	[SerializeField]
	[Range(0,5)]
	private float activateTimeStamp;

	private float recordTimeStamp;
	
	private Image visualQueue;

    private bool isEnter = false;

	private void Start() {
		visualQueue = transform.Find("loading_bar").GetComponent<Image>();
		ResetTimestamp();
	}

	public void OnPointerEnter(PointerEventData pointerEventData) {
        recordTimeStamp = Time.time;
        isEnter = true;
    }

	public void OnPointerExit(PointerEventData pointerEventData) {
        ResetTimestamp();
    }

    public void OnPointerUp(PointerEventData pointerEventData) {
		ResetTimestamp();
    }

    private void Update() {

        if (isEnter) {

            if (!TouchSerialPort.isPress) {
                recordTimeStamp = Time.time;
            }

            float timeLapse = Time.time - recordTimeStamp;
            float progress = timeLapse / activateTimeStamp;
            UpdateVisualQueue(progress);

            if (progress >= 1)
            {
                //Activate
                if (pressEvent != null)
                    pressEvent.Invoke();

                ResetTimestamp();
            }
        }

		//if (recordTimeStamp != 0 && TouchSerialPort.isPress) {
		//	float timeLapse = Time.time -  recordTimeStamp;
		//	float progress = timeLapse / activateTimeStamp;
		//	UpdateVisualQueue(progress);

		//	if (progress >= 1) {
		//		//Activate
		//		if (pressEvent != null)
		//			pressEvent.Invoke();

  //              ResetTimestamp();
  //          }
		//}

		//if (!TouchSerialPort.isPress && recordTimeStamp != 0)
		//	ResetTimestamp();
	}

	private void ResetTimestamp() {
		recordTimeStamp = 0;
        isEnter = false;
		UpdateVisualQueue(recordTimeStamp);
	}

	private void UpdateVisualQueue(float p_progress) {
		if (visualQueue != null) {
			visualQueue.fillAmount = Mathf.Clamp(p_progress, 0, 1);
		}
	}

}
