# Calificación — TP 01

---

## Bugs y problemas detectados

### Críticos (afectan funcionalidad o reproducibilidad)
- **`PauseGame.OnDisable`:** usa `+=` en lugar de `-=` → memory leak + suscripción duplicada cada vez que se reactiva el GameObject.
- **`CameraController.Rotation`:** clamp vertical inalcanzable (`&&` en vez de `||`). La cámara puede dar vueltas verticales sin tope.
- **ScriptableObjects mutados en runtime** (`StatsDataSO.level`, `GameDataSO.minEnemies/maxEnemies`, `ScoreSO.score`): persisten entre Play sessions del editor; el juego arranca con valores acumulados de la corrida anterior.
- **`LevelManager.CreatingEnemies`:** loop infinito si `maxEnemies > _npcList.Count` (la protección está después del incremento, no antes).
- **`StateWalk.OnEnter`:** `IndexOutOfRangeException` si `positions.Count == 0`.

### Importantes (rendimiento / robustez)
- **`PlayerController.CheckSpeed`:** clamp por eje se pisa entre sí.
- **`PlayerController.MovementHor`:** los `if` no son `else if` → teclas opuestas se pisan, se pierde la diagonal.
- **Pool no genérico ni reutilizable**, fixed-size sin expansión.
- **`SoundManager.OnXChanged`:** `Mathf.Log10(0) = -Infinity` cuando el slider llega a 0.
- **`UiLifeBar`:** división por cero si `maxLife == 0`.
- **`Bullets` (pool) sin `DontDestroyOnLoad`** — se recrea por escena.

### Calidad / estilo
- Nombres con typos: `corroutine` (×4), `BuleltsCollision`, `playerPos` no usado en `StateShoot`.
- Mismatch entre nombre de archivo y nombre de clase (`UiBtnHoverSFXEvent.cs` ↔ `UiButtonHoverSFXEvent`).
- `Debug.Log` en hot paths (coroutines de disparo, OnEnter/OnExit de cada estado, OnCollisionEnter).
- `StatsDataSO` es una "God SO": mezcla stats de Player y NPC en un único asset compartido.
- Singletons como campo `public static` mutable, en lugar de propiedad con setter privado.
- Magic numbers (45° de corrección, 0.5f de waypoint, 0.3f de cadencia).
- `[Header("Credits")]` duplicado en `UiMainMenu`.
- Campo `_player` en `NpcHealthSystem` declarado pero **nunca usado**.

---

## Nota Final: **8**

### Justificación
El proyecto cumple **íntegramente las Partes 1 y 2** (con bugs menores), demuestra comprensión de los pilares principales del curso (eventos C#, FSM por composición, ScriptableObjects para configuración, separación de responsabilidades, UI desacoplada por eventos) y tiene un Main Menu funcional con flujo de escenas correcto.

**Lo que impide subir la nota a 8–9:**
- No se aplicaron **Clases Abstractas para entidades** (`Person`, `Civil`, `Enemigo`) — punto explícito de la Parte 3.
- No se aplicaron **Interfaces** (`IDamageable`, etc.) — punto explícito de la Parte 3.
- El **Object Pool no es genérico**, no agrupa en un contenedor `"Content"` y no persiste entre escenas con `DontDestroyOnLoad`.
- No hay **pool de NPCs**; los enemigos/ciudadanos están preinstanciados.
- Bug funcional confirmado en cámara (clamp vertical inalcanzable) y bug crítico de suscripción duplicada en `PauseGame`.
- Estado runtime escrito sobre **ScriptableObjects mutables** que persisten entre playtests, lo que rompe la reproducibilidad del juego.

**Lo que evita bajar a 6:**
- FSM completa, animada y con transiciones por distancia al jugador.
- Doble sistema de disparo correctamente diferenciado (Raycast + objeto 3D con trayectoria precalculada sin `AddForce`).
- Daño bidireccional implementado.
- UI de vida y de score reactiva a eventos estáticos (`OnUpdateLife`, `OnScoreUpdated`).
- Flujo de escenas completo (MainMenu → Settings → Credits → Gameplay → Pause → Game Over) con fades.
- Uso correcto de `[SerializeField] private` en la mayoría de los campos.

### Recomendaciones
1. **Patrones de Diseño – Interfaces y Herencia:** implementar `IDamageable` y una clase abstracta `Person` con derivadas `Civil` y `Enemigo`. Reemplazar el `bool isEnemy` por polimorfismo.
2. **ScriptableObjects:** entender la diferencia entre *configuración* (read-only en runtime) y *estado* (debe vivir en componentes runtime, no en assets `.asset`). Resetear los valores al iniciar la partida o mover el estado a otro lado.
3. **Object Pool genérico:** implementar `Pool<T>` que sirva para balas, enemigos y ciudadanos, con `DontDestroyOnLoad` y agrupando los objetos bajo un contenedor `Content`.
4. **Pulido pre-entrega:** antes de subir el commit, cerrar y abrir Play 3 veces consecutivas para detectar estado persistente; revisar la consola en runtime para detectar `Debug.Log` y `NullReferenceException` que no se ven en una corrida corta.
