using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CircularSliderHandler : Slider
{
    private float direction = 1f;
    private RectTransform self;
    private Camera camera;

    void Start()
    {
        self = GetComponent<RectTransform>();
        var rect = self.rect;
        camera = Camera.main;
    }
    
    float getAngle(Vector2 position)
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(self, position, camera, out localPoint);
        return (Vector2.SignedAngle(Vector2.left, localPoint) + 180) / 360;
    }

    float getAngle(Vector2 position1, Vector2 position2)
    {
        Vector2 localPoint1;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(self, position1, camera, out localPoint1);
        Vector2 localPoint2;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(self, position2, camera, out localPoint2);
        return (Vector2.SignedAngle(localPoint1, localPoint2)) / 360;
    }

    override
    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.interactable)
        {
            this.Select();
            value = getAngle(eventData.position);
        }
    }

    override
    public void OnPointerUp(PointerEventData eventData)
    {
    }

    override
    public void OnDrag(PointerEventData eventData)
    {
        if (this.interactable)
        {
            float change = getAngle(eventData.position - eventData.delta, eventData.position);
            value += change;
        }
    }
}
