using System.Runtime.CompilerServices;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using Unity.Multiplayer.Center.Common;

public class TimingGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform movingImage;
    [SerializeField] private RectTransform targetAreaWide;
    [SerializeField] private RectTransform targetAreaPerfect;
    [SerializeField] private TMP_Text counterText;
    [SerializeField] private TMP_Text timeGainText;
    [SerializeField] private GameObject panelToDisable;

    [Header("Game Settings")]
    [SerializeField] private float speed = 300f;
    [SerializeField] int counterClienti = 5;

    [SerializeField] private float timeBonusWide = 0.25f;
    [SerializeField] private float timeBonusPerfect = 0.5f;

    [SerializeField] private float pauseDuration = 0.5f; // Durata della pausa dopo un colpo

    private float timeGain = 0f;
    private bool movingRight = true;
    private bool isPaused = false;
    private float leftLimit, rightLimit;

    void Start()
    {
        //Calcolo dei limiti del movimento
        float panelWidth = ((RectTransform)transform).rect.width;
        float halfImageWidth = movingImage.rect.width / 2;
        leftLimit = -panelWidth / 2 + halfImageWidth;
        rightLimit = panelWidth / 2 - halfImageWidth;

        panelToDisable.SetActive(false); // Disabilita il pannello inizialmente
    }


    void Update()
    {
        if (!panelToDisable.activeSelf || isPaused) return;

        MoveImage();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckHit();
        }
    }

    public void StartGame(int clienti, float speed)
    {
        counterClienti = clienti; // Imposta il numero di clienti
        this.speed = speed; // Imposta la velocità di movimento dell'immagine
        UpdateCounterUI();
        panelToDisable.SetActive(true); // Abilita il pannello di gioco
        movingImage.anchoredPosition = new Vector2(0, movingImage.anchoredPosition.y); // Reset della posizione dell'immagine
        movingRight = true; // Inizia a muovere l'immagine verso destra
        isPaused = false; // Assicurati che il gioco non sia in pausa
    }


    private void MoveImage()
    {
        float direction = movingRight ? 1 : -1;
        movingImage.anchoredPosition += new Vector2(direction * speed * Time.deltaTime, 0);

        if (movingImage.anchoredPosition.x >= rightLimit)
        {
            movingRight = false;
        }
        else if (movingImage.anchoredPosition.x <= leftLimit)
        {
            movingRight = true;
        }
    }

    private void CheckHit()
    {
        if (RectContains(targetAreaPerfect, movingImage))
        {
            timeGain += 0.5f; // Aggiungi tempo bonus
            StartCoroutine(PauseAfterHit());
            Debug.Log("Perfetto! Tempo guadagnato: " + timeBonusPerfect + "s");
        }
        else if (RectOverlaps(targetAreaWide, movingImage))
        {
            timeGain += 0.25f; // Aggiungi tempo bonus
            StartCoroutine(PauseAfterHit());
            Debug.Log("Buono! Tempo guadagnato: " + timeBonusWide + "s");
        }
        else
        {
            Debug.Log("Colpo mancato!");
            StartCoroutine(PauseAfterHit());
            return; // Esci se il colpo non è valido
        }
        
        counterClienti--;
        if (counterClienti <= 0)
        {
            FineGioco();
        }
        
        UpdateCounterUI();
    }

    private IEnumerator PauseAfterHit()
    {
        isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        isPaused = false;
    }

    void FineGioco()
    {
        panelToDisable.SetActive(false); // Disabilita il pannello di gioco
        Debug.Log("Gioco finito! Tempo guadagnato totale: " + timeGain + "s");
        // Qui puoi aggiungere logica per gestire la fine del gioco, come mostrare un messaggio o resettare il gioco
    }

    void UpdateCounterUI()
    {
        counterText.text = "Clienti restanti: " + counterClienti;
        timeGainText.text = "Tempo guadagnato: " + timeGain + "s";
    }

    //l'immagine è completamente contenuta nella zona
    bool RectContains(RectTransform container, RectTransform target)
    {
        Vector3[] containerCorners = new Vector3[4];
        Vector3[] targetCorners = new Vector3[4];

        container.GetWorldCorners(containerCorners);
        target.GetWorldCorners(targetCorners);

        Rect containerRect = new Rect(containerCorners[0], containerCorners[2] - containerCorners[0]);
        Rect targetRect = new Rect(targetCorners[0], targetCorners[2] - targetCorners[0]);

        return containerRect.Contains(targetRect.min) && containerRect.Contains(targetRect.max);
    }

    //Basta che ci sia una sovrapposizione tra i due rettangoli
    bool RectOverlaps(RectTransform a, RectTransform b)
    {
        Vector3[] aCorners = new Vector3[4];
        Vector3[] bCorners = new Vector3[4];

        a.GetWorldCorners(aCorners);
        b.GetWorldCorners(bCorners);

        Rect aRect = new Rect(aCorners[0], aCorners[2] - aCorners[0]);
        Rect bRect = new Rect(bCorners[0], bCorners[2] - bCorners[0]);

        return aRect.Overlaps(bRect);
    }
}
