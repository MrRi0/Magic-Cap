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
        public Queue<Stage> Stages { get; private set; }
        public Stage CurrentStage { get; private set; }
        
        public BossStage(Boss boss, List<Stage> stages)
        {
            Boss = boss;
            Stages = new Queue<Stage>(stages);
        }

        public Stage GetNextStage()
        {
            if (Stages.Count <= 0)
                return Stage.Empty;
            var nextStage = Stages.Dequeue();
            return nextStage;
        }
    }
}
