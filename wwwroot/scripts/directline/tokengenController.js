(function (angular) {
    angular
        .module("directlineApp")
        .controller("TokenGeneratorCtl", function () {
            this.botServiceUrl = "";
            this.botUsername = "";
            this.botPassword = "";

            this.generateToken = function () {
                alert("test");
            };

            this.token = null;
        });
})(angular);