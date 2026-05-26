using UnityEngine;

[CreateAssetMenu(fileName = "Score", menuName = "GameSettings/Score")]

public class ScoreSO : ScriptableObject
{
    // Warning: score es mutado por ScoreController en runtime. Como los SO persisten entre seciones en el editor,
    // el score queda acumulado entre playtests. Hay que resetearlo al iniciar el juego o usar variables runtime.
    // Suggestion: usar [SerializeField] private con properties en vez de campos públicos directos.
    public int score;
    public int maxScore;
}
