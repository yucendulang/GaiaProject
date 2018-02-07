

if (!!window.WebSocket && window.WebSocket.prototype.send) {

    var gp_socket;
    var gp_port = document.location.port ? (":" + document.location.port) : "";
    var connectionUrl = "ws://" + document.location.hostname + gp_port + "/ws?GameName=" + userInfo.GameName;


    function connectSocket() {
        gp_socket = new WebSocket(connectionUrl);
        gp_socket.onopen = function (event) {

        };
        gp_socket.onclose = function (event) {

        };
        gp_socket.onmessage = function (event) {
            //alert(event.data);
            if (event.data !== undefined && parseInt(event.data) === 200) {
                window.location.reload();
            }
        };
    }
    connectSocket();
//    for (var i = 0; i < 10000; i++) {
//        connectSocket();
//        if (!gp_socket || gp_socket.readyState !== WebSocket.OPEN) {
//            //alert("socket not connected");
//        }
//        //gp_socket.close(1000, "Closing from client");
//    }
}
else {
    alert("你的浏览器不支持及时自动刷新!");
}

