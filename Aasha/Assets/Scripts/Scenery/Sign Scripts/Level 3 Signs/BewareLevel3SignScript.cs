﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class BewareLevel3SignScript : MonoBehaviour
{
    private PlayableDirector onDir;
    private PlayableDirector offDir;

    private TextMeshProUGUI signText;

    private void Start()
    {
        offDir = GameObject.Find("Sign Canvas Off Director").GetComponent<PlayableDirector>();
        onDir = GameObject.Find("Sign Canvas On Director").GetComponent<PlayableDirector>();

        signText = GameObject.Find("Sign Text").GetComponent<TextMeshProUGUI>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            signText.text = "Beware! The following jump is extremely dangerous and takes pure skill to manuever.";

            offDir.Stop();
            onDir.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            onDir.Stop();
            offDir.Play();
        }
    }
}
