using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LightingDemoHelper : MonoBehaviour {

	void Update() {

		if (Input.GetKeyDown(KeyCode.Space)) {
			SceneManager.LoadScene("Part3Football", LoadSceneMode.Single);
		
		}

	}
}
