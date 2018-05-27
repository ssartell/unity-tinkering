using System;
using System.Collections.Generic;
using GamepadInput;
using UnityEngine;

namespace Assets.Scripts
{
    public class GamePadEventEmitter : MonoBehaviour
    {
        private readonly Dictionary<GamePad.Button, List<Action>> _handlers;

        public GamePadEventEmitter()
        {
            _handlers = new Dictionary<GamePad.Button, List<Action>>();
        }

        public void OnButton(GamePad.Button button, GamePad.Index index, Action handler)
        {
            if (!_handlers.ContainsKey(button))
                _handlers.Add(button, new List<Action>());

            _handlers[button].Add(handler);
        }

        private void FixedUpdate()
        {
            
        }
    }
}
