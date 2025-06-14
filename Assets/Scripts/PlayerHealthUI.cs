using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public enum OwnerType { Ranged, Melee }

public class PlayerHealthUI : MonoBehaviour
{
    public OwnerType ownerType;
    public List<Image> heartImages;

    public void SetHealth(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].enabled = (i < currentHealth);
        }
    }
}
