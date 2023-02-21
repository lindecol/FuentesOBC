using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections;
using System.IO;


namespace MovilidadCF.Windows.Forms
{

 /****************************************************************************
     *<author>Gitansu Behera emailId:gbehera@cellexchange.com</author>
     *<version>1.0</version>
     *<since>2004</since>
     *<summary>
     * Title:        SignatureControl
     * Version:      1.0
     * Author:       Geetansu Behera
     * Description:   Collects and displays a signature. The signature is made 
     * up of a list of line segments. Each segment contains an array of points 
     * (x and y coordinates). 
     * Draws to a memory bitmap to prevent flickering.
     * Raises the SignatureUpdate event when a new segment is added to 
     * the signature.
     *         Last updated
     * Modification Log :
     * Date             Author      Comments
     *
     * Unpublished Confidential Information of CellExchange.  Do not disclose.
     * Copyright © 1999-2004 CellExchange.com, Inc.  All Rights Reserved.
     ***************************************************************************
     * </summary>
     *<description></description>
     */

  public class SignatureControl : Control
  {
	private string background = null;
	public string Background
    {
      get 
      {
        return background;
      }
      set
      { 
        background = value;
  
      }

    }

    // gdi objects
    Bitmap bmp;
    
    Graphics graphics;
    Pen pen = null;

	//Lapiz para borrar
	Pen penBorrar = null;
	bool m_bBorrar = false;
	  	  
    // list of line segments
    ArrayList pVector = new ArrayList();
    Point lastPoint = new Point(0,0);
    
    // if drawing signature or not
    bool drawSign = false;

   // Tamaño de la pluma
    private int m_SizePen = 1;

    public int SizePen
    {
        get { return m_SizePen; }
        set { 
            m_SizePen = value;
            pen.Width = m_SizePen;
            penBorrar.Width = m_SizePen;
        }
    }

	// Tamaño de la pluma
	public bool m_bBorrarImagen = false;

    // notify parent that line segment was updated
    public event EventHandler SignatureUpdate;

    public SignatureControl()
    {
        //CREAR LOS LAPICES CON EL ANCHO ESPECIFICADO
        pen = new Pen(Color.Black, m_SizePen);

        //Lapiz para borrar
        penBorrar = new Pen(Color.White, m_SizePen);
    }

    protected override void OnPaint(PaintEventArgs e) 
    {
      // we draw on the memory bitmap on mousemove so there
      // is nothing else to draw at this time 
      CreateGdiObjects();
      e.Graphics.DrawImage(bmp, 0, 0);
    }

    protected override void OnPaintBackground(PaintEventArgs e) 
    {
      // don't pass to base since we paint everything, avoid flashing
    }

    protected override void OnMouseDown(MouseEventArgs e) 
    {
      base.OnMouseDown(e);

      // process if currently drawing signature
      if (!drawSign)
      {
        // start collecting points
        drawSign = true;
        
        // use current mouse click as the first point
        lastPoint.X = e.X;
        lastPoint.Y = e.Y;
      }
    }

    protected override void OnMouseUp(MouseEventArgs e) 
    {
      
      base.OnMouseUp(e);
	  // process if drawing signature
      if (drawSign)
      {
        // stop collecting points
        drawSign = false;   
      }
                  
    }

	  
  
    protected override void OnMouseMove(MouseEventArgs e)
    {
      base.OnMouseMove(e);      

      // process if drawing signature
      if (drawSign)
      {
		string sBorrar;
          // draw the new segment on the memory bitmap
		  int i = 0;
		  if (!m_bBorrar)
		  {
			  sBorrar = "N";
			  graphics.DrawLine(pen, lastPoint.X + i, lastPoint.Y + i, e.X + i, e.Y + i);
			  pVector.Add(lastPoint.X+" "+lastPoint.Y+" "+e.X+" "+e.Y + " " + sBorrar );
		  }
		  else
		  {
			  sBorrar = "B";
			  for ( i=0; i<=4; i++)
			  {
				  graphics.DrawLine(penBorrar, (lastPoint.X - 2) + i, (lastPoint.Y - 2), (e.X + 2) + i, e.Y + 2);
				  pVector.Add((lastPoint.X - 2 + i) +" "+ (lastPoint.Y - 2) + " "+(e.X + 2 + i)+" "+ (e.Y +2) + " " + sBorrar );
			  }
		}
		// update the current position
		lastPoint.X = e.X;
        lastPoint.Y = e.Y;

        // display the updated bitmap
        Invalidate();
      } 
    }

    /// <summary>
    /// Clear the signature.
    /// </summary>
    public void Clear()
    {
      pVector.Clear();
      InitMemoryBitmap();
	  Invalidate();
	}

	 public void Erase(bool bActivar)
	 {
		m_bBorrar = bActivar;
	 }

    /// <summary>
    /// Create any GDI objects required to draw signature.
    /// </summary>
    private void CreateGdiObjects()
    {
      // only create if don't have one or the size changed
      if (bmp == null || bmp.Width != this.Width || bmp.Height != this.Height)
      {
        // memory bitmap to draw on
        InitMemoryBitmap();
      }
    }
    
    /// <summary>
    /// Create a memory bitmap that is used to draw the signature.
    /// </summary>
    private void InitMemoryBitmap()
    {
      // load the background image
      if (this.Background==null) 
      {
        bmp = new Bitmap(this.Width,this.Height);
        graphics = Graphics.FromImage(bmp);
        graphics.Clear(Color.White);
      }
      else 
      {
        bmp = new Bitmap(this.Background);
        // get graphics object now to make drawing during mousemove faster
        graphics = Graphics.FromImage(bmp);
      }
    }
    
    /// <summary>
    /// Notify container that a line segment has been added.
    /// </summary>
    private void RaiseSignatureUpdateEvent()
    {
      if (this.SignatureUpdate != null)
        SignatureUpdate(this, EventArgs.Empty);
    }
    
    private void CreateFile(String fileName,String fileContent) 
    {
	  StreamWriter sr;
		if (File.Exists(fileName)) 
		{
			if (m_bBorrarImagen)
			{
				File.Delete(fileName);
				sr = File.CreateText(fileName);
				m_bBorrarImagen=false;
			}
			else
			{
				sr = File.AppendText(fileName);
			}
		}
		else
		{
			sr = File.CreateText(fileName);
		}
      sr.WriteLine (fileContent);
      sr.Close();
    }

    public void  StoreSigData(String fileName)
    {
      string sigData = "";
      for (int i = 0; i<pVector.Count ;i++) 
      {
        // This commented operation is used to convert decimal to hex and store
        //sigData = sigData + PadHex(int.Parse(xVector[i].ToString()))+" " + PadHex(int.Parse(yVector[i].ToString()))+ "\n";
        sigData = sigData + pVector[i].ToString()+ "\n";
      }
      CreateFile(fileName,sigData); 
                              
    }
    // this method PadHex can be used to convert int to hex format //  
    // not in use
    private string PadHex(int inNum ) 
    {
      uint uiDecimal = checked((uint)System.Convert.ToUInt32(inNum));
      string s = String.Format("{0:x2}", uiDecimal);
      return(s);
    }

	  public bool SaveToFile(Control cont, String fileName, short bpp, int width, int height)
	  {
		  // File and data streaming
		  BinaryWriter bw = null;
		  MemoryStream ms = null;
		  FileStream fs = null;

		  try
		  {
			  // Create the necessary bitmap file and info headers (must do info first)
			  BITMAPINFOHEADER infoHdr = new BITMAPINFOHEADER(bpp, width, height);
			  BITMAPFILEHEADER fileHdr = new BITMAPFILEHEADER(infoHdr);

			  // Create an array of bytes to hold the actual bitmap data.
			  byte[] bytes = new byte[fileHdr.bfSize];

			  // Map that array of bytes to a MemoryStream and create a BinaryWriter
			  ms = new MemoryStream(bytes);
			  bw = new BinaryWriter(ms);

			  // Write the file and info headers to the binary stream
			  fileHdr.Store(bw);
			  infoHdr.Store(bw, false);

			  // Get the HWND of the Control
			  bool captureState = cont.Capture;
			  cont.Capture = true;
			  IntPtr hwnd = Windows.GetCapture();
			  cont.Capture = captureState;

			  // Write the pixel info to the byte stream (bypassing bw)
			  if (!BitmapFile.CopyImageSource(hwnd, bytes, fileHdr, infoHdr))
			  {
				  return false;
			  }

			  // Create a file stream to the path provided
			  fs = File.Create(fileName);

			  // Write out our bitmap data to the file stream
			  fs.Write(bytes, 0, (int)fileHdr.bfSize);
		  }
		  finally
		  {
			  // Close streams still laying around.
			  if (fs != null)
				  fs.Close();
			  if (bw != null)
				  bw.Close();
			  if (ms != null)
				  ms.Close();
		 }

		  return true;
	  }
       
	   public void LoadImagen(Image Imagen)
	  {
		   Rectangle destRect;
		   destRect = new Rectangle(0, 0, Imagen.Width, Imagen.Height);
		   destRect.Height *= 2;
		   destRect.Width *= 2;
		   graphics.DrawImage(Imagen, 0, 0, destRect, GraphicsUnit.Pixel);
		 //graphics.DrawImage(Imagen,0,0);
	}

	public void LoadImage(string signatureFile) 
	  {
		  System.IO.StreamReader streamReader =	new System.IO.StreamReader(signatureFile);
		  string pointString = null;
           
		  while ((pointString = streamReader.ReadLine())!= null) 
		  {
			  if(pointString.Trim().Length>0)
			  {
				  String[] points = new String[4];
				  points = pointString.Split(new Char[]{' '});
				  Pen penLoad;
				  if (points[4] == "N")
				  {
                      penLoad = new Pen(Color.Black, m_SizePen);
				  }
				  else
				  {
                      penLoad = new Pen(Color.White, m_SizePen);
				  }
                  graphics.DrawLine(penLoad, (int.Parse(points[0].ToString())), (int.Parse(points[1].ToString())), (int.Parse(points[2].ToString())), (int.Parse(points[3].ToString())));
			  }
		  }
		  streamReader.Close();
	  }
	  
  }
}
