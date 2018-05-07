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
    public bool activateFlag;

    private Vector3 IDPosition, movePosition, distancePosition;

    void Start()
    {
        IDPosition = ID.transform.position;
        movePosition = move.transform.position;
        distancePosition = distance.transform.position;

        ID.transform.position = new Vector3 (IDPosition.x - 500, IDPosition.y, IDPosition.z);
        move.transform.position = new Vector3(movePosition.x - 500, movePosition.y, movePosition.z);
        distance.transform.position = new Vector3(distancePosition.x - 500, distancePosition.y, distancePosition.z);
    }

    // Update is called once per frame
    void Update () {

        if (activateFlag)
        {
            ID.transform.position = IDPosition;
            move.transform.position = movePosition;
            distance.transform.position = distancePosition;
        }
        else
        {
            ID.transform.position = new Vector3(IDPosition.x - 500, IDPosition.y, IDPosition.z);
            move.transform.position = new Vector3(movePosition.x - 500, movePosition.y, movePosition.z);
            distance.transform.position = new Vector3(distancePosition.x - 500, distancePosition.y, distancePosition.z);
        }
        if (Input.GetButtonDown("Submit") && !mover.busy && activateFlag)
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
