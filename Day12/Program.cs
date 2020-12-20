using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace Day12
{
    class Program
    {
        private static string[] exampleInput = new string[]
        {
            "F10",
            "N3",
            "F7",
            "R90",
            "F11"
        };

        static void Main(string[] args)
        {
            Console.WriteLine($"the example answer to part one is {Part01.Manhatten(exampleInput)}");
            Console.WriteLine($"the answer to part one is {Part01.Manhatten(File.ReadAllLines("data.txt"))}");
            Console.WriteLine($"the example answer to part two is {Part02.Manhatten(exampleInput)}");
            Console.WriteLine($"the answer to part two is {Part02.Manhatten(File.ReadAllLines("data.txt"))}");
        }

        public class Part01
        {
            public static int Manhatten(string[] input)
            {
                var direction = Direction.East;
                var northDistance = 0;
                var eastDistance = 0;

                foreach (var instruction in input)
                {
                    var action = instruction[0];
                    var value = int.Parse(instruction.Substring(1));

                    if (action == 'N')
                    {
                        northDistance += value;
                    }
                    else if (action == 'S')
                    {
                        northDistance -= value;
                    }
                    else if (action == 'E')
                    {
                        eastDistance += value;
                    }
                    else if (action == 'W')
                    {
                        eastDistance -= value;
                    }
                    else if (action == 'L')
                    {
                        direction = Rotate(direction, -value);
                    }
                    else if (action == 'R')
                    {
                        direction = Rotate(direction, value);
                    }
                    else if (action == 'F')
                    {
                        var (n, e) = Move(direction, value);
                        northDistance += n;
                        eastDistance += e;
                    }
                }

                return (Math.Abs(northDistance) + Math.Abs(eastDistance));
            }

            private static Direction Rotate(Direction originalDirection, int value)
            {
                var degrees = originalDirection switch
                {
                    Direction.North => 0,
                    Direction.East => 90,
                    Direction.South => 180,
                    Direction.West => 270
                } + value;

                return (degrees % 360) switch
                {
                    -270 => Direction.East,
                    -180 => Direction.South,
                    -90 => Direction.West,
                    0 => Direction.North,
                    90 => Direction.East,
                    180 => Direction.South,
                    270 => Direction.West
                };
            }

            private static (int, int) Move(Direction direction, int value)
            {
                return direction switch
                {
                    Direction.North => (value, 0),
                    Direction.East => (0, value),
                    Direction.South => (-value, 0),
                    Direction.West => (0, -value)
                };
            }

            private enum Direction
            {
                North,
                East,
                South,
                West
            }
        }

        public class Part02
        {
            public static int Manhatten(string[] input)
            {
                var (waypointN, waypointE) = (1, 10);
                var (shipN, shipE) = (0, 0);

                foreach (var instruction in input)
                {
                    var action = instruction[0];
                    var value = int.Parse(instruction.Substring(1));

                    if (action == 'N')
                    {
                        waypointN += value;
                    }
                    else if (action == 'S')
                    {
                        waypointN -= value;
                    }
                    else if (action == 'E')
                    {
                        waypointE += value;
                    }
                    else if (action == 'W')
                    {
                        waypointE -= value;
                    }
                    else if (action == 'R')
                    {
                        (waypointN, waypointE) = RotateRight(waypointN, waypointE, value);
                    }
                    else if (action == 'L')
                    {
                        (waypointN, waypointE) = RotateLeft(waypointN, waypointE, value);
                    }
                    else if (action == 'F')
                    {
                        var n = shipN + (waypointN * value);
                        var e = shipE + (waypointE * value);
                        (shipN, shipE) = (n, e);
                    }
                }

                return Math.Abs(shipN) + Math.Abs(shipE);
            }

            private static (int, int) RotateRight(int n, int e, int degrees)
            {
                if (degrees == 0)
                {
                    return (n, e);
                }
                if (degrees == 90)
                {
                    return (-e, n);
                }
                if (degrees == 180)
                {
                    return (-n, -e);
                }
                if (degrees == 270)
                {
                    return (e, -n);
                }

                throw new Exception("Unexpected value {degrees}");
            }

            private static (int, int) RotateLeft(int n, int e, int degrees)
            {
                if (degrees == 0)
                {
                    return (n, e);
                }
                if (degrees == 90)
                {
                    return (e, -n);
                }
                if (degrees == 180)
                {
                    return (-n, -e);
                }
                if (degrees == 270)
                {
                    return (-e, n);
                }

                throw new Exception("Unexpected value {degrees}");
            }
        }
    }
}
