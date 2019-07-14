(function (angular) {
    /*
    ** Defines "directlineApp" to be used in AngularJS module
    */
    angular
        .module("directlineApp", ["ngMaterial", "ngMessages", "ngAnimate", "ngRoute"])
        .config(
            function ($mdThemingProvider, $mdIconProvider,
                $routeProvider, $locationProvider) {
                // set location provider to enable html5mode
                $locationProvider
                    .html5Mode({
                        enabled: false,
                        requireBase: false
                    });
                // default theming for MaterialDesign
                $mdThemingProvider
                    .theme("default");
                // registering fontawesome as icon provider
                $mdIconProvider
                    .fontSet('fa', 'fontawesome');
                // set 
                $routeProvider
                    .when('/', {
                        templateUrl: 'pages/home.html',
                        controller: 'HomeCtl'
                    })
                    .when('/tokenGen', {
                        templateUrl: 'pages/tokengen.html',
                        controller: 'TokenGeneratorCtl',
                        controllerAs: 'ctl'
                    })
                    .when('/swagger', {
                        redirectTo: function (routeParams) {
                            return $location('/swagger');
                        }
                    })
                    .when('/stats', {
                        templateUrl: 'pages/statistics.html',
                        controller: 'StatisticsCtl',
                        controllerAs: 'ctl'
                    })
                    .when('/about', {
                        templateUrl: "pages/about.html"
                    })
                    .when('/test', {
                        template: '<h1>testlasdjfçalsdkjfçalkdfjaçlsdkfjaçsldkfjaçsdlkfjaçsldfkjaçsldkfjaçlsdkfjçalsdkfjaçsldkfj</h1>'
                    });

            })
        .run([
            '$rootScope',
            function ($rootScope) {
                // see what's going on when the route tries to change
                $rootScope.$on('$routeChangeStart', function (event, next, current) {
                    // next is an object that is the route that we are starting to go to
                    // current is an object that is the route where we are currently
                    var currentPath = current.originalPath;
                    var nextPath = next.originalPath;

                    console.log('Starting to leave %s to go to %s', currentPath, nextPath);
                });
            }
        ])
        .service('directlineAPI', function ($http) {
            // generateSecret returns a promise to the api/secret call. 
            this.generateSecret = function (botServiceUrl, botUsername, botPassword) {
                return $http({
                    url: '/api/secret',
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    data: { Url: botServiceUrl, Username: botUsername, Password: botPassword }
                });
            };
            // bring from API controller the Statistics for this service 
            this.statistics = function () {
                return $http({
                    url: '/api/statistics',
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });
            }; 
        })
        .controller("AppController", function ($scope, $mdSidenav, directlineAPI) {
            $scope.toggleMenu = function toggleMenu() {
                $mdSidenav('left').toggle();
            };
            $scope.pages = [
                { icon: "fas fa-key", name: "Generate Token", url: "tokenGen" },
                { icon: "fas fa-link", name: "API Primitives", url: "swagger" },
                { icon: "fas fa-info", name: "About", url: "about" }
            ];
            $scope.statistics = function () {
                directlineAPI.statistics().then(function (data) {

                });
            };
        });
})(angular);