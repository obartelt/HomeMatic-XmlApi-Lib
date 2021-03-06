﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace TRoschinsky.Lib.HomeMaticXmlApi
{
    public class HMDeviceChannel : HMBase
    {
        private const string default1stDataTypeName = "STATE";
        private const string default2ndDataTypeName = "MOTION";
        private const string default3rdDataTypeName = "ACTUAL_TEMPERATURE";
        private const string default4thDataTypeName = "STICKY_UNREACH";

        private string defaultDataType = String.Empty;
        public string DefaultDataType 
        {
            get { return String.IsNullOrEmpty(defaultDataType) ? default1stDataTypeName : defaultDataType; }
            set { defaultDataType = value; } 
        }
        public HMDeviceDataPoint PrimaryDataPoint { get { return GetPrimaryDataPoint(); } }
        public object PrimaryValue { get { return GetPrimaryDataPoint().Value; } }
        public DateTime PrimaryLastUpdate { get { return GetPrimaryDataPoint().LastUpdate; } }
        private Dictionary<string, HMDeviceDataPoint> dataPoints = new Dictionary<string, HMDeviceDataPoint>();
        public Dictionary<string, HMDeviceDataPoint> DataPoints { get { return dataPoints; } }

        public void AddDataPoint(string type, HMDeviceDataPoint dataPoint)
        {
            dataPoints.Add(type, dataPoint);
        }

        public override string ToString()
        {
            return String.Format("DCH: {0} >> {1} - Value '{2}' @ {3}", Address, Name, PrimaryValue, PrimaryLastUpdate);
        }

        private HMDeviceDataPoint GetPrimaryDataPoint()
        {
            try
            {
                if (dataPoints.Count > 0)
                {
                    if (dataPoints.ContainsKey(default1stDataTypeName))
                    {
                        return dataPoints[default1stDataTypeName];
                    }
                    else if (dataPoints.ContainsKey(default2ndDataTypeName))
                    {
                        return dataPoints[default2ndDataTypeName];
                    }
                    else if (dataPoints.ContainsKey(default3rdDataTypeName))
                    {
                        return dataPoints[default3rdDataTypeName];
                    }
                    else if (dataPoints.ContainsKey(default4thDataTypeName))
                    {
                        return dataPoints[default4thDataTypeName];
                    }
                    else
                    {
                        return dataPoints[dataPoints.Keys.First()];
                    }
                }

                return new HMDeviceDataPoint();
            }
            catch
            {
                return new HMDeviceDataPoint();
            }
        }

        private List<int> GetContainedChannels()
        {
            List<int> result = new List<int>();
            try
            {
                if (dataPoints.Count > 0)
                {
                    foreach (KeyValuePair<string, HMDeviceDataPoint> datapoint in dataPoints)
                    {
                        result.Add(datapoint.Value.InternalId);
                    }
                }
            }
            catch (Exception)
            {
                // nothing to do here
            }

            return result;
        }
    }
}
