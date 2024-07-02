using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player.AfterImage
{
    public class PlayerAfterImagePool : MonoBehaviour
    {
        [SerializeField] private GameObject afterImagePrefab;

        private Queue<GameObject> _availableObjects = new Queue<GameObject>();

        public static PlayerAfterImagePool Instance { get; private set; }

        private void Awake() {
            Instance = this;
            GrowPool();

        }

        private void GrowPool() {
            for (int i = 0; i < 10; i++) {
                var instanceToAdd = Instantiate(afterImagePrefab);
                instanceToAdd.transform.SetParent(transform);
                AddToPool(instanceToAdd);

            }
        }

        public void AddToPool(GameObject _instanceToAdd) {
            _instanceToAdd.SetActive(false);
            _availableObjects.Enqueue(_instanceToAdd);
        }

        public GameObject GetFromPool() {
            if (_availableObjects.Count == 0) {
                GrowPool();
            }

            var instance = _availableObjects.Dequeue();
            instance.SetActive(true);
            return instance;
        }
    }
}
