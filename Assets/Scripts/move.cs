﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public bool mapping = true;
	public bool newMarker = true;
	public GameObject correctionMarker;
	public GameObject correctionChild;
	public GameObject aruco;
	public List<GameObject> arucos;
    public float fall;
    public int sequenceNumber = 0;


    //Vector3 basePosition;
	void Start()
	{
		arucos = new List<GameObject>();
		correctionMarker = new GameObject ();
		correctionChild = new GameObject ();


	}
    // Update is called once per frame
    void Update()
	{
		string[] packets = Server.allReceivedUDPPackets.Split ('$'); 
		int packetsToHandle = Server.unhandledPackets;
		Server.unhandledPackets = 0;
		Server.allReceivedUDPPackets = "";
		if (Server.lastRecivedFromIP != "" && packetsToHandle != 0) {
			//Debug.Log("Last message from IP: " + Server.lastRecivedFromIP);
        
			for (int i = 0; i < packetsToHandle; i++) { //Handle all packets that has been received since last Update()

				string[] splitString; //Message container

				if (packets [i] != "") {
					splitString = packets [i].Split ('#');   //Split messages by delimiter #

                
					if (splitString [1] == "QR") {
						Debug.Log ("Found QR: " + splitString [2]);
					
						newMarker = true;
						foreach (GameObject marker in arucos) {
							if (marker.GetComponent<Marker> ().id == splitString [2]) {
								Debug.Log ("Marker exists");
								newMarker = false;
								break;
							}
						}
							
						if (newMarker) {  // if unique
							qr.transform.localPosition = new Vector3 (float.Parse (splitString [3]) / divisor, -(float.Parse (splitString [4])) / divisor, (float.Parse (splitString [5]) + 95.0f) / divisor);

							float xRotation = float.Parse (splitString [6]);
							float yRotation = float.Parse (splitString [7]);
							float zRotation = float.Parse (splitString [8]);
							float theta = (Mathf.Sqrt ((xRotation * xRotation) + (yRotation * yRotation) + (zRotation * zRotation)) * (180 / Mathf.PI));
							Vector3 axis = new Vector3 (-xRotation, yRotation, -zRotation);
							Quaternion rotation = Quaternion.AngleAxis (theta, axis);
							qr.transform.localRotation = rotation;

							arucos.Add (Instantiate (aruco, qr.transform.position, qr.transform.rotation));
							arucos.Last ().GetComponent<Marker> ().id = (splitString [2]);
						} else {
                            /*==========>
							// skapa child på aktuell markör
							GameObject selectedMarker = arucos.Find (x => x.GetComponent<Marker> ().id == splitString [2]);

							//correctionMarker = new GameObject ();
							//correctionChild = new GameObject ();

							//Debug.Log(arucos.Find(x => x.GetComponent<Marker>().id == splitString[2]).transform.position);
							//correctionMarker.transform.parent = (arucos.Find(x => x.GetComponent<Marker>().id == splitString[2])).transform;
							//correctionChild = new GameObject ();
							//correctionChild.transform.parent = correctionMarker.transform;
							Vector3 newposition = new Vector3 (float.Parse (splitString [3]) / divisor, -(float.Parse (splitString [4])) / divisor, (float.Parse (splitString [5]) + 95.0f) / divisor);
							correctionMarker.transform.parent = bot.transform;
							correctionMarker.transform.localPosition = newposition;


							float xRotation = float.Parse (splitString [6]);
							float yRotation = float.Parse (splitString [7]);
							float zRotation = float.Parse (splitString [8]);
							float theta = (Mathf.Sqrt ((xRotation * xRotation) + (yRotation * yRotation) + (zRotation * zRotation)) * (180 / Mathf.PI));
							Vector3 axis = new Vector3 (-xRotation, yRotation, -zRotation);
							Quaternion correctionRotation = Quaternion.AngleAxis (theta, axis);

							correctionMarker.transform.localRotation = correctionRotation;
							correctionChild.transform.parent = correctionMarker.transform;
							correctionChild.transform.position = bot.transform.position;
							correctionChild.transform.rotation = bot.transform.rotation;

							correctionMarker.transform.position = selectedMarker.transform.position;
							correctionMarker.transform.rotation = selectedMarker.transform.rotation;

							bot.transform.position = correctionChild.transform.position;
							bot.transform.rotation = correctionChild.transform.rotation;

							//correctionMarker.transform.localScale = new Vector3 (-1, -1, -1);
							//Vector3 correctionPosition = transform.InverseTransformPoint(newposition);
							//Vector3 correctionPosition = newposition;
							Debug.Log (newposition);
							//(float.Parse (splitString [3]), -(float.Parse (splitString [4])), (float.Parse (splitString [5]) + 95.0f)));
							//Debug.Log (correctionPosition);
							//correctionMarker.transform.localPosition = correctionPosition;

                            <========*/
							/*float xRotation = float.Parse (splitString [6]);
							float yRotation = float.Parse (splitString [7]);
							float zRotation = float.Parse (splitString [8]);
							float theta = (Mathf.Sqrt ((xRotation * xRotation) + (yRotation * yRotation) + (zRotation * zRotation)) * (180 / Mathf.PI));
							Vector3 axis = new Vector3 (-xRotation, yRotation, -zRotation);
							Quaternion correctionRotation = Quaternion.Inverse(Quaternion.AngleAxis (theta, axis));


							correctionMarker.transform.localRotation = correctionRotation;
							correctionMarker.transform.localPosition = correctionPosition;*/

							Debug.Log ("Moving bot");

							//bot.transform.position = correctionChild.transform.position;
							//bot.transform.rotation = correctionChild.transform.rotation;
							// hämta markörens tvec rvec
							// beräkna invers
							// sätt child local till invers
							// sätt robot transform till child transform
                            

						}

					} else if (splitString [1] == "0" || splitString [1] == "1" || splitString [1] == "MOVE" || splitString [1] == "TURN") {  //Movement handler

						// Debug.Log(splitString[0] + " sent " + splitString[1] + ", More:" + splitString[2] + ", Travel: " + splitString[3] + ", with: " + splitString[4] + "F, " + splitString[5] + "B, " + splitString[6] + "L, " + splitString[7] + "R");

						if (splitString [1] == "0" || splitString [1] == "MOVE") {
							bot.transform.position = basePosition.transform.position;
							bot.transform.rotation = basePosition.transform.rotation;
							bot.transform.Translate (Vector3.forward * (float.Parse (splitString [3]) / divisor));
						} else if (splitString [1] == "1" || splitString [1] == "TURN") {
							bot.transform.position = basePosition.transform.position;
							bot.transform.rotation = basePosition.transform.rotation;
							bot.transform.Rotate (new Vector3 (0, 1, 0) * (float.Parse (splitString [3])));
						}

						right.transform.localPosition = new Vector3 (float.Parse (splitString [7]) / divisor, 0.0f, 0.0f);
						left.transform.localPosition = new Vector3 (-(float.Parse (splitString [6]) / divisor), 0.0f, 0.0f);
						forward.transform.localPosition = new Vector3 (0.0f, 0.0f, float.Parse (splitString [4]) / divisor);
						back.transform.localPosition = new Vector3 (0.0f, 0.0f, -(float.Parse (splitString [5]) / divisor));
						float forwardStrength = DistanceToStrength (float.Parse (splitString [4]), fall);
						float backStrength = DistanceToStrength (float.Parse (splitString [5]), fall);
						float leftStrength = DistanceToStrength (float.Parse (splitString [6]), fall);
						float rightStrength = DistanceToStrength (float.Parse (splitString [7]), fall);
						Vector2 shForward = new Vector2 (forward.transform.position.x, forward.transform.position.z);
						Vector2 shBack = new Vector2 (back.transform.position.x, back.transform.position.z);
						Vector2 shLeft = new Vector2 (left.transform.position.x, left.transform.position.z);
						Vector2 shRight = new Vector2 (right.transform.position.x, right.transform.position.z);

						if (mapping)
							cShader.RunShader4 (shForward, shBack, shLeft, shRight, forwardStrength, backStrength, leftStrength, rightStrength);

						if (splitString [2] == "0") {
                            if (sequenceNumber <= int.Parse(splitString[0]))
                            {
                                basePosition.transform.position = bot.transform.position;
                                basePosition.transform.rotation = bot.transform.rotation;
                                
                                Debug.Log("Finished movement, position locked!");
                                Debug.Log(splitString[0] + " sent " + splitString[1] + ", More:" + splitString[2] + ", Travel: " + splitString[3] + ", with: " + splitString[4] + "F, " + splitString[5] + "B, " + splitString[6] + "L, " + splitString[7] + "R");
                                sequenceNumber = int.Parse(splitString[0]) + 1;

                                busy = false;
                            }
                            server.SendData("ACK");
							
						}
					} else {
						Debug.Log ("Unrecognized Command: " + splitString [1] + ", From: " + splitString [0]);
					}
				}
			}
		}
	}
    private float DistanceToStrength(float distance, float falloff)
    {
        float ret = Mathf.Exp(-(distance*falloff));
        if (ret > 1)
            ret = 1;
        return ret;
    }
}
