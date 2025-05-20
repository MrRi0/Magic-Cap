using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameWinForm.Model
{
    public class BossStage
    {
        public Boss Boss { get; private set; }
        public List<Stage> Stages { get; private set; }
        public Stage CurrentStage { get; private set; }
        public bool StageIsSwitch { get; private set; }
        private int _stageIndex;
        

        public BossStage(Boss boss, List<Stage> stages)
        {
            Boss = boss;
            Stages = stages;
        }

        public Stage GetNextStage()
        {
            var nextStage = Stages[_stageIndex % Stages.Count];
            _stageIndex++;
            return nextStage;
        }
    }
}
