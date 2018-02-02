

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

}
else {
    alert("你的浏览器不支持及时自动刷新!");
}

