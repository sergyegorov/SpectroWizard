using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SpectroWizard.data.lib
{
    public class GOST
    {
        public static string ParseErrorCandidate = "";

        public string GOSTIndex;
        public string BaseElement;
        public string Description;
        public List<GOSTLine> LineInfo = new List<GOSTLine>();
        public GOST(String fileName)
        {
            StreamReader fs = new StreamReader(fileName);
            try
            {
                String text = fs.ReadToEnd();
                String[] lines = text.Split('\n');
                String[] fields = lines[1].Split(';');
                try
                {
                    GOSTIndex = fields[0];
                    BaseElement = fields[1];
                    Description = fields[2];
                }
                catch (Exception ex)
                {
                    Log.OutNoMsg(ex);
                    throw ex;
                }
                String element = null;
                for (int i = 2; i < lines.Length; i++)
                {
                    ParseErrorCandidate = "File: " + fileName + " Line [" + lines[i] + "]";
                    fields = lines[i].Split(';');
                    String cand = fields[0].Replace(':',' ').Trim();
                    if (cand.Length <= 2 && cand.Length > 0 && Char.IsLetter(cand[0]))
                    {
                        if(ElementTable.FindIndex(cand) < 0)
                            throw new Exception("Wrong name of element "+cand);
                        element = cand;
                    }
                    else
                    {
                        //for (; i < lines.Length; i++)
                        try
                        {
                            string[] lfields = lines[i].Split(';');
                            GOSTLine li;
                            if (lfields[0].Trim().Length > 0)
                            {
                                li = new GOSTLine(element, lfields, true, BaseElement);
                                LineInfo.Add(li);
                            }
                            //string[] lfields = lines[i].Split(';');
                            if (lfields[2].Trim().Length > 0)
                            {
                                li = new GOSTLine(element, lfields, false, BaseElement);
                                LineInfo.Add(li);
                            }
                        }
                        catch(Exception ex)
                        {
                            Log.OutNoMsg(ex);
                        }
                    }
                }
            }
            finally
            {
                fs.Close();
            }
        }
    }
}
