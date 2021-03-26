using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Datmin_Projek_2
{
    public class Iris
    {
        //Intinya ini semua isi data dari data training, apa itu data training????
        public double sp_length;
        public double sp_width;
        public double pt_length;
        public double pt_width;
        public string clas;

        public Iris(double sp_length, double sp_width, double pt_length, double pt_width, string clas)
        {
            this.sp_length = sp_length;
            this.sp_width = sp_width;
            this.pt_length = pt_length;
            this.pt_width = pt_width;
            this.clas = clas;
        }

        public override string ToString()
        {
            return "" + sp_length + "," + sp_width + "," + pt_length + "," + pt_width + "," + clas;
        }

        public double[] getNum()
        {
            return new double[] { sp_length,sp_width, pt_length, pt_width };
        }
    }
}
