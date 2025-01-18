using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OneColor : MonoBehaviour
{
    public Sprite Blue, Green, Orange, Purple, Red, Yellow, White;
    public TMP_Dropdown Vrag1, Vrag2, Vrag3, Vrag4, Vrag5, Vrag6, Player;
    private TMP_Dropdown[] dropdowns;
    private Dictionary<TMP_Dropdown, Sprite> previousSelections = new Dictionary<TMP_Dropdown, Sprite>();

    void Start()
    {
        dropdowns = new TMP_Dropdown[] { Vrag1, Vrag2, Vrag3, Vrag4, Vrag5, Vrag6, Player };
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            dropdown.onValueChanged.AddListener((index) => OnDropdownValueChanged(dropdown, index));
        }
    }
    void OnDropdownValueChanged(TMP_Dropdown changedDropdown, int selectedIndex)
    {
        // Получаем выбранный спрайт
        Sprite selectedSprite = changedDropdown.options[selectedIndex].image;

        // Если был предыдущий выбор, возвращаем его обратно
        if (previousSelections.ContainsKey(changedDropdown))
        {
            Sprite previousSprite = previousSelections[changedDropdown];
            if (previousSprite != null)
            {
                AddSpriteToOtherDropdowns(changedDropdown, previousSprite);
            }
        }
        RemoveSpriteFromOtherDropdowns(changedDropdown, selectedSprite);

        previousSelections[changedDropdown] = selectedSprite;
    }
    // Удаляет выбранный спрайт из всех других Dropdown
    void RemoveSpriteFromOtherDropdowns(TMP_Dropdown currentDropdown, Sprite spriteToRemove)
    {
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            if (dropdown != currentDropdown && dropdown.gameObject.activeSelf)
            {
                for (int i = dropdown.options.Count - 1; i >= 0; i--)
                {
                    if (dropdown.options[i].image == spriteToRemove)
                    {
                        dropdown.options.RemoveAt(i);
                    }
                }
                dropdown.RefreshShownValue();  // Обновляем отображение
            }
        }
    }

    // Возвращает спрайт в другие Dropdown
    void AddSpriteToOtherDropdowns(TMP_Dropdown currentDropdown, Sprite spriteToAdd)
    {
        foreach (TMP_Dropdown dropdown in dropdowns)
        {
            if (dropdown != currentDropdown && dropdown.gameObject.activeSelf)
            {
                TMP_Dropdown.OptionData newOption = new TMP_Dropdown.OptionData
                {
                    image = spriteToAdd
                };
                dropdown.options.Add(newOption);
                dropdown.RefreshShownValue();
            }
        }
    }
}
