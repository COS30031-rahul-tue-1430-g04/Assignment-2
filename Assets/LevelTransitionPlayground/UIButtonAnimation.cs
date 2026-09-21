// UI Handler for Button Animations

using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAnimation :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("Hover")]
    public float hoverScale = 1.05f;

    [Header("Animation")]
    public float animationSpeed = 10f;

    [Header("Idle Pulse")]
    public bool pulseWhenIdle = false;
    public float pulseAmount = 0.03f;
    public float pulseSpeed = 3f;

    private Vector3 baseScale;
    private bool hovering;
    private bool pressing;

    void Awake()
    {
        baseScale = transform.localScale;
    }

    void Update()
    {
        Vector3 targetScale = baseScale;

        if (pressing)
        {
            targetScale = baseScale * 0.97f;
        }
        else if (hovering)
        {
            targetScale = baseScale * hoverScale;
        }
        else if (pulseWhenIdle)
        {
            float pulse =
                1f +
                Mathf.Sin(Time.unscaledTime * pulseSpeed)
                * pulseAmount;

            targetScale = baseScale * pulse;
        }

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.unscaledDeltaTime * animationSpeed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovering = false;
        pressing = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressing = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressing = false;
    }
}