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
        public CoordinationTask(ObservableCollection<OneBit> arBits)
        {
            this.arBits = arBits;
            int i = 0;
            foreach(OneBit ob in arBits)
            {
                i++;
                allHand += ob.hand;
                allLeg += ob.leg;
                if(i == 4)
                {
                    allHand += " ";
                    allLeg += " ";
                    i = 0;
                }
            }
        }
        public CoordinationTask(string strHand, string strLeg)
        {
            allHand = strHand;
            allLeg = strLeg;
            SetArray(strHand, strLeg);
        }
        public CoordinationTask() { }

        ObservableCollection<OneBit> arBits = new ObservableCollection<OneBit>();
        private string allHand;
        private string allLeg;


        public string AllHand { get => allHand; set => allHand = value; }
        public string AllLeg { get => allLeg; set => allLeg = value; }

        void SetArray(string strHand, string strLeg)
        {
            for (int i = 0; i < BitCount; i++)
            {
                OneBit ob = new OneBit() { hand = strHand[i].ToString(), leg = strLeg[i].ToString() };
                this.arBits.Add(ob);
            }
        }

        public void SetArray()
        {
            string _allHand = this.AllHand.ToString().Replace(" ", "");
            string _allLeg = this.AllLeg.ToString().Replace(" ", "");

            for (int i = 0; i < BitCount; i++)
            {
                OneBit ob = new OneBit() 
                { 
                    hand = _allHand[i].ToString(), 
                    leg = _allLeg[i].ToString()
                };
                this.arBits.Add(ob);
            }
        }
        public ObservableCollection<OneBit> GetOneBitColl()
        {
            return this.arBits;
        }
        public bool AddInOneBitColl(OneBit ob)
        {
            if (this.arBits.Count() == 12)
            {
                return false;
            }

            this.arBits.Add(ob);
            return true;
        }
    }

}
