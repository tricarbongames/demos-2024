namespace DeckManager.Domain;

public interface IDealer
{
    public Card DealCard();
    public void Shuffle();
    public void TryDiscard(Card card);
    public void TryCut(int splitIndex);
    public void Order();
    public void RebuildDeck();
    public Card Cheat();
}

public class Dealer : IDealer
{
    private readonly ILogger<Dealer> _logger;
    public List<Card> CardsInDeck { get; private set; } = [];
    public List<Card> DealtCards { get; private set; } = [];
    public List<Card> DiscardedCards { get; private set; } = [];

    public Dealer(ILogger<Dealer> logger)
    {
        _logger = logger;
        RebuildDeck();
    }

    public Card DealCard()
    {
        Card dealtCard;
        // lock allows for thread safety.
        lock (CardsInDeck)
        {
            if (CardsInDeck.Count is 0)
            {
                _logger.Log(LogLevel.Warning, $"Cannot deal card, deck is empty.");

                return null;
            }

            dealtCard = CardsInDeck.First();
            CardsInDeck.Remove(dealtCard);
        }

        lock (DealtCards)
        {
            DealtCards.Add(dealtCard);
        }

        return dealtCard;
    }

    // Shuffle assigns a random guid to each card in the deck, then sorts by the guids.
    public void Shuffle()
    {
        lock (CardsInDeck)
        {
            foreach (var card in CardsInDeck)
            {
                card.ShuffleOrder = Guid.NewGuid();
            }

            CardsInDeck = [.. CardsInDeck.OrderBy(c => c.ShuffleOrder)];
        }
    }

    public void TryDiscard(Card card)
    {
        // We can treat Suit + Rank as a composite key, meaning only one card could be removed at a time.
        // Not locking because remove is idempotent.
        var discardCount = DealtCards.RemoveAll(c => c.Suit == card.Suit && c.Rank == card.Rank);

        if (discardCount == 0)
        {
            _logger.Log(LogLevel.Warning, $"Attempted to discard {card.DisplayName} that was not dealt");
            return;
        }

        lock (DiscardedCards)
        {
            DiscardedCards.Add(card);
        }
    }

    public void TryCut(int splitIndex)
    {
        if (splitIndex < 0 || splitIndex >= CardsInDeck.Count - 1)
        {
            _logger.Log(LogLevel.Warning, $"Split index of {splitIndex} is invalid for a deck size of {CardsInDeck.Count}");
            return;
        }

        lock (CardsInDeck)
        {
            var topStack = CardsInDeck[..(splitIndex + 1)];
            var bottomStack = CardsInDeck.Slice(splitIndex + 1, CardsInDeck.Count - splitIndex - 1);
            CardsInDeck = bottomStack;
            CardsInDeck.AddRange(topStack);
        }
    }

    public void Order()
    {
        lock (CardsInDeck)
        {
            CardsInDeck = [.. CardsInDeck.OrderBy(c => c.SortOrderAfterRebuild)];
        }
    }

    public void RebuildDeck()
    {
        CardsInDeck = [];
        DealtCards = [];
        DiscardedCards = [];
        var sortOrder = 0;

        lock (CardsInDeck)
        {
            foreach (var suit in Enum.GetValues<Suit>())
            {
                foreach (var rank in Enum.GetValues<Rank>())
                {
                    CardsInDeck.Add(new Card(suit, rank, sortOrder));
                    sortOrder++;
                }
            }
        }
    }

    public Card Cheat()
    {
        if (CardsInDeck.Count is 0)
        {
            _logger.Log(LogLevel.Warning, $"Cannot cheat, deck is empty.");

            return null;
        }

        return CardsInDeck.First();
    }
}