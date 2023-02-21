using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MovilidadCF.Windows.Forms
{
	/// <summary>
	/// Descripción breve de Drawing.
	/// </summary>
	internal class DrawingHelper
	{
		public static void DrawGradient(Graphics g, Color color1, Color color2, Rectangle rect, bool horizontal, bool normal)
		{
			Pen pen = null;

			//draw the lines
			if( horizontal )
			{
				if(normal)
				{
					//Draw Normally
					for(int i=0;i<rect.Width;i++)
					{
						Color currColor = currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Width);;
						pen = new Pen(currColor);
						g.DrawLine(pen, rect.X + i, rect.Top, rect.X + i, rect.Height );
						pen.Dispose();
					}
				}
				else
				{
					//Draw from the middle out
					for(int i=0;i<=rect.Width/2;i++)
					{
						Color currColor = currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Width/2);
						pen = new Pen(currColor);
						g.DrawLine(pen, rect.X + i, rect.Top, rect.X + i, rect.Bottom );
						g.DrawLine(pen, rect.Width - i, rect.Top, rect.Width - i, rect.Height );
						pen.Dispose();
					}
				}
			}
			else
			{
				if(normal)
				{
					//Draw Normally
					for(int i=0;i<rect.Height;i++)
					{
						Color currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Height);
						pen = new Pen(currColor);
						g.DrawLine(pen, rect.X, rect.Top + i, rect.X , rect.Height );
						pen.Dispose();
					}
				}
				else
				{
					//Draw from the middle out
					for(int i=0;i<=rect.Height/2;i++)
					{
						Color currColor = currColor = InterpolateLinear(color1, color2, (float)i, (float)0, (float)rect.Height/2);
						pen = new Pen(currColor);
						g.DrawLine(pen, rect.X, rect.Top + i, rect.X,  rect.Top + i + 1 );						
						g.DrawLine(pen, rect.X, rect.Height-i, rect.X, rect.Height-i - 1);
						pen.Dispose();
					}
				}
			}
			

		}

		public static  Color InterpolateLinear(Color first, Color second, float position, float start, float end) 
		{ 		
			float R = ((second.R)*(position - start) + (first.R)*(end-position))/(end-start); 
			float G = ((second.G)*(position - start) + (first.G)*(end-position))/(end-start); 
			float B = ((second.B)*(position - start) + (first.B)*(end-position))/(end-start); 
			return Color.FromArgb((int)Math.Round((double)R), (int)Math.Round((double)G), (int)Math.Round((double)B)); 		
		}
	}
}
