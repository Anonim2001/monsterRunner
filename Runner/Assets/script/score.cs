using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class score : MonoBehaviour {

    [SerializeField]
    Text scoreUI;

    // Use this for initialization
    void Start () {
        scoreUI.text = PlayerPrefs.GetString("Score");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
