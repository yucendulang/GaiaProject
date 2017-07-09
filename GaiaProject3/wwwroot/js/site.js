// Write your Javascript code.
var hex_size = 30;
var hex_width = hex_size * Math.sqrt(3);
var hex_height = hex_size * 1.5;

function DrawMap() {
    console.log("DrawMap");
    var array = model["hexArray"];
    console.log(array);
    for (var i = 0; i < 20; i++) {
        for (var j = 0; j < 20; j++) {

            if (array[i][j] != null) {
                console.log(i, j, array[i][j].ogTerrain,ConvertIntToColor(array[i][j].ogTerrain));
                DrawOneHex(cxt, j, i, ConvertIntToColor(array[i][j].ogTerrain));
            }
        }
    }
}


function DrawOneHex(ctx, col, row, color) {
    console.log("DrawHex");
    var loc = hexCenter(col, row)
    console.log(loc[0], loc[1]);

    var x = loc[0] + Math.sin(Math.PI / 6) * hex_size;
    var y = loc[1] - Math.cos(Math.PI / 6) * hex_size;

    console.log(x,y);
    makeHexPath(ctx, x, y, hex_size, color);
    ctx.stroke();
    ctx.beginPath();
    ctx.strokeText(row.toString() + ","+col.toString(),loc[0], loc[1]);
    cxt.closePath();
    cxt.fillStyle = "black";
    cxt.fill();
}

function hexCenter(row, col) {
    console.log("hexCenter");
    var y_offset = 0;

    var x_offset = row % 2 ? hex_width / 2 : 0;
    var x = 5 + hex_size + col * hex_width + x_offset,
        y = 5 + hex_size + row * hex_height + y_offset;
    return [y, x];
}

function makeHexPath(ctx, x, y, size, color) {
    console.log("makeHexPath",x,y,size,color);
    var angle = 0;

    ctx.beginPath();
    ctx.moveTo(x, y);
    for (var i = 0; i < 6; i++) {
        ctx.lineTo(x, y);
        angle += Math.PI / 3;
        x += Math.cos(angle) * size;
        y += Math.sin(angle) * size;
    }
    cxt.fillStyle = color;
    cxt.fill();
    ctx.closePath();
}


function ConvertIntToColor(i) {
    switch (i) {
        case 0:
            return "Blue";
            break;
        case 1:
            return "Red";
            break;
        case 2:
            return "Orange";
            break;
        case 3:
            return "Yellow";
            break;
        case 4:
            return "Brown";
            break;
        case 5:
            return "Black";
            break;
        case 6:
            return "White";
            break;
        case 100:
            return "Green";
            break;
        case 200:
            return "Purple";
            break;
        case 300:
            return "Grey";
            break;
        default:
            return "Grey";
            break;
    }
}