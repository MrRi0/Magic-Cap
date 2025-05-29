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
        new public event Action<Vector2> PositionChanged;

        protected Timer _attackTimer = new Timer { Interval = 800 };
        private static int _windowWidth = Screen.PrimaryScreen.Bounds.Width;

        public Boss(Player player, Vector2 position, Vector2[] trajectory, int hp)
            : base(player, position, trajectory, hp)
        {
            _speed = 10f;
            AttackTrajectory = Vector2.Zero;
            Width = 768;
            Height = 200;
        }

        public void GetNextStage()
        {
            _moveTimer.Stop();
            if (Position.Y >= -Height)
                _targetPosition = Position + new Vector2(0, -Height);
            _isBorn = false;
        }

        public override void TakeDamage(int damage)
        {
            if (_isBorn)
                HP -= damage;
        }
    }
}
