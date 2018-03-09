using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace ResourceBalancing
{
    // Components that want to be notified of model state change events can 
    // do so through the GameEventManager singleton

    public class GameEventManager : MonoBehaviour
    {
        private Dictionary<ModelEventType, ModelEvent> eventDictionary;

        private static GameEventManager gameEventManager;

        public static GameEventManager instance
        {
            get
            {
                if (!gameEventManager)
                {
                    gameEventManager = FindObjectOfType<GameEventManager>();

                    if (!gameEventManager)
                    {
                        Debug.LogError("There must be one active GameEventManger script on a GameObject in the scene.");
                    }
                    else
                    {
                        gameEventManager.Init();
                    }
                }

                return gameEventManager;
            }
        }

        void Init()
        {
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<ModelEventType, ModelEvent>();
            }
        }

        public static void StartListening(ModelEventType modelEventType, UnityAction<int, int> listener)
        {
            ModelEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(modelEventType, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new ModelEvent();
                thisEvent.AddListener(listener);
                instance.eventDictionary.Add(modelEventType, thisEvent);
            }
        }

        public static void StopListening(ModelEventType modelEventType, UnityAction<int, int> listener)
        {
            if (gameEventManager == null) 
                return;

            ModelEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(modelEventType, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(ModelEventType modelEventType, int row, int column)
        {
            ModelEvent thisEvent = null;
            if (instance.eventDictionary.TryGetValue(modelEventType, out thisEvent))
            {
                thisEvent.Invoke(row, column);
            }
        }
    }
}