function GrowthInfoPage()
{
    this.queryGrowthInfoParam = {
        CompanyId: '',
        where: '',
        startTime: '',
        endTime: '',
        pageIndex: 1,
        pageSize: 10,
        sortField: '',
        sortMethod: ''
    };
    this.CompanyId = '';
    this.where = '';
    this.startTime = '';
    this.endTime = '';
    this.pageIndex = 1;
    this.pageSize = 10;
    this.sortField = '';
    this.sortMethod = '';
    this.PageCount = 0;
    this.RecordCount = 0;
    this.apiHelper = null;
    this.InitGrowthInfoPage = function (callback) {
        var _that = this;
        //#region 赋值分页参数
        _that.queryGrowthInfoParam.CompanyId = _that.CompanyId;
        _that.queryGrowthInfoParam.where = _that.where;
        _that.queryGrowthInfoParam.startTime = _that.startTime;
        _that.queryGrowthInfoParam.endTime = _that.endTime;
        _that.queryGrowthInfoParam.pageIndex = _that.pageIndex;
        _that.queryGrowthInfoParam.pageSize = _that.pageSize;
        _that.queryGrowthInfoParam.sortField = _that.sortField;
        _that.queryGrowthInfoParam.sortMethod = _that.sortMethod;
        //#endregion
        console.log("_that.queryGrowthInfoParam");
        console.log(_that.queryGrowthInfoParam);
        this.apiHelper.queryGrowthInfoPage(
            this.queryGrowthInfoParam,
            function (response) {
                if (response.data.status == 0) {
                    callback(response);
                }
            }
        );
    }
}
function FileInfoParameter()
{
    this.fileId = "";
    this.cropsId = "";
    this.growthRecordId = "";
    this.fileName = "";
    this.fileUrl = "";
    this.fileLength = "";
    this.createdDateTime = "";
    this.showParamJson = "";
}
function BatchSaveFileInfoParameter()
{
    this.cropsId = "";
    this.growthInfoId = "";
    this.companyId = "";
    this.token = "";
    this.saveFilesData = [];            
}