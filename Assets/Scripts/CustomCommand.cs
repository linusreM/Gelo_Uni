using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CustomCommand : MonoBehaviour {
    public Server server;
    public InputField ID;
    public InputField move;
    public InputField distance;
    // Use this for initialization
    public move mover;    
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButtonDown("Submit") && !mover.busy)
        {
            string message = ID.text + "#" + move.text + "#" + distance.text + "$";
            try
            {
                server.SendData(message);
                mover.busy = true;
                Debug.Log("Sending: " + message);
            }
            catch
            {
                Debug.LogError("Unable to send message!");
            }
            

        }
              
	}
}
