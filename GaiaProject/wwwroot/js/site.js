// Write your Javascript code.
var hex_size = 30;
var hex_width = hex_size * Math.sqrt(3);
var hex_height = hex_size * 1.5;

function DrawMap() {
    console.log("DrawMap");
    //console.log(model);
    var array = model["map"]["hexArray"];
    console.log(array);
    for (var i = 0; i < 20; i++) {
        for (var j = 0; j < 20; j++) {

            if (array[i][j] !== null) {
                //console.log(i, j, array[i][j].ogTerrain, ConvertIntToColor(array[i][j].ogTerrain), array[i][j].isCenter);
                DrawOneHex(cxt, j, i, ConvertIntToColor(array[i][j].ogTerrain), array[i][j].isCenter, array[i][j]);
                console.log(i, j, array[i][j].building);
                if (array[i][j].building !== null) {
                    //画房子
                    switch (array[i][j].building.name) {
                        case "Mine":
                            DrawMine(cxt, j, i, array[i][j].factionBelongTo);
                            break;
                        case "TradeCenter":
                            DrawTradingPost(cxt, j, i, array[i][j].factionBelongTo);
                            break;
                        case "ResearchLab":
                            DrawResearchLab(cxt, j, i, array[i][j].factionBelongTo);
                            break;
                        case "Academy":
                            DrawAcademy(cxt, j, i, array[i][j].factionBelongTo);
                            break;
                        case "StrongHold":
                            DrawStronghold(cxt, j, i, array[i][j].factionBelongTo);
                            break;
                        default:
                            console.log(array[i][j].building.name + "不支持");
                    }
                }
            }
        }
    }
}


function DrawOneHex(ctx, col, row, color,isCenter,hex) {
    //console.log("DrawHex");
    var loc = hexCenter(col, row)
    //console.log(loc[0], loc[1]);

    var x = loc[0] + Math.sin(Math.PI / 6) * hex_size;
    var y = loc[1] - Math.cos(Math.PI / 6) * hex_size;
    makeHexPath(ctx, x, y, hex_size, color);
    if (isCenter) {
        textSpaceSectorCenterName(ctx, loc[0], loc[1], hex.spaceSectorName)
    }
    textHexName(ctx, loc[0], loc[1], String.fromCharCode(65 + row) + col, color)

}
function textHexName(ctx, row, col, name, color) {
    //console.log("textHexName" +row + col + name);
    ctx.beginPath();
    cxt.font = "15px Verdana";
    cxt.fillStyle = ConvertBackGroundColorToTextColor(color);
    ctx.fillText(name, row - 15, col + 20);
    cxt.closePath();
}



function textSpaceSectorCenterName(ctx, row, col, name) {
    //console.log(row, col, "isCenter");
    ctx.beginPath();
    cxt.font = "18px Verdana";
    cxt.fillStyle = "White";
    ctx.fillText(name, row - 10, col - 10);
    cxt.closePath();
}

function hexCenter(row, col) {
    //console.log("hexCenter");
    var y_offset = 0;

    var x_offset = row % 2 ? hex_width / 2 : 0;
    var x = 5 + hex_size + col * hex_width + x_offset,
        y = 5 + hex_size + row * hex_height + y_offset;
    return [y, x];
}

function makeHexPath(ctx, x, y, size, color) {
    //console.log("makeHexPath",x,y,size,color);
    var angle = 0;

    ctx.beginPath();
    ctx.moveTo(x, y);
    for (var i = 0; i < 6; i++) {
        ctx.lineTo(x, y);
        angle += Math.PI / 3;
        x += Math.cos(angle) * size;
        y += Math.sin(angle) * size;
    }
    cxt.strokeStyle = "Black"
    cxt.fillStyle = color;
    cxt.fill();
    ctx.closePath();
    ctx.stroke();
}

function DrawMine(ctx, row, col,name) {
    var loc = hexCenter(row, col);

    ctx.save();

    ctx.beginPath();
    ctx.moveTo(loc[0], loc[1] - 10);
    ctx.lineTo(loc[0] + 10, loc[1]);
    ctx.lineTo(loc[0] + 10, loc[1] + 10);
    ctx.lineTo(loc[0] - 10, loc[1] + 10);
    ctx.lineTo(loc[0] - 10, loc[1]);
    ctx.closePath();

    fillBuilding(ctx, name);

    ctx.restore();
}

function DrawTradingPost(ctx, row, col, name) {
    var loc = hexCenter(row, col);

    ctx.save();

    ctx.beginPath();
    ctx.moveTo(loc[0], loc[1] - 20);
    ctx.lineTo(loc[0] + 10, loc[1] - 10);
    ctx.lineTo(loc[0] + 10, loc[1] - 3);
    ctx.lineTo(loc[0] + 20, loc[1] - 3);
    ctx.lineTo(loc[0] + 20, loc[1] + 10);
    ctx.lineTo(loc[0] - 10, loc[1] + 10);
    ctx.lineTo(loc[0] - 10, loc[1]);
    ctx.lineTo(loc[0] - 10, loc[1] - 10);
    ctx.closePath();

    fillBuilding(ctx, name);

    ctx.restore();
}

function DrawResearchLab(ctx, row, col, name) {
    var loc = hexCenter(row, col);
    loc[1] -= 5;

    ctx.save();

    ctx.beginPath();
    ctx.arc(loc[0], loc[1], 14, 0.001, Math.PI * 2, false);

    fillBuilding(ctx, name);

    ctx.restore();
}


function DrawStronghold(ctx, row, col, name) {
    var loc = hexCenter(row, col);
    loc[1] -= 5;
    var size = 15;
    var bend = 10;

    ctx.save();

    ctx.beginPath();
    ctx.moveTo(loc[0] - size, loc[1] - size);
    ctx.quadraticCurveTo(loc[0] - bend, loc[1],
        loc[0] - size, loc[1] + size);
    ctx.quadraticCurveTo(loc[0], loc[1] + bend,
        loc[0] + size, loc[1] + size);
    ctx.quadraticCurveTo(loc[0] + bend, loc[1],
        loc[0] + size, loc[1] - size);
    ctx.quadraticCurveTo(loc[0], loc[1] - bend,
        loc[0] - size, loc[1] - size);

    fillBuilding(ctx, name);

    ctx.restore();
}

function DrawAcademy(ctx, row, col, name) {
    var loc = hexCenter(row, col);
    var size = 7;
    loc[1] -= 5;

    ctx.save();

    ctx.beginPath();
    ctx.arc(loc[0] - size, loc[1], 12, Math.PI / 2, -Math.PI / 2, false);
    ctx.arc(loc[0] + size, loc[1], 12, -Math.PI / 2, Math.PI / 2, false);
    ctx.closePath();

    fillBuilding(ctx, name);

    ctx.restore();
}

function fillBuilding(ctx, name) {
    ctx.fillStyle = ConvertRaceIntToColor(name);
    ctx.fill();

    ctx.strokeStyle = "Black";
    ctx.lineWidth = 2;
    ctx.stroke();
}

function ConvertIntToColor(i) {
    switch (i) {
        case 0:
            return "blue";
        case 1:
            return "red";
        case 2:
            return "orange";
        case 3:
            return "yellow";
        case 4:
            return "brown";
        case 5:
            return "black";
        case 6:
            return "white";
        case 100:
            return "green";
        case 200:
            return "purple";
        case 300:
            return "grey";
        default:
            return "grey";
    }
}

function ConvertRaceIntToColor(i) {
    switch (i) {
        case 0:
            return "darkslateblue";
        case 1:
            return "darkslateblue";
        case 2:
            return "burlywood";
        case 3:
            return "burlywood";
        case 4:
            return "darkslategray";
        case 5:
            return "darkslategray";
        case 6:
            return "darkorange";
        case 7:
            return "darkorange";
        case 8:
            return "tomato";
        case 9:
            return "tomato";
        case 10:
            return "whitesmoke";
        case 11:
            return "whitesmoke";
        case 12:
            return "wheat";
        case 13:
            return "wheat";
        default:
            return "black";


    }
}

function ConvertBackGroundColorToTextColor(color) {
    if (color === "white" || color === "yellow") {
        return "black";
    } else {
        return "white";
    }
}
