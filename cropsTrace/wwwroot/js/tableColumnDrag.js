function sortTable(table, idx) {
    var otable = $(table +" .el-table__body")[0],
        //otody = otable.tBodies[0],
        otody = $(table + " .el-table__body tbody")[0]
        otr = otody.rows,
        tarr = [];
    for (var i = 1; i < otr.length; i++) {
        tarr[i - 1] = otr[i];
    };
    // console.log(tarr);
    if (otody.sortCol == idx) {
        tarr.reverse();
    } else {
        tarr.sort(function (tr1, tr2) {
            var value1 = tr1.cells[idx].innerHTML;
            var value2 = tr2.cells[idx].innerHTML;
            if (!isNaN(value1) && !isNaN(value2)) {
                return value1 - value2;
            } else {
                return value1.localeCompare(value2);
            }
        })
    }
    var fragment = document.createDocumentFragment();
    for (var i = 0; i < tarr.length; i++) {
        fragment.appendChild(tarr[i]);
    };
    otody.appendChild(fragment);
    otody.sortCol = idx;
}
//拖动
function Drag(table) {
    var ochek = document.getElementById("chenkbox"),
        otable = $(table + " .el-table__body")[0],
        jqtable = $(table + " .el-table__body"),
        //otody = otable.tBodies[0],
        otody = $(table + " .el-table__body tbody")[0],
        //oth = Array.from(otody.getElementsByTagName("th")),
        othead = $(table + " .el-table__header")[0].getElementsByTagName("th"),
        oth = $(table +" .el-table__body")[0].getElementsByTagName("tr"),
        //otd = Array.from(otody.getElementsByTagName("td")),
        otd = otody.getElementsByTagName("td"),
        //otd = $(table).find("td");
        box = document.getElementById("box"),
        arrn = [];
    console.log("Drag");
    console.log($(table));
    console.log("Drag otable");
    console.log(otable);
    console.log("Drag otd");
    console.log(otd);
    console.log("Drag oth");
    console.log(oth);
    console.log("Drag othead");
    console.log(othead);
    console.log("Drag box");
    console.log(box);
    $(box).css("height", jqtable.css("height"));
    console.log("otd.length="+otd.length);
    for (var i = 0; i < otd.length; i++) {
        console.log(otd[i]);
        otd[i].onmousedown = function (e) {
            var e = e || window.event,
                target = e.target || e.srcElement,
                thW = target.offsetWidth,
                maxl = ochek.offsetWidth - thW,
                rows = otable.rows,
                ckL = ochek.offsetLeft,
                disX = target.offsetLeft,
                _this = this,
                cdisX = e.clientX - ckL - disX;
            console.log("cdisX=" + cdisX);
            var thitem = othead[this.cellIndex];
            var ophead = document.createElement("p");
            ophead.innerHTML = thitem.innerHTML;
            console.log("ophead");
            console.log(ophead);
            box.appendChild(ophead);
            for (var i = 0; i < rows.length; i++) {
                var op = document.createElement("p");               
                op.innerHTML = rows[i].cells[this.cellIndex].innerHTML;
                box.appendChild(op);
            };
            for (var i = 0; i < othead.length-1; i++) {
                arrn.push(othead[i].offsetLeft);
            };
            console.log("arrn");
            console.log(arrn);
            box.style.display = "block";
            box.style.width = thW + "px";
            box.style.left = disX + "px";
            document.onmousemove = function (e) {
                var e = e || window.event,
                    target = e.target || e.srcElement,
                    thW = target.offsetWidth;
                box.style.top = 0;
                box.style.left = e.clientX - ckL - cdisX + "px";
                if (box.offsetLeft > maxl) {
                    box.style.left = maxl + "px";
                } else if (box.offsetLeft < 0) {
                    box.style.left = 0;
                }
                //console.log("target");
                //console.log(target);
                document.onselectstart = function () {
                    return false
                };
                window.getSelection ? window.getSelection().removeAllRanges() : doc.selection.empty();
            }
            document.onmouseup = function (e) {
                var e = e || window.event,
                    opr = box.getElementsByTagName("p"),
                    oboxl = box.offsetLeft + cdisX;
                console.log("oboxl=" + oboxl);
                for (var i = 0; i < arrn.length; i++) {
                    if (arrn[i] < oboxl) {
                        var index = i;
                    }
                };
                console.log("rows");
                console.log(rows);
                for (var i = 0; i < rows.length; i++) {
                    rows[i].cells[_this.cellIndex].innerHTML = "";
                    console.log("rows[i].cells");
                    console.log(rows[i].cells);
                    console.log("index=" + index);
                    console.log(rows[i].cells[index]);
                    //if (typeof (rows[i].cells[index]) != "undefined") {
                    console.log("_this.cellIndex=" + _this.cellIndex);
                    console.log(rows[i].cells[index]);
                    console.log("othead[_this.cellIndex]");
                    console.log(othead[_this.cellIndex])
                    console.log("othead[index]");
                    console.log(othead[index]);
                    var currentThead = othead[index].innerHTML;
                    console.log("currentThead");
                    console.log(currentThead);
                    $(table + " .el-table__header th").eq(index).html(ophead.innerHTML);
                    //othead[_this.cellIndex].innerHTML = othead[index].innerHTML;
                    $(table + " .el-table__header th").eq(_this.cellIndex).html(currentThead.innerHTML);
                    rows[i].cells[_this.cellIndex].innerHTML = rows[i].cells[index].innerHTML;
                    rows[i].cells[index].innerHTML = "";
                    rows[i].cells[index].innerHTML = opr[i+1].innerHTML;
                    //}
                };
                box.innerHTML = "";
                arrn.splice(0, arrn.length);
                box.style.display = "none";
                document.onmousemove = null;
                document.onmouseup = null;
                document.onselectstart = function () {
                    return false
                };
            }

        }
    };
    console.log("Drag otd");
    console.log(otd);

}
//调用代码("表格ID");
//Drag("tableSort");