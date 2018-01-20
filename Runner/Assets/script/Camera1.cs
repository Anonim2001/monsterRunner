using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1 : MonoBehaviour {

    [SerializeField]
    Transform target;

    Vector3 offset;

    void Start () {
        offset = target.transform.position - transform.position;
    }
	
	
	void Update () {
        Vector3 pos = target.transform.position - offset;
        this.transform.position = Vector3.Lerp(this.transform.position, pos, 1.5f);
    }
}
