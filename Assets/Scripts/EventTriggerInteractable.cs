using Interaction;
using UnityEngine;
using UnityEngine.Events;

public class EventTriggerInteractable : MonoBehaviour, IInteractable
{
    public UnityEvent onInteract = new();
    [SerializeField] private string interactPromptAddition;

    public void Interact()
    {
        onInteract.Invoke();
    }

    public string GetInteractablePromptAddition() => interactPromptAddition;
}