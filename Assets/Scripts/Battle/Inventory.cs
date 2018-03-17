using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Inventory : MonoBehaviour {

    [SerializeField] CharacterStatsObj playerStats;
    [SerializeField] PlayerCombatController playerController;
    [SerializeField] BuffItem buff;

    [SerializeField] Text descriptionText;

    [SerializeField] Button button;
    [SerializeField] Image image;
    [SerializeField] Sprite noItem;

    public void Start()
    {
        playerStats.InventoryUpdated += OnInventoryUpdate;
        OnInventoryUpdate();
    }

    public void OnInventoryUpdate()
    {
        if (playerStats.HasBuff(buff))
        {
            button.interactable = true;
            image.sprite = buff.sprite;
        }
        else
        {
            image.sprite = noItem;
            button.interactable = false;
        }
    }

    public void OnClick() {
        // TODO use the item!
        playerController.UseItem(buff);
    }

    public void OnSelect()
    {
        descriptionText.text = buff.description;
    }

    public void OnDeselect()
    {
        descriptionText.text = "";
    }
}
