setTimeout('GetNextGame()', 10000); //指定1秒刷新一次

function GetNextGame() {
    if ($("#isMyTurn").val().indexOf("True") === -1) {
        $.post("/home/GetNextGame",
            {
                name: $("input#username").val()
            },
            function (data, status) {
                if (data == undefined || data === "") {
                    //console.log(data);
                } else {
                    //console.log(data);
                    window.location.href = "/home/viewgame/" + data;
                    alert("你的回合");
                }

            });
    }
    setTimeout('GetNextGame()', 10000);
}


//setTimeout('GetNextGame()', 10000); //指定1秒刷新一次
//
//function GetNextGame() {
//    if ($("#isMyTurn").text().indexOf("True") == -1) {
//        $.post("/home/GetNextGame",
//            {
//                name: $("p#username").text()
//            },
//            function (data, status) {
//                console.log(data);
//                if (data == undefined || data == "") {
//                } else {
//                    window.location.href = "/home/viewgame/" + data;
//                    alert("你的回合");
//                }
//            });
//    }
//    setTimeout('GetNextGame()', 10000);
//}