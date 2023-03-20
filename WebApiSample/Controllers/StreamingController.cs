using Microsoft.AspNetCore.Mvc;
using StreamJsonRpc;

namespace WebApiSample.Controllers
{
    public class StreamingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// https://stackoverflow.com/questions/50938522/what-iactionresult-to-return-from-a-websocket-request-in-asp-net-core
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> PostIncomingWebSocketStream()
        {
            if (!this.HttpContext.WebSockets.IsWebSocketRequest)
            {
                return this.BadRequest();
            }

            //using (var webSocket = await this.HttpContext.WebSockets.AcceptWebSocketAsync())
            //{
            //    using (var jsonRpc = new JsonRpc(new WebSocketMessageHandler(webSocket)))
            //    {
            //        jsonRpc.AddLocalRpcTarget(new SocketServer());
            //        jsonRpc.StartListening();
            //        await jsonRpc.Completion;
            //    }
            //}

            return new EmptyResult();
        }
    }
}
