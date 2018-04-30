using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour {

	public string id;
	public string description;
	public Vector3 position;
	public Vector3 rotation;
	public Vector3 scale;


	public void setProperties(string _id, string _description)  {
		id = _id;
		description = _description;
	}


		
}
