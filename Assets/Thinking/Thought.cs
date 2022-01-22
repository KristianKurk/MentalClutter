using UnityEngine;
using UnityEngine.EventSystems;

public class Thought : EventTrigger
{
    // Dragging system
    protected bool dragging;
    protected Vector2 lastPosition, mouseDelta = Vector2.zero;

    // Velocity system
    protected GameObject thoughts;
    protected Vector2 velocity, newVelocity;
    protected int lastBoundCrossed;
    protected float maxSpeed = 75f, minSpeed = 100f, minVelocityChangeCooldown = 4f, maxVelocityChangeCooldown = 6f, velocityChangeCooldown;
    protected bool justChangedVelocity;

    protected virtual void Start()
    {
        newVelocity = RandomVelocity();
        velocityChangeCooldown = RandomVelocityChangeCooldown();
        thoughts = GameManager.instance.thoughts;
    }

    protected virtual void Update() 
    {
        if (dragging) 
            transform.position = new Vector2(Input.mousePosition.x + mouseDelta.x, Input.mousePosition.y + mouseDelta.y);
        else
        {
            Move();
        }
    }

    public override void OnPointerDown(PointerEventData eventData) 
    {
        dragging = true;
        mouseDelta = transform.position - Input.mousePosition;
        transform.SetParent(GameManager.instance.thoughtsParent);
        lastPosition = new Vector2(transform.position.x, transform.position.y);
    }

    protected void ReturnToPosition()
    {
        transform.SetParent(thoughts.transform);
        transform.position = new Vector3(lastPosition.x, lastPosition.y, transform.position.z);
    }

    protected void Move()
    {
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

    protected Vector2 RandomVelocity()
    {
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);

        if(minSpeed > maxSpeed)
            maxSpeed = minSpeed + 1f;
        var speed = Random.Range(minSpeed, maxSpeed);

        justChangedVelocity = true;

        return new Vector2(x, y).normalized * speed;
    }

    protected float RandomVelocityChangeCooldown()
    {
        return Random.Range(minVelocityChangeCooldown, maxVelocityChangeCooldown);
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

        return transform.position.x < maxX - 1f
            && transform.position.x > minX + 1f
            && transform.position.y < maxY - 1f
            && transform.position.y > minY + 1f;
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
