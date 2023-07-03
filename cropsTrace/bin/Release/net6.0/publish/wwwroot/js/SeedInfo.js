function SeedInfoPage() {
    this.queryPageParameter = {
        where: '',
        startTime: '',
        endTime: '',
        pageIndex: 1,
        pageSize: 10,
        sortField: 'CreatedDateTime',
        sortMethod: 'DESC'
    };
    this.startTime = "";
    this.endTime = "";
    this.queryWhere = "";
    this.PageIndex = 1;
    this.PageData = [];
    this.PageSize = 10;
    this.PageCount = 0;
    this.RecordCount = 0;
    this.Action = '';
    this.apiHelper = null;
    this.apiData = {
        cropsId: "",
        companyId: "",
        seedName: "",
        seedVariety: "",
        seedClass: "",
        plantArea: "",
        createdDateTime: "",
        modifiedDateTime: "",
        introduce: "",
        soilType:""
    };
    this.InitSeedInfoPage = function (callback) {
        var _that = this;
        this.queryPageParameter.where = this.queryWhere;
        this.queryPageParameter.startTime = this.startTime;
        this.queryPageParameter.endTime = this.endTime;
        this.queryPageParameter.pageSize = this.PageSize;
        this.queryPageParameter.pageIndex = this.PageIndex;
        console.log("this.queryPageParameter");
        console.log(this.queryPageParameter);
        this.apiHelper.querySeedInfoPage(
            this.queryPageParameter,
            function (response) {
                if (response.data.status == 0) {
                    callback(response);
                }
            }
        );
    };
    this.saveSeedInfo = function (callback) {
        switch (this.Action) {
            case "Add":
                this.apiHelper.addSeedInfo(
                    this.apiData,
                    function (response) {
                        callback(response);
                    }
                );
                break;
            case "Edit":
                this.apiHelper.editSeedInfo(
                    this.apiData,
                    function (response) {
                        callback(response);
                    }
                );
                break;
        }
    }
}