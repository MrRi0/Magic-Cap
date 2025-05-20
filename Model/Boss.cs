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

        public Boss(Player player, Vector2 position, Vector2[] trajectory, int width, int height, int hp)
            : base(player, position, trajectory, width, height, hp)
        {
            AttackTrajectory = Vector2.Zero;
        }

        public void GetNextStage()
        {
            _moveTimer.Stop();
            _targetPosition = Position + new Vector2(0, -Height);
            _isBorn = false;
        }
    }
}
