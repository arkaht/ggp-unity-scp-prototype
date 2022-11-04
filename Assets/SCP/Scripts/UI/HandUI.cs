using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class HandUI : MonoBehaviour
{
    public float ScreenToCenterWeight = 0.4f;
    public int ClampMargin = 200;
    public Sprite ItemSprite, ActionSprite;

    private Image image;
    private Vector2 screenSize = new(Screen.width, Screen.height);
    private Player player;
    private UseableEntity entity;

    private void Awake() => image = GetComponent<Image>();

    private void Start()
    {
        // cached fields for optimization
        player = Player.Instance;
        entity = player.UseEntity;
    }

    private void Update()
    {
        ChechPlayerInstance();

        CheckUseEentity();

        ShowImage();

        ScreenWiggle();
    }

    private void ChechPlayerInstance()
    {
        if (player == null) { Debug.LogError($"{typeof(HandUI)}.cs::{typeof(Player)}.cs is null reference."); return; }
    }

    private void CheckUseEentity()
    {
        if (entity == null || !entity.CanUse(player))
        {
            image.enabled = false;
            return;
        }
    }

    private void ShowImage()
    {
        image.enabled = true;
        image.sprite = entity is Item ? ItemSprite : ActionSprite;
    }

    private void ScreenWiggle()
    {
        Vector2 getScreenPosition = Camera.main.WorldToScreenPoint(entity.transform.position);

        getScreenPosition.x = Mathf.Clamp(getScreenPosition.x, ClampMargin, screenSize.x - ClampMargin);
        getScreenPosition.y = Mathf.Clamp(getScreenPosition.y, ClampMargin, screenSize.x - ClampMargin);

        transform.position = Vector2.Lerp(getScreenPosition, screenSize / 2.0f, ScreenToCenterWeight);
    }
}