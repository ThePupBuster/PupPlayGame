using UnityEngine;

namespace Game.Mode
{
    public abstract class AbstractGameMode
    {
        public virtual void Awake()
        {
            Debug.Log("Activate Game Mode: " + this.GetType().Name);
        }

        public virtual void Pause()
        {
            Debug.Log("Pause Game Mode: " + this.GetType().Name);
        }

        public virtual void Resume()
        {
            Debug.Log("Resume Game Mode: " + this.GetType().Name);
        }

        public virtual void Destroy()
        {
            Debug.Log("Destroy Game Mode: " + this.GetType().Name);
        }

        public virtual void Update()
        {
        }
    }
}
