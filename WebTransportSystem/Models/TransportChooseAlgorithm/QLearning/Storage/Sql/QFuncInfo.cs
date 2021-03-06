﻿using System.ComponentModel.DataAnnotations;

namespace WebTransportSystem.Models.TransportChooseAlgorithm.QLearning.Storage.Sql
{
    public class QFuncInfo
    {
        [Key]
        public string State { get; set; }
        public double CarReward { get; set; }
        public double BusReward { get; set; }

        public double GetBestReward()
        {
            return CarReward > BusReward ? CarReward : BusReward;
        }
    }
}