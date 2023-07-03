//显示字段代码
function ShowFieldsPage()
{
    this.queryShowFieldsParam = {
        CompanyId: '',
        where: "",
        condition: "",
        pageIndex: "",
        pageSize: "",
        sortField: "",
        sortMethod: ""
    };
    this.CompanyId = '';
    this.where = "";
    this.condition = "";
    this.pageIndex = 1;
    this.pageSize = 10;
    this.sortField = "RecordId";
    this.sortMethod = "DESC";
    this.PageCount = 0;
    this.RecordCount = 0;
    this.apiHelper = null;
    this.InitShowFieldsPage = function (callback) {
        var _that = this;
        //#region 赋值分页参数
        _that.queryShowFieldsParam.CompanyId = _that.CompanyId;
        _that.queryShowFieldsParam.where = _that.where;
        _that.queryShowFieldsParam.condition = _that.condition;
        _that.queryShowFieldsParam.pageIndex = _that.pageIndex;
        _that.queryShowFieldsParam.pageSize = _that.pageSize;
        _that.queryShowFieldsParam.sortField = _that.sortField;
        _that.queryShowFieldsParam.sortMethod = _that.sortMethod;
        //#endregion
        this.apiHelper.queryShowFieldsPage(
            this.queryShowFieldsParam,
            function (response) {
                if (response.data.status == 0) {
                    callback(response);
                }
            }
        );
    }
}