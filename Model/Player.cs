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

        public event Action<Vector2> PositionChanged;

        private const float _drag = 0.9f;
        private const float _acceleration = 6f;
        private const float _dashSpeed = 100f;

        public Player()
        {
            Missiles = new List<Missile>();
            Hp = 5;
            Width = 100;
            Height = 150;
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
            Position += new Vector2(direction.X, direction.Y).Normalize() * _dashSpeed;
        }

        public void Attack(Vector2 targetPosition)
        {
            Missiles.Add(new Missile(this, targetPosition, 1));
        }
    }
}
