using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpectroWizard.util
{
    public class FRectangle
    {
        float X1, X2, Y1, Y2;
        
        public float XFrom  {   get { return X1; }  }
        public float XTo    {   get { return X2; }  }
        public float YFrom  {   get { return Y1; }  }
        public float YTo    {   get { return Y2; }  }
        
        public float Width  { get { return X2 - X1; } }
        public float Height { get { return Y2 - Y1; } }

        public FRectangle()
        {
        }

        public FRectangle(FRectangle f)
        {
            InitBy(f);
        }

        public void InitBy(FRectangle f)
        {
            X1 = f.X1;
            X2 = f.X2;
            Y1 = f.Y1;
            Y2 = f.Y2;
        }

        public void SetupX(float from, float to)
        {
            if (from > to)
            {
                float tmp = to;
                to = from;
                from = tmp;
            }
            X1 = from;
            X2 = to;
        }

        public void SetupY(float from, float to)
        {
            if (from > to)
            {
                float tmp = to;
                to = from;
                from = tmp;
            }
            Y1 = from;
            Y2 = to;
        }

        public void FitIn(FRectangle f)
        {
            if (X1 >= f.X2)
            {
                X1 = f.X1;
                X2 = f.X2;
            }
            else
            {
                if (X1 < f.X1)
                    X1 = f.X1;
                if (X2 > f.X2)
                    X1 = f.X2;
            }
            if (Y1 >= f.Y2)
            {
                Y1 = f.Y1;
                Y2 = f.Y2;
            }
            else
            {
                if (Y1 < f.Y1)
                    Y1 = f.Y1;
                if (Y2 > f.Y2)
                    Y2 = f.Y2;
            }
        }

        public void Unite(FRectangle f)
        {
            if (X1 > f.X1)  X1 = f.X1;
            if (X2 < f.X2)  X2 = f.X2;
            if (Y1 > f.Y1) Y1 = f.Y1;
            if (Y2 < f.Y2) Y2 = f.Y2;
        }

        public FRectangle(float x1, float y1, float x2, float y2)
        {
            if (x1 < x2)
            {
                X1 = x1;
                X2 = x2;
            }
            else
            {
                X2 = x1;
                X1 = x2;
            }
            if (y1 < y2)
            {
                Y1 = y1;
                Y2 = y2;
            }
            else
            {
                Y2 = y1;
                Y1 = y2;
            }
        }
    }
}
