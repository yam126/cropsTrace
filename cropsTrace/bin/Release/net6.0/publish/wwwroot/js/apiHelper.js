var queryPageParameter = {
    where: '',
    startTime: '',
    endTime:'',
    pageIndex: "",
    pageSize: "",
    sortField: '',
    sortMethod: ''
};
var queryParam = {
    where: '',
    sortField: '',
    sortMethod: ''
};
var queryShowFieldsParam = {
    CompanyId:'',
    where : "",
    condition : "",
    pageIndex : "",
    pageSize : "",
    sortField : "",
    sortMethod : ""
};
var queryGrowthInfoParam = {
    CompanyId: '',
    where: '',
    startTime: '',
    endTime: '',
    pageIndex: "",
    pageSize: "",
    sortField: '',
    sortMethod: ''
};
var saveShowFieldsParam = {
    companyId: "",
    recordIds: [],
    unSelectedRecordIds: [],
    isShow: ""
};
var getHomeParam = {
    companyId: "",
    year: "",
    cropsId: "",
    growthName: "",
    where: ""
};
var MobileResultParameters = {
    companyId:"",
    year:"",
    cropsId:"",
    pumpHouseID: ""
};
var ShowFieldsParameter = {
    recordId: "",
    companyId: "",
    device: "",
    pointId: "",
    fieldName: "",
    showFieldName: "",
    unit: "",
    isShow: "",
    id: "",
    value: "",
    updateTime: "",
    deviceCode: "",
    deviceName: "",
    name: ""
};
//api帮助
var apiHelper = {
    //urlBase: 'http://192.168.1.23:5329/api',//后台api地址
    //urlBase: 'http://localhost:5329/api',//后台api地址
    //urlBase:'http://192.168.3.97:5329/api',
    urlBase:'http://8.142.169.233:6001/api/crops-trace/v1/api',
    urlBaseFram: 'http://8.142.169.233:6001/api/farm/v1',
    urlBaseLiot:'http://8.142.169.233:5001/api/v1',
    response: null,
    token: "",
    level:-1,
    getCookie(cookieName) {
        var cookieString = document.cookie;
        var start = cookieString.indexOf(cookieName + '=');
        // 加上等号的原因是避免在某些 Cookie 的值里有
        // 与 cookieName 一样的字符串。
        if (start == -1) // 找不到
            return null;
        start += cookieName.length + 1;
        var end = cookieString.indexOf(';', start);
        if (end == -1)
            return unescape(cookieString.substring(start));
        return unescape(cookieString.substring(start, end));
    },
    checkLevel: function ()
    {
        var result = false;
        if (apiHelper.level > 3 || apiHelper.level <= -1) {
            if (apiHelper.level <= -1) {
                alert("获取不到用户等级");
                result = false;
            }
            else if (apiHelper.level > 3) {
                alert("用户权限不足");
                result = false;
            } else {
                result = true;
            }
        } else {
            result = true;
        }
        return result;
    },
    getQueryString: function (queryParam) {
        var queryStr = "";
        for (var paramName in queryParam) {
            if (queryParam[paramName] != "")
                queryStr += paramName + "=" + queryParam[paramName] + "&";
        }
        if (queryStr != "")
            queryStr = queryStr.substring(0, queryStr.length - 1);
        return queryStr;
    },
    addSeedInfo: function (apiData,callback)
    {
        var vurl = this.urlBase + "/seed-info/";
        if (!this.checkLevel())
            return false;
        $.ajax({
            type: "POST",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            dataType: "json",
            data: JSON.stringify(apiData),
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("保存失败" + response.data.msg);
                else
                    alert("保存成功");
                callback(response);
            }
        });
    },
    addGrowthInfo: function (apiData,loadingfunction,callback)
    {
        var vurl = this.urlBase + "/growth-info/";
        console.log(vurl);
        $.ajax({
            type: "POST",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            dataType: "json",
            data: JSON.stringify(apiData),
            beforeSend: function (xhr)
            {
                if (loadingfunction != null)
                    loadingfunction();
            },
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("保存失败" + response.data.msg);
                else
                    alert("保存成功");
                callback(response);
            }
        });
    },
    editSeedInfo: function (apiData, callback) {
        var vurl = this.urlBase + "/seed-info/" + apiData.cropsId;
        $.ajax({
            type: "PUT",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            dataType: "json",
            data: JSON.stringify(apiData),
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("保存失败" + response.data.msg);
                else
                    alert("保存成功");
                callback(response);
            }
        });
    },
    editGrowthInfo: function (apiData,loadingfunction,callback) {
        var vurl = this.urlBase + "/growth-info/" + apiData.recordId;
        console.log(vurl);
        $.ajax({
            type: "PUT",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            dataType: "json",
            data: JSON.stringify(apiData),
            beforeSend: function ()
            {
                if (loadingfunction != null)
                    loadingfunction();
            },
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("保存失败" + response.data.msg);
                else
                    alert("保存成功");
                callback(response);
            }
        });
    },
    deleteSeedInfo: function (IDStr,callback)
    {
        var vurl = this.urlBase + "/seed-info/" + IDStr;
        $.ajax({
            type: "DELETE",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            //dataType: "json",
            //data: JSON.stringify(apiData),
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("删除失败" + response.data.msg);
                else
                    alert("删除成功");
                callback(response);
            }
        });
    },
    deleteGrowthInfo: function (IDStr, callback) {
        var vurl = this.urlBase + "/growth-info/" + IDStr;
        $.ajax({
            type: "DELETE",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            //dataType: "json",
            //data: JSON.stringify(IDAry),
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("删除失败" + response.data.msg);
                else
                    alert("删除成功");
                callback(response);
            }
        });
    },
    querySeedInfoPage: function (queryParam, callback) {
        var vurl = this.urlBase + "/seed-info/page?" + this.getQueryString(queryParam);
        console.log(vurl);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    queryShowFieldsPage: function (queryShowFieldsParam, callback) {
        var vurl = this.urlBase + "/show-field/page?" + this.getQueryString(queryShowFieldsParam);
        console.log(vurl);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    setIsShowFields: function (saveShowFieldsParam,callback) {
        var vurl = this.urlBase + "/show-field/set-is-show-fields";
        console.log(vurl);
        $.ajax({
            type: "PUT",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            dataType: "json",
            data: JSON.stringify(saveShowFieldsParam),
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("保存失败，原因[" + response.data.msg+"]");
                else
                    callback(response);
            }
        });
    },
    queryGrowthInfoPage: function (queryGrowthInfoParam, callback)
    {
        var vurl = this.urlBase + "/growth-info/page?" + this.getQueryString(queryGrowthInfoParam);
        console.log(vurl);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    queryPumpHouseInfoPage: function (queryGrowthInfoParam, callback) {
        var vurl = this.urlBase + "/pump-house/page?" + this.getQueryString(queryGrowthInfoParam);
        console.log("queryPumpHouseInfoPage");
        console.log(vurl);
        console.log(queryGrowthInfoParam);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    pumpHouseAutoComplate: function (CompanyId,searchKey,resultCount,callback)
    {
        var vurl = "";
        if (searchKey == '')
            vurl = this.urlBase + "/pump-house/auto-complate/" + resultCount + "?CompanyId=" + CompanyId;
        else
            vurl = this.urlBase + "/pump-house/auto-complate/" + resultCount + "?CompanyId=" + CompanyId + "&where=" + searchKey;
        console.log(vurl);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    seedInfoAutoComplate: function (CompanyId, searchKey, resultCount, callback)
    {
        var vurl = "";
        if (searchKey == '')
            vurl = this.urlBase + "/seed-info/auto-complate/" + resultCount + "?CompanyId=" + CompanyId;
        else
            vurl = this.urlBase + "/seed-info/auto-complate/" + resultCount + "?CompanyId=" + CompanyId + "&where=" + searchKey;
        console.log(vurl);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    queryFileInfoByGrowthId: function (growthId,callback)
    {
        var vurl = this.urlBase + "/file-info/by-growthinfo/" + growthId;
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    fileUpload: function (CompanyId,uploadFileControlId, callback)
    {
        var vurl = this.urlBase + "/file-info";
        console.log(vurl);
        var fileUpload = $("#" + uploadFileControlId).get(0);
        var file = fileUpload.files[0];
        var data = new FormData();
        data.append(file.name, file);
        data.append("CompanyId", CompanyId);
        $.ajax({
            type: "POST",
            url: vurl,
            //headers: {
            //    token: window.localStorage.getItem("token")
            //},
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {
                console.log(response);
                response.data = response;
                if (response.data.status != 0)
                    alert("保存失败" + response.data.msg);
                else
                    callback(response);
                //$uibModalInstance.close(e);
            },
            error: function () {
                //utils.showError("上传失败");
            },
            complete: function () {
                // utils.hideMask();
            }
        });
    },
    deleteFile: function (serverFileName, callback)
    {
        var vurl = this.urlBase + "/file-info/delete-file/" + serverFileName;
        $.ajax({
            type: "DELETE",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            //dataType: "json",
            //data: JSON.stringify(IDAry),
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("删除失败" + response.data.msg);
                else
                    alert("删除成功");
                callback(response);
            }
        });
    },
    batchSaveFileInfo: function (saveFileParam, callback)
    {
        var vurl = this.urlBase + "/file-info/batch-save-file/";
        console.log(vurl);
        console.log("saveFileParam");
        console.log(saveFileParam);
        console.log(JSON.stringify(saveFileParam));
        var data = new FormData();
        data.append("data", JSON.stringify(saveFileParam));
        $.ajax({
            type: "POST",
            url: vurl,
            async: false,
            jsonp: "",
            //contentType: "application/json",//设置请求参数类型为json字符串
            //dataType: "json",
            //data: JSON.stringify(saveFileParam),
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("保存文件失败" + response.data.msg);
                else
                    alert("保存文件成功");
                callback(response);
            }
        });
    },
    queryAllSeedInfoByCompanyId: function (companyId, Year, callback)
    {
        var vurl = this.urlBase + "/seed-info/" + companyId + "/" + Year;
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    getGrowthInfoAllYear: function (companyId,callback)
    {
        var vurl = this.urlBase + "/growth-info/allYear/" + companyId;
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    getHome: function (
        getHomeParam,
        loadingfunction,
        callback
    )
    {
        var vurl = this.urlBase + "/home";
        console.log(getHomeParam);
        console.log(vurl);
        $.ajax({
            type: "POST",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            dataType: "json",
            data: JSON.stringify(getHomeParam),
            beforeSend: function (xhr) {
                console.log("加载中...");
                if (loadingfunction != null)
                    loadingfunction();
            },
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                callback(response);
            }
        });
    },
    //农事接口生长周期期查询
    QueryGrowCycles: function (cropsId, callback) {
        var vurl = this.urlBaseFram + "/grow-cycles";
        var where = '';
        switch (cropsId) {
            case "1555469107414044672":
                where = 'type=0';
                break;
            case "1556457754158305280":
                where = 'type=1';
                break;
            case "1556461551165247488":
                where = 'type=2';
                break;
        }
        console.log(where);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "Authorization",
                "Authorization": 'Bearer ' + this.token
            },
            data: {
                where: where
            },
            success: function (response) {
                console.log("QueryGrowCycles");
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("调用接口出错,原因:\t\r\n" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    //农事接口生长周期数据查询(growId:生长周期)返回株高胸径等
    InterfaceGrowCycles: function (growId, callback) {
        var vurl = this.urlBaseFram + "/grow-datas";
        var where = 'growId=' + growId;
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "Authorization",
                "Authorization": 'Bearer ' + this.token
            },
            data: {
                where: where
            },
            success: function (response) {
                console.log("InterfaceGrowCycles");
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("调用接口出错,原因:\t\r\n" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    InterfaceGetPoints: function (searchKey, callback)
    {
        var vurl = this.urlBaseLiot + "/points?key=" + searchKey;
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            headers: {
                "Access-Control-Allow-Origin": "*",
                "Access-Control-Allow-Headers": "Authorization",
                "Authorization": 'Bearer ' + this.token
            },
            success: function (response) {
                console.log("InterfaceGetPoints");
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("调用接口出错,原因:\t\r\n" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    InitMobilePage: function (parm, callback)
    {
        var vurl = this.urlBase + "/mobile/mobilePageData?companyId=" + parm.companyId + '&year=' + parm.year + '&cropsId=' + parm.cropsId + '&pumpHouseID=' + parm.pumpHouseID;
        console.log("MobileResultParameters");
        console.log(parm);
        console.log(vurl);
        $.ajax({
            type: "GET",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            //dataType: "json",
            //data: parm,
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("读取失败" + response.data.msg);
                else
                    callback(response);
            }
        });
    },
    addShowFields(addData, callback)
    {
        var vurl = this.urlBase + "/show-field";
        console.log("addShowFields");
        console.log(addData);
        console.log(vurl);
        $.ajax({
            type: "POST",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            dataType: "json",
            data: JSON.stringify(addData),
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("添加失败,原因[" + response.data.msg+"]");
                else
                    callback(response);
            }
        });
    },
    deleteShowFields(recordIds, callback) {
        var vurl = this.urlBase + "/show-field/" + recordIds;
        console.log("deleteShowFields");
        console.log(vurl);
        $.ajax({
            type: "DELETE",
            url: vurl,
            async: false,
            jsonp: "",
            contentType: "application/json",//设置请求参数类型为json字符串
            success: function (response) {
                console.log(response);
                response.data = response;
                console.log(response);
                if (response.data.status != 0)
                    alert("删除失败,原因[" + response.data.msg + "]");
                else
                    callback(response);
            }
        });
    }
};