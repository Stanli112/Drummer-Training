using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoordinationTraining.Classes
{
    public class CoordinationTask
    {
        private const int BitCount = 12;
        public CoordinationTask(string strHand, string strLeg)
        {

            allHand = strHand;
            allLeg = strLeg;
            SetArray(strHand, strLeg);
        }
        public CoordinationTask() { }
        ObservableCollection<OneBit> arBits = new ObservableCollection<OneBit>();
        string allHand { get; set; }
        string allLeg { get; set; }

        void SetArray(string strHand, string strLeg)
        {
            for(int i = 0; i < BitCount; i++)
            {
                OneBit ob = new OneBit() { hand = strHand[i].ToString(), leg = strLeg[i].ToString() };
            }
        }

        public ObservableCollection<OneBit> GetOneBitColl()
        {
            return this.arBits;
        }
        public bool AddInOneBitColl(OneBit ob)
        {
            if(this.arBits.Count() == 12)
            {
                return false;
            }

            this.arBits.Add(ob);
            return true;
        }
    }

}
