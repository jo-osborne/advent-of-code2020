using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Day08
{
    public class Tests
    {
        [Test]
        public void ExampleParsing()
        {
            var instructions = new List<string>
            {
                "nop +0",
                "acc +1",
                "jmp +4",
                "acc +3",
                "jmp -3",
                "acc -99",
                "acc +1",
                "jmp -4",
                "acc +6"
            };

            var expectedInstructions = new Instruction[]
                {
                    new Nop(0),
                    new Acc(1),
                    new Jmp(4),
                    new Acc(3),
                    new Jmp(-3),
                    new Acc(-99),
                    new Acc(1),
                    new Jmp(-4),
                    new Acc(6)
                };

            var parsed = InstructionParser.Parse(instructions);

            Assert.That(parsed.Length, Is.EqualTo(expectedInstructions.Length));

            for (var i = 0; i < expectedInstructions.Length; i++)
            {
                Assert.That(parsed[i], Is.EqualTo(expectedInstructions[i]));
            }
        }

        [Test]
        public void ExampleExecution()
        {
            var computer = new Computer
            (
                new Instruction[]
                {
                    new Nop(0),
                    new Acc(1),
                    new Jmp(4),
                    new Acc(3),
                    new Jmp(-3),
                    new Acc(-99),
                    new Acc(1),
                    new Jmp(-4),
                    new Acc(6)
                }
            );

            computer.ExecuteUntilLoop();

            var pointers = computer.InstructionPointer;

            Assert.That(computer.Accumulator.Value, Is.EqualTo(5));
        }

        [Test]
        public void Puzzle01()
        {
            var input = File.ReadAllLines("data.txt").ToList();
            var instructions = InstructionParser.Parse(input);
            var computer = new Computer(instructions);
            computer.ExecuteUntilLoop();
            Assert.That(computer.Accumulator.Value, Is.EqualTo(2051));
        }

        [Test]
        public void Puzzle02()
        {
            var input = File.ReadAllLines("data.txt").ToList();
            var instructions = InstructionParser.Parse(input);

            //var instructions = new Instruction[]
            //    {
            //        new Nop(0),
            //        new Acc(1),
            //        new Jmp(4),
            //        new Acc(3),
            //        new Jmp(-3),
            //        new Acc(-99),
            //        new Acc(1),
            //        new Jmp(-4),
            //        new Acc(6)
            //    };

            var result = -1;
            for (var i = 0; i < instructions.Length; i++)
            {
                var originalIns = instructions[i];
                instructions[i] = originalIns switch
                {
                    Nop { Value: var v } => new Jmp(v),
                    Jmp { Value: var v } => new Nop(v),
                    _ => originalIns
                };
                var computer = new Computer(instructions);
                computer.ExecuteUntilLoop();
                if (!computer.TerminatedDueToInfiniteLoop)
                {
                    result = computer.Accumulator.Value;
                    break;
                }
                instructions[i] = originalIns;
            }

            Assert.That(result, Is.EqualTo(2304));
        }
    }

    public class Computer
    {
        public Computer(Instruction[] instructions)
        {
            Instructions = instructions; 
            foreach (var ins in Instructions)
            {
                ins.HasBeenExecuted = false;
            }
            Accumulator = new Accumulator();
        }

        public Accumulator Accumulator { get; }

        public int InstructionPointer { get; private set;  }

        public Instruction[] Instructions { get; }

        public bool TerminatedDueToInfiniteLoop { get; private set; }

        public void ExecuteUntilLoop()
        {
            while (!TerminatedDueToInfiniteLoop && InstructionPointer < Instructions.Length)
            {
                var ins = Instructions[InstructionPointer];

                if (!ins.HasBeenExecuted)
                {
                    ins.Execute(Accumulator);
                    var nextInsIdx = ins.NextInstructionIdx(InstructionPointer);
                    InstructionPointer = nextInsIdx;
                }
                else
                    TerminatedDueToInfiniteLoop = true;
            }
        }
    }

    public class Accumulator
    {
        public int Value { get; set; }
    }

    public interface Instruction
    {
        public bool HasBeenExecuted { get; set; }
        public void Execute(Accumulator acc);
        public int NextInstructionIdx(int currentInstructionIdx);
    }

    public record Nop : Instruction
    {
        public int Value { get; set; }

        public Nop(int value)
        {
            Value = value;
        }

        public bool HasBeenExecuted { get; set; }

        public void Execute(Accumulator acc)
        {
            HasBeenExecuted = true;
        }

        public int NextInstructionIdx(int currentInstructionIdx)
        {
            return currentInstructionIdx + 1;
        }
    }

    public record Jmp : Instruction
    {
        public int Value { get; set; }

        public Jmp(int value)
        {
            Value = value;
        }

        public bool HasBeenExecuted { get; set; }

        public void Execute(Accumulator acc)
        {
            HasBeenExecuted = true;
        }

        public int NextInstructionIdx(int currentInstructionIdx)
        {
            return currentInstructionIdx + Value;
        }
    }

    public record Acc : Instruction
    {
        private readonly int _value;

        public Acc(int value)
        {
            _value = value;
        }

        public bool HasBeenExecuted { get; set; }

        public void Execute(Accumulator acc)
        {
            acc.Value = acc.Value + _value;
            HasBeenExecuted = true;
        }

        public int NextInstructionIdx(int currentInstructionIdx)
        {
            return currentInstructionIdx + 1;
        }
    }

    public static class InstructionParser
    {
        public static Instruction[] Parse(List<string> instructions)
        {
            var result = new List<Instruction>();

            foreach (var instruction in instructions)
            {
                var split = instruction.Split(' ');
                var insType = split[0];
                var value = int.Parse(split[1]);
                
                result.Add(insType switch
                {
                    "nop" => new Nop(value),
                    "jmp" => new Jmp(value),
                    "acc" => new Acc(value)
                });
            }
            return result.ToArray();
        }
    }
}