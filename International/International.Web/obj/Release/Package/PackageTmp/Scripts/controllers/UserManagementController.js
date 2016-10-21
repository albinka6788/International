rootApplication.controller('userManagementController', function ($scope, utilities, $compile, $timeout, $filter) {
    $scope.submit = false;
    //utilities.ajax({
    //    url: '/home/get',
    //    method: 'GET',
    //    throbber: true,
    //    throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h2') },
    //    disableScreen: false,
    //    disableControl: angular.element('h2'),
    //    success: function (mvcResponse, angularResponse) {
    //        $scope.users = mvcResponse;
    //    }
    //});

   

    $scope.MultiselectDropdownSetting = {
        smartButtonMaxItems: 3,
        smartButtonTextConverter: function (itemText, originalItem) {           

            return itemText;
        }
    };

    $scope.filterModel =
                        {
                            GroupName: null,
                            UserName: null,
                            Status: 'All',
                            FromDate: '',
                            ToDate: ''
                        }

    $scope.redirect = function (path) {
        window.location = path;
    }

    configureDateControls = function (value) {
        $('#ToDate').datepicker('setStartDate', value);

    };

    $scope.ClearUser = function () {

        $scope.filterModel.UserName = null;
        $scope.filterModel.Status = 'All';
        $scope.filterModel.FromDate = null;
        $scope.filterModel.ToDate = null;
    };

    $scope.ClearGroup = function () {
        $scope.filterModel.GroupName = null;

        $scope.filterModel.Status = 'All';
        $scope.filterModel.FromDate =null;
        $scope.filterModel.ToDate =null;
    };

    $scope.CancelGroup = function () {
        if ($scope.GroupId == 0) {
            $(this).MessageBox('Group will not be created, do you want to proceed?', GroupRedirect('Groups'))
        }
        else {
            $(this).MessageBox('Group will not be updated, do you want to proceed?', GroupRedirect('Groups'))
        }
    }

    function GroupRedirect (path) {
        location.href = 'Groups';
    }
    //watches
    $scope.$watch('filterModel.FromDate', function (value) {
        if (value) {
            configureDateControls(value);
        }
    }, true);

    $scope.getGroups = function () {

        utilities.ajax({
            url: '/UserManagement/GetGroups',
            method: 'get',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            params: { GroupName: $scope.filterModel.GroupName, Status: $scope.filterModel.Status, FromDate: $scope.filterModel.FromDate, ToDate: $scope.filterModel.ToDate },
            validate: true,
            form: 'form',
            success: function (mvcResponse) {

                table = utilities.dataTable({
                    element: angular.element('#groupList'),
                    scope: $scope,
                    aaData: mvcResponse,
                    aoColumns: [
                         {
                             "sTitle": "Actions", "bSortable": false, "sClass": 'actions', "sWidth": "70", "filter": false, "mRender": function (val, type, row) {
                                 var html = "";
                                 if (row.Status == true) {
                                     html = html + '<a href="/UserManagement/Group?GroupId={0}&isView=true" rowid={0} class="eye-link"><i class="fa fa-eye"></i></a>'.format(row.Id) +
                                        '<a href="/UserManagement/Group?GroupId={0}" rowid="{0}" class="eye-edit"><i class="fa fa-edit"></i></a>'.format(row.Id) +
                                          '&nbsp <a  href="" rowid={0} ng-click="UpdateGroupStatus({0},false)" class="eye-trash"><i class="fa fa-trash" ></i></a>'.format(row.Id)
                                    + '<a  href="" rowid={0} ng-click="UpdateGroupStatus({0},true)" style="display:none;" class="eye-undo"><i class="fa fa-undo" ></i></a>'.format(row.Id);
                                 }
                                 else
                                     html = html + '<a href="/UserManagement/Group?GroupId={0}&isView=true" rowid={0} class="eye-link"><i class="fa fa-eye"></i></a>'.format(row.Id) +
                                        '<a href="/UserManagement/Group?GroupId={0}" style="display:none;" rowid="{0}" class="eye-edit"><i class="fa fa-edit"></i></a>'.format(row.Id) +
                                        '&nbsp <a  href="" rowid={0} ng-click="UpdateGroupStatus({0},false)" style="display:none;" class="eye-trash"><i class="fa fa-trash" ></i></a>'.format(row.Id)
                                    + '<a href="" rowid={0} ng-click="UpdateGroupStatus({0},true)"  class="eye-undo"><i class="fa fa-undo" ng-if="{0}"></i></a>'.format(row.Id);

                                 return html;
                             }
                         },
                            { "mData": "GroupName", "sTitle": "Group Name", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                            {
                                "mData": "Status", "sTitle": "Status", "filter": false, "mRender": function (val, type, row) {
                                    if (val == null) return '';
                                    if (val == true) return '<span rowid=' + row.Id + '>Active</span>';
                                    else return '<span rowid=' + row.Id + '>Inactive</span>';
                                }
                            },
                            { "mData": "CreatedByUserId", "filter": false, "sTitle": "Created By", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                            { "mData": "CreatedOnDate", "sTitle": "Created On" },
                            { "mData": "LastModifiedByUserId", "sTitle": "Modified By", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' }},
                            { "mData": "LastModifedOnDate", "sTitle": "Modified On" },
                    ]
                });
            }
        });
    };

    $scope.Search = function () {

        var ToDate = new Date($scope.filterModel.ToDate);
        var FromDate = new Date($scope.filterModel.FromDate);

        if (FromDate > ToDate) {
            $scope.filterModel.ToDate = null;
            return false;
        }


        table.destroy();
        $scope.getGroups();
    }


    $scope.saveGroup = function () {





        utilities.ajax({
            url: '/UserManagement/SaveGroup',
            method: 'POST',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h4') },
            disableScreen: true,
            disableControl: angular.element('button'),
            data: $scope.group,
            validate: true,
            form: 'form',
            success: function (data) {
                if (data.status == true)
                    window.location.href = '/UserManagement/Groups';
                else
                    toastr.warning(data.message);
            }
        });
    };

    $scope.getGroup = function () {
        utilities.ajax({
            url: '/UserManagement/GetGroup',
            method: 'GET',
            params: { groupId: $scope.GroupId },
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h2') },
            disableScreen: false,
            disableControl: angular.element('h2'),
            success: function (mvcResponse, angularResponse) {
                $scope.group = mvcResponse;
                //$scope.getRights();
            }
        });
    }

    $scope.UpdateRights = function (index, right) {
        if ($scope.group.RightsToRoleList != undefined || $scope.group.RightsToRoleList != null) {
            $scope.group.RightsToRoleList[index].rights = right
        }
    };


    $scope.UpdateGroupStatus = function (id, staus) {
        //var msg = !staus ? 'Are you sure you want to delete this record ?' : 'Are you sure you want to restore this record ?';
        //if (confirm(msg) == true) {
        utilities.ajax({
            url: '/UserManagement/UpdateGroupStatus?id={0}'.format(id) + '&status={0}'.format(staus),
            method: 'GET',
            success: function (mvcResponse) {
                if (mvcResponse.status == true)
                    if (staus == false) {
                        $('a[rowid=' + id + '] .fa-edit').parents('a:first').hide();
                        $('a[rowid=' + id + '] .fa-trash').parents('a:first').hide();
                        $('a[rowid=' + id + '] .fa-undo').parents('a:first').show();
                        $('span[rowid=' + id + '] ').text('InActive');

                    }
                    else if (staus == true) {
                        $('a[rowid=' + id + '] .fa-edit').parents('a:first').show();
                        $('a[rowid=' + id + '] .fa-trash').parents('a:first').show();
                        $('a[rowid=' + id + '] .fa-undo').parents('a:first').hide();
                        $('span[rowid=' + id + '] ').text('Active');
                    }
                $scope.Search();
            }
        });
        // }
    };

    //-------------------------------------***************************************** User Management *************************----------------------------------------//



    $scope.getUsers = function () {

        utilities.ajax({
            url: '/UserManagement/GetUsers',
            method: 'get',
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('th') },
            disableScreen: false,
            disableControl: angular.element('button'),
            params: { UserName: $scope.filterModel.UserName, Status: $scope.filterModel.Status, FromDate: $scope.filterModel.FromDate, ToDate: $scope.filterModel.ToDate },
            validate: true,
            form: 'form',
            success: function (mvcResponse) {

                table = utilities.dataTable({
                    element: angular.element('#UserList'),
                    scope: $scope,
                    aaData: mvcResponse,
                    aoColumns: [
                         {
                             "sTitle": "Actions", "bSortable": false, "sClass": 'actions', "sWidth": "70", "filter": false, "mRender": function (val, type, row) {
                                 var html = "";
                                 if (row.Status == "Active") {
                                     html = html + '<a href="/UserManagement/User?UserId={0}&isView=true" rowid={0} class="eye-link"><i class="fa fa-eye"></i></a>'.format(row.UserId) +
                                        '<a href="/UserManagement/User?UserId={0}" rowid="{0}" class="eye-edit"><i class="fa fa-edit"></i></a>'.format(row.UserId) +
                                      '&nbsp <a  href="" rowid={0} ng-click="UpdateUserStatus({0},false)" class="eye-trash"><i class="fa fa-trash" ></i></a>'.format(row.UserId)
                                    + '<a  href="" rowid={0} ng-click="UpdateUserStatus({0},true)" style="display:none;" class="eye-undo"><i class="fa fa-undo" ></i></a>'.format(row.UserId);
                                 }
                                 else
                                     html = html + '<a href="/UserManagement/User?UserId={0}&isView=true" rowid={0} class="eye-link"><i class="fa fa-eye"></i></a>'.format(row.UserId) +
                                        '<a href="/UserManagement/User?UserId={0}" style="display:none;" rowid="{0}" class="eye-edit"><i class="fa fa-edit"></i></a>'.format(row.UserId) +
                                         '&nbsp <a  href="" rowid={0} ng-click="UpdateUserStatus({0},false)" style="display:none;" class="eye-trash"><i class="fa fa-trash" ></i></a>'.format(row.UserId)
                                    + '<a href="" rowid={0} ng-click="UpdateUserStatus({0},true)"  class="eye-undo"><i class="fa fa-undo" ng-if="{0}"></i></a>'.format(row.UserId);

                                 return html;
                             }
                         },
                            { "mData": "UserName", "sTitle": "User Name", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                            { "mData": "Status", "sTitle": "Status" },
                             { "mData": "Regions", "sTitle": "Region" },
                            { "mData": "CreatedByUserId", "filter": false, "sTitle": "Created By", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return ''}  },
                            { "mData": "CreatedOnDate", "sTitle": "Created On" },
                            { "mData": "LastModifiedByUserId", "sTitle": "Modified By", "mRender": function (val, type, row) { if (val != null) { return val.fixLength(50); } else return '' } },
                            { "mData": "LastModifedOnDate", "sTitle": "Modified On" },
                    ]
                });
            }
        });
    };

    $scope.SearchUsers = function () {

        var ToDate = moment($scope.filterModel.ToDate, 'MMM-DD-YYYY');
        var FromDate = moment($scope.filterModel.FromDate, 'MMM-DD-YYYY');// new Date($scope.filterModel.FromDate);

        if (FromDate > ToDate) {
            $scope.filterModel.ToDate = null;

            return false;
        }


        table.destroy();
        $scope.getUsers();
    }


    $scope.saveUser = function () {

        if ($scope.userModel.UserRegionList.length < 1) {
            $scope.submit = true;



            var form = angular.element('#userForm');
            $.validator.unobtrusive.parse(form);

            if (!$(form).valid()) {
                console.log('invalid'); return false;
            }
            return false;
        }

        utilities.ajax({
            url: '/UserManagement/SaveUser',
            method: 'POST',
            data: $scope.userModel,
            validate: true,
            form: 'form',
            success: function (data) {
                if (data.status == true)
                    window.location.href = '/UserManagement/Users';
                else
                    toastr.warning(data.message);
            }
        });
    };

    $scope.getUser = function () {
        utilities.ajax({
            url: '/UserManagement/GetUser',
            method: 'GET',
            params: { userId: $scope.UserId },
            throbber: true,
            throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h2') },
            disableScreen: false,
            disableControl: angular.element('h2'),
            success: function (mvcResponse, angularResponse) {
                $scope.userModel = mvcResponse;
                if ($scope.userModel != undefined && $scope.userModel.UserId == 0) {
                    //$scope.userModel.Email = '';
                    //$scope.userModel.Password = '';
                }
                //$scope.getRights();
            }
        });
    }


    $scope.CancelUser = function () {
        if ($scope.GroupId == 0) {
            $(this).MessageBox('User will not be created, do you want to proceed?', GroupRedirect('Users'))
        }
        else {
            $(this).MessageBox('User will not be updated, do you want to proceed?', GroupRedirect('Users'))
        }
    }



    //$scope.getRights = function () {
    //    utilities.ajax({
    //        url: '/UserManagement/GetRights',
    //        method: 'GET',
    //        throbber: true,
    //        throbberPosition: { my: 'center center', at: 'center center', of: angular.element('h2') },
    //        disableScreen: false,
    //        disableControl: angular.element('h2'),
    //        success: function (mvcResponse, angularResponse) {
    //            $scope.rightsModel = mvcResponse;
    //        }
    //    });
    //}


    $scope.UpdateUserStatus = function (id, status) {
        //var msg = !staus ? 'Are you sure you want to delete this record ?' : 'Are you sure you want to restore this record ?';
        //if (confirm(msg) == true) {
        utilities.ajax({
            url: '/UserManagement/UpdateUserStatus?id={0}'.format(id) + '&status={0}'.format(status),
            method: 'GET',
            success: function (mvcResponse) {
                //if (mvcResponse.status == true)
                //    if (staus == false) {
                //        $('a[rowid=' + id + '] .fa-edit').parents('a:first').hide();
                //        $('a[rowid=' + id + '] .fa-trash').parents('a:first').hide();
                //        $('a[rowid=' + id + '] .fa-undo').parents('a:first').show();
                //        $('span[rowid=' + id + '] ').text('InActive');

                //    }
                //    else if (staus == true) {
                //        $('a[rowid=' + id + '] .fa-edit').parents('a:first').show();
                //        $('a[rowid=' + id + '] .fa-trash').parents('a:first').show();
                //        $('a[rowid=' + id + '] .fa-undo').parents('a:first').hide();
                //        $('span[rowid=' + id + '] ').text('Active');
                //    }
                $scope.SearchUsers();
            }
        });
        // }
    };


});

