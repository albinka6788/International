rootApplication.ProjectDetailController = function ($scope, utilities, $compile, $timeout, $filter) {

    //Project Details

    $scope.InitProjectDetailPage = function () {
        $.validator.unobtrusive.parse('#ProjectForm');
        $('#ProjectForm').validate();
    }
    $scope.IslongRange = false;
    $scope.IslatRange = false;
    $scope.setStateList = function () {
        if ($scope.submissionModel.ProjectDetail.CountryId) {
            utilities.ajax({
                url: '/Master/stateByCountry',//.format($scope.brokerContactPersonId),
                method: 'GET',
                params: { countryId: $scope.submissionModel.ProjectDetail.CountryId },
                success: function (mvcResponse) {
                    $scope.submissionListModel.StateList = mvcResponse;
                }
            });
        }
        else $scope.submissionListModel.StateList = null;
    };

    $scope.setCityList = function () {
        if ($scope.submissionModel.ProjectDetail.StateId) {
            utilities.ajax({
                url: '/Master/CityByState',//.format($scope.brokerContactPersonId),
                method: 'GET',
                params: { stateId: $scope.submissionModel.ProjectDetail.StateId },
                success: function (mvcResponse) {
                    $scope.submissionListModel.CityList = mvcResponse;
                }
            });
        }
        else $scope.submissionListModel.CityList = null;
    };

    $scope.saveProject = function () {
        if ($scope.IslongRange || $scope.IslatRange) return false;
        var form = angular.element('#ProjectForm');
        $.validator.unobtrusive.parse(form);

        if (!$(form).valid()) {
            console.log('invalid project'); return false;
        }
        else {
            $scope.incrementStep();
        }
       
    }

    $scope.checkLatRange = function () {
        if ($scope.submissionModel.ProjectDetail.Latitude)
        {
            if (parseInt($scope.submissionModel.ProjectDetail.Latitude) > 90 || !(parseInt($scope.submissionModel.ProjectDetail.Latitude) > -91))
                $scope.IslatRange = true;
            else
                $scope.IslatRange = false;
        }
      
    }

    $scope.checkLongRange = function () {
        if ($scope.submissionModel.ProjectDetail.Longitude)
        {
            if (parseInt($scope.submissionModel.ProjectDetail.Longitude) > 180 || !(parseInt($scope.submissionModel.ProjectDetail.Longitude) > -181))
                $scope.IslongRange = true;
            else
                $scope.IslongRange = false;
        }
  
    }
    // End Project Details
}