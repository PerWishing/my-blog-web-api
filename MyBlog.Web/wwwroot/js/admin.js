$(document).on('click', '#blockUserBtn', function () {
    var username = $('#userPublicHidden').val();
    try {
        $.ajax({
            type: "POST",
            url: 'Admin/BlockUser/?username=' + username,
            data: false,
            contentType: false,
            processData: false,
            success: function (res) {
                $('#mainInLayout').load("/User/UserPublicProfile?username=" + username);
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
    catch (e) {
        console.log(e);
    }
});

$(document).on('click', '#unblockUserBtn', function () {
    var username = $('#userPublicHidden').val();
    try {
        $.ajax({
            type: "POST",
            url: 'Admin/UnblockUser/?username=' + username,
            data: false,
            contentType: false,
            processData: false,
            success: function (res) {
                $('#mainInLayout').load("/User/UserPublicProfile?username=" + username);
            },
            error: function (err) {
                console.log(err);
            }
        })
    }
    catch (e) {
        console.log(e);
    }
});