namespace DeckManager.Domain;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Suit
{
    Spades,
    Hearts,
    Clubs,
    Diamonds
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Rank
{
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine,
    Ten,
    Jack,
    Queen,
    King,
    Ace
}