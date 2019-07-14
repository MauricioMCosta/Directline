(function (angular) {
    angular
        .module("directlineApp")
        .controller("TokenGeneratorCtl", function (directlineAPI) {
            var _this = this;
            this.botServiceUrl = "";
            this.botUsername = "";
            this.botPassword = "";

            this.generateToken = function () {
                directlineAPI.generateSecret(this.botServiceUrl, this.botUsername, this.botPassword).then(function (data) {
                    _this.token = data.data;
                });
            };
            this.token = null;
        });
})(angular);