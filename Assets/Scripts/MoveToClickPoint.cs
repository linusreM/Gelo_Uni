using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;

public class MoveToClickPoint : MonoBehaviour {
	NavMeshAgent agent;
	public GameObject MoveMarker;
	public move Mover;
	public Server Server;
	public bool turn;
	public int cornerCount;
	public Vector3[] corners; 
	public bool activateFlag;
	public bool exitFlag;
	public bool timeFlag;
	public float time;
	public LineRenderer line;
	public NavMeshPath path;
   
    


	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.updatePosition = false;

		time = 0;
		timeFlag = true;

		line = gameObject.AddComponent<LineRenderer> ();
		line.material = new Material (Shader.Find ("Standard"));
		line.material.EnableKeyword ("_EMISSION");
		line.material.SetColor ("_EmissionColor", Color.yellow);
		line.widthMultiplier = 1.0f;
		line.positionCount = 0;

	}

	void Update() {
		
		agent.nextPosition = Mover.bot.transform.position;


		if(Input.GetKeyUp("c")){
			activateFlag = true;
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			activateFlag = false;
		}

		if (cornerCount == corners.Length && !Mover.busy) {
			line.positionCount = 0;
		}
		

		if (!Mover.busy) {
			time += Time.deltaTime;
			
			if (Input.GetMouseButtonDown (0) && activateFlag) {

				//NavMeshPath path = new NavMeshPath ();
				RaycastHit hit;

				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 1000)) {
					GetPath (hit.point);

					/*agent.CalculatePath (hit.point, path);
					corners = path.corners;
					cornerCount = 1;
					activateFlag = false;
					line.positionCount = corners.Length;


					for (int i = 0; i < corners.Length; i++) {
						line.SetPosition (i, corners [i]);
						//Debug.DrawLine (path.corners [i], path.corners [i + 1], Color.red, 10);
					}*/
				}
			}
			 

			FollowPath (path);
			/*
			if (corners.Length != 0 && cornerCount <= corners.Length - 1) {

				if (time > 0.1) {
					
					if (turn) {
						Debug.Log ("HERE");
						Debug.Log (corners [cornerCount]);
						MoveMarker.transform.position = transform.position;
						MoveMarker.transform.LookAt (corners [cornerCount]);

						float angle = MoveMarker.transform.localEulerAngles.y;
						if (angle > 180) {
							angle = angle - 360;
						}
							
						Debug.Log (MoveMarker.transform.localEulerAngles.y);
						string message = "BOT1#" + "TURN#" + angle.ToString () + "$";
						Debug.Log (message);
						Mover.busy = true;
						turn = false;
						Server.SendData (message);
						time = 0;

					} else {
						MoveMarker.transform.position = corners [cornerCount];
						Debug.Log (MoveMarker.transform.localPosition.z * Mover.divisor);
						string message = "BOT1#" + "MOVE#" + ((MoveMarker.transform.localPosition.z) * Mover.divisor).ToString () + "$";
						Debug.Log (message);
						Mover.busy = true;
						turn = true;
						cornerCount++;
						Server.SendData (message);
						time = 0;

					}
				}
			}*/

		}
	}

	public void GetPath(Vector3 point){
		NavMeshPath path = new NavMeshPath ();

		agent.CalculatePath (point, path);
		corners = path.corners;
		cornerCount = 1;
		activateFlag = false;
		line.positionCount = corners.Length;

		for (int i = 0; i < corners.Length; i++) {
			line.SetPosition (i, corners [i]);
		}
	}
    public void GetPath(Vector3 point, Vector3 look)
    {
        NavMeshPath path = new NavMeshPath();

        agent.CalculatePath(point, path);
        corners = path.corners;
        List<Vector3> tempList = corners.ToList();
        tempList.Add(look);
        corners = tempList.ToArray();
        cornerCount = 1;
        activateFlag = false;
        line.positionCount = corners.Length;

        for (int i = 0; i < corners.Length; i++)
        {
            line.SetPosition(i, corners[i]);
        }
    }

    public void FollowPath(NavMeshPath path){
		if (corners.Length != 0 && cornerCount <= corners.Length - 1) {

			if (time > 0.1) {

				if (turn) {
					Debug.Log ("HERE");
					Debug.Log (corners [cornerCount]);
					MoveMarker.transform.position = transform.position;
					MoveMarker.transform.LookAt (corners [cornerCount]);

					float angle = MoveMarker.transform.localEulerAngles.y;
					if (angle > 180) {
						angle = angle - 360;
					}

					Debug.Log (MoveMarker.transform.localEulerAngles.y);
					string message = "BOT1#" + "TURN#" + angle.ToString () + "$";
					Debug.Log (message);
					Mover.busy = true;
					turn = false;
					Server.SendData (message);
					time = 0;

				} else {
					MoveMarker.transform.position = corners [cornerCount];
					Debug.Log (MoveMarker.transform.localPosition.z * Mover.divisor);
					string message = "BOT1#" + "MOVE#" + ((MoveMarker.transform.localPosition.z) * Mover.divisor).ToString () + "$";
					Debug.Log (message);
					Mover.busy = true;
					turn = true;
					cornerCount++;
					Server.SendData (message);
					time = 0;

				}
			}
		}
	}
}