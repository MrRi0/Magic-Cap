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
        private Queue<Stage> _stages;
        private BossStage _bossStage;

        public Level(int number, Queue<Stage> stages, BossStage bossStage)
        {
            Number = number;
            _stages = stages;
            _bossStage = bossStage;
        }

        public Stage GetNextStage()
        {
            if (_stages.Count != 0)
                return _stages.Dequeue();
            return Stage.Empty;
        }

        public BossStage GetBossStage() => _bossStage;
    }
}
