using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLaunch : MonoBehaviour {

	// Use this for initialization
	void Start () {
        XluaManager.Instance.OnInit();	
	}
	
}
