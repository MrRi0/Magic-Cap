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

        private Vector2[] _trajectoryDashAttack;
        private int _stageIndex;
        

        public BossStage(Boss boss, List<Stage> stages, Vector2[] trajectoryDashAttack)
        {
            Boss = boss;
            Stages = stages;
            _trajectoryDashAttack = trajectoryDashAttack;
            CurrentStage = GetNextStage();
        }

        public void Update()
        {
            Boss.Update();
            StageIsSwitch = false;

            if (Boss.IsDeath())
                return;

            if (Boss.Hp < (double)Boss.Hp / Stages.Count * (_stageIndex + 1)
                || (CurrentStage.IsEmpty() && Boss.AttackTrajectory == Vector2.Zero))
            {
                CurrentStage = GetNextStage();
                StageIsSwitch = true;
            }
        }

        private Stage GetNextStage()
        {
            Boss.GetNextStage();
            var nextStage = Stages[_stageIndex];
            if (nextStage.IsEmpty())
            { 
                nextStage.Script();
                Boss.SetTrajectory(_trajectoryDashAttack);
            }
            _stageIndex++;
            return nextStage;
        }
    }
}
