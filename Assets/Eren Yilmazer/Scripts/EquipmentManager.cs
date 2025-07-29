using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EquipmentManager : MonoBehaviour
{

    public Transform playerHand;
    public Transform inActiveSlot;

    public Weapon revolver;
    public FlashBomb flashBomb;
    

    public Image revolverIcon;
    public Image flashBombIcon;
    public Image handIcon;


    public Color activeEquippedColor = Color.yellow; 
    public Color inactiveEquippedColor = Color.grey; 

    public enum EquippedItem
    {
        None,
        Revolver,
        FlashBomb
    }

    private EquippedItem currentEquipped = EquippedItem.None;
    public static EquipmentManager Instance;

    void Awake()
    {
            Instance = this;
        revolverIcon.gameObject.SetActive(false);
        flashBombIcon.gameObject.SetActive(false);
        handIcon.gameObject.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool hasRevolver = revolver && (revolver.transform.parent == playerHand || revolver.transform.parent == inActiveSlot);
            bool hasFlashBomb = flashBomb && (flashBomb.transform.parent == playerHand || flashBomb.transform.parent == inActiveSlot);

            // Eğer revolver VE flashBomb zaten alınmışsa, pickup yapmaya gerek yok
            if (!hasRevolver || !hasFlashBomb)
            {
                TryPickupNearest();
            }
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentEquipped != EquippedItem.None)
                DropCurrent();
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (revolver && 
                (revolver.transform.parent == playerHand || revolver.transform.parent == inActiveSlot))
            {
                EquipWeapon(EquippedItem.Revolver);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            
            if (flashBomb && 
                (flashBomb.transform.parent == playerHand || flashBomb.transform.parent == inActiveSlot))
            {
                EquipWeapon(EquippedItem.FlashBomb);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            switch (currentEquipped)
            {
                case EquippedItem.Revolver:
                    revolver.MoveTo(inActiveSlot);
                    break;
                case EquippedItem.FlashBomb:
                    flashBomb.MoveTo(inActiveSlot);
                    break;
            }

            currentEquipped = EquippedItem.None;
            UpdateUIIcons();
        }



    }

    void TryPickupNearest()
    {
        float revolverDist = float.MaxValue;
        float flashDist = float.MaxValue;

        if (revolver && !revolver.IsEquipped())
        {
            if (revolver.transform && playerHand)
                revolverDist = Vector3.Distance(revolver.transform.position, playerHand.position);
        }

        if (flashBomb && !flashBomb.IsEquipped())
        {
            if (flashBomb.transform && playerHand)
                flashDist = Vector3.Distance(flashBomb.transform.position, playerHand.position);
        }

        
        if (revolverDist == float.MaxValue && flashDist == float.MaxValue)
            return;

        
        if (revolverDist < flashDist && revolverDist <= 2f)
        {
            EquipWeapon(EquippedItem.Revolver);
        }
        else if (flashDist < revolverDist && flashDist <= 2f)
        {
            EquipWeapon(EquippedItem.FlashBomb);
        }
    }


    public void EquipWeapon(EquippedItem toEquip)
    {
        if (toEquip == currentEquipped)
        {
            bool alreadyInHand = false;

            switch (toEquip)
            {
                case EquippedItem.Revolver:
                    alreadyInHand = revolver.transform.parent == playerHand;
                    break;
                case EquippedItem.FlashBomb:
                    alreadyInHand = flashBomb.transform.parent == playerHand;
                    break;
            }

            if (alreadyInHand)
                return;
        }

        
        if (currentEquipped != EquippedItem.None)
        {

            switch (currentEquipped)
            {
                case EquippedItem.Revolver:
                    revolver.MoveTo(inActiveSlot);
                    break;
                case EquippedItem.FlashBomb:
                    flashBomb.MoveTo(inActiveSlot);
                    break;
            }
        }

        switch (toEquip)
        {
            case EquippedItem.Revolver:
                revolver.MoveTo(playerHand);
                revolver.SetEquipped(true);
                break;
            case EquippedItem.FlashBomb:
                flashBomb.MoveTo(playerHand);
                flashBomb.SetEquipped(true);

                break;
        }

        currentEquipped = toEquip;
        
        
        UpdateUIIcons();
    }

    private void DropCurrent()
    {
        if (currentEquipped == EquippedItem.Revolver)
        {
            revolver.DropFromHand();
            currentEquipped = EquippedItem.None;
        }
        else if (currentEquipped == EquippedItem.FlashBomb)
        {
            flashBomb.DropFromHand();
            currentEquipped = EquippedItem.None;
        }
        UpdateUIIcons();
    }

    public void ClearFlashBomb()
    {
        if (currentEquipped == EquippedItem.FlashBomb)
        {
            currentEquipped = EquippedItem.None;
        }

        flashBomb = null;
    }
    
    private void UpdateUIIcons()
    {
        bool hasRevolver = revolver && (revolver.transform.parent == playerHand || revolver.transform.parent == inActiveSlot);
        bool hasFlashBomb = flashBomb && (flashBomb.transform.parent == playerHand || flashBomb.transform.parent == inActiveSlot);

        bool revolverInHand = revolver && revolver.transform.parent == playerHand;
        bool flashBombInHand = flashBomb && flashBomb.transform.parent == playerHand;

        
        // Opak ve yarı saydam renkler
        Color visible = new Color(1f, 1f, 1f, 1f);       // Tam opak (ikon aktif)
        Color faded = new Color(1f, 1f, 1f, 0.2f);        // Saydam (ikon silik)
        
        if (revolverIcon)
        {
            revolverIcon.gameObject.SetActive(hasRevolver);
            revolverIcon.color = revolverInHand ? visible : faded;
        }

        if (flashBombIcon)
        {
            flashBombIcon.gameObject.SetActive(hasFlashBomb);
            flashBombIcon.color = flashBombInHand ? visible : faded;
        }

        // El ikonunu her zaman göster ama uygun şekilde saydam yap
        if (handIcon)
        {
            handIcon.gameObject.SetActive(true);
            handIcon.color = (currentEquipped == EquippedItem.None) ? visible : faded;
        }
    }
    
}