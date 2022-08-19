using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButtonManager : MonoBehaviour
{
    [SerializeField]
    private Sprite muteSprite;
    [SerializeField]
    private Sprite unmuteSprite;

    private Button muteButton;

    private void Start()
    {
        muteButton = this.GetComponent<Button>();
    }

    private void Update()
    {
        if (AudioManagerScript.instance.mute) muteButton.image.sprite = muteSprite;
        else muteButton.image.sprite = unmuteSprite;
    }

    public void MuteButton()
    {
        AudioManagerScript.instance.Mute();
    }
}
