using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class UpgradeCanvasManager : MonoBehaviour
{
    public static UpgradeCanvasManager instance;
    private PlayerMovement pmController;

    // Parts of the canvas
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI continueText;

    public SpriteRenderer upgradeIcon;
    [SerializeField] public Sprite wallJump;
    [SerializeField] public Sprite jumpBoost;
    [SerializeField] public Sprite speedBoost;
    [SerializeField] public Sprite doubleJump;
    [SerializeField] public Sprite dash;
    [SerializeField] public Sprite heart;

    private PlayableDirector offDir;
    private PlayableDirector onDir;

    private bool canvasTimerSatisfied;

    private void Awake()
    {
        if(instance == null) instance = this;
        pmController = GameObject.Find("Aashaa").GetComponent<PlayerMovement>();
    }

    void Start()
    {
        titleText = GameObject.Find("Title Text").GetComponent<TextMeshProUGUI>();
        infoText = GameObject.Find("Information Text").GetComponent<TextMeshProUGUI>();
        continueText = GameObject.Find("Continue Text").GetComponent<TextMeshProUGUI>();

        upgradeIcon = GameObject.Find("Upgrade Icon").GetComponent<SpriteRenderer>();

        offDir = GameObject.Find("Canvas Off Director").GetComponent<PlayableDirector>();
        onDir = GameObject.Find("Canvas On Director").GetComponent<PlayableDirector>();
    }

    void Update()
    {
        if (PlayerMovement.instance.interact && titleText.text != "Upgrade Name")
        {
            if (canvasTimerSatisfied)
            {
                onDir.Stop();
                offDir.Play();
                pmController.playerActive = true;
                canvasTimerSatisfied = !canvasTimerSatisfied;
            }
        }
    }

    public void UpgradeCollected()
    {
        offDir.Stop();
        onDir.Play();
        pmController.playerActive = false;

        StartCoroutine(timelineTimer());
    }

    IEnumerator timelineTimer()
    {
        yield return new WaitForSeconds(1f);

        canvasTimerSatisfied = true;
    }
}
