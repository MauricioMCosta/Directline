(function (angular) {
    angular
        .module("directlineApp")
        .controller("StatisticsCtl", function (directlineAPI) {
            var _this = this;
            this.data = {};

            this.generateToken = function () {
                directlineAPI.statistics().then(function (data) {
                    _this.data = data.data;
                });
            };
        });
})(angular);