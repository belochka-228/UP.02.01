using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesToys
{
    public partial class Products
    {
        public string ImagePath => Image ?? "/Image/picture.png";
    }
}
