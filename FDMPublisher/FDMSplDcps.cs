using DDS;
using DDS.OpenSplice.CustomMarshalers;
using System;
using System.Runtime.InteropServices;

namespace FDMData
{
    #region __FDM
    [StructLayout(LayoutKind.Sequential)]
    public sealed class __FDM
    {
        public int userID;
        public double Lattitude;
        public double Longitude;
        public double Altitude;
        public double aa;
    }
    #endregion

    #region FDMMarshaler
    public sealed class FDMMarshaler : DDS.OpenSplice.CustomMarshalers.DatabaseMarshaler
    {
        private static readonly Type type = typeof(__FDM);
        public static readonly int Size = Marshal.SizeOf(type);

        private static readonly int offset_userID = (int)Marshal.OffsetOf(type, "userID");
        private static readonly int offset_Lattitude = (int)Marshal.OffsetOf(type, "Lattitude");
        private static readonly int offset_Longitude = (int)Marshal.OffsetOf(type, "Longitude");
        private static readonly int offset_Altitude = (int)Marshal.OffsetOf(type, "Altitude");
        private static readonly int offset_aa = (int)Marshal.OffsetOf(type, "aa");


        public override void InitEmbeddedMarshalers(IDomainParticipant participant)
        {
        }

        public override object[] SampleReaderAlloc(int length)
        {
            return new FDM[length];
        }

        public override bool CopyIn(System.IntPtr basePtr, System.IntPtr from, System.IntPtr to)
        {
            GCHandle tmpGCHandle = GCHandle.FromIntPtr(from);
            object fromData = tmpGCHandle.Target;
            return CopyIn(basePtr, fromData, to, 0);
        }

        public override bool CopyIn(System.IntPtr basePtr, object untypedFrom, System.IntPtr to, int offset)
        {
            FDM from = untypedFrom as FDM;
            if (from == null) return false;
            Write(to, offset + offset_userID, from.userID);
            Write(to, offset + offset_Lattitude, from.Lattitude);
            Write(to, offset + offset_Longitude, from.Longitude);
            Write(to, offset + offset_Altitude, from.Altitude);
            Write(to, offset + offset_aa, from.aa);
            return true;
        }

        public override void CopyOut(System.IntPtr from, System.IntPtr to)
        {
            GCHandle tmpGCHandleTo = GCHandle.FromIntPtr(to);
            object toObj = tmpGCHandleTo.Target;
            CopyOut(from, ref toObj, 0);
            if (toObj != tmpGCHandleTo.Target) tmpGCHandleTo.Target = toObj;
        }

        public override void CopyOut(System.IntPtr from, ref object to, int offset)
        {
            FDM dataTo = to as FDM;
            if (dataTo == null) {
                dataTo = new FDM();
                to = dataTo;
            }
            dataTo.userID = ReadInt32(from, offset + offset_userID);
            dataTo.Lattitude = ReadDouble(from, offset + offset_Lattitude);
            dataTo.Longitude = ReadDouble(from, offset + offset_Longitude);
            dataTo.Altitude = ReadDouble(from, offset + offset_Altitude);
            dataTo.aa = ReadDouble(from, offset + offset_aa);
        }

    }
    #endregion

}

