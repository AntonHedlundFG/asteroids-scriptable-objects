using DefaultNamespace.ScriptableEvents;
using UnityEngine;
using Variables;

namespace Ship
{
    public class Hull : MonoBehaviour
    {
        //[SerializeField] private IntVariable _health;
        [SerializeField] private ScriptableEventIntReference _onHealthChangedEvent;
        //[SerializeField] private IntReference _healthRef;
        //[SerializeField] private IntObservable _healthObservable;
        [SerializeField] private ShipSettings _settings;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (string.Equals(other.gameObject.tag, "Asteroid"))
            {
                Debug.Log("Hull collided with Asteroid");
                // TODO can we bake this into one call?
                //_healthRef.ApplyChange(-1);
                //_onHealthChangedEvent.Raise(_healthRef);
                //_healthObservable.ApplyChange(-1);
                _settings.Health.ApplyChange(-1);
            }
        }
    }
}
