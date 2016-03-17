using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PresentationTrainer
{
    class IndividualDancing: IndividualTracker
    {
        public IndividualDancing (MainWindow parent)
        {
            myVoiceAndMovementObject = new PresentationAction();
            periodicMovements = new PeriodicMovements();

            tempMistakeList = new ArrayList();
            mistakeList = new ArrayList();
            voiceAndMovementsList = new ArrayList();
            audioMovementMistakeTempList = new ArrayList();
            bodyMistakes = new ArrayList();
            this.parent = parent;
        }
        public override void analyze()
        {
            bfpa = this.parent.bodyFrameHandler.bodyFramePreAnalysis;
            apa = this.parent.audioHandler.audioPreAnalysis;

            mistakeList = new System.Collections.ArrayList();

            if (bfpa.body != null)
            {
                if (periodicMovements.checPeriodicMovements(bfpa.body))
                {
                    PresentationAction pa = new PresentationAction(PresentationAction.MistakeType.DANCING);
                    mistakeList.Insert(0, pa);
                }
            }
        }
    }
}
