using System;
using _Project.Data;
using UnityEngine;

namespace _Project.Gameplay
{
    public class LevelProgressService : ILevelProgressService
    {
        private readonly LevelProgress _levelProgress;

        private int _easyZombieKilledCounter;
        private int _mediumZombieKilledCounter;
        
        public LevelProgressService(LevelProgress levelProgress)
        {
            _levelProgress = levelProgress;
            
            _easyZombieKilledCounter = 0;
            _mediumZombieKilledCounter = 0;
        }
        
        public void ZombieKilled(ZombieType zombieType)
        {
            switch (zombieType)
            {
                case ZombieType.Easy:
                    _easyZombieKilledCounter++;
                    if (_easyZombieKilledCounter == 2)
                    {
                        _levelProgress.Progress.Value += 1;
                        _easyZombieKilledCounter = 0;
                    }
                    break;
                
                case ZombieType.Medium:
                    _mediumZombieKilledCounter++;
                    if (_mediumZombieKilledCounter == 2)
                    {
                        _levelProgress.Progress.Value += 3;
                        _mediumZombieKilledCounter = 0;
                    }
                    break;
                
                case ZombieType.Hard:
                    _levelProgress.Progress.Value += 2;
                    break;
                
                default:
                    throw new Exception();
            }
        }
    }
}