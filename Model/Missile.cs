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
    public class Missile : Entity
    {
        public Vector2 Trajectory { get; set; }
        public Vector2 AttackTrajectory { get; set; }

        public int Damage { get; private set; }

        private float _speed = 25f;
        private Timer _showTrajectory = new Timer { Interval = 2000 };

        public event Action<Vector2> PositionChanged;

        private static int _windowHeight = Screen.PrimaryScreen.Bounds.Height;
        private static int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public Missile(Player player, Vector2 targetPosition, int damage)
        {
            Damage = damage;
            Width = 15;
            Height = 15;
            Position = player.GetCenterPosition();
            Velocity = (targetPosition - Position).Normalize();
            AttackTrajectory = Vector2.Zero;
        }

        public Missile(Enemy enemy, Vector2 targetPosition, int damage)
        {
            Damage = damage;
            Width = 15;
            Height = 15;
            Position = enemy.GetCenterPosition();
            Velocity = (targetPosition - Position).Normalize();
            AttackTrajectory = Vector2.Zero;
        }

        public Missile(Vector2 position, Vector2 targetPosition, int damage)
        {
            Damage = damage;
            Width = 15;
            Height = 15;
            Position = position;
            Velocity = Vector2.Zero;
            AttackTrajectory = targetPosition;
            _showTrajectory.Tick += UnshowTrajectory;
        }

        public bool IsDeath()
        {
            return Position.X > _windowWidth + 500 || Position.X < -500 || Position.Y > _windowHeight + 500 || Position.Y < -500;
        }

        public void Update()
        {
            if (AttackTrajectory != Vector2.Zero)
                _showTrajectory.Start();
            Position += Velocity * _speed;
            PositionChanged?.Invoke(Position);
            if (IsDeath())
                _speed = 0;
        }

        private void UnshowTrajectory(object sender, EventArgs e)
        {
            Velocity = (AttackTrajectory - Position).Normalize();
            AttackTrajectory = Vector2.Zero;
            _showTrajectory.Stop();
        }
    }
}
