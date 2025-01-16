using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AddVrags : MonoBehaviour
{
    public TMP_Dropdown enemyCountDropdown;
    public GameObject enemiesImagePanel;
    public TMP_FontAsset customFont; 

    void Start()
    {
        // Подключаем событие к Dropdown
        enemyCountDropdown.onValueChanged.AddListener(OnEnemyCountChanged);
    }

    // Метод для обработки изменения выбора в Dropdown (он теперь public)
    public void OnEnemyCountChanged(int selectedValue)
    {
        Debug.Log("Выбранное количество противников: " + (selectedValue + 1));  // Плюс 1 для учёта сдвига
        for (int i = enemiesImagePanel.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(enemiesImagePanel.transform.GetChild(i).gameObject);
        }
        // Изначальные координаты для первого противника
        float startX = 0f;
        float startY = 0f;
        float offsetY = 60f;  // Отступ по Y для следующих противников
        float offsetX = 0f;    // Если вы хотите отступ по оси X, можно задать

        // Создаём новые блоки для каждого противника
        for (int i = 0; i < selectedValue; i++)
        {
            // Создаём контейнер для противника с компонентом Image
            GameObject enemyContainer = new GameObject($"Enemy_{i + 1}", typeof(Image)); // Создаём сразу с Image
            enemyContainer.transform.SetParent(enemiesImagePanel.transform);

            // Получаем компонент Image и настраиваем его
            Image enemyImage = enemyContainer.GetComponent<Image>();
            enemyImage.color = Color.gray;  // Просто серый цвет для фона

            // Настройка размеров контейнера (используем RectTransform)
            RectTransform rectTransform = enemyContainer.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(65, 4); // Устанавливаем размеры для каждого блока (ширина 610, высота 40)

            // Устанавливаем позицию для текущего противника с учётом отступов
            rectTransform.anchoredPosition = new Vector2(startX + offsetX, startY - (i * offsetY)); // Отнимаем от Y для вертикальной раскладки

            // Создание и настройка текста "Противник X"
            GameObject textObj = new GameObject("EnemyText");
            textObj.transform.SetParent(enemyContainer.transform);
            TextMeshProUGUI enemyText = textObj.AddComponent<TextMeshProUGUI>();
            enemyText.text = $"Противник {i + 1}";
            enemyText.fontSize = 2;   // Устанавливаем нужный размер шрифта
            enemyText.color = Color.black;
            // Применяем ваш шрифт
            if (customFont != null)
            {
                enemyText.font = customFont; // Назначаем собственный шрифт
            }

            // Настройка RectTransform для текста
            RectTransform textRect = enemyText.GetComponent<RectTransform>();
            textRect.anchoredPosition = new Vector2(-20, 0);  // Размещаем текст в левом углу

            // Устанавливаем размер и отступы для текста (если нужно)
            textRect.sizeDelta = new Vector2(15,3); // Задаём ширину и высоту текста (пример)
        }


    }
}
