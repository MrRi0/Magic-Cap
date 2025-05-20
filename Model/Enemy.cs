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
        public Vector2[] Trajectory { get; protected set; }

        protected Player _player;

        protected Boss _boss;

        protected Vector2 _targetPosition;

        protected float _speed = 5f;

        protected Timer _moveTimer = new Timer { Interval = 4000 };

        protected int _indexTrajectory = 0;

        protected bool _isBorn = false;

        public event Action<Vector2> PositionChanged;

        public Enemy()
        {
            Missiles = new List<Missile>();
        }

        public Enemy(Player player, Vector2 position, Vector2[] trajectory, int width, int height, int hp)
        {
            Missiles = new List<Missile>();
            Hp = hp;
            Position = position;
            Width = width;
            Height = height;
            this._player = player;
            Trajectory = trajectory;
            if (Trajectory.Length != 0)
                _targetPosition = Trajectory[_indexTrajectory];
        }

        public Enemy(Player player, Boss boss, Vector2 position, int width, int height, int hp)
        {
            Missiles = new List<Missile>();
            Hp = hp;
            Position = position;
            Width = width;
            Height = height;
            _player = player;
            _boss = boss;
            Trajectory = boss.Trajectory.Select(x => x + (Position - boss.Position)).ToArray();
            if (Trajectory.Length != 0)
                _targetPosition = Trajectory[_indexTrajectory];
        }

        protected void SetTimers()
        {
            if (Trajectory.Length != 0)
            {
                _moveTimer.Tick += MoveOnTrajectory;
                _moveTimer.Start();
            }
        }

        public void Update()
        {
            if (!_isBorn)
            { 
                SetTimers();
                _isBorn = true;
            }
            Move();
            Position += Velocity;
            foreach (var missile in Missiles)
            {
                missile.Update();
            }
            PositionChanged?.Invoke(Position);
        }

        public void Move()
        {
            if (Position.X > _targetPosition.X - _speed && Position.X < _targetPosition.X + _speed
                && Position.Y > _targetPosition.Y - _speed && Position.Y < _targetPosition.Y + _speed)
            { 
                Velocity = Vector2.Zero;
                return;
            }
            Velocity = (_targetPosition - Position).Normalize() * _speed;
        }

        public new void TakeDamage(int damage)
        {
            Hp -= damage;
        }

        protected void MoveOnTrajectory(object sender, EventArgs e)
        {
            _indexTrajectory++;
            if (Trajectory.Length != 0)
                _targetPosition = Trajectory[_indexTrajectory % Trajectory.Length];
        }
    }
}
