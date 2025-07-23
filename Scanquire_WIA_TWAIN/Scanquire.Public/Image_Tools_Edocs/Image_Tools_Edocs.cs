//using code from https://www.codeproject.com https://www.codeproject.com/Articles/33838/Image-Processing-using-C and per The Code Project Open License (CPOL) https://www.codeproject.com/info/cpol10.aspx can use code
//icons come from <a href="https://www.flaticon.com/free-icons/undo" title="undo icons">Undo icons created by Md Tanvirul Haque - Flaticon</a>
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scanquire.Public.Image_Tools_Edocs
{
    public partial class Edocs : Form
    {
         ImageHandler imageHandler = new  ImageHandler();
        double zoomFactor = 1.0;
        public Edocs()
        {
            InitializeComponent();
        }
    }
}
