using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Liminal.Examples
{
    /// <summary>
    /// Just by implementing IPointerClickHandler like any UI, you can click on objects
    /// </summary>
    public class Interactable : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent OnClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClick.Invoke();
        }
    }
}
