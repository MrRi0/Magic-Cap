using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Model
{
    public class Stage
    {
        public List<Enemy> Enemies { get; private set; }
        public List<Missile> Missiles { get; private set; }
        public Action Script { get; private set; }

        public Stage(List<Enemy> enemies, List<Missile> missiles)
        {
            Enemies = enemies;
            Missiles = missiles;
        }
        public Stage(List<Enemy> enemies, List<Missile> missiles, Action script)
        {
            Enemies = enemies;
            Missiles = missiles;
            Script = script;
        }

        public bool IsEmpty() => Enemies.Count == 0 && Missiles.Count == 0;
    }
}
