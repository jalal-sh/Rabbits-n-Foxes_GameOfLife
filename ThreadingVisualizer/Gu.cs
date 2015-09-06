using System;
using System.Drawing;

namespace WindowTransformation
{
class Gu
{
    float Sx, Sy, Tx, Ty;
    Brush brush;
    Pen pen;
    Font font;
    Color[] color={Color.AliceBlue,Color.Red,Color.Green};
    public Gu()
    {
        brush = Brushes.Black;
        pen = new Pen(brush);
        font = new Font("Times New Roman", 12.0f);
    }

    public void GuLine(double x1, double y1, double x2, double y2, Graphics g)
    {
        int ixx1 = (int)Math.Round(Sx * x1 + Tx);
        int ixx2 = (int)Math.Round(Sx * x2 + Tx);
        int iyy1 = (int)Math.Round(-Sy * y1 + Ty);
        int iyy2 = (int)Math.Round(-Sy * y2 + Ty);
        g.DrawLine(pen, ixx1, iyy1, ixx2, iyy2);
    }

    public void GuText(String s, double x, double y, Graphics g)
    {
        int ix = (int)Math.Round(Sx * x + Tx);
        int iy = (int)Math.Round(-Sy * y + Ty);
        g.DrawString(s, font, brush, ix, iy);
        
    }

    public void GuSetColor(int c)
    {
        pen.Color = color[c];
        brush = pen.Brush;
    }

    public void GuSetFont(int lThick)
    {
        font = new Font("Times New Roman", lThick);
    }

    public void GuCloseGraph()
    {
        Environment.Exit(0);
    }

    public void GuWindowView(float wxb, float wyb, float wxt, float wyt,
                             float vxt, float vyt, float vxb, float vyb)
    {
        Sx = (vxb - vxt) / (wxt - wxb);
        Sy = (vyb - vyt) / (wyt - wyb);
        Tx = vxt - Sx * wxb;
        Ty = vyb + Sy * wyb;
    }

    public float GuPickX(int xdc)
    {
        return ((float)xdc - Tx) / Sx;
    }

    public float GuPickY(int ydc)
    {
        return (-(float)ydc + Ty) / Sy;
    }
}
}
