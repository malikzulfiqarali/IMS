using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;


namespace IMS
{
    public class Photo
    {
       public byte[] SavePhoto()
        {
            CustomerDefinitionForm customerDefinitionForm = new CustomerDefinitionForm();
            MemoryStream ms = new MemoryStream();
            customerDefinitionForm.CustomerPictureBox.Image.Save(ms, customerDefinitionForm.CustomerPictureBox.Image.RawFormat);
            return ms.GetBuffer();

        }
        public Image GetPhoto(byte[] photo)
        {
            MemoryStream ms = new MemoryStream(photo);
            return Image.FromStream(ms);
        }
    }
}
