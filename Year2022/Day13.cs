using System.Text;
using NUnit.Framework;

namespace Year2022;

public class Day13 : BaseDayTest
{
    [Test]
    public override void Part1()
    {
        var result = 0;
        var round = 1;
        for (var index = 0; index < lines.Count; index += 3)
        {
            if (Compare(lines[index][1..^1], lines[index + 1][1..^1]) < 0)
            {
                result += round;
                Console.WriteLine(round);
            }

            round++;
        }

        Console.WriteLine(result);

        Assert.Pass();
    }

    private static int Compare(string strLeft, string strRight)
    {
        var left = NumberOrArray.Parse(strLeft);
        var right = NumberOrArray.Parse(strRight);

        return left.CompareTo(right);
    }

    [Test]
    public override void Part2()
    {
        var list = (from line in lines where !string.IsNullOrWhiteSpace(line) select NumberOrArray.Parse(line[1..^1])).ToList();

        var number2 = NumberOrArray.Parse("[2]");
        var number6 = NumberOrArray.Parse("[6]");

        list.Add(number2);
        list.Add(number6);

        list.Sort();

        var result = (list.FindIndex(x => x == number2) + 1) * (list.FindIndex(x => x == number6) + 1);

        Console.WriteLine(result);

        Assert.Pass();
    }

    public class NumberOrArray : IComparable<NumberOrArray>
    {
        private TypeNuberOrArray _type;
        private IList<NumberOrArray> _arrays = new List<NumberOrArray>();
        private int _number;
        private NumberOrArray? _parrent;


        public static NumberOrArray Parse(string line)
        {
            var current = new NumberOrArray { _type = TypeNuberOrArray.Array };
            var sb = new StringBuilder();

            foreach (var chr in line)
            {
                switch (chr)
                {
                    case '[':
                        current!.AddNumberIfHasValue(sb);
                        current = current.AddChild(new NumberOrArray() { _type = TypeNuberOrArray.Array });
                        break;
                    case ']':
                        current!.AddNumberIfHasValue(sb);
                        current = current._parrent;
                        break;
                    case ',':
                        current!.AddNumberIfHasValue(sb);
                        break;
                    default:
                        sb.Append(chr);
                        break;
                }
            }

            current!.AddNumberIfHasValue(sb);
            return current;
        }

        private void AddNumberIfHasValue(StringBuilder sb)
        {
            if (sb.Length == 0) return;
            AddChild(new NumberOrArray()
            {
                _type = TypeNuberOrArray.Number,
                _number = int.Parse(sb.ToString())
            });
            sb.Clear();
        }

        private NumberOrArray AddChild(NumberOrArray child)
        {
            child._parrent = this;
            _arrays.Add(child);
            return child;
        }

        private enum TypeNuberOrArray
        {
            Number,
            Array
        }

        public int CompareTo(NumberOrArray? other)
        {
            switch (_type)
            {
                case TypeNuberOrArray.Number when other!._type is TypeNuberOrArray.Number:
                    return _number.CompareTo(other._number);
                case TypeNuberOrArray.Array when other!._type is TypeNuberOrArray.Array:
                {
                    for (var i = 0; i < Math.Min(_arrays.Count, other._arrays.Count); i++)
                    {
                        var compare = _arrays[i].CompareTo(other._arrays[i]);
                        if (compare != 0)
                        {
                            return compare;
                        }
                    }

                    return _arrays.Count.CompareTo(other._arrays.Count);
                }
            }

            if (other!._type is TypeNuberOrArray.Number)
            {
                other = new NumberOrArray()
                {
                    _type = TypeNuberOrArray.Array,
                    _arrays = new List<NumberOrArray>() { other }
                };
                return CompareTo(other);
            }

            var arrayThis = new NumberOrArray()
            {
                _type = TypeNuberOrArray.Array,
                _arrays = new List<NumberOrArray>() { this }
            };

            return arrayThis.CompareTo(other);
        }

        public override string ToString()
        {
            return _type is TypeNuberOrArray.Number
                ? $"{{Number : {_number}}}"
                : $"{{{string.Join(", ", _arrays.Select(x => x.ToString()))}}}";
        }
    }
}