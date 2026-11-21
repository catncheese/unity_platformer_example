# Example of 2D Unity-Platformer game

Проект для демонстрации студентам Unity 💻
Несколько полезных README-файлов, которые я писал для себя. Для вас это будет полезной памяткой, чем мы занимались на уроках.

---

Статья: https://habr.com/ru/articles/211472/

Статья 2014 года описывает Unity 4.3. С тех пор многое изменилось, но базовые принципы (спрайты, Rigidbody 2D, Animator) остались. Главные «точки обновления»:

1.  **Настройка проекта**: вместо галочки «2D» при создании — выбор шаблона **2D (Built-In Render Pipeline)** или **2D (URP)**.
2.  **Спрайты**: логика нарезки (`Sprite Mode: Multiple` → `Sprite Editor` → `Slice`) почти не изменилась, но интерфейс стал удобнее .
3.  **Физика**: `Rigidbody 2D` и `Interpolate` остались, но появились новые настройки (например, `Collision Detection` по умолчанию `Discrete`).
4.  **Анимация**: `Animator` и `Animate Physics` на месте , но процесс создания клипов через окно `Animation` стал интуитивнее.
5.  **Ввод (КЛЮЧЕВОЕ)**: `Input.GetKeyDown` устарел. Нужно использовать **Input System** (как вы уже выяснили).

---

#### Часть 1: Заготовка и анимация покоя (Обновленная)

**1. Создание проекта**
*   **Было**: `File > New Project`, галочка `2D`.
*   **Стало**: В Unity Hub выберите шаблон **2D (Built-In Render Pipeline)**. Это ближайший аналог старой настройки.

**2. Спрайты и платформа**
*   Создайте папку `Sprites`, импортируйте картинки.
*   Перетащите `Platform` на сцену. Unity создаст объект с `Sprite Renderer`.
*   Добавьте `Box Collider 2D` (как в статье).

**3. Персонаж (GameObject)**
*   `GameObject > Create Empty`, назовите `Character`.
*   Добавьте `Rigidbody 2D`. **Важно**: В новых версиях `Fixed Angle` может называться **`Constraints > Freeze Rotation Z`**. Поставьте галочку.
*   `Interpolate` установите в **`Interpolate`** — это сгладит движение камеры .
*   Добавьте `Sprite Renderer`, но пока не назначайте спрайт (он будет из листа).

**4. Нарезка спрайтшита (Idle)**
*   Выделите `Idle.png` в Project.
*   В Inspector: `Sprite Mode` → **`Multiple`**.
*   Нажмите `Sprite Editor`. В новом окне выберите `Slice` → `Type: Automatic` (или `Grid By Cell Size`, если кадры ровные). Нажмите `Slice`, затем `Apply` .
*   Разверните `Idle` в Project, перетащите `Idle_0` в поле `Sprite` у `Sprite Renderer` персонажа.

**5. Коллайдер персонажа**
*   Добавьте `Box Collider 2D`. Уменьшите его, чтобы он был только у ног (как в статье).

**6. Анимация покоя (Animator)**
*   Добавьте компонент `Animator`.
*   Снимите галочку `Apply Root Motion`.
*   Поставьте галочку **`Animate Physics`** (в новых версиях может быть скрыта в выпадающем меню или называться `Update Mode: Animate Physics`) .
*   Создайте `Animator Controller` в папке `Animations`, перетащите его в поле `Controller` компонента `Animator`.
*   Откройте окно `Animation` (`Window > Animation > Animation`), выберите персонажа, нажмите `Create New Clip`, назовите `Idle`.
*   Выделите все кадры `Idle_0`...`Idle_7` в Project и перетащите в окно `Animation`.
*   Установите `Samples` (например, 10).

**✅ Результат**: Персонаж стоит и «дышит».

---

#### Часть 2: Бегущий персонаж (Обновленная)

**7. Нарезка спрайтшита (Run)**
*   Проделайте те же шаги, что и для `Idle`, но с файлом `Run.png`.

**8. Создание анимации бега**
*   В окне `Animation` нажмите `Create New Clip`, назовите `Run`.
*   Перетащите кадры бега в таймлайн.
*   В окне `Animator` создайте переход (`Make Transition`) от `Idle` к `Run` и обратно.

**9. Параметры перехода**
*   В окне `Animator` перейдите на вкладку `Parameters`, создайте параметр **`Float`** с именем `Speed` (или `MoveInput`).
*   Настройте условия перехода: `Idle → Run` при `Speed > 0.1`, `Run → Idle` при `Speed < 0.1`.

**10. Скрипт движения (С учетом Input System)**
*   Создайте скрипт `PlayerMovement`.
*   **Ключевое изменение**: используйте `Keyboard.current` для чтения ввода.

```csharp
using UnityEngine;
using UnityEngine.InputSystem; // Обязательно!

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator anim;
    private float moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Чтение ввода через новый Input System
        moveInput = 0f;
        if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
        if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;

        // Передача параметра в Animator
        anim.SetFloat("Speed", Mathf.Abs(moveInput));
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
    }
}
```

**11. Разворот персонажа**
*   В `Update` скрипта добавьте логику разворота спрайта при смене направления (через `transform.localScale.x` или `Sprite Renderer.flipX`).

---

#### Часть 3: Прыжки и падения (Обновленная)

**12. Нарезка спрайтшита (Jump)**
*   Аналогично предыдущим.

**13. Анимации Jump и Fall**
*   Создайте клипы `Jump` и `Fall` (или `Fall` можно сделать из одного кадра).
*   В `Animator` создайте параметры **`Bool`**: `IsGrounded`, `IsJumping`. Настройте переходы.

**14. Скрипт прыжка (Input System)**
*   В `Update` добавьте проверку нажатия пробела:

```csharp
if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
{
    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    anim.SetBool("IsJumping", true);
}
```

*   **Проверка земли**: используйте `OnCollisionEnter2D` и `OnCollisionExit2D` для установки `isGrounded` (как в оригинальной статье).

---

### 💡 Что еще стоит показать студентам (Бонус)

*   **Tilemap**: В современных версиях Unity для платформ лучше использовать `Tilemap` вместо отдельных спрайтов. Это можно упомянуть как «следующий шаг».
*   **Cinemachine**: Для слежения камеры за персонажем (в статье это не описано, но сейчас это стандарт).
*   **Input Actions**: Показать, что помимо прямого `Keyboard.current`, существует более продвинутый способ через `Input Actions` (но это можно оставить для отдельного урока) .

# 📋 Шпаргалка: Урок по 2D-движению в Unity

## 1. Создание проекта
*   **Unity Hub** → шаблон **2D (Built-In Render Pipeline)**.
*   Папки: `Sprites`, `Animations`, `Scripts`.

## 2. Спрайты и нарезка (Sprite Sheet)
*   Выделить файл → Inspector → **Sprite Mode: Multiple** → **Sprite Editor**.
*   **Slice** → `Automatic` (или `Grid By Cell Size`) → **Slice** → **Apply**.
*   В Project развернуть треугольник, перетащить нужный кадр в **Sprite Renderer** (для статики) или в окно **Animation** (для анимации).

## 3. Платформа (составная)
*   Собрать из тайлов: выделить тайл → зажать **`V`** → тянуть за угол к соседнему тайлу (Vertex Snap).
*   **Сгруппировать**: выделить все тайлы → `Ctrl+C` → ПКМ по пустышке → **Paste Special → Paste as Child (Keep World Transform)**.
*   **Центрирование**: выделить пустышку → меню **Tools → Center Parent on Children** (скрипт из папки `Editor`).
*   На платформу: **Box Collider 2D**.

## 4. Персонаж
*   `Create Empty` → имя `Character`.
*   **Rigidbody 2D**:
    *   `Constraints → Freeze Rotation Z` ✅
    *   `Interpolate → Interpolate`
    *   `Gravity Scale = 0` (для движения влево-вправо).
*   **Sprite Renderer** + **Box Collider 2D** (у ног).
*   **Animator**: снять `Apply Root Motion`, включить `Animate Physics` (Update Mode).

## 5. Анимации (Animator)
*   Создать **Animator Controller** → перетащить в поле `Controller`.
*   Окно **Animation** (`Window → Animation`) → `Create New Clip`.
*   Перетащить кадры в таймлайн, `Samples = 10`.
*   **Одноразовая анимация**: выделить клип → Inspector → снять **Loop Time**.
*   **Переход после одноразовой**: в Animator выделить стрелку → удалить все Conditions → `Has Exit Time` ✅ → `Exit Time = 0.99` → `Transition Duration = 0`.

## 6. Скрипты (Input System!)
> ⚠️ Везде `using UnityEngine.InputSystem;`

**Движение:**
```csharp
void Update() {
    float moveInput = 0f;
    if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveInput = -1f;
    if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveInput = 1f;
    anim.SetFloat("Speed", Mathf.Abs(moveInput));
}
void FixedUpdate() {
    rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
}
```

**Прыжок:**
```csharp
if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
```

**Движущаяся платформа (интерполяция):**
```csharp
public Transform pointA, pointB;
public float speed = 1f;

void Update() {
    float t = Mathf.PingPong(Time.time * speed, 1f);
    transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
}
```

## 7. Ключевые тезисы для студентов
*   `Transform` — телепорт; `Rigidbody2D` — физика (инерция, столкновения).
*   `GetComponent<T>()` — скрипт «узнаёт» о компоненте.
*   `rb.velocity` — задаём скорость, движок сам двигает и обрабатывает коллизии.
*   `Lerp(A, B, t)` — точка между A и B; `t=0` → A, `t=1` → B.
*   `PingPong` — заставляет `t` ходить 0↔1.
*   `Keyboard.current.spaceKey.wasPressedThisFrame` — аналог `GetKeyDown`.

## 8. Частые ошибки
*   Забыли прикрепить скрипт к объекту.
*   `Rigidbody` вместо `Rigidbody2D`.
*   Забыли `using UnityEngine.InputSystem;`.
*   Анимация зацикливается → не снят **Loop Time**.
*   Переход не срабатывает → `Exit Time = 1.0` (ставить `0.99`).

## 9. Бонус на будущее
*   **Tilemap** — правильный способ сборки уровней из тайлов.
*   **Cinemachine** — слежение камеры за персонажем.
*   **Input Actions** — продвинутая работа с вводом.

# 📋 Шпаргалка: Коллизии и Instantiate в 2D-платформере

## 1. Теория: разница событий

| Событие | Когда срабатывает | Настройки коллайдера |
| :--- | :--- | :--- |
| `OnCollisionEnter2D` | Физическое столкновение | `Is Trigger` **выключен** |
| `OnCollisionExit2D` | Объекты перестали касаться | `Is Trigger` **выключен** |
| `OnCollisionStay2D` | Объекты касаются каждый кадр | `Is Trigger` **выключен** |
| `OnTriggerEnter2D` | Пересечение зоны-триггера | `Is Trigger` **включен** |
| `OnTriggerExit2D` | Выход из зоны-триггера | `Is Trigger` **включен** |
| `OnTriggerStay2D` | Объект в зоне каждый кадр | `Is Trigger` **включен** |

*   **Коллизии** — физический контакт (отталкивание, опора).
*   **Триггеры** — просто «замечаем» пересечение, объекты проходят сквозь друг друга.
*   Для срабатывания хотя бы у одного объекта должен быть `Rigidbody2D`.

---

## 2. Практика 1: Проверка прыжка (OnCollision)

**Шаг 1: Настройка земли**
*   У платформы/земли есть `BoxCollider2D`, галочка `Is Trigger` **снята**.
*   Создать тег `Ground` и назначить его платформе.

**Шаг 2: Дополнение в скрипт игрока**
```
private bool isGrounded = false;

void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = true;
    }
}

void OnCollisionExit2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Ground"))
    {
        isGrounded = false;
    }
}
```

**Шаг 3: Обновление прыжка**
```
if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
{
    rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
}
```

**💡 Нюанс:** если персонаж стоит на двух платформах сразу, `OnCollisionExit2D` может сработать при переходе между ними. Для базового урока достаточно `bool`, но упомяните, что правильнее — счётчик коллизий.

---

## 3. Практика 2: Сбор предмета (OnTrigger + Instantiate)

**Шаг 1: Монетка (префаб)**
*   Спрайт монетки на сцене.
*   `CircleCollider2D` + галочка `Is Trigger` ✅.
*   Перетащить в папку Project → получится **Prefab**. Удалить со сцены.
*   Назначить тег `Coin`.

**Шаг 2: Эффект (префаб)**
*   Создать эффект (Particle System или спрайт «вспышки»).
*   Сделать **Prefab**, удалить со сцены.

**Шаг 3: Скрипт игрока (сбор)**
```
public GameObject pickupEffectPrefab; // перетащить префаб эффекта
public int score = 0;

void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("Coin"))
    {
        // 1. Создаём эффект в позиции монетки
        Instantiate(pickupEffectPrefab, other.transform.position, Quaternion.identity);

        // 2. Уничтожаем монетку
        Destroy(other.gameObject);

        // 3. Начисляем очки
        score++;
        Debug.Log("Score: " + score);
    }
}
```

**Шаг 4: Настройка**
*   Назначить тег `Coin` префабу монетки.
*   Расставить монетки по уровню (или создать ряд через `Instantiate` в `Start`).

**💡 Бонус:** `OnTriggerStay2D` — для «кислоты»/«шипов», наносящих урон каждую секунду.

---

## 4. Сигнатуры методов

```
// Коллизии — параметр Collision2D
void OnCollisionEnter2D(Collision2D collision) { }
void OnCollisionExit2D(Collision2D collision) { }
void OnCollisionStay2D(Collision2D collision) { }

// Триггеры — параметр Collider2D
void OnTriggerEnter2D(Collider2D other) { }
void OnTriggerExit2D(Collider2D other) { }
void OnTriggerStay2D(Collider2D other) { }
```

---

## 5. Instantiate — кратко

```
Instantiate(prefab, position, rotation);
```

*   Возвращает ссылку на клон (можно сохранить в переменную).
*   `Quaternion.identity` — без поворота.
*   Клон появляется в **текущей сцене**.

---

## 6. Частые ошибки

*   Забыли `Rigidbody2D` хотя бы у одного объекта — события не сработают.
*   Перепутали `Collision2D` и `Collider2D` в сигнатуре.
*   Не поставили/не сняли `Is Trigger`.
*   Забыли назначить тег или перепутали его в `CompareTag`.
*   Префаб не перетащен в поле в Inspector → `NullReferenceException`.
*   `Instantiate` без сохранения ссылки, если клон нужен позже.

---

## 7. Ключевые тезисы

*   **Enter** — момент входа, **Exit** — момент выхода, **Stay** — каждый кадр внутри.
*   Коллизии — для физики, триггеры — для логики (сбор, зоны).
*   `Instantiate` — «клонировать префаб»; `Destroy` — «удалить объект».
*   Теги + `CompareTag` — быстрый способ понять, с чем столкнулись.

## Меню, загрузка сцены и пауза

**Настройка сцен:**
1. Создать сцену MainMenu (индекс 0) и Game (индекс 1).
2. File → Build Settings → добавить обе сцены.

**Скрипт MainMenu.cs (на кнопку Start):**
```
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
```

**Скрипт PauseMenu.cs (на игрока):**
```
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;
    private bool isPaused = false;

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            isPaused = !isPaused;
            pausePanel.SetActive(isPaused);
            Time.timeScale = isPaused ? 0f : 1f;
        }
    }
}
```

**Ключевые тезисы:**
- `SceneManager.LoadScene(index)` — загрузка сцены по индексу.
- `Time.timeScale = 0f` — пауза (замораживает физику и анимации).
- TextMeshProUGUI — современная замена Legacy Text, используйте `TMPro`.

## Покачивание монетки (интерполяция)

**Скрипт CoinFloat.cs (на префаб монетки):**
```
using UnityEngine;

public class CoinFloat : MonoBehaviour
{
    public float amplitude = 0.25f;
    public float speed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0f, offsetY, 0f);
    }
}
```

**Альтернатива через PingPong + Lerp (знакомая связка):**
```
float t = Mathf.PingPong(Time.time * speed, 1f);
Vector3 top = startPos + new Vector3(0f, amplitude, 0f);
transform.position = Vector3.Lerp(startPos, top, t);
```

**Ключевые тезисы:**
- `startPos` запоминаем в `Start`, иначе монетка «уползает».
- `Mathf.Sin` — плавное покачивание (замедление в крайних точках).
- `PingPong` + `Lerp` — знакомо по платформе, но разворот резче.
- Скрипт вешается на префаб → все экземпляры качаются автоматически.
- Движение через `Transform` (не `Rigidbody2D`), т.к. монетка — триггер.
