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
        public int SkillCoolDown { get; private set; }
        public bool IsShield { get; private set; }
        public Dictionary<Upgrades, int> SelectedUpgrades { get; private set; }
        public Action<Vector2> CurrentSkill { get; private set; }

        public event Action<Vector2> PositionChanged;

        private int _skillCoolDownTime;
        private int _attackCoolDownTime = 250;
        private bool _attackIsReady;
        private int _damage = 1;
        private Timer _skillRechargeTimer = new Timer { Interval = 20 };
        private Timer _attackRechargeTimer = new Timer();
        private float _dashSpeed = 100f;
        private Timer _shieldTimer = new Timer();
        private int _shieldDuration = 600;

        private const float _drag = 0.9f;
        private float _acceleration = 6f;

        private readonly int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private readonly int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public Player()
        {
            Missiles = new List<Missile>();
            HP = 5;
            Width = 90;
            Height = 140;
            Position = new Vector2((_windowWidth - Width) / 2, (_windowHeight - Height) / 2);
            _attackIsReady = true;
            _attackRechargeTimer.Interval = _attackCoolDownTime;
            _attackRechargeTimer.Tick += RechargeAttack;
            SelectedUpgrades = new Dictionary<Upgrades, int>();
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

        public void UseSkill(Vector2 direction)
        {
            CurrentSkill(direction);
        }

        public void SetSkill(Skills skill)
        {
            switch (skill)
            {
                case Skills.Dash:
                    _skillCoolDownTime = 2000;
                    CurrentSkill = Dash;
                    break;

                case Skills.Shield:
                    _skillCoolDownTime = 2500;
                    CurrentSkill = Shield;
                    _shieldTimer.Tick += UseShield;
                    break;
            }
            SkillCoolDown = _skillCoolDownTime;
            _skillRechargeTimer.Tick += RechargeSkill;
        }

        public void UpgradePlayer(Upgrades upgrade)
        {
            if (!SelectedUpgrades.TryAdd(upgrade, 0))
                SelectedUpgrades[upgrade] += 1;
            switch (upgrade)
            {
                case Upgrades.HP:
                    HP += 1;
                    break;

                case Upgrades.AttackSpeed:
                    _attackCoolDownTime -= 20;
                    break;

                case Upgrades.Speed:
                    _acceleration += 1;
                    break;

                case Upgrades.ShieldDuration:
                    _shieldDuration += 100;
                    break;

                case Upgrades.DashSpeed:
                    _dashSpeed += 20;
                    break;

                case Upgrades.Damage:
                    _damage += 1;
                    break;

                case Upgrades.SkillCoolDown:
                    _skillCoolDownTime -= _skillCoolDownTime / 10;
                    break;
            }
        }

        private void Dash(Vector2 direction)
        {
            if (SkillCoolDown >= _skillCoolDownTime)
            {
                Position += new Vector2(direction.X, direction.Y).Normalize() * _dashSpeed;
                SkillCoolDown = 0;
                _skillRechargeTimer.Start();
            }
        }

        private void Shield(Vector2 _)
        {
            if (SkillCoolDown >= _skillCoolDownTime)
            {
                IsInvulnerability = true;
                IsShield = true;
                _shieldTimer.Interval = _shieldDuration;
                _shieldTimer.Start();
            }
        }

        public void Attack(Vector2 targetPosition)
        {
            if (_attackIsReady)
            { 
                Missiles.Add(new Missile(this, targetPosition, _damage));
                _attackIsReady = false;
                _attackRechargeTimer.Start();
            }
        }

        private void RechargeSkill(object sender, EventArgs e)
        {
            SkillCoolDown += _skillCoolDownTime / _skillRechargeTimer.Interval;
            if (SkillCoolDown >= _skillCoolDownTime)
                _skillRechargeTimer.Stop();
        }

        private void UseShield(object sender, EventArgs e)
        {
            IsInvulnerability = false;
            IsShield = false;
            SkillCoolDown = 0;
            _skillRechargeTimer.Start();
            _shieldTimer.Stop();
        }

        private void RechargeAttack(object sender, EventArgs e)
        {
            _attackIsReady = true;
            _attackRechargeTimer.Stop();
        }
    }
}
