﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DevOpsMetrics.Core
{
    /// <summary>
    /// Mean time to restore (MTTR): How quickly we can restore production in an outage or degradation
    /// </summary>
    public class MeanTimeToRestore
    {
        private List<KeyValuePair<DateTime, TimeSpan>> MeanTimeToRestoreList;

        public MeanTimeToRestore()
        {
            MeanTimeToRestoreList = new List<KeyValuePair<DateTime, TimeSpan>>();
        }

        public float ProcessMeanTimeToRestore(List<KeyValuePair<DateTime, TimeSpan>> meanTimeToRestoreList, string resourceName, int numberOfDays)
        {
            if (meanTimeToRestoreList != null)
            {
                foreach (KeyValuePair<DateTime, TimeSpan> item in meanTimeToRestoreList)
                {
                    AddMeanTimeToRestore(resourceName, item.Key, item.Value);
                }
            }
            return CalculateMeanTimeToRestore(resourceName, numberOfDays);
        }

        private bool AddMeanTimeToRestore(string resourceName, DateTime eventDateTime, TimeSpan restoreDuration)
        {
            MeanTimeToRestoreList.Add(new KeyValuePair<DateTime, TimeSpan>(eventDateTime, restoreDuration));
            return true;
        }

        private float CalculateMeanTimeToRestore(string resourceName, int numberOfDays)
        {
            List<KeyValuePair<DateTime, TimeSpan>> items = GetMeanTimeToRestore(resourceName, numberOfDays);

            //Count up the total MTTR minutes
            double totalMinutes = 0;
            foreach (KeyValuePair<DateTime, TimeSpan> item in items)
            {
                totalMinutes += item.Value.TotalMinutes;
            }

            //Calculate mean time for changes per day
            float meanTimeForChanges = 0;
            if (items.Count > 0)
            {
                meanTimeForChanges = (float)totalMinutes / (float)items.Count;
            }

            meanTimeForChanges = (float)Math.Round((double)meanTimeForChanges, 4);

            return meanTimeForChanges;
        }

        //Filter the list by date
        private List<KeyValuePair<DateTime, TimeSpan>> GetMeanTimeToRestore(string resourceName, int numberOfDays)
        {
            return MeanTimeToRestoreList.Where(x => x.Key > DateTime.Now.AddDays(-numberOfDays)).ToList();
        }
    }
}
