using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SpectroWizard.data;

namespace SpectroWizard.method
{
    public class SortingProgram
    {
        public SortingProgram()
        {
        }

        public string Parse(string text)
        {
            string str = "";
            int type = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (text[i] == (char)0xD)
                    continue;
                if (type == 0 && text[i] == '#')
                {
                    for (; i < text.Length && text[i] == '\n'; i++);
                    continue;
                }
                if (text[i] == '"' | text[i] == '\'')
                {
                    if (type == 0)
                        type = 1;
                    else
                        type = 0;
                }
                if(text[i] == 0xA)
                {
                    if(str.Length == 0 ||
                        (str.Length > 0 && text[i-1] == 0xA))
                        continue;
                }
                str += text[i];
            }
            return str;
        }
    }

    public abstract class SortingProgramNode
    {
        public enum StepType
        {
            Block,
            CheckExisting,
            CheckRelation,
            Any,
            All
        }

        StepType Type;
        protected SortingProgramNode(StepType type)
        {
            Type = type;
        }

        public StepType GetType()
        {
            return Type;
        }
        abstract public bool Run(Spectr sp);
        public static SortingProgramNode TryParse(string text, ref int position)
        {
            return null;
        }
    }

    public class SPNComment : SortingProgramNode
    {
        public SPNComment()
            : base(SortingProgramNode.StepType.Block)
        {
        }

        public override bool Run(Spectr sp)
        {
            throw new NotImplementedException();
        }
    }
}
