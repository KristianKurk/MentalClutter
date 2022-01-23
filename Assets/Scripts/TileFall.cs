using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileFall : MonoBehaviour
{
    public int ySpawn = 353;
    public int yEnd = -353;
    public int targetCenter = -280;
    public int targetHeight = 100;
    public int beatMargin = 6;
    public float secondsToFall;
    public KeyCode keyCode;

    private Vector2 startPoint;
    private Vector2 endPoint;
    private float requiredSpeed;

    public int beatToHit;

    void Start()
    {
        startPoint = new Vector2(0, ySpawn);
        endPoint = new Vector2(0, targetCenter);
        Debug.DrawLine(new Vector2(-1000, targetCenter), new Vector2(1000, targetCenter), Color.green, 1000, false);

        transform.localPosition = startPoint;
        requiredSpeed = (startPoint.y - endPoint.y) / secondsToFall;
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

        GetComponent<Image>().color = Color.white;
        if (Mathf.Abs(MusicManager.instance.songPositionInBeats - beatToHit) < beatMargin)
        {
            GetComponent<Image>().color = Color.blue;
            if (Input.GetKeyDown(keyCode))
            {
                ScoreManager.instance.IncrementSuccesses();
                Destroy(gameObject);
            }
        }

        if (transform.localPosition.y < yEnd)
        {
            ScoreManager.instance.IncrementFailures();
            Destroy(gameObject);
        }
    }
}

