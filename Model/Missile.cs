using GameWinForm.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Model
{
    public class Missile : Entity
    {
        public Vector2 Trajectory { get; set; }

        public int Damage { get; private set; }

        private const float _speed = 25f;

        public event Action<Vector2> PositionChanged;

        public Missile(Player player, Vector2 targetPosition, int damage)
        {
            Damage = damage;
            Width = 15;
            Height = 15;
            Position = player.GetCenterPosition();
            Velocity = (targetPosition - Position).Normalize();
        }

        public Missile(Enemy enemy, Vector2 targetPosition, int damage)
        {
            Damage = damage;
            Width = 15;
            Height = 15;
            Position = enemy.GetCenterPosition();
            Velocity = (targetPosition - Position).Normalize();
        }

        public void Update()
        {
            Position += Velocity * _speed;
            PositionChanged?.Invoke(Position);
        }
    }
}
