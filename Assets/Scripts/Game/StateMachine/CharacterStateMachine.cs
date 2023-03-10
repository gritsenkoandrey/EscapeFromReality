using CodeBase.Game.Components;
using CodeBase.Infrastructure.Input;
using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.Game.StateMachine
{
    public sealed class CharacterStateMachine
    {
        private readonly CCharacter _character;
        private readonly IInputService _inputService;
        private CEnemy _target;
        
        private Camera _camera;

        private float _maxDelay;
        private float _delay;
        private float _attackRadius;
        private float _gravity;
        private float _speed;
        private float _velocity;
        private float _angle;
        
        private bool _isAttack;

        public CharacterStateMachine(CCharacter character, IInputService inputService)
        {
            _character = character;
            _inputService = inputService;
        }

        public void Init()
        {
            _camera = Camera.main;
            
            _maxDelay = 1f;
            _delay = _maxDelay;
            _attackRadius = 4f;
            _gravity = Physics.gravity.y * 10f;
            _speed = 7.5f;
        }

        public void Tick()
        {
            Input();
            Move();
            Target();
            Rotate();
            Attack();
        }

        private void Input()
        {
            if (_inputService.Value.magnitude > 0.1f)
            {
                _angle = Mathf.Atan2(_inputService.Value.x, _inputService.Value.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            }
        }

        private void Move()
        {
            Vector3 move = Vector3.zero;

            if (_inputService.Value.magnitude > 0.1f)
            {
                move = Quaternion.Euler(0f, _angle, 0f) * Vector3.forward;

                Vector3 next = _character.transform.position + move * _speed * Time.deltaTime;
                        
                Ray ray = new Ray { origin = next, direction = Vector3.down };

                if (!Physics.Raycast(ray, 5f, Layers.Ground))
                {
                    return;
                }
            }

            move.y = _character.CharacterController.isGrounded ? 0f : _gravity;

            _character.CharacterController.Move(move * _speed * Time.deltaTime);
        }

        private void Target()
        {
            foreach (CEnemy enemy in _character.Enemies)
            {
                if (Vector3.Distance(enemy.Position, _character.Position) < _attackRadius)
                {
                    _target = enemy;
                    
                    _isAttack = true;

                    break;
                }

                _isAttack = false;
            }
        }

        private void Rotate()
        {
            if (_isAttack)
            {
                Quaternion lookRotation = Quaternion.LookRotation(_target.Position - _character.Position);
                
                _character.transform.rotation = Quaternion.Slerp(_character.transform.rotation, lookRotation, 0.75f);
            }
            else
            {
                float smoothAngle = Mathf.SmoothDampAngle(_character.transform.eulerAngles.y, _angle, ref _velocity, 0.05f);

                _character.transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);
            }
        }

        private void Attack()
        {
            if (_isAttack)
            {
                if (_delay < 0f)
                {
                    _character.Attack.Attack.Execute(_target);
                    
                    _delay = _maxDelay;
                }
            }

            _delay -= Time.deltaTime;
        }
    }
}