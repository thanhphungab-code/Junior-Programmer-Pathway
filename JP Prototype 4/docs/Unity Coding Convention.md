# Unity Coding Convention

References:

- [https://github.com/thomasjacobsen-unity/Unity-Code-Style-Guide](https://github.com/thomasjacobsen-unity/Unity-Code-Style-Guide)
- [https://unity.com/resources/c-sharp-style-guide-unity-6](https://unity.com/resources/c-sharp-style-guide-unity-6)

-> The goal of this guide is to ensure our codebase is consistent, readable, and maintainable.

→ The most important rule is to **be consistent**.

### Guiding Principles

- **Readability over Brevity**:
    - Write clear, self-documenting code.
    - A good name is better than a clever abbreviation.
- **Consistency is Key**:
    - If you edit a file, follow the style of that file.
    - For new code, follow this guide.
- **Comment the "Why", not the "What"**:
    - Good code explains *what* it does.
    - Comments should explain *why* it does it, for later references.

---

### Quick Reference Table

| **Element Type** | **Casing** | **Prefix** | **Example** |
| --- | --- | --- | --- |
| Classes, Structs, Enums | PascalCase | None | `PlayerController`, `GameState` |
| Interfaces | PascalCase | `I` | `IDamageable` |
| Public/Protected Fields | PascalCase | None | `public int Score;` |
| Public Properties | PascalCase | None | `public int Health { get; }` |
| Private/Internal Fields | camelCase | `_` | `int _maxHealth;` |
| Methods & Events | PascalCase | None | `CalculateDamage()`, `OnPlayerDeath` |
| Local Variables | camelCase | None | `float totalDamage;` |
| Method Parameters | camelCase | None | `TakeDamage(int amount)` |

---

## 1. Naming

- **Be Descriptive**:
    - Names should reveal their intent, for example `targetEnemy` is better than `tEn`.
- **Booleans**:
    - Prefix with a verb to form a question, like `isDead`, `hasKey`, or `canJump`.
- **Methods:**
    - Name with verb phrases that describe an action, like `FireWeapon()` or `LoadScene()`.
    - Methods that return a `boolean` should also be questions, like `IsGameOver()`.
- **Enums**:
    - Use singular nouns for standard enums (`Direction`)
    - Use plural nouns for flag enums (`AttackModes`).
- **Interfaces**:
    - Name with adjectives and prefix with I, like `IDamageable`.

## 2. Formatting

- **Braces**:
    - Braces on a new line.
    - **Always use braces**, even for single-line blocks.
    
    ```csharp
    // Correct
    if (isReady)
    {
        PerformAction();
    }
    
    // Incorrect
    if (isReady) PerformAction();
    ```
    
- **Spacing**:
    - Use a single space after commas in argument lists: `MyMethod(arg1, arg2)`.
    - Use a single space around operators: `x = y + 5;`.
    - **No space** between a method name and its opening parenthesis: `MyMethod()`.
    - **No space** inside parentheses or brackets: `MyMethod(arg1)`, `data[index]`.
- **Line Length**:
    - Keep lines under 120 characters for readability.
    - Break up long statements.

## 3. Class & File Structure

- **File Naming**:
    - A C# file containing a `MonoBehaviour` must have the same name as the class.
    - **One `MonoBehaviour` per file**. Helper classes/structs are allowed in the same file.
    - **Order of Members**:
        1. Fields (Static, Constant, Serialized, Private)
        2. Properties
        3. Events
        4. MonoBehaviour Methods (Awake, Start, Update, etc.)
        5. Public Methods
        6. Private Methods
        7. Nested Classes/Structs
- **`using` Directives**:
    - Place all using directives at the top of the file.
    - Remove any that are unused.
- **Avoid `#region`**:
    - Regions can hide oversized classes.
    - A well-structured class should be readable without them.
    - If a class is too long, refactor it.
- **Namespaces**:
    - Group all code into a project-specific namespace, e.g., `MyGame.Gameplay`.
    - Namespaces should reflect the folder structure.

## 4. Comments & Documentation

- **Public API**:
    - Use XML comments for all public methods, properties, and classes
    
    ```csharp
    /// <summary>
    /// Applies damage to the character and checks for death.
    /// </summary>
    /// <param name="amount">The amount of damage to apply.</param>
    public void TakeDamage(int amount)
    {
        // ...
    }
    ```
    
- **Inspector Fields**:
    - Use `[Tooltip("...")]` to explain serialized fields in the Inspector.
    - This is better than a comment
    
    ```csharp
    [Tooltip("The speed at which the player moves.")]
    [SerializeField] private float _moveSpeed = 5f;
    ```
    
- **Avoid Attributions**:
    - Use version control (e.g., Git) to track authors and changes.
    - Don't add `// Created by...` comments.

## 5. C# Language Guidelines

### Fields and Variables

- **Access Modifiers**:
    - Explicitly declare `private`, `internal`, `protected`, `public` → It makes intent clear.
- **Serialization**:
    - Use `[SerializeField]` to expose private fields to the Inspector. Do not make fields public just for Inspector access.
- **`var` Keyword**:
    - Use var only when the type is **explicitly obvious** from the right side of the assignment.
    
    ```csharp
    // Good: Type is obvious
    var player = GetComponent<PlayerController>();
    var numbers = new List<int>();
    
    // Bad: Type is not obvious
    var data = GetSomeData();
    ```

### Properties

- Prefer properties over public fields to encapsulate logic.
- Use auto-implemented properties for simple get/set.
- Use expression-bodied members for concise read-only properties.
    
    ```csharp
    // Auto-implemented property
    public string PlayerName { get; set; }
    
    // Read-only property with a private setter and backing field
    public int Health { get; private set; }
    private int _maxHealth;
    
    // Concise read-only property
    public bool IsAtMaxHealth => Health == _maxHealth;
    ```

### Events

- Use `System.Action` for simple events.
- Name events with verb phrases (e.g., `PlayerDied`, `TimerExpired`).
- Name event-raising methods with an On prefix (e.g., `OnPlayerDied()`).
- Always use the null-conditional operator for invocation to prevent errors.
    
    ```csharp
    public event Action PlayerDied;
    
    private void Die()
    {
        // ... death logic
        PlayerDied?.Invoke();
    }
    ```