var app = angular.module('app', ['ngTouch', 'ui.grid',
    'ui.grid.expandable', 'ui.grid.selection',
    'ui.grid.pinning', 'ui.grid.exporter']);

app.controller('ParamDefinitionCtrl', ['$scope', '$http', function ($scope, $http) {
    $scope.quarterGrid = {
        enableSorting: false,
        enableColumnMenus: false,
        expandableRowTemplate: '<div ui-grid="row.entity.paramerterGrid" ui-grid-expandable></div>',
        expandableRowHeight: 350,
        onRegisterApi: function (gridApi) {

            $scope.paramerterGridApi = gridApi;

            gridApi.expandable.on.rowExpandedStateChanged($scope, function (row) {
                if (row.isExpanded) {
                    row.entity.paramerterGrid = {
                        enableSorting: false,
                        enableColumnMenus: false,
                        expandableRowTemplate: '<div ui-grid="row.entity.definitionGrid" style="height : 150px"></div>',
                        expandableRowHeight: 150,
                        onRegisterApi: function (gridApi) {

                            $scope.definitionGridApi = gridApi;

                            gridApi.expandable.on.rowExpandedStateChanged($scope, function (row) {
                                if (row.isExpanded) {
                                    row.entity.definitionGrid = {
                                        enableSorting: false,
                                        enableColumnMenus: false,
                                        columnDefs: [
                                            { name: 'MinValue', cellClass: 'definitionRow' },
                                            { name: 'MaxValue', cellClass: 'definitionRow' },
                                            { name: 'Result', cellClass: 'definitionRow' }
                                        ]
                                    };

                                    $scope.definitionGridData;
                                    var data = { AccountParamID: row.entity.AccountParamID };
                                    $http({
                                        url: '/Parameters/GetParameterBounds',
                                        data: data,
                                        method: 'POST'
                                    }).then(function (response) {
                                        row.entity.definitionGrid.data = response.data;
                                    });
                                }
                            });
                        },
                        columnDefs: [
                            { name: 'AccountParamID', visible: false },
                            { name: 'ParameterName', pinnedLeft: true, cellClass: 'parameterRow' },
                            { name: 'Weightage', cellClass: 'parameterRow' }
                        ]
                    };

                    var data = {
                        AccountID: $scope.selectedProject,
                        Quarter: row.entity.Quarter,
                        Year: $scope.selectedYear
                    };
                    $http({
                        url: '/Parameters/GetAccountParameters',
                        data: data,
                        method: 'POST'
                    }).then(function (response) {
                        row.entity.paramerterGrid.data = response.data;
                        $scope.parameterGridData = response.data;
                    });
                }
            });
        },
        columnDefs: [
            { name: 'Quarter', pinnedLeft: true, cellClass: 'quarterRow' }
        ]

    };

    var expandQuarterRow = function () {
        $scope.paramerterGridApi.expandable.collapseAllRows();
        var index, rows = $scope.quarterGrid.data;

        for (index = 0; index < rows.length; index++) {
            if (rows[index].Quarter === $scope.selectedQuarter) {
                $scope.paramerterGridApi.expandable.expandRow(rows[index]);
                
            }
        }
    },

    expandParameterRow = function () {
        $scope.definitionGridApi.expandable.collapseAllRows();
        var index, rows = $scope.parameterGridData;

        for (index = 0; index < rows.length; index++) {
            if (rows[index].AccountParamID === $scope.selectedParameter) {
                $scope.definitionGridApi.expandable.expandRow(rows[index]);
            }
        }
    },


   


    populateGrid = function () {
        //$http({
        //    url: '/Parameters/GetQuarterJson',
        //    data: { AccountID: $scope.selectedProject, Year : $scope.selectedYear },
        //    method: 'POST'
        //}).then(function (response) {
        //    $scope.quarterGrid.data = response.data;

        //    var data = { AccountID: $scope.selectedProject, Quarter: $scope.selectedQuarter };
        //    $http({
        //        url: '/Parameters/GetAccountParameters',
        //        data: data,
        //        method: 'POST'
        //    }).then(function (response) {
        //        $scope.parameterGridData = response.data;
        //        expandQuarterRow(response.data);

        //        var data = { AccountParamID: $scope.selectedParameter };
        //        $http({
        //            url: '/Parameters/GetParameterBounds',
        //            data: data,
        //            method: 'POST'
        //        }).then(function (response) {
        //            $scope.definitionGridData = response.data;
        //            expandParameterRow();
        //        });
        //    });
        //});

       return $http({
            url: '/Parameters/GetQuarterJson',
            data: { AccountID: $scope.selectedProject, Year: $scope.selectedYear },
            method: 'POST'
        }).then(function (response) {
            $scope.quarterGrid.data = response.data;
        });
    };

    //$scope.$watch(function ($scope) { return $scope.populateGrid },
    //    function (value) {
    //        if (value === true)
    //            populateGrid();
    //    });

    //$scope.$watch(function ($scope) { return $scope.expandQuarterRow },
    //    function (value) {
    //        if (value === true)
    //            expandQuarterRow();   
    //    });

    //$scope.$watch(function ($scope) { return $scope.expandParameterRow },
    //    function (value) {
    //        if (value === true)
    //            expandParameterRow();
    //    });

    $http.get("/Parameters/GetAccounts")
        .then(function (response) {
            $scope.projects = response.data;
        });


    $scope.selectedProject = "";
    $scope.quarters = ["Quarter1", "Quarter2", "Quarter3", "Quarter4"];

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
        $scope.selectedQuarter = "";
        $scope.selectedParameter = "";
        $scope.defineParameter = "";
        $scope.definitionExist = "";

        $scope.populateGrid = true;
        populateGrid();
        filterQuarter();
    };

    var filterQuarter = function (startDate) {
        var option1 = document.getElementById("option1");
        var option2 = document.getElementById("option2");
        var option3 = document.getElementById("option3");
        var option4 = document.getElementById("option4");

        //var today = new Date('4/1/2017');
        //var today = new Date('7/1/2017');
        //var today = new Date('10/1/2017');
        //var today = new Date('1/1/2017');

        var today = new Date();
        today.setMonth(today.getMonth() - 3);

        if ($scope.selectedYear === today.getFullYear().toString()) {
            var month = today.getMonth() + 1;

            if (month <= 3)
                option2.disabled = true;
            if (month <= 6)
                option3.disabled = true;
            if (month <= 9)
                option4.disabled = true;
        }
    }

    $scope.getAllParameters = function () {
        $http.get("/Parameters/GetAccountParameters")
         .then(function (response) {
             $scope.allParameters = response.data;
         });
    };

    $scope.parameterSelection = function (parameterID) {
        var index = $scope.selectedParameters.indexOf(parameterID);

        // Is currently selected
        if (index > -1) {
            $scope.selectedParameters.splice(index, 1);
        }
            // Is newly selected
        else {
            $scope.selectedParameters.push(parameterID);
        }
    };

    $scope.saveNewParameter = function () {

        var confirmed = confirm("Are you sure? Changes once saved can't be undone. ");
        if (confirmed) {
            var projectParameters = {
                ProjectId: $scope.selectedProject,

                Quarter: $scope.selectedQuarter,
                Paramters: $scope.selectedParameters,
                Year: $scope.selectedYear
            };

            $http({
                method: 'POST',
                dataType: 'JSON',
                url: "/Parameters/SaveAccountParameters",
                data: projectParameters
            }).then(function (response) {
                alert("Parameters has been added for the project");



                populateGrid().then(function () {
                    $scope.getProjectParameters();
                });
            });
        }
    };

    $scope.addParameter = function () {
        var confirmed = confirm("Are you sure? Changes once saved can't be undone. ");
        if (confirmed) {
            var newParameterData = {
                ParamName: $scope.newParameter
            };
            $http({
                method: 'POST',
                url: '/Parameters/AddNewParameter',
                dataType: 'json',
                data: newParameterData
            }).then(function (response) {
                alert(response.data);
                $scope.newParameter = "";
                $scope.newParamaterForm.$setPristine();
                $scope.getAllParameters();
            });
        }
    };


    $scope.getProjectParameters = function () {

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

            if ($scope.parameters === null || $scope.parameters.length === 0) {
                $scope.defineParameter = true;
                $scope.selectedParameters = [];
            }
            else { 
                $scope.defineParameter = "";
                expandQuarterRow();

            }
        });
    };

    $scope.createParameterDefinitionTemplate = function () {

        if ($scope.selectedParameter !== null) {


            $scope.projectQuarters = [];
            var accountParamId = {
                AccountParamID: $scope.selectedParameter
            };
            $http({
                url: '/Parameters/GetParameterBounds',
                data: accountParamId,
                method: 'POST'
            }).then(function (response) {
                $scope.parameterDefinitions = response.data;

                if ($scope.parameterDefinitions === null || $scope.parameterDefinitions.length === 0) {
                    $scope.definitionExist = "";
                    $scope.weightage = "";

                    $scope.parameterDefinitions = [{
                        AccountParamID: $scope.selectedParameter,
                        MinValue: "",
                        MaxValue: "",
                        Result: ""
                    }];

                    var selectedData = {
                        AccountID: $scope.selectedProject,
                        Year: $scope.selectedYear
                    };

                    $http({
                        url: '/Parameters/GetQuarterJson',
                        data: { AccountID: $scope.selectedProject, Year: new Date().getFullYear() },
                        method: 'POST'
                    }).then(function (response) {
                        $scope.projectQuarters = response.data;
                        $scope.projectQuarters.pop();
                    });

                }
                else {
                    $scope.definitionExist = true;
                    expandParameterRow();
                }
            });
        }
        else {
            $scope.definitionGridApi.expandable.collapseAllRows();
        }
    };

    $scope.addParameterDefinitionTemplate = function () {
        $scope.parameterDefinitions.push({
            AccountParamID: $scope.selectedParameter,
            MinValue: "",
            MaxValue: "",
            Result: ""
        });
    };

    $scope.removeParameterDefinitionTemplate = function (index) {
        $scope.parameterDefinitions.splice(index, 1);
    };

    $scope.saveParameterDefinition = function () {

        $scope.expandQuarterRow = false;

        var confirmed = confirm("Are you sure? Changes once saved can't be undone. ");
        if (confirmed) {

            var bounds = $scope.parameterDefinitions,
            weightage = {
                AccountParamID: $scope.selectedParameter,
                Weightage: $scope.weightage
            };

            $http({
                url: "/Parameters/SaveWeightage",
                data: weightage,
                method: 'POST'
            }).then(function () {
                $http({
                    url: "/Parameters/SaveParameterBounds",
                    method: 'POST',
                    data: bounds
                }).then(function () {

                    alert("Definitions have been added for parameter");
                    $scope.definitionForm.$setPristine();
                });
                $scope.expandParameterRow = true;
            });


        }
    };

    $scope.clearParameterName = function () {
        $scope.newParameter = "";
        $scope.newParamaterForm.$setPristine();
    };

    $scope.copyDefinitions = function (quarter) {

        var cloneData = {
            AccountID: $scope.selectedProject,
            AccountParamID: $scope.selectedParameter,
            Quarter: quarter,
            Year: $scope.selectedYear
        };

        $http({
            url: '/Parameters/CopyPreviousParameterBounds',
            data: cloneData,
            method: 'POST'
        }).then(function (response) {
            if (response.data !== null || response.data !== 0) {
                $scope.parameterDefinitions = response.data;
                var i;
                for (i = 0; i < $scope.parameterDefinitions.length; i++) {
                    $scope.parameterDefinitions[i].AccountParamID = $scope.selectedParameter;
                }
            }
        });
    };

    $scope.allSelected = function () {
        return $scope.selectedProject && $scope.selectedYear && $scope.selectedQuarter;
    };

}]);