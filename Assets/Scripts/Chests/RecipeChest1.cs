using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecipeChest1 : TreasureChest
{
    private GameObject physicalPanel;
    private Image physicalIcon;
    private TextMeshProUGUI damageTextPhysical;
    private TextMeshProUGUI physicalEffectText;
    private Button physicalButton;
    private GameObject gunpowderPanel;
    private Image gunpowderIcon;
    private TextMeshProUGUI damageTextGunPowder;
    private TextMeshProUGUI gunPowderEffectText;
    private Button gunPowderButton;
    private GameObject explosivePanel;
    private Image explosiveIcon;
    private TextMeshProUGUI damageTextExplosive;
    private TextMeshProUGUI explosiveEffectText;
    private Button explosiveButton;
    void Start()
    {
        physicalPanel = RecipeUIPanel.transform.GetChild(1).gameObject;
        physicalIcon = physicalPanel.transform.GetChild(0).GetComponent<Image>();
        damageTextPhysical = physicalPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalEffectText = physicalPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        physicalButton = physicalPanel.transform.GetChild(3).GetComponent<Button>();
        gunpowderPanel = RecipeUIPanel.transform.GetChild(2).gameObject;
        gunpowderIcon = gunpowderPanel.transform.GetChild(0).GetComponent<Image>();
        damageTextGunPowder = gunpowderPanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunPowderEffectText = gunpowderPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        gunPowderButton = gunpowderPanel.transform.GetChild(3).GetComponent<Button>();
        explosivePanel = RecipeUIPanel.transform.GetChild(3).gameObject;
        explosiveIcon = explosivePanel.transform.GetChild(0).GetComponent<Image>();
        damageTextExplosive = explosivePanel.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveEffectText = explosivePanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        explosiveButton = explosivePanel.transform.GetChild(3).GetComponent<Button>();
        RecipeUIPanel.SetActive(false);
    }
    void Update()
    {
        bool success = attemptToOpenChest();
        if (success)
        {
            OpenChest();
        }
    }

    protected override int OpenChest()
    {
        int chestID = base.OpenChest();
        GameObject physicalRecipe = spawnRecipe(LootListType.PhysicalRecipe, -3, -3, chestID);
        GameObject gunPowderRecipe = spawnRecipe(LootListType.GunpowderRecipe, 0, 3, chestID);
        GameObject explosiveRecipe = spawnRecipe(LootListType.ExplosiveRecipe, 3, -3, chestID);
        Time.timeScale = 0;
        FindObjectOfType<Player>().enablePlayer(false);

        //Make this a method at some point
        //physical
        physicalIcon.sprite = physicalRecipe.GetComponent<Recipe>().item.icon;
        damageTextPhysical.text = physicalRecipe.GetComponent<Recipe>().damageDescription;
        physicalEffectText.text = physicalRecipe.GetComponent<Recipe>().effectDescription;
        physicalButton.onClick.AddListener(() => physicalRecipe.GetComponent<Recipe>().MakeSelection());

        //GunPowder
        gunpowderIcon.sprite = gunPowderRecipe.GetComponent<Recipe>().item.icon;
        damageTextGunPowder.text = gunPowderRecipe.GetComponent<Recipe>().damageDescription;
        gunPowderEffectText.text = gunPowderRecipe.GetComponent<Recipe>().effectDescription;
        gunPowderButton.onClick.AddListener(() => gunPowderRecipe.GetComponent<Recipe>().MakeSelection());

        //Explosive
        explosiveIcon.sprite = explosiveRecipe.GetComponent<Recipe>().item.icon;
        damageTextExplosive.text = explosiveRecipe.GetComponent<Recipe>().damageDescription;
        explosiveEffectText.text = explosiveRecipe.GetComponent<Recipe>().effectDescription;
        explosiveButton.onClick.AddListener(() => explosiveRecipe.GetComponent<Recipe>().MakeSelection());

        RecipeUIPanel.SetActive(true);
        return chestID;
    }

    //Make popup on UI, And have it enabled when you open this chest(Place method in parent class protected)
    //Disable player movement and slow down time to zero(Place method in parent class protected)
    //Center the items you're spawning on it.
    //Make buttons appear over them
    //Make them hightlight when cursor over them (Use the image Highlight function, not the shader)
    //Needs a description on the item to appear below >> HOW???? > But a description string in each recipe class. Have it appear in the text panel when cursor over button
}
