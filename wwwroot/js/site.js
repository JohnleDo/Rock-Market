var $userTable = $('#userTable')
var $roleTable = $('#roleTable')
var removeRoles = []
var removeUsers = []
var test

// Function for adding the extra detail to our User table per user.
function UserDetailFormatter(index, row) {
    var html = []
    if (model['users'][index]['email'] == row['username'] && model['users'][index]['id'] == row['id']) {
        html.push('<p><b>Username/Email :</b> ' + row['username'] + '</p>')
        html.push('<hr>')
        html.push('<p><b>ID :</b> ' + row['id'] + '</p>')
        html.push('<hr>')
        html.push('<p><b>First Name :</b> ' + model['users'][index]['firstName'] + '</p>')
        html.push('<hr>')
        html.push('<p><b>Last Name :</b> ' + model['users'][index]['lastName'] + '</p>')
        html.push('<hr>')
        html.push('<p><b>Phone Number :</b> ' + model['users'][index]['phone'] + '</p>')
        html.push('<hr>')
        html.push('<p><b>Address :</b> ' + model['users'][index]['address'] + '</p>')
        html.push('<hr>')
        html.push('<p><b>City :</b> ' + model['users'][index]['city'] + '</p>')
        html.push('<hr>')
        html.push('<p><b>State :</b> ' + model['users'][index]['state'] + '</p>')
        html.push('<hr>')
        return html.join('')
    }
    else {
        for (i = 0; i < model['users'].length; i++) {
            if (model['users'][i]['email'] == row['username'] && model['users'][i]['id'] == row['id']) {
                html.push('<p><b>Username/Email :</b> ' + row['username'] + '</p>')
                html.push('<hr>')
                html.push('<p><b>ID :</b> ' + row['id'] + '</p>')
                html.push('<hr>')
                html.push('<p><b>First Name :</b> ' + model['users'][i]['firstName'] + '</p>')
                html.push('<hr>')
                html.push('<p><b>Last Name :</b> ' + model['users'][i]['lastName'] + '</p>')
                html.push('<hr>')
                html.push('<p><b>Phone Number :</b> ' + model['users'][i]['phone'] + '</p>')
                html.push('<hr>')
                html.push('<p><b>Address :</b> ' + model['users'][i]['address'] + '</p>')
                html.push('<hr>')
                html.push('<p><b>City :</b> ' + model['users'][i]['city'] + '</p>')
                html.push('<hr>')
                html.push('<p><b>State :</b> ' + model['users'][i]['state'] + '</p>')
                html.push('<hr>')
                return html.join('')
            }
        }
    }
    return "Could Not Find User Data"
}

// This function is for making sure no one is able to delete the two main Roles from our system.
function roleStateFormatter(value, row, index) {
    test = row;
    if (row["roleName"] == "User" || row["roleName"] == "Administrator") {
        return {
            disabled: true
        }
    }
    return value
}

// Function for initializing our userTable
function inituserTable() {
    $userTable.bootstrapTable({
        columns: [{
            field: 'state',
            checkbox: true,
        }, {
            title: 'Username',
            field: 'username',
            sortable: true,
            searchable: true
        }, {
            title: 'ID',
            field: 'id',
            sortable: true,
            searchable: true,
            visible: false
        }, {
            title: 'First Name',
            field: 'firstName',
            sortable: true,
            searchable: true,
            visible: false
        }, {
            title: 'Last Name',
            field: 'lastName',
            sortable: true,
            searchable: true,
            visible: false
        }, {
            title: 'Phone Number',
            field: 'phoneNumber',
            sortable: true,
            searchable: true,
            visible: false
        }, {
            title: 'Address',
            field: 'address',
            sortable: true,
            searchable: true,
            visible: false
        }, {
            title: 'City',
            field: 'city',
            sortable: true,
            searchable: true,
            visible: false
        }, {
            title: 'State',
            field: 'stateLoc',
            sortable: true,
            searchable: true,
            visible: false
        }, {
            title: 'Roles',
            sortable: true,
            searchable: true,
            field: 'role'
        }, {
            title: 'Edit/Delete',
            field: 'modify'
        }]
    })
}

// Function for initializing our roleTable
function initroleTable() {
    $roleTable.bootstrapTable({
        columns: [{
            field: 'state',
            checkbox: true,
            formatter: "roleStateFormatter",
        }, {
            title: 'Role Name',
            field: 'roleName',
            sortable: true
        }, {
            title: 'Role ID',
            sortable: true,
            field: 'roleId'
        }]
    })
}

function initTables() {
    inituserTable()
    initroleTable()
}

// This is a event call. Whenever a user checks a box it will activate and get the value of the row and add it to our array of roles to deleted.
$("#roleTable").on("check.bs.table", function (field, value, row, $el) {
    removeRoles.push(value['roleId'])
});

// This is a event call. Whenever a user unchecks a box it will activate and get the value of the row and remove that Id from our array of roles to be deleted.
$("#roleTable").on("uncheck.bs.table", function (field, value, row, $el) {
    var index = removeRoles.indexOf(value['roleId'])
    removeRoles.splice(index, 1)
});

// This is a event call. Whenever a user checks the top checkbox it will get the values from each role and add it to our array of roles to be deleted.
$("#roleTable").on("check-all.bs.table", function (field, value, row, $el) {
    removeRoles = []
    for (i = 0; i < value.length; i++) {
        removeRoles.push(value[i]['roleId'])
    }
});

// This is a event call. Whenever a user unchecks the top checkbox it will reset our array of roles to be deleted.
$("#roleTable").on("uncheck-all.bs.table", function (field, value, row, $el) {
    removeRoles = []
});

// This is a event call. Whenever a user clicks on our button #btnDeleteRoles it will send a post request to /Admin/DeleteRoles passing a json containing our removeRoles array.
$("#btnDeleteRoles").on("click", function (e) {
    e.preventDefault();

    if (removeRoles.length == 0) {
        alert("No Roles were selected to be deleted");
    }

    else {
        var result = confirm("Are you sure you want to delete these role(s)?");

        if (result) {
            $.ajax({
                type: "post",
                dataType: "json",
                url: "/Admin/DeleteRoles",
                data: {
                    roleIds: removeRoles,
                },
                success: function (data) {
                    console.log(data.message);
                    window.location.reload();
                },
            });
        }
    }

});

//$(function () {
//    inituserTable()
//})