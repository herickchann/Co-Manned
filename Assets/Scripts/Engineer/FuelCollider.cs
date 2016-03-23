using UnityEngine;
using System.Collections;

public class FuelCollider : MonoBehaviour
{

    MechBehaviour mech;
    AmmoBehaviour cell;
    // Use this for initialization
    void Start()
    {
        mech = this.GetComponent<Transform>().parent.parent.parent.GetComponent<MechBehaviour>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnergyCell")
        {
            cell = other.gameObject.GetComponent<AmmoBehaviour>();
                mech.Load(cell.GetValue(), 1);
                Destroy(other.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
