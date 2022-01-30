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

    private Color clickColor;

    public int beatToHit;

    void Start()
    {
        clickColor = new Color32(174, 41, 41, 255);
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
            GetComponent<Image>().color = clickColor;
            if (Input.GetKeyDown(keyCode))
            {
                RhythmManager.instance.areAnyGood = true;
                RhythmManager.instance.displayedSentence += word + " ";
                if (RhythmManager.instance.scoreManager && RhythmManager.instance.sfx)
                {
                    RhythmManager.instance.scoreManager.IncrementSuccesses();
                    switch (points)
                    {
                        case 2:
                            RhythmManager.instance.sfx.PlayBadWordHit();
                            break;
                        case 3:
                            RhythmManager.instance.sfx.PlayOKWordHit();
                            break;
                        case 4:
                            RhythmManager.instance.sfx.PlayGoodWordHit();
                            break;
                        default:
                            RhythmManager.instance.sfx.PlayTileSuccessSFX();
                            break;
                    }
                    RhythmManager.instance.scoreManager.totalScore += points;
                }
                Destroy(gameObject);
            }
        }

        if (transform.localPosition.y < yEnd + 1)
        {
            int random = Random.Range(0, RhythmManager.instance.missedNoteTexts.Length);
            RhythmManager.instance.displayedSentence += RhythmManager.instance.missedNoteTexts[random] + " ";
            if (RhythmManager.instance.scoreManager && RhythmManager.instance.sfx)
            {
                RhythmManager.instance.sfx.PlayMissedNoteSFX(random);
                RhythmManager.instance.scoreManager.IncrementFailures();
                RhythmManager.instance.scoreManager.totalScore--;
            }
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

