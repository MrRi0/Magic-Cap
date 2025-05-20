using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

namespace GameWinForm.Model
{
    public class Shooter : Enemy
    {
        public Vector2 AttackTrajectory { get; protected set; }
        protected Timer _attackTimer = new Timer { Interval = 700 };
        protected Timer _trajectoryAttackTimer = new Timer { Interval = 3000 };

        public Shooter(Player player, Vector2 position, Vector2[] trajectory, int width, int height, int hp)
            : base(player, position, trajectory, width, height, hp)
        {
            AttackTrajectory = Vector2.Zero;
        }

        public Shooter(Player player, Boss boss, Vector2 position, int width, int height, int hp)
            : base(player, boss, position, width, height, hp)
        {
            AttackTrajectory = Vector2.Zero;
        }

        protected override void SetTimers()
        {
            if (Trajectory != null && Trajectory.Length != 0 && _boss == null)
            {
                _moveTimer.Tick += MoveOnTrajectory;
                _moveTimer.Start();
            }
            _attackTimer.Tick += Attack;
            _trajectoryAttackTimer.Tick += ShowTrajectoryAttack;
            _trajectoryAttackTimer.Start();
        }

        protected void Attack(object sender, EventArgs e)
        {
            Missiles.Add(new Missile(this, AttackTrajectory, 1));
            AttackTrajectory = Vector2.Zero;
            _attackTimer.Stop();
        }

        protected void ShowTrajectoryAttack(object sender, EventArgs e)
        {
            AttackTrajectory = (_player.GetCenterPosition() - GetCenterPosition()) * 200;
            _attackTimer.Start();
        }
    }
}
