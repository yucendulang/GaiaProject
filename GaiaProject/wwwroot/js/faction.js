﻿
var list = canvasobjList;

function getEventPosition(ev) {
    var x, y;
    //if (ev.layerX || ev.layerX == 0) {
    //    x = ev.layerX;
    //    y = ev.layerY;
    //} else if (ev.offsetX || ev.offsetX == 0) { // Opera
    //    x = ev.offsetX;
    //    y = ev.offsetY;
    //}
    x = ev.offsetX;
    y = ev.offsetY;
    return { x: x, y: y };
}

function getClickObj(x, y) {
    for (var i = 0; i < list.length; i++) {
        if (list[i].xmax >= x && list[i].xmin <= x && list[i].ymax >= y && list[i].ymin <= y) {
            return list[i];
        }
        //            console.log(list[i]);
        //            console.log(x+"/"+y)
    }
};



String.prototype.format = function (args) {
    var result = this;
    if (arguments.length > 0) {
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                if (args[key] != undefined) {
                    var reg = new RegExp("({" + key + "})", "g");
                    result = result.replace(reg, args[key]);
                }
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] != undefined) {
                    //var reg = new RegExp("({[" + i + "]})", "g");//这个在索引大于9时会有问题，谢谢何以笙箫的指出
                    var reg = new RegExp("({)" + i + "(})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
    }
    return result;
}

createMap({ id: "myCanvas", type:"build"});


//弹出地图选择位置
function selectMapPos(value) {
    $("#allistdiv").hide();
    $("#syntax").val("action {0}".format(value));
    $("#myModalCanves").modal();
    createMap({ id: "myCanvasSelect",type: "act"});
}
//选择种族，选择回合版,快速行动
$(".selectchange").change(function () {
    var obj = $(this);
    var syntax = obj.attr("syntax");
    if (syntax.indexOf("{0}") > 0) {
        $("#syntax").val(syntax.format($(this).val()));
    } else {
        $("#syntax").val(syntax + $(this).val());
    }
    //alert($(this).val());
});

//执行行动
$(".actionGp").click(function () {
    //$("#syntax").val("{0}-{1}".format($(this).attr("syntax"), $($(this).attr("valueid")).val()));
    submitData();
});
//只是赋值
$(".actionGp").click(function () {
    $("#syntax").val("{0}{1}".format($(this).attr("syntax"), $($(this).attr("valueid")).val()));
});

if (userInfo.isRound) {
    //魔力行动
    $("#actBody div").click(function () {
        var value = this.id.replace(" ", "");
        if ($(this).find("a").css("color") === "rgb(255, 0, 0)") {
            alert("行动以被执行");
        }
        else {

            if (value === "ACT6" || value === "ACT2") {
                selectMapPos(value);
            } else {
                openQueryWindow("action {0}".format(value));
                //$("#syntax").val("action {0}".format(value));
            }
        }

        //alert($(this).find("a").css("color"));
    });

    //卫星建立联邦
    $("#createAl").click(function () {
        $("#allistdiv").show();

        $("#myModalCanves").modal();
        createMap({ id: "myCanvasSelect", type:"al1"});
    });
    //直接建立联邦
    $("#createZjAl").click(function () {
        $("#allistdiv").show();

        $("#myModalCanves").modal();
        createMap({ id: "myCanvasSelect", type: "al2" });
    });
    //点击回合组推板
    $("#rbt_s_list div").click(function () {
        openQueryWindow(this.id, "确认PASS?");
    });

    //特殊行动
    $("#playerFaction .STT1False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, "确认执行?");
        }
    );
    //AC2=Q
    $("#playerFaction .AC2False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, "确认执行?");
        }
    );

    //种族能力
    $(".MapAction").click(function () {
        //
        var id = this.id;
        
        //大使星人
        if (id === "AmbFalse") {
            $("#myModalCanves").modal();
            createMap({ id: "myCanvasSelect", type: "pos", syntax:"action amb.swap {0}" });
        }
        //章鱼人
        else if (id === "FirFalse") {
            $("#myModalCanves").modal();
            createMap({
                id: "myCanvasSelect", type: "pos", showid:"#mapkjlist", func: function(pos) {
                var value = "action fir.downgrade {0}.advance {1}";
                return value.format(pos, $("#mapkjlist").val());
            } });
        }

    });
}

//弹出确认对话框
function openQueryWindow(type, title) {
    //actType = "action {0}".format(actType);
    //actType = type;
    $("#syntax").val(type);
    $("#querysyntax").text(type);
    if (title != undefined) {
        $("#querytitle").html(title);
    } else {
        //
        $("#querytitle").html("您确认要执行吗?");
    }

    $("#querycfmModel").modal();
}
//确认对话框
$("#querycfmModelYes").click(function () {
    //var value = $(this).attr("act");
    $("#querycfmModel").modal('hide');
    submitData();
});


//提交命令
function submitData() {
    var value = $("#syntax").val();
    if (value == "") {
        alert("请先选择行动");
        return;
    }
    $.post("/home/Syntax",
        {
            name: $("#test1").val(),
            syntax: value,
            factionName: $("#test3").val(),
        },
        function (data, status) {
            if (data.indexOf("error") != -1) {
                alert(data);
            } else {
                location.reload();
            }
        });
}

$(document).ready(function () {
    $('button#pwincome').click(function () {
        $.post("/home/Syntax",
            {
                name: $("#test1").val(),
                syntax: "setpower " + $(this).attr("value"),
                factionName: $("#test3").val(),
            },
            function (data, status) {
                if (data.indexOf("error") != -1) {
                    alert(data);
                } else {
                    location.reload();
                }
            });
    });
});

$(document).ready(function () {
    $("button#syn").click(function () {
        submitData();
    });
});

setTimeout('GetNextGame()', 10000); //指定1秒刷新一次

function GetNextGame() {
    if ($("#isMyTurn").val().indexOf("True") == -1) {
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


$(document).ready(function () {
    $("button#undo").click(function () {
        UndoOneStep();
    });
});
function UndoOneStep() {
    var value = $("#test1").val();
    console.log(value);
    $.get("/home/UndoOneStep/" + value,
        function (data, status) {
            console.log(data);
            location.reload();
        }
    );
}

$(document).ready(function () {
    $("button#delete").click(function () {
        var value = $("#test1").val();
        console.log(value);
        $.get("/home/DeleteOneGame/" + value,
            function (data, status) {
                console.log(data);
                location.reload();
            }
        );
    });
});