using UnityEngine;
using System.Collections;

public class LoadingCollider : MonoBehaviour {

    MechBehaviour mech;
    AmmoBehaviour cell;
	// Use this for initialization
	void Start () {
        mech = this.GetComponent<Transform>().parent.parent.parent.GetComponent<MechBehaviour>();
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnergyCell")
        {
            cell = other.gameObject.GetComponent<AmmoBehaviour>();
            if (!mech.AmmoFull())
            {
                mech.AddAmmo(cell.GetValue());
                Destroy(other.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update () {
	
	}
}
