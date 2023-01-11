using UnityEngine;
using UnityEngine.UI;

namespace View.Popup
{
    public sealed class HealthItemView : MonoBehaviour
    {
        [SerializeField] private Image _healthAvailableImage;
        
        public bool IsAvailable
        {
            get => _healthAvailableImage.gameObject.activeInHierarchy;
            set => _healthAvailableImage.gameObject.SetActive(value);
        }
    }
}