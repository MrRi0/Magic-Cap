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
    public class Boss : Enemy
    {
        public Vector2 AttackTrajectory { get; protected set; }
        public event Action<Vector2> PositionChanged;

        protected Timer _attackTimer = new Timer { Interval = 800 };

        public Boss(Player player, Vector2 position, Vector2[] trajectory, int width, int height, int hp)
            : base(player, position, trajectory, width, height, hp)
        {
            AttackTrajectory = Vector2.Zero;
        }

        public void DashAttack()
        {
            _speed = 15f;
            _moveTimer.Interval = 800;
            _moveTimer.Start();
            AttackTrajectory = Trajectory[0];
        }

        public void SetTrajectory(Vector2[] trajectory)
        {
            Trajectory = trajectory;
        }

        public void GetNextStage()
        {
            _moveTimer.Stop();
            Trajectory = Array.Empty<Vector2>();
            _targetPosition = Position + new Vector2(0, -Height);
        }

        new public void Move()
        {
            if (Position.X > _targetPosition.X - _speed && Position.X < _targetPosition.X + _speed
                && Position.Y > _targetPosition.Y - _speed && Position.Y < _targetPosition.Y + _speed)
            {
                Velocity = Vector2.Zero;
                if (AttackTrajectory != Vector2.Zero)
                {
                    if (_indexTrajectory + 1 == Trajectory.Length)
                        AttackTrajectory = Vector2.Zero;
                    else
                        AttackTrajectory = Trajectory[_indexTrajectory + 1];
                }
                return;
            }
            Velocity = (_targetPosition - Position).Normalize() * _speed;
        }
    }
}
