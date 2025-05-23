using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GameWinForm.Core;
using Timer = System.Windows.Forms.Timer;

namespace GameWinForm.Model
{
    public class Player : Entity
    {
        public Vector2 LastMoveDirection { get; set; }
        public int DashCoolDown { get; private set; }

        public event Action<Vector2> PositionChanged;

        private int _dashCoolDownTime = 2000;
        private int _attackCoolDownTime = 250;
        private bool _attackIsReady;
        private Timer _dashRechargeTimer = new Timer { Interval = 20 };
        private Timer _attackRechargeTimer = new Timer();

        private const float _drag = 0.9f;
        private const float _acceleration = 6f;
        private const float _dashSpeed = 100f;

        private readonly int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private readonly int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public Player()
        {
            Missiles = new List<Missile>();
            Hp = 5;
            Width = 90;
            Height = 140;
            DashCoolDown = _dashCoolDownTime;
            Position = new Vector2((_windowWidth - Width) / 2, (_windowHeight - Height) / 2);
            _attackIsReady = true;
            _dashRechargeTimer.Tick += RechargeDash;
            _attackRechargeTimer.Interval = _attackCoolDownTime;
            _attackRechargeTimer.Tick += RechargeAttack;
        }

        public void Update()
        {
            if (IsDeath()) return;

            if (LastMoveDirection.Length() == 0)
                Velocity *= _drag;

            if (Velocity.Length() < 0.1f)
                Velocity = Vector2.Zero;

            Position += Velocity;

            foreach (var missile in Missiles)
            {
                missile.Update();
            }

            PositionChanged?.Invoke(Position);
        }

        public void Move(Vector2 direction)
        {
            Velocity = new Vector2(direction.X, direction.Y).Normalize() * _acceleration;
        }

        public void Dash(Vector2 direction)
        {
            if (DashCoolDown >= _dashCoolDownTime)
            {
                Position += new Vector2(direction.X, direction.Y).Normalize() * _dashSpeed;
                DashCoolDown = 0;
                _dashRechargeTimer.Start();
            }
        }

        public void Attack(Vector2 targetPosition)
        {
            if (_attackIsReady)
            { 
                Missiles.Add(new Missile(this, targetPosition, 1));
                _attackIsReady = false;
                _attackRechargeTimer.Start();
            }
        }

        private void RechargeDash(object sender, EventArgs e)
        {
            DashCoolDown += _dashCoolDownTime / _dashRechargeTimer.Interval;
            if (DashCoolDown >= _dashCoolDownTime)
                _dashRechargeTimer.Stop();
        }

        private void RechargeAttack(object sender, EventArgs e)
        {
            _attackIsReady = true;
            _attackRechargeTimer.Stop();
        }
    }
}
