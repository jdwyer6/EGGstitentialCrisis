using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MaterialSelector : MonoBehaviour
{
    private WorldMaterial[] materials;
    public int currentMaterialIdx = 0;
    public Image materialIcon;
    public TextMeshProUGUI materialText;
    private GameObject gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("gm");
        materials = gm.GetComponent<Data>().materials;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)) {
            currentMaterialIdx++;
            SetIcon();
        }
    }

    void SetIcon()
    {
        if (currentMaterialIdx > materials.Length - 1)
        {
            currentMaterialIdx = 0;
        }

        materialText.text = "Q " + materials[currentMaterialIdx].name;
        materialIcon.sprite = materials[currentMaterialIdx].icon;
    }
}
