using UnityEngine;

namespace View
{
    public sealed class EnemyViewVariant : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
        [field: SerializeField] public Animator Animator { get; private set; }
        [SerializeField] private EnemyView _enemyView;

        public void OnAttackAnimationFinished() // TODO: может переделать обращение к enemyview?
        {
            _enemyView.OnAttackAnimationFinished(); 
        }
    }
}