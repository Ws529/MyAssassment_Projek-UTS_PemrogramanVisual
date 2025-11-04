// NAMA MODUL HARUS 'myAssessmentApp' agar konsisten dengan app.js
angular.module('myAssessmentApp') 
.controller('LaporanNilaiController', ['$scope', '$http', function($scope, $http) {
    $scope.nilai = [];
    $scope.search = '';
    $scope.filterMataKuliah = '';
    
    function loadData() {
        // PATH API HARUS '/api/penilaian' agar konsisten
        $http.get('/api/penilaian').then(function(response) { 
            $scope.nilai = response.data;
        });
    }
    
    loadData();
    
    $scope.sort = function(field) {
        $scope.sortField = field;
        $scope.sortReverse = !$scope.sortReverse;
    };
    
    $scope.showDetail = function(nilai) {
        // Implementation for modal detail
        alert(JSON.stringify(nilai, null, 2)); // Contoh detail sederhana
    };
    
    $scope.deleteNilai = function(id) {
        if(confirm('Yakin ingin menghapus data ini?')) {
            // PATH API HARUS '/api/penilaian'
            $http.delete('/api/penilaian/' + id) 
                .then(function() {
                    loadData();
                });
        }
    };
    
    $scope.exportToExcel = function() {
        // PATH API HARUS '/api/penilaian'
        window.location.href = '/api/penilaian/export'; 
    };
}]);