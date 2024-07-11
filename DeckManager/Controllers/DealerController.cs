using Microsoft.AspNetCore.Mvc;
using DeckManager.Domain;

namespace DeckManager.Controllers;

[ApiController]
[Route("[controller]")]
public class DealerController : ControllerBase
{
    private readonly ILogger<DealerController> _logger;
    private readonly IDealer _dealer;

    public DealerController(ILogger<DealerController> logger, IDealer dealer)
    {
        _logger = logger;
        _dealer = dealer;
    }

    [HttpGet("DealCard")]
    public Card DealCard()
    {
        return _dealer.DealCard();
    }

    [HttpGet("Shuffle")]
    public void Shuffle()
    {
        _dealer.Shuffle();
    }

    [HttpPost("Discard")]
    public void TryDiscard([FromBody] Card card)
    {
        _dealer.TryDiscard(card);
    }

    [HttpGet("Cut")]
    public void TryCut([FromQuery] int splitIndex)
    {
        _dealer.TryCut(splitIndex);
    }

    [HttpGet("Order")]
    public void Order()
    {
        _dealer.Order();
    }

    [HttpGet("RebuildDeck")]
    public void RebuildDeck()
    {
        _dealer.RebuildDeck();
    }

    [HttpGet("Cheat")]
    public Card Cheat()
    {
        return _dealer.Cheat();
    }
}