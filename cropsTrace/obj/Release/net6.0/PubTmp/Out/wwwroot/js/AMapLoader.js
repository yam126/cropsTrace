window._AMapSecurityConfig = {
    securityJsCode: 'acca991ccaf8f2bb97df91a7bf547a24', //高德Api密钥
};
var map;
var amap;
var rootDistrictName = "晋中市";
var districtNameDatas = new Array();//下级行政区列表
var polygons = null;//高德地块儿点位
var district = null;
var markers = [];
var markerIcon = ["http://192.168.3.82:5000/images/icon/corn_small.png", "http://192.168.3.82:5000/images/icon/soybean_small.png", "http://192.168.3.82:5000/images/icon/wheat_small.png"];
var crops = ["玉米","大豆","小麦"];
var GlobalBoundaries = [];
var defaultCenter = ['111.9490384003906', '38.75884550512727'];
var drawLocation = [
    {
        id: 10,
        name: '寿阳-尹灵芝',
        LngLat: ['113.301552', '37.935143']
    },
    {
        id: 7,
        name: '内蒙古-磴口',
        LngLat: ['107.00864', '40.33056']
    },
    {
        id: 11,
        name: '寿阳-景尚',
        LngLat: ['113.18932347341921', '37.74771399639911']
    }
];
function CreateAMap(htmlElementId, districtName, polygon_event) {
    AMapLoader.load({
        "key": "af03b4b1406996840b39d11d81850677",              // 申请好的Web端开发者Key，首次调用 load 时必填
        "version": "2.0",   // 指定要加载的 JSAPI 的版本，缺省时默认为 1.4.15
        "plugins": [
            "AMap.Autocomplete",
            "AMap.PlaceSearch",
            "AMap.Scale",
            "AMap.OverView",
            "AMap.ToolBar",
            "AMap.MapType",
            "AMap.PolyEditor",
            "AMap.CircleEditor",
            "AMap.DistrictSearch",
            "AMap.Geocoder"
        ],           // 需要使用的的插件列表，如比例尺'AMap.Scale'等
        "AMapUI": {             // 是否加载 AMapUI，缺省不加载
            "version": '1.1',   // AMapUI 版本
            "plugins": ['overlay/SimpleMarker'],       // 需要加载的 AMapUI ui插件
        },
        "Loca": {                // 是否加载 Loca， 缺省不加载
            "version": '2.0'  // Loca 版本
        },
    }).then((AMap) => {
        map = new AMap.Map(htmlElementId);
        var opts = {
            level: "biz_area",
            extensions: "all",
            showbiz: true,
            subdistrict: 4
        };
        district = new AMap.DistrictSearch(opts);
        map.setMapStyle("amap://styles/blue");
        getChildDistrictRegion(polygon_event);
    }).catch((e) => {
        console.error(e);  //加载错误提示
    });
}

/**
 * 初始化高德地图
 * @param {any} htmlElementId html容器
 * @param {any} callback 回调函数
 */
function CreateAMapWeather(htmlElementId,callback) {
    AMapLoader.load({
        "key": "af03b4b1406996840b39d11d81850677",              // 申请好的Web端开发者Key，首次调用 load 时必填
        "version": "2.0",   // 指定要加载的 JSAPI 的版本，缺省时默认为 1.4.15
        "plugins": [
            "AMap.Autocomplete",
            "AMap.PlaceSearch",
            "AMap.Scale",
            "AMap.OverView",
            "AMap.ToolBar",
            "AMap.MapType",
            "AMap.PolyEditor",
            "AMap.CircleEditor",
            "AMap.DistrictSearch",
            "AMap.Geocoder"
        ],           // 需要使用的的插件列表，如比例尺'AMap.Scale'等
        "AMapUI": {             // 是否加载 AMapUI，缺省不加载
            "version": '1.1',   // AMapUI 版本
            "plugins": ['overlay/SimpleMarker'],       // 需要加载的 AMapUI ui插件
        },
        "Loca": {                // 是否加载 Loca， 缺省不加载
            "version": '2.0'  // Loca 版本
        },
    }).then((AMap) => {      
        var opts = {
            //level: "biz_area",
            //extensions: "all",
            //showbiz: true,
            //subdistrict: 4,
            resizeEnable:true,
            center: defaultCenter
        };
        map = new AMap.Map(htmlElementId, opts);
        callback(AMap);
        map.setMapStyle("amap://styles/blue");

    }).catch((e) => {
        console.error(e);  //加载错误提示
    });
}
function resizeMapCenter(screenWidth) {
    console.log("resizeMapCenter");
    console.log(screenWidth);
    if (screenWidth <= 1670) {
        map.setCenter(['111.8172024628906', '38.69885127745263']);
        map.setZoom(7);
        if (screenWidth <= 1650) {
            map.setCenter(['111.2898587128906', '38.74170943821583']);
            map.setZoom(6.5);
        }
    }
    else
        map.setZoom(7);
}
//画位置
/**
 * @param {any} currentLocationName 当前位置名称
 * @param {any} AMap 高德地图object
 * @param {any} clickEvent 位置点击事件
 * @param {any} level 预警级别
 */
function drawLocationWeather(currentLocationName, AMap,clickEvent) {
    for (var i = 0; i < drawLocation.length; i++) {
        var contentHtml = "<div class='AMapLocation' title='" + drawLocation[i].name + "' >";
        var localName = 'local33x55blue.png';
        apiHelper.alertWeather(
            drawLocation[i].id,
            function (response) {
                console.log("drawLocationWeather");
                console.log(drawLocation[i].id);
                console.log(drawLocation[i].name);
                console.log(response.data.result.level);
                switch (response.data.result.level) {
                    case "蓝色":
                        localName = 'local33x55blue.png';
                        break;
                    case "黄色":
                        localName = 'local33x55yellow.png';
                        break;
                    case "橙色":
                        localName = 'local33x55orange.png';
                        break;
                    case "红色":
                        localName = 'local33x55red.png';
                        break;
                }
                contentHtml += "<img id='AMapLocationIcon" + drawLocation[i].id + "' src='/images/newhome/" + localName + "' style='margin-left:48%;width:23px;height:23px;cursor:pointer;' />";
                contentHtml += "<div class='locationName' >智农现代农业示范园<br/>(" + drawLocation[i].name + ")</div>";
                contentHtml += "</div>";
                var marker = new AMap.Marker({
                    map: map,
                    position: new AMap.LngLat(drawLocation[i].LngLat[0], drawLocation[i].LngLat[1]),
                    content: contentHtml,
                    zIndex: 200 + i,
                    extData: drawLocation[i]
                    //offset: new AMap.Pixel(left, top)
                });
                AMap.Event.addListener(
                    marker,
                    "click",
                    function (e) {
                        var clickLocation = e.target.getExtData();
                        clickEvent(clickLocation.name, clickLocation.id);
                    }
                );
                map.setFitView();
                map.setCenter(defaultCenter);
            });
    }
}
function initAllChildDistrict() {
    AMapLoader.load({
        "key": "af03b4b1406996840b39d11d81850677",              // 申请好的Web端开发者Key，首次调用 load 时必填
        "version": "2.0",   // 指定要加载的 JSAPI 的版本，缺省时默认为 1.4.15
        "plugins": [
            "AMap.Autocomplete",
            "AMap.PlaceSearch",
            "AMap.Scale",
            "AMap.OverView",
            "AMap.ToolBar",
            "AMap.MapType",
            "AMap.PolyEditor",
            "AMap.CircleEditor",
            "AMap.DistrictSearch",
            "AMap.Geocoder"
        ],           // 需要使用的的插件列表，如比例尺'AMap.Scale'等
        "AMapUI": {             // 是否加载 AMapUI，缺省不加载
            "version": '1.1',   // AMapUI 版本
            "plugins": ['overlay/SimpleMarker'],       // 需要加载的 AMapUI ui插件
        },
        "Loca": {                // 是否加载 Loca， 缺省不加载
            "version": '2.0'  // Loca 版本
        },
    }).then((AMap) => {
        map = new AMap.Map(htmlElementId);
        var opts = {
            level: "biz_area",
            extensions: "all",
            showbiz: true,
            subdistrict: 4
        };
        district = new AMap.DistrictSearch(opts);
        map.setMapStyle("amap://styles/blue");
        getChildDistrictAll(rootDistrictName, AMap);
    }).catch((e) => {
        console.error(e);  //加载错误提示
    });
}
function getGlobalRegions(DistrictId) {
    var result = null;
    for (var i = 0; i < globalAllRegions.length; i++) {
        if (globalAllRegions[i].id == DistrictId)
            result = globalAllRegions[i];
    }
    return result;
}
var tempRegionItem = null;
//地图划分行政区
function mapDivideDistrict(polygon_event) {
    if (polygons != null)
        map.remove(polygons);
    GlobalBoundaries = [];
    for (var i = 0; i < districtNameDatas.length; i++) {
        tempRegionItem = districtNameDatas[i];
        console.log(i);
        console.log(tempRegionItem.name);
        district.search(tempRegionItem.name, function (status, result) {
            if (status == 'complete') {
                polygons = [];
                console.log("mapDivideDistrict");
                console.log(tempRegionItem);
                for (var j = 0; j < districtNameDatas.length; j++) {
                    if (result.districtList[0].name == districtNameDatas[j].name)
                        tempRegionItem = districtNameDatas[j];
                }
                console.log(tempRegionItem);
                var bounds = result.districtList[0].boundaries;
                if (bounds) {
                    for (var i = 0, l = bounds.length; i < l; i++) {
                        //生成行政区划polygon
                        if (bounds[i] != null && bounds[i] != undefined) {
                            var polygon = new AMap.Polygon({
                                map: map,
                                strokWeight: 1,
                                path: bounds[i],
                                fillOpacity: 0.4,
                                fillColor: "#CCF3FF",
                                strokeColor: "#CC66CC",
                                cursor: "pointer",
                                bubble: true,
                                extData: tempRegionItem
                            });
                            //行政区点击事件
                            polygon.on("mouseover", function (e) {
                                e.target._opts.fillColor = "#FF4D4D";
                                e.target.setOptions(e.target._opts);
                                polygon_event("mouseover", e.target._opts.extData);
                            });
                            polygon.on("mouseout", function (e) {
                                e.target._opts.fillColor = "#CCF3FF";
                                e.target.setOptions(e.target._opts);
                                polygon_event("mouseout", e.target._opts.extData);
                            });
                            GlobalBoundaries.push(bounds[i]);
                        }
                        polygons.push(polygon);
                    }
                    map.setFitView();
                }
            }
        });
    }
}
function getChildDistrictAll(districtName, AMap) {
    districtNameDatas = [];
    district.search(districtName, function (status, result) {
        if (status == 'complete') {
            if (districtNameDatas == null || districtNameDatas.length <= 0) {
                for (var i = 0; i < result.districtList[0].districtList[0].districtList.length; i++) {
                    var item = result.districtList[0].districtList[0].districtList[i];
                    districtNameDatas.push(item.name);
                }
            }
        }
    });
}

//随机标记地图
function mapRandomSign(typeInfo)
{
    map.clearMap();


    //var labelMarker = new AMap.LabelMarker({
    //    name: "京味斋烤鸭店",
    //    position: [115.898359, 39.909869],
    //    icon: {
    //        type: "image",
    //        image: "https://a.amap.com/jsapi_demos/static/images/poi-marker.png",
    //        clipOrigin: [547, 92],
    //        clipSize: [50, 68],
    //        size: [25, 34],
    //        anchor: "bottom-center",
    //        angel: 0,
    //        retina: true,
    //    },
    //    text: {
    //        content: "京味斋烤鸭店",
    //        direction: "top",
    //        offset: [0, 0],
    //        style: {
    //            fontSize: 13,
    //            fontWeight: "normal",
    //            fillColor: "#fff",
    //            padding: "2, 5",
    //            backgroundColor: "#22884f",
    //        },
    //    },
    //});

    //layer.add(labelMarker);
    for (var i = 0; i < GlobalBoundaries.length; i++)
    {
        var cropNameIndex = getRandom(0, crops.length-1);
        var cropName = crops[cropNameIndex];
        var iconPath = "";
        var tipsText = "";
        switch (cropName)
        {
            case "玉米":
                iconPath = markerIcon[0];
                break;
            case "大豆":
                iconPath = markerIcon[1];
                break;
            case "小麦":
                iconPath = markerIcon[2];
                break;
        }
        console.log(iconPath);
        switch (typeInfo)
        {
            case "种植面积":
                tipsText = cropName+typeInfo+"亩";
                break;
            default:
                tipsText = typeInfo;
                break;
        }
        var layer = new AMap.LabelsLayer({
            zooms: [3, 20],
            zIndex: 1000+i,
            // 开启标注避让，默认为开启，v1.4.15 新增属性
            collision: true,
            // 开启标注淡入动画，默认为开启，v1.4.15 新增属性
            animation: true,
        });
        map.add(layer);
        var labelMarker = new AMap.LabelMarker({
            name: tipsText,
            position: [GlobalBoundaries[i][0].lng, GlobalBoundaries[i][0].lat],
            icon: {
                type: "image",
                image: iconPath,
                clipOrigin: [547, 92],
                clipSize: [50, 68],
                size: [25, 34],
                anchor: "bottom-center",
                angel: 0,
                retina: true,
            },
            text: {
                content: tipsText + getRandom(1,100),
                direction: "top",
                offset: [0, 0],
                style: {
                    fontSize: 13,
                    fontWeight: "normal",
                    fillColor: "#fff",
                    padding: "2, 5",
                    backgroundColor: "#22884f",
                },
            },
        });        
        markers.push(labelMarker);
    }
    layer.add(markers);
    map.setFitView();
}

function getChildDistrictRegionById(regionId) {
    var result = null;
    for (var i = 0; i < globalAllRegions.length; i++) {
        if (globalAllRegions[i].id == regionId)
            result=globalAllRegions[i];
    }
    return result;
}

function getChildDistrictRegion(polygon_event) {
    districtNameDatas = [];
    for (var i = 0; i < globalAllRegions.length; i++) {
        if (globalAllRegions[i].parent_id == rootDistrictId)
            districtNameDatas.push(globalAllRegions[i]);
    }
    if (districtNameDatas == null || districtNameDatas.length <= 0)
        districtNameDatas.push(getChildDistrictRegionById(rootDistrictId));
    mapDivideDistrict(polygon_event);
}

//获得下级行政区
function getChildDistrict(districtName, AMap, polygon_event) {
    districtNameDatas = [];
    district.search(districtName, function (status, result) {
        console.log("districtName=" + districtName);
        if (status == 'complete') {
            if (districtNameDatas == null || districtNameDatas.length <= 0) {
                if (result.districtList[0].level != "district") {
                    for (var i = 0; i < result.districtList[0].districtList[0].districtList.length; i++) {
                        var item = result.districtList[0].districtList[0].districtList[i];
                        districtNameDatas.push(item.name);
                    }
                }
                else {
                    var item = result.districtList[0];
                    districtNameDatas.push(item.name);
                }
                mapDivideDistrict(polygon_event);
            }
        }
    });
}