var app = angular.module('nilaiApp', ['ngRoute']);

app.config(['$routeProvider', function($routeProvider) {
    $routeProvider
        .when('/input', {
            templateUrl: 'views/input-nilai.html',
            controller: 'InputNilaiController'
        })
        .when('/laporan', {
            templateUrl: 'views/laporan-nilai.html',
            controller: 'LaporanNilaiController'
        })
        .otherwise({
            redirectTo: '/input'
        });
}]);