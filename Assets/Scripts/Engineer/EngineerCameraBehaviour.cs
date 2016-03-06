using UnityEngine;
using System.Collections;

public class EngineerCameraBehaviour : MonoBehaviour
{

    float dist;
    Transform toDrag;
    bool dragging = false;
    Vector3 offset;
    AmmoBehaviour scr;

    // Use this for initialization
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount > 0)
        //{
            Vector3 v3;
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began)
			//if (Input.GetMouseButtonDown(0))
			{
				//Destroy(Camera.main);
					RaycastHit hit;
					//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					Ray ray = Camera.main.ScreenPointToRay(touch.position);
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.transform.gameObject.tag == "EnergyCell") { 
                        //Destroy(hit.transform.gameObject);
                        scr = hit.transform.gameObject.GetComponent<AmmoBehaviour>();
                        toDrag = hit.transform;
                        dist = hit.transform.position.z - Camera.main.transform.position.z;
                        v3 = new Vector3(touch.position.x, touch.position.y, dist);
                        //v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                        v3 = Camera.main.ScreenToWorldPoint(v3);
                        //v3.z += 20;
                        //	offset = toDrag.position - v3;
                        //toDrag.position = offset;
                        dragging = true;
                        scr.SetSnapBack(false);
                        toDrag.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
                        //Destroy(Camera.main);
					}
					}
				}
			if (touch.phase == TouchPhase.Moved)
			//if (Input.GetMouseButton(0))
			{
					if (dragging)
					{
					v3 = new Vector3(touch.position.x, touch.position.y, dist);
					//v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
					v3 = Camera.main.ScreenToWorldPoint(v3);
						toDrag.position = v3 + offset;
					}
				}
			if (touch.phase == TouchPhase.Ended)
			//if (Input.GetMouseButtonUp(0))
			{
				if (dragging) {
				v3 = toDrag.position;
				//v3.z += 20;
				//toDrag.position = v3;
				dragging = false;
				toDrag.gameObject.GetComponent<Rigidbody>().detectCollisions = true;
				scr.SetSnapBack(true);
				}
			}
		}
			if (Input.GetMouseButtonDown(0))
			{
					RaycastHit hit;
					Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.transform.gameObject.tag == "EnergyCell")
						{
						scr= hit.transform.gameObject.GetComponent<AmmoBehaviour>();
							toDrag = hit.transform;
							dist = hit.transform.position.z - Camera.main.transform.position.z;
						v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
						v3 = Camera.main.ScreenToWorldPoint(v3);
						v3.z += 20;
							offset = toDrag.position - v3;
						toDrag.position = offset;
							dragging = true;
						scr.SetSnapBack(false);
					}
					}
				}
			if (Input.GetMouseButton(0))
			{
					if (dragging)
					{
					v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
					v3 = Camera.main.ScreenToWorldPoint(v3);
						toDrag.position = v3 + offset;
					}
				}
			if (Input.GetMouseButtonUp(0))
			{
				if (dragging) {
				v3 = toDrag.position;
				v3.z += 20;
				toDrag.position = v3;
				dragging = false;
				scr.SetSnapBack(true);
				}
					
			}
    }

}
