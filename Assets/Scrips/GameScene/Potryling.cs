using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potryling : MonoBehaviour
{
    public GameObject ZonePlayer; // Объект базы (центр защиты)
    string[] unitNames = { "Siege_Tower", "Light_infantry", "Heavy", "Catapult", "Archer" };
    int radius = 10; // Радиус расстановки юнитов вокруг базы

    public void Control()
    {
        Debug.Log("[Potryling] Запуск контроля защиты базы.");
        List<GameObject> units = GetUnitsFromZonePlayer();

        if (units.Count == 0)
        {
            Debug.LogWarning("[Potryling] Юниты не найдены в базе.");
            return;
        }

        ArrangeUnitsInCircle(units);
        SetUnitsToDefend(units);
    }

    List<GameObject> GetUnitsFromZonePlayer()
    {
        List<GameObject> units = new List<GameObject>();
        Debug.Log($"[Potryling] Поиск юнитов в базе {ZonePlayer.name}.");

        foreach (Transform child in ZonePlayer.transform)
        {
            foreach (string unitName in unitNames)
            {
                if (child.name.Contains(unitName))
                {
                    units.Add(child.gameObject);
                    Debug.Log($"[Potryling] Найден юнит: {child.name}");
                    break;
                }
            }
        }
        Debug.Log($"[Potryling] Всего найдено юнитов: {units.Count}");
        return units;
    }

    void ArrangeUnitsInCircle(List<GameObject> units)
    {
        if (units.Count == 0) return;

        float angleStep = 360f / units.Count;
        Vector3 center = ZonePlayer.transform.position;
        Debug.Log($"[Potryling] Расставляем {units.Count} юнитов по кругу вокруг базы {ZonePlayer.name}.");

        for (int i = 0; i < units.Count; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 position = center + new Vector3(
                Mathf.Cos(angle) * radius,
                0,
                Mathf.Sin(angle) * radius
            );

            units[i].transform.position = position;
            units[i].transform.LookAt(center);
            Debug.Log($"[Potryling] {units[i].name} установлен на позицию {position}");
        }
    }

    void SetUnitsToDefend(List<GameObject> units)
    {
        foreach (GameObject unit in units)
        {
            UnitHpAndCommand unitScript = unit.GetComponent<UnitHpAndCommand>();
            if (unitScript != null)
            {
               //unitScript.ActivateDefense(ZonePlayer.transform.position, radius);
                Debug.Log($"[Potryling] {unit.name} готов к защите базы!");
            }
            else
            {
                Debug.LogWarning($"[Potryling] У юнита {unit.name} отсутствует компонент UnitHpAndCommand!");
            }
        }
    }
}
