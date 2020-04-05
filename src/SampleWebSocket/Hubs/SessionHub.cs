using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SampleWebSocket.Context;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SampleWebSocket.Hubs
{
    public class SessionHub : Hub
    {
        private readonly IConfiguration _configuration;
        private readonly IHubContext<SessionHub> _hub;
        private readonly RealTimeContext _context;

        public SessionHub(IConfiguration configuration, IHubContext<SessionHub> hub, RealTimeContext context)
        {
            _configuration = configuration;
            _hub = hub;
            _context = context;
        }

        public void Clear()
        {
            Clients.All.SendAsync("clear").GetAwaiter().GetResult();
        }

        public async Task<string> KickOffAsync()
        {
            return await Task.Run(async () =>
            {
                var sessions = await _context.Sessions.ToListAsync();

                foreach (var session in sessions)
                    session.Status = "Logout";

                await _context.SaveChangesAsync();

                await Clients.All.SendAsync("logon", new { }, sessions);

                return "session hub worked!";
            });
        }

        public async Task SubscribeAsync()
        {
            await Task.Run(() =>
            {
                using var connection = new SqlConnection(_configuration["ConnectionString"]);
                connection.Open();

                var command = new SqlCommand("select [Id], [Code], [User], [Name], [Role], [LogonTime], [Status] from [dbo].[Session]", connection);
                var dependency = new SqlDependency(command);

                dependency.OnChange -= OnDbChanges;
                dependency.OnChange += OnDbChanges;

                command.ExecuteReader();
            });
        }

        private async void OnDbChanges(object sender, SqlNotificationEventArgs e)
        {
            var db = new RealTimeContext(_configuration["ConnectionString"]);
            var source = sender as SqlDependency;

            var sessions = await db.Sessions.ToListAsync();

            if (source.HasChanges)
                await _hub.Clients.All.SendAsync("logon", new[] { sender, e.Info.ToString(), e.Source.ToString(), e.Type.ToString() }, sessions);

            source.OnChange -= OnDbChanges;

            await SubscribeAsync();
        }
    }
}
