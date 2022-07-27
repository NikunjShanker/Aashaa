using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class CoronationSceneScript : MonoBehaviour
{
    public CoronationDialogue[] dialogue;

    private int dialogueIndex;

    private PlayerControls controls;

    private GameObject rajaBubble;
    private GameObject aashaaBubble;
    private GameObject bakwasBubble;

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

        rajaSpeech = rajaBubble.GetComponentInChildren<TextMeshProUGUI>();
        bakwasSpeech = bakwasBubble.GetComponentInChildren<TextMeshProUGUI>();
        aashaaSpeech = aashaaBubble.GetComponentInChildren<TextMeshProUGUI>();

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
    }

    public void aashaaEntered()
    {
        StartCoroutine(displayDialogue());
    }

    private void nextEvent()
    {
        StopAllCoroutines();
        dialogueIndex++;
        StartCoroutine(displayDialogue());
    }

    IEnumerator displayDialogue()
    {
        rajaBubble.SetActive(false);
        aashaaBubble.SetActive(false);
        bakwasBubble.SetActive(false);
        textMesh = null;

        rajaSpeech.fontSize = aashaaSpeech.fontSize = bakwasSpeech.fontSize = dialogue[dialogueIndex].font;

        if (dialogue[dialogueIndex].name == "Raja")
        {
            rajaBubble.SetActive(true);
            rajaSpeech.text = "";

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
            aashaaSpeech.text = "";

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
            bakwasSpeech.text = "";

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
