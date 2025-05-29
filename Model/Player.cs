using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using GameWinForm.Core;
using Timer = System.Windows.Forms.Timer;
using System.Security.Cryptography;

namespace GameWinForm.Model
{
    public class Player : Entity
    {
        public Vector2 LastMoveDirection { get; set; }
        public Vector2 LastDirectionView { get; private set; }
        public int SkillCoolDown { get; private set; }
        public int SkillCoolDownTime { get; private set; }
        public bool IsShield { get; private set; }
        public bool IsDash { get; private set; }
        public Dictionary<Upgrades, int> SelectedUpgrades { get; private set; }
        public Skills CurrentSkill { get; private set; }

        public event Action<Vector2> PositionChanged;

        private int _attackCoolDownTime = 250;
        private bool _attackIsReady;
        private int _damage = 1;
        private Timer _skillRechargeTimer = new Timer { Interval = 20 };
        private Timer _attackRechargeTimer = new Timer();
        private float _dashSpeed = 20f;
        private Timer _shieldTimer = new Timer();
        private Timer _dashTimer = new Timer();
        private int _shieldDuration = 600;

        private const float _drag = 0.9f;
        private float _acceleration = 6f;

        private readonly int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private readonly int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public Player()
        {
            Missiles = new List<Missile>();
            HP = 5;
            Width = 140;
            Height = 140;
            Position = new Vector2((_windowWidth - Width) / 2, (_windowHeight - Height) / 2);
            CurrentSkill = Skills.NoSkill;
            _attackIsReady = true;
            _attackRechargeTimer.Interval = _attackCoolDownTime;
            _attackRechargeTimer.Tick += RechargeAttack;
            _dashTimer.Tick += UseDash;
            SelectedUpgrades = Enum.GetValues(typeof(Upgrades)).Cast<Upgrades>().ToDictionary(x => x, y => 0);
        }

        public void Update()
        {
            if (IsDeath()) return;

            if (LastMoveDirection.Length() == 0 && !IsDash)
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
            if (direction.X != 0)
                LastDirectionView = new Vector2(direction.X, 0);
        }

        public void UseSkill(Vector2 direction)
        {
            switch (CurrentSkill)
            {
                case Skills.Dash:
                    Dash(direction);
                    break;

                case Skills.Shield:
                    Shield();
                    break;
            }
        }

        public void SetSkill(Skills skill)
        {
            switch (skill)
            {
                case Skills.Dash:
                    SkillCoolDownTime = 2000;
                    CurrentSkill = Skills.Dash;
                    break;

                case Skills.Shield:
                    SkillCoolDownTime = 2600;
                    CurrentSkill = Skills.Shield;
                    _shieldTimer.Tick += UseShield;
                    break;
            }
            SkillCoolDown = SkillCoolDownTime;
            _skillRechargeTimer.Tick += RechargeSkill;
        }

        public void UpgradePlayer(Upgrades upgrade)
        {
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
                    _dashSpeed += 2;
                    break;

                case Upgrades.Damage:
                    _damage += 1;
                    break;

                case Upgrades.SkillCoolDown:
                    SkillCoolDownTime -= SkillCoolDownTime / 10;
                    break;
            }
        }

        private void Dash(Vector2 direction)
        {
            if (direction == Vector2.Zero)
                direction = LastDirectionView;
            if (SkillCoolDown >= SkillCoolDownTime)
            {
                IsDash = true;
                Velocity = direction.Normalize() * _dashSpeed;
                _dashTimer.Interval = (int)(16 * 200 / _dashSpeed);
                _dashTimer.Start();
            }
        }

        private void Shield()
        {
            if (SkillCoolDown >= SkillCoolDownTime)
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
            SkillCoolDown += _skillRechargeTimer.Interval;
            if (SkillCoolDown >= SkillCoolDownTime)
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

        private void UseDash(object sender, EventArgs e)
        {
            IsDash = false;
            SkillCoolDown = 0;
            _skillRechargeTimer.Start();
            Velocity = Velocity.Normalize() * _acceleration;
            _dashTimer.Stop();
        }

        private void RechargeAttack(object sender, EventArgs e)
        {
            _attackIsReady = true;
            _attackRechargeTimer.Stop();
        }
    }
}
