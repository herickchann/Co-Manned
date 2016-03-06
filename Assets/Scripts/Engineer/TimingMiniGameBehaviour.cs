using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimingMiniGameBehaviour : MonoBehaviour {

    public MechBehaviour mech;
    public Image bar1;
    public Image bar2;
    public Image bar3;
    public GameObject panel;
    private int bar1Dir;
    private int bar2Dir;
    private int bar3Dir;
    // Use this for initialization
    void Start () {
        bar1.GetComponent<Transform>().localScale = GetComponent<Transform>().localScale;
        bar2.GetComponent<Transform>().localScale = GetComponent<Transform>().localScale;
        bar1.fillAmount = 0;
        bar2.fillAmount = 0;
        bar3.fillAmount = 0;
        bar1Dir = 0;
        bar2Dir = 0;
        bar3Dir = 0;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (bar1.fillAmount > 0.85)
        {
            bar1.fillAmount += bar1Dir * (float)0.03;
            bar1.color = new Color(0, 1, 0);
        }
        else if (bar1.fillAmount > 0.5)
        {
            bar1.fillAmount += bar1Dir*(float)0.02;
            bar1.color = new Color(1, 1, 0);
        }
        else
        {
            bar1.fillAmount += bar1Dir * (float)0.012;
            bar1.color = new Color(1, 0, 0);
        }
        if (bar1.fillAmount >= 1)
        {
            bar1.fillAmount = 1;
            bar1Dir = -1;
        }
        else if (bar1.fillAmount <= 0 && bar1Dir !=0)
        {
            bar1.fillAmount = 0;
            bar1Dir = 1;
        }


        if (bar2.fillAmount > 0.85)
        {
            bar2.fillAmount += bar2Dir * (float)0.05;
            bar2.color = new Color(0, 1, 0);
        }
        else if (bar2.fillAmount > 0.5)
        {
            bar2.fillAmount += bar2Dir * (float)0.03;
            bar2.color = new Color(1, 1, 0);
        }
        else
        {
            bar2.fillAmount += bar2Dir * (float)0.012;
            bar2.color = new Color(1, 0, 0);
        }
        if (bar2.fillAmount >= 1)
        {
            bar2.fillAmount = 1;
            bar2Dir = -1;
        }
        else if (bar2.fillAmount <= 0 && bar2Dir != 0)
        {
            bar2.fillAmount = 0;
            bar2Dir = 1;
        }

        if (bar3.fillAmount > 0.85)
        {
            bar3.fillAmount += bar3Dir * (float)0.04;
            bar3.color = new Color(0, 1, 0);
        }
        else if (bar3.fillAmount > 0.5)
        {
            bar3.fillAmount += bar3Dir * (float)0.03;
            bar3.color = new Color(1, 1, 0);
        }
        else
        {
            bar3.fillAmount += bar3Dir * (float)0.018;
            bar3.color = new Color(1, 0, 0);
        }
        if (bar3.fillAmount >= 1)
        {
            bar3.fillAmount = 1;
            bar3Dir = -1;
        }
        else if (bar3.fillAmount <= 0 && bar3Dir != 0)
        {
            bar3.fillAmount = 0;
            bar3Dir = 1;
        }

    }

    public void StartBar(int barNo)
    {
        if (barNo == 0 && bar1Dir == 0)
        {
            bar1Dir = 1;
        }
        else if (barNo == 1 && bar2Dir == 0)
        {
            bar2Dir = 1;
        }
        else if (barNo == 2 && bar3Dir == 0)
        {
            bar3Dir = 1;
        }
    }

    public void EndGame()
    {
        double healthVal;
        double fuelVal;
        double ammoVal;
        if (bar1.fillAmount >= 0.85)
        {
            healthVal = 2;
        }
        else if  (bar1.fillAmount >= 0.5)
        {
            healthVal = 1;
        }
        else
        {
            healthVal = 0.5;
        }
        bar1.fillAmount = 0;
        bar1Dir = 0;

        if (bar2.fillAmount >= 0.85)
        {
            fuelVal = 2;
        }
        else if (bar2.fillAmount >= 0.5)
        {
            fuelVal = 1;
        }
        else
        {
            fuelVal = 0.5;
        }
        bar2.fillAmount = 0;
        bar2Dir = 0;

        if (bar3.fillAmount >= 0.85)
        {
            ammoVal = 2;
        }
        else if (bar3.fillAmount >= 0.5)
        {
            ammoVal = 1;
        }
        else
        {
            ammoVal = 0.5;
        }
        bar3.fillAmount = 0;
        bar3Dir = 0;

        mech.Convert(healthVal,fuelVal, ammoVal);

        //Destroy(panel);
    }
}
