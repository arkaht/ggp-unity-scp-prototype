using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class HandUI : MonoBehaviour
{
    public float ScreenToCenterWeight = 0.4f;
    public int ClampMargin = 200;
    public Sprite ItemSprite, ActionSprite;

    private Image image;
    private Vector2 screenSize = new(Screen.width, Screen.height);

    private void Awake() => image = GetComponent<Image>();

    private void Update()
    {
        Player player = Player.Instance;
        if (player == null) { Debug.LogError($"{typeof(HandUI)}.cs::{typeof(Player)}.cs is null reference."); return; }

        UseableEntity entity = player.UseEntity;
        if (entity == null || !entity.CanUse(player))
        {
            image.enabled = false;
            return;
        }

        ShowImage(ref entity);

        ScreenWiggle(ref entity);
    }


    private void ShowImage(ref UseableEntity entity)
    {
        image.enabled = true;
        image.sprite = entity is Item ? ItemSprite : ActionSprite;
    }

    private void ScreenWiggle(ref UseableEntity entity)
    {
        if (entity == null) { Debug.LogWarning($"{typeof(HandUI)}.cs::{nameof(entity)} is null reference."); return; }

        Vector2 getScreenPosition = Camera.main.WorldToScreenPoint(entity.transform.position);

        getScreenPosition.x = Mathf.Clamp(getScreenPosition.x, ClampMargin, screenSize.x - ClampMargin);
        getScreenPosition.y = Mathf.Clamp(getScreenPosition.y, ClampMargin, screenSize.x - ClampMargin);

        transform.position = Vector2.Lerp(getScreenPosition, screenSize / 2.0f, ScreenToCenterWeight);
    }
}