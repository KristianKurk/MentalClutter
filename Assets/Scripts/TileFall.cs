using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileFall : MonoBehaviour
{
    public int ySpawn;
    public int yEnd = -353;
    public int targetCenter = -280;
    public int beatMargin = 6;
    public float secondsToFall;
    public KeyCode keyCode;
    public int points = 1;
    public string word;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private float requiredSpeed;

    public int beatToHit;

    void Start()
    {
        startPoint = new Vector2(0, ySpawn);
        endPoint = new Vector2(0, targetCenter);

        transform.localPosition = startPoint;
        requiredSpeed = (startPoint.y - endPoint.y) / secondsToFall;
    }

    private void Update()
    {
        GetComponent<Image>().color = Color.white;
        if (Mathf.Abs(MusicManager.instance.songPositionInBeats - beatToHit) < beatMargin + 0.5f)
        {
            GetComponent<Image>().color = Color.blue;
            if (Input.GetKeyDown(keyCode))
            {
                RhythmManager.instance.areAnyGood = true;
                RhythmManager.instance.displayedSentence += word + " ";
                ScoreManager.instance.IncrementSuccesses();
                SFX.instance.PlayTileSuccessSFX();
                Destroy(gameObject);
            }
        }

        if (transform.localPosition.y < yEnd + 1)
        {
            int random = Random.Range(0, RhythmManager.instance.missedNoteTexts.Length);
            RhythmManager.instance.displayedSentence += RhythmManager.instance.missedNoteTexts[random] + " ";
            SFX.instance.PlayMissedNoteSFX(random);
            ScoreManager.instance.IncrementFailures();
            Destroy(gameObject);
        }
    }

    void FixedUpdate()
    {
        if (transform.localPosition.y > endPoint.y + 1)
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, endPoint, Time.fixedDeltaTime * requiredSpeed);
        }
        else
        {
            transform.localPosition = Vector2.MoveTowards(transform.localPosition, new Vector2(0, yEnd), Time.fixedDeltaTime * requiredSpeed);
        }
    }
}

