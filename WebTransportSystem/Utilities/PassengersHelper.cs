﻿using System;
using System.Collections.Generic;
using System.Linq;
using WebTransportSystem.Models;
using WebTransportSystem.Models.TransportChooseAlgorithm;

namespace WebTransportSystem.Utilities
{
    public static class PassengersHelper
    {
        public static Passenger CreatePassenger(int number, TransmissionType transmissionType)
        {
            var rnd = new Random();
            var transport = rnd.Next(2) == 1 ? TransportType.Bus : TransportType.Car;
            var quality = Math.Round(rnd.NextDouble(), 2);
            var satisfaction = Math.Round(rnd.NextDouble(), 2);
            return new Passenger(transport, transmissionType, quality, satisfaction, number);
        }

        public static void SetNeighborsPassengers(Passenger[][] passengers)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    if (i > 0)
                    {
                        passengers[i][j].AddNeighbor(passengers[i - 1][j]);
                    }
                    if (j > 0)
                    {
                        passengers[i][j].AddNeighbor(passengers[i][j - 1]);
                    }
                    if (i < rowCount - 1)
                    {
                        passengers[i][j].AddNeighbor(passengers[i + 1][j]);
                    }
                    if (j < columnCount - 1)
                    {
                        passengers[i][j].AddNeighbor(passengers[i][j + 1]);
                    }
                }
            }
        }

        public static void ClearNeighborsPassengers(Passenger[][] passengers)
        {
            var rowCount = passengers.Length;
            var columnCount = passengers.First().Length;
            for (var i = 0; i < rowCount; i++)
            for (var j = 0; j < columnCount; j++)
                passengers[i][j].Neighbors = new HashSet<Passenger>();
        }
    }
}