
var app = angular.module('app', ['ui.bootstrap']);

app.directive("report", function () {
    return {
        templateUrl: '/KraScores/ReportTemplate',
        scope: {
            parameterDetails: '=parameterdetails',
            finalScore: '=finalscore',
            quarters: '=quarters'
        },
        restrict: 'E'
    };
});

app.controller('ADMReportCtrl', function ($scope, $http) {

    $scope.managers = [{ id: 1, name: "Manager A" }, { id: 2, name: "Manager B" }, { id: 3, name: "Manager C" }];
    $scope.quarters = ["Q1", "Q2", "Q3", "Q4"];

    $scope.years = ["2012", "2013", "2014", "2015", "2016", "2017"];
    //$http({
    //    url: '/Parameters/GetYears',
    //    data: data,
    //    method: 'POST'
    //}).then(function (response) {
    //    $scope.years = response.data;
    //});
    $scope.selectedYear = "";

    $scope.getProjects = function () {

        $http({
            url: "/Parameters/GetAccounts",
            data: { ManagerID: $scope.selectedManager },
            method: 'POST'
        }).then(function (response) {
            $scope.projects = response.data;
        });
        $scope.selectedYear = "";
    }

    //    $scope.parameterDetails = [{ "AccountParamID": 5, "Score": [20, 20], "ParameterName": "CSAT", "AccountID": 0, "Weightage": 25 },
    //{ "AccountParamID": 6, "Score": [20, 20], "ParameterName": "Gross Margin", "AccountID": 0, "Weightage": 25 },
    //{ "AccountParamID": 7, "Score": [20, 20], "ParameterName": "Compliance Score", "AccountID": 0, "Weightage": 25 },
    //{ "AccountParamID": 8, "Score": [20, 20], "ParameterName": "DRE", "AccountID": 0, "Weightage": 25 }];

    $scope.generateADMReport = function () {
        $scope.finalScore = [];
        $scope.parameterDetails = [];
        var managerid = {ManagerID:$scope.selectedManager ,Year : $scope.selectedYear}
        $http(
            {
                url: "/KraScores/GetAdmReport",
                method: 'POST',
                data : managerid
            }).then(function (response) {
            var i, j, total;
            $scope.parameterDetails = response.data;
            for (i = 0; i < $scope.parameterDetails[0].Score.length; i++) {
                total = 0;
                for (j = 0; j < $scope.parameterDetails.length; j++) {
                    total += $scope.parameterDetails[j].Score[i];
                }
                $scope.finalScore.push(total);
            }
        });
    };

    $scope.generatePMReport = function (projectId) {
        $scope.finalScore = [];
        $scope.parameterDetails = [];

        var selectedData = { AccountID: projectId ,Year:$scope.selectedYear };
        $http({
            url: '/KraScores/GetFinalScore',
            data: selectedData,
            method: 'POST'
        }).then(function (response) {
            $scope.parameterDetails = response.data;
            var i, j, total;
            for (i = 0; i < $scope.parameterDetails[0].Score.length; i++) {
                total = 0;
                for (j = 0; j < $scope.parameterDetails.length; j++) {
                    total += $scope.parameterDetails[j].Score[i];
                }
                $scope.finalScore.push(total);
            }
        });
    };
});

app.filter('roundoff', function () {
    return function (value) {
        return Math.ceil(value);
    };
});