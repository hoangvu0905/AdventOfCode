using System.Collections;
using NUnit.Framework;

namespace Year2023;

public class Day07 : BaseDayTest
{
    private static readonly char[] LabelCards = { '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A' };
    private static readonly char[] LabelCardsJ = { 'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A' };
    public const int NumberCards = 13;

    [Test]
    public override void Part1()
    {
        var result = 0L;
        
        var games = lines.Select(x => Game.ParseLine(x, LabelCards, false)).ToList();
        var orderGames = games.Order().ToList();
        for (var i = 0; i < orderGames.Count; i++)
        {
            result += orderGames[i].Score * (i + 1);
        }

        Console.WriteLine(result);
    }

    [Test]
    public override void Part2()
    {
        var result = 0L;
        
        var games = lines.Select(x => Game.ParseLine(x, LabelCardsJ, true)).ToList();
        var orderGames = games.Order().ToList();
        for (var i = 0; i < orderGames.Count; i++)
        {
            result += orderGames[i].Score * (i + 1);
        }

        Console.WriteLine(result); // 250825971
    }

    public class Game : IComparable
    {
        public int Score { get; set; }
        public string Hand { get; set; }
        public int PointCard { get; set; }

        public TypeCardInHand TypeCardInHand { get; set; }

        public static Game ParseLine(string line, char[] labelCards, bool hasReplaceJ = false)
        {
            var parts = line.Split(' ');
            var hand = parts[0];
            if (hasReplaceJ)
            {
                var bestHandWithoutJ = parts[0].Where(x => x is not 'J').GroupBy(x => x)
                    .ToDictionary(x => x.Key, x => x.Count())
                    .OrderByDescending(x => x.Value).Select(x => x.Key).FirstOrDefault('J');
                parts[0] = parts[0].Replace('J', bestHandWithoutJ);
            }
            
            var dict = parts[0].GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count())
                .OrderByDescending(x => x.Value).ThenByDescending(x => Array.IndexOf(labelCards, x.Key))
                .ToList();
            var typeCardInHand = TypeCardInHand.None;

            var cardIsMost = dict[0];
            switch (cardIsMost.Value)
            {
                case 5:
                    typeCardInHand = TypeCardInHand.FiveKind;
                    break;
                case 4:
                    typeCardInHand = TypeCardInHand.FourKind;
                    break;
                case 3:
                    var cardIsSecondMost = dict.Skip(1).First();
                    typeCardInHand = cardIsSecondMost.Value == 2 ? TypeCardInHand.FullHouse : TypeCardInHand.ThreeKind;

                    break;
                case 2:
                    cardIsSecondMost = dict.Skip(1).First();
                    if (cardIsSecondMost.Value == 2)
                    {
                        typeCardInHand = TypeCardInHand.TwoPair;
                    }
                    else
                    {
                        typeCardInHand = TypeCardInHand.OnePair;
                    }

                    break;
                case 1:
                    typeCardInHand = TypeCardInHand.HighCard;
                    break;
                default:
                    throw new NotImplementedException();
            }
            var pointCard = GetPointLabelCards(labelCards, hand.ToCharArray());

            return new Game
            {
                Hand = hand,
                Score = int.Parse(parts[1]),
                TypeCardInHand = typeCardInHand,
                PointCard = pointCard,
            };
        }

        public int CompareTo(object? obj)
        {
            if (obj is not Game game)
            {
                throw new NotSupportedException();
            }
            
            var compareType = TypeCardInHand.CompareTo(game.TypeCardInHand);
            return compareType != 0 ? compareType : PointCard.CompareTo(game.PointCard);
        }

        public override string ToString()
        {
            return $"{Hand} {Score} {TypeCardInHand} {PointCard}";
        }

        private static int GetPointLabelCards(char[] labelCardsIndex, params char[] labelCards)
        {
            var result = 0;
            foreach (var labelCard in labelCards)
            {
                result = result * NumberCards + Array.IndexOf(labelCardsIndex, labelCard);
            }

            return result;
        }
    }

    public enum TypeCardInHand
    {
        None,
        HighCard,
        OnePair,
        TwoPair,
        ThreeKind,
        FullHouse,
        FourKind,
        FiveKind,
    }
}