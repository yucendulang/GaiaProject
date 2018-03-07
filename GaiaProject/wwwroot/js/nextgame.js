var timeJiange = 10000;
if (typeof (IsSocket) === 'undefined') {
    IsSocket = false;
}
if (IsSocket) {
    timeJiange = 10000*6;//1分钟
}
setTimeout('GetNextGame()', timeJiange); //指定10秒刷新一次

function GetNextGame() {
    var obj = $("#isMyTurn");
    if (obj.length===0 || obj.val().indexOf("True") === -1) {
        $.post("/home/GetNextGame",
            {
                name: $("#username").val()
            },
            function (data, status) {
                if (data == undefined || data === "") {
                    //console.log(data);
                } else {
                    //console.log(data);
                    if (IsSocket) {
                        $("#gameOtherInfo").html('<a href="/home/viewgame/' + data + '" target="_blank">游戏' + data +'</a>到你的回合');
                    } else {
                        window.location.href = "/home/viewgame/" + data;
                        alert("你的回合");
                    }
                }

            });
    }
    setTimeout('GetNextGame()', timeJiange);
}

