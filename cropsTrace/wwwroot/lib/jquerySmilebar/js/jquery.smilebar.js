"use strict";
(function ($) {
    /**
     * Smilebar:自定义滚动条
     * Ele:指向实例的this
     * config:配置
     * update(position):当滚动盒子固定宽高发生改变时调用  position 移动的坐标对象(默认为initPosition)
     * *一个页面内出现多个带有键盘方向控制效果的自定义滚动条时会有小bug
     */
    /**
     * 限制范围  <number | string>
     * @param value 值
     * @param min 最小范围
     * @param max 最大范围
     * @param unit 单位         T为string时,带unit;为number时,不带unit
     */
    function limits(value, min, max, unit) {
        // 字符串
        if (typeof value === "string" &&
            typeof min === "string" &&
            typeof max === "string" &&
            unit) {
            var Value = Number(value.split(unit)[0]);
            var Min = Number(min.split(unit)[0]);
            var Max = Number(max.split(unit)[0]);
            var res = Value < Min ? Min : Value > Max ? Max : Value;
            return String(res) + unit;
        }
        // 数值
        if (typeof value === "number") {
            return value < min ? min : value > max ? max : value;
        }
    }
    /**
     * 自定义滚动条
     * @param Ele    实例this元素对象
     * @param config 配置
     */
    function Smilebar(Ele, config) {
        // @ts-ignore  this
        var _this = this;
        // 默认配置
        _this.config = {
            type: "xy",
            size: "12px",
            color: "#252a35",
            silderSize: "60%",
            silderColor: "#404550",
            clickBln: true,
            keyBln: true,
            keySpeed: 1,
            wheelBln: true,
            wheelSpeed: 1,
            handBln: false,
            handSpeed: 30,
            initPosition: { x: 0, y: 0 },
        };
        // 合并配置
        _this.config = $.extend({}, _this.config, config);
        // 限制滑块范围[20%,100%]
        _this.config.silderSize = limits(_this.config.silderSize, "20%", "100%", "%");
        // 初始化
        _this.init(Ele);
        // 检测滚动条
        _this.checkScrollbar();
        // 监听盒子内容
        _this.monitorContent();
        // 初始移动位置
        _this.moving(_this.config.initPosition);
        // 绑定事件
        _this.bind();
    }
    /**
     * 初始化
     * @param Ele 滚动盒子元素对象
     */
    Smilebar.prototype.init = function (Ele) {
        // @ts-ignore  this
        var _this = this;
        //#region 初始值
        // 滚动盒子元素
        _this.container = null;
        // 内容盒子元素
        _this.content = null;
        // y轴滑块元素
        _this.sliderY = null;
        // y轴滚动条元素
        _this.scrollbarY = null;
        // x轴滑块元素
        _this.sliderX = null;
        // x轴滚动条元素
        _this.scrollbarX = null;
        // xy轴交叉框
        _this.cross = null;
        // 所有滑块元素
        _this.sliders = null;
        // 滚动盒子宽
        _this.containerWidth = 0;
        // 滚动盒子高
        _this.containerHeight = 0;
        // 内容盒子宽
        _this.contentWidth = 0;
        // 内容盒子高
        _this.contentHeight = 0;
        // y轴滚动条的长度
        _this.trackY = 0;
        // y轴滑块长度
        _this.lenY = 0;
        // y轴滑块可移动top值
        _this.moveTop = 0;
        // y轴滚动盒子与内容盒子的差距(若作为内容可移动top值,需加上y轴滚动条宽度)
        _this.diffY = 0;
        // x轴滚动条的长度
        _this.trackX = 0;
        // X轴滑块长度
        _this.lenX = 0;
        // x轴滑块可移动left值
        _this.moveLeft = 0;
        // x轴滚动盒子与内容盒子的差距(若作为内容可移动left值,需加上x轴滚动条高度)
        _this.diffX = 0;
        //#endregion
        // 生成自定义滚动条元素对象
        _this.createScrollEle(Ele);
        switch (_this.config.type) {
            // x轴
            case "x":
                _this.container.prepend(_this.scrollbarX);
                break;
            // y轴
            case "y":
                _this.container.prepend(_this.scrollbarY);
                break;
            // xy轴
            default:
                _this.container.prepend(_this.scrollbarX, _this.scrollbarY, _this.cross);
                break;
        }
        // 点击鼠标样式
        if (_this.config.clickBln || _this.container.attr("data-click") == "true") {
            _this.scrollbars.css({
                cursor: "pointer",
            });
        }
        else {
            _this.scrollbars.css({
                cursor: "",
            });
        }
        // 抓手鼠标样式
        if (_this.config.handBln || _this.container.attr("data-hand") == "true") {
            _this.container.css({
                cursor: "url('smilebarImg/grasp.png'),auto",
            });
        }
        else {
            _this.container.css({
                cursor: "",
            });
        }
        // 是否开启键盘方向键
        if (_this.config.keyBln || _this.container.attr("data-key") == "true") {
            Smilebar.prototype.keyTotal += 1;
        }
    };
    /**
     * 绑定触发事件
     */
    Smilebar.prototype.bind = function () {
        var doc = $(window.document);
        // @ts-ignore  this
        var _this = this;
        /**
         * 滚动条点击触发
         * @param event 事件对象
         */
        _this.bindEvent(_this.scrollbars, "click", function (event) {
            var e = event || window.event;
            // 检测点击的元素是否为滑块
            if (e.target === _this.sliderX[0])
                return;
            if (e.target === _this.sliderY[0])
                return;
            // 检测是否开启功能
            if (!_this.config.clickBln &&
                !(_this.container.attr("data-click") == "true"))
                return;
            // 判断点击为x轴还是y轴
            _this.clicked = e.target === _this.scrollbarX[0] ? "x" : "y";
            // 当前坐标
            var X = e.pageX; //x轴坐标
            var Y = e.pageY; //y轴坐标
            var Top = _this.scrollbarY.offset().top; //y轴到顶部距离
            var Left = _this.scrollbarX.offset().left; //x轴到左侧距离
            switch (_this.clicked) {
                // x轴
                case "x":
                    var X_diff = X - Left;
                    // 计算top值
                    var X_left = (X_diff / _this.trackX) * _this.moveLeft;
                    // 移动x轴滑块与内容
                    _this.moveScorollbar(X_left, _this.clicked);
                    break;
                // y轴
                case "y":
                    var Y_diff = Y - Top;
                    // 计算top值
                    var Y_top = (Y_diff / _this.trackY) * _this.moveTop;
                    // 移动y轴滑块与内容
                    _this.moveScorollbar(Y_top, _this.clicked);
                    break;
                default:
                    break;
            }
        });
        /**
         * 滑块移动事件
         * @param event 事件对象
         */
        _this.bindEvent(_this.sliders, "mousedown", function (event) {
            // @ts-ignore  this
            var _this = this;
            var e = event || window.event;
            // 判断按下的滑块为x轴还是y轴
            _this.pressed = e.target === _this.sliderX[0] ? "x" : "y";
            // 当前坐标
            var X = e.pageX; //x轴坐标
            var Y = e.pageY; //y轴坐标
            var Top = _this.sliderY.position().top; //顶部距离
            var Left = _this.sliderX.position().left; //左边距离
            _this.bindEvent(doc, "mousemove", function (event) {
                var e = event || window.event;
                switch (_this.pressed) {
                    // x轴
                    case "x":
                        // 计算滑块移动
                        var X_diff = e.pageX - X;
                        var X_left = Left + X_diff;
                        // 移动x轴滑块与内容
                        _this.moveScorollbar(X_left, _this.pressed);
                        break;
                    // y轴
                    case "y":
                        // 计算滑块移动
                        var Y_diff = e.pageY - Y;
                        var Y_top = Top + Y_diff;
                        // 移动y轴滑块与内容
                        _this.moveScorollbar(Y_top, _this.pressed);
                        break;
                    default:
                        break;
                }
            });
            _this.bindEvent(doc, "selectstart", function (event) {
                var e = event || window.event;
                e.preventDefault();
            });
        });
        /**
         * 内容抓手事件
         * @param event 事件对象
         */
        _this.bindEvent(_this.container, "mousedown", function (event) {
            var e = event || window.event;
            // 判断是否开启抓手功能
            if (!_this.config.handBln &&
                !(_this.container.attr("data-hand") == "true"))
                return;
            // 检测按下的元素是否为滑块
            if (e.target === _this.sliderX[0])
                return;
            if (e.target === _this.sliderY[0])
                return;
            _this.container.css({
                cursor: "url('smilebarImg/grasp_active.png'),auto",
            });
            // 当前坐标
            var X = e.pageX; //x轴坐标
            var Y = e.pageY; //y轴坐标
            var Top = _this.sliderY.position().top; //顶部距离
            var Left = _this.sliderX.position().left; //左边距离
            _this.bindEvent(doc, "mousemove", function (event) {
                var e = event || window.event;
                var X_diff = -(e.pageX - X);
                var Y_diff = -(e.pageY - Y);
                // 计算y轴
                var addY = (Y_diff * _this.config.handSpeed) / 100;
                var Y_top = Top + addY;
                // 计算x轴
                var addX = (X_diff * _this.config.handSpeed) / 100;
                var X_left = Left + addX;
                // 移动滑块与内容
                switch (_this.config.type) {
                    // x轴
                    case "x":
                        _this.moveScorollbar(X_left, "x");
                        break;
                    // y轴
                    case "y":
                        _this.moveScorollbar(Y_top, "y");
                        break;
                    // xy轴
                    default:
                        _this.moveScorollbar(X_left, "x");
                        _this.moveScorollbar(Y_top, "y");
                        break;
                }
            });
            _this.bindEvent(doc, "selectstart", function (event) {
                var e = event || window.event;
                e.preventDefault();
            });
        });
        /**
         * 鼠标抬起
         */
        _this.bindEvent(doc, "mouseup", function () {
            _this.pressed = -1;
            if (_this.config.handBln || _this.container.attr("data-hand") == "true") {
                _this.container.css({
                    cursor: "url('smilebarImg/grasp.png'),auto",
                });
            }
            else {
                _this.container.css({
                    cursor: "",
                });
            }
            doc.unbind("mousemove");
            doc.unbind("selectstart");
        });
        // 只允许开启一个键盘方向键功能,当开启多个时,只有第一个会生效
        if (_this.keyTotal == 1) {
            /**
             * 键盘方向键
             * @param event 事件对象
             */
            _this.bindEvent(doc, "keydown", function (event) {
                var e = event || window.event;
                // 判断是否开启键盘功能
                if (!_this.config.keyBln &&
                    !(_this.container.attr("data-key") == "true"))
                    return;
                // 保存当前值
                var Top = _this.sliderY.position().top; //y轴到顶部距离
                var Left = _this.sliderX.position().left; //x轴到左侧距离
                switch (e.keyCode) {
                    // 上
                    case 38:
                        if (_this.config.type == "x")
                            return;
                        // 计算移动top距离
                        var moveY = (_this.trackY * _this.config.keySpeed) / 100;
                        var Y_top = Top - moveY;
                        // 移动y轴滑块与内容
                        _this.moveScorollbar(Y_top, "y");
                        break;
                    // 下
                    case 40:
                        if (_this.config.type == "x")
                            return;
                        // 计算移动top距离
                        var moveY = (_this.trackY * _this.config.keySpeed) / 100;
                        var Y_top = Top + moveY;
                        // 移动y轴滑块与内容
                        _this.moveScorollbar(Y_top, "y");
                        break;
                    // 左
                    case 37:
                        if (_this.config.type == "y")
                            return;
                        // 计算移动top距离
                        var moveX = (_this.trackX * _this.config.keySpeed) / 100;
                        var X_left = Left - moveX;
                        // 移动y轴滑块与内容
                        _this.moveScorollbar(X_left, "x");
                        break;
                    // 右
                    case 39:
                        if (_this.config.type == "y")
                            return;
                        // 计算移动top距离
                        var moveX = (_this.trackX * _this.config.keySpeed) / 100;
                        var X_left = Left + moveX;
                        // 移动y轴滑块与内容
                        _this.moveScorollbar(X_left, "x");
                        break;
                    default:
                        break;
                }
            });
        }
        /**
         * 鼠标中心滚轮
         * @param event 事件对象
         */
        _this.bindEvent(_this.container, "mousewheel DOMMouseScroll", function (event) {
            var e = event || window.event;
            e.stopPropagation();
            e.preventDefault();
            // 判断是否开启键盘功能
            if (!_this.config.wheelBln &&
                !(_this.container.attr("data-wheel") == "true"))
                return;
            // x轴无滚动事件
            if (_this.config.type === "x")
                return;
            // 保存当前值
            var Top = _this.sliderY.position().top; //y轴到顶部距离
            var wheel = e.originalEvent.wheelDelta || -e.originalEvent.detail;
            if (wheel < 0) {
                //向下滚动
                var moveY = (_this.trackY * _this.config.wheelSpeed) / 100;
                var Y_top = Top + moveY;
                _this.moveScorollbar(Y_top, "y");
            }
            else {
                //向上滚动
                var moveY = (_this.trackY * _this.config.wheelSpeed) / 100;
                var Y_top = Top - moveY;
                _this.moveScorollbar(Y_top, "y");
            }
        });
    };
    /**
     * 移动滑块与内容对应
     * @param move 滑块移动距离
     * @param type 移动轴类型  x轴:"x" y轴:"y"x轴:"x" y轴:"y"
     */
    Smilebar.prototype.moveScorollbar = function (move, type) {
        // @ts-ignore  this
        var _this = this;
        switch (_this.config.type) {
            // x轴
            case "x":
                // 只能移动x轴
                if (type === "y")
                    return;
                // 滑块移动
                var moving = limits(move, 0, _this.moveLeft);
                _this.sliderX.css({
                    left: moving + "px",
                });
                // 内容移动
                var content_diffX = _this.diffX;
                var content_X = Math.round((moving / _this.moveLeft) * content_diffX);
                _this.content.css({
                    left: "-" + content_X + "px",
                });
                break;
            // y轴
            case "y":
                // 只能移动y轴
                if (type === "x")
                    return;
                // 滑块移动
                var moving = limits(move, 0, _this.moveTop);
                _this.sliderY.css({
                    top: moving + "px",
                });
                // 内容移动
                var content_diffY = _this.diffY;
                var content_Y = Math.round((moving / _this.moveTop) * content_diffY);
                _this.content.css({
                    top: "-" + content_Y + "px",
                });
                break;
            // xy轴
            default:
                // x轴移动
                if (type === "x") {
                    // 滑块移动
                    var moving = limits(move, 0, _this.moveLeft);
                    _this.sliderX.css({
                        left: moving + "px",
                    });
                    // 内容移动
                    var content_diffX = _this.scrollbarY.css("display") === "none"
                        ? _this.diffX
                        : _this.diffX + _this.scrollbarY.width();
                    var content_X = Math.round((moving / _this.moveLeft) * content_diffX);
                    _this.content.css({
                        left: "-" + content_X + "px",
                    });
                }
                // y轴移动
                if (type === "y") {
                    // 滑块移动
                    var moving = limits(move, 0, _this.moveTop);
                    _this.sliderY.css({
                        top: moving + "px",
                    });
                    // 内容移动
                    var content_diffY = _this.scrollbarX.css("display") === "none"
                        ? _this.diffY
                        : _this.diffY + _this.scrollbarX.height();
                    var content_Y = Math.round((moving / _this.moveTop) * content_diffY);
                    _this.content.css({
                        top: "-" + content_Y + "px",
                    });
                }
                break;
        }
    };
    /**
     * 移动位置
     * @param position 移动的坐标对象
     */
    Smilebar.prototype.moving = function (position) {
        // @ts-ignore  this
        var _this = this;
        // x轴
        if (position.x != undefined) {
            var typeX = typeof position.x;
            if (typeX == "number") {
                _this.moveScorollbar(position.x, "x");
            }
            else if (typeX == "string") {
                // 保存x轴移动的位置
                var X_left;
                switch (position.x) {
                    case "first":
                        X_left = 0;
                        break;
                    case "center":
                        X_left = _this.percentToNumber("50%", "x");
                        break;
                    case "last":
                        X_left = _this.moveLeft;
                        break;
                    default:
                        X_left = _this.percentToNumber(position.x, "x");
                        break;
                }
                // x轴移动滑块与内容
                _this.moveScorollbar(X_left, "x");
            }
        }
        // y轴
        if (position.y != undefined) {
            var typeY = typeof position.y;
            if (typeY == "number") {
                // number型
                _this.moveScorollbar(position.y, "y");
            }
            else if (typeY == "string") {
                // 保存y轴移动的位置
                var Y_top;
                switch (position.y) {
                    case "first":
                        Y_top = 0;
                        break;
                    case "center":
                        Y_top = _this.percentToNumber("50%", "y");
                        break;
                    case "last":
                        Y_top = _this.moveTop;
                        break;
                    default:
                        Y_top = _this.percentToNumber(position.x, "y");
                        break;
                }
                // y轴移动滑块与内容
                _this.moveScorollbar(Y_top, "y");
            }
        }
    };
    /**
     * 生成滚动条元素对象
     * @param Ele  滚动盒子元素对象
     */
    Smilebar.prototype.createScrollEle = function (Ele) {
        // @ts-ignore  this
        var _this = this;
        // 滚动盒子
        _this.container = $(Ele).css({ position: "relative", overflow: "hidden" });
        _this.container.contents().wrapAll($("<div/>", {
            class: "smilebar-content",
        }));
        // 内容盒子
        _this.content = _this.container.find(".smilebar-content");
        // y轴滑块
        _this.sliderY = $("<div/>", {
            class: "smilebar-slider",
        }).css({
            width: _this.config.silderSize,
            background: _this.config.silderColor,
        });
        // y轴滚动条
        _this.scrollbarY = $("<div/>", {
            class: "smilebar-scrollbar smilebar-scrollbarY",
        }).css({
            width: _this.config.size,
            background: _this.config.color,
        });
        // x轴滑块
        _this.sliderX = $("<div/>", {
            class: "smilebar-slider",
        }).css({
            height: _this.config.silderSize,
            background: _this.config.silderColor,
        });
        // x轴滚动条
        _this.scrollbarX = $("<div/>", {
            class: "smilebar-scrollbar smilebar-scrollbarX",
        }).css({
            height: _this.config.size,
            background: _this.config.color,
        });
        // xy轴交叉框
        _this.cross = $("<div class='smilerbar-cross'></div>").css({
            width: _this.config.size,
            height: _this.config.size,
            background: _this.config.color,
        });
        // 组合滚动条模块
        _this.sliders = _this.sliderY.add(_this.sliderX);
        _this.scrollbars = _this.scrollbarY.add(_this.scrollbarX);
        _this.scrollbarY.append(_this.sliderY);
        _this.scrollbarX.append(_this.sliderX);
    };
    /**
     * 检测滚动条,计算:可滚动长度,滑块可滑动距离,滑块长度
     */
    Smilebar.prototype.checkScrollbar = function () {
        // @ts-ignore  this
        var _this = this;
        // 盒子宽高
        _this.containerWidth = _this.container.width();
        _this.containerHeight = _this.container.height();
        // 内容宽高
        _this.contentWidth = _this.content.width();
        _this.contentHeight = _this.content.height();
        // x轴内容盒子与滚动盒子的差距(若作为内容可移动left值,需加上x轴滚动条高度)
        _this.diffX = _this.content.width() - _this.container.width();
        // y轴内容盒子与滚动盒子的差距(若作为内容可移动top值,需加上y轴滚动条宽度)
        _this.diffY = _this.content.height() - _this.container.height();
        // 计算各种滚动条情况下的数据
        switch (_this.config.type) {
            // x轴
            case "x":
                _this.trackX = _this.containerWidth; //x轴滚动条长度
                // 是否出现滚动条
                if (_this.diffX > 0) {
                    _this.lenX = (_this.containerWidth / _this.contentWidth).toFixed(2); // X轴滑块长度
                    _this.sliderX.css({
                        width: _this.lenX * 100 + "%",
                    });
                    _this.scrollbarX.show();
                    _this.moveLeft = _this.trackX - _this.sliderX.width();
                }
                else {
                    _this.scrollbarX.hide();
                    _this.lenX = 0;
                    _this.moveLeft = 0;
                }
                break;
            // y轴
            case "y":
                _this.trackY = _this.containerHeight; // y轴滚动条长度
                // 是否出现滚动条
                if (_this.diffY > 0) {
                    _this.lenY = (_this.containerHeight / _this.contentHeight).toFixed(2); // y轴滑块长度
                    _this.sliderY.css({
                        height: _this.lenY * 100 + "%",
                    });
                    _this.scrollbarY.show();
                    _this.moveTop = _this.trackY - _this.sliderY.height();
                }
                else {
                    _this.scrollbarY.hide();
                    _this.lenY = 0;
                    _this.moveTop = 0;
                }
                break;
            // xy轴
            default:
                var crossW = _this.cross.width(); // xy轴交叉框宽
                var crossH = _this.cross.height(); // xy轴交叉框高
                // xy特殊处理
                if (_this.diffX < 0 && this.diffY < 0) {
                    // 无任何滚动条出现
                    _this.scrollbarX.hide();
                    _this.scrollbarY.hide();
                    _this.lenY = 0;
                    _this.lenX = 0;
                    _this.moveTop = 0;
                    _this.moveLeft = 0;
                }
                else {
                    if (_this.diffX > 0) {
                        var Y_diff = _this.diffY + _this.scrollbarX.height(); //需加上x轴滚动条的高度
                        // x滚动条开启后判断是否开启y轴滚动条
                        if (Y_diff > 0) {
                            // 开启xy轴滚动条
                            _this.lenX = (_this.containerWidth / _this.contentWidth).toFixed(2); // X轴滑块长度
                            _this.sliderX.css({
                                width: _this.lenX * 100 + "%",
                            });
                            _this.lenY = (_this.containerHeight / _this.contentHeight).toFixed(2); // y轴滑块长度
                            _this.sliderY.css({
                                height: _this.lenY * 100 + "%",
                            });
                            _this.scrollbarX.show();
                            _this.scrollbarY.show();
                            _this.cross.show();
                            // 滚动条长度
                            _this.trackX = _this.containerWidth - crossW;
                            _this.trackY = _this.containerHeight - crossH;
                            // 计算滑块可以移动top/left值
                            _this.moveTop = _this.trackY - _this.sliderY.height();
                            _this.moveLeft = _this.trackX - _this.sliderX.width();
                        }
                        else {
                            // 只开启x滚动条
                            _this.lenX = (_this.containerWidth / _this.contentWidth).toFixed(2); // X轴滑块长度
                            _this.sliderX.css({
                                width: _this.lenX * 100 + "%",
                            });
                            _this.scrollbarX.show();
                            _this.scrollbarY.hide();
                            _this.cross.hide();
                            _this.lenY = 0;
                            _this.trackX = _this.containerWidth;
                            _this.trackY = 0;
                            _this.moveLeft = _this.trackX - _this.sliderX.width();
                            _this.moveTop = 0;
                        }
                    }
                    else if (_this.diffY > 0) {
                        var X_diff = _this.diffX + _this.sliderY.width(); //需加上Y轴滚动条的宽度
                        // y轴滚动条开启后判断是否开启x轴滚动条
                        if (X_diff > 0) {
                            // 开启xy轴滚动条
                            _this.lenX = (_this.containerWidth / _this.contentWidth).toFixed(2); // X轴滑块长度
                            _this.sliderX.css({
                                width: _this.lenX * 100 + "%",
                            });
                            _this.lenY = (_this.containerHeight / _this.contentHeight).toFixed(2); // y轴滑块长度
                            _this.sliderY.css({
                                height: _this.lenY * 100 + "%",
                            });
                            _this.scrollbarX.show();
                            _this.scrollbarY.show();
                            _this.cross.show();
                            _this.trackX = _this.containerWidth - crossW;
                            _this.trackY = _this.containerHeight - crossH;
                            // 计算滑块可以移动top/left值
                            _this.moveTop = _this.trackY - _this.sliderY.height();
                            _this.moveLeft = _this.trackX - _this.sliderX.width();
                        }
                        else {
                            // 只开启y滚动条
                            _this.lenY = (_this.containerHeight / _this.contentHeight).toFixed(2); // y轴滑块长度
                            _this.sliderY.css({
                                height: _this.lenY * 100 + "%",
                            });
                            _this.scrollbarY.show();
                            _this.scrollbarX.hide();
                            _this.cross.hide();
                            _this.lenX = 0;
                            _this.trackY = _this.containerHeight;
                            _this.trackX = 0;
                            _this.moveTop = _this.trackY - _this.sliderY.height();
                            _this.moveLeft = 0;
                        }
                    }
                }
                break;
        }
    };
    /**
     * 监听内容盒子变化
     */
    Smilebar.prototype.monitorContent = function () {
        // @ts-ignore  this
        var _this = this;
        _this.timer = setInterval(function () {
            // 宽差距
            var diffW = _this.content.width() - _this.contentWidth;
            // 高差距
            var diffH = _this.content.height() - _this.contentHeight;
            if (_this.config.type == "y") {
                //仅y轴滚动条
                if (!diffH)
                    return;
                _this.checkScrollbar();
            }
            else if (_this.config.type == "x") {
                //仅x轴滚动条
                if (!diffW)
                    return;
                _this.checkScrollbar();
            }
            else {
                //xy轴滚动条
                if (!diffW && !diffH)
                    return;
                _this.checkScrollbar();
            }
        }.bind(_this), 200);
    };
    /**
     * 修改绑定事件的回调函数的_this指向
     * @param cb 回调函数
     */
    Smilebar.prototype.applyFun = function (cb) {
        // @ts-ignore  this
        var _this = this;
        return function () {
            cb.apply(_this, Array.prototype.slice.call(arguments));
        };
    };
    /**
     * 绑定事件
     * @param Ele 绑定事件的元素
     * @param event 绑定的事件名称
     * @param cb 回调函数
     */
    Smilebar.prototype.bindEvent = function (Ele, event, cb) {
        // @ts-ignore  this
        var _this = this;
        return Ele.on(event, _this.applyFun(cb));
    };
    /**
     * 百分比移动转换为number移动
     * @param value 百分比值
     * @param type  移动轴类型  x轴:"x" y轴:"y"x轴:"x" y轴:"y"
     */
    Smilebar.prototype.percentToNumber = function (value, type) {
        // @ts-ignore  this
        var _this = this;
        // 保存返回结果
        var result = 0;
        // 切割"%"
        var res = Number(value.split("%")[0]);
        // 预防NaN
        res = isNaN(res) ? 0 : res;
        if (type == "x") {
            // x轴
            result = (_this.trackX * res) / 100 - _this.sliderX.width() / 2;
        }
        else if (type == "y") {
            // y轴
            result = (_this.trackY * res) / 100 - _this.sliderY.height() / 2;
        }
        return result;
    };
    /**
     * 更新数据   当滚动盒子固定宽高发生改变时调用
     * @param position 移动的坐标对象(默认为initPosition)
     */
    Smilebar.prototype.update = function (position) {
        // @ts-ignore  this
        var _this = this;
        _this.config.initPosition = position ? position : _this.config.initPosition;
        // 重新计算对应参数
        _this.checkScrollbar();
        // 重新移动位置
        _this.moving(_this.config.initPosition);
    };
    /**
     * 统计开启键盘方向的总数
     */
    Smilebar.prototype.keyTotal = 0;
    /**
     * 绑定到jq原型链上
     * @param config 配置
     */
    $.fn.smilebar = function (config) {
        // @ts-ignore  绑定到jq原型链
        return new Smilebar(this, config);
    };
    // @ts-ignore  需引入jq
})(jQuery);
