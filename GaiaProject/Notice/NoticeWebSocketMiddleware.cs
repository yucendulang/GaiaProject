using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GaiaCore.Gaia;
using GaiaDbContext.Models;
using Microsoft.AspNetCore.Http;

namespace GaiaProject.Notice
{
    /// <summary>
    /// socket信息
    /// </summary>
//    public class SocketInfo
//    {
//        /// <summary>
//        /// 游戏名称
//        /// </summary>
//        public string GameName { get; set; }
//        /// <summary>
//        /// 用户名称
//        /// </summary>
//        public string username { get; set; }
//        /// <summary>
//        /// socket
//        /// </summary>
//        public  WebSocket { get; set; }
//    }

    public class NoticeWebSocketMiddleware
    {
        //private static ConcurrentDictionary<string, WebSocket> _sockets = new ConcurrentDictionary<string, WebSocket>();

        private static ConcurrentDictionary<string, ConcurrentDictionary<string,WebSocket>> gameList;

        static NoticeWebSocketMiddleware()
        {
            gameList = new ConcurrentDictionary<string, ConcurrentDictionary<string, WebSocket>>();
        }

        private readonly RequestDelegate _next;

        public NoticeWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await _next.Invoke(context);
                return;
            }
            CancellationToken ct = context.RequestAborted;
            WebSocket currentSocket = await context.WebSockets.AcceptWebSocketAsync();

            string gameName = context.Request.Query["GameName"].ToString() ?? "GameName";
            ConcurrentDictionary<string, WebSocket> socketList;
            //不包括游戏就创建
            if (!gameList.ContainsKey(gameName))
            {
                socketList = new ConcurrentDictionary<string, WebSocket>();
                gameList.TryAdd(gameName, socketList);
            }
            else
            {
                socketList = gameList[gameName];
            }
            //包括游戏的话，删除
            if (socketList.ContainsKey(context.User.Identity.Name))
            {
                WebSocket outSocket;
                socketList.TryRemove(context.User.Identity.Name, out outSocket);
            }
            //将socket添加到里面，则添加
            socketList.TryAdd(context.User.Identity.Name, currentSocket);

            while (true)
            {
                if (ct.IsCancellationRequested)
                {
                    break;
                }

                if (currentSocket.State != WebSocketState.Open)
                {
                    break;
                }

                continue;

                //                foreach (var socket in socketList)
                //                {
                //                    if (socket.Value.State != WebSocketState.Open)
                //                    {
                //                        continue;
                //                    }
                //
                //                    await SendStringAsync(socket.Value, response, ct);
                //                }
            }
            //从socket删除
            WebSocket dummy;
            socketList.TryRemove(context.User.Identity.Name, out dummy);
            //如果socket删除完毕，删除游戏进程
            if (socketList.Count == 0)
            {
                ConcurrentDictionary<string, WebSocket> ls;
                gameList.TryRemove(gameName,out ls);
            }

            await currentSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
            currentSocket.Dispose();
        }


        private static Task SendStringAsync(WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);
            if (socket.State == WebSocketState.Open)
            {
                return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
            }
            return null;
        }


        /// <summary>
        /// 游戏行动提醒
        /// </summary>
        /// <param name="gaiaGame"></param>
        /// <param name="user"></param>
        public static async void GameActive(GaiaGame gaiaGame, ClaimsPrincipal user)
        {

            try
            {
                string gameName = gaiaGame.GameName;
                //gameName = "test";
                //包含游戏
                if (gameList.ContainsKey(gameName))
                {
                    ConcurrentDictionary<string, WebSocket> socketInfo = gameList[gameName];
                    //遍历用户
                    foreach (string socketInfoKey in socketInfo.Keys)
                    {
                        //不是当前用户，发送提醒
                        if (socketInfoKey != user.Identity.Name)
                        {
                            WebSocket socket = socketInfo[socketInfoKey];
                            if (socket.State != WebSocketState.Open)
                            {
                                continue;
                            }
                            //socket.CloseAsync(WebSocketCloseStatus.Empty, "", cancellationToken: new CancellationToken());
                            //socket = new WebSocket();
                            await SendStringAsync(socket, "200");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }




    }
}
