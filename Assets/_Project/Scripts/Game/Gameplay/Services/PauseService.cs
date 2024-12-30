using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Project.Gameplay
{
    public class PauseService : IPauseService
    {
        private readonly List<IPauseHandler> _pauseHandlers = new List<IPauseHandler>();
        
        public void SetPause(bool isPaused)
        {
            foreach (var pauseHandler in _pauseHandlers)
                pauseHandler?.HandlePause(isPaused);

            if (isPaused)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }

        public void Register(IPauseHandler pauseHandler)
        {
            _pauseHandlers.Add(pauseHandler);
        }

        public void Unregister(IPauseHandler pauseHandler)
        {
            if(_pauseHandlers.Contains(pauseHandler) == false)
                throw new Exception();
            
            _pauseHandlers.Remove(pauseHandler);
        }
    }
}