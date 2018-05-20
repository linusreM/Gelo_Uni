using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderPanel : MonoBehaviour {


    public bool activateFlag;
    public bool loaded;
    private Vector3 activePosition;
    private Vector3 disabledPosition;
    public move mover;
    public GameObject buttonPrefab;
    public RectTransform ParentPanel;
    public MoveToClickPoint click;

    // Use this for initialization
    void Start () {
        activePosition = transform.position;
        transform.position -= new Vector3(500, 0, 0);
        disabledPosition = transform.position;
        activateFlag = false;
	}
	
	// Update is called once per frame
	void Update () {
        if (activateFlag)
        {
            transform.position = activePosition;
            if (!loaded)
            {
                Debug.Log("Arucos: " + mover.arucos.Count);
                foreach (GameObject mark in mover.arucos)
                {
                    GameObject markerButton = (GameObject)Instantiate(buttonPrefab);
                    markerButton.transform.SetParent(ParentPanel, false);
                    markerButton.transform.localScale = new Vector3(1, 1, 1);
                    Button tempButton = markerButton.GetComponent<Button>();
                    //markerButton.GetComponent<Button>().GetComponent<Text>().text = mark.GetComponent<Marker>().id;
                    tempButton.GetComponentInChildren<Text>().text = "item " + mark.GetComponent<Marker>().id;
                    Vector3 markerPoint = mark.transform.GetChild(0).transform.position;
                    mark.transform.GetChild(0).transform.localPosition += new Vector3(0, 0, 1.0f / 25.6f);
                    Vector3 lookPoint = mark.transform.GetChild(0).transform.position;
                    lookPoint.y = 0;
                    markerPoint.y = 0;
                    tempButton.onClick.AddListener(() => MarkerPressed(markerPoint, lookPoint));
                }
                loaded = true;

            }
        }
        else
        {
            transform.position = disabledPosition;
            if (loaded)
            {
                foreach(Transform child in ParentPanel.transform)
                {
                    GameObject.Destroy(child.gameObject);
                    loaded = false;
                }

            }
        }
	}

    void MarkerPressed(Vector3 markerPosition, Vector3 lookPosition)
    {
        Debug.Log("marker pressed = " + markerPosition);
        click.GetPath(markerPosition, lookPosition);
        click.FollowPath(click.path);
    }
}
