using UnityEngine;
using System.Collections;

public class HealthCollider : MonoBehaviour
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
            if (!mech.HealthFull())
            {
                //mech.AddHealth(cell.GetValue()*25);
                mech.Load(cell.GetValue(), 0);
                Destroy(other.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
