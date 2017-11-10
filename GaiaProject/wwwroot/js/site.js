// Write your Javascript code.
var hex_size = 30;
var hex_width = hex_size * Math.sqrt(3);
var hex_height = hex_size * 1.5;

function DrawMap() {
    console.log("DrawMap");
    //console.log(model);
    var array = model["map"]["hexArray"];
    console.log(array);
    renderColorCycle(cxt, "white");
    for (var i = 0; i < 20; i++) {
        for (var j = 0; j < 20; j++) {

            if (array[i][j] !== null) {
                //console.log(i, j, array[i][j].ogTerrain, ConvertIntToColor(array[i][j].ogTerrain), array[i][j].isCenter);
                DrawOneHex(cxt, j, i, ConvertIntToColor(array[i][j].tfTerrain), array[i][j].isCenter, array[i][j]);
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
                        case "GaiaBuilding":
                            DrawGaiaBuilding(cxt, j, i, array[i][j].factionBelongTo);
                            break;
                        default:
                            console.log(array[i][j].building.name + "不支持");
                    }
                }
                if (array[i][j].satellite !== null) {
                    DrawSatellite(cxt, j, i, array[i][j].satellite);
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
    cxt.font = "12px Verdana";
    cxt.fillStyle = ConvertBackGroundColorToTextColor(color);
    ctx.fillText(name, row - 11, col + 20);
    cxt.closePath();
}



function textSpaceSectorCenterName(ctx, row, col, name) {
    //console.log(row, col, "isCenter");
    ctx.beginPath();
    cxt.font = "12px Verdana";
    cxt.fillStyle = "White";
    ctx.fillText(name, row - 8, col - 12);
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

function DrawGaiaBuilding(ctx, row, col, name) {
    var loc = hexCenter(row, col);
    loc[1] -= 15;
    loc[0] -= 10;
    ctx.save();
    fillBuilding(ctx, name);

    ctx.beginPath();
    ctx.fillRect(loc[0], loc[1], 20, 20);
    ctx.strokeRect(loc[0], loc[1], 20, 20);

    ctx.restore();
}

function DrawSatellite(ctx, row, col, satellite) {
    switch (satellite.length) {
        case 1:
            var loc = hexCenter(row, col);
            loc[1] -= 9;
            loc[0] -= 4;
            ctx.save();
            fillBuilding(ctx, satellite[0]);
            ctx.beginPath();
            ctx.fillRect(loc[0], loc[1], 8, 8);
            ctx.strokeRect(loc[0], loc[1], 8, 8);
            ctx.restore();
            break;
        default:
            break;
    }
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
            return "#60C0F0";
        case 1:
            return "#F08080";
        case 2:
            return "#F0C060";
        case 3:
            return "#F0F080";
        case 4:
            return "#B08040";
        case 5:
            return "#C0C0C0";
        case 6:
            return "#E0F0FF";
        case 100:
            return "#80F080";
        case 200:
            return "#D19FE8";
        case 300:
            return "#FFFFFF";
        default:
            return "#FFFFFF";
    }
}

var cycle = ["blue", "red", "orange", "yellow", "brown", "black", "white"]; 

function ConvertRaceIntToColor(i) {
    switch (i) {
        case 0:
            return "#2080F0";
        case 1:
            return "#2080F0";
        case 2:
            return "#A06040";
        case 3:
            return "#A06040";
        case 4:
            return "#808080";
        case 5:
            return "#808080";
        case 6:
            return "#F0A020";
        case 7:
            return "#F0A020";
        case 8:
            return "#E04040";
        case 9:
            return "#E04040";
        case 10:
            return "#F0F8FF";
        case 11:
            return "#F0F8FF";
        case 12:
            return "#E0E040";
        case 13:
            return "#E0E040";
        default:
            return "black";


    }
}

function ConvertBackGroundColorToTextColor(color) {
    if (color === "white" || color === "yellow") {
        return "white";
    } else {
        return "black";
    }
}

function renderColorCycle(ctx, startColor) {

    ctx.save()
    //ctx.scale(2, 2);
    ctx.translate(830, 50);

    var base = cycle.indexOf(startColor);

    for (var i = 0; i < 7; ++i) {
        var terrain = cycle[(base + i) % 7];

        ctx.save()

        var size = 10;

        var angle = (Math.PI * 2 / 7) * i - Math.PI / 2;
        ctx.translate(30 * Math.cos(angle), 30 * Math.sin(angle));

        ctx.beginPath();
        ctx.arc(0, 0, size, Math.PI * 2, 0, false);
        console.log(terrain);
        ctx.fillStyle = terrain;
        ctx.fill();

        ctx.stroke();

        ctx.restore();
    }
    ctx.restore();
}
