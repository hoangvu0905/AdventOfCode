using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Year2022;

public class DayTest : BaseDayTest
{
    [Test]
    public override void Part1()
    {
       
    }

    [Test]
    public override void Part2()
    {
        var result = Solve(
            string.Join("\n", lines),
            @"
A -> B0 C0 D2 F1
B -> E2 C1 A0 F0
C -> B3 E0 D3 A0
D -> E0 F0 A2 C1
E -> B2 F1 D0 C0
F -> E3 B0 A3 D0
"
        );

        Console.WriteLine(result);
    }

    const int blockSize = 50;
    const int right = 0;
    const int down = 1;
    const int left = 2;
    const int up = 3;

    int Solve(string input, string topology) {
        var (map, cmds) = Parse(input);
        var state = new State("A", new Coord(0, 0), right);

        foreach (var cmd in cmds) {
            switch (cmd) {
                case Left:
                    state = state with { dir = (state.dir + 3) % 4 };
                    break;
                case Right:
                    state = state with { dir = (state.dir + 1) % 4 };
                    break;
                case Forward(var n):
                    for (var i = 0; i < n; i++) {
                        var stateNext = Step(topology, state);
                        var global = ToGlobal(stateNext);
                        if (map[global.irow][global.icol] == '.') {
                            state = stateNext;
                        } else {
                            break;
                        }
                    }
                    break;
            }

            Console.WriteLine($"{state.block}\t{state.coord.irow}\t{state.coord.icol}\t{state.dir}");
        }

        return 1000 * (ToGlobal(state).irow + 1) + 
                  4 * (ToGlobal(state).icol + 1) + 
                      state.dir;
    }

    Coord ToGlobal(State state) => 
        state.block switch {
            "A" => state.coord + new Coord(0, blockSize),
            "B" => state.coord + new Coord(0, 2 * blockSize),
            "C" => state.coord + new Coord(blockSize, blockSize),
            "D" => state.coord + new Coord(2 * blockSize, 0),
            "E" => state.coord + new Coord(2 * blockSize, blockSize),
            "F" => state.coord + new Coord(3 * blockSize, 0),
            _ => throw new Exception()
        };

    State Step(string topology, State state) {

        bool wrapsAround(Coord coord) =>
            coord.icol < 0 || coord.icol >= blockSize || 
            coord.irow < 0 || coord.irow >= blockSize;

        var (srcBlock, coord, dir) = state;
        var dstBlock = srcBlock;

        // take one step, if there is no wrap around we are all right
        coord = dir switch {
            left => coord with { icol = coord.icol - 1 },
            down => coord with { irow = coord.irow + 1 },
            right => coord with { icol = coord.icol + 1 },
            up => coord with { irow = coord.irow - 1 },
            _ => throw new Exception()
        };

        if (wrapsAround(coord)) {
            // check the topology, select the dstBlock and rotate coord and dir 
            // as much as needed this is easier to follow through an example
            // if srcBlock: "C", dir: 2

            var line = topology.Split(new []{'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries).Single(x => x.StartsWith(srcBlock));
            // line: C -> B3 E0 D3 A0

            var mapping = line.Split(" -> ")[1].Split(" ");
            // mapping: B3 E0 D3 A0

            var neighbour = mapping[dir];
            // neighbour: D3

            dstBlock = neighbour.Substring(0, 1);
            // dstBlock: D

            var rotate = int.Parse(neighbour.Substring(1));
            // rotate: 3

            // go back to the 0..49 range first, then rotate as much as needed
            coord = coord with {
                irow = (coord.irow + blockSize) % blockSize,
                icol = (coord.icol + blockSize) % blockSize,
            };

            for (var i = 0; i < rotate; i++) {
                coord = coord with { 
                    irow = coord.icol, 
                    icol = blockSize - coord.irow - 1 
                };
                dir = (dir + 1) % 4;
            }
        }

        return new State(dstBlock, coord, dir);
    }

    (string[] map, Cmd[] path) Parse(string input) {
        var blocks = input.Split("\n\n");

        var map = blocks[0].Split("\n");
        var commands = Regex
            .Matches(blocks[1], @"(\d+)|L|R")
            .Select<Match, Cmd>(m =>
                m.Value switch {
                    "L" => new Left(),
                    "R" => new Right(),
                    string n => new Forward(int.Parse(n)),
                })
            .ToArray();

        return (map, commands);
    }

    record State(string block, Coord coord, int dir);

    record Coord(int irow, int icol) {
        public static Coord operator +(Coord a, Coord b) =>
            new Coord(a.irow + b.irow, a.icol + b.icol);

        public static Coord operator -(Coord a, Coord b) =>
            new Coord(a.irow - b.irow, a.icol - b.icol);
    }

    interface Cmd { }
    record Forward(int n) : Cmd;
    record Right() : Cmd;
    record Left() : Cmd;
}