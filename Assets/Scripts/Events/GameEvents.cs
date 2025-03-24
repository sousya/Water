using System;

namespace WaterGame.Events
{
    public static class GameEvents
    {
        public static readonly EventId OnLevelStart = new EventId("OnLevelStart");
        public static readonly EventId OnLevelComplete = new EventId("OnLevelComplete");
        public static readonly EventId OnBottleSelected = new EventId("OnBottleSelected");
        public static readonly EventId OnWaterMoved = new EventId("OnWaterMoved");
        public static readonly EventId OnGamePaused = new EventId("OnGamePaused");
        public static readonly EventId OnGameResumed = new EventId("OnGameResumed");
        public static readonly EventId OnMoveCountChanged = new EventId("OnMoveCountChanged");
        public static readonly EventId OnTimeChanged = new EventId("OnTimeChanged");
    }

    public class EventId
    {
        private readonly string _id;

        public EventId(string id)
        {
            _id = id;
        }

        public override string ToString()
        {
            return _id;
        }
    }

    public class EventManager
    {
        private static readonly Dictionary<EventId, Action<object[]>> _events = new Dictionary<EventId, Action<object[]>>();

        public static void AddListener(EventId eventId, Action<object[]> listener)
        {
            if (!_events.ContainsKey(eventId))
            {
                _events[eventId] = listener;
            }
            else
            {
                _events[eventId] += listener;
            }
        }

        public static void RemoveListener(EventId eventId, Action<object[]> listener)
        {
            if (_events.ContainsKey(eventId))
            {
                _events[eventId] -= listener;
            }
        }

        public static void Trigger(EventId eventId, params object[] args)
        {
            if (_events.ContainsKey(eventId))
            {
                _events[eventId]?.Invoke(args);
            }
        }
    }
} 