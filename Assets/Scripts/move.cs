using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class move : MonoBehaviour
{
    //private Vector3 position;
    // Use this for initialization
    static public string lastReceivedUDPPacket;
    public GameObject forward, back, left, right, bot, basePosition, qr;
    public float divisor = 1.0f;
    public Compshader cShader;
    public Server server;
    public bool busy = false;
	public GameObject aruco;

    //Vector3 basePosition;
	void Start()
	{
		aruco = GameObject.CreatePrimitive (PrimitiveType.Cube);
	}

    // Update is called once per frame
    void Update()
    {
        string[] packets = Server.allReceivedUDPPackets.Split('$'); 
        int packetsToHandle = Server.unhandledPackets;
        Server.unhandledPackets = 0;
        Server.allReceivedUDPPackets = "";
        if(Server.lastRecivedFromIP != "" && packetsToHandle != 0)
            Debug.Log("Last message from IP: " + Server.lastRecivedFromIP);
        
        for (int i = 0; i < packetsToHandle ; i++)
        {

            string[] splitString;


			if (packets [i] != "") {

				splitString = packets [i].Split ('#');

				//print(splitString[0]);
				//float xpos = float.Parse(Server.lastReceivedUDPPacket);
				//transform.eulerAngles = new Vector3(float.Parse(splitString[0]), float.Parse(splitString[1]), float.Parse(splitString[2]));
				Debug.Log ("Split1: [" + splitString [1] + "]");
				if (splitString [1] == "QR") {
					//0ID#1TYPE#2DATA#3X#4Y#5Z#6ROTATION TYP
					Debug.Log ("Found QR: " + splitString [2]);

					qr.transform.localPosition = new Vector3 (float.Parse (splitString [3]) / divisor, -(float.Parse (splitString [4])) / divisor, float.Parse (splitString [5]) / divisor);

					float xRotation = float.Parse (splitString [6]);
					float yRotation = float.Parse (splitString [7]);
					float zRotation = float.Parse (splitString [8]);
					float theta = (Mathf.Sqrt ((xRotation * xRotation) + (yRotation * yRotation) + (zRotation * zRotation))* (180 / Mathf.PI)); 
					Vector3 axis = new Vector3 (-xRotation, yRotation, -zRotation);
					Quaternion rotation = Quaternion.AngleAxis (theta, axis);
					Debug.Log (rotation);
					qr.transform.localRotation = rotation;
				
					//cube1.AddComponent<Rigidbody>();
					aruco.transform.localScale = new Vector3 (7f, 7f, 7f);
					aruco.transform.position = qr.transform.position;
					aruco.transform.rotation = qr.transform.rotation;
		
					//Instantiate("Prefabs/cube", new Vector3(float.Parse(splitString[3]), float.Parse(splitString[5]), float.Parse(splitString[4])), Quaternion.identity);
				} else {
					try {
						Debug.Log (splitString [0] + " sent " + splitString [1] + ", More:" + splitString [2] + ", Travel: " + splitString [3] + ", with: " + splitString [4] + "F, " + splitString [5] + "B, " + splitString [6] + "L, " + splitString [7] + "R");
						bot.transform.position = basePosition.transform.position;
						bot.transform.rotation = basePosition.transform.rotation;
						if (splitString [1] == "0" || splitString [1] == "MOVE")
							bot.transform.Translate (Vector3.forward * (float.Parse (splitString [3]) / divisor));
						else if (splitString [1] == "1" || splitString [1] == "TURN")
							bot.transform.Rotate (new Vector3 (0, 1, 0) * (float.Parse (splitString [3])));

                    //bot.transform.Rotate(new Vector3(0,1,0) * (float.Parse(splitString[3])));


                    else {
							bot.transform.Translate (Vector3.forward * (float.Parse (splitString [3]) / divisor));
							Debug.Log ("Unrecognized movement: [" + splitString [3] + "], Interpreted as : [Move]");
						}
						right.transform.localPosition = new Vector3 (float.Parse (splitString [7]) / divisor, 0.0f, 0.0f);
						left.transform.localPosition = new Vector3 (-(float.Parse (splitString [6]) / divisor), 0.0f, 0.0f);
						forward.transform.localPosition = new Vector3 (0.0f, 0.0f, float.Parse (splitString [4]) / divisor);
						back.transform.localPosition = new Vector3 (0.0f, 0.0f, -(float.Parse (splitString [5]) / divisor));

						Vector2 shForward = new Vector2 (forward.transform.position.x, forward.transform.position.z);
						Vector2 shBack = new Vector2 (back.transform.position.x, back.transform.position.z);
						Vector2 shLeft = new Vector2 (left.transform.position.x, left.transform.position.z);
						Vector2 shRight = new Vector2 (right.transform.position.x, right.transform.position.z);




						//RunShader(div_x, div_y);

						cShader.RunShader4 (shForward, shBack, shLeft, shRight);
						if (int.Parse (splitString [2]) == 0) {
							basePosition.transform.position = bot.transform.position;
							basePosition.transform.rotation = bot.transform.rotation;
							busy = false;
							//server.SendData("Hello from Unity", "Bot1");
						}


					} catch {
						Debug.LogError ("Argument error (too few?)");
					}



				}
			}
        }

    }


}