
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

if (false &&  userInfo != undefined && userInfo.paygrade > 0) {
    (function bkimg() {
        var planetImgs = [
            "terra", "oxide", "volcanic", "desert", "swamp", "titanium", "ice", "gaia", "transdim", "space"
        ];
        colorPlant = {
            "#6bd8f3": "terra",
            "#f23c4d": "oxide",
            "#ea8736": "volcanic",
            "#facd2f": "desert",
            "#ad5e2f": "swamp",
            "#a3a3a3": "titanium",
            "#d3f1f5": "ice",
            "#80F080": "gaia",
            "#D19FE8": "transdim",
            "#000000": "space"
        };
        var loadedImgs = 0;
        for (var i = 0; i < planetImgs.length; i++) {
            var img = $('<img src="/images/planet/' +
                planetImgs[i] +
                '.png" id="' +
                planetImgs[i] +
                '" style="display: none;" />')[0];
            img.onload = function() {
                this.onload = null;
                loadedImgs++;
                if (loadedImgs >= 10) {
                    createMap({ id: "myCanvas", type: "build" });
                }
            }
            document.getElementsByTagName("body")[0].appendChild(img);
        }
    })();
} else {
    createMap({ id: "myCanvas", type: "build" });
}


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

//弹出地图选择位置
function selectMapPos(value, syntax, action) {
    $("#allistdiv").hide();
    $("#myModalCanves").modal();
    if (action === undefined || action === "ACT6" || action === "ACT2" || action === "RBT1" || action === "RBT2") {
        //action = "act";
        syntax = "action {0}".format(value) + " {0}";
        //$("#syntax").val(syntax);
    } else {
        //action = "planet";
    }
    createMap({ id: "myCanvasSelect", type: "act", action: action, syntax: syntax });
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
    //submitData();
    openQueryWindow($("#syntax").val(), _("确认?"));
});


if (userInfo.isRound) {
    //能量行动
    $("#actBody div").click(function () {
        var value = this.id.replace(" ", "");
        if ($(this).find("a").css("color") === "rgb(255, 0, 0)") {
            alert(_("行动已被执行"));
        } else {

            if (value === "ACT6" || value === "ACT2") {
                selectMapPos(value, null, value);
            } else if (value === "ACT8") {
                //$('#myModal').modal();
                openSelectTT("action ACT8.{0}");
            } else if (value === "ACT9") {
                $("#myAltModal").modal();
            } else {
                openQueryWindow("action {0}".format(value), _("确认执行?"), $(this).attr("tishi"));
                //$("#syntax").val("action {0}".format(value));
            }
        }

        //alert($(this).find("a").css("color"));
    });

    //选择建筑位置
    $("#createZjAl").click(function () {
        $("#allistdiv").show();

        $("#myModalCanves").modal();
        createMap({ id: "myCanvasSelect", type: "al2", action: _("建筑") });
    });

    //点击回合组推板
    $("#rbt_s_list div").click(function () {
        openQueryWindow(this.id, _("确认PASS?"));
    });

    //特殊行动
    $("#playerFaction .STT1False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, _("确认执行?"));
        }
    );
    //AC2=Q
    $("#playerFaction .AC2False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, _("确认执行?"));
        }
    );
    //AC2=4C
    $("#playerFaction .BalAC2False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, _("确认执行?"));
        }
    );

    //放置黑星
    $("#planetMine").click(function () {
        selectMapPos("", "planet {0}", "planet");
    });

    //种族能力
    $(".MapAction").click(function () {
        //
        var id = this.id;

        //大使星人
        if (id === "AmbFalse") {
            $("#myModalCanves").modal();
            createMap({ id: "myCanvasSelect", type: "pos", syntax: "action amb.swap {0}" });
        }
        //章鱼人
        else if (id === "FirFalse") {
            $("#myModalCanves").modal();
            createMap({
                id: "myCanvasSelect",
                type: "pos",
                showid: "#mapkjlist",
                func: function (pos) {
                    var value = "action fir.downgrade {0}.advance {1}";
                    return value.format(pos, $("#mapkjlist").val());
                }
            });
        }
        //蜂人
        else if (id === "HivFalse") {
            selectMapPos("hiv");
        }

    });


    //快速行动插入
    $(".ksaction").click(function () {

        var obj = $(this);

        //行动类型，前后
        var actionType = obj.attr("actionType");

        var oldcode = $("#syntax").val();
        if (actionType !== 'now' && oldcode === "") {
            alert(_("必须先选择主要行动"));
            return;
        }


        //对象ID
        var controlid = obj.attr("controlid");
        //下拉对象
        var control = $(controlid);
        //下来值
        var syntax = control.attr("syntax");
        //下拉命令
        var value = control.val();
        //下拉代码
        if (value === "") {
            alert(_("请选择要进行的操作"));
            return;
        } else {
            value = syntax.format(value);
        }
        //是不是执行多次
        var nowcode = "";
        var actionnumber = obj.attr("actionnumber");
        if (actionnumber !== undefined && actionnumber !== null && actionnumber !== "") {
            var number = parseInt($(actionnumber).val());
            if (number > 0) {
                for (var i = 0; i < number; i++) {
                    //oldcode = $("#syntax").val();
                    nowcode = nowcode + value + ".";
                }
                nowcode = nowcode.substr(0, nowcode.length - 1);
            }
        } else {
            nowcode = value;
        }

        if (actionType === "before") {
            $("#syntax").val(nowcode + "." + oldcode);
        } else if (actionType === "after") {
            $("#syntax").val(oldcode + '.' + nowcode);
        }
        else if (actionType === "now") {
            openQueryWindow(nowcode, _("确认执行?"));
        }

    });

    //点击高级板块
    $("#playerFaction .ATT1False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, _("确认执行?"));
        }
    );
    $("#playerFaction .ATT2False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, _("确认执行?"));
        }
    );
    $("#playerFaction .ATT3False").click(function () {
            var obj = $(this);
            openQueryWindow(obj.attr("syntax") + this.id, _("确认执行?"));
        }
    );
} else {
    //放置黑星
    $("#planetMine").click(function () {
        selectMapPos("", "planet {0}", "planet");
    });
}


var windowFunc;
//弹出确认对话框
function openQueryWindow(type, title, tishi, func) {
    //actType = "action {0}".format(actType);
    //actType = type;
    $("#syntax").val(type);
    $("#querysyntax").text(type);
    if (tishi != undefined) {
        $("#querytishi").html(tishi);
    }
    if (title != undefined) {
        $("#querytitle").html(title);
    } else {
        //
        $("#querytitle").html(_("您确认要执行吗?"));
    }

    $("#querycfmModel").modal();
    windowFunc = func;
}
//手动输入
$("#queryHandinput").click(function () {
    //$("#syntax").focus();
    if (windowFunc != undefined) {
        windowFunc();
    }
});
//确认对话框
$("#querycfmModelYes").click(function () {
    //var value = $(this).attr("act");
    $("#querycfmModel").modal('hide');
    submitData();
    if (windowFunc != undefined) {
        windowFunc();
    }
});
//执行并且跳过回合
$("#queryAndPass").click(function () {
    //var value = $(this).attr("act");
    $("#querycfmModel").modal('hide');
    //alert(userInfo.stage);
    //如果是行动阶段，直接执行
    if (userInfo.stage === 5) {
        $("#syntax").val($("#syntax").val() + ".pass turn");
    }
    submitData();
    if (windowFunc != undefined) {
        windowFunc();
    }
});

function syntaxCode(syntax, faction) {
    $.post("/home/Syntax",
        {
            name: $("#test1").val(),
            syntax: syntax,
            factionName: faction,
        },
        function (data, status) {
            if (data.indexOf("error") != -1) {
                alert(data);
            } else {
                location.reload();
            }
        });
}

//提交命令
function submitData() {
    var value = $("#syntax").val();
    if (value === "") {
        alert(_("请先选择行动"));
        return;
    }
    $.post("/home/Syntax",
        {
            name: $("#test1").val(),
            syntax: value,
            factionName: $("#test3").val(),
        },
        function(data, status) {
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

$(document).ready(function () {
    $("button#redo").click(function () {
        var value = $("#test1").val();
        $.get("/home/RedoOneStep/" + value,
            function (data, status) {
                location.reload();
            }
        );
    });
});

$(document).ready(function () {
    $("button#report").click(function () {
        var value = $("#test1").val();
        $.get("/home/ReportBug/" + value,
            function (data, status) {
                location.reload();
            }
        );
    });
});


//
function faction_getLog(gameName, factionName, factionType) {
    $.get("/Home/SyntaxLog/" + gameName + "?factionName=" + factionName + "&factionType=" + factionType,
        function (data) {
            //alert(data);
            $("#queryInfoBody").html("<table class='table'><thead data-toggle='collapse' data-target='#demo'><tr><th>种族名</th><th class='text-right'>变化数值</th><th>分数</th><th class='text-right'>变化数值</th><th>信用点</th><th class='text-right'>变化数值</th><th>矿</th><th class='text-right'>变化数值</th><th>QIC</th><th class='text-right'>变化数值</th><th>知识</th><th class='text-right'>变化数值</th><th>能量</th><th>语句</th></tr></thead>" + data.data + "</table>");
            $("#queryInfoModel").modal();
        });
}
//得分明细
function faction_scoreShow(gameName, factionName) {
    //alert(factionName);
    //openQueryWindow($("#syntax").val(), "确认?");
    faction_getLog(gameName, factionName, 1);
}
//日志明细
function faction_logShow(gameName, factionName) {
    //alert(factionName);
    faction_getLog(gameName, factionName, 0);

}

//drop种族
function dropFaction(gameName, factionName) {
    //alert(username);
    $.get("/Home/DropFaction/" + gameName + "?factionName=" + factionName, function(data) {
        if (data.info.state === 200) {
            location.reload();
        }
    });
}