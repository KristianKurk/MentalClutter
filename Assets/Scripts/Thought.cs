using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Thought : EventTrigger
{
    // Dragging system
    protected bool dragging, disabled, returningToPosition;
    protected Vector2 lastPosition, mouseDelta = Vector2.zero;

    // Velocity system
    protected GameObject thoughts;
    protected Vector2 velocity, newVelocity;
    protected int lastBoundCrossed;
    protected float minSpeed, maxSpeed, explosionSpeed, minVelocityChangeCooldown, maxVelocityChangeCooldown, velocityChangeCooldown;
    protected bool justChangedVelocity;

    protected virtual void Start()
    {
        minSpeed = InterviewManager.instance.thoughtsMinSpeed;
        maxSpeed = InterviewManager.instance.thoughtsMaxSpeed;
        explosionSpeed = InterviewManager.instance.thoughtsExplosionSpeed;
        maxVelocityChangeCooldown = InterviewManager.instance.minVelocityCooldown;
        minVelocityChangeCooldown = InterviewManager.instance.maxVelocityCooldown;

        velocity = RandomVelocity().normalized * explosionSpeed;
        newVelocity = RandomVelocity();
        velocityChangeCooldown = RandomVelocityChangeCooldown();
        thoughts = InterviewManager.instance.thoughts;
    }

    protected virtual void Update() 
    {
        if(disabled) return;

        if (dragging) 
            transform.position = new Vector2(Input.mousePosition.x + mouseDelta.x, Input.mousePosition.y + mouseDelta.y);
        else
        {
            Move();
        }
    }

    public override void OnPointerDown(PointerEventData eventData) 
    {
        if(disabled) return;

        dragging = true;
        mouseDelta = transform.position - Input.mousePosition;
        transform.SetParent(InterviewManager.instance.thoughtsDragParent);
        lastPosition = new Vector2(transform.position.x, transform.position.y);
    }

    protected void Move()
    {
        //if(returningToPosition) return;

        velocity = Vector3.Slerp(velocity, newVelocity, Time.deltaTime);
        transform.Translate(velocity * Time.deltaTime);

        if(IsInvisible())
            TeleportToOppositeSide();
        else if(WithinBounds())
        {
            velocityChangeCooldown -= Time.deltaTime;
            if(velocityChangeCooldown < 0)
            {
                velocityChangeCooldown = RandomVelocityChangeCooldown();
                newVelocity = RandomVelocity();
            }
        }
    }

    protected virtual Vector2 RandomVelocity()
    {
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);

        if(minSpeed > maxSpeed)
            maxSpeed = minSpeed + 1f;
        var speed = Random.Range(minSpeed, maxSpeed);

        var resolution = Screen.currentResolution;
        var xRatio = resolution.width / 1920f;
        var yRatio = resolution.height / 1080f;
        Debug.Log(xRatio);
        var nonAdjustedVelocity = new Vector2(x, y).normalized;
        var adjustedVelocity = new Vector2(nonAdjustedVelocity.x * xRatio, nonAdjustedVelocity.y * yRatio);

        justChangedVelocity = true;
        return adjustedVelocity * speed;
    }

    protected float RandomVelocityChangeCooldown()
    {
        return Random.Range(minVelocityChangeCooldown, maxVelocityChangeCooldown);
    }

    protected void ReturnToPosition()
    {
        returningToPosition = true;
        StartCoroutine(TravelBack());
    }

    protected IEnumerator TravelBack()
    {
        var destination = new Vector3(lastPosition.x, lastPosition.y, transform.position.z);
        while(Mathf.Abs((transform.position - destination).magnitude) > 1f )
        {
            var direction = (destination - transform.position).normalized;
            transform.Translate(direction * Time.deltaTime * InterviewManager.instance.travelSpeed);

            yield return null;
        }

        transform.SetParent(thoughts.transform);
        returningToPosition = false;
    }

    protected bool WithinBounds()
    {
        var trt = thoughts.GetComponent<RectTransform>();
        var v = new Vector3[4];
        trt.GetWorldCorners(v);
        var maxX = v[2].x;
        var minX = v[0].x;
        var maxY = v[1].y;
        var minY = v[3].y;

        return transform.position.x < maxX - 0.01f
            && transform.position.x > minX + 0.01f
            && transform.position.y < maxY - 0.01f
            && transform.position.y > minY + 0.01f;
    }

    protected bool IsInvisible()
    {
        var rt = GetComponent<RectTransform>();
        var v = new Vector3[4];
        rt.GetWorldCorners(v);
        var xExtent = Mathf.Abs(transform.position.x - v[0].x);
        var yExtent = Mathf.Abs(transform.position.y - v[0].y);

        var trt = thoughts.GetComponent<RectTransform>();
        v = new Vector3[4];
        trt.GetWorldCorners(v);
        var maxX = v[2].x;
        var minX = v[0].x;
        var maxY = v[1].y;
        var minY = v[3].y;

        var rightInvisiblePos = maxX + xExtent;
        var leftInvisiblePos = minX - xExtent;
        var upInvisiblePos = maxY + yExtent;
        var downInvisiblePos = minY - yExtent;

        return transform.position.x > rightInvisiblePos 
            || transform.position.x < leftInvisiblePos 
            || transform.position.y > upInvisiblePos 
            || transform.position.y < downInvisiblePos;
    }

    protected void TeleportToOppositeSide()
    {
        var rt = GetComponent<RectTransform>();
        var v = new Vector3[4];
        rt.GetWorldCorners(v);
        var xExtent = Mathf.Abs(transform.position.x - v[0].x);
        var yExtent = Mathf.Abs(transform.position.y - v[0].y);

        var trt = thoughts.GetComponent<RectTransform>();
        v = new Vector3[4];
        trt.GetWorldCorners(v);

        var maxX = v[2].x;
        var minX = v[0].x;
        var maxY = v[1].y;
        var minY = v[3].y;

        var xFromCenter = transform.position.x - thoughts.transform.position.x;
        var yFromCenter = transform.position.y - thoughts.transform.position.y;
        
        if(transform.position.x > maxX + xExtent && (lastBoundCrossed != 2 || justChangedVelocity))
        {
            lastBoundCrossed = 1;
            transform.position = new Vector3(transform.position.x - 2 * xFromCenter, transform.position.y, transform.position.z);
        }
        else if(transform.position.x < minX - xExtent && (lastBoundCrossed != 1 || justChangedVelocity))
        {
            lastBoundCrossed = 2;
            transform.position = new Vector3(transform.position.x - 2 * xFromCenter, transform.position.y, transform.position.z);
        }
        else if(transform.position.y > maxY + yExtent && (lastBoundCrossed != 4 || justChangedVelocity))
        {
            lastBoundCrossed = 3;
            transform.position = new Vector3(transform.position.x, transform.position.y - 2 * yFromCenter, transform.position.z);
        }
        else if(transform.position.y < minY - yExtent && (lastBoundCrossed != 3 || justChangedVelocity))
        {
            lastBoundCrossed = 4;
            transform.position = new Vector3(transform.position.x, transform.position.y - 2 * yFromCenter, transform.position.z);
        }

        justChangedVelocity = false;
    }
}
