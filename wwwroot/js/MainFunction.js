var s_Count = 0;    //新增表身空白列計數
var s_status = 0;   //資料處理狀態
function Main() {
    if (selectOption != undefined) {
        selectOption.forEach(function (element0, index0) {  //選項代入
            element0[1].split(",").forEach(function (element1, index1) {
                $('#' + element0[0]).append("<option value='" + element1.split("@")[0] + "'>" + element1.split("@")[1] +"</option>")
            });
        });
    }
    $('#btnOk').attr("disabled", true);
    $('#btnCan').attr("disabled", true);
    $('#btnPrev').click(function () {
        btnPrev();
    });
    $('#btnNext').click(function () {
        btnNext();
    });
    $('#btnIns').click(function () {
        btnIns();
    });
    $('#btnModi').click(function () {
        btnModi();
    });
    $('#btnOk').click(function () {
        btnOk();
        s_status = 0;
    });
    $('#btnPlus').click(function () {
        btnPlus();
    });
    $('img[id^=btnMinus_]').unbind('click').click(function () {
        btnMinus();
    });
    $('#btnCan').click(function () {
        btnCan();
        s_status = 0;
    });
    $('#btnDel').click(function () {
        btnDel();
    });
    $('input[id^=vChk_]').click(function() {
        $('input[id^=vChk_]').prop('checked',false);
        $(this).prop('checked',true);
        refresh();
    });
    $('#tView tr').click(function () {
        $(this).find('input[id*=vChk_]').click();
    });
    refresh();
    if (typeof (tHeadListen) == 'function')
        tHeadListen();
}

function q_gtNoa(DataBase, async, Noa, page, count) {
    var result;
    $.ajax({
        'type': 'post',
        'url': '/home/GetData',
        data: { "DB": DataBase, "Noa": Noa, "page": page, "count": count },
        traditional: true,
        async: async,
        success: function (data) {
            result = data;
        },
        error: function (message) {
            alert('error!');
        }
    });
    return result;
}
function q_gt(DataBase, async, swhere) {
    /*
     swhere 格式
     swhere = "Noa:1101007001;Flavor:起司"
     欄位名稱第一個字大寫!!
     */
    var result;
    $.ajax({
        'type': 'post',
        'url': escape("/home/GetDataWhere"),
        data: { "DB": DataBase, "swhere": swhere },
        traditional: true,
        async: async,
        success: function (data) {
            result = data;
        },
        error: function (message) {
            alert('error!');
        }
    });
    return result;
}
function refresh() {
    var nowPage = $('#nowPage').val();
    var nowCount = parseInt(getIDNum($('input[id^=vChk_]:checked').attr('id')));
    var abbm = q_gtNoa(DB, 0, "", nowPage, brwCount);  //表頭
    if (abbm!=undefined && abbm.length > 0) {
        //view值刷新
        for (var i = 0; i < brwCount; i++) {
            var currentDataCount = (nowPage - 1) * brwCount + i;
            if (currentDataCount < abbm.length) {
                Object.keys(abbm[currentDataCount]).forEach(function (part) {
                    $('#v' + part.substr(0, 1).toUpperCase() + part.substr(1, part.length) + '_' + i).text(abbm[currentDataCount][part]);
                });
            }
            else {
                Object.keys(abbm[0]).forEach(function (part) {
                    $('#v' + part.substr(0, 1).toUpperCase() + part.substr(1, part.length) + '_' + i).text('');
                });
            }
        }
        //表頭abbm值刷新
        var currentDataCount = (nowPage - 1) * brwCount + nowCount;
        if (abbm[currentDataCount] != undefined) {
            Object.keys(abbm[currentDataCount]).forEach(function (part) {
                var partToID = part.substr(0, 1).toUpperCase() + part.substr(1, part.length);
                if ($('#txt' + partToID).length > 0)
                    $('#txt' + partToID).val(abbm[currentDataCount][part]);
                if ($('#chk' + partToID).length>0)
                    $('#chk' + partToID).prop('checked', abbm[currentDataCount][part]);
                if ($('#select' + partToID).length > 0)
                    $('#select' + partToID + ' option[value="' + abbm[currentDataCount][part]+'"]').prop('selected', true);
            });
            //點選到的表頭有值 表身刷新--------
            $.ajax({
                url: "/Home/GetPartial",
                type: 'GET',
                cache: false,
                data: { "DB": DB + "s", "Noa": abbm[currentDataCount].noa}
            }).done(function (result) {
                $('#abbs').html(result);
            });
            //$("#tBody tBody").load("/Home/GetPartial", DB + "s"+";"+abbm[currentDataCount].noa);
            //---------------------
        }
        else {
            //點選到的表頭為空 表身刷新--------
            $.ajax({
                url: "/Home/GetPartial",
                type: 'GET',
                cache: false,
                data: { "DB": DB + "s", "Noa": null }
            }).done(function (result) {
                $('#abbs').html(result);
            });
            $('#tHead input[id^=txt]').val('');
            $('#tHead textarea').val('');
            $('#tHead select').each(function () {
                $(this)[0].selectedIndex = 0;
            });
            $('#tBody input[id^=txt]').val('');
            $('#tBody textarea').val('');
            $('#tBody select').each(function () {
                $(this)[0].selectedIndex = 0;
            });
        }
    }
    else {
        //全部沒值 表身刷新--------
        $.ajax({
            url: "/Home/GetPartial",
            type: 'GET',
            cache: false,
            data: { "DB": DB + "s", "Noa": null }
        }).done(function (result) {
            $('#abbs').html(result);
        });
        $('#tHead input[id^=txt]').val('');
        $('#tHead textarea').val('');
        $('#tHead select').each(function () {
            $(this)[0].selectedIndex = 0;
        });
        $('#tBody input[id^=txt]').val('');
        $('#tBody textarea').val('');
        $('#tBody select').each(function () {
            $(this)[0].selectedIndex = 0;
        });
    }
}
function inputMask() {
    var tBobyCount = $('#tBody tr').length - 1;
    if (HeadMask != undefined) {
        HeadMask.forEach(function (element, index) {
            $('#' + element[0]).inputmask("remove");
            $('#' + element[0]).inputmask({
                autoUnmask: true,
                mask: element[1]
            });
        });
    }
    if (HeadNumMask != undefined) {
        HeadNumMask.forEach(function (element, index) {
            $('#' + element[0]).inputmask("remove");
            $('#' + element[0]).inputmask({
                alias: 'currency',
                allowMinus: true,
                digits: element[1],
                autoUnmask: true,
                prefix: ""
            });
        });
    }
    if (BodyMask != undefined) {
        BodyMask.forEach(function (element, index) {
            for (var i = 0; i < tBobyCount; i++) {
                $('#' + element[0] + i).inputmask("remove");
                $('#' + element[0] + i).inputmask({
                    autoUnmask: true,
                    mask: element[1]
                });
            }
        });
    }
    if (BodyNumMask != undefined) {
        BodyNumMask.forEach(function (element, index) {
            for (var i = 0; i < tBobyCount; i++) {
                $('#' + element[0] + i).inputmask("remove");
                $('#' + element[0] + i).inputmask({
                    alias: 'currency',
                    allowMinus: true,
                    digits: element[1],
                    autoUnmask: true,
                    prefix: ""
                });
            }
        });
    }
}
function getIDNum(ID) {
    return ID.split('_')[1];
}
function getID(ID,ifLine) {
    return ID.substr(0, ID.indexOf('_') + ifLine);
}
function right(str, len) {
    return str.substr(str.length - len, len);
}
function left(str, len) {
    return str.substr(0, len);
}

function s_href(Href) {
    var ObjHref = {};
    var HomeIndex = Href.indexOf('Home');
    if (HomeIndex > -1) {
        ObjHref['location'] = Href.substr(Href.indexOf('/', HomeIndex) + 1, Href.length - HomeIndex - 1);
        return ObjHref.location;
    }
    return 0;
}
function btnPrev() {
    if ($('#nowPage').val() > 1) {
        $('#nowPage').val(parseInt($('#nowPage').val()) - 1);
        $('input[id^=vChk_]').prop('checked', false);
        $('#vChk_0').prop('checked', true);
        refresh();
    }
}
function btnNext() {
    if ($('#nowPage').val() < $('#totPage').val()) {
        $('#nowPage').val(parseInt($('#nowPage').val()) + 1);
        $('input[id^=vChk_]').prop('checked', false);
        $('#vChk_0').prop('checked', true);
        refresh();
    }
}
function btnIns() {     //新增
    s_status = 1;
    //tHead
    $('#tHead input[type=text]').each(function () {
        $(this).val('');
        if (stableInputHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tHead input[type=checkbox]').each(function () {
        $(this).prop('checked', false);
        if (stableInputHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tHead select').each(function () {
        $(this)[0].selectedIndex = 0;
        if (stableInputHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tHead textarea').each(function () {
        $(this).val('');
        if (stableInputHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    //tBody
    $.ajax({
        url: "/Home/GetPartial",
        type: 'GET',
        cache: false,
        data: { "DB": DB + "s", "Noa": null }
    }).done(function (result) {
        $('#abbs').html(result);
        $('#abbs input').each(function () {
            if (stableInputBodyField.indexOf($(this).attr("id")) < 0) {
                $(this).removeAttr('disabled');
            }
        });
        $('#abbs select').each(function () {
            if (stableInputBodyField.indexOf($(this).attr("id")) < 0) {
                $(this).removeAttr('disabled');
            }
        });
        $('#abbs textarea').each(function () {
            if (stableInputBodyField.indexOf($(this).attr("id")) < 0) {
                $(this).removeAttr('disabled');
            }
        });
        tBodyRelisten();
        inputMask();
    });
    $('#btnMinus_' + s_Count).click(function (e) {
        btnMinus(e);
    });
    s_Count++;
    $('#tView input[id*=vChk_]').each(function () {
        $(this).attr('disabled', 'disabled');
    });
    $('#btnPlus').css("display", "block");
    $('#btnIns').attr("disabled", true);
    $('#btnModi').attr("disabled", true);
    $('#btnDel').attr("disabled", true);
    $('#btnOk').attr("disabled", false);
    $('#btnCan').attr("disabled", false);
    $('#btnPrev').attr("disabled", true);
    $('#btnNext').attr("disabled", true);
}
function btnModi() {     //修改
    s_status = 2;
    //tHead
    $('#tHead input[type=text]:not([id=txtNoa])').each(function () {
        if (stableHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tHead input[type=checkbox]').each(function () {
        if (stableHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tHead textarea').each(function () {
        if (stableHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tHead select').each(function () {
        if (stableHeadField.indexOf($(this).attr("id")) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    //tBody
    $('#tBody input[type=text]:not([id^=txtNoq_])').each(function () {
        if (stableBodyField.indexOf(getID($(this).attr("id"), 1)) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tBody input[type=checkbox]').each(function () {
        if (stableBodyField.indexOf(getID($(this).attr("id"), 1)) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tBody select').each(function () {
        if (stableBodyField.indexOf(getID($(this).attr("id"), 1)) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#tBody textarea').each(function () {
        if (stableBodyField.indexOf(getID($(this).attr("id"), 1)) < 0) {
            $(this).removeAttr('disabled');
        }
    });
    $('#btnPlus').css("display", "block");
    $('#btnIns').attr("disabled", true);
    $('#btnModi').attr("disabled", true);
    $('#btnDel').attr("disabled", true);
    $('#btnOk').attr("disabled", false);
    $('#btnCan').attr("disabled", false);
    $('#btnPrev').attr("disabled", true);
    $('#btnNext').attr("disabled", true);
    tBodyRelisten();
    inputMask();
}
function btnDel() {     //刪除
    var choice = confirm("確定刪除?");
    s_status = 3;
    if (choice) {
        btnOk();
    }
    else {
        btnCan();
    }
}


function btnCan() {
    s_status = 0;
    //tHead
    $('#tHead input[type=text]').each(function () {
        $(this).attr('disabled',true);
    });
    $('#tHead input[type=checkbox]').each(function () {
        $(this).attr('disabled', true);
    });
    $('#tHead select').each(function () {
        $(this).attr('disabled', true);
    });
    $('#tHead textarea').each(function () {
        $(this).attr('disabled', true);
    });
    //tBody
    s_Count = 0;
    $('img[id^=btnMinus_]').hide();
    $('input[id^=vChk_]').each(function () {
        $(this).removeAttr('disabled');
    });
    $('#btnIns').attr("disabled", false);
    $('#btnModi').attr("disabled", false);
    $('#btnDel').attr("disabled", false);
    $('#btnOk').attr("disabled", true);
    $('#btnCan').attr("disabled", true);
    $('#btnPrev').attr("disabled", false);
    $('#btnNext').attr("disabled", false);
    refresh();
}

function btnPlus() {
    if (s_status == 1 || s_status == 2) {
        if (s_status == 2)
            s_Count = $('#abbs th input[id^=txtNoq_]').length;
        var HTML = $('#abbs tr:first-child')[0].outerHTML;
        HTML = HTML.replaceAll("_0", "_" + s_Count);
        $('#abbs tr:last-child').after(HTML);
        $('#abbs tr:last-child input').val('');
        $('#abbs tr:last-child textarea').val('');
        $('#abbs tr:last-child input[type=checkbox]').each(function () {
            $(this).prop('checked', false);
        });
        $('#abbs tr:last-child select option').remove();
        $('#btnMinus_' + s_Count).click(function (e) {
            btnMinus(e);
        });
        $('#txtNoq_' + s_Count).val(right('000' + (parseInt(s_Count,10)+1), 3));
        s_Count++;
        tBodyRelisten();
        inputMask();
    }
}

function btnMinus(e) {
    if (s_status == 1 || s_status == 2) {
        $('#' + e.currentTarget.id).parents('tr').children('td').children('input[type=text]:not([id^=txtNoq_])').val('');
    }
}

function btnOk() {
    switch (s_status) {
        case 1:     //新增
            //tHead
            var tHead = {};
            $('#tHead input[id^=txt]').each(function () {
                tHead[$(this).attr("id").replace('txt', '').toLowerCase()] = $(this).val();
            });
            $('#tHead input[id^=chk]').each(function () {
                tHead[$(this).attr("id").replace('chk', '').toLowerCase()] = $(this).prop('checked');
            });
            $('#tHead select[id^=select]').each(function () {
                tHead[$(this).attr("id").replace('select', '').toLowerCase()] = $(this).val();
            });
            $('#tHead textarea[id^=txt]').each(function () {
                tHead[$(this).attr("id").replace('txt', '').toLowerCase()] = $(this).val();
            });
            //tBody
            var tBody = [];
            var tBodytemp = {};
            $('#tBody tBody tr').each(function () {
                $(this).find('input[id^=txt]:not([id^=txtNoq_])').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('txt', '').toLowerCase()] = $(this).val();
                });
                $(this).find('input[id^=chk]').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('chk', '').toLowerCase()] = $(this).prop('checked');
                });
                $(this).find('select[id^=select]').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('select', '').toLowerCase()] = $(this).val();
                });
                $(this).find('textarea[id^=txt]').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('txt', '').toLowerCase()] = $(this).val();
                });
                for (var i = 0; i < mustBodyField.length; i++) {    //若一定要輸入的欄位為空則不寫入資料
                    if (tBodytemp[mustBodyField] == null || tBodytemp[mustBodyField] == "") {
                        tBodytemp = {};
                        break;
                    }
                }
                if (Object.keys(tBodytemp).length>0)
                    tBody.push(tBodytemp);
                tBodytemp = {};
            });
            break;
        case 2:     //修改
            //tHead
            var tHead = {};
            $('#tHead input[id^=txt]').each(function () {
                tHead[$(this).attr("id").replace('txt', '').toLowerCase()] = $(this).val();
            });
            $('#tHead input[id^=chk]').each(function () {
                tHead[$(this).attr("id").replace('chk', '').toLowerCase()] = $(this).prop('checked');
            });
            $('#tHead select[id^=select]').each(function () {
                tHead[$(this).attr("id").replace('select', '').toLowerCase()] = $(this).val();
            });
            $('#tHead textarea[id^=txt]').each(function () {
                tHead[$(this).attr("id").replace('txt', '').toLowerCase()] = $(this).val();
            });
            //tBody
            var tBody = [];
            var tBodytemp = {};
            $('#tBody tBody tr').each(function () {
                $(this).find('input[id^=txt]:not([id^=txtNoq_])').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('txt', '').toLowerCase()] = $(this).val();
                });
                $(this).find('input[id^=chk]').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('chk', '').toLowerCase()] = $(this).prop('checked');
                });
                $(this).find('select[id^=select]').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('select', '').toLowerCase()] = $(this).val();
                });
                $(this).find('textarea[id^=txt]').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('txt', '').toLowerCase()] = $(this).val();
                });
                for (var i = 0; i < mustBodyField.length; i++) {    //若一定要輸入的欄位為空則不寫入資料
                    if (tBodytemp[mustBodyField] == null || tBodytemp[mustBodyField] == "") {
                    }
                    else {
                        tBody.push(tBodytemp);
                    }
                }
                tBodytemp = {};
            });
            break;
        case 3:     //刪除
            //tHead
            var tHead = { Noa: $('#tHead input[id=txtNoa]').val() };
            //tBody
            var tBody = [];
            var tBodytemp = {};
            $('#tBody tBody tr').each(function () {
                $(this).find('input[id^=txtNoq]').each(function () {
                    tBodytemp[getID($(this).attr("id"), 0).replace('txt', '').toLowerCase()] = $(this).val();
                });
                tBody.push(tBodytemp);
            });
            break;
    }
    $.ajax({
        'type': 'post',
        'url': '/Home/sqlSave',
        //Home代表HomeController
        data: { DB: DB, status: s_status, tHead: JSON.stringify(tHead), tBody: JSON.stringify(tBody) },
        traditional: true,
        async: true,
        success: function () {
            btnCan();
            refresh();
        },
        error: function (message) {
            alert('error!');
        }
    });

}

