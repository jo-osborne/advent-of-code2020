using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day11
{
    class Program
    {
        private static string[] example01 = new string[]
            {
                "L.LL.LL.LL",
                "LLLLLLL.LL",
                "L.L.L..L..",
                "LLLL.LL.LL",
                "L.LL.LL.LL",
                "L.LLLLL.LL",
                "..L.L.....",
                "LLLLLLLLLL",
                "L.LLLLLL.L",
                "L.LLLLL.LL"
            };

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("data.txt");
            Seats(input);

            //var seatingArea = new SeatingArea(input);
            //Console.WriteLine(seatingArea.OccupiedSeats());
        }

        static void Seats(string[] input)
        {
            var last = new string[0];
            var next = input;

            while (!AreEqual(last, next))
            {
                Console.WriteLine("next round of musical chairs");

                last = next;
                next = new string[input.Length];

                for (var lineIdx = 0; lineIdx < last.Length; lineIdx++)
                {
                    var line = last[lineIdx];
                    var nextLine = "";
                    for(var seatIdx = 0; seatIdx < line.Length; seatIdx++)
                    {
                        var seat = line[seatIdx];

                        if (seat == '.')
                        {
                            nextLine += seat;
                        }
                        else if (seat == 'L')
                        {
                            nextLine = ShouldOccupy(last, lineIdx, seatIdx) ? $"{nextLine}#" : $"{nextLine}L";
                        }
                        else if (seat == '#')
                        {
                            nextLine = ShouldVacate(last, lineIdx, seatIdx) ? $"{nextLine}L" : $"{nextLine}#";
                        }
                    }
                    next[lineIdx] = nextLine;
                }
            };

            Console.WriteLine($"Number of occupied seats: {next.Sum(s => s.Count(c => c == '#'))}");

        }

        static bool ShouldOccupy(string[] seats, int lineIdx, int seatIdx)
        {
            var inSight = new char[]
            {
                UpLeft(seats, lineIdx, seatIdx),
                Up(seats, lineIdx, seatIdx),
                UpRight(seats, lineIdx, seatIdx),
                Right(seats, lineIdx, seatIdx),
                DownRight(seats, lineIdx, seatIdx),
                Down(seats, lineIdx, seatIdx),
                DownLeft(seats, lineIdx, seatIdx),
                Left(seats, lineIdx, seatIdx),
            };

            return (!inSight.Any(seat => seat == '#'));
        }

        static bool ShouldVacate(string[] seats, int lineIdx, int seatIdx)
        {
            var inSight = new char[]
            {
                UpLeft(seats, lineIdx, seatIdx),
                Up(seats, lineIdx, seatIdx),
                UpRight(seats, lineIdx, seatIdx),
                Right(seats, lineIdx, seatIdx),
                DownRight(seats, lineIdx, seatIdx),
                Down(seats, lineIdx, seatIdx),
                DownLeft(seats, lineIdx, seatIdx),
                Left(seats, lineIdx, seatIdx),
            };

            return inSight.Count(seat => seat == '#') > 4;
        }

        static char UpLeft(string[] seats, int lineIdx, int seatIdx) 
        {
            var l = lineIdx;
            var s = seatIdx;

            while (l > 0 && s > 0)
            {
                l--;
                s--;
                if (seats[l][s] != '.') return seats[l][s];
            }

            return '-';
        }
        static char Up(string[] seats, int lineIdx, int seatIdx) 
        {
            var l = lineIdx;

            while (l > 0)
            {
                l--;
                if (seats[l][seatIdx] != '.') return seats[l][seatIdx];
            }

            return '-';
        }
        static char UpRight(string[] seats, int lineIdx, int seatIdx) 
        {
            var l = lineIdx;
            var s = seatIdx;

            while (l > 0 && s < seats[0].Length - 1)
            {
                l--;
                s++;
                if (seats[l][s] != '.') return seats[l][s];
            }

            return '-';
        }
        static char Right(string[] seats, int lineIdx, int seatIdx)
        {
            var s = seatIdx;

            while (s < seats[0].Length - 1)
            {
                s++;
                if (seats[lineIdx][s] != '.') return seats[lineIdx][s];
            }

            return '-';
        }
        static char DownRight(string[] seats, int lineIdx, int seatIdx)
        {
            var l = lineIdx;
            var s = seatIdx;

            while (l < seats.Length - 1 && s < seats[0].Length - 1)
            {
                l++;
                s++;
                if (seats[l][s] != '.') return seats[l][s];
            }

            return '-';
        }
        static char Down(string[] seats, int lineIdx, int seatIdx)
        {
            var l = lineIdx;
            
            while (l < seats.Length - 1)
            {
                l++;
                if (seats[l][seatIdx] != '.') return seats[l][seatIdx];
            }

            return '-';
        }
        static char DownLeft(string[] seats, int lineIdx, int seatIdx)
        {
            var l = lineIdx;
            var s = seatIdx;

            while (l < seats.Length - 1 && s > 0)
            {
                l++;
                s--;
                if (seats[l][s] != '.') return seats[l][s];
            }

            return '-';
        }
        static char Left(string[] seats, int lineIdx, int seatIdx)
        {
            var s = seatIdx;

            while (s > 0)
            {
                s--;
                if (seats[lineIdx][s] != '.') return seats[lineIdx][s];
            }

            return '-';
        }


        static bool AreEqual(string[] item1, string[] item2)
        {
            if (item1.Length != item2.Length) return false;

            for (var i = 0; i < item1.Length; i++)
            {
                if (item1[i] != item2[i]) return false;
            }

            return true;
        }

    }

    class SeatingArea
    {
        private List<Seat> _seats;
        private int _width;
        private int _length;

        public SeatingArea(string[] input)
        {
            _width = input[0].Length;
            _length = input.Length;
            _seats = new List<Seat>();

            for (var lineNumber = 0; lineNumber < _length; lineNumber++)
            {
                for (var charNumber = 0; charNumber < _width; charNumber++)
                {
                    var coord = new Coord(charNumber, lineNumber);
                    var status = input[lineNumber][charNumber] switch
                    {
                        '.' => SeatStatus.Floor,
                        'L' => SeatStatus.Empty,
                        '#' => SeatStatus.Full
                    };

                    var seat = new Seat(coord, status);
                    _seats.Add(seat);
                }
            }
        }

        public int OccupiedSeats()
        {
            var next = MoveNext();

            while (!AreEqual(_seats, next))
            {
                _seats = next;
                next = MoveNext();
            }

            return next.Count(s => s.Status == SeatStatus.Full);
        }

        private bool AreEqual(List<Seat> item1, List<Seat> item2)
        {
            return item1.Zip(item2).All(pair => pair.First.Status == pair.Second.Status);
        }

        private List<Seat> MoveNext()
        {
            var next = new List<Seat>();

            foreach (var seat in _seats)
            {
                if (seat.Status == SeatStatus.Floor)
                {
                    next.Add(seat);
                }
                else
                {
                    var adjacentSeats = Adjacent(seat);
                    var occupiedAdjacentSeats = adjacentSeats.Count(s => s.Status == SeatStatus.Full);
                    if (seat.Status == SeatStatus.Empty && occupiedAdjacentSeats == 0)
                    {
                        next.Add(new Seat(seat.Coord, SeatStatus.Full));
                    }
                    else if (seat.Status == SeatStatus.Full && occupiedAdjacentSeats >= 4)
                    {
                        next.Add(new Seat(seat.Coord, SeatStatus.Empty));
                    }
                    else
                    {
                        next.Add(seat);
                    }
                }
            }

            Console.WriteLine(".");
            //Console.WriteLine($"Moving next will go from {_seats.Count(s => s.Status == SeatStatus.Full)} full seats to {next.Count(s => s.Status == SeatStatus.Full)} full seats");
            //PrintSeats();

            return next;
        }

        private void PrintSeats()
        {
            for (var i = 0; i < _width; i++)
            {
                Console.WriteLine();

                for (var j = 0; j < _length; j++)
                {
                    var s = GetSeat(new Coord(j, i));
                    var p = s.Status switch
                    {
                        SeatStatus.Empty => 'L',
                        SeatStatus.Floor => '.',
                        SeatStatus.Full => '#'
                    };

                    Console.Write(p);
                }

            }
        }

        private Seat GetSeat(Coord coord)
        {
            return _seats.First(s => s.Coord == coord);
        }

        private Seat[] Adjacent(Seat seat)
        {
            var coords = seat.Coord.Adjacent.Where(c => c.X >= 0 && c.X < _width && c.Y >= 0 && c.Y < _length);
            return coords.Select(GetSeat).ToArray();
        }

        record Coord(int X, int Y)
        {
            public Coord[] Adjacent =>
                new Coord[]
                {
                    new Coord(X - 1, Y - 1),
                    new Coord(X, Y - 1),
                    new Coord(X + 1, Y - 1),
                    new Coord(X + 1, Y),
                    new Coord(X + 1, Y + 1),
                    new Coord(X, Y + 1),
                    new Coord(X - 1, Y + 1),
                    new Coord(X - 1, Y),
                };
        }

        enum SeatStatus 
        {
            Floor,
            Empty,
            Full
        }

        record Seat(Coord Coord, SeatStatus Status);
    }
}
