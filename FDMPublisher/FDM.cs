using DDS;
using System.Runtime.InteropServices;

namespace FDMData
{
    #region FDM
    [StructLayout(LayoutKind.Sequential)]
    public sealed class FDM
    {
        public int userID;
        public double Lattitude;
        public double Longitude;
        public double Altitude;
        public double aa;
    };
    #endregion

}

