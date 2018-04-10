var app = angular.module("app", []);

app.controller("InputScoreCtrl", function ($scope, $http) {

    $http.get("/Parameters/GetAccounts")
        .then(function (response) {
            $scope.projects = response.data;
        });

    //  $scope.projects = [{ id: "1", name: "HTS" }, { id: "2", name: "MS" }, { id: "3", name: "PMO" }];
    $scope.selectedProject = "";

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

    $scope.getQuarters = function () {

        $scope.quarters = "";

        var selectedData = {
            AccountID: $scope.selectedProject,
            Year: $scope.selectedYear
        };

        $http({
            url: '/Parameters/GetQuarters',
            data: selectedData,
            method: 'POST'
        }).then(function (response) {
            $scope.quarters = response.data;
        });

        // $scope.quarters = ["Q1", "Q2", "Q3", "Q4"];
        $scope.selectedQuarter = "";
    };


    $scope.getParameters = function () {

        //$scope.parameters = [{ AccParamID: "1", name: "CSAT", weightage: 25, score: 0 },
        //                  { AccParamID: "2", name: "Gross Margin", weightage: 25, score: 0 },
        //                  { AccParamID: "3", name: "Compliance Score", weightage: 25, score: 0 },
        //                  { AccParamID: "4", name: "DRE", weightage: 25, score: 0 }
        //];

        var selectedData = {
            AccountID: $scope.selectedProject,
            Quarter: $scope.selectedQuarter,
            Year: $scope.selectedYear
        };
       
        $http({
            url: '/Parameters/GetAccountParameters',
            data: selectedData,
            method: 'POST'
        }).then(function (response) {
            $scope.parameters = response.data;
        });

        $http({
            url: '/Parameters/GetTeamSize',
            data: selectedData,
            method: 'POST'
        }).then(function (response) {
            $scope.teamSize = response.data;
        });
    };


    $scope.saveScores = function () {

        var teamSizeData = {
            AccountID: $scope.selectedProject,
            TeamSize: $scope.teamSize,
            Quarter: $scope.selectedQuarter,
            Year: $scope.selectedYear
        };

        $http({
            url: '/KraScores/SaveAccountScores',
            method: 'POST',
            data: $scope.parameters
        }).then(function() {
            $http({
                url: '/Parameters/UpdateTeamSize',
                method: 'POST',
                data: teamSizeData
            }).then(function () {
                alert("Scores saved");
                $scope.selectedQuarter = "";
                $scope.selectedProject = "";
            });
        });
    };
});