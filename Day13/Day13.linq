<Query Kind="Program">
  <Namespace>System.Numerics</Namespace>
</Query>

void Main()
{
	Console.WriteLine(DoExample01());
    Console.WriteLine(DoPuzzle01());
    DoExample02();
    Console.WriteLine(DoPuzzle02());
}

int DoPuzzle01()
{
    var input = File.ReadLines(@"C:\Code\repos\advent-of-code2020\Day13\data.txt").ToList();
    return Puzzle01(input[0], input[1]);
}

BigInteger DoPuzzle02()
{
    var input = File.ReadLines(@"C:\Code\repos\advent-of-code2020\Day13\data.txt").ToList();
    return Puzzle02(input[1]);
}

int DoExample01()
{
	var expectedArrival = "939";
    var busTimes = "7,13,x,x,59,x,31,19";
	return Puzzle01(expectedArrival, busTimes);
}

void DoExample02()
{
    Console.WriteLine($"{Puzzle02("7,13,x,x,59,x,31,19")} should be 1068781"); // should be 1068781
    Console.WriteLine($"{Puzzle02("17,x,13,19")} should be 3417"); // should be 3417
    Console.WriteLine($"{Puzzle02("67,7,59,61")} should be 754018"); // should be 754018
    Console.WriteLine($"{Puzzle02("67,x,7,59,61")} should be 779210"); // should be 779210
    Console.WriteLine($"{Puzzle02("67,7,x,59,61")} should be 1261476"); // should be 1261476
    Console.WriteLine($"{Puzzle02("1789,37,47,1889")} should be 1202161486"); // should be 1202161486
}

int Puzzle01(string expectedArrivalInput, string busTimesInput)
{
	var expectedArrival = int.Parse(expectedArrivalInput);
    var busTimes = busTimesInput.Split(",").Where(x => x != "x").Select(int.Parse).ToList();
    
    var busArrived = false;
    var time = expectedArrival - 1;
    var busNumber = -1;
    
    while (!busArrived)
    {
        time++;
        foreach (var bus in busTimes)
        {
            if (time % bus == 0)
            {
                busArrived = true;
                busNumber = bus;
                break;
            }
        }
    }
    
    return (time - expectedArrival) * busNumber;
}

record Bus(long Id, int Offset);

long Puzzle02(string busTimesInput)
{
    // stolen from https://github.com/AmanAgnihotri/Advent-of-Code/blob/master/src/ShuttleSearch/Program.cs
    var buses = busTimesInput.Split(',')
      .Select((value, offset) => (value, offset))
      .Where(pair => pair.value != "x")
      .Select(pair => new Bus(long.Parse(pair.value), pair.offset)).ToList();

    var (baseBusId, _) = buses[0];
    var (time, period) = (baseBusId, baseBusId);

    foreach (var (schedule, offset) in buses.Skip(1))
    {
        while ((time + offset) % schedule != 0) time += period;

        period *= schedule;
    }

    Console.WriteLine(time);
    return time;
}

