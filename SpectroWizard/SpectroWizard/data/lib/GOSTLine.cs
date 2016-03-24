using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectroWizard.data.lib
{
    public class GOSTLine
    {
        public string Element;
        public int IonLevel;
        public double Ly;
        public bool IsComp;
        public GOSTLine(string element, string[] fields, bool isFirst,string baseElement)
        {
            Element = element;
            if (isFirst)
            {
                parseLy(fields[0]);
                IsComp = Element.Equals(baseElement);
            }
            else
            {
                parseLy(fields[2]);
                IsComp = true;
            }
        }

        void parseLy(string field)
        {
            if(field.Equals("F")){
                Ly = -1;
                return;
            }
            String[] fields = field.Trim().Split(' ');
            if (fields.Length == 2)
            {
                IonLevel = serv.ParseRim(fields[0]);
                Ly = serv.ParseDouble(fields[1]) * 10;
            }
            else
                Ly = serv.ParseDouble(fields[0])*10;
        }

        public string getFullDescrition()
        {
            string ret = "";
            if (IsComp)
                ret += "Линия сравнения ";
            ret += Ly;
            return ret;
        }
    }
}
