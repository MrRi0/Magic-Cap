using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Model
{
    public class Stage
    {
        public List<Enemy> Enemies { get; private set; }
        public List<Missile> Missiles { get; private set; }
        public Action<Vector2[]> MoveScript { get; private set; }

        public static readonly Stage Empty = new Stage(new List<Enemy>(), new List<Missile>());

        public Stage(List<Enemy> enemies, List<Missile> missiles)
        {
            Enemies = enemies;
            Missiles = missiles;
        }
        public Stage(Action<Vector2[]> script)
        {
            Enemies = new List<Enemy>();
            Missiles = new List<Missile>();
            MoveScript = script;
        }

        public bool IsEmpty() => Enemies.Count == 0 && Missiles.Count == 0;
    }
}
