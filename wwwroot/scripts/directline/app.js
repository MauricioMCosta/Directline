angular.module("directlineApp", ["ngMaterial", "ngMessages","ngAnimate","ngMaterial", "ngRoute"])
    .config(function ($mdThemingProvider, $mdIconProvider, $routeProvider) {
        $mdThemingProvider.theme("default");
        $mdIconProvider          
            .fontSet('fa', 'fontawesome');
        $routeProvider
            .when('/', {

            })
            .when('/tokenGen', {

            })
            .when('/swagger', {
                redirectTo: function (routeParams) {
                    return $location('/swagger');
                }
            })
            .when('/about', {
                template:"<h1>About</h1>"
            });
       
    })
   
    .controller("AppController", function ($scope, $mdSidenav) {
        $scope.toggleMenu = function toggleMenu() {
            $mdSidenav('left').toggle();
        };
        $scope.pages = [
            { icon: "fas fa-key", name: "Generate Token", url:"tokenGen"},
            { icon: "fas fa-link", name: "API Primitives", url: "swagger" },
            { icon: "fas fa-info", name: "About", url:"about"}
        ];
    });