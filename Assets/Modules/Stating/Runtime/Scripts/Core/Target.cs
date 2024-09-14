using System;
using UnityEngine;

namespace AS.Modules.Stating.Core
{
    public class Target : MonoBehaviour
    {
        public event Action<Collider> TriggerEnter;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(other);
        }
    }
}
