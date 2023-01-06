using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CoronationSceneScript : MonoBehaviour
{
    public CoronationDialogue[] dialogue;

    private int dialogueIndex;
    private bool dialogueFinished;

    private PlayerControls controls;

    private GameObject rajaBubble;
    private GameObject aashaaBubble;
    private GameObject bakwasBubble;

    private GameObject spaceBar;
    private GameObject eastObject;

    private TextMeshProUGUI rajaSpeech;
    private TextMeshProUGUI aashaaSpeech;
    private TextMeshProUGUI bakwasSpeech;

    TMP_Text textMesh;
    Mesh mesh;
    Vector3[] vertices;

    private void Awake()
    {
        rajaBubble = GameObject.Find("Raja Bubble");
        aashaaBubble = GameObject.Find("Aashaa Bubble");
        bakwasBubble = GameObject.Find("Bakwas Bubble");

        spaceBar = GameObject.Find("Space Bar");
        eastObject = GameObject.Find("East");

        rajaSpeech = rajaBubble.GetComponentInChildren<TextMeshProUGUI>();
        bakwasSpeech = bakwasBubble.GetComponentInChildren<TextMeshProUGUI>();
        aashaaSpeech = aashaaBubble.GetComponentInChildren<TextMeshProUGUI>();

        AudioManagerScript.instance.Play("day sounds");

        controls = new PlayerControls();
        controls.Gameplay.Interact.performed += ctx => nextEvent();
    }

    private void Start()
    {
        rajaBubble.SetActive(false);
        aashaaBubble.SetActive(false);
        bakwasBubble.SetActive(false);
        textMesh = null;

        dialogueIndex = 0;
        dialogueFinished = false;
    }

    private void Update()
    {
        if(textMesh != null)
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

        if(!dialogueFinished)
        {
            if(dialogue[dialogueIndex].name == "Raja" && rajaSpeech.text != "")
            {
                int randomNum = Random.Range(1, 4);
                if (!AudioManagerScript.instance.isPlaying("deep male blah 1") && !AudioManagerScript.instance.isPlaying("deep male blah 2") && !AudioManagerScript.instance.isPlaying("deep male blah 3"))
                {
                    AudioManagerScript.instance.sounds[23 + randomNum].pitch = Random.Range(0.9f, 1.1f);
                    AudioManagerScript.instance.Play("deep male blah " + randomNum);
                }
            }
            else if (dialogue[dialogueIndex].name == "Aashaa" && aashaaSpeech.text != "")
            {
                int randomNum = Random.Range(1, 4);
                if (!AudioManagerScript.instance.isPlaying("female blah 1") && !AudioManagerScript.instance.isPlaying("female blah 2") && !AudioManagerScript.instance.isPlaying("female blah 3"))
                {
                    AudioManagerScript.instance.sounds[26 + randomNum].pitch = Random.Range(0.9f, 1.1f);
                    AudioManagerScript.instance.Play("female blah " + randomNum);
                }
            }
            else if (dialogue[dialogueIndex].name == "Bakwas" && bakwasSpeech.text != "")
            {
                int randomNum = Random.Range(1, 4);
                if (!AudioManagerScript.instance.isPlaying("male blah 1") && !AudioManagerScript.instance.isPlaying("male blah 2") && !AudioManagerScript.instance.isPlaying("male blah 3"))
                {
                    AudioManagerScript.instance.sounds[20 + randomNum].pitch = Random.Range(0.9f, 1.1f);
                    AudioManagerScript.instance.Play("male blah " + randomNum);
                }
            }

            spaceBar.SetActive(false);
            eastObject.SetActive(false);
        }
        else
        {
            spaceBar.SetActive(true);
            eastObject.SetActive(true);
        }
    }

    public void aashaaEntered()
    {
        StartCoroutine(displayDialogue());
    }

    public void groundShake()
    {
        CameraShakeScript.shake.shakeCamera(1.5f, 4.1f);
    }

    public void groundShook()
    {
        StopAllCoroutines();
        dialogueIndex++;
        StartCoroutine(displayDialogue());
    }

    public void changeScene()
    {
        AudioManagerScript.instance.Stop("day sounds");
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void nextEvent()
    {
        if(dialogueFinished)
        {
            if (dialogueIndex < 21)
            {
                StopAllCoroutines();
                dialogueIndex++;
                StartCoroutine(displayDialogue());

                if (dialogueIndex == 20)
                {
                    GameObject.Find("Bakwas Exit").GetComponent<PlayableDirector>().Play();
                }
            }
            else if (dialogueIndex == 22)
            {
                StopAllCoroutines();
                GameObject.Find("Aashaa Falls").GetComponent<PlayableDirector>().Play();

                rajaBubble.SetActive(false);
                aashaaBubble.SetActive(false);
                bakwasBubble.SetActive(false);
                textMesh = null;

                rajaSpeech.text = "";
                aashaaSpeech.text = "";
                bakwasSpeech.text = "";

                spaceBar.SetActive(false);
                eastObject.SetActive(false);
            }
            else
            {
                GameObject.Find("Ground Breaks").GetComponent<PlayableDirector>().Play();

                rajaBubble.SetActive(false);
                aashaaBubble.SetActive(false);
                bakwasBubble.SetActive(false);
                textMesh = null;

                rajaSpeech.text = "";
                aashaaSpeech.text = "";
                bakwasSpeech.text = "";

                spaceBar.SetActive(false);
                eastObject.SetActive(false);
            }
        }
    }

    IEnumerator displayDialogue()
    {
        rajaBubble.SetActive(false);
        aashaaBubble.SetActive(false);
        bakwasBubble.SetActive(false);
        textMesh = null;

        rajaSpeech.text = "";
        aashaaSpeech.text = "";
        bakwasSpeech.text = "";

        dialogueFinished = false;

        rajaSpeech.fontSize = aashaaSpeech.fontSize = bakwasSpeech.fontSize = dialogue[dialogueIndex].font;

        if(dialogue[dialogueIndex].center)
        {
            rajaSpeech.alignment = aashaaSpeech.alignment = bakwasSpeech.alignment = TextAlignmentOptions.Midline;
        }
        else
        {
            rajaSpeech.alignment = aashaaSpeech.alignment = bakwasSpeech.alignment = TextAlignmentOptions.MidlineLeft;
        }

        if (dialogue[dialogueIndex].name == "Raja")
        {
            rajaBubble.SetActive(true);

            if(dialogue[dialogueIndex].quirky)
            {
                textMesh = rajaSpeech.GetComponent<TMP_Text>();
            }

            foreach (char letter in dialogue[dialogueIndex].phrase.ToCharArray())
            {
                yield return new WaitForSeconds(0.05f);
                rajaSpeech.text += letter;
                yield return null;
            }
        }
        else if (dialogue[dialogueIndex].name == "Aashaa")
        {
            aashaaBubble.SetActive(true);

            if(dialogue[dialogueIndex].quirky)
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
        else if (dialogue[dialogueIndex].name == "Bakwas")
        {
            bakwasBubble.SetActive(true);

            if(dialogue[dialogueIndex].quirky)
            {
                textMesh = bakwasSpeech.GetComponent<TMP_Text>();
            }

            foreach (char letter in dialogue[dialogueIndex].phrase.ToCharArray())
            {
                yield return new WaitForSeconds(0.075f);
                bakwasSpeech.text += letter;
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
