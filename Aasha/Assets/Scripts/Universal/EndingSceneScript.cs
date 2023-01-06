using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingSceneScript : MonoBehaviour
{
    public CoronationDialogue[] dialogue;

    private int dialogueIndex;
    private bool dialogueFinished;

    private PlayerControls controls;

    private GameObject aashaaBubble;
    private GameObject spaceBar;
    private GameObject eastObject;
    private TextMeshProUGUI aashaaSpeech;

    private bool exitScene;

    TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;

    private void Awake()
    {
        aashaaBubble = GameObject.Find("Aashaa Bubble");
        aashaaSpeech = aashaaBubble.GetComponentInChildren<TextMeshProUGUI>();

        spaceBar = GameObject.Find("Space Bar");
        eastObject = GameObject.Find("East");

        AudioManagerScript.instance.Play("night sounds");

        controls = new PlayerControls();
        controls.Gameplay.Interact.performed += ctx => nextEvent();
    }

    private void Start()
    {
        aashaaBubble.SetActive(false);
        spaceBar.SetActive(false);
        eastObject.SetActive(false);
        textMesh = null;

        dialogueIndex = 0;
        dialogueFinished = false;

        exitScene = false;
    }

    private void Update()
    {
        if (textMesh != null)
        {
            textMesh.ForceMeshUpdate();
            mesh = textMesh.mesh;
            vertices = mesh.vertices;

            for (int i = 0; i < textMesh.textInfo.characterCount; i++)
            {
                TMP_CharacterInfo c = textMesh.textInfo.characterInfo[i];
                int index = c.vertexIndex;

                Vector3 offset = Wobble(Time.time + i);
                vertices[index] += offset;
                vertices[index + 1] += offset;
                vertices[index + 2] += offset;
                vertices[index + 3] += offset;
            }

            mesh.vertices = vertices;
            textMesh.canvasRenderer.SetMesh(mesh);
        }

        if (!dialogueFinished)
        {
            if (dialogue[dialogueIndex].name == "Aashaa" && aashaaSpeech.text != "")
            {
                int randomNum = Random.Range(1, 4);
                if (!AudioManagerScript.instance.isPlaying("female blah 1") && !AudioManagerScript.instance.isPlaying("female blah 2") && !AudioManagerScript.instance.isPlaying("female blah 3"))
                {
                    AudioManagerScript.instance.sounds[26 + randomNum].pitch = Random.Range(0.9f, 1.1f);
                    AudioManagerScript.instance.Play("female blah " + randomNum);
                }
                spaceBar.SetActive(false);
                eastObject.SetActive(false);
            }
        }
        else if (aashaaSpeech.text != "")
        {
            spaceBar.SetActive(true);
            eastObject.SetActive(true);
        }
    }

    public void aashaaEntered()
    {
        StartCoroutine(displayDialogue());
    }

    public void fadeToBlack()
    {
        PlayableDirector fadeOut = GameObject.Find("/Timeline/Fade Out").GetComponentInChildren<PlayableDirector>();
        fadeOut.Play();
    }

    public void playTheme()
    {
        AudioManagerScript.instance.Play("theme");
    }

    public void revealStats()
    {
        TextMeshProUGUI heartsText = GameObject.Find("/Stats Canvas/Hearts Stats").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI deathsText = GameObject.Find("/Stats Canvas/Deaths Stats").GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI timeText = GameObject.Find("/Stats Canvas/Time Stats").GetComponentInChildren<TextMeshProUGUI>();

        heartsText.text = "You Found " + (UniversalScript.instance.maxHealth - 3) + " out of 7 Extra Hearts";
        deathsText.text = "You Died " + UniversalScript.instance.deathCounter + " times";
        timeText.text = "Final Time: " + UniversalScript.instance.minutes + ":" + UniversalScript.instance.seconds + ":" + UniversalScript.instance.milliseconds;

        UniversalScript.instance.compareTimes();

        PlayableDirector statsReveal = GameObject.Find("/Timeline/Stats Reveal").GetComponentInChildren<PlayableDirector>();
        statsReveal.Play();

        aashaaSpeech.text = " ";
        spaceBar.SetActive(true);
        eastObject.SetActive(true);
    }

    public void readyToExit()
    {
        exitScene = true;
    }

    private void nextEvent()
    {
        if (dialogueFinished)
        {
            if (dialogueIndex < 7)
            {
                StopAllCoroutines();
                dialogueIndex++;
                StartCoroutine(displayDialogue());
            }
            else if(!exitScene)
            {
                AudioManagerScript.instance.Stop("night sounds");
                AudioManagerScript.instance.Play("laugh track");
                GameObject.Find("Credits Roll").GetComponent<PlayableDirector>().Play();
                aashaaBubble.SetActive(false);
                aashaaSpeech.text = "";
                spaceBar.SetActive(false);
                eastObject.SetActive(false);
                textMesh = null;
            }
        }

        if(exitScene)
        {
            exitScene = false;
            SceneManager.LoadSceneAsync(1);
        }
    }

    IEnumerator displayDialogue()
    {
        aashaaBubble.SetActive(false);
        textMesh = null;

        aashaaSpeech.text = "";

        dialogueFinished = false;

        aashaaSpeech.fontSize = dialogue[dialogueIndex].font;

        if (dialogue[dialogueIndex].center)
        {
            aashaaSpeech.alignment = TextAlignmentOptions.Midline;
        }
        else
        {
            aashaaSpeech.alignment = TextAlignmentOptions.MidlineLeft;
        }

        if (dialogue[dialogueIndex].name == "Aashaa")
        {
            aashaaBubble.SetActive(true);

            if (dialogue[dialogueIndex].quirky)
            {
                textMesh = aashaaSpeech.GetComponent<TMP_Text>();
            }

            foreach (char letter in dialogue[dialogueIndex].phrase.ToCharArray())
            {
                yield return new WaitForSeconds(0.075f);
                aashaaSpeech.text += letter;
                yield return null;
            }
        }

        dialogueFinished = true;
    }

    Vector2 Wobble(float time)
    {
        return new Vector2(Mathf.Sin(time * 3.3f), Mathf.Cos(time * 2f));
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
}
