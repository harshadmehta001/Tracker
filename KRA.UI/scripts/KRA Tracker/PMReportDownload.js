var app = angular.module("app", []);

app.controller("PMReportCtrl", function ($scope, $http) {

    $http.get("/Parameters/GetAccounts")
        .then(function (response) {
            $scope.projects = response.data;
        });

    //$scope.projects = [{ id: "1", name: "HTS" }, { id: "2", name: "MS" }, { id: "3", name: "PMO" }];
    $scope.selectedProject = "";
    $scope.quarters = ["Q1", "Q2", "Q3", "Q4"];
    $scope.dataPresent = true;

    $scope.getYears = function () {

        //$scope.years = ["2012", "2013", "2014", "2015", "2016", "2017"];

        var data = { AccountID: $scope.selectedProject };
        $http({
            url: '/Parameters/GetYears',
            data: data,
            method: 'POST'
        }).then(function (response) {
            $scope.years = response.data;

            var today = new Date();
            today.setMonth(today.getMonth() - 3);
            $scope.years.push(today.getFullYear().toString());
        });

        $scope.selectedYear = "";

    };

    $scope.generatePMReport = function () {

        //$scope.parameterDetails = [{ "AccountParamID": 5, "Score": [20, 20, 0, 10], "ParameterName": "CSAT", "AccountID": 0, "Weightage": 0 },
        // { "AccountParamID": 6, "Score": [20, 20, 0, 10], "ParameterName": "Gross Margin", "AccountID": 0, "Weightage": 25 },
        // { "AccountParamID": 7, "Score": [20, 20, 0, 10], "ParameterName": "Compliance Score", "AccountID": 0, "Weightage": 25 },
        // { "AccountParamID": 8, "Score": [20, 20, 0, 10], "ParameterName": "DRE", "AccountID": 0, "Weightage": 25 }];
        $scope.message = "";
        $scope.dataPresent = false;
        if ($scope.selectedYear !== null) {
            var selectedData = { AccountID: $scope.selectedProject, Year: $scope.selectedYear };
         $http({
                url: '/KraScores/GetFinalScore',
                data: selectedData,
                method: 'POST'
            }).then(function (response) {
                $scope.parameterDetails = response.data;
                if ($scope.parameterDetails.length > 0 && $scope.parameterDetails !== null) {
                    $scope.finalScore = [];
                    var i, j, total;

                    for (i = 0; i < $scope.parameterDetails[0].Score.length; i++) {
                        total = 0;
                        for (j = 0; j < $scope.parameterDetails.length; j++) {
                            total += $scope.parameterDetails[j].Score[i];
                        }
                        $scope.finalScore.push(total);
                    }

                    $scope.dataPresent = true;
                }
                $scope.message = "--- No Data Available ---";
            });
        }
    };
});

app.filter('roundoff', function () {
    return function (value) {
        return Math.ceil(value);
    };
});