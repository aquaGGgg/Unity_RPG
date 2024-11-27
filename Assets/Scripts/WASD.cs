using UnityEngine;

public class WASD : MonoBehaviour
{
    public float speed = 5f; // Скорость передвижения
    public float jumpForce = 10f; // Сила прыжка

    private Rigidbody2D rb;
    private bool isGrounded;
    public bool isMove; // Флаг движения
    public bool isAttack; // Флаг атаки

    private Animator animator; // Ссылка на Animator

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Автоматическая привязка компонента Animator
    }

    void Update()
    {
        // Управление движением
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y); // Используем velocity вместо linearVelocity

        // Проверка на движение
        if (Mathf.Abs(moveInput) > 0.3f) // Скорость больше 0.3
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }

        // Проверка на нажатие ЛКМ для атаки
        if (Input.GetMouseButtonDown(0)) // 0 - левая кнопка мыши
        {
            isAttack = true;
            animator.SetBool("isAttack", true); // Включаем анимацию атаки
        }
        else if (Input.GetMouseButtonUp(0)) // Когда отпускаем кнопку, анимация должна завершиться
        {
            isAttack = false;
            animator.SetBool("isAttack", false); // Отключаем анимацию атаки
        }

        // Обновление параметра Speed в Animator для анимации
        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        // Поворот персонажа в зависимости от направления движения
        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1); // Поворот вправо
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1); // Поворот влево

        // Прыжок
        if (Input.GetButtonDown("Jump") && isGrounded) // Прыжок только когда на земле
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // Применение силы прыжка
            animator.SetTrigger("Jump"); // Включаем анимацию прыжка
        }

        // Обновление состояния анимации для idle/бег (для плавных переходов)
        if (isMove)
        {
            animator.SetBool("isMoving", true); // Включаем анимацию движения
        }
        else
        {
            animator.SetBool("isMoving", false); // Отключаем анимацию движения
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Проверка, что персонаж стоит на земле
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true); // Включаем анимацию стояния на земле
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Проверка, что персонаж продолжает стоять на земле
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true; // Находимся на земле
            animator.SetBool("isGrounded", true); // Обновляем состояние на земле
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Когда персонаж выходит с земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false; // Не на земле
            animator.SetBool("isGrounded", false); // Отключаем анимацию стояния на земле
        }
    }
}

