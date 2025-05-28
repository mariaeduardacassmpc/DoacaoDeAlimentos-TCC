using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCDoacaoDeAlimentos.Shared.Dto
{
    public class Geolocalizacao
    {
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public double? altitude { get; set; }
    }
}
