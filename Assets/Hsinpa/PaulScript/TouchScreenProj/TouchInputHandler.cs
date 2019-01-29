using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

public class TouchInputHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	[SerializeField]
	private string fieldName;
    private Vector2 touchPosition;

    [SerializeField]
    TouchScreenMain touchScreenMain;

    [SerializeField]
    Image selectedVisualQueue;

    private bool isEnter = false;


    private void Update() {
        if (isEnter) {
            if (touchScreenMain.currentSelectedField == "" || touchScreenMain.currentSelectedField == fieldName) {
                touchScreenMain.currentSelectedField = fieldName;
                touchScreenMain.torchSerialPort.OnTouch(fieldName, GetMouseDiff());
            }
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData) {
        isEnter = true;
        touchScreenMain.SetInGameBGModel(fieldName);
        touchScreenMain.torchSerialPort.OnTouchExist();
	}

    private float GetMouseDiff() {
        if (touchScreenMain.isDebugMode) {
            float diff = 0;
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (touchPosition != Vector2.zero) {
                diff = (mousePos - touchPosition).magnitude;
            }
            touchPosition = mousePos;
            return diff;
        }

        //Debug.Log("Input.touchSupported " + Input.touchSupported +" TouchCount " + Input.touchCount);
        var fingerCount = 0;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                fingerCount++;
            }
        }
        // var fingerIndex = -1;
        // for (int i = 0; i < Input.touches.Length; i++) {
        //     Touch touch = Input.GetTouch(i);
        //     if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
        //     {
        //         fingerIndex = i;
        //     }
        // }

        if (fingerCount > 0) {
            Touch touch = Input.GetTouch(fingerCount - 1);
            return touch.deltaPosition.magnitude;
        }

        return 0;
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        isEnter = false;
        touchPosition = Vector2.zero;
        touchScreenMain.currentSelectedField = "";
        touchScreenMain.torchSerialPort.OnTouchExist();
	}

}
