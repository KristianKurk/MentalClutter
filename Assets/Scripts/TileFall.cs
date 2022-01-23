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

    private float timer = 0f;
    private float percent;
    private Vector2 startPoint;
    private Vector2 endPoint;
    private Vector2 difference;

    public int beatToHit;

    void Start()
    {
        startPoint = new Vector2(0, ySpawn);
        endPoint = new Vector2(0, targetCenter);
        difference = endPoint - startPoint;
        transform.localPosition = startPoint;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= secondsToFall)
        {
            timer += Time.deltaTime;
            percent = timer / secondsToFall;
            transform.localPosition = startPoint + difference * percent;

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

        }
        else
        {
            if (Mathf.Abs(transform.localPosition.y - yEnd) > 10)
            {
                timer += Time.deltaTime;
                percent = timer / secondsToFall;
                transform.localPosition = startPoint + difference * percent;
            }
            else
            {
                ScoreManager.instance.IncrementFailures();
                Destroy(gameObject);
            }
        }
    }
}
