using System;
using UnityEngine;

namespace Player.AfterImage
{
    public class PlayerAfterImageSprite : MonoBehaviour
    {
        [SerializeField] private float activeTime = 0.1f;
        private float _timeActivated;
        private float _alpha;

        [SerializeField] private float alphaSet = 0.8f;
        [SerializeField] private float alphaDecay = 0.85f;

        private Transform _player;

        private SpriteRenderer _sr;
        private SpriteRenderer _playerSr;

        private Color _color;

        private void OnEnable() {
            _sr = GetComponent<SpriteRenderer>();
            _player = GameObject.FindGameObjectWithTag("Player").transform;
            _playerSr = _player.GetComponentInChildren<SpriteRenderer>();

            _alpha = alphaSet;
            _sr.sprite = _playerSr.sprite;
            transform.position = _player.position;
            transform.rotation = _player.rotation;
            _timeActivated = Time.time;
        }

        private void Update() {
            _alpha -= alphaDecay * Time.deltaTime;
            _color = new Color(1f, 1f, 1f, _alpha);
            _sr.color = _color;

            if (Time.time >= (_timeActivated + activeTime)) {
                PlayerAfterImagePool.Instance.AddToPool(gameObject);
            }
        }
    }
}
