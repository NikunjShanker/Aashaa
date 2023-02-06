using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BeginningCutSceneController : MonoBehaviour
{
    private PlayableDirector openingCutscene;

    // props
    private GameObject aashaaDress;
    private GameObject aashaaProp;
    private GameObject armorUpgrade;

    private void Awake()
    {
        openingCutscene = GameObject.Find("Beginning Cutscene").GetComponent<PlayableDirector>();

        aashaaDress = GameObject.Find("Aashaa Dress");
        aashaaProp = GameObject.Find("Aashaa Prop");
        armorUpgrade = GameObject.Find("Armor Upgrade Object");

        if (UniversalScript.instance.cutsceneProgress == 0)
        {
            openingCutscene.Play();
            UniversalScript.instance.cutsceneProgress = 1;
        }
        else
        {
            Destroy(aashaaDress);
            Destroy(aashaaProp);
            Destroy(armorUpgrade);
        }
    }
}
