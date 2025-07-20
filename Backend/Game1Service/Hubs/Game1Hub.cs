using Grpc.Core;

namespace GameService.Services;
public class CoinFlipGameService : CoinFlipManager.CoinFlipManagerBase
{
    public override async Task<CoinFlipReply> StartGame(CoinFlipRequest request, ServerCallContext context)
    {
        Console.WriteLine($"[CoinFlip] Starting game in room: {request.RoomId}");
        Console.WriteLine($"[CoinFlip] Players: {request.User1Id} vs {request.User2Id}");
        
        await Task.Delay(3000); 

        var random = new Random();
        var winnerId = random.Next(0, 2) == 0 ? request.User1Id : request.User2Id;

        Console.WriteLine($"[CoinFlip] Winner is: {winnerId}");

        return new CoinFlipReply
        {
            WinnerId = winnerId
        };
    }

        public async Task StartGameSession(Guid tableId)
    {
        var table = GetTableInformation(tableId);

        Console.WriteLine("Game session is being prepared");
        Console.WriteLine($"Table has {table?.Players.Count} players with entry fee of {table?.EntryFee}");


        if (table?.Players is { Count: >= 2 })
        {
            UserConnections.TryGetValue(table.Players[0], out var playerOneConnectionId);
            UserConnections.TryGetValue(table.Players[1], out var playerTwoConnectionId);

            Console.WriteLine("Game is being started");
            Console.WriteLine($"Before game is started: Player 1: {table.Players[0]}, Player 2: {table.Players[1]}");
            var gameResult = await gameTableManager.StartGameSession(tableId, table.Players[0], table.Players[1]);
            Console.WriteLine("Game has ended");

            Console.WriteLine($"Game winner is {gameResult.WinnerUserId}, total win is: {gameResult.WinnerAmount}");
            if (!string.IsNullOrEmpty(playerOneConnectionId) && !string.IsNullOrEmpty(playerTwoConnectionId))
            {
                await Clients.Clients(playerOneConnectionId, playerTwoConnectionId)
                    .GameConcluded(gameResult);
            }
        }
    }

    public async Task<List<Transaction>> GetUserTransactions(string jwtToken)
        => await gameTableManager.GetUserTransactions(jwtToken);

    public async Task FillUpBalance(decimal amount, string jwtToken)
    {
        var transaction = await gameTableManager.FillUpBalance(amount, jwtToken);

        var userWallet = await gameTableManager.GetUserBalance(jwtToken);

        await Clients.User(transaction.UserId).BalanceFilled(transaction, userWallet.Balance);
    }

    public async Task RollbackTransaction(string transactionId, string jwtToken)
    {
        var transaction = await gameTableManager.RollbackTransaction(transactionId, jwtToken);

        await Clients.User(transaction.UserId).TransactionAdded(transaction);
    }

    public async Task CancelGame(Guid tableId)
    {
        var refundResult = gameTableManager.CancelTable(tableId);
        await Clients.All.GameCanceled(tableId);
    }

}
