using GameWinForm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace GameWinForm.Model
{
    public class Enemy : Entity
    {
        public Vector2 AttackTrajectory { get; private set; }

        private readonly Player _player;

        private Vector2 _targetPosition;

        private const float _speed = 5f;

        private Timer _attackTimer = new Timer { Interval = 800 };
        private Timer _moveTimer = new Timer { Interval = 5000 };
        private Timer _trajectoryAttackTimer = new Timer { Interval = 3000 };

        private int index = 0;

        private List<Vector2> _trajectory;

        public event Action<Vector2> PositionChanged;

        public Enemy(Player player, Vector2 position, List<Vector2> trajectory, int width, int height, int hp)
        {
            Missiles = new List<Missile>();
            AttackTrajectory = Vector2.Zero;
            Hp = hp;
            Position = position;
            Width = width;
            Height = height;
            _player = player;
            _trajectory = trajectory;
            _targetPosition = _trajectory[index];
            SetTimers();
        }

        private void SetTimers()
        {
            _attackTimer.Tick += Attack;
            _moveTimer.Tick += MoveOnTrajectory;
            _trajectoryAttackTimer.Tick += ShowTrajectoryAttack;

            _moveTimer.Start();
            _trajectoryAttackTimer.Start();
        }

        public void Update()
        {
            Move();
            Position += Velocity;
            foreach (var missile in Missiles)
            {
                missile.Update();
            }
            PositionChanged?.Invoke(Position);
        }

        private void Attack(object sender, EventArgs e)
        {
            Missiles.Add(new Missile(this, AttackTrajectory, 1));
            AttackTrajectory = Vector2.Zero;
            _attackTimer.Stop();
        }

        public void Move()
        {
            Velocity = (_targetPosition - Position).Normalize() * _speed;
        }

        public new void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        private void MoveOnTrajectory(object sender, EventArgs e)
        {
            index++;
            _targetPosition = _trajectory[index % _trajectory.Count];
        }

        private void ShowTrajectoryAttack(object sender, EventArgs e)
        {
            AttackTrajectory = (_player.GetCenterPosition() - GetCenterPosition()) * 200;
            _attackTimer.Start();
        }
    }
}
