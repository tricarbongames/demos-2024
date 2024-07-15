using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace DeckManager.Domain;

public class Card
{
    [Required]
    public Suit? Suit { get; set; }
    [Required]
    public Rank? Rank { get; set; }
    public string? DisplayName { get; }
    public int? SortOrderAfterRebuild { get; }
    public Guid? ShuffleOrder { get; private set; }

    public Card()
    {

    }

    public Card(Suit suit, Rank rank, int sortOrderAfterRebuild)
    {
        Suit = suit;
        Rank = rank;
        SortOrderAfterRebuild = sortOrderAfterRebuild;
        DisplayName = DisplayNameHelper.GetDisplayName(suit, rank);
    }

    public void SetShuffleOrder(Guid shuffleOrder)
    {
        ShuffleOrder = shuffleOrder;
    }
}