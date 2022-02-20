using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;

public class SpikeSignScript : MonoBehaviour
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
            signText.text = "WATCH OUT! Spikes can hurt you with just a touch.";

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
