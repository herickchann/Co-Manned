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
            Vector3 v3;
		foreach (Touch touch in Input.touches) {
			if (touch.phase == TouchPhase.Began) { 
                RaycastHit hit;
            Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "EnergyCell")
                {
                    scr = hit.transform.gameObject.GetComponent<AmmoBehaviour>();
                    toDrag = hit.transform;
                    dist = hit.transform.position.z - gameObject.GetComponent<Camera>().transform.position.z;
                    v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                    v3 = gameObject.GetComponent<Camera>().ScreenToWorldPoint(v3);
                    v3.z += 20;
                    offset = toDrag.position - v3;
                    toDrag.position = offset;
                    dragging = true;
                    scr.SetSnapBack(false);
                }
            }
        }
			if (touch.phase == TouchPhase.Moved)
			{
                if (dragging)
                {
                    v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
                    v3 = gameObject.GetComponent<Camera>().ScreenToWorldPoint(v3);
                    toDrag.position = v3 + offset;
                }
            }
			if (touch.phase == TouchPhase.Ended)
			{
                if (dragging)
                {
                    v3 = toDrag.position;
                    v3.z += 20;
                    toDrag.position = v3;
                    dragging = false;
                    scr.SetSnapBack(true);
                }
            }
		}
			if (Input.GetMouseButtonDown(0))
			{
					RaycastHit hit;
					Ray ray = gameObject.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
					if (Physics.Raycast(ray, out hit))
					{
						if (hit.transform.gameObject.tag == "EnergyCell")
						{
						scr= hit.transform.gameObject.GetComponent<AmmoBehaviour>();
							toDrag = hit.transform;
							dist = hit.transform.position.z - gameObject.GetComponent<Camera>().transform.position.z;
						v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
						v3 = gameObject.GetComponent<Camera>().ScreenToWorldPoint(v3);
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
					v3 = gameObject.GetComponent<Camera>().ScreenToWorldPoint(v3);
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
