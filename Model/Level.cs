using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Model
{
    public class Level
    {
        public readonly int Number;
        private Queue<List<Enemy>> _stages;

        public Level(int number, Queue<List<Enemy>> stages)
        {
            Number = number;
            _stages = stages;
        }

        public List<Enemy> GetNextStage()
        {
            if (_stages.Count != 0)
                return _stages.Dequeue();
            return null;
        }
    }
}
