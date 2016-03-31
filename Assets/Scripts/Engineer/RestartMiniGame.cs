using UnityEngine;
using System.Collections;

public class RestartMiniGame : MonoBehaviour {

    public GameObject panel;
    public MechBehaviour mech;
    private const int maxButtons=6;
    public GameObject[] buttons = new GameObject[maxButtons];
    private Vector3[] startPos = new Vector3[maxButtons];
    private int next;
    // Use this for initialization
    void Start () {
    }

    public void Setup()
    {
        float w = panel.GetComponent<Transform>().localScale.x * panel.gameObject.GetComponent<RectTransform>().rect.width;
        float h = panel.GetComponent<Transform>().localScale.y * panel.gameObject.GetComponent<RectTransform>().rect.height;
        for (int x = 0; x < buttons.Length; x++)
        {
            startPos[x] = buttons[x].GetComponent<Transform>().localPosition;
        }
        next = 1;
        int[] taken = new int[maxButtons];
        for (int x = 0; x < maxButtons; x++)
        {
            int id;
            do {
                id = (int)Mathf.Ceil(Random.value * maxButtons);
            }
                while (System.Array.IndexOf(taken,id )>=0) ;
            taken[x] = id;
            buttons[id - 1].GetComponent<UnityEngine.UI.Button>().interactable = true;
            buttons[id-1].GetComponent<Transform>().localPosition = startPos[x];
        }
    }
	
    public void Click(int id)
    {
        if (next == id)
        {
            buttons[id-1].GetComponent<UnityEngine.UI.Button>().interactable = false;
            if (next == maxButtons)
            {
                EndGame();
            }
            next++;
        }
        else
        {
            Setup();
        }
    }

    void EndGame()
    {
        mech.reboot();
    }

    // Update is called once per frame
    void Update () {
	
	}
}
