using UnityEngine;
using System.Collections;

public class AmmoBehaviour : MonoBehaviour {

    int type;
    public int index;
    public MechBehaviour mech;
    Transform t;
    Vector3 startPos;
    bool snapback;
    float nextSnapBack;
    float snapBackDelay = (float)0.02;


	// Use this for initialization
	void Start () {
        nextSnapBack = 0;
        snapback = true;
        type = 1;
	    t = GetComponent<Transform>();
        startPos = t.position;
    }
	
	// Update is called once per frame
	void Update () {
        if (snapback && Time.time > nextSnapBack)
        {
            t.position = startPos;
        }
	}

    public void SetSnapBack(bool snap)
    {
        nextSnapBack = Time.time + snapBackDelay;
        snapback = snap;
    }

    public int GetValue()
    {
        return type;
    }

    void OnDestroy()
    {
        mech.DeleteCell(index);
    }
}
