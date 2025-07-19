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


    public Color activeEquippedColor = Color.green; // elde olan ikon
    public Color inactiveEquippedColor = Color.white; // elde olmayan ama alınmış ikon

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
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (revolver != null || flashBomb != null)
                TryPickupNearest();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentEquipped != EquippedItem.None)
                DropCurrent();
        }
        
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (revolver != null && revolver.IsEquipped())
            {
                EquipWeapon(EquippedItem.Revolver);
                
            }
            else if (revolver != null && flashBomb != null && flashBomb.IsEquipped())
            {
                EquipWeapon(EquippedItem.Revolver);
                
            }
            else if (revolver != null && revolver.IsEquipped() == false && (flashBomb == null || !flashBomb.IsEquipped()))
            {
                EquipWeapon(EquippedItem.Revolver);
                
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (flashBomb != null && flashBomb.IsEquipped())
            {
                EquipWeapon(EquippedItem.FlashBomb);
                ;
            }
            else if (flashBomb != null && revolver != null && revolver.IsEquipped())
            {
                EquipWeapon(EquippedItem.FlashBomb);
               
            }
            else if (flashBomb != null && flashBomb.IsEquipped() == false && (revolver == null || !revolver.IsEquipped()))
            {
                EquipWeapon(EquippedItem.FlashBomb);
                
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentEquipped != EquippedItem.None)
            {
                // Elimizdekini bırakmadan sadece inActiveSlot'a çekiyoruz
                if (currentEquipped == EquippedItem.Revolver)
                    revolver.MoveTo(inActiveSlot);
                else if (currentEquipped == EquippedItem.FlashBomb)
                    flashBomb.MoveTo(inActiveSlot);

                currentEquipped = EquippedItem.None;
                UpdateUIIcons();
            }
        }
        
            
    }

    void TryPickupNearest()
    {
        float revolverDist = float.MaxValue;
        float flashDist = float.MaxValue;

        if (revolver != null && !revolver.IsEquipped())
        {
            if (revolver.transform != null && playerHand != null)
                revolverDist = Vector3.Distance(revolver.transform.position, playerHand.position);
        }

        if (flashBomb != null && !flashBomb.IsEquipped())
        {
            if (flashBomb.transform != null && playerHand != null)
                flashDist = Vector3.Distance(flashBomb.transform.position, playerHand.position);
        }

        // İkisi de eldeyse ya da yakında değilse çık
        if (revolverDist == float.MaxValue && flashDist == float.MaxValue)
            return;

        // En yakın olanı seç
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

        // Önce elindeki objeyi InActiveSlot'a taşı
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
        bool hasRevolver = revolver != null && (revolver.transform.parent == playerHand || revolver.transform.parent == inActiveSlot);
        bool hasFlashBomb = flashBomb != null && (flashBomb.transform.parent == playerHand || flashBomb.transform.parent == inActiveSlot);

        bool revolverInHand = revolver != null && revolver.transform.parent == playerHand;
        bool flashBombInHand = flashBomb != null && flashBomb.transform.parent == playerHand;

        // Revolver iconu
        if (revolverIcon != null)
        {
            revolverIcon.gameObject.SetActive(hasRevolver); // Elimde ya da inActiveSlot'ta varsa göster
            if (revolverInHand)
                revolverIcon.color = activeEquippedColor; // yeşil
            else if (hasRevolver)
                revolverIcon.color = inactiveEquippedColor; // beyaz
        }

        // Flash Bomb iconu
        if (flashBombIcon != null)
        {
            flashBombIcon.gameObject.SetActive(hasFlashBomb);
            if (flashBombInHand)
                flashBombIcon.color = activeEquippedColor;
            else if (hasFlashBomb)
                flashBombIcon.color = inactiveEquippedColor;
        }
        // Hand Icon - her zaman açık 
        if (handIcon != null)
        {
            handIcon.gameObject.SetActive(true); // her zaman açık

            if (currentEquipped == EquippedItem.None)
                handIcon.color = activeEquippedColor; // yeşil: sadece el aktif
            else
                handIcon.color = inactiveEquippedColor; // beyaz: el alınmış ama aktif değil
        }
    }
    
}