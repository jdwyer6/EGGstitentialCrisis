using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour
{
    private int currentModeIdx = 0;
    public bool handEnabled = false;
    public bool buildEnabled = false;
    public bool sculptEnabled = false;

    private AudioManager am;

    private Color greenColor = new Color(92f/255f, 219f/255f, 168f/255f);
    private Color redColor = new Color(219f/255f, 92f/255f, 126f/255f);

    public Image[] icons;

    // Start is called before the first frame update
    void Start()
    {
        am = FindObjectOfType<AudioManager>();
        UpdateIcons();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentModeIdx = 0;
            SetMode(0);
            UpdateIcons();
            sculptEnabled = false;
            buildEnabled = false;
            handEnabled = true;
        }else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentModeIdx = 1;
            SetMode(1);
            UpdateIcons();
            sculptEnabled = false;
            buildEnabled = true;
            handEnabled = false;
        }else if (Input.GetKeyDown(KeyCode.Alpha3)) 
        {
            currentModeIdx = 2;
            SetMode(2);
            UpdateIcons();
            sculptEnabled = true;
            buildEnabled = false;
            handEnabled = false;
        }
    }

    private void SetMode(int modeIdx)
    {
        currentModeIdx = modeIdx;
        am.Play("CycleMode");
        if (currentModeIdx > icons.Length - 1)
        {
            currentModeIdx = 0;
        }
    }

    private void UpdateIcons()
    {
        for (int i = 0; i < icons.Length; i++)
        {
            icons[i].color = redColor;
            if(i == currentModeIdx)
            {
                icons[i].color = greenColor;
            }
        }

    }
}
