angular.module('nilaiApp')
.controller('InputNilaiController', ['$scope', '$http', function($scope, $http) {
    $scope.nilai = {};
    $scope.nilaiAkhir = 0;
    
    $scope.mataKuliah = [];
    
    // Load mata kuliah
    $http.get('/api/matakuliah').then(function(response) {
        $scope.mataKuliah = response.data;
    });
    
    $scope.hitungNilaiAkhir = function() {
        var tugas = $scope.nilai.tugas || 0;
        var uts = $scope.nilai.uts || 0;
        var uas = $scope.nilai.uas || 0;
        
        $scope.nilaiAkhir = (tugas * 0.3) + (uts * 0.3) + (uas * 0.4);
    };
    
    $scope.submitNilai = function() {
        $http.post('/api/nilai', $scope.nilai)
            .then(function(response) {
                alert('Nilai berhasil disimpan!');
                $scope.nilai = {};
                $scope.nilaiAkhir = 0;
            })
            .catch(function(error) {
                alert('Error: ' + error.data.message);
            });
    };
}]);