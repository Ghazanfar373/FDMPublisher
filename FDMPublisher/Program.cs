using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDS;
using DDS.OpenSplice;
using FDMData;
using DDSErrorHandle;
using System.Threading;


namespace FDMPublisher
{
    class Program
    {
        static private FDMTypeSupport fdmDataType;
        static private ITopic fdmDataTopic;
        static private PublisherQos pQos;
        static private IPublisher Publisher;
        static private DataWriterQos dwQos;
        static private IDataWriter parentWriter;
        static private FDMDataWriter fdmDataWriter;
        static public FDM fdmData;


        private const string Domain = "DDSDomain";
        private const string PartitionName = "DDSDomain";

        static private DomainParticipantFactory _dpf;
        static private IDomainParticipant _dp;

        static void Main(string[] args)
        {
            _dpf = DomainParticipantFactory.Instance;
            ErrorHandler.checkHandle(_dpf, "Domain Participant Factory");

            _dp = _dpf.CreateParticipant(Domain);

            //Initialize QOS
            pQos = new PublisherQos();
            dwQos = new DataWriterQos();

            fdmDataType = new FDMTypeSupport();
            string FDMDATATypeName = fdmDataType.TypeName;
            ReturnCode status = fdmDataType.RegisterType(_dp, FDMDATATypeName);
            ErrorHandler.checkStatus(status, "FDMDATA: Cannot Register Type");


            //Create FDMDATA Topic
            fdmDataTopic = _dp.FindTopic("FDMDATA", Duration.FromMilliseconds(1000));
            if (fdmDataTopic == null)
                fdmDataTopic = _dp.CreateTopic("FDMDATA", FDMDATATypeName);
            ErrorHandler.checkHandle(fdmDataTopic, "Cannot Create Topic FDMDATA");

            //Get Publisher QOS and Set Partition Name
            _dp.GetDefaultPublisherQos(ref pQos);
            pQos.Partition.Name = new String[1];
            pQos.Partition.Name[0] = PartitionName;

            //Create Subscriber for FDMDATA Topic
            Publisher = _dp.CreatePublisher(pQos);
            ErrorHandler.checkHandle(Publisher, "Cannot Create FDMDATA Publisher");

            //Get Data Writer QOS and Set History Depth
            Publisher.GetDefaultDataWriterQos(ref dwQos);
            ErrorHandler.checkHandle(dwQos, "Cannot get Data Writer Qos");
            dwQos.History.Depth = 5;
            dwQos.Reliability.Kind = ReliabilityQosPolicyKind.BestEffortReliabilityQos;


            //Create DataReader for FDMDATA Topic
            parentWriter = Publisher.CreateDataWriter(fdmDataTopic, dwQos);
            ErrorHandler.checkHandle(parentWriter, "Cannot Create FDMDATA Data Writer");

            //Narrow abstract parentWriter into its typed representative
            fdmDataWriter = parentWriter as FDMDataWriter;
            ErrorHandler.checkHandle(fdmDataWriter, "Cannot Narrow FDMDATA Data Writer");

            fdmData = new FDM();
            while (true)
            {
                StartPublish();
                Console.WriteLine("Publishing data! ");
                Thread.Sleep(10);
            }
        }

        static void StartPublish()
        {

            fdmData.userID = 1;
            fdmData.Lattitude = 32.33;
            fdmData.Longitude = 72.74;
            fdmData.Altitude = 10000;
            fdmData.aa = 099;

            InstanceHandle handle = fdmDataWriter.RegisterInstance(fdmData);
            ErrorHandler.checkHandle(handle, "FDMDataWriter.RegisterInstance");

            ReturnCode status = fdmDataWriter.Write(fdmData, handle);
            ErrorHandler.checkStatus(status, "Unable to Write : FDMDATA");

        }
    }
}
