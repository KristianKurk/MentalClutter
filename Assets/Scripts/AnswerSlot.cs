using UnityEngine;
using UnityEngine.EventSystems;

public class AnswerSlot : Thought
{
    [HideInInspector] public Word word;
    [HideInInspector] public int index;
    [HideInInspector] public bool occupied;

    protected override void Start()
    {
        minSpeed = InterviewManager.instance.thoughtsMinSpeed;
        maxSpeed = InterviewManager.instance.thoughtsMaxSpeed;
        maxVelocityChangeCooldown = InterviewManager.instance.minVelocityCooldown;
        minVelocityChangeCooldown = InterviewManager.instance.maxVelocityCooldown;

        velocity = RandomVelocity().normalized * explosionSpeed;
        newVelocity = RandomVelocity();
        velocityChangeCooldown = RandomVelocityChangeCooldown();
        thoughts = InterviewManager.instance.thoughts;
    }

    public override void OnPointerDown(PointerEventData eventData) 
    {
        return;
    }

    protected override Vector2 RandomVelocity()
    {
        var x = Random.Range(-1f, 1f);
        var y = Random.Range(-1f, 1f);

        var speed = InterviewManager.instance.answerSlotsSpeed;

        justChangedVelocity = true;

        return new Vector2(x, y).normalized * speed;
    }
}
