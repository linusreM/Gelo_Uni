using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

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

	void Start() {
		agent = GetComponent<NavMeshAgent>();
		agent.updatePosition = false;
		 
	}

	void Update() {

		if(Input.GetKeyUp("c")){
			activateFlag = true;
		}

		if (Input.GetKeyDown(KeyCode.Escape)){
			activateFlag = false;
		}
		

		if (!Mover.busy) {
			
			if (Input.GetMouseButtonDown (0) && activateFlag) {

				NavMeshPath path = new NavMeshPath ();
				RaycastHit hit;

				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 1000)) {

					agent.CalculatePath (hit.point, path);
					corners = path.corners;
					cornerCount = 1;
					activateFlag = false;


					for (int i = 0; i < path.corners.Length - 1; i++) {
						Debug.DrawLine (path.corners [i], path.corners [i + 1], Color.red, 10);
					}
				}
			}

			if (corners.Length != 0 && cornerCount <= corners.Length - 1) {
				
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
					string message = "BOT1#" + "TURN#" + angle.ToString() + "$";
					Debug.Log (message);
					Mover.busy = true;
					turn = false;
					Server.SendData (message);

				} else {
					MoveMarker.transform.position = corners [cornerCount];
					Debug.Log (MoveMarker.transform.localPosition.z * Mover.divisor);
					string message = "BOT1#" + "MOVE#" + ((MoveMarker.transform.localPosition.z) * Mover.divisor).ToString() + "$";
					Debug.Log (message);
					Mover.busy = true;
					turn = true;
					cornerCount++;
					Server.SendData (message);

				}
			}
		}
	}
}