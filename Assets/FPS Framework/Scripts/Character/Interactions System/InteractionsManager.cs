using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using System;
using System.Reflection;

namespace Akila.FPSFramework
{
    [AddComponentMenu("Akila/FPS Framework/Player/Interactions Manager")]
    public class InteractionsManager : MonoBehaviour
    {
        [Tooltip("The allowed range for any interaction")]
        public float range = 2;
        [Tooltip("If 1 player interaction angle is 360 if 0.5 interaction angle is 180")]
        public float fieldOfInteractions = 0.5f;
        [Tooltip("What layer to interact with")]
        public LayerMask interactableLayers = -1;
        [Tooltip("The UI Object which contains all the data about the interaction")]
        public GameObject HUDObject;
        [Tooltip("The display text for interact key")]
        public TextMeshProUGUI interactKeyText;
        [Tooltip("The interaction name which will show if in range EXAMPLES (Open, Pickup, etc..)")]
        public TextMeshProUGUI interactActionText;
        public AudioProfile defaultInteractAudio;

        public Audio interactAudio;
        public IInventory Inventory { get; private set; }
        public Controls controls { get; private set; }

        public bool isActive { get; set; } = true;

        public AudioClip currentInteractAudioClip { get; set; }

        public PlayerController player;

        private void Start()
        {
            Inventory = GetComponent<IInventory>();
            controls = new Controls();
            controls.Player.Enable();
            player = GetComponentInParent<PlayerController>();
        }

        private void OnEnable()
        {
            interactAudio = new Audio();

            if (defaultInteractAudio != null)
                currentInteractAudioClip = defaultInteractAudio.audioClip;

            interactAudio.Setup(this, defaultInteractAudio);
        }

        private void Update()
        {
            if (player.player.userID != SocketManager.Instance.player.userID) return;
            IInteractable interactable = GetInteractable();

            if (HUDObject)
                HUDObject.SetActive(isActive && interactable != null);

            if (interactable != null && isActive)
            {
                if (interactKeyText) interactKeyText.SetText(controls.Player.Intract.GetBindingDisplayString());
                if (interactActionText) interactActionText.SetText(interactable.GetInteractionName());

                if (controls.Player.Intract.triggered)
                {

                    interactable.Interact(this);
                }
            }
        }

        public IInteractable GetInteractable()
        {
            List<IInteractable> interactables = new List<IInteractable>();

            Collider[] colliders = Physics.OverlapSphere(transform.position, range, interactableLayers);
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent(out IInteractable interactable))
                {
                    interactables.Add(interactable);
                }
            }

            IInteractable closestInteractable = null;
            foreach (IInteractable interactable in interactables)
            {
                Vector3 position = transform.position;
                Vector3 interactablePosition = interactable.transform.position;


                if (closestInteractable == null)
                {
                    closestInteractable = interactable;
                }
                else if (Vector3.Distance(position, interactablePosition) < Vector3.Distance(position, closestInteractable.transform.position))
                {
                    closestInteractable = interactable;
                }

                var dir = (closestInteractable.transform.position - position).normalized;
                if (Vector3.Dot(transform.forward, dir) <= fieldOfInteractions)
                {
                    closestInteractable = null;
                }
            }

            return closestInteractable;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.white;
        }

#if UNITY_EDITOR
        [ContextMenu("Setup/Network Components")]
        public void Convert()
        {
            bool notFound = true;
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.Name == "FPSFrameworkComponentsManager")
                    {
                        type.GetMethod("ConvertInteractionsManager").Invoke(this, new object[] { this });
                        notFound = false;
                    }
                }
            }

            if (notFound)
            {
                Debug.LogError("Please install 'FPS Framework: Multiplayer Edition' before trying to network your components.");
            }
        }
#endif
    }
}