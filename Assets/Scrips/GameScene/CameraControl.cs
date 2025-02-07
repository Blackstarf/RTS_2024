using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float RotateSpeed = 10f;
    private float Speed = 30f;
    private float ZoomSpeed = 1000f;
    private float EdgeScrollThreshold = 0.05f; // 5% от края экрана
    private float _mult = 1f;

    private void Update()
    {
        float hor = Input.GetAxis("Horizontal"); // A/D или ←/→
        float ver = Input.GetAxis("Vertical");   // W/S или ↑/↓

        // Обработка краевого скролла мышью
        Vector2 mousePos = Input.mousePosition;
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        // Если курсор в левых 5% экрана → движение влево
        if (mousePos.x <= screenSize.x * EdgeScrollThreshold) hor -= 1;

        // Если курсор в правых 5% экрана → движение вправо
        if (mousePos.x >= screenSize.x * (1 - EdgeScrollThreshold)) hor += 1;

        // Аналогично для верха/низа экрана
        if (mousePos.y <= screenSize.y * EdgeScrollThreshold) ver -= 1;
        if (mousePos.y >= screenSize.y * (1 - EdgeScrollThreshold)) ver += 1;

        // Ограничим значения от -1 до 1
        hor = Mathf.Clamp(hor, -1f, 1f);
        ver = Mathf.Clamp(ver, -1f, 1f);

        // Модификатор скорости
        _mult = Input.GetKey(KeyCode.LeftShift) ? 2f : 1f;

        transform.Translate(new Vector3(hor, 0, ver) * Time.deltaTime * _mult * Speed, Space.Self);

        // Обработка зума
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        transform.position += transform.up * ZoomSpeed * Time.deltaTime * scroll;
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Clamp(transform.position.y, -5f, 30f),
            transform.position.z
        );
    }
}