using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GameHub : Hub
{
    private static Dictionary<string, DateTime> playerExitTimes = new();
    private static Dictionary<string, DateTime> lastProductionTimes = new();

    private IHubContext<GameHub> _hubContext;
    
    public GameHub(IHubContext<GameHub> HubContext)
    {
        _hubContext = HubContext;
    }

    public override async Task OnConnectedAsync()
    {
        string connectionId = Context.ConnectionId;
        Console.WriteLine($"Client connected: {connectionId}");
        await Clients.Caller.SendAsync("OnConnected", connectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        string connectionId = Context.ConnectionId;
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SaveExitTime(DateTime exitTime)
    {
        string connectionId = Context.ConnectionId;
        playerExitTimes[connectionId] = exitTime;
        Console.WriteLine($"Exit time saved for {connectionId}: {exitTime}");
    }

    public async Task<DateTime> GetLastProductionTime()
    {
        string connectionId = Context.ConnectionId;
        
        if (!lastProductionTimes.ContainsKey(connectionId))
        {
            lastProductionTimes[connectionId] = DateTime.UtcNow; // Eğer hiç yoksa şu anki zamanı ata
        }

        return lastProductionTimes[connectionId];
    }

    public async Task UpdateProductionTime(DateTime productionTime)
    {
        string connectionId = Context.ConnectionId;
        lastProductionTimes[connectionId] = productionTime;
    }
}
