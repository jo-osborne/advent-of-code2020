using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day05
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var input = "FBFBBFFRLR";

            var seat = BoardingPassReader.Read(input);

            var expectedRow = 44;
            var expectedCol = 5;

            Assert.That(seat.Row, Is.EqualTo(expectedRow));
            Assert.That(seat.Col, Is.EqualTo(expectedCol));
        }

        [Test]
        public void Puzzle01()
        {
            var data = File.ReadAllLines("data.txt");
            var seats = data.Select(BoardingPassReader.Read).ToList();
            var seatIds = seats.Select(s => s.SeatId());
            var highestSeatId = seatIds.OrderByDescending(s => s).First();
            Console.WriteLine($"The highest seat id is {highestSeatId}");
            Assert.That(highestSeatId, Is.EqualTo(998));
        }

        [Test]
        public void Puzzle02()
        {
            var data = File.ReadAllLines("data.txt");
            var seats = data.Select(BoardingPassReader.Read).ToList();
            var freeSeats = SeatAllocator.FindFreeSeats(seats);
            var allocatedSeatIds = seats.Select(s => s.SeatId());

            foreach (var seat in freeSeats)
            {
                var freeSeatId = seat.SeatId();
                if (allocatedSeatIds.Contains(freeSeatId - 1) && allocatedSeatIds.Contains(freeSeatId + 1))
                {
                    Console.WriteLine($"Is this my seat? {freeSeatId}");
                }
            }
        }
    }

    public class BoardingPassReader
    {
        public static Seat Read(string input)
        {
            var rowUpperBound = 128;
            var rowLowerBound = 0;
            var colUpperBound = 8;
            var colLowerBound = 0;

            for (var i = 0; i < 10; i++)
            {
                var x = input[i];

                if (x == 'F')
                {
                    rowUpperBound = rowUpperBound - ((rowUpperBound - rowLowerBound) / 2);
                }
                else if (x == 'B')
                {
                    rowLowerBound = rowLowerBound + ((rowUpperBound - rowLowerBound) / 2);
                }
                else if (x == 'R')
                {
                    colLowerBound = colLowerBound + ((colUpperBound - colLowerBound) / 2);
                }
                else if (x == 'L')
                {
                    colUpperBound = colUpperBound - ((colUpperBound - colLowerBound) / 2);
                }
            }

            if (rowUpperBound - 1 != rowLowerBound) throw new Exception($"rowUpperBound={rowUpperBound}, rowLowerBound={rowLowerBound}");
            if (colUpperBound - 1 != colLowerBound) throw new Exception($"colUpperBound={colUpperBound}, colLowerBound={colLowerBound}");

            return new Seat(rowLowerBound, colLowerBound);
        }
    }

    public class SeatAllocator
    {
        public static List<Seat> FindFreeSeats(List<Seat> allocatedSeats)
        {
            var sorted = allocatedSeats.OrderBy(s => (s.Row, s.Col)).ToList();

            var freeSeats = new List<Seat>();

            for (var row = 0; row < 128; row++)
            {
                for (var col = 0; col < 8; col++)
                {
                    if (!allocatedSeats.Exists(seat => seat.Row == row && seat.Col == col))
                        freeSeats.Add(new Seat(row, col));
                }
            }

            return freeSeats;
        }
    }

    public record Seat(int Row, int Col)
    {
        public int SeatId()
        {
            return (Row * 8) + Col;
        }
    }
}